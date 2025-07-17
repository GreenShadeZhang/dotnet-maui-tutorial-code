using System.ComponentModel;
using System.Globalization;
using System.Resources;

namespace LottieEmojisPlayer.Services;

public class LocalizationService : INotifyPropertyChanged
{
    private static LocalizationService? _instance;
    public static LocalizationService Instance => _instance ??= new LocalizationService();

    private readonly ResourceManager _resourceManager;
    private CultureInfo _currentCulture = CultureInfo.CurrentUICulture;

    public event PropertyChangedEventHandler? PropertyChanged;

    private LocalizationService()
    {
        _resourceManager = new ResourceManager("LottieEmojisPlayer.Resources.Languages.AppResources", typeof(LocalizationService).Assembly);
    }

    public CultureInfo CurrentCulture
    {
        get => _currentCulture;
        set
        {
            if (_currentCulture != value)
            {
                _currentCulture = value;
                CultureInfo.CurrentUICulture = value;
                CultureInfo.CurrentCulture = value;
                
                // 通知所有绑定的属性更新 - 使用空字符串通知所有属性
                OnPropertyChanged(string.Empty);
            }
        }
    }

    public string this[string key]
    {
        get
        {
            var value = _resourceManager.GetString(key, _currentCulture);
            return value ?? key;
        }
    }

    public List<CultureInfo> SupportedCultures { get; } = new()
    {
        new CultureInfo("en"),
        new CultureInfo("zh-CN")
    };

    private void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
