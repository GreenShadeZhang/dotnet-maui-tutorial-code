using ActressLibrary.Interfaces;
using ActressLibrary.Models;
using ActressLibrary.Helpers;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActressLibrary.ViewModels
{
    [QueryProperty("Person", "Person")]
    public partial class DetailViewModel : BaseViewModel
    {
        private readonly IPersonalInfoRepository _repository;

        private PersonalInfoDto _infoDto;

        public DetailViewModel(IPersonalInfoRepository repository)
        {
            _repository = repository;
        }

        public PersonalInfoDto InfoDto
        {
            get
            {
                return _infoDto;
            }
            set
            {
                SetProperty(ref _infoDto, value);
            }
        }

        [ObservableProperty]
        PersonalInfoDto person;

        public async Task InitializeAsync()
        {
            var ret = await _repository.GetAsync(Person.Name);

            if (ret != null)
            {
                var temp = new PersonalInfoDto
                {
                    Name = ret.Name,
                    Desc = ret.Desc,
                    Hobbies = ret.Hobbies,
                    Tags = ret.Tags
                };

                // 使用字节数组而不是流，更优雅更高效
                var avatarBytes = ret.GetAvatarBytes();
                if (avatarBytes != null && avatarBytes.Length > 0)
                {
                    temp.AvatarBitmap = avatarBytes;
                    temp.ImageSource = ImageHelper.CreateImageSource(avatarBytes);
                }

                InfoDto = temp;
            }
        }
    }
}
