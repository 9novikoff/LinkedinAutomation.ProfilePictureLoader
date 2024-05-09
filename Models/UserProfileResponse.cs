using Newtonsoft.Json;

namespace LinkedinAutomation.DTO;

public class UserProfileResponse
{
    [JsonProperty("name")]
    public string Name { get; set; }
    [JsonProperty("picture")]
    public string Picture { get; set; }
}