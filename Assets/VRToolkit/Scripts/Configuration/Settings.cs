using System;
using System.IO;
using UnityEngine;
using VRToolkit.Utils;

namespace VRToolkit.Configuration
{
    public static class Settings
    {
        public static SettingsSO ReadSettings()
        {
            try
            {
                string settingsJson = File.ReadAllText(Statics.settingsPath);

                if (!string.IsNullOrEmpty(settingsJson))
                {
                    SettingsSO settings = ScriptableObject.CreateInstance<SettingsSO>();
                    JsonUtility.FromJsonOverwrite(settingsJson, settings);
                    return settings;
                }

                Debug.LogWarning($"Something is wrong with your json file, loading default.");
            }
            catch (Exception e)
            {
                Debug.LogWarning($"An error ocurred reading settings json file, loading default. Error: {e}");
            }

            return Resources.Load<SettingsSO>(Statics.defaultSettings);
        }
    }
}
