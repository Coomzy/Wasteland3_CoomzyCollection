using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using BepInEx.IL2CPP;
using HarmonyLib;

namespace FreeRetrain_BepInEx
{
    [BepInPlugin("coomzy.FreeRetrain", "FreeRetrain", "1.0.0")]
    public class FreeRetrain_BepInEx : BasePlugin
    {
        public static Harmony harmony;

        public override void Load()
        {
            Log.LogMessage("FreeRetrain loaded!");

            harmony = new Harmony("coomzy.FreeRetrain");
            harmony.PatchAll();
        }
    }

    [HarmonyPatch(typeof(RetrainCharacterUtil), "GetCostForRetrain", MethodType.Normal)]
    class Patch_1 { [HarmonyPostfix] static void Postfix(ref int __result) { __result = 0; } }

    [HarmonyPatch(typeof(RetrainCharacterUtil), "CanAffordToRetrain", MethodType.Normal)]
    class Patch_2 { [HarmonyPostfix] static void Postfix(ref bool __result) { __result = true; } }
}
