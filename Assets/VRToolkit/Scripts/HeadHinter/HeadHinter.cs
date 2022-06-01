using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VRToolkit.Managers;
using VRToolkit.Utils;

namespace VRToolkit.HeadHinter
{
    public class HeadHinter : MonoBehaviour
    {
        public Material arrowMaterial;

        public UnityEvent onRotationWarning;
        public UnityEvent onRotationRecover;

        private Vector3 initialForward;
        private Transform camTransform;

        private float softThreshold = 45f;
        private float hardThreshold = 90f;

        private float secondsUntilWarning;

        private Coroutine checkThresholdsCoroutine;
        private Coroutine delayedWarningCoroutine;

        private Coroutine fadingArrows;
        private float fadeDuration = 0.3f;

        private int alphaProperty;
        private bool arrowsVisible;
        private bool fading;

        public void Start()
        {
            DontDestroyOnLoad(gameObject);

            EventManager.Instance.StartListening(Statics.Events.camRepositioned, SetUp);
            EventManager.Instance.StartListening(Statics.Events.stopCheckHinter, StopAllCoroutines);
            EventManager.Instance.StartListening(Statics.Events.camRepositioned, UpdatePositionAndRotation);
        }

        public void SetUp()
        {
            StopAllCoroutines();

            arrowsVisible = false;
            fading = false;

            alphaProperty = Shader.PropertyToID("_Alpha");

            arrowMaterial.SetFloat(alphaProperty, 0f);

            checkThresholdsCoroutine = null;

            softThreshold = VRToolkitManager.Instance.settings.softThresholdAngle;
            hardThreshold = VRToolkitManager.Instance.settings.hardThresholdAngle;

            secondsUntilWarning = VRToolkitManager.Instance.settings.secondsUntilWarning;

            checkThresholdsCoroutine = StartCoroutine(CheckThresholds());
        }

        private void UpdatePositionAndRotation()
        {
            initialForward = VRToolkitManager.Instance.initialForward;
            camTransform = VRToolkitManager.Instance.head.transform;

            transform.position = VRToolkitManager.Instance.rigContainer.transform.position;
            transform.rotation = VRToolkitManager.Instance.rigContainer.transform.rotation;
        }

        private IEnumerator CheckThresholds()
        {
            while (true)
            {
                if (CheckThreshold(hardThreshold))
                {
                    CancelCoroutine(ref delayedWarningCoroutine);
                    ToggleArrows(true);
                }
                else if (CheckThreshold(softThreshold))
                {
                    if (!arrowsVisible && !fading)
                    {
                        delayedWarningCoroutine = StartCoroutine(DelayedWarning());
                    }
                }
                else
                {
                    CancelCoroutine(ref delayedWarningCoroutine);
                    CancelCoroutine(ref fadingArrows);
                    ToggleArrows(false);
                }

                yield return new WaitForSeconds(0.5f);
            }
        }

        private void CancelCoroutine(ref Coroutine co)
        {
            if (co != null)
            {
                StopCoroutine(co);
                co = null;
            }
        }

        private bool CheckThreshold(float threshold)
        {
            if (camTransform == null) return false;

            Vector3 currentForward = camTransform.forward;
            currentForward.y = 0; // remove the Y axis of the equation, we only want to check angles at y = 0

            return Mathf.Abs(Vector3.Angle(currentForward, initialForward)) >= threshold;
        }

        private void ToggleArrows(bool state)
        {
            foreach (Transform tr in transform)
            {
                tr.gameObject.SetActive(state ? true : tr.gameObject.activeInHierarchy);
            }

            if (state != arrowsVisible)
            {
                fadingArrows = StartCoroutine(ToggleWithFade(state));
            }
        }

        private IEnumerator DelayedWarning()
        {
            yield return new WaitForSeconds(secondsUntilWarning);

            ToggleArrows(true);
        }

        private IEnumerator ToggleWithFade(bool state)
        {
            fading = true;

            float startAlpha = arrowMaterial.GetFloat(alphaProperty);
            float endAlpha = state ? 1f : 0f;

            for (float t = 0.0f; t < fadeDuration; t += Time.deltaTime)
            {
                arrowMaterial.SetFloat(alphaProperty, Mathf.Lerp(startAlpha, endAlpha, t / fadeDuration));
                yield return null;
            }

            fading = false;

            arrowMaterial.SetFloat(alphaProperty, endAlpha);

            foreach (Transform tr in transform)
            {
                tr.gameObject.SetActive(state);
            }

            arrowsVisible = state;
        }
    }
}