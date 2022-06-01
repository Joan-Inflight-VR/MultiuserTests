using TMPro;
using UnityEngine;
using VRToolkit.Managers;
using VRToolkit.Utils;

namespace VRToolkit.VRKeyboard.Utils
{
    public class Key : MonoBehaviour
    {
        protected TextMeshProUGUI key;

        public delegate void OnKeyClickedHandler(string key);

        // The event which other objects can subscribe to
        // Uses the function defined above as its type
        public event OnKeyClickedHandler OnKeyClicked;

        public virtual void Awake()
        {
            if (transform.Find("Text (TMP)") == null)
            {
                Debug.Log(transform.name);
            }
            key = transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();

            GetComponent<FillerInteractionHandler>().filler.OnFillCompleted.AddListener(() =>
            {
                Utilities.PlayOneShotSFX(VRToolkitManager.Instance.audioSettings.confirmNeutral, 1f, Vector3.zero);
                OnKeyClicked(key.text);
            });
        }

        public virtual void CapsLock(bool isUppercase) { }
        public virtual void ShiftKey() { }
    };
}