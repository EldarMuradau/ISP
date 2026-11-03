namespace Kr1.Library.Streams;

public class SyncProgress<T> : IProgress<T>
{
    public event EventHandler<T>? ProgressChanged;

    public void Report(T value) => ProgressChanged?.Invoke(this, value);
}
