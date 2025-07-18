using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActressLibrary.Extensions
{
    public static class ImageSourceExtensions
    {
        /// <summary>
        /// 从字节数组创建ImageSource
        /// </summary>
        /// <param name="imageBytes">图片字节数组</param>
        /// <returns>ImageSource对象</returns>
        public static ImageSource FromByteArray(byte[] imageBytes)
        {
            if (imageBytes == null || imageBytes.Length == 0)
                return null;

            return ImageSource.FromStream(() => new MemoryStream(imageBytes));
        }

        /// <summary>
        /// 从流读取字节数组
        /// </summary>
        /// <param name="stream">输入流</param>
        /// <returns>字节数组</returns>
        public static async Task<byte[]> ToByteArrayAsync(Stream stream)
        {
            if (stream == null)
                return null;

            using (var memoryStream = new MemoryStream())
            {
                await stream.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// 从流读取字节数组（同步版本）
        /// </summary>
        /// <param name="stream">输入流</param>
        /// <returns>字节数组</returns>
        public static byte[] ToByteArray(Stream stream)
        {
            if (stream == null)
                return null;

            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
