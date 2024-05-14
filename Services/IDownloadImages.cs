using System.Threading.Tasks;

namespace evantage.Services;

public interface IDownloadImages
{
    Task<bool> DownloadImages(params ImageDownload[] requested_downloads);
}