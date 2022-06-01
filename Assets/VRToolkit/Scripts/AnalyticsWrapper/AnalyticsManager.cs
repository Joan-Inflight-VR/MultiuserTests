namespace VRToolkit.AnalyticsWrapper
{
    public static class AnalyticsManager
    {
        static IAnalytics analyticsEngine;

        public static void SetUp(IAnalytics anEngine)
        {
            analyticsEngine = anEngine;

            analyticsEngine?.SetUp();
        }

        public static void AnalyticsStart()
        {
            analyticsEngine?.AnalyticsStart();
        }

        public static void AnalyticsQuit()
        {
            analyticsEngine?.AnalyticsQuit();
        }

        /// <summary>
        /// Record an analytics event with name and details.
        /// </summary>
        /// <param name="eventName">The name of the event to send</param>
        /// <param name="analyticsObject">The details of the event as an AnalyrticsObject</param>
        public static void RecordEvent(string eventName, AnalyticsObject analyticsObject)
        {
            analyticsEngine?.RecordEvent(eventName, analyticsObject);
        }

    }
}
