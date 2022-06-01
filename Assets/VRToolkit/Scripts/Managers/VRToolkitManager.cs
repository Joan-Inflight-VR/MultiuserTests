using UnityEngine;
using VRToolkit.AnalyticsWrapper;
using VRToolkit.AnalyticsWrapper.Implementations;
using VRToolkit.Configuration;
using VRToolkit.Localization;
using VRToolkit.Middleware;
using VRToolkit.Surveys;
using VRToolkit.Utils;
using VRToolkit.Advertisement;
using VRToolkit.Advertisement.AdProvider.Implementations;

namespace VRToolkit.Managers
{
    public class VRToolkitManager : MonoBehaviour
    {
        public SettingsSO settings;

        public UIAudioSO audioSettings;

        [HideInInspector]
        public InputManager inputManager;

        [HideInInspector]
        public SurveyManager surveymanager;

        [HideInInspector]
        public Camera head;

        [HideInInspector]
        public GameObject rigContainer;

        [HideInInspector]
        public Vector3 initialForward;

        [HideInInspector]
        public WiFiHelper wifiHelper;

        [HideInInspector]
        public HeadHinter.HeadHinter headHinter;

        private static VRToolkitManager _instance;

        public static VRToolkitManager Instance { get { return _instance; } }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
            }

            SetUp();
        }

        private void SetUp()
        {
            if (settings == null)
            {
                settings = Settings.ReadSettings();
            }

            LocalizationManager.LoadLocalizations();

            UIAudio.SetUp(audioSettings);

            EventManager.Instance.StartListening(Statics.Events.camHelperReady, OnCamHelperReady);

            if (settings.middlewareEnabled)
            {
                LoadMiddleware();
            }
            else
            {
                // if a new analytics engine is implemented, here must be the setup for the new instance.
            }

            inputManager = gameObject.AddComponent<InputManager>();
            ImageStorage.SetUp();

            surveymanager = gameObject.AddComponent<SurveyManager>();

            if (settings.headHinterEnabled)
            {
                GameObject hinterPrefab = Resources.Load<GameObject>(Statics.Resources.hinterPrefab);
                GameObject hinter = Instantiate(hinterPrefab);

                headHinter = hinter.GetComponent<HeadHinter.HeadHinter>();
                headHinter.SetUp();
            }

            AdManager.SetUpProvider(new LocalAdProvider());
        }

        private void LoadMiddleware()
        {
            GameObject middlewareConnectorPrefab = Resources.Load<GameObject>(Statics.Resources.middlewareConnectorObject);
            GameObject instancedConnector = Instantiate(middlewareConnectorPrefab);
            instancedConnector.name = middlewareConnectorPrefab.name; //Put Original name so we avoid missmatch with middleware.

            if (settings.wifiEnabled)
            {
                wifiHelper = instancedConnector.AddComponent<WiFiHelper>();
            }
            
            AnalyticsManager.SetUp(new AnalyticsMiddleware());
        }

        private void OnCamHelperReady(object value)
        {
            head = (Camera)value;
            initialForward = head.transform.forward;

#if UNITY_EDITOR
            head.transform.parent.gameObject.AddComponent<MoveCamera>();
#endif
        }
    }
}
