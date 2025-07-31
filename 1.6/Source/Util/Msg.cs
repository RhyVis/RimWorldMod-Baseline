namespace Rhynia.Baseline.Util;

public static class Msg
{
    public static void LogLabeled(this string label, string message, string color = "00EEFFFF") =>
        Log.Message($"<color=#{color}>[{label}]</color> {message}");

    public static void DebugLabeled(this string label, string message) =>
        Log.Message($"<color=#888888FF>[{label}]</color> {message}");

    public static void InfoLabeled(this string label, string message) =>
        Log.Message($"<color=#00AAFFFF>[{label}]</color> {message}");

    public static void WarningLabeled(this string label, string message) =>
        Log.Warning($"<color=#FFFF00FF>[{label}]</color> {message}");

    public static void ErrorLabeled(this string label, string message) =>
        Log.Error($"<color=#FF0000FF>[{label}]</color> {message}");

    internal static void Debug(string message)
    {
        if (ModSettings_Baseline.DebugMode)
            Constant.ModName.DebugLabeled(message);
    }

    internal static void Info(string message) => Constant.ModName.InfoLabeled(message);

    internal static void Warning(string message) => Constant.ModName.WarningLabeled(message);

    internal static void Error(string message) => Constant.ModName.ErrorLabeled(message);
}
