
using UnityEngine;

namespace VRToolkit.Utils
{
    public static class Statics
    {
        public readonly static string folderPath = "/Inflight";
        public readonly static string inflightPath = Application.persistentDataPath + "/../.." + folderPath;
        public readonly static string elementsPath = inflightPath + "/elements.json";
        public readonly static string localizationPath = inflightPath + "/Localization/";
        public readonly static string settingsPath = inflightPath + "/VRTSettings.json";
        public readonly static string surveysPath = inflightPath + "/Surveys/surveydata.json";
        public readonly static string defaultSettings = "Data/Default";
        public readonly static string splashscreen = inflightPath + "/Splashscreen.png";
        public readonly static string vrPlatformSettings = inflightPath + "/VRPlatformSettings.json";
        public readonly static string environmentSettings = inflightPath + "/ScenesSettings.json";
        public readonly static string defaultVRPlatformSettings = "Data/PlatformSettingsData";
        public readonly static string defaultEnvironmentSettings = "Data/ScenesSettingsData";

        public readonly static string failsafeSceneName = "IFVR_Recaro_Economy_3x3";

        public struct AdManager
        {
            public readonly static string videos2DRoot = inflightPath + "/Ads/Videos2D/";
            public readonly static string videos360Root = inflightPath + "/Ads/Videos360/";
        }

        public struct Events
        {
            public static readonly string onTick = "onTick";
            public static readonly string askForData = "askForData";
            public static readonly string dataReady = "dataReady";
            public static readonly string loadMainUI = "loadMainUI";
            public static readonly string uiReady = "uiReady";
            public static readonly string sceneReady = "sceneReady";
            public static readonly string onLanguageChange = "onLanguageChange";
            public static readonly string leftHandToggle = "leftHandToggle";
            public static readonly string rightHandToggle = "rightHandToggle";
            public static readonly string headGazeToggle = "headGazeToggle";
            public static readonly string camHelperReady = "camHelperReady";
            public static readonly string splashscreenDone = "splashscreenDone";
            public static readonly string toggleInteraction = "toggleInteraction";
            public static readonly string camRepositioned = "camRepositioned";
            public static readonly string onNetworksReceived = "onNetworksReceived";
            public static readonly string homeButtonChangeAction = "homeButtonChangeAction";
            public static readonly string homeButtonOneTimeAction = "homeButtonOneTimeAction";
            public static readonly string homeButtonChangeIcon = "homeButtonChangeIcon";
            public static readonly string homeButtonToggleVisibility = "homeButtoToggleActive";
            public static readonly string homeButtonToggleInteraction = "homeButtoToggleInteraction";
            public static readonly string homeButtonCleanAction = "homeButtoCleanAction";
            public static readonly string homeButtonChangeSorting = "homeButtonChangeSorting";
            public static readonly string homeButtonRecoverPrevState = "homeButtonRecoverPrevState"; 
            public static readonly string homeButtonSetUp = "homeButtonSetUp";
            public static readonly string stopCheckHinter = "stopCheckHinter";
            public static readonly string showDetailView = "showDetailView";
            public static readonly string hideDetailView = "hideDetailView";
            public static readonly string launchApp = "launchApp";
            public static readonly string headAllInteractionToggle = "headAllInteractionToggle";

            public struct InputManager
            {
                public static readonly string triggerPressed = "triggerPressed";
            }

            public struct MediaPlayer
            {
                public static readonly string playReady = "playReady";
                public static readonly string playPressed = "playPressed";
                public static readonly string seek = "seek";
                public static readonly string seekPreview = "seekPreview";
                public static readonly string startVideo = "startVideo";
                public static readonly string quitAndSave = "quitAndSave";

                public static readonly string showVideoPlayerUI = "showVideoPlayerUI";
                public static readonly string hideVideoPlayerUI = "hideVideoPlayerUI";

                public static readonly string languageChangeAudio = "languageChangeAudio";
                public static readonly string languageChangeSubtitle = "languageChangeSubtitle";

                public static readonly string setUpProgressBar = "setUpProgressBar";
                public static readonly string setUpPlayButton = "setUpPlayButton";
                public static readonly string progressBarReady = "progressBarReady";
                public static readonly string timeDisplayReady = "timeDisplayReady";
                public static readonly string mediaPlayerStarted = "mediaPlayerStarted";
            }

            public struct Middleware
            {
                public static readonly string sendCommand = "sendCommand";
                public static readonly string sendCommandAsync = "sendCommandAsync";
                public static readonly string sendSubscribe = "sendSubscribe";
                public static readonly string sendSubscribeGeneric = "sendSubscribeGeneric";
                public static readonly string sendUnsusbscribe = "sendUnsusbscribe";

                public struct Subscriptions
                {
                    public static readonly string notificationReceived = "notificationReceived";
                    public static readonly string cabinCrewAcknowledged = "cabinCrewAcknowledged";
                    public static readonly string getCallCabinCrewStatusResponse = "cabinCrewCallIsActive";
                }
            }

            public struct SurveyManager
            {
                public static readonly string startSurveyManager = "startSurveyManager";
                public static readonly string resumeSurveyManager = "resumeSurveyManager";
                public static readonly string stopSurveyManager = "stopSurveyManager";
                public static readonly string pauseSurveyManager = "pauseSurveyManager";
                public static readonly string forceNextQuestionSet = "forceNextQuestionSet";
                public static readonly string setDone = "setDone";
            }

            public struct Tutorial
            {
                public static readonly string startTutorial = "startTutorial";
                public static readonly string restartTutorial = "restartTutorial";
                public static readonly string endTutorial = "endTutorial";
                public static readonly string tutorialFinished = "tutorialFinished";
            }
        }

        public struct MiddlewareCommands
        {
            public struct AnalyticsModule
            {
                public static readonly string applicationStart = "applicationStart";
                public static readonly string applicationQuit = "applicationQuit";
                public static readonly string recordEvent = "recordEvent";
                public static readonly string getUseAnalytics = "getUseAnalytics";
            }

            public struct DeviceInformationModule
            {
                public static readonly string getMaxVolume = "getMaxVolume";
                public static readonly string getVolume = "getVolume";
                public static readonly string setVolume = "setVolume";
                public static readonly string getBatteryLevel = "getBatteryLevel";
                public static readonly string getTargetHardware = "getTargetHardware";
            }

            public struct DemoModule
            {
                public static readonly string startCountdown = "startCountdown";
                public static readonly string stopCountdown = "stopCountdown";
            }

            public struct WifiHelperModule
            {
                public static readonly string getAvailableNetworks = "getAvailableNetworks";
                public static readonly string connectToNetwork = "connectToNetwork";
                public static readonly string disconnect = "disconnect";
            }
        }

        public struct Tags
        {
            public static readonly string videoPlayerMenu = "videoPlayerMenu";
            public static readonly string cameraSpawnName = "CameraSpawn";
        }

        public struct Resources
        {
            public static readonly string sfxPath = "Audio/SFX";
            public static readonly string cameraFade = "Prefabs/CameraFade";
            public static readonly string middlewareConnectorObject = "Prefabs/Middleware/InflightVRManager";
            public static readonly string defaultSplashscreen = "Sprites/Splashscreen";
            public static readonly string audioSubSelector = "Prefabs/AudioSubSelector";
            public static readonly string surveyManagerPrefab = "Prefabs/SurveyManager/SurveyContainer";
            public static readonly string imageNotFoundSprite = "Sprites/ImageNotFound";
            public static readonly string thumbnailMarker = "Prefabs/ThumbnailMarker";
            public static readonly string paginationMarker = "Prefabs/PaginationMarker";
            public static readonly string batterySpriteFolder = "Sprites/Battery/";
            public static readonly string networkItemPrefab = "Prefabs/NetworkItem";
            public static readonly string environmentPrefab = "Prefabs/EnvironmentItem";
            public static readonly string languageOptionsPrefab = "Prefabs/LanguageOptionsItem";

            public static readonly string homeSprite = "Sprites/Home";
            public static readonly string backSprite = "Sprites/Back";

            public static readonly string categoryPrefab = "Prefabs/Category";
            public static readonly string hinterPrefab = "Prefabs/HeadHinter";

            public static readonly string adPlayer = "Prefabs/AdPlayer";
            public static readonly string faderFollower = "Prefabs/FaderFollower";

            public static readonly string playSprite = "Sprites/Play";
            public static readonly string folderSprite = "Sprites/SubCategory";
            public static readonly string playerIconSprite = "Sprites/PlayerIcon";
        }

        public struct StreamingAssets
        {
        }

        public struct Localization
        {
            public static readonly string defaultLanguage = "en-gb";
            public static readonly string internalLocalizationDataKey = "Internal_Usage";
            public static readonly string applicationLocalizationKey = "com.picovr.vrlauncher"; //Application.identifier; This should be identifier, leaving hardcoded for now
            public static readonly string availableAudio = "available_audio";
            public static readonly string availableSubtitle = "available_subtitle";
            public static readonly string wifiSecureNetwork = "wifi_secure_network";
            public static readonly string wifiOpenNetwork = "wifi_open_network";
            public static readonly string adCountdown = "ad_countdown";
            public static readonly string skip = "skip";
            public static readonly string ad = "ad";
            public static readonly string volumeTooltip = "volume_tooltip";
        }

        public struct AnalyticsEvents
        {
            public static readonly string applicationQuit = "Application_Quit";
            public static readonly string applicationLaunch = "Application_Launch";
            public static readonly string routeLoaded = "Route_Loaded";

            public static readonly string menuItemOnSelect = "MenuItem_OnSelect";
            public static readonly string categoryOnSelect = "Category_OnSelect";
            public static readonly string languageOnSelect = "Language_OnSelect";

            public static readonly string mediaLanguageSelected = "Media_Language_Selected";

            public static readonly string menuLayout = "Menu_Layout";
            public static readonly string settingsOnSelect = "Settings_OnSelect";
            public static readonly string returnOnSelect = "Return_OnSelect";
            public static readonly string cameraLocation = "Camera_Location";
            public static readonly string environmentOnChange = "Environment_OnChange";
            public static readonly string adView = "Ad_View";
            public static readonly string adSkip = "Ad_Skip";

            public struct SurveyManager
            {
                public static readonly string surveySetStart = "Survey_Set_Start";
                public static readonly string surveySetfinish = "Survey_Set_Finish";
                public static readonly string surveyQuestionAnswer = "Survey_Question_Answer";
            }

            public struct Tutorial
            {
                public static readonly string tutorialStart = "Tutorial_Start";
                public static readonly string tutorialFinish = "Tutorial_Finish";
                public static readonly string tutorialStepFinish = "Tutorial_Step_Finish";
            }
        }

        public struct Tutorial
        {
            public readonly static string settingsPath = inflightPath + "/TutorialSettings.json";
            public readonly static string defaultSettings = "Data/TutorialSettingsDefault";
            public readonly static string customStepName = "CustomStep";

            public struct Localizations
            {
                public static readonly string videoPlayerByGaze = "video_player_gaze_appear";
                public static readonly string videoPlayerByButton = "video_player_button_appear";
            }
        }

        public struct Scenes
        {
            public readonly static string scenesPath = "Assets/Scenes/Application";
            public readonly static string persistentScene = "Persistent";
            public readonly static string splashscreenScene = "Splashscreen";
        }
    }
}
