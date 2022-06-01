using System.IO;
using UnityEngine;
using UnityEngine.XR;

namespace VRToolkit.Utils
{
    /// <summary>
    /// Static common Utilities class
    /// </summary>
    public static class Utilities
    {
        /// <summary>
        /// Creates an array of type T from JSONArray data.
        /// </summary>
        /// <typeparam name="T">Generic type of the array.</typeparam>
        /// <param name="data">JSONArray containing the info.</param>
        /// <param name="target">The target array we want to fill with the data.</param>
        public static void CreateArray<T>(JSONArray data, ref T[] target)
        {
            if (data != null && data.Count > 0)
            {
                target = new T[data.Count];
            }
            for (int i = 0; i < data.Count; ++i)
            {
                target[i] = (T)(object)data[i].Value;
            }
        }

        /// <summary>
        /// Plays once the desired clip in the position
        /// </summary>
        /// <param name="audioClipName">Name of the clip</param>
        /// <param name="volume">Volume you want to reproduce it</param>
        /// <param name="position">Where do you want to instantiate de sound</param>
        public static void PlayOneShotSFX(string audioClipName, float volume, Vector3 position)
        {
            AudioClip audioClip = Resources.Load<AudioClip>($"{Statics.Resources.sfxPath}/{audioClipName}");
            AudioSource.PlayClipAtPoint(audioClip, position, volume);
        }

        /// <summary>
        /// Plays once the desired clip in the position
        /// </summary>
        /// <param name="audioClip">clip</param>
        /// <param name="volume">Volume you want to reproduce it</param>
        /// <param name="position">Where do you want to instantiate de sound</param>
        public static void PlayOneShotSFX(AudioClip audioClip, float volume, Vector3 position)
        {
            AudioSource.PlayClipAtPoint(audioClip, position, volume);
        }

        /// <summary>
        /// Gets an string array with the existing scenes inside the specified bundle name.
        /// </summary>
        /// <param name="bundleName">Bundle name where the scenes are located. It defaults to "scenes".</param>
        /// <returns>String Array of the found scenes inside the bundle. If Assetbundle results in Null, it will return a 1 element array with Default Scene.</returns>
        public static string[] GetScenesFromAssetBundle(string bundleName = "scenes")
        {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            string assetBundlePath = Path.Combine(Application.persistentDataPath + "/../../Inflight/", "AssetBundles/AssetBundles_Windows");
#elif UNITY_ANDROID
        string assetBundlePath = Path.Combine(Application.persistentDataPath + "/../../Inflight/", "AssetBundles/AssetBundles_Android");
#endif
            AssetBundle bundle = AssetBundle.LoadFromFile(Path.Combine(assetBundlePath, bundleName));

            if (bundle == null)
            {
                return new string[] { Statics.failsafeSceneName };
            }

            return bundle.GetAllScenePaths();
        }

        public static AssetBundle LoadAssetBundleFromPath(string path)
        {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            string assetBundlePath = Path.Combine(Application.persistentDataPath + "/../../Inflight/", "AssetBundles/AssetBundles_Windows");
#elif UNITY_ANDROID
        string assetBundlePath = Path.Combine(Application.persistentDataPath + "/../../Inflight/", "AssetBundles/AssetBundles_Android");
#endif
            assetBundlePath = Path.Combine(assetBundlePath, path);

            return AssetBundle.LoadFromFile(assetBundlePath);
        }

        /// <summary>
        /// Returns batterty level for the device in percentage (0% - 100%). Returns -1 if battery level not accesible for the device.
        /// </summary>
        public static float GetDeviceBattery()
        {
            float batteryLevel = SystemInfo.batteryLevel;

            if (batteryLevel == -1)
            {
                Debug.LogWarning("Battery Level not accesible for device.");
                return batteryLevel;
            }
            else
            {
                return batteryLevel * 100f;
            }
        }

        /// <summary>
        /// Method that helps retreiving if the headset is mounted or not. 
        /// </summary>
        /// <returns>True if the user has the headset on, false if the headset is removed.</returns>
        public static bool GetUserPresent()
        {
            InputDevice headDevice = InputDevices.GetDeviceAtXRNode(XRNode.Head);
            if (headDevice.isValid == false) return false;
            bool presenceFeatureSupported = headDevice.TryGetFeatureValue(CommonUsages.userPresence, out bool userPresent);

            if (presenceFeatureSupported)
                return userPresent;

            Debug.Log($"User presence not supported in this target device. Returning false");
            return userPresent;
        }

        /// <summary>
        /// Loads an image from disk and returns it as a Texture2D.
        /// </summary>
        /// <param name="filePath">Path to the file you want to load.</param>
        /// <returns>Returns the created Texture2D.</returns>
        public static Texture2D LoadToTexture2D(string filePath)
        {
            Texture2D tex = null;
            byte[] fileData;

            if (File.Exists(filePath))
            {
                fileData = File.ReadAllBytes(filePath);
                tex = new Texture2D(2, 2, TextureFormat.ARGB32, true);
                tex.LoadImage(fileData);
            }
            return tex;
        }

        /// <summary>
        /// Creates an sprite with the given texture.
        /// </summary>
        /// <param name="texture"></param>
        /// <returns>Returns the created Sprite</returns>
        public static Sprite Texture2DToSprite(Texture2D texture)
        {
            return Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
        }
    }
}
