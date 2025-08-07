namespace Rhynia.Baseline;

public static class FloatMenuHelper
{
    public static void SpawnMenu(params FloatMenuOption[] options)
    {
        if (options is null or { Length: 0 })
            return;
        Find.WindowStack.Add(new FloatMenu([.. options]));
    }

    public static void SpawnMenuTitled(string title, params FloatMenuOption[] options)
    {
        if (options is null or { Length: 0 })
            return;
        Find.WindowStack.Add(new FloatMenu([.. options], title));
    }
}
