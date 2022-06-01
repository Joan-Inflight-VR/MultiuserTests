using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using VRToolkit.Managers;

namespace VRToolkit.Utils
{
    public class Fader : MonoBehaviour
    {
        public Image fadeImage;
        private float duration = 1f;
        private bool fadeIn;

        public Color transparentColor;
        public Color fadedColor;

        private Action callback;

        /// <summary>
        /// Set up the fader to do a fade in or fade out depending on the parameters with de desired duration
        /// </summary>
        /// <param name="fadeIn">True means Fade In, False means Fade Out</param>
        /// <param name="duration">Duration of the fade</param>
        public void SetUp(bool fadeIn, float duration = 1f, Action callback = null)
        {
            this.fadeIn = fadeIn;
            this.duration = duration;
            this.callback = callback;

            fadeImage.color = fadeIn ? fadedColor : transparentColor;
            fadeImage.gameObject.SetActive(true);

            DontDestroyOnLoad(gameObject);

            StartCoroutine(Fade());
        }

        private IEnumerator Fade()
        {
            EventManager.Instance.TriggerEvent(Statics.Events.headAllInteractionToggle, false);

            Color startColor = fadeIn ? fadedColor : transparentColor;
            Color endColor = fadeIn ? transparentColor : fadedColor;

            for (float t = 0.0f; t < duration; t += Time.deltaTime)
            {
                fadeImage.color = Color.Lerp(startColor, endColor, t / duration);
                yield return null;
            }

            fadeImage.color = endColor;

            callback?.Invoke();

            if (fadeIn)
            {
                EventManager.Instance.TriggerEvent(Statics.Events.headAllInteractionToggle, true);
                Destroy(gameObject);
            }
        }
    }
}
