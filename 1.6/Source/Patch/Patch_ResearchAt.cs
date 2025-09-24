namespace Rhynia.Baseline;

internal static class Patch_ResearchAt
{
    internal static void Apply(Harmony harmony)
    {
        try
        {
            if (
                AccessTools.Method(
                    typeof(ResearchProjectDef),
                    nameof(ResearchProjectDef.CanBeResearchedAt)
                ) is
                { } method
            )
            {
                harmony.Patch(
                    method,
                    transpiler: new(typeof(Patch_ResearchAt), nameof(Transpiler))
                );
                Debug("Applied Patch_ResearchAt.");
            }
            else
                Error("Failed to find target method for Patch_ResearchAt.");
        }
        catch (Exception ex)
        {
            Error($"Failed to apply Patch_ResearchAt: {ex}");
        }
    }

    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var matcher = new CodeMatcher(instructions);

        var thingDefField = AccessTools.Field(typeof(Thing), nameof(Thing.def));
        var requiredResearchBuildingField = AccessTools.Field(
            typeof(ResearchProjectDef),
            nameof(ResearchProjectDef.requiredResearchBuilding)
        );

        matcher
            .MatchStartForward(
                new(OpCodes.Ldarg_1),
                new(OpCodes.Ldfld, thingDefField),
                new(OpCodes.Ldarg_0),
                new(OpCodes.Ldfld, requiredResearchBuildingField),
                new(OpCodes.Beq_S)
            )
            .ThrowIfInvalid("Could not find target IL pattern for research building comparison.");

        var bi = matcher.InstructionAt(4).Clone().operand;

        matcher
            .RemoveInstructions(5)
            .Insert(
                new(OpCodes.Ldarg_0),
                new(OpCodes.Ldfld, requiredResearchBuildingField),
                new(OpCodes.Ldarg_1),
                new(OpCodes.Ldfld, thingDefField),
                new(
                    OpCodes.Call,
                    AccessTools.Method(
                        typeof(DefModExt_EqualResearchUse_Helper),
                        nameof(DefModExt_EqualResearchUse_Helper.IsValid)
                    )
                ),
                new(OpCodes.Brtrue_S, bi)
            );

        return matcher.InstructionEnumeration();
    }
}
