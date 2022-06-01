using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using VRToolkit.Utils;

namespace VRToolkit.Managers
{
    public class InputManager : MonoBehaviour
    {
        private int numOfControllers;
        private int controllersDetected;

        private XRNode xRNode = XRNode.LeftHand;
        private InputDevice device;

        private List<InputDevice> devices;

        private void Awake()
        {
            numOfControllers = VRToolkitManager.Instance.settings.numOfControllers;
        }

        private IEnumerator Start()
        {
            devices = new List<InputDevice>();

            yield return new WaitForEndOfFrame();

            EventManager.Instance.StartListening(Statics.Events.camRepositioned, OnRigReady);
        }

        private void OnRigReady()
        {
            CheckForGaze();
        }

        private void OnEnable()
        {
            if (numOfControllers > 0)
            {
                InputDevices.deviceConnected += OnDeviceConnected;
                InputDevices.deviceDisconnected += OnDeviceDisconnected;
            }
        }

        private void OnDisable()
        {
            if (numOfControllers > 0)
            {
                InputDevices.deviceConnected -= OnDeviceConnected;
                InputDevices.deviceDisconnected -= OnDeviceDisconnected;
            }
        }

        private void OnApplicationPause(bool pause)
        {
            if (!pause)
            {
                CheckForGaze();
            }
        }

        private void CheckForGaze()
        {
            List<InputDevice> allDevices = new List<InputDevice>();
            InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeldInHand, allDevices);

            Debug.Log($"Total connected devices: {allDevices.Count}");

            EventManager.Instance.TriggerEvent(Statics.Events.headGazeToggle, allDevices.Count == 0);
        }

        private void OnDeviceConnected(InputDevice device)
        {
            Debug.Log($"Device connected and valid: {device.isValid}, {device.characteristics}");

            if (device.characteristics.HasFlag(InputDeviceCharacteristics.HeldInHand))
            {
                EventManager.Instance.TriggerEvent(Statics.Events.headGazeToggle, false);

                controllersDetected++;

                if (controllersDetected <= numOfControllers)
                {
                    if (device.characteristics.HasFlag(InputDeviceCharacteristics.Left))
                    {
                        EventManager.Instance.TriggerEvent(Statics.Events.leftHandToggle, true);
                    }

                    if (device.characteristics.HasFlag(InputDeviceCharacteristics.Right))
                    {
                        EventManager.Instance.TriggerEvent(Statics.Events.rightHandToggle, true);
                    }
                }
            }
        }

        private void OnDeviceDisconnected(InputDevice device)
        {
            Debug.Log($"Device Disconnected and valid: {device.isValid}, {device.characteristics}");

            if (device.characteristics.HasFlag(InputDeviceCharacteristics.Left))
            {
                EventManager.Instance.TriggerEvent(Statics.Events.leftHandToggle, false);
                controllersDetected--;
            }

            if (device.characteristics.HasFlag(InputDeviceCharacteristics.Right))
            {
                EventManager.Instance.TriggerEvent(Statics.Events.rightHandToggle, false);
                controllersDetected--;
            }

            if (controllersDetected == 0)
            {
                EventManager.Instance.TriggerEvent(Statics.Events.headGazeToggle, true);
            }
        }

        private void GetDevice()
        {
            InputDevices.GetDevicesAtXRNode(xRNode, devices);

            if (devices.Count > 0)
            {
                device = devices[0];
            }
            else
            {
                device = new InputDevice();
            }
        }

        private void Update()
        {
            if (!device.isValid)
            {
                GetDevice();
            }

            device.TryGetFeatureValue(CommonUsages.triggerButton, out bool triggerButtonValue);

            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.Escape) || triggerButtonValue)
            {
                EventManager.Instance.TriggerEvent(Statics.Events.InputManager.triggerPressed);
            }
        }
    }
}
