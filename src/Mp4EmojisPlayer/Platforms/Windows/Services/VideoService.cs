using Mp4EmojisPlayer.Services;

namespace Mp4EmojisPlayer.Platforms.Windows.Services
{
    public class VideoService : IVideoService
    {
        public async Task<string> GetVideoUriAsync(string videoFileName)
        {
            try
            {
                // 在Windows上使用ms-appx协议
                return $"ms-appx:///Resources/Raw/videos/{videoFileName}";
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
                var stream = await FileSystem.OpenAppPackageFileAsync($"videos/{videoFileName}");
                stream?.Dispose();
                return stream != null;
            }
            catch
            {
                return false;
            }
        }
    }
}
