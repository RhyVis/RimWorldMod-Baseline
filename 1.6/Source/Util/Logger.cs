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
    private static readonly string Label = typeof(T)
        .GetCustomAttributes(typeof(LoggerLabelAttribute), false)
        .FirstOrDefault()
        is LoggerLabelAttribute attribute
        ? attribute.Label
        : typeof(T).Name;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void TemplateDbg(string label, string message, string color)
    {
        if (Prefs.DevMode)
            Log.Message($"<color=#{color}>[{label}]</color> {message}");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void TemplateDbg(string label, string message, string color, object? o)
    {
        if (Prefs.DevMode)
            Log.Message($"<color=#{color}>[{label}]</color> ({o ?? "<null>"}) {message}");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void Template(string label, string message, string color) =>
        Log.Message($"<color=#{color}>[{label}]</color> {message}");

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void Template(string label, string message, string color, object? o) =>
        Log.Message($"<color=#{color}>[{label}]</color> ({o ?? "<null>"}) {message}");

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void TemplateWrn(string label, string message, string color) =>
        Log.Warning($"<color=#{color}>[{label}]</color> {message}");

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void TemplateWrn(string label, string message, string color, object? o) =>
        Log.Warning($"<color=#{color}>[{label}]</color> ({o ?? "<null>"}) {message}");

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void TemplateErr(string label, string message, string color) =>
        Log.Error($"<color=#{color}>[{label}]</color> {message}");

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void TemplateErr(string label, string message, string color, object? o) =>
        Log.Error($"<color=#{color}>[{label}]</color> ({o ?? "<null>"}) {message}");

    /// <summary>
    /// Logs a debug message.
    /// </summary>
    public static void Debug(string message) => TemplateDbg(Label, message, CD);

    /// <summary>
    /// Logs an informational message.
    /// </summary>
    public static void Info(string message) => Template(Label, message, CI);

    /// <summary>
    /// Logs a warning message.
    /// </summary>
    public static void Warn(string message) => TemplateWrn(Label, message, CW);

    /// <summary>
    /// Logs an error message.
    /// </summary>
    public static void Error(string message) => TemplateErr(Label, message, CE);

    /// <summary>
    /// Logs a debug message.
    /// <param name="message">The message to log</param>
    /// <param name="o">An optional object to include in the log</param>
    /// </summary>
    public static void Debug(string message, object? o) => TemplateDbg(Label, message, CD, o);

    /// <summary>
    /// Logs an informational message.
    /// <param name="message">The message to log</param>
    /// <param name="o">An optional object to include in the log</param>
    /// </summary>
    public static void Info(string message, object? o) => Template(Label, message, CI, o);

    /// <summary>
    /// Logs a warning message.
    /// <param name="message">The message to log</param>
    /// <param name="o">An optional object to include in the log</param>
    /// </summary>
    public static void Warn(string message, object? o) => TemplateWrn(Label, message, CW, o);

    /// <summary>
    /// Logs an error message.
    /// <param name="message">The message to log</param>
    /// <param name="o">An optional object to include in the log</param>
    /// </summary>
    public static void Error(string message, object? o) => TemplateErr(Label, message, CE, o);

    /// <summary>
    /// Logs a debug message.
    /// </summary>
    internal static void I_Debug(string message, string label) => TemplateDbg(label, message, CD);

    /// <summary>
    /// Logs an informational message.
    /// </summary>
    internal static void I_Info(string message, string label) => Template(label, message, CI);

    /// <summary>
    /// Logs a warning message.
    /// </summary>
    internal static void I_Warn(string message, string label) => TemplateWrn(label, message, CW);

    /// <summary>
    /// Logs an error message.
    /// </summary>
    internal static void I_Error(string message, string label) => TemplateErr(label, message, CE);

    private const string CD = "888888FF";
    private const string CI = "00AAFFFF";
    private const string CW = "FFFF00FF";
    private const string CE = "FF0000FF";
}
