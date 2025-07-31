namespace Rhynia.Baseline.Util;

public static class StringExtension
{
    /// <summary>
    /// Checks if the string contains the target string, ignoring case.
    /// </summary>
    public static bool ContainsIgnoreCase(this string s, string target) =>
        s.IndexOf(target, StringComparison.OrdinalIgnoreCase) >= 0;

    /// <summary>
    /// Checks if the string contains any of the target strings, ignoring case.
    /// </summary>
    public static bool ContainsAnyOfIgnoreCase(this string s, params string[] targets) =>
        targets.Any(s.ContainsIgnoreCase);
}
