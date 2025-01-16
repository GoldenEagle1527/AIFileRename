using AiFileNameTranslation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace AIFileRename
{
    /// <summary>
    /// TranslationPage.xaml 的交互逻辑
    /// </summary>
    public partial class TranslationPage : Page
    {
        List<string> filePaths = [];
        ObservableCollection<FileItem> fileItems = [];
        bool running;

        public TranslationPage()
        {
            InitializeComponent();
            Application.Current.ThemeMode = MainWindow.isDarkTheme ? ThemeMode.Dark : ThemeMode.Light;
        }

        private async void Prompt_KeyDown(object sender, KeyEventArgs e)
        {
            string question = Prompt.Text.Trim();
            if (e.Key == Key.Enter && fileItems.Count > 0 && !string.IsNullOrEmpty(question))
            {
                try
                {
                    Prompt.IsReadOnly = true;
                    Mask.Visibility = Visibility.Visible;
                    FileList.IsReadOnly = true;
                    StartButton.Content = "翻译中";
                    running = true;
                    AISend send = new(MainWindow.SettingPage.ApiKey, MainWindow.SettingPage.BaseUrl, MainWindow.SettingPage.ModelName, MainWindow.SettingPage.SystemPrompt);
                    foreach (FileItem item in fileItems)
                    {
                        string answer = await send.GetOpenAIAnswer(Prompt.Text, Path.GetFileNameWithoutExtension(item.OriginalFileName));
                        item.RenamedFileName = $"{answer.Trim()}{Path.GetExtension(item.OriginalFileName)}";
                        ProgressText.Text = $"正在翻译文件名 {item.OriginalFileName}=>{item.RenamedFileName}";
                    }
                    EnsureUniqueFileNames();
                    FileList.Items.Refresh();
                    StartButton.Content = "重命名";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex);
                }
                finally
                {
                    Prompt.IsReadOnly = false;
                    Mask.Visibility = Visibility.Hidden;
                    FileList.IsReadOnly = false;
                    running = false;
                }
            }
        }

        private void FileList_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                BackgroundTip.Visibility = Visibility.Hidden;
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (string file in files)
                {
                    FileInfo fileInfo = new(file);
                    filePaths.Add(file);
                    fileItems.Add(new FileItem(fileInfo.Name, fileInfo.Name));
                }
                FileList.ItemsSource = fileItems;
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (!running)
            {
                try
                {
                    for (int i = 0; i < filePaths.Count; i++)
                    {
                        string oldFilePath = filePaths[i];
                        string newFileName = fileItems[i].RenamedFileName;
                        string newFilePath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(oldFilePath), newFileName);

                        if (!File.Exists(newFilePath))
                        {
                            File.Move(oldFilePath, newFilePath);
                        }
                        else
                        {
                            MessageBox.Show($"文件 {newFileName} 已经存在，无法重命名 {oldFilePath}");
                        }
                    }
                    filePaths.Clear();
                    fileItems.Clear();
                    BackgroundTip.Visibility = Visibility.Visible;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        void RemoveSelectItem(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Right)
            {
                int index = FileList.SelectedIndex;
                if (index >= 0)
                {
                    filePaths.RemoveAt(index);
                    fileItems.RemoveAt(index);
                    if (filePaths.Count == 0)
                        BackgroundTip.Visibility = Visibility.Visible;
                }
            }
        }

        private void EnsureUniqueFileNames()
        {
            for (int i = 0; i < fileItems.Count; i++)
            {
                string newFileName = fileItems[i].RenamedFileName;
                int count = 1;
                for (int j = 0; j < fileItems.Count; j++)
                {
                    if (i != j && newFileName == fileItems[j].RenamedFileName)
                    {
                        fileItems[j].RenamedFileName = $"{Path.GetFileNameWithoutExtension(fileItems[j].RenamedFileName)} ({count}){Path.GetExtension(fileItems[j].RenamedFileName)}";
                        count++;
                    }
                }
            }
        }
    }
}