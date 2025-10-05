namespace Rhynia.Baseline.Util;

public static class FloatMenuHelper
{
    public static void SpawnMenu(params FloatMenuOption[] options)
    {
        if (options is null or { Length: 0 })
            return;
        Find.WindowStack.Add(new FloatMenu([.. options]));
    }

    public static void SpawnMenu(List<FloatMenuOption> options)
    {
        if (options is null or { Count: 0 })
            return;
        Find.WindowStack.Add(new FloatMenu(options));
    }

    public static void SpawnMenuTitled(string title, params FloatMenuOption[] options)
    {
        if (options is null or { Length: 0 })
            return;
        Find.WindowStack.Add(new FloatMenu([.. options], title));
    }

    public static void SpawnMenuTitled(string title, List<FloatMenuOption> options)
    {
        if (options is null or { Count: 0 })
            return;
        Find.WindowStack.Add(new FloatMenu(options, title));
    }

    public static void BuildMenu(this IEnumerable<FloatMenuOption> options)
    {
        if (options is null)
            return;
        if (!options.Any())
            return;
        Find.WindowStack.Add(new FloatMenu([.. options]));
    }

    public static void BuildMenu(this IEnumerable<FloatMenuOption> options, string title)
    {
        if (options is null)
            return;
        if (!options.Any())
            return;
        Find.WindowStack.Add(new FloatMenu([.. options], title));
    }
}
