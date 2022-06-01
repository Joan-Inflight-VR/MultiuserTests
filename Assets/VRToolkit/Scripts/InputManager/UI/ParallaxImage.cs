using UnityEngine;
using VRToolkit.Managers;

public class ParallaxImage : MonoBehaviour
{
    public float moveModifier;

    public bool debugMouse = false;

    private Vector3 startPos;
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        startPos = rectTransform.localPosition;
    }

    private void OnDisable()
    {
        rectTransform.localPosition = startPos;
    }

    void Update()
    {
        Vector3 parallax = Vector3.zero;

        if (debugMouse)
        {
            parallax = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        }
        else
        {
            if (VRToolkitManager.Instance != null && VRToolkitManager.Instance.head != null)
            {
                Ray ray = VRToolkitManager.Instance.head.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
                parallax = ray.direction;
            }
        }

        rectTransform.localPosition = new Vector3(startPos.x + (parallax.x * moveModifier), startPos.y + (parallax.y * moveModifier), 0);
    }
}
