using ActressLibrary.Interfaces;
using ActressLibrary.Models;
using ActressLibrary.Helpers;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace ActressLibrary.ViewModels
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

        [RelayCommand]
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

                        // 使用字节数组而不是流，更优雅更高效
                        var avatarBytes = item.GetAvatarBytes();
                        if (avatarBytes != null && avatarBytes.Length > 0)
                        {
                            temp.AvatarBitmap = avatarBytes;
                            temp.ImageSource = ImageHelper.CreateImageSource(avatarBytes);
                        }

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
                await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
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
