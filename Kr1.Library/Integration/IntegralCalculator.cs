using System.Diagnostics;

namespace Kr1.Library.Integration;

public class IntegralProgressEventArgs : EventArgs
{
    public int ThreadId { get; init; }
    public int Percent { get; init; }
}

public class IntegralCompletedEventArgs : EventArgs
{
    public int ThreadId { get; init; }
    public double Result { get; init; }
    public TimeSpan Elapsed { get; init; }
}

public class IntegralCalculator
{
    private readonly object _exclusiveLock = new();
    private readonly SemaphoreSlim _limiter;
    private double _sink = 0.0;

    public IntegralCalculator(int maxConcurrentThreads = 1)
    {
        if (maxConcurrentThreads < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(maxConcurrentThreads));
        }

        MaxConcurrentThreads = maxConcurrentThreads;
        _limiter = new SemaphoreSlim(maxConcurrentThreads, maxConcurrentThreads);
    }

    public double From { get; init; } = 0;
    public double To { get; init; } = 1;
    public double Step { get; init; } = 0.00000001;
    public int DelayIterations { get; init; } = 100000;
    public int MaxConcurrentThreads { get; }

    public event EventHandler<IntegralProgressEventArgs>? ProgressChanged;
    public event EventHandler<IntegralCompletedEventArgs>? Completed;

    public static double Function(double x) => Math.Sin(x);

    public double Calculate()
    {
        var threadId = Thread.CurrentThread.ManagedThreadId;
        var stopwatch = Stopwatch.StartNew();

        long count = (long)Math.Round((To - From) / Step);
        double sum = 0;
        int lastPercent = -1;

        for (long i = 0; i < count; i++)
        {
            sum += Function(From + (i + 0.5) * Step) * Step;
            Delay();

            int percent = (int)((i + 1) * 100 / count);
            if (percent != lastPercent)
            {
                lastPercent = percent;
                ProgressChanged?.Invoke(this, new IntegralProgressEventArgs { ThreadId = threadId, Percent = percent });
            }
        }

        stopwatch.Stop();
        Completed?.Invoke(this, new IntegralCompletedEventArgs
        {
            ThreadId = threadId,
            Result = sum,
            Elapsed = stopwatch.Elapsed
        });

        return sum;
    }

    public double CalculateExclusive()
    {
        lock (_exclusiveLock)
        {
            return Calculate();
        }
    }

    public double CalculateLimited()
    {
        _limiter.Wait();
        try
        {
            return Calculate();
        }
        finally
        {
            _limiter.Release();
        }
    }

    private void Delay()
    {
        double x = 0;
        for (int j = 0; j < DelayIterations; j++)
        {
            x += j * 1.000001;
        }
        _sink = x;
    }
}
