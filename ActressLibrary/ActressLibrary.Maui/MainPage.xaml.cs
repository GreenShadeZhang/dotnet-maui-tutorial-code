using ActressLibrary.Core.Interfaces;
using ActressLibrary.Core.Models;
using ActressLibrary.Maui.Models;
using ActressLibrary.Maui.Pages;
using ActressLibrary.Maui.ViewModels;

namespace ActressLibrary.Maui;

public partial class MainPage : ContentPage
{
    private readonly IPersonalInfoRepository _repository;

    MainViewModel vm => BindingContext as MainViewModel;

    public MainPage(IPersonalInfoRepository repository, MainViewModel vm)
    {
        InitializeComponent();

        _repository = repository;

        BindingContext = vm;
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

