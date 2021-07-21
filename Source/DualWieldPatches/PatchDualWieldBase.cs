using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using RimWorld;
using Verse;
using HarmonyLib;
using System.Reflection;

namespace DArcaneTechnology.DualWieldPatches
{
    [StaticConstructorOnStartup]
    public static class PatchDualWieldBase
    {

        public static Type aou;

        static PatchDualWieldBase()
        {

            try
            {
                ((Action)(() =>
                {
                    MethodInfo target1;
                    var harmony = new Harmony("io.github.dametri.arcanetechnology");
                    if (LoadedModManager.RunningModsListForReading.Any(x => x.Name.ToLower() == "dual wield"))
                    {
                        Log.Message("Arcane Technology: Dual wield running, attempting to patch");
                        aou = AccessTools.TypeByName("DualWield.Harmony.FloatMenuMakerMap_AddHumanlikeOrders");
                        target1 = AccessTools.Method(aou, "GetEquipOffHandOption");
                        var invoke1 = AccessTools.Method(typeof(Patch_GetEquipOffHandOption_Prefix), "Prefix");
                        if (target1 != null && invoke1 != null)
                            harmony.Patch(target1, prefix: new HarmonyMethod(invoke1));
                    }
                }))();
            }
            catch (TypeLoadException ex) { Log.Message(ex.ToString()); }
        }
    }
}
