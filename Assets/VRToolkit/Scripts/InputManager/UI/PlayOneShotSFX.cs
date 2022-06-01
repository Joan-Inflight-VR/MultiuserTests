using UnityEngine;
using VRToolkit.Utils;

public class PlayOneShotSFX : MonoBehaviour
{
    public void PlayOneShotAudio(AudioClip audio)
    {
        Utilities.PlayOneShotSFX(audio, 1f, Vector3.zero);
    }
}
