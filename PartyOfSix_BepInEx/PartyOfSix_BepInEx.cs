using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using BepInEx.IL2CPP;
using HarmonyLib;
using InXile.Trade;
using InXile.UI.WL3;

namespace PartyOfSix_BepInEx
{
    [BepInPlugin("coomzy.PartyOfSix", "PartyOfSix", "1.0.0")]
    public class PartyOfSix_BepInEx : BasePlugin
    {
        public static Harmony harmony;

        public override void Load()
        {
            Log.LogMessage("PartyOfSix loaded!");

            harmony = new Harmony("coomzy.PartyOfSix");
            harmony.PatchAll();
        }
    }

    [HarmonyPatch(typeof(PartyMemberTrade), "GetNumPlayerCreatedCharactersAfterTrade", MethodType.Normal)]
    class Patch_1 { [HarmonyPostfix] static void Postfix(ref int __result) { __result = 3; } }

    [HarmonyPatch(typeof(PartyMemberTrade), "WillPartyBeOverfilled", MethodType.Normal)]
    class Patch_2 { [HarmonyPostfix] static void Postfix(ref bool __result) { __result = false; } }

    [HarmonyPatch(typeof(PartyMemberTrade), "WillPartiesHaveTooManyCNPCs", MethodType.Normal)]
    class Patch_3 { [HarmonyPostfix] static void Postfix(ref bool __result) { __result = false; } }

    [HarmonyPatch(typeof(PartyMemberTrade), "WillPartiesHaveLessThanMaxCreatedCharacters", MethodType.Normal)]
    class Patch_4 { [HarmonyPostfix] static void Postfix(ref bool __result) { __result = true; } }

    [HarmonyPatch(typeof(PartyManagementWindow), "DoesSinglePlayerHaveLessThanMaxCreatedCharacters", MethodType.Normal)]
    class Patch_5 { [HarmonyPostfix] static void Postfix(ref bool __result) { __result = true; } }

    [HarmonyPatch(typeof(PartyManagementWindow), "DoBothPartiesHaveLessThanMaxCreatedCharacters", MethodType.Normal)]
    class Patch_6 { [HarmonyPostfix] static void Postfix(ref bool __result) { __result = true; } }
}
