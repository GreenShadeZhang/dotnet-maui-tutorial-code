using LottieEmojisPlayer.ViewModels;
using LottieEmojisPlayer.Services;
using System.Globalization;

namespace LottieEmojisPlayer
{
    public partial class MainPage : ContentPage
    {
        private MainPageViewModel? _viewModel;

        public MainPage()
        {
            InitializeComponent();
            _viewModel = BindingContext as MainPageViewModel;
            
            // 设置 LottieView 引用到 ViewModel
            if (_viewModel != null)
            {
                _viewModel.LottieView = LottieView;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            
            // 确保 ViewModel 有 LottieView 引用
            if (_viewModel?.LottieView == null)
            {
                if (_viewModel != null)
                {
                    _viewModel.LottieView = LottieView;
                }
            }
        }

        private async void OnLanguageButtonClicked(object sender, EventArgs e)
        {
            var localizationService = LocalizationService.Instance;
            
            var languages = new List<string>
            {
                "English",
                "中文"
            };
            
            var cultures = new List<CultureInfo>
            {
                new CultureInfo("en"),
                new CultureInfo("zh-CN")
            };

            var result = await DisplayActionSheet(
                localizationService["Language"], 
                "Cancel", 
                null, 
                languages.ToArray());

            if (!string.IsNullOrEmpty(result) && result != "Cancel")
            {
                var selectedIndex = languages.IndexOf(result);
                if (selectedIndex >= 0)
                {
                    localizationService.CurrentCulture = cultures[selectedIndex];
                }
            }
        }
    }
}
