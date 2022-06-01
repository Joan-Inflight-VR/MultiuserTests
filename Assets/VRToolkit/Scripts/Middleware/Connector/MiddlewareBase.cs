using Newtonsoft.Json.Linq;
using System;
using UnityEngine;
using VRToolkit.Managers;
using VRToolkit.Utils;
using static VRToolkit.Middleware.EventParams;

namespace VRToolkit.Middleware
{
    public abstract class MiddlewareBase : MonoBehaviour, IMiddleware
    {
        public delegate void MiddlewareNotification<in T>(T payload);
        public delegate void MiddlewareNotification(JToken payload);

        public static event EventHandler MiddlewareReady;

        private static bool isMiddlewareReady = false;
        public static bool IsMiddledwareReady { get { return isMiddlewareReady; } }

        private static string bundleID = null;

        public static string BundleID
        {
            get
            {
                if (bundleID == null)
                {
                    bundleID = Application.identifier;
                }

                return bundleID;
            }
        }

        public abstract void SendCommand(string command, params object[] parameters);
        public abstract void SendCommandAsync<T>(string command, Action<T> callback, params object[] parameters);
        public abstract void Subscribe(string subscription, MiddlewareNotification action);
        public abstract void SubscribeGeneric<T>(string subscription, MiddlewareNotification<T> action);
        public abstract void Unsubscribe(string subscription);

        protected void RaiseMiddlewareReady()
        {
            Debug.Log("Middleware is now ready");
            MiddlewareReady?.Invoke(this, EventArgs.Empty);

            isMiddlewareReady = true;
        }

        private void Start()
        {
            EventManager.Instance.StartListening(Statics.Events.Middleware.sendCommand, OnSendCommand);
            EventManager.Instance.StartListening(Statics.Events.Middleware.sendCommandAsync, OnSendCommandAsync);
            EventManager.Instance.StartListening(Statics.Events.Middleware.sendUnsusbscribe, OnSendSubscribe);
            EventManager.Instance.StartListening(Statics.Events.Middleware.sendSubscribeGeneric, OnSendSubscribeGeneric);
            EventManager.Instance.StartListening(Statics.Events.Middleware.sendUnsusbscribe, OnSendUnsubscribe);
        }

        private void OnSendCommand(object parameters)
        {
            CommandParams commandParams = (CommandParams)parameters;

            SendCommand(commandParams.command, commandParams.parameters);
        }

        private void OnSendCommandAsync(object parameters)
        {
            CommandAsyncParams commandParams = (CommandAsyncParams)parameters;

            SendCommandAsync(commandParams.command, commandParams.callback, commandParams.parameters);
        }

        private void OnSendSubscribe(object parameters)
        {
            SubscribeParams commandParams = (SubscribeParams)parameters;

            Subscribe(commandParams.subscription, commandParams.action);
        }

        private void OnSendSubscribeGeneric(object parameters)
        {
            SubscribeGenericParams commandParams = (SubscribeGenericParams)parameters;

            SubscribeGeneric(commandParams.subscription, commandParams.action);
        }

        private void OnSendUnsubscribe(object parameters)
        {
            UnsubscribeParams commandParams = (UnsubscribeParams)parameters;

            Unsubscribe(commandParams.subscription);
        }
    }
}