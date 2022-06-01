namespace VRToolkit.AnalyticsWrapper
{
    public interface IAnalytics
    {
        void SetUp();
        void RecordEvent(string eventName, AnalyticsObject analyticsObject);
        void AnalyticsStart();
        void AnalyticsQuit();
    }
}
