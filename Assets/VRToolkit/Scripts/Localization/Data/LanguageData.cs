using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using VRToolkit.Managers;

namespace VRToolkit.Localization
{
    public class LanguageData
    {
        public Texture2D flag;
        public Language language; 

        public LanguageData(string flagPath)
        {
            VRToolkitManager.Instance.StartCoroutine(LoadFlag(flagPath));
        }

        public bool LoadData(string filePath)
        {
            bool response = false;
            try
            {
                string body = File.ReadAllText(filePath);
                if (!string.IsNullOrEmpty(body))
                {
                    language = JsonConvert.DeserializeObject<Language>(body);
                    response = true;
                }
                else
                {
                    Debug.LogError($"File {filePath} was empty or null.");
                }
            }
            catch(Exception e)
            {
                Debug.LogError($"Error reading file: {filePath}, with error {e.Message}");
            }

            return response;
        }

        public void AddExtraData(string filePath)
        {
            try
            {
                string body = File.ReadAllText(filePath);
                if (!string.IsNullOrEmpty(body))
                {
                    Dictionary<string, string> extraLocalization = JsonConvert.DeserializeObject<Dictionary<string, string>>(body);

                    foreach (KeyValuePair<string, string> extra in extraLocalization)
                    {
                        language.localizations.Add(extra.Key, extra.Value);
                    }
                }
                else
                {
                    Debug.LogError($"File {filePath} was empty or null.");
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error reading file: {filePath}, with error {e.Message}");
            }
        }

        private IEnumerator LoadFlag(string filePath)
        {
            using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture($"file://{filePath}"))
            {
                yield return uwr.SendWebRequest();

                if (uwr.responseCode != 200)
                {
                    Debug.Log(uwr.error);
                }
                else
                {
                    flag = DownloadHandlerTexture.GetContent(uwr);
                }
            }
        }

        public string Get(string key)
        {
            return language.Get(key);
        }
    }
}
