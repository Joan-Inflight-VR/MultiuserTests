using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace VRToolkit.Managers
{
    public class UnityEventObject : UnityEvent<object> { };

    /// <summary>
    /// EventManager class for managing events/actions with data/orNot
    /// </summary>
    public class EventManager : MonoBehaviour
    {
        private Dictionary<string, UnityEvent> events;
        private Dictionary<string, UnityEventObject> eventsWithData;

        private static EventManager eventManager;

        public static EventManager Instance
        {
            get
            {
                if (!eventManager)
                {
                    eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

                    if (!eventManager)
                    {
                        Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
                    }
                    else
                    {
                        eventManager.Initialize();
                    }
                }

                return eventManager;
            }
        }


        private void Initialize()
        {
            events = new Dictionary<string, UnityEvent>();
            eventsWithData = new Dictionary<string, UnityEventObject>();
        }

        /// <summary>
        /// Start listerner to an event without extra data
        /// </summary>
        /// <param name="eventName">Name of the event</param>
        /// <param name="action">Action to execute</param>
        public void StartListening(string eventName, UnityAction action)
        {
            if (Instance.events.TryGetValue(eventName, out UnityEvent auxEvent))
            {
                auxEvent.AddListener(action);
            }
            else
            {
                auxEvent = new UnityEvent();
                auxEvent.AddListener(action);
                Instance.events.Add(eventName, auxEvent);
            }
        }

        /// <summary>
        /// Start listener to an event with extra data
        /// </summary>
        /// <param name="eventName">Name of the event</param>
        /// <param name="action">Action to execute with the extra data</param>
        public void StartListening(string eventName, UnityAction<object> action)
        {
            if (Instance.eventsWithData.TryGetValue(eventName, out UnityEventObject auxEvent))
            {
                auxEvent.AddListener(action);
            }
            else
            {
                auxEvent = new UnityEventObject();
                auxEvent.AddListener(action);
                Instance.eventsWithData.Add(eventName, auxEvent);
            }
        }

        /// <summary>
        /// Stop a specific listener from an event
        /// </summary>
        /// <param name="eventName">Name of the event</param>
        /// <param name="action">Action to stop</param>
        public void StopListening(string eventName, UnityAction action)
        {
            if (Instance.events.TryGetValue(eventName, out UnityEvent auxEvent))
            {
                auxEvent.RemoveListener(action);
            }
        }

        /// <summary>
        /// Stop a specific listener from an event with data
        /// </summary>
        /// <param name="eventName">Name of the event</param>
        /// <param name="action">Action to stop</param>
        public void StopListening(string eventName, UnityAction<object> action)
        {
            if (Instance.eventsWithData.TryGetValue(eventName, out UnityEventObject auxEvent))
            {
                auxEvent.RemoveListener(action);
            }
        }

        /// <summary>
        /// Trigger a speficic event
        /// </summary>
        /// <param name="eventName">Name of the event</param>
        public void TriggerEvent(string eventName)
        {
            if (Instance.events.TryGetValue(eventName, out UnityEvent auxEvent))
            {
                if (auxEvent != null)
                    auxEvent.Invoke();
            }
        }

        /// <summary>
        /// Trigger a specific event with data
        /// </summary>
        /// <param name="eventName">Name of the event</param>
        /// <param name="data">Data to send</param>
        public void TriggerEvent(string eventName, object data)
        {
            if (Instance.eventsWithData.TryGetValue(eventName, out UnityEventObject auxEvent))
            {
                if (auxEvent != null)
                    auxEvent.Invoke(data);
            }
        }
    }
}
