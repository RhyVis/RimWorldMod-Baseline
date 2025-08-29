using System.Runtime.CompilerServices;

namespace Rhynia.Baseline.Util;

/// <summary>
/// Attribute to specify the log label, used by <see cref="Logger{T}"/>.
/// </summary>
/// <param name="label">Label used for logging</param>
[AttributeUsage(
    AttributeTargets.Class | AttributeTargets.Struct,
    Inherited = false,
    AllowMultiple = false
)]
public class LoggerLabelAttribute(string label) : Attribute
{
    /// <summary>
    /// Label used for logging.
    /// </summary>
    public string Label { get; } = label;
}

/// <summary>
/// Generic static logger template class. If <see cref="LoggerLabelAttribute"/> not provided, the type name will be used.
/// </summary>
/// <typeparam name="T">Class or struct type, using <see cref="LoggerLabelAttribute"/> to specify the log label.</typeparam>
public static class Logger<T>
{
    private static readonly string _label = typeof(T)
        .GetCustomAttributes(typeof(LoggerLabelAttribute), false)
        .FirstOrDefault()
        is LoggerLabelAttribute attribute
        ? attribute.Label
        : typeof(T).Name;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void Template(string label, string message, string color) =>
        Log.Message($"<color=#{color}>[{label}]</color> {message}");

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void Template(string label, string message, string color, object? o) =>
        Log.Message($"<color=#{color}>[{label}]</color> ({o ?? "<null>"}) {message}");

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void TemplateErr(string label, string message, string color) =>
        Log.Error($"<color=#{color}>[{label}]</color> {message}");

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void TemplateErr(string label, string message, string color, object? o) =>
        Log.Error($"<color=#{color}>[{label}]</color> ({o ?? "<null>"}) {message}");

    /// <summary>
    /// Logs a debug message.
    /// </summary>
    public static void Debug(string message)
    {
        if (Prefs.DevMode)
            Template(_label, message, "888888FF");
    }

    /// <summary>
    /// Logs an informational message.
    /// </summary>
    public static void Info(string message)
    {
        Template(_label, message, "00AAFFFF");
    }

    /// <summary>
    /// Logs a warning message.
    /// </summary>
    public static void Warn(string message)
    {
        Template(_label, message, "FFFF00FF");
    }

    /// <summary>
    /// Logs an error message.
    /// </summary>
    public static void Error(string message)
    {
        TemplateErr(_label, message, "FF0000FF");
    }

    /// <summary>
    /// Logs a debug message.
    /// <param name="message">The message to log</param>
    /// <param name="o">An optional object to include in the log</param>
    /// </summary>
    public static void Debug(string message, object? o)
    {
        if (Prefs.DevMode)
            Template(_label, message, "888888FF", o);
    }

    /// <summary>
    /// Logs an informational message.
    /// <param name="message">The message to log</param>
    /// <param name="o">An optional object to include in the log</param>
    /// </summary>
    public static void Info(string message, object? o)
    {
        Template(_label, message, "00AAFFFF", o);
    }

    /// <summary>
    /// Logs a warning message.
    /// <param name="message">The message to log</param>
    /// <param name="o">An optional object to include in the log</param>
    /// </summary>
    public static void Warn(string message, object? o)
    {
        Template(_label, message, "FFFF00FF", o);
    }

    /// <summary>
    /// Logs an error message.
    /// <param name="message">The message to log</param>
    /// <param name="o">An optional object to include in the log</param>
    /// </summary>
    public static void Error(string message, object? o)
    {
        TemplateErr(_label, message, "FF0000FF", o);
    }
}
