using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit.UI;

public class MoveCamera : MonoBehaviour
{
    public bool enableYaw = true;
    public bool autoRecenterPitch = true;

    private float mouseX;
    private float mouseY;
    private float mouseZ;

    void Update()
    {
        bool pitched = false;
        if (Input.GetKey(KeyCode.LeftControl))
        {
            XRUIInputModule eventSystem = FindObjectOfType<XRUIInputModule>();
            eventSystem.enableMouseInput = false;
            Cursor.lockState = CursorLockMode.Locked;

            pitched = true;
            if (enableYaw)
            {
                mouseX += Input.GetAxis("Mouse X") * 5;
                if (mouseX <= -180)
                {
                    mouseX += 360;
                }
                else if (mouseX > 180)
                {
                    mouseX -= 360;
                }
            }
            mouseY -= Input.GetAxis("Mouse Y") * 2.4f;
            mouseY = Mathf.Clamp(mouseY, -85, 85);
        }

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            XRUIInputModule eventSystem = FindObjectOfType<XRUIInputModule>();
            eventSystem.enableMouseInput = true;
            Cursor.lockState = CursorLockMode.None;
        }

        if (!pitched && autoRecenterPitch)
        {
            mouseY = Mathf.Lerp(mouseY, 0, Time.deltaTime / (Time.deltaTime + 0.1f));
        }

        transform.localRotation = Quaternion.Euler(mouseY, mouseX, mouseZ);
    }
}
