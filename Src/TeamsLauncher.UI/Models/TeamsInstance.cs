using System.ComponentModel;
using System.Xml.Serialization;
using TeamsLauncher.UI.Annotations;

namespace TeamsLauncher.UI.Models
{
    public class TeamsInstance : INotifyPropertyChanged
    {
        public string Icon
        {
            get => _icon;
            set
            {
                if (value == _icon) return;
                _icon = value;
                OnPropertyChanged(nameof(Icon));

                OnPropertyChanged(nameof(HasIcon));
            }
        }

        public string DisplayName
        {
            get => _displayName;
            set
            {
                if (value == _displayName) return;
                _displayName = value;
                OnPropertyChanged(nameof(DisplayName));
            }
        }

        public string Alias
        {
            get => _alias;
            set
            {
                if (value == _alias) return;
                _alias = value;
                OnPropertyChanged(nameof(Alias));
            }
        }

        public bool Startup
        {
            get => _startup;
            set
            {
                if (value == _startup) return;
                _startup = value;
                OnPropertyChanged(nameof(Startup));
            }
        }

        [XmlIgnore]
        public bool HasIcon => Icon != null;

        private string _displayName;
        private string _alias;
        private bool _startup;
        private string _icon;
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
