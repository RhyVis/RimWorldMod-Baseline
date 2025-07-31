namespace Rhynia.Baseline.Util;

public static class RoomExtension
{
    public static IEnumerable<Thing> ThingGrid(this Room? room) =>
        room?.Cells.SelectMany(cell => cell.GetThingList(room.Map)) ?? [];
}
