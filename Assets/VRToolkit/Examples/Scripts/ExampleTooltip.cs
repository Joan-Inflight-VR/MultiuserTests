using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRToolkit.Managers;
using VRToolkit.Utils.Tooltips;

public class ExampleTooltip : MonoBehaviour
{
    public GameObject tooltipPrefabTimed;
    public GameObject tooltipPrefabDismissable;

    public Transform target;

    public TooltipType tType;

    private void Start()
    {
        GameObject tooltip;

        switch (tType)
        {
            default:
            case TooltipType.DISSMISSIBLE:
                tooltip = tooltipPrefabDismissable;
                break;
            case TooltipType.TIMED:
                tooltip = tooltipPrefabTimed;
                break;
        }

        TooltipCreator.CreateTooltipAtTransform(tooltip, tType, target, VRToolkitManager.Instance.rigContainer.transform);
    }
}
