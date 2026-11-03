using Kr1.Library.Bank;
using Kr1.Library.Streams;

namespace Kr1.Task2;

internal class Program
{
    private static readonly string[] FirstNames =
        ["John", "Mary", "James", "Anna", "Robert", "Linda", "Michael", "Emma", "David", "Olivia"];

    private static readonly string[] LastNames =
        ["Smith", "Johnson", "Williams", "Brown", "Jones", "Miller", "Davis", "Wilson"];

    private static async Task Main(string[] args)
    {
        const string fileName = "clients.jsonl";

        var clients = CreateClients(1000);

        Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}: program started");

        var progress = new SyncProgress<string>();
        progress.ProgressChanged += (_, message) => Console.WriteLine(message);

        var service = new StreamService<BankClient>();
        using var stream = new MemoryStream();

        var writeTask = Task.Run(() => service.WriteToStreamAsync(stream, clients, progress));
        Thread.Sleep(150);
        var copyTask = Task.Run(() => service.CopyFromStreamAsync(stream, fileName, progress));

        Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}: tasks 1 and 2 started");

        await Task.WhenAll(writeTask, copyTask);

        int currentYear = DateTime.Now.Year;
        int count = await service.GetStatisticsAsync(fileName, c => c.AccountOpenedDate.Year == currentYear);

        Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}: " +
                          $"clients who opened an account in {currentYear}: {count} of {clients.Count}");
    }

    private static List<BankClient> CreateClients(int count)
    {
        var today = DateTime.Today;
        var start = today.AddYears(-5);
        int days = (today - start).Days;

        return Enumerable.Range(1, count)
            .Select(i => new BankClient
            {
                Id = i,
                FullName = $"{FirstNames[Random.Shared.Next(FirstNames.Length)]} {LastNames[Random.Shared.Next(LastNames.Length)]}",
                AccountOpenedDate = start.AddDays(Random.Shared.Next(days + 1))
            })
            .ToList();
    }
}
