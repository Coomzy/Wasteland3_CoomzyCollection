using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MelonLoader;
//using Harmony; // If you're using 0.3.0 and lower
using HarmonyLib;
using InXile.Trade;
using InXile.UI.WL3;

[assembly: MelonInfo(typeof(FreeRetrain.FreeRetrain), "Free Retrain", "1.0.0", "Coomzy")]
[assembly: MelonGame("InxileEntertainment", "Wasteland 3")]
namespace FreeRetrain
{
    public class FreeRetrain : MelonMod { }

    [HarmonyPatch(typeof(RetrainCharacterUtil), "GetCostForRetrain", MethodType.Normal)]
    class Patch_1 { static void Postfix(ref int __result) { __result = 0; } }

    [HarmonyPatch(typeof(RetrainCharacterUtil), "CanAffordToRetrain", MethodType.Normal)]
    class Patch_2 { static void Postfix(ref bool __result) { __result = true; } }
}
