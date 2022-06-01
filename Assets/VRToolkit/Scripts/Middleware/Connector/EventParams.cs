using System;
using static VRToolkit.Middleware.MiddlewareBase;

namespace VRToolkit.Middleware
{
    public class EventParams
    {
        public struct CommandParams
        {
            public string command;
            public object[] parameters;

            public CommandParams(string command, params object[] parameters)
            {
                this.command = command;
                this.parameters = parameters;
            }
        }

        public struct CommandAsyncParams
        {
            public string command;
            public Action<object> callback;
            public object[] parameters;

            public CommandAsyncParams(string command, Action<object> callback, params object[] parameters)
            {
                this.command = command;
                this.callback = callback;
                this.parameters = parameters;
            }
        }

        public struct SubscribeParams
        {
            public string subscription;
            public MiddlewareNotification action;

            public SubscribeParams(string subscription, MiddlewareNotification action)
            {
                this.subscription = subscription;
                this.action = action;
            }
        }

        public struct SubscribeGenericParams
        {
            public string subscription;
            public MiddlewareNotification<object> action;

            public SubscribeGenericParams(string subscription, MiddlewareNotification<object> action)
            {
                this.subscription = subscription;
                this.action = action;
            }
        }

        public struct UnsubscribeParams
        {
            public string subscription;

            public UnsubscribeParams(string subscription)
            {
                this.subscription = subscription;
            }
        }
    }
}
