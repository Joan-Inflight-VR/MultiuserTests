using UnityEngine;
using VRToolkit.Managers;

namespace VRToolkit.Utils.Tooltips
{
    /// <summary>
    /// By including this to the desired tooltip you can call the Dissmiss directly from Unity Events at the inspector. 
    /// It is optional to attach this object since it will be added in runtime if not present. Only attach it if you need it.
    /// </summary>
    public class Tooltip : MonoBehaviour
    {
        public bool audioEnabled = true;

        public void OnEnable()
        {
            AudioClip audioClipAppearence = VRToolkitManager.Instance.audioSettings.notification;
            if (audioClipAppearence != null && audioEnabled) Utilities.PlayOneShotSFX(audioClipAppearence, 1f, Vector3.zero);
        }

        public void StartCountdown(float time)
        {
            Invoke("Dissmiss", time);
        }

        public void LookAtPlayer(Transform target)
        {
            Vector3 targetPostition = new Vector3(target.position.x,
                                        transform.position.y,
                                        target.position.z);
            transform.LookAt(targetPostition);
            transform.Rotate(0, 180, 0); //Compensate wrong rotation
        }

        public void Dissmiss()
        {
            Destroy(gameObject);
        }
    }
}
