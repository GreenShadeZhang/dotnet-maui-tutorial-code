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

                Debug.WriteLine($"从Repository获取到 {ret?.Count ?? 0} 条数据");

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
                            Debug.WriteLine($"为 {item.Name} 创建了ImageSource，字节数组长度: {avatarBytes.Length}");
                        }
                        else
                        {
                            Debug.WriteLine($"警告: {item.Name} 没有头像数据");
                        }

                        list.Add(temp);
                    }
                }

                Debug.WriteLine($"准备添加 {list.Count} 条数据到UI集合");

                // 确保在主线程上更新UI
                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    // 方法1: 逐个添加到现有集合
                    Infos.Clear();
                    foreach (var info in list)
                        Infos.Add(info);
                    
                    Debug.WriteLine($"UI集合已更新，当前数量: {Infos.Count}");
                    
                    // 强制通知属性更改（为了解决Windows平台的潜在问题）
                    OnPropertyChanged(nameof(Infos));
                    
                    // 方法2: 如果方法1不行，可以尝试创建新的ObservableCollection
                    // Infos = new ObservableCollection<PersonalInfoDto>(list);
                });

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

        [ObservableProperty]
        ObservableCollection<PersonalInfoDto> infos = new();
    }
}
