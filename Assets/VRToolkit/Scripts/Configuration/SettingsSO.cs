using UnityEngine;

namespace VRToolkit.Configuration
{
    public enum TargetDevice
    {
        PICO = 0,
        OCULUS
    }

    [CreateAssetMenu(fileName = "SettingsData", menuName = "VRToolkit/SettingsData", order = 1)]
    public class SettingsSO : ScriptableObject
    {
        public float splashscreenDuration;

        public bool middlewareEnabled;
        public bool wifiEnabled; // This requires middlewareEnabled to work. Even if it is set to true, if middlewareEnabled is set to false it wont work.
        public int numOfControllers;

        public TargetDevice targetDevice;

        public bool headHinterEnabled;

        public float softThresholdAngle;
        public float hardThresholdAngle;

        public float secondsUntilWarning;
    }
}