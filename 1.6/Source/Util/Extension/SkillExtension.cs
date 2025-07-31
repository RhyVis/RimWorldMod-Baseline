namespace Rhynia.Baseline.Util;

public static class SkillExtension
{
    /// <summary>
    /// Depletes the skill level by the specified amount of experience points.
    /// If the skill level is already at 0 or the experience points are less than or equal to 0,
    /// the method returns false without making any changes.
    /// If the skill level is greater than 0, it reduces the level and adjusts the
    /// experience points accordingly. If the level reaches 0, it resets the experience points to 0.
    /// The method returns true if the skill level was successfully depleted.
    /// </summary>
    public static bool DepleteSkillLevel(this SkillRecord skillRecord, float xp)
    {
        if (skillRecord.levelInt <= 0 || skillRecord.TotallyDisabled || xp <= 0)
            return false;

        while (xp > skillRecord.xpSinceLastLevel)
        {
            --skillRecord.levelInt;
            skillRecord.xpSinceLastLevel += skillRecord.XpRequiredForLevelUp;

            if (skillRecord.levelInt > 0)
                continue;

            skillRecord.levelInt = 0;
            skillRecord.xpSinceLastLevel = 0f;
            return true;
        }

        if (skillRecord.xpSinceLastLevel >= xp)
            skillRecord.xpSinceLastLevel -= xp;

        skillRecord.xpSinceLastLevel = 0;
        return true;
    }
}
