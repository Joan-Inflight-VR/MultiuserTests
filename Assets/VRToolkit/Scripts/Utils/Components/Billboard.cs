using UnityEngine;
using VRToolkit.Managers;
using VRToolkit.Utils;

public class Billboard : MonoBehaviour
{
    public Transform camTransform;

    public float distanceOffset;

    public bool playOnUpdate = false;

    public void LookAtPlayer()
    {
        transform.rotation = Quaternion.identity;
        if (camTransform == null && VRToolkitManager.Instance != null && VRToolkitManager.Instance.head != null)
            camTransform = VRToolkitManager.Instance.rigContainer.transform;
    
        Vector3 camOffset = camTransform.transform.position + VRToolkitManager.Instance.initialForward * distanceOffset;
        Vector3 target = new Vector3(camOffset.x, transform.position.y, camOffset.z);

        transform.LookAt(target);
        transform.Rotate(Vector3.up, 180, Space.Self);
    }

    private void Update()
    {
        if (!playOnUpdate) return;

        LookAtPlayer();
    }
}
