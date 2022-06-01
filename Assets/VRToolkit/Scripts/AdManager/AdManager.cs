using System;
using System.Collections.Generic;
using VRToolkit.Advertisement.AdProvider;

namespace VRToolkit.Advertisement
{
    public static class AdManager
    {
        public static IAdProvider adProvider = null;
        public static Dictionary<AdType, List<AdObject>> adsAvailable;

        public static void SetUpProvider(IAdProvider provider)
        {
            if (provider == null) return;

            adsAvailable = new Dictionary<AdType, List<AdObject>>();

            adProvider = provider;

            foreach (AdType adType in Enum.GetValues(typeof(AdType)))
            {
                List<AdObject> ads = new List<AdObject>();
                switch (adType)
                {
                    case AdType.Video2D:
                        ads = adProvider.GetVideo2DAds();
                        break;
                    case AdType.Video360:
                        ads = adProvider.GetVideo360Ads();
                        break;
                }

                if (ads != null && ads.Count != 0)
                {
                    adsAvailable.Add(adType, ads);
                }
            }
        }

        public static AdObject AskForAd(AdType type, string id = "")
        {
            if (adProvider == null || adsAvailable.Count == 0) return null;

            if (!string.IsNullOrEmpty(id))
            {
                List<AdObject> adsByType = adsAvailable[type];
                foreach (AdObject ad in adsByType)
                {
                    if (ad.id.Equals(id))
                    {
                        return ad;
                    }
                }
            }

            return GetRandomAdByType(type);
        }

        private static AdObject GetRandomAdByType(AdType type)
        {
            List<AdObject> adsByType = adsAvailable[type];

            return adsByType[UnityEngine.Random.Range(0, adsByType.Count)];
        }
    }
}
