using Rhynia.Baseline.Util;

namespace Rhynia.Baseline;

public class Mod_Baseline(ModContentPack mod) : Mod(mod)
{
    public override void DoSettingsWindowContents(Rect inRect)
    {
        base.DoSettingsWindowContents(inRect);
        var list = new Listing_Standard();
        list.Begin(inRect);
        list.CheckboxLabeled(
            "RhyniaBaseline_Settings_Debug".Translate(),
            ref ModSettings_Baseline.DebugMode
        );
        list.End();
    }

    public override string SettingsCategory() => "RhyniaBaseline_Settings_Name".Translate();
}

public class ModSettings_Baseline : ModSettings
{
    public static bool DebugMode = false;

    public override void ExposeData()
    {
        Scribe_Values.Look(ref DebugMode, "DebugMode", false);
        base.ExposeData();
    }
}

[StaticConstructorOnStartup]
public static class Mod_Init
{
    static Mod_Init()
    {
        Msg.Info("Mod Rhynia Baseline initialized.");
    }
}
