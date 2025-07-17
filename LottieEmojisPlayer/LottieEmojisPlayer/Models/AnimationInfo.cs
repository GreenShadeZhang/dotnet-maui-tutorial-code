using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LottieEmojisPlayer.Models
{
    public class AnimationInfo : INotifyPropertyChanged
    {
        private string _version = string.Empty;
        private TimeSpan _duration;
        private double _fps;
        private double _inPoint;
        private double _outPoint;

        public AnimationInfo(string version, TimeSpan duration, double fps, double inPoint, double outPoint)
        {
            Version = version;
            Duration = duration;
            Fps = fps;
            InPoint = inPoint;
            OutPoint = outPoint;
        }

        public string Version
        {
            get => _version;
            private set
            {
                _version = value;
                OnPropertyChanged();
            }
        }

        public TimeSpan Duration
        {
            get => _duration;
            private set
            {
                _duration = value;
                OnPropertyChanged();
            }
        }

        public double Fps
        {
            get => _fps;
            private set
            {
                _fps = value;
                OnPropertyChanged();
            }
        }

        public double InPoint
        {
            get => _inPoint;
            private set
            {
                _inPoint = value;
                OnPropertyChanged();
            }
        }

        public double OutPoint
        {
            get => _outPoint;
            private set
            {
                _outPoint = value;
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
