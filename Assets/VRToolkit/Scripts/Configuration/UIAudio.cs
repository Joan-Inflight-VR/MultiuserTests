using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UIAudio
{
    public enum ProgressSFX
    {
        NONE,
        NEUTRAL,
        FORWARD,
        BACKWARD
    }

    public enum ConfirmationSFX
    {
        NONE,
        NEUTRAL,
        FORWARD,
        BACKWARD
    }

    static UIAudioSO audioSettings;

    public static void SetUp(UIAudioSO audioSettingsSO)
    {
        audioSettings = audioSettingsSO;
    }

    public static AudioClip GetProgressSFX(ProgressSFX progress)
    {
        switch (progress)
        {
            case ProgressSFX.NEUTRAL:
                return audioSettings.progressNeutral;
            case ProgressSFX.FORWARD:
                return audioSettings.progressForward;
            case ProgressSFX.BACKWARD:
                return audioSettings.progressBackward;
            default:
                return null;
        }
    }

    public static AudioClip GetConfirmationSFX(ConfirmationSFX progress)
    {
        switch (progress)
        {
            case ConfirmationSFX.NEUTRAL:
                return audioSettings.confirmNeutral;
            case ConfirmationSFX.FORWARD:
                return audioSettings.confirmForward;
            case ConfirmationSFX.BACKWARD:
                return audioSettings.confirmBackward;
            default:
                return null;
        }
    }
}
