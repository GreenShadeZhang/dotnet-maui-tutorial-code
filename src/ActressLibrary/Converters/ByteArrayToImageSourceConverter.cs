using System;
using System.Globalization;
using System.IO;

namespace ActressLibrary.Converters
{
    /// <summary>
    /// 字节数组到ImageSource的转换器
    /// </summary>
    public class ByteArrayToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is byte[] imageBytes && imageBytes.Length > 0)
            {
                return ImageSource.FromStream(() => new MemoryStream(imageBytes));
            }
            
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("ConvertBack is not supported for ByteArrayToImageSourceConverter");
        }
    }
}
