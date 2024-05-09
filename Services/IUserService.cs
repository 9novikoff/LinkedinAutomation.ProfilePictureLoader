using LinkedinAutomation.DTO;

namespace LinkedinAutomation.Services;

public interface IUserService
{
    public Task<UserProfileResponse> GetUserProfileAsync(string uri, string accessToken, CancellationToken cancellationToken);
}