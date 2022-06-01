using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using VRToolkit.AnalyticsWrapper;
using VRToolkit.Managers;

namespace VRToolkit.Utils
{
    public class CamHeadHelper : MonoBehaviour
    {
        [SerializeField]
        private Camera head;

        [SerializeField]
        private GameObject headGazer;

        [SerializeField]
        private GameObject leftHand;
        [SerializeField]
        private GameObject rightHand;

        private void Awake()
        {
            EventManager.Instance.StartListening(Statics.Events.leftHandToggle, (x) => Toggle(leftHand, (bool)x));
            EventManager.Instance.StartListening(Statics.Events.rightHandToggle, (x) => Toggle(rightHand, (bool)x));
            EventManager.Instance.StartListening(Statics.Events.headGazeToggle, (x) => Toggle(headGazer, (bool)x));

            EventManager.Instance.StartListening(Statics.Events.headAllInteractionToggle, (x) => AllInteractionToggle((bool)x));

            EventManager.Instance.StartListening(Statics.Events.sceneReady, RepositionCamera);
        }

        private void Start()
        {
            EventManager.Instance.TriggerEvent(Statics.Events.camHelperReady, head);
        }

        private void OnDestroy()
        {
            EventManager.Instance?.StopListening(Statics.Events.leftHandToggle, (x) => Toggle(leftHand, (bool)x));
            EventManager.Instance?.StopListening(Statics.Events.rightHandToggle, (x) => Toggle(rightHand, (bool)x));
            EventManager.Instance?.StopListening(Statics.Events.headGazeToggle, (x) => Toggle(headGazer, (bool)x));
        }

        private void Toggle(GameObject go, bool enable)
        {
            if (go == null) return;

            go.SetActive(enable);
        }

        private void AllInteractionToggle(bool state)
        {
            leftHand.GetComponent<XRController>().enabled = state;
            rightHand.GetComponent<XRController>().enabled = state;
            headGazer.GetComponent<XRController>().enabled = state;

            leftHand.GetComponent<XRRayInteractor>().enabled = state;
            rightHand.GetComponent<XRRayInteractor>().enabled = state;
            headGazer.GetComponent<XRRayInteractor>().enabled = state;

            leftHand.GetComponent<XRInteractorLineVisual>().enabled = state;
            rightHand.GetComponent<XRInteractorLineVisual>().enabled = state;
            headGazer.GetComponent<XRInteractorLineVisual>().enabled = state;
        }
        
        private void RepositionCamera()
        {
            GameObject cameraSpawn = GameObject.Find(Statics.Tags.cameraSpawnName);
            transform.position = cameraSpawn.transform.position;
            transform.rotation *= cameraSpawn.transform.rotation;
            VRToolkitManager.Instance.rigContainer = gameObject;

            Transform rigTrans = VRToolkitManager.Instance.rigContainer.transform;

            AnalyticsObject details = new AnalyticsObject();
            details.AddData("world_position", $"({rigTrans.position.x},{rigTrans.position.y},{rigTrans.position.z})");
            details.AddData("rotation", $"({rigTrans.rotation.eulerAngles.x},{rigTrans.rotation.eulerAngles.y},{rigTrans.rotation.eulerAngles.z})");

            AnalyticsManager.RecordEvent(Statics.AnalyticsEvents.cameraLocation, details);

            EventManager.Instance.TriggerEvent(Statics.Events.camRepositioned);
        }
    }
}