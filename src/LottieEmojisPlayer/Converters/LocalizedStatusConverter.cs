using LottieEmojisPlayer.Services;
using System.ComponentModel;
using System.Globalization;

namespace LottieEmojisPlayer.Converters;

public class LocalizedStatusConverter : IValueConverter, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public LocalizedStatusConverter()
    {
        LocalizationService.Instance.PropertyChanged += (sender, e) => 
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Convert)));
        };
    }

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isPlaying)
        {
            var statusFormat = LocalizationService.Instance["StatusFormat"];
            var currentCulture = LocalizationService.Instance.CurrentCulture.Name;
            
            string statusText;
            if (currentCulture.StartsWith("zh"))
            {
                statusText = isPlaying ? "播放中" : "已停止";
            }
            else
            {
                statusText = isPlaying ? "Playing" : "Stopped";
            }
            
            return string.Format(statusFormat, statusText);
        }
        return string.Empty;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
