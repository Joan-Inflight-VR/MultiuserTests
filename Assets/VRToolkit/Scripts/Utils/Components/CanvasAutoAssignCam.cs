using UnityEngine;
using VRToolkit.Managers;

namespace VRToolkit.Utils
{
    public class CanvasAutoAssignCam : MonoBehaviour
    {
        private Canvas canvas;

        private void Awake()
        {
            canvas = GetComponent<Canvas>();

            canvas.worldCamera = VRToolkitManager.Instance.head;
        }
    }
}
