using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using VRToolkit.Managers;

namespace VRToolkit.Utils
{
    public static class FaderFollower
    {
        private static GameObject followerRef;

        private static Coroutine fadeCoroutineRef;

        public static void ShowFadeFollower()
        {
            if (fadeCoroutineRef != null)
            {
                VRToolkitManager.Instance.StopCoroutine(fadeCoroutineRef);
                fadeCoroutineRef = null;
                UnityEngine.Object.Destroy(followerRef);
            }

            GameObject prefab = Resources.Load<GameObject>(Statics.Resources.faderFollower);
            followerRef = UnityEngine.Object.Instantiate(prefab);
            UnityEngine.Object.DontDestroyOnLoad(followerRef);

            fadeCoroutineRef = VRToolkitManager.Instance.StartCoroutine(Fade(followerRef.GetComponent<CanvasGroup>(), true));
        }

        public static void HideFadeFollower()
        {
            if (followerRef == null) return;

            if (fadeCoroutineRef != null)
            {
                VRToolkitManager.Instance.StopCoroutine(fadeCoroutineRef);
                fadeCoroutineRef = null;
            }

            fadeCoroutineRef = VRToolkitManager.Instance.StartCoroutine(Fade(followerRef.GetComponent<CanvasGroup>(), false, () => UnityEngine.Object.Destroy(followerRef) ));
        }

        private static IEnumerator Fade(CanvasGroup canvasGroup, bool ToOne, Action callback = null)
        {
            float duration = 1.0f;

            float startAlpha = canvasGroup.alpha;
            float endAlpha = ToOne ? 1f : 0f;

            for (float t = 0.0f; t < duration; t += Time.deltaTime)
            {
                canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, t / duration);
                yield return null;
            }

            canvasGroup.alpha = endAlpha;

            fadeCoroutineRef = null;

            callback?.Invoke();
        }
    }
}
