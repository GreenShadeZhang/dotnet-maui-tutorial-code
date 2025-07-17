using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LottieEmojisPlayer.Models
{
    public class LottieFileItem : INotifyPropertyChanged
    {
        private string _name = string.Empty;
        private string _filePath = string.Empty;
        private bool _isSelected;

        public LottieFileItem(string name, string filePath)
        {
            Name = name;
            FilePath = filePath;
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public string FilePath
        {
            get => _filePath;
            set
            {
                _filePath = value;
                OnPropertyChanged();
            }
        }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
