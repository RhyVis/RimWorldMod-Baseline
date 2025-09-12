using Rhynia.Baseline.Util;

namespace Rhynia.Baseline;

[StaticConstructorOnStartup]
public class Mod_Baseline(ModContentPack mod) : Mod(mod)
{
    internal static readonly Harmony harmony = new("Rhynia.Mod.Baseline");

    static Mod_Baseline() => Info("Mod initialized.");
}

[LoggerLabel("Rhynia.Baseline")]
internal struct LogLabel;
