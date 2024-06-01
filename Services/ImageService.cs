using System.Net;

namespace evantage.Services;

public class ImageDownloader : IDownloadImages
{
    public async Task<bool> DownloadImages(params ImageDownload[] requested_downloads)
    {
        // string imageUrl = "https://picsum.photos/id/102/4320/3240";
        string image_folder = "./wwwroot/img";
        if (!Directory.Exists(image_folder))
            Directory.CreateDirectory(image_folder);

        using (WebClient client = new WebClient())
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            client.Headers.Add("user-agent", "user agent string");

            var tasks = requested_downloads
                .Select(async req => { await DownloadImage(client, req); });

            // TaskExtensions.WhenAll<string>(tasks);
            Task.WhenAll(tasks);

            return true;
        }

        return false;
    }

    private async Task<string> DownloadImage(WebClient client, ImageDownload imageDownload)
    {
        string imageUrl = imageDownload.source.ToString();
        byte[] imageBytes = client.DownloadData(imageUrl);
        string save_path = $"./wwwroot/img/{imageDownload.save_name}.jpg";
        File.WriteAllBytes(save_path, imageBytes);
        return save_path;
    }
}

public record ImageDownload
{
    public Uri source { get; set; }
    public string save_name { get; set; }
}