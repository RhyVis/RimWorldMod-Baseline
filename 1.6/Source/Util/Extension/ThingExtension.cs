namespace Rhynia.Baseline.Util;

/// <summary>
/// Extension methods for the <see cref="Thing"/> class.
/// </summary>
public static class ThingExtension
{
    /// <summary>
    /// Finds all pawns within a specified range of the given thing.
    /// <br/>
    /// The calculation is based on the squared distance.
    /// </summary>
    public static IEnumerable<Pawn> FindPawnsInRange(this Thing? thing, float range)
    {
        if (thing is null || thing.Map is null)
            return [];
        var rangeSquared = range * range;
        return thing.Map.mapPawns.AllPawnsSpawned.Where(pawn =>
            pawn.Position.DistanceToSquared(thing.Position) < rangeSquared
        );
    }

    /// <summary>
    /// Similar to <see cref="FindPawnsInRange(Thing?, float)"/>, but only returns pawns that are alive.
    /// </summary>
    public static IEnumerable<Pawn> FindPawnsAliveInRange(this Thing? thing, float range) =>
        thing?.FindPawnsInRange(range).Where(pawn => pawn?.Dead == false) ?? [];

    /// <summary>
    /// Based on <see cref="FindPawnsInRange(Thing?, float)"/>.
    /// </summary>
    public static IEnumerable<Pawn> FindPawnsInRange(this ThingComp? comp, float range) =>
        comp?.parent?.FindPawnsInRange(range) ?? [];

    /// <summary>
    /// Based on <see cref="FindPawnsAliveInRange(Thing?, float)"/>.
    /// </summary>
    public static IEnumerable<Pawn> FindPawnsAliveInRange(this ThingComp? comp, float range) =>
        comp?.parent?.FindPawnsAliveInRange(range) ?? [];

    /// <summary>
    /// Sets the stack count of the thing.
    /// <br />
    /// If the count is less than or equal to zero, the thing is destroyed.
    /// If the count exceeds the stack limit, it is capped at the stack limit.
    /// If the thing is null, no action is taken.
    /// If the count is valid, it updates the stack count of the thing.
    /// </summary>
    /// <returns>The updated thing or null if it was destroyed or invalid.</returns>
    public static Thing? SetStackCount(this Thing? thing, int count)
    {
        if (thing is null)
            return null;

        if (count <= 0)
        {
            thing.Destroy();
            return null;
        }

        thing.stackCount = count <= thing.def.stackLimit ? count : thing.def.stackLimit;
        return thing;
    }

    /// <summary>
    /// Throws a text mote at the thing's position.
    /// </summary>
    public static void ThrowMote(this Thing? thing, string s)
    {
        if (thing is null || thing.Map is null)
            return;

        MoteMaker.ThrowText(thing.DrawPos, thing.Map, s);
    }

    /// <summary>
    /// Throws a text mote at the comp's parent position.
    /// </summary>
    public static void ThrowMote(this ThingComp? comp, string s) => comp?.parent?.ThrowMote(s);

    /// <summary>
    /// Spawns a thing at the specified position in the given map.
    /// </summary>
    public static void SpawnAt(this ThingDef def, Map map, IntVec3 pos, int count = 1)
    {
        var thing = ThingMaker.MakeThing(def);
        thing.SetStackCount(count);
        GenPlace.TryPlaceThing(thing, pos, map, ThingPlaceMode.Near);
    }

    /// <summary>
    /// Checks if the thing has a specific designation.
    /// </summary>
    /// <param name="thing">The thing to check.</param>
    /// <param name="def">The designation definition to check for.</param>
    /// <returns>True if the thing has the designation, false otherwise.</returns>
    public static bool HasDesignation(this Thing? thing, DesignationDef def)
    {
        if (thing is null || thing.Map is null)
            return false;
        return thing.Map.designationManager.DesignationOn(thing, def) is not null;
    }

    /// <summary>
    /// Adds a designation to the thing.
    /// </summary>
    /// <param name="thing">The thing to add the designation to.</param>
    /// <param name="def">The designation definition to add.</param>
    public static void AddDesignation(this Thing? thing, DesignationDef def)
    {
        if (thing is null || thing.Map is null)
            return;
        if (thing.Map.designationManager.DesignationOn(thing, def) is null)
            thing.Map.designationManager.AddDesignation(new Designation(thing, def));
    }

    /// <summary>
    /// Removes a designation from the thing.
    /// </summary>
    /// <param name="thing">The thing to remove the designation from.</param>
    /// <param name="def">The designation definition to remove.</param>
    public static void RemoveDesignation(this Thing? thing, DesignationDef def)
    {
        if (thing is null || thing.Map is null)
            return;
        var designation = thing.Map.designationManager.DesignationOn(thing, def);
        if (designation is not null)
            thing.Map.designationManager.RemoveDesignation(designation);
    }
}
