namespace Rhynia.Baseline;

public class DefModExt_CopyFacilities : DefModExtension
{
    public List<ThingDef> thingDefs = [];
}

[StaticConstructorOnStartup]
public static class DefModExt_CopyFacilities_Helper
{
    static DefModExt_CopyFacilities_Helper()
    {
        var defs = DefDatabase<ThingDef>.AllDefs.Where(def =>
            def.HasModExtension<DefModExt_CopyFacilities>()
        );
        if (!defs.Any())
            return;

        Debug($"DefModExt_CopyFacilities: Resolving uses with {defs.Count()} defs.");

        foreach (var def in defs)
        {
            if (
                def.GetModExtension<DefModExt_CopyFacilities>() is not { } ext
                || def.GetCompProperties<CompProperties_AffectedByFacilities>()
                    is not { } affectedByFacilityProps
            )
            {
                Warn(
                    $"DefModExt_CopyFacilities: Def {def.defName} is missing either the DefModExt_CopyFacilities or CompAffectedByFacilities, skipping."
                );
                return;
            }

            var facilitiesToCopy = new HashSet<ThingDef>();
            foreach (var thingDef in ext.thingDefs)
            {
                var targetProp = thingDef.GetCompProperties<CompProperties_AffectedByFacilities>();
                if (targetProp == null)
                {
                    Warn(
                        $"DefModExt_CopyFacilities: Target thingDef {thingDef.defName} requested by {def.defName} does not have CompAffectedByFacilities, skipping."
                    );
                    continue;
                }

                foreach (var facilityDef in targetProp.linkableFacilities)
                    if (facilityDef != def)
                        facilitiesToCopy.Add(facilityDef);
            }

            affectedByFacilityProps.linkableFacilities ??= [];
            affectedByFacilityProps.linkableFacilities.AddRangeUnique(facilitiesToCopy);
        }

        foreach (var def in DefDatabase<ThingDef>.AllDefs.Where(def => def.HasComp<CompFacility>()))
            def.GetCompProperties<CompProperties_Facility>().ResolveReferences(def);
    }
}
