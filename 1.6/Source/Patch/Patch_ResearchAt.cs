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
                Info("Applied Patch_ResearchAt.");
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
        var codes = new List<CodeInstruction>(instructions);
        var found = false;

        // Field access patterns to search for
        var requiredResearchBuildingField = AccessTools.Field(
            typeof(ResearchProjectDef),
            nameof(ResearchProjectDef.requiredResearchBuilding)
        );
        var thingDefField = AccessTools.Field(typeof(Thing), "def");

        for (int i = 0; i < codes.Count - 4; i++)
        {
            // Pattern to find:
            // ldarg.1 (bench)
            // ldfld Thing::def
            // ldarg.0 (this)
            // ldfld ResearchProjectDef::requiredResearchBuilding
            // beq.s (branch instruction)
            if (
                codes[i].opcode == OpCodes.Ldarg_1
                && codes[i + 1].opcode == OpCodes.Ldfld
                && codes[i + 1].operand.Equals(thingDefField)
                && codes[i + 2].opcode == OpCodes.Ldarg_0
                && codes[i + 3].opcode == OpCodes.Ldfld
                && codes[i + 3].operand.Equals(requiredResearchBuildingField)
                && codes[i + 4].opcode == OpCodes.Beq_S
            )
            {
                // Replace with DefModExt_EqualResearchUse.IsValid(requiredResearchBuilding, bench.def)
                var newInstructions = new List<CodeInstruction>
                {
                    // Load this.requiredResearchBuilding (first parameter)
                    new(OpCodes.Ldarg_0),
                    new(OpCodes.Ldfld, requiredResearchBuildingField),
                    // Load bench.def (second parameter)
                    new(OpCodes.Ldarg_1),
                    new(OpCodes.Ldfld, thingDefField),
                    // Call IsReplaceable method
                    new(
                        OpCodes.Call,
                        AccessTools.Method(
                            typeof(DefModExt_EqualResearchUse_Helper),
                            nameof(DefModExt_EqualResearchUse_Helper.IsValid)
                        )
                    ),
                    // If returns true, jump to original target (continue); if returns false, continue to next instruction (return false)
                    new(OpCodes.Brtrue_S, codes[i + 4].operand),
                };

                // Remove original 5 instructions and insert new instructions
                codes.RemoveRange(i, 5);
                codes.InsertRange(i, newInstructions);

                found = true;
                Debug("Successfully replaced research building comparison logic.");
                break;
            }
        }

        if (!found)
            Error("Could not find target IL pattern for research building comparison.");

        return codes;
    }
}
