using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using HarmonyLib;
using Verse;
using SimpleSidearms.rimworld;

namespace DArcaneTechnology.DualWieldPatches
{
    class Patch_GetEquipOffHandOption_Prefix
    {
        private static bool Prefix(Pawn pawn, ThingWithComps equipment, ref FloatMenuOption __result)
        {
            if (Base.IsResearchLocked(equipment.def, pawn))
            {
                string labelShort = equipment.LabelShort;
                __result = new FloatMenuOption("CannotEquip".Translate(labelShort) + " " + "DW_AsOffHand".Translate() + " (" + "DUnknownTechnology".Translate() + ")", 
                    null, MenuOptionPriority.Default, null, null, 0f, null, null); ;
                return false;
            }
            return true;
        }
    }
}
