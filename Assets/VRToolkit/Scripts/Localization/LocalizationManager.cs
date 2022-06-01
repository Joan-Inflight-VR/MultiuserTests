using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using VRToolkit.AnalyticsWrapper;
using VRToolkit.Managers;
using VRToolkit.Utils;

namespace VRToolkit.Localization
{
    public static class LocalizationManager
    {
        public static string CurrentLanguage { get; private set; }

        public static string staticDefaultLanguage;

        private static Dictionary<string, LanguageData> localizedDictionary = new Dictionary<string, LanguageData>();

        public static void LoadLocalizations()
        {
            DirectoryInfo localizationDirectory = new DirectoryInfo(Statics.localizationPath);

            if (!localizationDirectory.Exists)
            {
                Debug.LogError("Localization Folder does not exists.");
                return;
            }

            string path = Statics.localizationPath + "Languages";
            DirectoryInfo info = new DirectoryInfo(path);

            if (!info.Exists)
            {
                Debug.LogError("Localization Folder (Languages) does not exists.");
                return;
            }
            
            //get the different language folders 
            DirectoryInfo[] directories = info.GetDirectories();
            if (directories == null || directories.Length == 0)
            {
                Debug.LogError("Main Localization Folder is Empty.");
                return;
            }

            //read the language info for each language
            for (int i = 0; i < directories.Length; ++i)
            {
                string directoryName = directories[i].Name;

                FileInfo[] directoryFiles = directories[i].GetFiles();
                if (directoryFiles == null || directoryFiles.Length == 0)
                {
                    Debug.LogError($"Localization Folder {directoryName} is Empty.");
                    continue;
                }

                AddLocalization(directoryName, path + "/" + directoryName);
            }

            Debug.Log($"<color=Blue>All available files loaded.</color>");
        }

        private static void AddLocalization(string languageCode, string path)
        {
            string flagImg = string.Concat(languageCode, ".png");
            string flagPath = Path.Combine(path, flagImg);

            localizedDictionary[languageCode] = new LanguageData(flagPath);
            if (localizedDictionary[languageCode].LoadData($"{path}/{languageCode}.json"))
            {
                Debug.Log($"<color=Green>Language {languageCode}: successfully loaded.</color>");
            }

            // Add survey and tutorial localization
            localizedDictionary[languageCode].AddExtraData($"{path}/survey.json");
            localizedDictionary[languageCode].AddExtraData($"{path}/tutorial.json");
        }

        /// <summary>
        /// Set the language code to use for localization
        /// example: en-gb, es-es
        /// </summary>
        /// <param name="languageCode"></param>
        public static void SetLanguage(string languageCode)
        {
            if (string.IsNullOrEmpty(languageCode) || (CurrentLanguage != null && CurrentLanguage.Equals(languageCode)) || !GetLanguagesLCID().Contains(languageCode)) return;

            CurrentLanguage = languageCode;

            AnalyticsObject details = new AnalyticsObject();
            details.AddData("language_code", languageCode);

            AnalyticsManager.RecordEvent(Statics.AnalyticsEvents.languageOnSelect, details);

            EventManager.Instance.TriggerEvent(Statics.Events.onLanguageChange);
        }

        /// <summary>
        /// Gets the value of the localized string
        /// </summary>
        /// <param name="localizationKey">the key for the string to localize</param>
        /// <returns>The localized string for the current language</returns>
        public static string Get(string localizationKey, string languageCode = "")
        {
            if (string.IsNullOrEmpty(localizationKey)) { return string.Empty; }

            string lan_code = string.IsNullOrEmpty(languageCode) ? CurrentLanguage : languageCode;

            if (localizedDictionary.TryGetValue(lan_code, out LanguageData value))
            {
                string temp = value.Get(localizationKey);
                if (!string.IsNullOrEmpty(temp))
                {
                    return temp;
                }
            }

            if (localizedDictionary.TryGetValue(staticDefaultLanguage, out LanguageData defaultLanguageData))
            {
                string temp = defaultLanguageData.Get(localizationKey);
                if (!string.IsNullOrEmpty(temp))
                {
                    return temp;
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Gets the dynamic localization for the specified bundle ID and language code. This can be found in the PersistentDataPath/Inflight/DynamicLocalization folder.
        /// </summary>
        /// <param name="bundleId">Bundle ID for the app you want to load the language.</param>
        /// <param name="languageCode">The language code you want to load</param>
        /// <returns></returns>
        public static Dictionary<string, string> GetDynamicLocalization(string bundleId, string languageCode = "")
        {
            string specifiedLanguage = string.IsNullOrEmpty(languageCode) ? CurrentLanguage : languageCode;

            string path = Statics.localizationPath + "DynamicLocalizations/" + specifiedLanguage + ".json";
            try
            {
                string contentToDeserialize = File.ReadAllText(path);

                Dictionary<string, Dictionary<string, string>> dynamicLocalizations = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(contentToDeserialize);

                if (dynamicLocalizations.TryGetValue(bundleId, out Dictionary<string, string> specificLocalization))
                {
                    return specificLocalization;
                }
            }
            catch (Exception e)
            {
                Debug.LogError("An error has ocurred when parsing the DynamicLozalization: " + e.ToString());
            }

            return null;
        }

        /// <summary>
        /// Loads the flag for a specific language if it is available, only on device
        /// </summary>
        /// <param name="languageCode">The language to retrieve the flag for</param>
        /// <returns>A texture2D for the flag if available, null if not</returns>
        public static Texture2D GetFlag(string languageCode)
        {
            if (string.IsNullOrEmpty(languageCode)) { return null; }
            if (localizedDictionary.TryGetValue(languageCode, out LanguageData value))
            {
                return value.flag;
            }

            return null;
        }

        /// <summary>
        /// Get all of the languages available from those that have been automatically loaded
        /// </summary>
        /// <returns>An array of strings containing the languages available</returns>
        public static List<string> GetLanguagesLCID()
        {
            return new List<string>(localizedDictionary?.Keys);
        }

        /// <summary>
        /// Get all of the languages available from those that have been automatically loaded
        /// </summary>
        /// <returns>A List of LanguageData containing the languages available</returns>
        public static List<LanguageData> GetLanguagesData()
        {
            return new List<LanguageData>(localizedDictionary?.Values);
        }

        /// <summary>
        /// Returns the LanguageData object by lcid.
        /// </summary>
        /// <param name="lcid">The LCID of the language you want to obtain data</param>
        /// <returns>A LanguageData object stored in memory</returns>
        public static LanguageData GetLangdata(string lcid)
        {
            return localizedDictionary[lcid];
        }
    }
}
