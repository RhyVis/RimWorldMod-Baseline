using System.Threading;

namespace Rhynia.Baseline.Util;

public class AtomicBool(bool initialValue = false)
{
    private int _value = initialValue ? 1 : 0;

    public bool Value
    {
        get => Interlocked.CompareExchange(ref _value, 0, 0) == 1;
        set => Interlocked.Exchange(ref _value, value ? 1 : 0);
    }

    public bool TrySet(bool newValue)
    {
        int newInt = newValue ? 1 : 0;
        int oldInt = newValue ? 0 : 1;
        return Interlocked.CompareExchange(ref _value, newInt, oldInt) == oldInt;
    }
}
