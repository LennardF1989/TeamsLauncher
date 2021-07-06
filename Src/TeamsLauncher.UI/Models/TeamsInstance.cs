using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using TeamsLauncher.UI.Annotations;

namespace TeamsLauncher.UI.Models
{
    public class TeamsInstance : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Icon
        {
            get => _icon;
            set
            {
                if (value == _icon) return;
                _icon = value;
                OnPropertyChanged();

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
                OnPropertyChanged();
            }
        }

        public string Alias
        {
            get => _alias;
            set
            {
                if (value == _alias) return;
                _alias = value;
                OnPropertyChanged();
            }
        }

        public bool Startup
        {
            get => _startup;
            set
            {
                if (value == _startup) return;
                _startup = value;
                OnPropertyChanged();
            }
        }

        [XmlIgnore]
        public bool HasIcon => Icon != null;

        private string _displayName;
        private string _alias;
        private bool _startup;
        private string _icon;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
