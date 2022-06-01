using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using VRToolkit.Managers;

namespace VRToolkit.Utils
{
    public class Splashscreen : MonoBehaviour
    {
        public float duration = 5f;

        public Image logo;

        public CanvasGroup canvasGroup;

        public float fadeInDuration = 1.5f;

        private void Start()
        {
            logo.preserveAspect = true;

            EventManager.Instance.TriggerEvent(Statics.Events.headAllInteractionToggle, false);

            float settingsDuration = VRToolkitManager.Instance.settings.splashscreenDuration;
            float duration = settingsDuration == 0 ? 1f : settingsDuration;

            StartCoroutine(Fade(0f, 1f, fadeInDuration, () => Invoke(nameof(Finish), duration)));

            Utilities.PlayOneShotSFX(VRToolkitManager.Instance.audioSettings.splashScreenSound, 1f, Vector3.zero);

            try
            {
                Texture2D texture = Utilities.LoadToTexture2D(Statics.splashscreen);
                logo.sprite = Utilities.Texture2DToSprite(texture);
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Cannot load the specified splashscreen with error: {e.Message}");
                Debug.LogWarning($"Loading the default one from build memory");

                logo.sprite = Resources.Load<Sprite>(Statics.Resources.defaultSplashscreen);
            }
        }

        private void Finish()
        {
            EventManager.Instance.TriggerEvent(Statics.Events.splashscreenDone);
        }

        private IEnumerator Fade(float start, float end, float time, Action callback)
        {
            for (float t = 0.0f; t < time; t += Time.deltaTime)
            {
                canvasGroup.alpha = Mathf.Lerp(start, end, t / time);
                yield return null;
            }

            canvasGroup.alpha = end;

            callback?.Invoke();
        }
    }
}
