using System.Text;

namespace Kr1.Task1;

public class ProgressView
{
    private const int BarWidth = 30;

    private readonly object _consoleLock = new();
    private readonly Dictionary<int, int> _rows = new();
    private readonly Dictionary<int, int> _lastPrinted = new();
    private readonly bool _redirected = Console.IsOutputRedirected;
    private readonly int _top;

    public ProgressView(IEnumerable<int> threadIds)
    {
        var ids = threadIds.ToList();
        _top = _redirected ? 0 : Console.CursorTop;

        for (int i = 0; i < ids.Count; i++)
        {
            _rows[ids[i]] = i;
            _lastPrinted[ids[i]] = -1;
            Write(ids[i], $"Thread {ids[i],3}: waiting...");
        }
    }

    public void Update(int threadId, int percent)
    {
        if (_redirected)
        {
            lock (_consoleLock)
            {
                if (percent % 10 != 0 || _lastPrinted[threadId] == percent)
                {
                    return;
                }
                _lastPrinted[threadId] = percent;
            }
        }

        Write(threadId, $"Thread {threadId,3}: {Bar(percent)} {percent,3}%");
    }

    public void Complete(int threadId, double result, TimeSpan elapsed) =>
        Write(threadId, $"Thread {threadId,3}: {Bar(100)} 100%  result = {result:F9}, time = {elapsed:mm\\:ss\\.fff}");

    public void MoveBelow()
    {
        if (!_redirected)
        {
            Console.SetCursorPosition(0, _top + _rows.Count);
        }
    }

    private void Write(int threadId, string text)
    {
        lock (_consoleLock)
        {
            if (_redirected)
            {
                Console.WriteLine(text);
                return;
            }

            Console.SetCursorPosition(0, _top + _rows[threadId]);
            Console.Write(text.PadRight(Math.Max(Console.WindowWidth - 1, text.Length)));
        }
    }

    private static string Bar(int percent)
    {
        int filled = percent * BarWidth / 100;
        var sb = new StringBuilder("[");
        sb.Append('=', Math.Max(filled - 1, 0));
        if (filled > 0)
        {
            sb.Append(filled == BarWidth ? '=' : '>');
        }
        sb.Append(' ', BarWidth - filled);
        sb.Append(']');
        return sb.ToString();
    }
}
