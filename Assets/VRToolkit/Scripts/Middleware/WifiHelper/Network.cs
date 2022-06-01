using Newtonsoft.Json;

public class Network
{
    [JsonProperty("id")]
    public int id;
    [JsonProperty("ssid")]
    public string ssid;
    [JsonProperty("intensity")]
    public int intensity;
    [JsonProperty("capabilities")]
    public string capabilities;
}
