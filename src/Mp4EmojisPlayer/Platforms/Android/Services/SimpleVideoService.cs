using Mp4EmojisPlayer.Services;

namespace Mp4EmojisPlayer.Platforms.Android.Services
{
    /// <summary>
    /// 简化的Android视频服务，避免LocalBroadcastManager依赖
    /// </summary>
    public class SimpleVideoService : IVideoService
    {
        public async Task<string> GetVideoUriAsync(string videoFileName)
        {
            try
            {
                // 检查文件是否存在
                if (!await VideoExistsAsync(videoFileName))
                {
                    System.Diagnostics.Debug.WriteLine($"Video file not found: {videoFileName}");
                    return string.Empty;
                }

                // 对于Android上的MAUI应用，使用embed://协议
                // 根据微软官方文档，embed:// 是推荐的方式
                var resourcePath = $"embed://videos/{videoFileName}";
                System.Diagnostics.Debug.WriteLine($"Generated Android video URI: {resourcePath}");
                return resourcePath;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting video URI: {ex.Message}");
                return string.Empty;
            }
        }

        public async Task<bool> VideoExistsAsync(string videoFileName)
        {
            try
            {
                // 使用正确的路径检查文件是否存在
                using var stream = await FileSystem.OpenAppPackageFileAsync($"videos/{videoFileName}");
                var exists = stream != null;
                System.Diagnostics.Debug.WriteLine($"Video file exists check for {videoFileName}: {exists}");
                return exists;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error checking video file existence for {videoFileName}: {ex.Message}");
                return false;
            }
        }
    }
}
