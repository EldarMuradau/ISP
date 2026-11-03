using System.Globalization;
using Kr1.Library.Integration;
using Kr1.Task1;

// Usage: dotnet run -- [1|5|6|all] [maxThreads] [step]

string mode = args.Length > 0 ? args[0] : "all";
int limit = args.Length > 1 ? int.Parse(args[1]) : 2;
double step = args.Length > 2 ? double.Parse(args[2], CultureInfo.InvariantCulture) : 0.0001;

Console.WriteLine($"Integral of sin(x) on [0; 1], step {step}, exact value {1 - Math.Cos(1):F9}");

if (mode is "1" or "all")
{
    Console.WriteLine("\n4. Single background thread");
    var calculator = new IntegralCalculator { Step = step };
    Run(calculator, 1, calculator.Calculate);
}

if (mode is "5" or "all")
{
    Console.WriteLine("\n5. Five threads, only one runs at a time (lock)");
    var calculator = new IntegralCalculator { Step = step };
    Run(calculator, 5, calculator.CalculateExclusive);
}

if (mode is "6" or "all")
{
    Console.WriteLine($"\n6. Five threads, at most {limit} run at a time (SemaphoreSlim)");
    var calculator = new IntegralCalculator(limit) { Step = step };
    Run(calculator, 5, calculator.CalculateLimited);
}

static void Run(IntegralCalculator calculator, int threadCount, Func<double> method)
{
    var threads = Enumerable.Range(0, threadCount)
        .Select(_ => new Thread(() => method()))
        .ToList();

    var view = new ProgressView(threads.Select(t => t.ManagedThreadId));

    calculator.ProgressChanged += (_, e) => view.Update(e.ThreadId, e.Percent);
    calculator.Completed += (_, e) => view.Complete(e.ThreadId, e.Result, e.Elapsed);

    foreach (var thread in threads)
    {
        thread.Start();
    }
    foreach (var thread in threads)
    {
        thread.Join();
    }

    view.MoveBelow();
}
