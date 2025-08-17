using System.Diagnostics;

namespace Rhynia.Baseline.Util;

/// <summary>
/// A scope for measuring the duration of an operation.
/// <br />
/// It will log the duration of the operation when disposed.
/// </summary>
/// <param name="action">The logger action to exec when disposed</param>
public class TimingScope(Action<TimeSpan> action) : IDisposable
{
    private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

    public void Dispose()
    {
        _stopwatch.Stop();
        action(_stopwatch.Elapsed);
    }

    /// <summary>
    /// Starts a new timing scope.
    /// </summary>
    /// <param name="action">The logger action to exec when disposed</param>
    public static TimingScope Start(Action<TimeSpan> action) => new(action);
}
