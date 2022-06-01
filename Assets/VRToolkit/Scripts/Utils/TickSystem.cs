using UnityEngine;
using VRToolkit.Managers;

namespace VRToolkit.Utils
{
    public static class TickSystem
    {
        private static int tick;

        private static GameObject tickSystemObject;

        public static float ticksPerSecond = 1;

        public static void CreateTickSystem(float tps = 1)
        {
            if (tickSystemObject == null)
            {
                tickSystemObject = new GameObject("TickSystemObject");
                tickSystemObject.AddComponent<TickSystemObject>();
                ticksPerSecond = tps;
            }
        }

        public static void DestroyTickSystem()
        {
            if (tickSystemObject != null)
            {
                Object.Destroy(tickSystemObject);
            }
        }

        public class TickSystemObject : MonoBehaviour
        {
            private float tickTimer;

            private void Awake()
            {
                tick = 0;
            }

            private void Update()
            {
                tickTimer += Time.deltaTime;

                if (tickTimer >= (1 / ticksPerSecond))
                {
                    tickTimer -= (1 / ticksPerSecond);
                    tick++;

                    EventManager.Instance.TriggerEvent(Statics.Events.onTick, tick);
                }
            }
        }
    }
}