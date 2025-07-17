using LottieEmojisPlayer.ViewModels;

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
    }
}
