using LottieEmojisPlayer.Services;

namespace LottieEmojisPlayer.Extensions;

[ContentProperty(nameof(Key))]
public class TranslateExtension : IMarkupExtension
{
    public string Key { get; set; } = string.Empty;

    public object ProvideValue(IServiceProvider serviceProvider)
    {
        return LocalizationService.Instance[Key];
    }
}
