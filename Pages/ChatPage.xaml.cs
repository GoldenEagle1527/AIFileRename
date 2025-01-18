using AiFileNameTranslation;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace AIFileRename
{
    /// <summary>
    /// ChatPage.xaml 的交互逻辑
    /// </summary>
    public partial class ChatPage : Page
    {
        public ChatPage()
        {
            InitializeComponent();
        }

        private async void ChatToAI(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
                return;
            string question = UserInput.Text.Trim();
            if (!string.IsNullOrEmpty(question))
            {
                UserInput.Text = "正则表达式如何匹配";
                Mask.Visibility = Visibility.Visible;
                try
                {
                    ProgressText.Content = $"正在尝试连接{MainWindow.SettingPage.ChatModelName}";
                    AISend send = new(MainWindow.SettingPage.ChatApiKey, MainWindow.SettingPage.ChatBaseUrl, MainWindow.SettingPage.ChatModelName, MainWindow.SettingPage.ChatSystemPrompt);
                    string answer = await send.GetOpenAIAnswer(MainWindow.SettingPage.ChatSystemPrompt, question);
                    ChatView.Text += $"用户：{question}\n{MainWindow.SettingPage.ChatModelName}：{answer}\n";
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
    }
}