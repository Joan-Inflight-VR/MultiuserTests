using System;
using System.Collections.Generic;
using System.IO;
using VRToolkit.Utils;

namespace VRToolkit.Advertisement.AdProvider.Implementations
{
    public class LocalAdProvider : IAdProvider
    {
        public List<AdObject> GetVideo2DAds()
        {
            return GetAdObjectsByPathAndType(Statics.AdManager.videos2DRoot, AdType.Video2D);
        }

        public List<AdObject> GetVideo360Ads()
        {
            return GetAdObjectsByPathAndType(Statics.AdManager.videos360Root, AdType.Video360);
        }

        private List<AdObject> GetAdObjectsByPathAndType(string path, AdType type)
        {
            try
            {
                List<AdObject> result = new List<AdObject>();

                DirectoryInfo directory = new DirectoryInfo(path);

                foreach (FileInfo file in directory.GetFiles())
                {
                    result.Add(new AdObject(file.FullName, file.Name, type));
                }

                return result;
            }
            catch(Exception e)
            {
                UnityEngine.Debug.LogWarning($"Error accessing path: {path} - {e.Message}");
                return null;
            }
        }
    }
}