using System;
using System.IO;

namespace ActressLibrary.Helpers
{
    /// <summary>
    /// 图片处理辅助类
    /// </summary>
    public static class ImageHelper
    {
        /// <summary>
        /// 从字节数组创建ImageSource
        /// </summary>
        /// <param name="imageBytes">图片字节数组</param>
        /// <returns>ImageSource对象，如果字节数组为空或null则返回null</returns>
        public static ImageSource CreateImageSource(byte[] imageBytes)
        {
            if (imageBytes == null || imageBytes.Length == 0)
            {
                System.Diagnostics.Debug.WriteLine("ImageHelper: 图片字节数组为空");
                return null;
            }

            try
            {
                System.Diagnostics.Debug.WriteLine($"ImageHelper: 创建ImageSource，字节数组长度: {imageBytes.Length}");
                return ImageSource.FromStream(() => new MemoryStream(imageBytes));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ImageHelper: 创建ImageSource失败: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 从流读取字节数组
        /// </summary>
        /// <param name="stream">输入流</param>
        /// <returns>字节数组</returns>
        public static byte[] StreamToByteArray(Stream stream)
        {
            if (stream == null)
                return null;

            using (var memoryStream = new MemoryStream())
            {
                stream.Position = 0;
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// 异步从流读取字节数组
        /// </summary>
        /// <param name="stream">输入流</param>
        /// <returns>字节数组</returns>
        public static async Task<byte[]> StreamToByteArrayAsync(Stream stream)
        {
            if (stream == null)
                return null;

            using (var memoryStream = new MemoryStream())
            {
                stream.Position = 0;
                await stream.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// 验证图片字节数组是否有效
        /// </summary>
        /// <param name="imageBytes">图片字节数组</param>
        /// <returns>是否有效</returns>
        public static bool IsValidImageBytes(byte[] imageBytes)
        {
            if (imageBytes == null || imageBytes.Length < 8)
                return false;

            // 检查常见的图片文件头
            // PNG: 89 50 4E 47 0D 0A 1A 0A
            if (imageBytes.Length >= 8 && 
                imageBytes[0] == 0x89 && imageBytes[1] == 0x50 && 
                imageBytes[2] == 0x4E && imageBytes[3] == 0x47)
                return true;

            // JPEG: FF D8 FF
            if (imageBytes.Length >= 3 && 
                imageBytes[0] == 0xFF && imageBytes[1] == 0xD8 && imageBytes[2] == 0xFF)
                return true;

            // GIF: 47 49 46 38
            if (imageBytes.Length >= 4 && 
                imageBytes[0] == 0x47 && imageBytes[1] == 0x49 && 
                imageBytes[2] == 0x46 && imageBytes[3] == 0x38)
                return true;

            // BMP: 42 4D
            if (imageBytes.Length >= 2 && 
                imageBytes[0] == 0x42 && imageBytes[1] == 0x4D)
                return true;

            return false;
        }
    }
}
