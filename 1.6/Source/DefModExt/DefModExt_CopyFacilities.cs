namespace Rhynia.Baseline;

public class DefModExt_CopyFacilities : DefModExtension
{
    public List<ThingDef> thingDefs = [];

    [Unsaved]
    public CompProperties_AffectedByFacilities? propFacilities = null;

    public override void ResolveReferences(Def parentDef)
    {
        if (
            parentDef is ThingDef def
            && def.GetCompProperties<CompProperties_AffectedByFacilities>() is { } prop
        )
            propFacilities = prop;
        else
            Error(
                $"DefModExt_CopyFacilities can only be applied to ThingDef with CompAffectedByFacilities. Offending def: {parentDef.defName}"
            );
    }
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

        var defsBeenCopied = new HashSet<ThingDef>();
        foreach (var def in defs)
        {
            if (
                def.GetModExtension<DefModExt_CopyFacilities>() is not { } ext
                || ext.propFacilities == null
            )
                return;

            var facilitiesToCopy = new HashSet<ThingDef>();
            foreach (var thingDef in ext.thingDefs)
            {
                var targetProp = thingDef.GetCompProperties<CompProperties_AffectedByFacilities>();
                if (targetProp == null)
                {
                    Warn(
                        $"DefModExt_CopyFacilities: Target thingDef {thingDef.defName} does not have CompAffectedByFacilities, skipping."
                    );
                    continue;
                }

                foreach (var facility in targetProp.linkableFacilities)
                    facilitiesToCopy.Add(facility);
                defsBeenCopied.Add(thingDef);
            }

            foreach (var facility in facilitiesToCopy)
                if (!ext.propFacilities.linkableFacilities.Contains(facility) && facility != def)
                    ext.propFacilities.linkableFacilities.Add(facility);
        }

        foreach (var def in DefDatabase<ThingDef>.AllDefs.Where(def => def.HasComp<CompFacility>()))
            def.GetCompProperties<CompProperties_AffectedByFacilities>()?.ResolveReferences(def);
    }
}
