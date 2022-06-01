using System;
using UnityEngine;

namespace VRToolkit.InputManager.UI
{
    public class Toggle : MonoBehaviour
    {
        public ToggleGroup group;

        private BaseInteractionHandler interactor;

        public void SetUp(BaseInteractionHandler interactor)
        {
            this.interactor = interactor;
        }

        public void UpdateState()
        {
            if (!interactor.active && this != group.currentSelected)
            {
                interactor.active = true;
                interactor.OnExit();
            }
        }
    }
}
