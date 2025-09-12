namespace Rhynia.Baseline;

public class DefModExt_EqualResearchUse : DefModExtension
{
    public List<ThingDef> thingDefs = [];
}

[StaticConstructorOnStartup]
public static class DefModExt_EqualResearchUse_Helper
{
    private static readonly Dictionary<ThingDef, List<ThingDef>> _cache = [];

    public static bool IsValid(ThingDef required, ThingDef provided) =>
        (required == provided)
        || (_cache.TryGetValue(required, out var list) && list.Contains(provided));

    static DefModExt_EqualResearchUse_Helper()
    {
        var defs = DefDatabase<ThingDef>.AllDefsListForReading.Where(def =>
            def.HasModExtension<DefModExt_EqualResearchUse>()
        );
        if (!defs.Any())
            return;

        Debug($"DefModExt_EqualResearchUse: Resolving uses with {defs.Count()} defs.");

        foreach (var def in defs)
            if (def.GetModExtension<DefModExt_EqualResearchUse>() is { } ext)
                foreach (var thingDef in ext.thingDefs)
                {
                    if (!_cache.ContainsKey(thingDef))
                        _cache[thingDef] = [];
                    _cache[thingDef].Add(def);
                }

        Patch_ResearchAt.Apply(Mod_Baseline.harmony);
    }
}
