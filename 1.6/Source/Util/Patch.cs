using System.Reflection;

namespace Rhynia.Baseline.Util;

/// <summary>
/// A base class for collecting patches to be applied via Harmony.
/// </summary>
public abstract class PatchBase(Harmony harmony)
{
    /// <summary>
    /// The name of the patch collection.
    /// </summary>
    public abstract string Name { get; }

    /// <summary>
    /// The mod ID that this patch collection is associated with.
    /// </summary>
    public abstract string ModId { get; }

    /// <summary>
    /// The log label used for logging messages related to this patch collection.
    /// </summary>
    public abstract string LogLabel { get; }

    /// <summary>
    /// A collection of patch providers that define the patches to be applied.
    /// </summary>
    protected abstract IEnumerable<PatchProvider> PatchProviders { get; }

    /// <summary>
    /// Determines whether all patches should be applied based on the mod's active status.
    /// <br />
    /// This check is performed before applying any patches.
    /// </summary>
    protected virtual bool ShouldApply =>
        ModsConfig.IsActive(ModId) || ModLister.GetActiveModWithIdentifier(ModId, true) is not null;

    public void Apply()
    {
        if (!ShouldApply)
        {
            I_Debug($"Skipping all patches for {Name}: {ModId} as it is not active.", LogLabel);
            return;
        }

        using var _ = TimingScope.Start(
            (t) =>
                I_Debug(
                    $"Applied all patches for {Name}: {ModId} in {t.TotalMilliseconds}ms",
                    LogLabel
                )
        );

        foreach (var provider in PatchProviders)
        {
            if (!provider.ShouldApply())
            {
                I_Debug(
                    $"Skipping patch {provider.Name} for {Name}: {ModId} as ShouldApply returned false.",
                    LogLabel
                );
                continue;
            }

            try
            {
                if (provider.TargetMethodProvider() is not { } original)
                {
                    I_Warn(
                        $"Target method for patch {provider.Name} in {Name}: {ModId} is null. Skipping patch.",
                        LogLabel
                    );
                    continue;
                }

                switch (provider.Type)
                {
                    case HarmonyPatchType.Prefix:
                        if (provider.LatePatch)
                        {
                            LongEventHandler.QueueLongEvent(
                                () =>
                                {
                                    harmony.Patch(original, prefix: provider.PatchMethodProvider());
                                    I_Info(
                                        $"Actually applied late patch {provider.Name} for {Name}: {ModId}",
                                        LogLabel
                                    );
                                },
                                $"LatePatch {provider.Name} for {Name}: {ModId}",
                                provider.LatePatchAsync,
                                (ex) =>
                                    I_Error(
                                        $"Failed to actually apply late patch {provider.Name} for {Name}: {ModId}: {ex}",
                                        LogLabel
                                    )
                            );
                        }
                        else
                            harmony.Patch(original, prefix: provider.PatchMethodProvider());
                        break;
                    case HarmonyPatchType.Postfix:
                        if (provider.LatePatch)
                        {
                            LongEventHandler.QueueLongEvent(
                                () =>
                                {
                                    harmony.Patch(
                                        original,
                                        postfix: provider.PatchMethodProvider()
                                    );
                                    I_Info(
                                        $"Actually applied late patch {provider.Name} for {Name}: {ModId}",
                                        LogLabel
                                    );
                                },
                                $"LatePatch {provider.Name} for {Name}: {ModId}",
                                provider.LatePatchAsync,
                                (ex) =>
                                    I_Error(
                                        $"Failed to actually apply late patch {provider.Name} for {Name}: {ModId}: {ex}",
                                        LogLabel
                                    )
                            );
                        }
                        else
                            harmony.Patch(original, postfix: provider.PatchMethodProvider());
                        break;
                    case HarmonyPatchType.Transpiler:
                        if (provider.LatePatch)
                        {
                            LongEventHandler.QueueLongEvent(
                                () =>
                                {
                                    harmony.Patch(
                                        original,
                                        transpiler: provider.PatchMethodProvider()
                                    );
                                    I_Info(
                                        $"Actually applied late patch {provider.Name} for {Name}: {ModId}",
                                        LogLabel
                                    );
                                },
                                $"LatePatch {provider.Name} for {Name}: {ModId}",
                                provider.LatePatchAsync,
                                (ex) =>
                                    I_Error(
                                        $"Failed to actually apply late patch {provider.Name} for {Name}: {ModId}: {ex}",
                                        LogLabel
                                    )
                            );
                        }
                        else
                            harmony.Patch(original, transpiler: provider.PatchMethodProvider());
                        break;
                    case HarmonyPatchType.Finalizer:
                        if (provider.LatePatch)
                        {
                            LongEventHandler.QueueLongEvent(
                                () =>
                                {
                                    harmony.Patch(
                                        original,
                                        finalizer: provider.PatchMethodProvider()
                                    );
                                    I_Info(
                                        $"Actually applied late patch {provider.Name} for {Name}: {ModId}",
                                        LogLabel
                                    );
                                },
                                $"LatePatch {provider.Name} for {Name}: {ModId}",
                                provider.LatePatchAsync,
                                (ex) =>
                                    I_Error(
                                        $"Failed to actually apply late patch {provider.Name} for {Name}: {ModId}: {ex}",
                                        LogLabel
                                    )
                            );
                        }
                        else
                            harmony.Patch(original, finalizer: provider.PatchMethodProvider());
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(
                            $"Unsupported patch type {provider.Type} for patch {provider.Name} in {Name}: {ModId}"
                        );
                }

                I_Info($"Applied patch {provider.Name} for {Name}: {ModId}", LogLabel);
            }
            catch (Exception e)
            {
                I_Error(
                    $"Failed to apply patch {provider.Name} for {Name}: {ModId}: {e}",
                    LogLabel
                );
            }
        }
    }
}

/// <summary>
/// A record representing a patch provider with necessary details for applying a Harmony patch.
/// </summary>
/// <param name="Name">Name of the patch provider.</param>
/// <param name="ShouldApply">A function that determines whether the patch should be applied.</param>
/// <param name="TargetMethodProvider">A function that provides the target method for the patch.</param>
/// <param name="PatchMethodProvider">A function that provides the patch method.</param>
/// <param name="Type">The type of the patch.</param>
/// <param name="LatePatch">Whether the patch should be applied late (after initialization).</param>
/// <param name="LatePatchAsync">Whether the late patch should be applied asynchronously.</param>
public record PatchProvider(
    string Name,
    Func<bool> ShouldApply,
    Func<MethodBase> TargetMethodProvider,
    Func<HarmonyMethod> PatchMethodProvider,
    HarmonyPatchType Type,
    bool LatePatch = false,
    bool LatePatchAsync = false
);
