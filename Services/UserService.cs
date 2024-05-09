using System.Net;
using System.Net.Http.Headers;
using LinkedinAutomation.DTO;
using Newtonsoft.Json;

namespace LinkedinAutomation.Services;

class UserService : IUserService
{
    private readonly HttpClient _httpClient;
    
    public UserService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<UserProfileResponse> GetUserProfileAsync(string uri, string accessToken, CancellationToken cancellationToken)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var avatarResponse = await _httpClient.GetAsync(uri, cancellationToken);

        if (avatarResponse.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new UnauthorizedAccessException();
        }
        
        var responseContent = await avatarResponse.Content.ReadAsStringAsync(cancellationToken);
        return JsonConvert.DeserializeObject<UserProfileResponse>(responseContent);
    }
}