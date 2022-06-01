using UnityEngine;

[CreateAssetMenu(fileName = "UIAudioData", menuName = "VRToolkit/UIAudioData", order = 1)]
public class UIAudioSO : ScriptableObject
{
    public AudioClip splashScreenSound;

    public AudioClip newWindowOpen;

    public AudioClip notification;

    [Header("Button Interactions")]

    public AudioClip itemHover;

    public AudioClip progressForward;
    public AudioClip progressBackward;
    public AudioClip progressNeutral;

    public AudioClip confirmForward;
    public AudioClip confirmBackward;
    public AudioClip confirmNeutral;
}
