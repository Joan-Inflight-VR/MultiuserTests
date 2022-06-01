using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public abstract class BaseInteractionHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public UnityEvent OnEnterEvent;
    public UnityEvent OnExitEvent;
    public UnityEvent OnClickEvent;

    public bool active = true;

    protected virtual void Awake()
    {

    }

    public abstract void OnEnter(PointerEventData eventData);
    public abstract void OnExit(PointerEventData eventData = null);
    public abstract void OnClick(PointerEventData eventData = null);

    public abstract void ForceClick();

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!active) return;

        OnEnterEvent.Invoke();
        OnEnter(eventData);
    }

    public void OnPointerExit(PointerEventData eventData = null)
    {
        if (!active) return;

        OnExitEvent.Invoke();
        OnExit(eventData);
    }

    public void OnPointerClick(PointerEventData eventData = null)
    {
        if (!active) return;

        OnClickEvent.Invoke();
        OnClick(eventData);
    }
}
