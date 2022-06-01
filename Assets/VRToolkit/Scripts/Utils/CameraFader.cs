using System;
using UnityEngine;

namespace VRToolkit.Utils
{
    public static class CameraFader
    {
        private static Fader faderRef;

        /// <summary>
        /// Use it to fade to transparent (exit to the last fade to black)
        /// </summary>
        /// <param name="duration">Amount of seconds you want the transition to last</param>
        /// <param name="callback">If you want to call an action just after the fade finishes</param>
        public static void FadeIn(float duration = 1f, Action callback = null)
        {
            Fader fader = GetFader();
            fader.SetUp(true, duration, callback);
        }

        /// <summary>
        /// Use it to fade to black your scene
        /// </summary>
        /// <param name="duration">Amount of seconds you want the transition to last</param>
        /// <param name="callback">If you want to call an action just after the fade finishes</param>
        public static void FadeOut(float duration = 1f, Action callback = null)
        {
            Fader fader = GetFader();
            fader.SetUp(false, duration, callback);
        }

        private static Fader GetFader()
        {
            if (faderRef == null)
            {
                Fader fadePrefab = Resources.Load<Fader>(Statics.Resources.cameraFade);
                faderRef = UnityEngine.Object.Instantiate(fadePrefab);
            }
            return faderRef;
        }
    }
}
