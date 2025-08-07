namespace Rhynia.Baseline.Util;

public static class TranslationExtension
{
    private const string Namespace = "RhyniaBaseline";
    private const string Prefix = Namespace + "_";

    private const string EnableKey = Prefix + "Enable";
    private const string DisableKey = Prefix + "Disable";
    private const string YesKey = Prefix + "Yes";
    private const string NoKey = Prefix + "No";

    public static TaggedString TranslateAsEnable(this bool value) =>
        value ? EnableKey.Translate() : DisableKey.Translate();

    public static TaggedString TranslateAsYes(this bool value) =>
        value ? YesKey.Translate() : NoKey.Translate();
}
