using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using VRToolkit.Localization;
using VRToolkit.Utils;

public class NetworkItem : MonoBehaviour
{
    public TextMeshProUGUI ssid;
    public TextMeshProUGUI security;
    public FillerInteractionHandler button;

    public void SetUp(string ssid, bool secure, Action buttonAction)
    {
        this.ssid.text = ssid;
        string localizationKey = secure ? Statics.Localization.wifiSecureNetwork : Statics.Localization.wifiOpenNetwork;
        security.text = LocalizationManager.Get(localizationKey);

        button.filler.OnFillCompleted.RemoveAllListeners();
        button.filler.OnFillCompleted.AddListener(() => buttonAction());
    }
}
