using UnityEngine;
using UnityEngine.Events;
using VRToolkit.Managers;

namespace VRToolkit.Utils
{
    /// <summary>
    /// Attach this component to an object if you want that object to follow the camera view
    /// </summary>
    public class FollowHead : MonoBehaviour
    {
        public float distance = 0.32f;

        private Transform head = null;

        private UnityAction attachCam;

        private void Start()
        {
            head = VRToolkitManager.Instance.head.transform;

            attachCam = () => head = VRToolkitManager.Instance.head.transform;

            EventManager.Instance.StartListening(Statics.Events.camRepositioned, attachCam);
        }

        void LateUpdate()
        {
            if (head != null)
            {
                gameObject.transform.position = head.position + head.forward * distance;
                gameObject.transform.rotation = head.rotation;
            }
        }

        private void OnDestroy()
        {
            EventManager.Instance.StopListening(Statics.Events.camRepositioned, attachCam);
        }
    }
}
