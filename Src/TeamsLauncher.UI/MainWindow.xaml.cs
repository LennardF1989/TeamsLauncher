using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using TeamsLauncher.UI.Annotations;
using TeamsLauncher.UI.Models;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;
using XmlConvert = TeamsLauncher.UI.Helpers.XmlConvert;

namespace TeamsLauncher.UI
{
    public partial class MainWindow : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        public ObservableCollection<TeamsInstance> Instances { get; set; }

        public TeamsInstance SelectedInstance
        {
            get => _selectedInstance;
            set
            {
                if (Equals(value, _selectedInstance)) return;
                _selectedInstance = value;
                OnPropertyChanged(nameof(SelectedInstance));
                
                OnPropertyChanged(nameof(IsInstanceSelected));
            }
        }

        public bool IsInstanceSelected => SelectedInstance != null;

        private static readonly string TITLE = "TeamsLauncher UI";
        private static readonly string RESOURCE_ICON = "pack://application:,,,/app.ico";
        private static readonly string FILE_INSTANCES = "instances.cfg";
        private static readonly string FILE_CONFIGURATION = "configuration.xml";

        private NotifyIcon _notifyIcon;
        private ContextMenuStrip _contextMenuStrip;
        private TeamsInstance _selectedInstance;

        public MainWindow()
        {
            SetupWindow();
            LoadTeamsInstances();
            SetupTrayIcon();

            LaunchTeams();

            InitializeComponent();

            Title = $"{TITLE} {GetVersion()}";
        }

        private void SetupWindow()
        {
            Icon = new BitmapImage(new Uri(RESOURCE_ICON));

            Visibility = Visibility.Hidden;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            Closing += (sender, args) =>
            {
                Visibility = Visibility.Hidden;

                args.Cancel = true;
            };
        }

        private void SetupTrayIcon()
        {
            var iconStream = Application.GetResourceStream(new Uri(RESOURCE_ICON));

            _contextMenuStrip = new ContextMenuStrip();

            UpdateMenuStrip();

            _notifyIcon = new NotifyIcon
            {
                Icon = new Icon(iconStream.Stream),
                Text = TITLE,
                ContextMenuStrip = _contextMenuStrip,
                Visible = true
            };

            _notifyIcon.DoubleClick += (sender, args) =>
            {
                Visibility = Visibility.Visible;
            };
        }

        private void UpdateMenuStrip()
        {
            _contextMenuStrip.Items.Clear();

            foreach (var teamsInstance in Instances)
            {
                Image image = null;

                try
                {
                    image = Image.FromFile(teamsInstance.Icon);
                }
                catch
                {
                    //Do nothing
                }

                _contextMenuStrip.Items.Add(teamsInstance.DisplayName, image, (sender, args) =>
                {
                    LaunchTeams(teamsInstance.Alias);
                });
            }

            if (Instances.Any())
            {
                _contextMenuStrip.Items.Add(new ToolStripSeparator());
            }

            _contextMenuStrip.Items.Add("Configure", null, (sender, args) =>
            {
                Visibility = Visibility.Visible;
            });

            _contextMenuStrip.Items.Add("Exit", null, (sender, args) =>
            {
                Application.Current.Shutdown();
            });
        }

        private void LoadTeamsInstances()
        {
            var teamsInstances = 
                XmlConvert.DeserializeObject<List<TeamsInstance>>(FILE_CONFIGURATION) 
                ?? new List<TeamsInstance>();
            
            Instances = new ObservableCollection<TeamsInstance>(teamsInstances);
        }

        private void LaunchTeams(string teamsInstanceAlias = null)
        {
            TeamsHelper.Start(teamsInstanceAlias);
        }

        private string GetVersion()
        {
            Version version = Assembly.GetCallingAssembly().GetName().Version;

            return $"v{version.Major}.{version.Minor}.{version.Build}";
        }

        private void BrowseIconButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedInstance == null)
            {
                return;
            }

            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Title = "Select an image to use as an icon"
            };

            var result = openFileDialog.ShowDialog();

            if (result.HasValue && result.Value)
            {
                SelectedInstance.Icon = openFileDialog.FileName;
            }
            else
            {
                SelectedInstance.Icon = null;
            }
        }

        private void AddNewButton_Click(object sender, RoutedEventArgs e)
        {
            var newInstance = new TeamsInstance
            {
                Icon = null,
                DisplayName = "Default",
                Alias = "Default",
                Startup = false
            };

            Instances.Add(newInstance);

            SelectedInstance = newInstance;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedInstance == null)
            {
                return;
            }

            var confirmResult = MessageBox.Show(
                "Are you sure you want to delete the selected instance?",
                "Delete instance",
                MessageBoxButton.YesNo, 
                MessageBoxImage.Question
            );

            if (confirmResult != MessageBoxResult.Yes)
            {
                return;
            }

            Instances.Remove(_selectedInstance);
        }

        private void MoveUpButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedInstance == null)
            {
                return;
            }

            int index = Instances.IndexOf(SelectedInstance);

            Instances.Move(index, (index - 1) % Instances.Count);
        }

        private void MoveDownButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedInstance == null)
            {
                return;
            }

            int index = Instances.IndexOf(SelectedInstance);

            Instances.Move(index, (index + 1) % Instances.Count);
        }

        private void ApplyAndSaveButton_Click(object sender, RoutedEventArgs e)
        {
            XmlConvert.SerializeObject(FILE_CONFIGURATION, Instances.ToList());

            var instances = Instances
                .Where(x => x.Startup)
                .Select(x => x.Alias)
                .ToArray();

            File.WriteAllLines(FILE_INSTANCES, instances);

            UpdateMenuStrip();
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
