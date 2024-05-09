using System.Diagnostics;
using System.Net.Http.Headers;
using LinkedinAutomation.DTO;
using LinkedinAutomation.Services;
using Newtonsoft.Json;

namespace LinkedinAutomation;

public class Cli : BackgroundService
{
    private readonly IUserService _userService;
    private readonly IImageLoader _imageLoader;
    private readonly IConfiguration _config;
    private readonly IHostApplicationLifetime _hostApplicationLifetime;

    public Cli(IConfiguration config, IUserService userService, IImageLoader imageLoader, IHostApplicationLifetime hostApplicationLifetime)
    {
        _config = config;
        _userService = userService;
        _imageLoader = imageLoader;
        _hostApplicationLifetime = hostApplicationLifetime;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var accessToken = _config["Authentication:AccessToken"];

        if (accessToken == null)
        {
            Console.WriteLine("Add Authentication:AccessToken to the appsettings.json");
            _hostApplicationLifetime.StopApplication();
            return;
        }
        
        try
        {
            if (accessToken.Equals(string.Empty))
            {
                Console.WriteLine("Seems that you are not authenticated. Do you want to do it now? Y/n");

                while (!stoppingToken.IsCancellationRequested)
                {
                    var response = Console.ReadLine();

                    if (response == null || response.ToLower().Equals("no") || response.ToLower().Equals("n"))
                    {
                        _hostApplicationLifetime.StopApplication();
                        return;
                    }

                    if (response.ToLower().Equals("yes") || response.ToLower().Equals("y"))
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            FileName =
                                $"{_config["Url:AuthorizationUrl"]}?response_type=code&client_id={_config["Credentials:ClientId"]}&redirect_uri={_config["Url:RedirectUrl"]}&scope=profile%20openid",
                            UseShellExecute = true
                        });
                        break;
                    }

                    Console.WriteLine("Invalid input. Try again");
                }

                while (!stoppingToken.IsCancellationRequested && accessToken.Equals(string.Empty))
                {
                    await Task.Delay(5000, stoppingToken);
                    accessToken = _config["Authentication:AccessToken"];
                }

                Utils.AddOrUpdateAppSetting("Authentication:AccessToken", accessToken);
            }
            
            var userProfileResponse =
                await _userService.GetUserProfileAsync(_config["Url:UserInfoUrl"]!, accessToken, stoppingToken);
            await _imageLoader.LoadImageAsync(userProfileResponse.Picture, null, stoppingToken);
            Console.WriteLine("Profile picture successfully saved");
            _hostApplicationLifetime.StopApplication();
        }
        catch (UnauthorizedAccessException e)
        {
            Console.WriteLine("Invalid token. Please authorize");
            Utils.AddOrUpdateAppSetting("Authentication:AccessToken", "");
            _hostApplicationLifetime.StopApplication();
        }
    }
}