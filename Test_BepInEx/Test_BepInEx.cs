using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using BepInEx.IL2CPP;
using HarmonyLib;
using UnityEngine;

namespace Test_BepInEx
{
    [BepInPlugin("com.coomzy.coomzytest", "Coomzy Test", "1.0.0")]
    public class Test_BepInEx : BasePlugin
    {
        public static Test_BepInEx instance;
        public static Harmony harmony;

        public override void Load()
        {
            instance = this;

            Log.LogMessage("Coomzy BepInEx Test Plugin is loaded!");

            harmony = new Harmony("com.coomzy.coomzytest");
            harmony.PatchAll();
        }
    }

    [HarmonyPatch(typeof(ItemManager), "Init", MethodType.Normal)]
    class Patch_1
    { 
        [HarmonyPostfix] 
        static void Postfix(ref ItemManager __instance)
        {
            Test_BepInEx.instance.Log.LogMessage($"ItemManager::Init() items = {__instance.items.Count}");

            for (int i = 0; i < __instance.items.Count; i++)
            {
                var item = __instance.items[i];
                //Test_BepInEx.instance.Log.LogMessage($"{item.DisplayName},{__instance.items[i].GetIl2CppType().Name},{item.ResourceID.Path}");

                if (item.DisplayName == "SOCOM Assault Rifle")
                {
                    var socomRifle = (item as Il2CppSystem.Object).TryCast<ItemTemplate_WeaponRanged>();

                    Test_BepInEx.instance.Log.LogMessage($"");
                    Test_BepInEx.instance.Log.LogMessage($"{socomRifle.DisplayName} armorPenetrationAsFlat: {socomRifle.statBonus.armorPenetrationAsFlat}");
                    Test_BepInEx.instance.Log.LogMessage($"");
                }
            }

            foreach (var item in __instance.itemsIndexLookup)
            {
                //Test_BepInEx.instance.Log.LogMessage($"{item.Key}, {item.Value}");
            }
        }
    }
}
