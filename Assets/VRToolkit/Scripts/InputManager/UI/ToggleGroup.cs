using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRToolkit.InputManager.UI
{
    public class ToggleGroup : MonoBehaviour
    {
        public Toggle currentSelected = null;

        public List<Toggle> toggles = new List<Toggle>();

        public void UpdateToggles()
        {
            for (int i = 0; i < toggles.Count; ++i)
            {
                toggles[i].UpdateState();
            }
        }

        public void RegisterToggle(Toggle tg)
        {
            toggles.Add(tg);
            tg.group = this;
        }

        public void CleanToggles()
        {
            toggles.Clear();
        }
    }
}
