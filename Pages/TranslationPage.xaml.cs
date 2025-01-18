using AiFileNameTranslation;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AIFileRename
{
    /// <summary>
    /// TranslationPage.xaml 的交互逻辑
    /// </summary>
    public partial class TranslationPage : Page
    {
        List<string> filePaths = [];
        ObservableCollection<FileItem> fileItems = [];
        ObservableCollection<RegularItem> regularItems = [];

        public TranslationPage()
        {
            InitializeComponent();
            FileList.ItemsSource = fileItems;
            RegularList.ItemsSource = regularItems;
        }

        private async void Prompt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
                return;
            string question = Prompt.Text.Trim();
            if (fileItems.Count > 0 && !string.IsNullOrEmpty(question))
            {
                try
                {
                    Mask.Visibility = Visibility.Visible;
                    StartButton.Content = "翻译中";
                    ProgressText.Content = $"正在尝试连接{MainWindow.SettingPage.TranslationModelName}";
                    AISend send = new(MainWindow.SettingPage.TranslationApiKey, MainWindow.SettingPage.TranslationBaseUrl, MainWindow.SettingPage.TranslationModelName, MainWindow.SettingPage.TranslationSystemPrompt);
                    foreach (FileItem item in fileItems)
                    {
                        string answer = await send.GetOpenAIAnswer(Prompt.Text, Path.GetFileNameWithoutExtension(item.OriginalFileName));
                        item.AIRenamedFileName = $"{answer.Trim()}{Path.GetExtension(item.OriginalFileName)}";
                        item.RegularRenamedFileName = item.AIRenamedFileName;
                        ProgressText.Content = $"正在翻译文件名 {item.OriginalFileName}=>{item.AIRenamedFileName}";
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
                    Mask.Visibility = Visibility.Hidden;
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
                    fileItems.Add(new FileItem(fileInfo.Name, fileInfo.Name, fileInfo.Name));
                }
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                for (int i = 0; i < filePaths.Count; i++)
                {
                    string oldFilePath = filePaths[i];
                    string newFileName = fileItems[i].RegularRenamedFileName;
                    string newFilePath = Path.Combine(Path.GetDirectoryName(oldFilePath), newFileName);

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
                string newFileName = fileItems[i].AIRenamedFileName;
                int count = 1;
                for (int j = 0; j < fileItems.Count; j++)
                {
                    if (i != j && newFileName == fileItems[j].AIRenamedFileName)
                    {
                        fileItems[j].AIRenamedFileName = $"{Path.GetFileNameWithoutExtension(fileItems[j].AIRenamedFileName)} ({count}){Path.GetExtension(fileItems[j].AIRenamedFileName)}";
                        count++;
                    }
                }
            }
        }

        private void AddRegularButton_Click(object sender, RoutedEventArgs e)
        {
            regularItems.Add(new RegularItem(string.Empty, string.Empty, true));
            RegularTip.Visibility = regularItems.Count > 0 ? Visibility.Hidden : Visibility.Visible;
        }

        private void DeleteRegularButton_Click(object sender, RoutedEventArgs e)
        {
            int index = RegularList.SelectedIndex;
            if (index != -1)
                regularItems.RemoveAt(index);
            RegularTip.Visibility = regularItems.Count > 0 ? Visibility.Hidden : Visibility.Visible;
        }

        private void ApplyRegularButton_Click(object sender, RoutedEventArgs e)
        {
            if (regularItems.Count == 0)
                return;
            foreach (FileItem item in fileItems)
            {
                string sourceName = item.AIRenamedFileName;

                foreach (RegularItem regularItem in regularItems)
                {
                    if (regularItem.RegularExpressionCheck)
                    {
                        try
                        {
                            sourceName = Regex.Replace(sourceName, regularItem.RegularExpression, regularItem.RegularReplace);
                        }
                        catch
                        {
                            MessageBox.Show($"正则表达式错误: {regularItem.RegularExpression}");
                        }
                    }
                }

                item.RegularRenamedFileName = sourceName;
            }
            FileList.Items.Refresh();
        }
    }
}