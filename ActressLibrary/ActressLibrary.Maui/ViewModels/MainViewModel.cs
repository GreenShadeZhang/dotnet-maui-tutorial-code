using ActressLibrary.Core.Interfaces;
using ActressLibrary.Maui.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace ActressLibrary.Maui.ViewModels
{
    public partial class MainViewModel : BaseViewModel
    {
        private readonly IPersonalInfoRepository _repository;
        public MainViewModel(IPersonalInfoRepository repository)
        {
            _repository = repository;
        }


        [ObservableProperty]
        bool isRefreshing;

        [CommunityToolkit.Mvvm.Input.RelayCommand]
        async Task LoadActorsAsync()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                var ret = await _repository.GetListAsync(1, 30);

                var list = new List<PersonalInfoDto>();

                if (ret != null)
                {
                    foreach (var item in ret)
                    {
                        var temp = new PersonalInfoDto
                        {
                            Name = item.Name,
                            Desc = item.Desc,
                            Hobbies = item.Hobbies,
                            Tags = item.Tags
                        };
                        temp.ImageSource = ImageSource.FromStream(() => item.AvatarStream);

                        list.Add(temp);
                    }

                }

                if (Infos.Count != 0)
                    Infos.Clear();

                foreach (var info in list)
                    Infos.Add(info);

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to get actors: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error!", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
                IsRefreshing = false;
            }

        }

        private ObservableCollection<PersonalInfoDto> infos = new();

        public ObservableCollection<PersonalInfoDto> Infos
        {
            get
            {
                return infos;
            }
            set
            {
                SetProperty(ref infos, value);
            }
        }
    }
}
