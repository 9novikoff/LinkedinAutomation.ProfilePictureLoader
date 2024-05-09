using LinkedinAutomation.DTO;
using LinkedinAutomation.Services;
using Newtonsoft.Json;
using HttpClient = System.Net.Http.HttpClient;

namespace LinkedinAutomation;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddHttpClient<IUserService, UserService>((client, p) => 
            new UserService(client));

        builder.Services.AddHttpClient<IImageLoader, ImageLoader>((client, p) => 
            new ImageLoader(client));
        
        builder.Services.AddHostedService<Cli>();

        var app = builder.Build();

        app.MapGet("/callback", async (string code) =>
        {
            using var httpClient = new HttpClient();
            var url = new Uri($"{builder.Configuration["Url:AccessTokenUrl"]!}?grant_type=authorization_code&code={code}" +
                              $"&client_id={builder.Configuration["Credentials:ClientId"]}&client_secret={builder.Configuration["Credentials:ClientSecret"]}" +
                              $"&redirect_uri={builder.Configuration["Url:RedirectUrl"]}");
            
            var response = await httpClient.PostAsync(url, null);
            var responseContent = await response.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<TokenResponse>(responseContent);
            
            builder.Configuration["Authentication:AccessToken"] = token.AccessToken;

            return "Your call is authenticated";
        });

        app.Run();
    }
}