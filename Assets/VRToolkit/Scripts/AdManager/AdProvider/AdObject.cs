namespace VRToolkit.Advertisement.AdProvider
{
    public class AdObject
    {
        public string id;
        public string uri;
        public AdType type;
        //We can expand this once more requirements arrive, such as if it is skippable, the provider source, etc...

        public AdObject(string uri, string id, AdType type)
        {
            this.id = id;
            this.uri = uri;
            this.type = type;
        }
    }
}
