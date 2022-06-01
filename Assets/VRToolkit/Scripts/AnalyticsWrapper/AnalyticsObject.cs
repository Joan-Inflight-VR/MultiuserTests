using Newtonsoft.Json;
using System.Collections.Generic;

namespace VRToolkit.AnalyticsWrapper
{
    public class AnalyticsObject
    {
        private Dictionary<string, object> analyticDetails;

        public AnalyticsObject()
        {
            analyticDetails = new Dictionary<string, object>();
        }

        public void AddData(string dataName, object dataDetails)
        {
            analyticDetails.Add(dataName, dataDetails);
        }

        public void RemoveData(string dataName)
        {
            if (analyticDetails.TryGetValue(dataName, out object details))
            {
                analyticDetails.Remove(dataName);
            }
        }

        public void ModifyDetails(string dataName, object newDataDetails)
        {
            analyticDetails[dataName] = newDataDetails;
        }

        public string GetObjectAsJson()
        {
            return JsonConvert.SerializeObject(analyticDetails);
        }
    }
}
