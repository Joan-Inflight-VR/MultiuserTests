using System;
using static VRToolkit.Middleware.MiddlewareBase;

namespace VRToolkit.Middleware
{
    public interface IMiddleware
    {
        /// <summary>
        /// Sends a command to the middleware
        /// </summary>
        /// <param name="command">The name of the command</param>
        /// <param name="parameters">The data to send along with the command</param>
        void SendCommand(string command, params object[] parameters);

        /// <summary>
        /// An asynchronous call to send a command to the middleware
        /// </summary>
        /// <typeparam name="T">The type that is being sent along with the command</typeparam>
        /// <param name="command">The name of the command being sent</param>
        /// <param name="callback">The action to invoke once the middleware replies</param>
        /// <param name="parameters">The data to send along with the command</param>
        void SendCommandAsync<T>(string command, Action<T> callback, params object[] parameters);

        /// <summary>
        /// A subscription to middleware events
        /// </summary>
        /// <param name="subscription">The event name to subscribe to</param>
        /// <param name="action">The action to invoke on receiving the event</param>
        void Subscribe(string subscription, MiddlewareNotification action);

        /// <summary>
        /// A subscription to middleware events
        /// </summary>
        /// <typeparam name="T">The type of data to receive from the event</typeparam>
        /// <param name="subscription">The event name to subscribe to</param>
        /// <param name="action">The action to invoke on receiving the event with the data</param>
        void SubscribeGeneric<T>(string subscription, MiddlewareNotification<T> action);

        /// <summary>
        /// Unsubscribe to an event
        /// </summary>
        /// <param name="subscription"></param>
        void Unsubscribe(string subscription);
    }
}
