using Mp4EmojisPlayer.Services;

namespace Mp4EmojisPlayer.Platforms.Android.Services
{
    public class VideoService : IVideoService
    {
        public async Task<string> GetVideoUriAsync(string videoFileName)
        {
            try
            {
                // 首先检查文件是否存在
                var stream = await FileSystem.OpenAppPackageFileAsync($"videos/{videoFileName}");
                if (stream == null)
                {
                    return string.Empty;
                }
                stream.Dispose();

                // 在Android上，对于MAUI应用的Raw资源，使用android.resource协议更合适
                var packageName = Platform.CurrentActivity?.PackageName ?? "com.companyname.mp4emojisplayer";
                var resourceName = Path.GetFileNameWithoutExtension(videoFileName);
                
                // 尝试使用android.resource协议，如果失败则回退到ms-appx
                try
                {
                    // 对于在Raw/videos子目录中的文件，Android可能需要特殊处理
                    // 先尝试直接的资源路径
                    return $"android.resource://{packageName}/raw/{resourceName}";
                }
                catch
                {
                    // 如果android.resource失败，回退到ms-appx协议
                    return $"ms-appx:///Resources/Raw/videos/{videoFileName}";
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting video URI: {ex.Message}");
                // 回退到基本的ms-appx协议
                return $"ms-appx:///Resources/Raw/videos/{videoFileName}";
            }
        }

        public async Task<bool> VideoExistsAsync(string videoFileName)
        {
            try
            {
                // 检查Raw资源中是否存在该文件
                var stream = await FileSystem.OpenAppPackageFileAsync($"videos/{videoFileName}");
                if (stream != null)
                {
                    stream.Dispose();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
