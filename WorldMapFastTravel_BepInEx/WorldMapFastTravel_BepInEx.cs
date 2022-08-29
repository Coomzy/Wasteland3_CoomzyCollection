using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using HarmonyLib;
using InXile.UI.WL3;
using UnityEngine;
using Input = UnityEngine.Input;

namespace WorldMapFastTravel_BepInEx
{
    [BepInPlugin("coomzy.WorldMapFastTravel", "WorldMapFastTravel", "1.0.0")]
    public class WorldMapFastTravel_BepInEx : BasePlugin
    {
        public static Harmony harmony;
        public static ConfigEntry<bool> canTravelToUndiscoveredEntry;

        public override void Load()
        {
            Log.LogMessage("WorldMapFastTravel loaded!");

            harmony = new Harmony("coomzy.WorldMapFastTravel");
            harmony.PatchAll();

            canTravelToUndiscoveredEntry = Config.Bind("General",
                                            "canTravelToUndiscoveredEntry",
                                            false,
                                            "Allows traveling to undiscovered map markers. DICLAIMER: More likely to get game breaking bugs!");
        }
    }

    [HarmonyPatch(typeof(MiniMapWindow), "OnMarkerClicked", MethodType.Normal)]
    class Patch_MiniMapWindow_OnMarkerClicked
    {
        static void Postfix(ref MiniMapObject marker, ref MiniMapWindow __instance)
        {
            // Right mouse button allows fast travel
            if (!Input.GetKeyUp(KeyCode.Mouse1))
            {
                return;
            }

            bool isDiscovered = true;

            // Undiscovered locations will have the [Undiscovered] suffix, we need to remove it for the text to find the correct POI later
            var textParsed = marker.Text;
            int undiscoveredIndex = textParsed.IndexOf('[');
            if (undiscoveredIndex != -1)
            {
                undiscoveredIndex--;
                textParsed = textParsed.Substring(0, undiscoveredIndex);
                isDiscovered = false;
            }

            // Setting for if we can fast travel to undiscovered locations, off by default as it's more likely to cause quest breaking bugs by missing triggers on the world map
            if (!isDiscovered && !WorldMapFastTravel_BepInEx.canTravelToUndiscoveredEntry.Value)
            {
                return;
            }

            //WorldMapFastTravel.Log($"OnMarkerClicked() text = {marker.Text}, textParsed = {textParsed}, LinkedObject = {marker.LinkedObject.name}");

            var pois = UnityEngine.Object.FindObjectsOfType<POI>();
            POI selectedPOI = null;
            foreach (var poi in pois)
            {
                if (poi.displayName == textParsed)
                {
                    selectedPOI = poi;
                    break;
                }

                //WorldMapFastTravel.Log($"POI displayName = {poi.displayName}");
            }

            var wmts = UnityEngine.Object.FindObjectsOfType<WorldMapTransition>();
            WorldMapTransition selectedWmt = null;
            foreach (var wmt in wmts)
            {
                if (selectedPOI != null && wmt.myPOI == selectedPOI)
                {
                    selectedWmt = wmt;
                    break;
                }

                //WorldMapFastTravel.Log($"wmt POI displayName = {wmt.myPOI?.displayName}");
            }

            if (selectedWmt != null)
            {
                // Open Prompt for fast travel
                WorldMapManager.instance.OpenPrompt(selectedWmt);

                // Close the Mini Map Window
                __instance.OnCloseClicked();

                //WorldMapFastTravel.Log($"selectedPOI displayName = {selectedPOI.displayName}, uniqueId = {selectedPOI.uniqueId}");
            }
        }
    }
}
