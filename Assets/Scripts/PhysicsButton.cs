using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PhysicsButton : MonoBehaviour
{
    [SerializeField]
    private float threshold = .1f;

    [SerializeField]
    private float deadZone = .025f;

    private bool isPressed;
    private Vector3 startPosition;
    private ConfigurableJoint joint;

    public UnityEvent onPressed, onReleased;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.localPosition;

        joint = GetComponent<ConfigurableJoint>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPressed && GetValue() + threshold >= 1)
        {
            Pressed();
        }

        if (isPressed && GetValue() - threshold <= 1)
        {
            Released();
        }
    }

    private float GetValue()
    {
        float value = Vector3.Distance(startPosition, transform.localPosition) / joint.linearLimit.limit;

        if (Mathf.Abs(value) < deadZone)
        {
            value = 0;
        }

        return Mathf.Clamp(value, -1f, 1f);
    }

    private void Pressed()
    {
        isPressed = true;

        onPressed.Invoke();

        Debug.Log("Pressed");
    }

    private void Released()
    {
        isPressed = false;

        onReleased.Invoke();

        Debug.Log("Released");
    }
}
