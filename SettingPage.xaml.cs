using AiFileNameTranslation;
using System;
using System.Collections.Generic;
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
    /// SettingPage.xaml 的交互逻辑
    /// </summary>
    public partial class SettingPage : Page
    {
        public string ApiKey { get => ApiKeyInput.Text; }
        public string BaseUrl { get => UrlInput.Text; }
        public string ModelName { get => ModelNameInput.Text; }
        public string SystemPrompt { get => SystemPromptInput.Text; }
        readonly string savePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "save.dat");

        public SettingPage()
        {
            InitializeComponent();

            Application.Current.ThemeMode = MainWindow.isDarkTheme ? ThemeMode.Dark : ThemeMode.Light;
            LoadSettings();
        }

        private void LoadSettings()
        {
            try
            {
                if (File.Exists(savePath))
                {
                    string[] lines = File.ReadAllLines(savePath);
                    ApiKeyInput.Text = lines[0];
                    UrlInput.Text = lines[1];
                    ModelNameInput.Text = lines[2];
                    SystemPromptInput.Text = lines[3];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载设置失败：" + ex.Message);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using StreamWriter writer = new(savePath);
                writer.WriteLine(ApiKey);
                writer.WriteLine(BaseUrl);
                writer.WriteLine(ModelName);
                writer.WriteLine(SystemPrompt);
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败：" + ex.Message);
            }
        }
    }
}