using Newtonsoft.Json;

namespace LinkedinAutomation.DTO;

public class TokenResponse
{
    [JsonProperty("access_token")]
    public string AccessToken { get; set; }
    [JsonProperty("expires_in")]
    public int ExpiresIn { get; set; }
    [JsonProperty("scope")]
    public string Scope { get; set; }
}