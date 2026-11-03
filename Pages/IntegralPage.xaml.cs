namespace ipr1.Pages;

public partial class IntegralPage : ContentPage
{
    private const double Step = 0.00000001;
    // Increase for a slower calculation, decrease for a faster one.
    private const int DelayIterations = 30;

    private CancellationTokenSource? _cts;

    public IntegralPage()
    {
        InitializeComponent();
    }

    private async void OnStartClicked(object? sender, EventArgs e)
    {
        _cts = new CancellationTokenSource();
        StartButton.IsEnabled = false;
        CancelButton.IsEnabled = true;
        StatusLabel.Text = "Вычисление";
        SetProgress(0);

        var progress = new Progress<double>(SetProgress);

        try
        {
            double result = await Task.Run(() => CalculateIntegral(progress, _cts.Token), _cts.Token);
            StatusLabel.Text = $"Результат: {result:F8}";
        }
        catch (OperationCanceledException)
        {
            StatusLabel.Text = "Задание отменено";
        }
        finally
        {
            StartButton.IsEnabled = true;
            CancelButton.IsEnabled = false;
            _cts.Dispose();
            _cts = null;
        }
    }

    private void OnCancelClicked(object? sender, EventArgs e)
    {
        _cts?.Cancel();
    }

    private void SetProgress(double value)
    {
        Progress.Progress = value;
        PercentLabel.Text = $"{value * 100:F0}%";
    }

    // Integral of sin(x) on [0, 1] using the midpoint rectangle method.
    private static double CalculateIntegral(IProgress<double> progress, CancellationToken token)
    {
        const double a = 0, b = 1;
        long n = (long)Math.Round((b - a) / Step);
        long reportEvery = n / 100;
        double sum = 0;

        for (long i = 0; i < n; i++)
        {
            double x = a + (i + 0.5) * Step;
            sum += Math.Sin(x);

            double dummy = 1;
            for (int j = 0; j < DelayIterations; j++)
                dummy *= 1.0000001;

            if (i % reportEvery == 0)
            {
                token.ThrowIfCancellationRequested();
                progress.Report((double)i / n);
            }
        }

        progress.Report(1);
        return sum * Step;
    }
}
