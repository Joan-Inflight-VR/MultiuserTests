using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AppLauncher
{
    public struct ExtraData
    {
        public string key;
        public string value;

        public ExtraData(string key, string value)
        {
            this.key = key;
            this.value = value;
        }
    }

    public static void LaunchApp(string appBundleId, ExtraData[] extras = null)
    {
        if (extras != null)
        {
            for (int i = 0; i < extras.Length; ++i)
            {
                ExtraData extra = extras[i];
                Debug.Log("[Launcher] extra[" + extra.key + "]: " + extra.value);
            }
        }

        using AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        using AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        using AndroidJavaObject packageManager = currentActivity.Call<AndroidJavaObject>("getPackageManager");

        try
        {
            using AndroidJavaObject launchIntent = packageManager.Call<AndroidJavaObject>("getLaunchIntentForPackage", appBundleId);
            if (extras != null)
            {
                for (int i = 0; i < extras.Length; ++i)
                {
                    ExtraData extra = extras[i];
                    launchIntent.Call<AndroidJavaObject>("putExtra", extra.key, extra.value);
                }
            }
            currentActivity.Call("startActivity", launchIntent);
        }
        catch (Exception e)
        {
            Debug.LogError("Error Launching App: " + e);
        }
    }
}
