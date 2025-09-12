namespace Rhynia.Baseline.Util;

public static class PawnExtension
{
    /// <summary>
    /// Applies a hediff to the pawn.
    /// </summary>
    public static void ApplyHediff(this Pawn? pawn, HediffDef def, float severityAdjust = 1.0f)
    {
        if (pawn is null)
            return;

        if (pawn.health.hediffSet.GetFirstHediffOfDef(def) is { } hediff)
            hediff.Severity += severityAdjust;
        else
        {
            hediff = HediffMaker.MakeHediff(def, pawn);
            hediff.Severity = severityAdjust;
            pawn.health.AddHediff(hediff);
        }
    }

    /// <summary>
    /// Applies a hediff to the pawn with the specified stats and severity adjustment.
    /// </summary>
    public static void ApplyHediffWithStat(
        this Pawn? pawn,
        HediffDef def,
        List<StatDef>? stats = null,
        float severityAdjust = 1.0f
    )
    {
        if (pawn is null)
            return;
        if (!stats.NullOrEmpty())
            stats!.ForEach(stat => severityAdjust *= pawn.GetStatValue(stat));

        if (pawn.health.hediffSet.GetFirstHediffOfDef(def) is { } hediff)
            hediff.Severity += severityAdjust;
        else
        {
            hediff = HediffMaker.MakeHediff(def, pawn);
            hediff.Severity = severityAdjust;
            pawn.health.AddHediff(hediff);
        }
    }

    /// <summary>
    /// Removes a hediff from the pawn.
    /// </summary>
    public static void RemoveHediff(this Pawn? pawn, HediffDef def)
    {
        if (pawn is { } p && p.health.hediffSet.GetFirstHediffOfDef(def) is { } hediff)
            p.health.RemoveHediff(hediff);
    }

    /// <summary>
    /// Checks if the pawn has a specific hediff.
    /// </summary>
    public static bool HasHediff(this Pawn? pawn, HediffDef hediff)
    {
        return pawn?.health.hediffSet.GetFirstHediffOfDef(hediff) is not null;
    }

    /// <summary>
    /// Damages a specific body part of the pawn.
    /// </summary>
    public static void DamageBodyPart(
        this Pawn? pawn,
        BodyPartRecord bodyPart,
        DamageDef? def = null,
        float amount = 9999f,
        float armorPenetration = 999f
    )
    {
        if (pawn is null || bodyPart is null)
            return;
        Debug($"Doing damage to {pawn.Name} - {bodyPart.def.label}");
        pawn.TakeDamage(
            new(def ?? DamageDefOf.SurgicalCut, amount, armorPenetration, -1f, null, bodyPart)
        );
    }

    /// <summary>
    /// Damages a random body part of the pawn.
    /// </summary>
    public static void DamageRandomBodyPart(this Pawn? pawn, float amount = 1f)
    {
        if (
            pawn is null
            || pawn.health.hediffSet.GetNotMissingParts().RandomElement() is not { } target
        )
            return;
        Debug($"Doing damage to {pawn.Name} - {target.def.defName}({target.def.LabelCap})");
        pawn.TakeDamage(new(DamageDefOf.SurgicalCut, amount, 999f, -1f, null, target));
    }

    /// <summary>
    /// Checks if the pawn is a normal colonist.
    /// <br />
    /// Using <see cref="Pawn.IsColonist"/>, <see cref="Pawn.IsSlaveOfColony"/> and <see cref="Pawn.IsPrisonerOfColony"/>.
    /// </summary>
    public static bool IsManagedColonist(this Pawn? pawn)
    {
        if (pawn is null)
            return false;
        return pawn.IsColonist || pawn.IsSlaveOfColony || pawn.IsPrisonerOfColony;
    }

    /// <summary>
    /// Spawn the pawn at the position of the thing.
    /// </summary>
    public static void SpawnToThing(this Pawn? pawn, Thing thing)
    {
        if (pawn is null || thing is null or { Map: null })
            return;
        GenSpawn.Spawn(pawn, thing.Position, thing.Map);
    }
}
