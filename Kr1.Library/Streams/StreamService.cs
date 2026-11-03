using System.Text;
using System.Text.Json;

namespace Kr1.Library.Streams;

public class StreamService<T>
{
    private const int ItemsPerDelay = 10;
    private const int DelayMs = 40;

    private readonly SemaphoreSlim _writeCompleted = new(0, 1);

    public async Task WriteToStreamAsync(Stream stream, IEnumerable<T> data, IProgress<string> progress)
    {
        Report(progress, "entered WriteToStreamAsync");
        try
        {
            Report(progress, "writing to stream started");

            await using (var writer = new StreamWriter(stream, Encoding.UTF8, leaveOpen: true))
            {
                int written = 0;
                foreach (var item in data)
                {
                    await writer.WriteLineAsync(JsonSerializer.Serialize(item));

                    if (++written % ItemsPerDelay == 0)
                    {
                        await Task.Delay(DelayMs);
                    }
                }
                await writer.FlushAsync();
            }

            Report(progress, "writing to stream finished");
        }
        finally
        {
            _writeCompleted.Release();
        }
    }

    public async Task CopyFromStreamAsync(Stream stream, string filename, IProgress<string> progress)
    {
        Report(progress, "entered CopyFromStreamAsync");
        Report(progress, "waiting for writing to finish");

        await _writeCompleted.WaitAsync();

        Report(progress, "copying from stream to file started");

        stream.Position = 0;
        await using (var file = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None,
                         bufferSize: 4096, useAsync: true))
        {
            await stream.CopyToAsync(file);
        }

        Report(progress, "copying from stream to file finished");
    }

    public async Task<int> GetStatisticsAsync(string fileName, Func<T, bool> filter)
    {
        int count = 0;

        using var reader = new StreamReader(fileName, Encoding.UTF8);
        while (await reader.ReadLineAsync() is { } line)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                continue; 
            }

            var item = JsonSerializer.Deserialize<T>(line);
            if (item is not null && filter(item))
            {
                count++;
            }
        }

        return count;
    }

    private static void Report(IProgress<string> progress, string action) =>
        progress.Report($"Thread {Thread.CurrentThread.ManagedThreadId}: {action}");
}
