using UnityEngine;
using UnityEngine.EventSystems;

public class FillerInteractionHandler : BaseInteractionHandler
{
    public Filler filler;

    public bool sideReactive;

    public UIAudio.ProgressSFX onEnterSFX;

    private AudioSource audioSource;

    private float angleCorner;
    private RectTransform rectTransform;

    protected override void Awake()
    {
        base.Awake();

        rectTransform = GetComponent<RectTransform>();
        angleCorner = (rectTransform.sizeDelta.y / 2) / (rectTransform.sizeDelta.x / 2) * Mathf.Rad2Deg;
    }

    public override void OnEnter(PointerEventData eventData)
    {
        AudioClip onEnterClip = UIAudio.GetProgressSFX(onEnterSFX);

        if (onEnterClip != null)
        {
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.playOnAwake = false;
                audioSource.Stop();
            }
            else
            {
                audioSource.GetComponent<AudioSource>();
            }
            audioSource.volume = 1f;
            audioSource.clip = onEnterClip;
            audioSource.Play();

            Invoke(nameof(StopAndDestroy), onEnterClip.length);
        }

        if (sideReactive)
        {
            Vector3 currentEnterPos = eventData.pointerCurrentRaycast.worldPosition;
            Vector3 angleVector = currentEnterPos - rectTransform.position;

            float angle = Vector3.Angle(angleVector, Vector3.right);

            if (angle <= angleCorner)
            {
                filler.StartFiller(1); //right
            }
            else if (angle >= 180 - angleCorner)
            {
                filler.StartFiller(3); //left
            }
            else if (currentEnterPos.y > rectTransform.position.y)
            {
                filler.StartFiller(2); //top
            }
            else
            {
                filler.StartFiller(0); //bottom
            }
        }
        else
        {
            filler.StartFiller();
        }
    }

    public override void OnExit(PointerEventData eventData = null)
    {
        StopAndDestroy();
        filler.StopFiller();
    }

    public override void OnClick(PointerEventData eventData = null)
    {
        StopAndDestroy();
        filler.ForceComplete();
    }

    public override void ForceClick()
    {
        if (!active) return;
        filler.ForceComplete();
    }

    public void ForceFill()
    {
        filler.ForceFill();
    }

    private void StopAndDestroy()
    {
        CancelInvoke(nameof(StopAndDestroy));
        if (audioSource != null)
        {
            audioSource.Stop();
            Destroy(audioSource);
            audioSource = null;
        }
    }
}
