namespace Rhynia.Baseline.Util;

/// <summary>
/// Extension methods for Harmony.
/// </summary>
public static class HarmonyExtension
{
    /// <summary>
    /// Directly convert IEnumerable of CodeInstruction to CodeMatcher
    /// </summary>
    public static CodeMatcher AsCodeMatcher(this IEnumerable<CodeInstruction> instructions) =>
        new(instructions);
}
