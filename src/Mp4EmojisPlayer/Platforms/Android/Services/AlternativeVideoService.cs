using Mp4EmojisPlayer.Services;

namespace Mp4EmojisPlayer.Platforms.Android.Services
{
    /// <summary>
    /// 备用Android视频服务，提供多种路径尝试方式
    /// </summary>
    public class AlternativeVideoService : IVideoService
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

                // 尝试多种Android路径格式
                var possiblePaths = new[]
                {
                    $"ms-appx:///Resources/Raw/videos/{videoFileName}",  // 标准MAUI路径
                    $"ms-appdata:///local/Resources/Raw/videos/{videoFileName}", // 本地数据路径
                    $"videos/{videoFileName}", // 简单相对路径
                    $"Resources/Raw/videos/{videoFileName}", // 完整相对路径
                    $"android_asset/Resources/Raw/videos/{videoFileName}" // Android资源路径
                };

                // 返回第一个尝试路径，实际使用中可以循环测试
                var selectedPath = possiblePaths[0];
                System.Diagnostics.Debug.WriteLine($"Generated Android video URI: {selectedPath}");
                return selectedPath;
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
