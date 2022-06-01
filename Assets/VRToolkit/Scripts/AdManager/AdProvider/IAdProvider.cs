using System.Collections.Generic;

namespace VRToolkit.Advertisement.AdProvider
{
    public interface IAdProvider
    {
        List<AdObject> GetVideo2DAds();
        List<AdObject> GetVideo360Ads();

        //Expand in the future with more types
    }
}
