using System;
using UnityEngine;
using UnityEngine.Events;
using VRToolkit.Managers;
using VRToolkit.Utils;

namespace VRToolkit.Middleware
{
    [Serializable]
    public class NetWorkEvent : UnityEvent<string> { }

    public class WiFiHelper : MonoBehaviour
    {
        /// <summary>
        /// This is being called by the Middleware's Unity bridge once the wifi scan finishes
        /// </summary>
        /// <param name="availableWifiNetworks">The json array with all the networks</param>
        public void OnWifiScanCompleted(string availableWifiNetworks)
        {
            Debug.Log($"WifiScanResult: {availableWifiNetworks}");
            EventManager.Instance.TriggerEvent(Statics.Events.onNetworksReceived, availableWifiNetworks);
        }

        /// <summary>
        /// Asks Middleware for starting a network scan
        /// </summary>
        public void GetAvailableNetworks()
        {
            EventManager.Instance.TriggerEvent(
                Statics.Events.Middleware.sendCommand,
                new EventParams.CommandParams(Statics.MiddlewareCommands.WifiHelperModule.getAvailableNetworks));
        }

        /// <summary>
        /// Connect to specific network from the list. It sends the info to the middleware to perform the connection.
        /// </summary>
        /// <param name="id">id of the network</param>
        /// <param name="password">clear text password from VR</param>
        public void ConnectToNetwork(int id, string password)
        {
            EventManager.Instance.TriggerEvent(
                Statics.Events.Middleware.sendCommand,
                new EventParams.CommandParams(Statics.MiddlewareCommands.WifiHelperModule.connectToNetwork, id, password));

        }

        /// <summary>
        /// Disconnect the current wifi network
        /// </summary>
        public void Disconnect()
        {
            EventManager.Instance.TriggerEvent(
                Statics.Events.Middleware.sendCommand,
                new EventParams.CommandParams(Statics.MiddlewareCommands.WifiHelperModule.disconnect));
        }
    }
}
