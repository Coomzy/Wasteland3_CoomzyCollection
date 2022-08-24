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
using InXile.Scenes;

[assembly: MelonInfo(typeof(Test.Test), "Test", "1.0.0", "Tester")]
[assembly: MelonGame("InxileEntertainment", "Wasteland 3")]
namespace Test
{
    public class Test : MelonMod
    {
        public static Test instance;

        public override void OnApplicationStart()
        {
            base.OnApplicationStart();

            instance = this;

            Log($"Test Init!");
        }

        void LogInternal(string log)
        {
            LoggerInstance.Msg(log);
        }

        public static void Log(string log)
        {
            instance.LogInternal(log);
        }
    }

    [HarmonyPatch(typeof(WorldMapManager), "SetPlayerDestination_Server", MethodType.Normal)]
    class Patch_1
    {
        static void Postfix(ref Vector3 destination, ref bool showPath, ref WorldMapManager __instance)
        {
            Test.Log($"SetPlayerDestination_Server() destination = {destination}, showPath = {showPath}");
        }
    }

    [HarmonyPatch(typeof(WorldMapManager), "StartAutoTravel_Server", MethodType.Normal)]
    class Patch_2
    {
        static void Postfix(ref Vector3 destination, ref WorldMapManager __instance)
        {
            Test.Log($"StartAutoTravel_Server() destination = {destination}");
        }
    }

    [HarmonyPatch(typeof(WorldMapManager), "SetWorldMapPromptActive", MethodType.Normal)]
    class Patch_3
    {
        static void Postfix(ref bool newState, ref WorldMapManager __instance)
        {
            Test.Log($"SetWorldMapPromptActive() newState = {newState}");
        }
    }

    [HarmonyPatch(typeof(WorldMapManager), "RequestTransition", MethodType.Normal)]
    class Patch_5
    {
        static void Postfix(ref WorldMapSceneStartOption startOption, ref WorldMapManager __instance)
        {
            Test.Log($"RequestTransition() startOption = {startOption}");
        }
    }

    [HarmonyPatch(typeof(WorldMapManager), "OpenPrompt", MethodType.Normal)]
    class Patch_6
    {
        static void Postfix(ref WorldMapTransition transition, ref WorldMapManager __instance)
        {
            Test.Log($"OpenPrompt() transition = {transition.returnLocationIdentifier}");
        }
    }

    [HarmonyPatch(typeof(WorldMapManager), "DoTransitionMap", MethodType.Normal)]
    class Patch_7
    {
        static void Postfix(ref Scenes scene, ref WorldMapManager __instance)
        {
            Test.Log($"DoTransitionMap() scene = {scene}");
        }
    }

    [HarmonyPatch(typeof(WorldMapManager), "TransitionMap", MethodType.Normal)]
    class Patch_8
    {
        static void Postfix(ref Scenes sceneToLoad, ref WorldMapManager __instance)
        {
            Test.Log($"TransitionMap() sceneToLoad = {sceneToLoad}");
        }
    }

    [HarmonyPatch(typeof(Cutscene_0_TowToHQ), "StartCutsceneImpl", MethodType.Normal)]
    class Patch_9
    {
        static void Postfix(ref Cutscene_0_TowToHQ __instance)
        {
            if (__instance?.WP_TeleportDestination != null)
            {
                Test.Log($"StartCutsceneImpl() WP_TeleportDestination = {__instance.WP_TeleportDestination.name} - {__instance.WP_TeleportDestination.position}");
            }
            else
            {
                Test.Log($"StartCutsceneImpl() WP_TeleportDestination = null");
            }
        }
    }

    [HarmonyPatch(typeof(InteractableWorldMapPOI), "SetPOI", MethodType.Normal)]
    class Patch_10
    {
        static void Postfix(ref POI point, ref InteractableWorldMapPOI __instance)
        {
            Test.Log($"SetPOI() point = {point.displayName}");
        }
    }

    [HarmonyPatch(typeof(POI), "OnPOIOverlap", MethodType.Normal)]
    class Patch_11
    {
        static void Postfix(bool didDiscover, ref POI __instance)
        {
            Test.Log($"OnPOIOverlap() point = {__instance.displayName}");
        }
    }

    [HarmonyPatch(typeof(WorldMapHUDController), "OnRadioClicked", MethodType.Normal)]
    class Patch_12
    {
        static void Postfix(ref WorldMapHUDController __instance)
        {
            Test.Log($"OnRadioClicked()");
        }
    }

    [HarmonyPatch(typeof(POI), "GetDisplayName", MethodType.Normal)]
    class Patch_13
    {
        static void Postfix(ref string __result, ref POI __instance)
        {
            Test.Log($"GetDisplayName() __result = {__result}");
        }
    }

    [HarmonyPatch(typeof(POI), "GetDisplayName", MethodType.Normal)]
    class Patch_14
    {
        static void Postfix(ref string __result, ref POI __instance)
        {
            Test.Log($"GetDisplayName() __result = {__result}");
        }
    }

    [HarmonyPatch(typeof(MiniMapWindow), "OnMarkerSelected", MethodType.Normal)]
    class Patch_15
    {
        static void Postfix(ref MiniMapObject marker, ref MiniMapWindow __instance)
        {
            Test.Log($"OnMarkerSelected() text = {marker.Text}, LinkedObject = {marker.LinkedObject.name}");
        }
    }
}
