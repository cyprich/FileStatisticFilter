using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FileStatisticsFilter.CommonLibrary;
using Microsoft.Win32;

namespace FileStatisticsFilter.WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static readonly SearchedFiles SearchedFiles = new SearchedFiles();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void UpdateUi()
        {
            var files = SearchedFiles.Files;

            MainDataGrid.ItemsSource = SearchedFiles.Files;

            FilesTextBlock.Text = files.Length.ToString();
            DirectoriesTextBlock.Text = "?";
            SizeTextBlock.Text = SearchedFile.GetReadableSize(files.Sum(f => f.Size)).ToString();
            OldestCreatedTextBlock.Text = files.MinBy(f => f.CreatedTime)?.CreatedTime.ToLongDateString();
            NewestCreatedTextBlock.Text = files.MaxBy(f => f.CreatedTime)?.CreatedTime.ToLongDateString();
            OldestModifiedTextBlock.Text = files.MinBy(f => f.ModifiedTime)?.ModifiedTime.ToLongDateString();
            NewestModifiedTextBlock.Text = files.MaxBy(f => f.ModifiedTime)?.ModifiedTime.ToLongDateString();
            ReadonlyTextBlock.Text = files.Count(f => f.IsReadOnly).ToString();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog()
            {
                Filter = "CSV files (*.csv)|*.csv",
                CheckFileExists = true
            };

            if (openFileDialog.ShowDialog() == true)
            {
                var file = openFileDialog.FileName;
                SearchedFiles.LoadFromCsv(new FileInfo(file));
                UpdateUi();
            }
        }
    }
}