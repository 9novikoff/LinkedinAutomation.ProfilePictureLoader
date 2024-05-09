namespace LinkedinAutomation.Services;

class ImageLoader : IImageLoader
{
    private readonly HttpClient _httpClient;
    public ImageLoader(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task LoadImageAsync(string uri, string? path, CancellationToken cancellationToken)
    {
        var httpResult = await _httpClient.GetAsync(uri, cancellationToken);
        await using var resultStream = await httpResult.Content.ReadAsStreamAsync(cancellationToken);
        
        path ??= AppContext.BaseDirectory + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".jpg";
        
        await using var fileStream = File.Create(path);
        await resultStream.CopyToAsync(fileStream, cancellationToken);
    }
}