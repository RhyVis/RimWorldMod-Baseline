namespace Rhynia.Baseline.Util;

/// <summary>
/// Extension helper methods for translation.
/// </summary>
public static class TranslationExtension
{
    private const string Namespace = "RhyniaBaseline";
    private const string Prefix = Namespace + "_";

    private const string EnableKey = Prefix + "Enable";
    private const string DisableKey = Prefix + "Disable";
    private const string YesKey = Prefix + "Yes";
    private const string NoKey = Prefix + "No";

    /// <summary>
    /// Translates the boolean value to a localized string for enable/disable.
    /// </summary>
    public static TaggedString TranslateAsEnable(this bool value) =>
        value ? EnableKey.Translate() : DisableKey.Translate();

    /// <summary>
    /// Translates the boolean value to a localized string for yes/no.
    /// </summary>
    public static TaggedString TranslateAsYes(this bool value) =>
        value ? YesKey.Translate() : NoKey.Translate();
}
