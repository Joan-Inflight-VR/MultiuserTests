using UnityEngine;
using VRToolkit.Managers;
using VRToolkit.Utils;

namespace VRToolkit.Localization
{
    /// <summary>
    /// In order to use properly this class you need to extend from it and use your target graphic for the text
    /// </summary>
    public abstract class UILocalizeBase : MonoBehaviour
    {
        public string localizationKey;
        public string lcid;

        private void OnEnable()
        {
            OnLanguageChange();
        }

        protected void SetUp()
        {
            EventManager.Instance.StartListening(Statics.Events.onLanguageChange, OnLanguageChange);
        }

        private void OnLanguageChange()
        {
            ChangeLocalization(LocalizationManager.Get(localizationKey, lcid));
        }

        public abstract void ChangeLocalization(string localizedText);
    }
}
