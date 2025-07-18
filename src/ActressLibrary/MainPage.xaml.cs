using ActressLibrary.Interfaces;
using ActressLibrary.Models;
using ActressLibrary.Pages;
using ActressLibrary.ViewModels;

namespace ActressLibrary;

public partial class MainPage : ContentPage
{
    private readonly IPersonalInfoRepository _repository;

    private MainViewModel? ViewModel;

    public MainPage(IPersonalInfoRepository repository, MainViewModel vm)
    {
        InitializeComponent();

        _repository = repository;

        ViewModel = vm;
        BindingContext = ViewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        string firstPath = FileSystem.AppDataDirectory + @"\first.json";

        if (!File.Exists(firstPath))
        {
            await using var stream = await FileSystem.OpenAppPackageFileAsync("db-data.json");

            using var reader = new StreamReader(stream);

            var contents = reader.ReadToEnd();

            var dataModel = System.Text.Json.JsonSerializer.Deserialize<PersonalInfoModel>(contents);

            var dataList = dataModel.Data;

            if (dataList != null && dataList.Count > 0)
            {
                foreach (var item in dataList)
                {
                    await using var stream1 = await FileSystem.OpenAppPackageFileAsync($"Images\\{item.AvatarName}");

                    await _repository.AddAsync(item, stream1);
                }
            }

            File.Create(firstPath);
        }

        // 添加测试数据验证UI绑定
        await Task.Delay(500); // 确保UI完全加载
        
        System.Diagnostics.Debug.WriteLine($"MainPage OnAppearing: ViewModel is null? {ViewModel == null}");
        System.Diagnostics.Debug.WriteLine($"MainPage OnAppearing: BindingContext is null? {BindingContext == null}");
        System.Diagnostics.Debug.WriteLine($"MainPage OnAppearing: Infos count: {ViewModel?.Infos?.Count ?? -1}");
        
        // 为了测试UI绑定，添加一些测试数据
        if (ViewModel != null && ViewModel.Infos.Count == 0)
        {
            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                // 添加测试数据
                var testItem = new PersonalInfoDto
                {
                    Name = "测试用户",
                    Desc = "这是一个测试用户，用于验证UI绑定是否正常工作",
                    Hobbies = "测试爱好",
                    Tags = new List<string> { "测试", "UI" }
                };
                
                ViewModel.Infos.Add(testItem);
                System.Diagnostics.Debug.WriteLine($"添加了测试数据，当前Infos数量: {ViewModel.Infos.Count}");
            });
        }
    }

    private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
        var peroson = ((VisualElement)sender).BindingContext as PersonalInfoDto;

        if (peroson == null)
            return;

        await Shell.Current.GoToAsync(nameof(DetailPage), true, new Dictionary<string, object>
        {
            {"Person", peroson }
        });
    }
}

