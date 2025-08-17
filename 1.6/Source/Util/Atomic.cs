using System.Threading;

namespace Rhynia.Baseline.Util;

/// <summary>
/// A thread-safe container for a single value.
/// </summary>
/// <typeparam name="T">Type of the class</typeparam>
/// <param name="initialValue">Initial stored value</param>
public class AtomicContainer<T>(T initialValue)
    where T : class
{
    private T _value = initialValue;
    private readonly object _syncRoot = new();

    /// <summary>
    /// Gets or sets the current value.
    /// </summary>
    public T Value
    {
        get => _value;
        set => Interlocked.Exchange(ref _value, value);
    }

    /// <summary>
    /// Safely accesses the current value, allowing for thread-safe read operations.
    /// <br />
    /// Note: This method only guarantees that the reference to the object is read atomically.
    /// It does not protect the object itself from being modified by other threads during the execution of the accessor.
    /// If the object of type T is mutable, it must be internally thread-safe.
    /// </summary>
    /// <typeparam name="TResult">The type of the result from the function.</typeparam>
    /// <param name="accessor">The function to execute on the value.</param>
    /// <returns>The result from the accessor function.</returns>
    public TResult Access<TResult>(Func<T, TResult> accessor)
    {
        var current = _value;
        return accessor(current);
    }

    /// <summary>
    /// Executes a synchronized action on the contained value, providing thread-safe access for mutable objects.
    /// This method uses a lock to ensure that no other synchronized action can run concurrently.
    /// </summary>
    /// <param name="action">The action to perform on the value.</param>
    public void SynchronizedAction(Action<T> action)
    {
        lock (_syncRoot)
        {
            action(_value);
        }
    }

    /// <summary>
    /// Executes a synchronized function on the contained value, providing thread-safe access for mutable objects and returning a result.
    /// This method uses a lock to ensure that no other synchronized action can run concurrently.
    /// </summary>
    /// <typeparam name="TResult">The type of the result from the function.</typeparam>
    /// <param name="accessor">The function to execute on the value.</param>
    /// <returns>The result from the accessor function.</returns>
    public TResult SynchronizedAction<TResult>(Func<T, TResult> accessor)
    {
        lock (_syncRoot)
        {
            return accessor(_value);
        }
    }
}

/// <summary>
/// Similar to <see cref="AtomicContainer{T}"/>, but allows for null values.
/// </summary>
public class AtomicContainerNullable<T>(T? initialValue = null)
    where T : class
{
    private T? _value = initialValue;
    private readonly object _syncRoot = new();

    /// <summary>
    /// Gets or sets the current value.
    /// </summary>
    public T? Value
    {
        get => _value;
        set => Interlocked.Exchange(ref _value, value);
    }

    /// <summary>
    /// Safely accesses the current value, allowing for thread-safe read operations.
    /// <br />
    /// Note: This method only guarantees that the reference to the object is read atomically.
    /// It does not protect the object itself from being modified by other threads during the execution of the accessor.
    /// If the object of type T is mutable, it must be internally thread-safe.
    /// </summary>
    /// <typeparam name="TResult">The type of the result from the function.</typeparam>
    /// <param name="accessor">The function to execute on the value.</param>
    /// <returns>The result from the accessor function, or default if the contained value is null.</returns>
    public TResult? Access<TResult>(Func<T?, TResult?> accessor)
    {
        var current = _value;
        return accessor(current);
    }

    /// <summary>
    /// Executes a synchronized action on the contained value, providing thread-safe access for mutable objects.
    /// This method uses a lock to ensure that no other synchronized action can run concurrently.
    /// </summary>
    /// <param name="action">The action to perform on the value.</param>
    public void SynchronizedAction(Action<T?> action)
    {
        lock (_syncRoot)
        {
            action(_value);
        }
    }

    /// <summary>
    /// Executes a synchronized function on the contained value, providing thread-safe access for mutable objects and returning a result.
    /// This method uses a lock to ensure that no other synchronized action can run concurrently.
    /// </summary>
    /// <typeparam name="TResult">The type of the result from the function.</typeparam>
    /// <param name="accessor">The function to execute on the value.</param>
    /// <returns>The result from the accessor function.</returns>
    public TResult? SynchronizedAction<TResult>(Func<T?, TResult?> accessor)
    {
        lock (_syncRoot)
        {
            return accessor(_value);
        }
    }
}

/// <summary>
/// A thread-safe boolean container.
/// </summary>
/// <param name="initialValue">Initial stored value, defaults to false</param>
public class AtomicBool(bool initialValue = false)
{
    private int _value = initialValue ? 1 : 0;

    /// <summary>
    /// Gets or sets the current value.
    /// </summary>
    public bool Value
    {
        get => Interlocked.CompareExchange(ref _value, 0, 0) == 1;
        set => Interlocked.Exchange(ref _value, value ? 1 : 0);
    }
}
