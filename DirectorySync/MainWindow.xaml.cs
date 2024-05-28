using System.Reflection;
using System.Text.Json;
using System.Windows;
using DirectorySync.Model;
using DirectorySync.Service;

namespace DirectorySync
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Directories> directories = new List<Directories>();

        public MainWindow()
        {
            InitializeComponent();
            LoadDirectories();

            this.Title = this.Title + " v" + Assembly.GetExecutingAssembly().GetName().Version.ToString(2);
        }

        void LoadDirectories()
        {
            // [{"Source":"c:\\work\\dir1\\","Target":"c:\\work\\dir2\\"}] -- sample json
            string jsonString = System.IO.File.ReadAllText(@"c:\work\syncConfig.json");
            var _directories = JsonSerializer.Deserialize<List<Directories>>(jsonString);

            if (_directories != null)
            {
                directories.AddRange(_directories);
            }

            grid.ItemsSource = directories;
        }

        void SaveDirectories()
        {
            string jsonData = JsonSerializer.Serialize(directories);
            System.IO.File.WriteAllText(@"c:\work\syncConfig.json", jsonData);
        }

        private void btSync_Click(object sender, RoutedEventArgs e)
        {
            directories.ForEach(x => x.SetSyncStatus(""));

            foreach (var d in directories)
            {
                FileService.CopyDirectory(d.Source, d.Target, tb);
                d.SetSyncStatus("completed at " + DateTime.Now.ToLongTimeString());
                grid.ItemsSource = null;
                grid.ItemsSource = directories;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveDirectories();
        }
    }
}