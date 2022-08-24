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

[assembly: MelonInfo(typeof(WorldMapFastTravel.WorldMapFastTravel), "World Map Fast Travel", "1.0.0", "Coomzy")]
[assembly: MelonGame("InxileEntertainment", "Wasteland 3")]
namespace WorldMapFastTravel
{
    public class WorldMapFastTravel : MelonMod
    {
        public static WorldMapFastTravel instance;

        public static MelonPreferences_Category prefsCategory;
        public static MelonPreferences_Entry<bool> canTravelToUndiscoveredEntry;

        public override void OnApplicationStart()
        {
            base.OnApplicationStart();

            instance = this;

            //Log($"WorldMapFastTravel Init!");

            prefsCategory = MelonPreferences.CreateCategory("World Map Fast Travel");
            canTravelToUndiscoveredEntry = prefsCategory.CreateEntry("canTravelToUndiscovered", false);
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
            if (!isDiscovered && !WorldMapFastTravel.canTravelToUndiscoveredEntry.Value)
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
