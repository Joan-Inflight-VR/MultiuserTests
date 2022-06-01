using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace VRToolkit.Middleware
{
    public class AndroidMiddleware : MiddlewareBase
    {
        private Dictionary<string, MiddlewareNotification> subscriptions = new Dictionary<string, MiddlewareNotification>();
        private Dictionary<string, MiddlewareNotification> methodSubscriptions = new Dictionary<string, MiddlewareNotification>();

        private AndroidJavaClass connector;

        private AndroidJavaClass Connector
        {
            get
            {
                if (connector == null)
                {
                    CreateConnector();
                }

                return connector;
            }
        }

        private void Awake()
        {
            CreateConnector();
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            Connector?.CallStatic("toggleFront", !pauseStatus);
        }

        private void OnDestroy()
        {
            Connector?.CallStatic("unbindService");
            connector = null;
        }

        #region Middleware internals
        /// <summary>
        /// Creates the connector to the Unity bridge
        /// </summary>
        private void CreateConnector()
        {
            if (Application.isEditor || !Application.isMobilePlatform) { return; }

            connector = new AndroidJavaClass("com.InflightVR.unitybridge.UnityBridge");
            AndroidJavaClass unityActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject context = unityActivity.GetStatic<AndroidJavaObject>("currentActivity");

            if (Connector == null)
            {
                Debug.LogError("Failed to create Connector");
                return;
            }

            Connector.CallStatic("bindService", context, BundleID);
        }

        /// <summary>
        /// Method executed when a notification arrives
        /// </summary>
        /// <param name="payload">Notification data</param>
        private void AndroidNotificationReceiver(string payload)
        {
            if (Application.isEditor || !Application.isMobilePlatform) { return; }

            JArray parsed = JArray.Parse(payload);
            if (subscriptions.TryGetValue(parsed[0].ToObject<string>(), out MiddlewareNotification value))
            {
                value.Invoke(parsed[1]);
            }
        }

        /// <summary>
        /// Method executed when a remote call returns
        /// </summary>
        /// <param name="payload">Call data</param>
        private void AndroidReturnReceiver(string payload)
        {
            if (Application.isEditor || !Application.isMobilePlatform) { return; }
            if (string.IsNullOrEmpty(payload)) { return; }

            JArray parsed = JArray.Parse(payload);
            string callback = parsed[0].ToObject<string>();

            if (methodSubscriptions.TryGetValue(callback, out MiddlewareNotification value))
            {
                value.Invoke(parsed[1]);
                methodSubscriptions.Remove(callback);
            }
        }
        #endregion

        public override void SendCommand(string command, params object[] parameters)
        {
            if (Application.isEditor || !Application.isMobilePlatform) { return; }
            if (Connector == null)
            {
                Debug.LogError("Connector is not present");
                return;
            }

            JToken param = JToken.FromObject(parameters);

            string paramsSent = param?.ToString() ?? string.Empty;

            Connector.CallStatic("sendCommand", command, paramsSent);
        }

        public override void SendCommandAsync<T>(string command, Action<T> callback, params object[] parameters)
        {
            if (Application.isEditor || !Application.isMobilePlatform) { return; }
            if (Connector == null)
            {
                Debug.LogError("Connector is not present");
                return;
            }

            JToken param = JToken.FromObject(parameters);

            string paramsSent = param?.ToString() ?? string.Empty;

            string callbackUid = Guid.NewGuid().ToString();

            methodSubscriptions[callbackUid] = payload =>
            {
                callback(payload.ToObject<T>());
            };

            Connector.CallStatic("sendCommandAsync", command, paramsSent, callbackUid);
        }

        public override void Subscribe(string subscription, MiddlewareNotification action)
        {
            subscriptions[subscription] = action;
        }

        public override void SubscribeGeneric<T>(string subscription, MiddlewareNotification<T> action)
        {
            subscriptions[subscription] = (payload) => {
                action.Invoke(JsonUtility.FromJson<T>(payload.ToString()));
            };
        }

        public override void Unsubscribe(string subscription)
        {
            if (subscriptions.TryGetValue(subscription, out MiddlewareNotification value))
            {
                subscriptions.Remove(subscription);
            }
        }
    }
}
