using System.Collections.Generic;
using UnityEngine;

namespace VRToolkit.Localization
{
    public class Language
    {
        public LanguageInfo languageInfo;
        public Dictionary<string, string> localizations;

        public string Get(string key)
        {
            if (localizations.TryGetValue(key, out string value))
            {
                if (!string.IsNullOrEmpty(value))
                {
                    return value;
                }
            }

            Debug.LogWarning($"The key {key} was not found for language {languageInfo.displayName}.");

            return string.Empty;
        }
    }
}
