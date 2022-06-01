using System;
using UnityEngine;
using VRToolkit.Managers;
using VRToolkit.Middleware;
using VRToolkit.Utils;
using static VRToolkit.Middleware.EventParams;

namespace VRToolkit.AnalyticsWrapper.Implementations
{
    public class AnalyticsMiddleware : IAnalytics
    {
        private string bundleID;

        private bool sessionStarted = false;
        private bool recordingAnalytics = false;

        public void SetUp()
        {
            if (Application.isEditor) { return; }
            if (sessionStarted)
            {
                Debug.LogError("Analytics session already started");
                return;
            }

            bundleID = MiddlewareBase.BundleID;

            if (MiddlewareBase.IsMiddledwareReady)
            {
                EventManager.Instance.TriggerEvent(
                    Statics.Events.Middleware.sendCommandAsync,
                    new CommandAsyncParams(Statics.MiddlewareCommands.AnalyticsModule.getUseAnalytics, Initialize));
            }
            else
            {
                MiddlewareBase.MiddlewareReady += OnMiddlewareRaised;
            }
        }

        private void OnMiddlewareRaised(object sender, EventArgs args)
        {
            MiddlewareBase.MiddlewareReady -= OnMiddlewareRaised;
            EventManager.Instance.TriggerEvent(
                Statics.Events.Middleware.sendCommandAsync,
                new CommandAsyncParams(Statics.MiddlewareCommands.AnalyticsModule.getUseAnalytics, Initialize));
        }

        private void Initialize(object value)
        {
            bool recordAnalytics = (bool)value;
            Debug.Log("Initializing analytics from Middleware config: " + recordAnalytics);
            recordingAnalytics = recordAnalytics;

            if (!recordingAnalytics) { return; }

            AnalyticsStart();
        }

        private void RecordEvent(string eventName, string eventDetails = "")
        {
            if (Application.isEditor) { return; }
            if (!recordingAnalytics) { return; }

            if (!sessionStarted)
            {
                Debug.Log("Event not logged, please initialize Analytics first");
            }
            else
            {
                EventManager.Instance.TriggerEvent(
                    Statics.Events.Middleware.sendCommand,
                    new CommandParams(Statics.MiddlewareCommands.AnalyticsModule.recordEvent, bundleID, Application.version, eventName, eventDetails));
            }
        }

        public void RecordEvent(string eventName, AnalyticsObject analyticsObject)
        {
            RecordEvent(eventName, analyticsObject.GetObjectAsJson());
        }

        public void AnalyticsStart()
        {
            if (!recordingAnalytics) { return; }

            sessionStarted = true;
        }

        public void AnalyticsQuit()
        {
            if (Application.isEditor) { return; }
            if (!recordingAnalytics) { return; }

            if (sessionStarted)
            {
                sessionStarted = false; // this stops the session, so it does not make sense to call it as it is right now...
            }
            else
            {
                Debug.Log("Failed to end analytics session as it never started");
            }
        }
    }
}
