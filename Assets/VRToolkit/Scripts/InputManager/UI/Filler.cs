using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VRToolkit.Utils;

public class Filler : MonoBehaviour
{
    public Image[] fillers;

    public float duration = 2;

    public UIAudio.ConfirmationSFX onCompleteSFX;

    public UnityEvent OnFillCompleted;

    public bool remainFilledAfterExecute = false;

    private Coroutine fill;

    private void ResetFiller()
    {
        if (fillers.Length == 0) return;

        foreach (Image img in fillers)
        {
            if (img != null)
                img.fillAmount = 0;
        }

        fill = null;
    }

    public void StartFiller(int side = -1)
    {
        if (fillers.Length == 0) return;

        ResetFiller();

        if (side != -1)
        {
            foreach (Image img in fillers)
            {
                img.fillOrigin = side;
            }
        }

        fill = StartCoroutine(Fill());
    }

    public void StopFiller()
    {
        if (fillers.Length == 0) return;

        if (fill != null)
        {
            StopCoroutine(fill);
            fill = null;
        }

        ResetFiller();
    }

    public void ForceComplete()
    {
        AudioClip onCompleteAudioClip = UIAudio.GetConfirmationSFX(onCompleteSFX);
        if (onCompleteAudioClip != null)
        {
            Utilities.PlayOneShotSFX(onCompleteAudioClip, 1f, Vector3.zero);
        }

        ForceFill();
        OnFillCompleted.Invoke();

        if (!remainFilledAfterExecute)
        {
            StopFiller();
        }
    }

    private IEnumerator Fill()
    {
        foreach (Image img in fillers)
        {
            img.fillAmount = 0;
        }

        while (fillers[0].fillAmount < 1f)
        {
            foreach (Image img in fillers)
            {
                img.fillAmount += Time.deltaTime / duration;
            }
            yield return null;
        }

        foreach (Image img in fillers)
        {
            img.fillAmount = 1;
        }

        fill = null;

        AudioClip onCompleteAudioClip = UIAudio.GetConfirmationSFX(onCompleteSFX);

        if (onCompleteAudioClip != null)
        {
            Utilities.PlayOneShotSFX(onCompleteAudioClip, 1f, Vector3.zero);
        }

        if (!remainFilledAfterExecute)
        {
            StopFiller();
        }

        OnFillCompleted.Invoke();
    }

    public void ForceFill()
    {
        StopFiller();

        foreach (Image img in fillers)
        {
            img.fillAmount = 1;
        }
    }
}
