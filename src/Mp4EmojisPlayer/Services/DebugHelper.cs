using Mp4EmojisPlayer.Services;
using Mp4EmojisPlayer.Models;

namespace Mp4EmojisPlayer.Services
{
    /// <summary>
    /// 调试帮助服务，用于测试文件路径和资源访问
    /// </summary>
    public class DebugHelper
    {
        public static async Task<string> DiagnoseVideoPathsAsync(string videoFileName)
        {
            var result = new System.Text.StringBuilder();
            result.AppendLine($"=== 诊断视频文件路径: {videoFileName} ===");

            // 1. 检查文件是否存在
            try
            {
                using var stream = await FileSystem.OpenAppPackageFileAsync($"videos/{videoFileName}");
                if (stream != null)
                {
                    result.AppendLine("✅ 文件存在于 videos/ 路径");
                    result.AppendLine($"   文件大小: {stream.Length} bytes");
                }
                else
                {
                    result.AppendLine("❌ 文件不存在于 videos/ 路径");
                }
            }
            catch (Exception ex)
            {
                result.AppendLine($"❌ 访问 videos/{videoFileName} 失败: {ex.Message}");
            }

            // 2. 测试完整路径
            try
            {
                using var stream = await FileSystem.OpenAppPackageFileAsync($"Resources/Raw/videos/{videoFileName}");
                if (stream != null)
                {
                    result.AppendLine("✅ 文件存在于 Resources/Raw/videos/ 路径");
                    result.AppendLine($"   文件大小: {stream.Length} bytes");
                }
                else
                {
                    result.AppendLine("❌ 文件不存在于 Resources/Raw/videos/ 路径");
                }
            }
            catch (Exception ex)
            {
                result.AppendLine($"❌ 访问 Resources/Raw/videos/{videoFileName} 失败: {ex.Message}");
            }

            // 3. 列出所有可用的Raw资源
            try
            {
                // 尝试读取emotions.json来验证Raw资源访问
                using var emotionsStream = await FileSystem.OpenAppPackageFileAsync("emotions.json");
                if (emotionsStream != null)
                {
                    result.AppendLine("✅ emotions.json 可以正常访问");
                }
                else
                {
                    result.AppendLine("❌ emotions.json 无法访问");
                }
            }
            catch (Exception ex)
            {
                result.AppendLine($"❌ 访问 emotions.json 失败: {ex.Message}");
            }

            // 4. 生成建议的URI路径
            result.AppendLine("\n=== 建议的URI路径 ===");
            var suggestedPaths = new[]
            {
                $"ms-appx:///Resources/Raw/videos/{videoFileName}",
                $"videos/{videoFileName}",
                $"Resources/Raw/videos/{videoFileName}",
                $"file:///android_asset/Resources/Raw/videos/{videoFileName}"
            };

            foreach (var path in suggestedPaths)
            {
                result.AppendLine($"🔗 {path}");
            }

            return result.ToString();
        }

        public static async Task<List<string>> ListAvailableVideosAsync()
        {
            var availableVideos = new List<string>();
            
            // 从emotions.json获取所有定义的视频文件
            try
            {
                using var stream = await FileSystem.OpenAppPackageFileAsync("emotions.json");
                using var reader = new StreamReader(stream);
                var json = await reader.ReadToEndAsync();
                
                var emotions = System.Text.Json.JsonSerializer.Deserialize<List<EmotionModel>>(json) ?? new();
                
                foreach (var emotion in emotions)
                {
                    var videoFileName = $"{emotion.CmdTag}.mp4";
                    try
                    {
                        using var videoStream = await FileSystem.OpenAppPackageFileAsync($"videos/{videoFileName}");
                        if (videoStream != null)
                        {
                            availableVideos.Add(videoFileName);
                        }
                    }
                    catch
                    {
                        // 文件不存在，跳过
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error listing available videos: {ex.Message}");
            }

            return availableVideos;
        }
    }
}
