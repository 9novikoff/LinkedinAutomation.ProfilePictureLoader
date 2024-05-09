namespace LinkedinAutomation.Services;

public interface IImageLoader
{
    public Task LoadImageAsync(string uri, string? path, CancellationToken cancellationToken);
}