using UnityEngine;

namespace VRToolkit.Utils.Tooltips
{
    public enum TooltipType
    {
        DISSMISSIBLE,
        TIMED
    }

    public static class TooltipCreator
    {
        /// <summary>
        /// Creates a tooltip at the target Transform.
        /// </summary>
        /// <param name="tooltipPrefab">The Object you want to spawn.</param>
        /// <param name="type">Type of the tooltip, can be TIMED or DISSMISSIBLE.</param>
        /// <param name="target">Target where the prefab will spawn.</param>
        /// <param name="lookAtPlayer">Optional param if you want the tooltip to look at the player's direction. This won't update in realtime.</param>
        /// <param name="time">Optional param only used by TIMED type</param>
        /// <returns>It will return a reference of the tooltip in case you want to destroy it yourself.</returns>
        public static GameObject CreateTooltipAtTransform(GameObject tooltipPrefab, TooltipType type, Transform target, Transform lookAtPlayer = null, float time = 5f, bool isChild = false, Vector3 offset = new Vector3())
        {
            GameObject tooltipInstance;
            if (isChild)
            {
                tooltipInstance = Object.Instantiate(tooltipPrefab, target, false);
            }
            else
            {
                tooltipInstance = Object.Instantiate(tooltipPrefab);
                tooltipInstance.transform.position = target.position + offset;
            }

            Tooltip tooltip = tooltipInstance.GetComponent<Tooltip>();

            if (tooltip == null)
            {
                tooltip = tooltipInstance.AddComponent<Tooltip>();
            }

            if (type == TooltipType.TIMED)
            {
                tooltip.StartCountdown(time);
            }

            if (lookAtPlayer != null)
            {
                tooltip.LookAtPlayer(lookAtPlayer);
            }

            return tooltipInstance;
        }
    }
}