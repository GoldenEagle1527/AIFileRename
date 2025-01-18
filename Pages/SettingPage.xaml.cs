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
        public string TranslationApiKey { get => TranslationApiKeyInput.Text; }
        public string TranslationBaseUrl { get => TranslationUrlInput.Text; }
        public string TranslationModelName { get => TranslationModelNameInput.Text; }
        public string TranslationSystemPrompt { get => TranslationSystemPromptInput.Text; }
        public string ChatApiKey { get => ChatApiKeyInput.Text; }
        public string ChatBaseUrl { get => ChatUrlInput.Text; }
        public string ChatModelName { get => ChatModelNameInput.Text; }
        public string ChatSystemPrompt { get => ChatSystemPromptInput.Text; }
        readonly string savePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "save.dat");

        public SettingPage()
        {
            InitializeComponent();
            LoadSettings();
        }

        private void LoadSettings()
        {
            try
            {
                if (File.Exists(savePath))
                {
                    string[] lines = File.ReadAllLines(savePath);
                    TranslationApiKeyInput.Text = lines[0];
                    TranslationUrlInput.Text = lines[1];
                    TranslationModelNameInput.Text = lines[2];
                    TranslationSystemPromptInput.Text = lines[3];
                    ChatApiKeyInput.Text = lines[4];
                    ChatUrlInput.Text = lines[5];
                    ChatModelNameInput.Text = lines[6];
                    ChatSystemPromptInput.Text = lines[7];
                }
            }
            catch
            {
                MessageBox.Show($"加载设置失败\n请删除{savePath}以重置设置");
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using StreamWriter writer = new(savePath);
                writer.WriteLine(TranslationApiKey);
                writer.WriteLine(TranslationBaseUrl);
                writer.WriteLine(TranslationModelName);
                writer.WriteLine(TranslationSystemPrompt);
                writer.WriteLine(ChatApiKey);
                writer.WriteLine(ChatBaseUrl);
                writer.WriteLine(ChatModelName);
                writer.WriteLine(ChatSystemPrompt);
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败：" + ex.Message);
            }
        }
    }
}