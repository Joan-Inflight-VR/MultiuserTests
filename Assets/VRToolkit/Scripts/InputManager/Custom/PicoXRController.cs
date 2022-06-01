using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PicoXRController : XRController
{
    protected override void UpdateInput(XRControllerState controllerState)
    {
        bool pressed = Input.GetMouseButton(0) || Input.GetKey(KeyCode.JoystickButton0) || Input.GetKey(KeyCode.Escape);

        if (pressed)
        {
            if (!controllerState.uiPressInteractionState.active)
            {
                controllerState.uiPressInteractionState.activatedThisFrame = true;
                controllerState.uiPressInteractionState.active = true;
            }
        }
        else
        {
            if (controllerState.uiPressInteractionState.active)
            {
                controllerState.uiPressInteractionState.deactivatedThisFrame = true;
                controllerState.uiPressInteractionState.active = false;
            }
        }
    }
}
