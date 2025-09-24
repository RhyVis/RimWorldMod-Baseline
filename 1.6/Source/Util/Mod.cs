namespace Rhynia.Baseline.Util;

public static class ModUtil
{
    /// <summary>
    /// Checks if a mod with the given modId is active.
    /// <br/>
    /// Twice checks ModsConfig and ModLister to ensure local mods compatibility.
    /// </summary>
    public static bool IsModActive(string modId) =>
        ModsConfig.IsActive(modId) || ModLister.GetActiveModWithIdentifier(modId, true) is not null;
}
