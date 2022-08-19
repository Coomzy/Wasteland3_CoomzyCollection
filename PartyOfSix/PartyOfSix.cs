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
using UnityEngine;

[assembly: MelonInfo(typeof(PartyOfSix.PartyOfSix), "Party of Six", "1.0.0", "Coomzy")]
[assembly: MelonGame("InxileEntertainment", "Wasteland 3")]
namespace PartyOfSix
{
    public class PartyOfSix : MelonMod {}

    [HarmonyPatch(typeof(PartyMemberTrade), "GetNumPlayerCreatedCharactersAfterTrade", MethodType.Normal)]
    class Patch_1 { static void Postfix(ref int __result) { __result = 3; } }

    [HarmonyPatch(typeof(PartyMemberTrade), "WillPartyBeOverfilled", MethodType.Normal)]
    class Patch_2 { static void Postfix(ref bool __result) { __result = false; } }

    [HarmonyPatch(typeof(PartyMemberTrade), "WillPartiesHaveTooManyCNPCs", MethodType.Normal)]
    class Patch_3 { static void Postfix(ref bool __result) { __result = false; } }

    [HarmonyPatch(typeof(PartyMemberTrade), "WillPartiesHaveLessThanMaxCreatedCharacters", MethodType.Normal)]
    class Patch_4 { static void Postfix(ref bool __result) { __result = true; } }

    [HarmonyPatch(typeof(PartyManagementWindow), "DoesSinglePlayerHaveLessThanMaxCreatedCharacters", MethodType.Normal)]
    class Patch_5 { static void Postfix(ref bool __result) { __result = true; } }

    [HarmonyPatch(typeof(PartyManagementWindow), "DoBothPartiesHaveLessThanMaxCreatedCharacters", MethodType.Normal)]
    class Patch_6 { static void Postfix(ref bool __result) { __result = true; } }
}
