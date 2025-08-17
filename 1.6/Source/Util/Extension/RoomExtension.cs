namespace Rhynia.Baseline.Util;

public static class RoomExtension
{
    /// <summary>
    /// Gets all the things in the room's ThingGrid.
    /// </summary>
    public static IEnumerable<Thing> ThingGrid(this Room? room) =>
        room?.Cells.SelectMany(cell => cell.GetThingList(room.Map)) ?? [];
}
