using AiFileNameTranslation;
using System.ComponentModel;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Microsoft.Win32;
using System.Data;
using System.Collections.ObjectModel;

namespace AIFileRename
{
    public partial class MainWindow : Window
    {

        public static bool isDarkTheme;
        public static TranslationPage TranslationPage = null!;
        public static SettingPage SettingPage = null!;
        public static ChatPage ChatPage = null!;

        public MainWindow()
        {
            InitializeComponent();
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize");
            if (key != null)
            {
                int appsUseLightTheme = (int)key.GetValue("AppsUseLightTheme", -1);
                if (appsUseLightTheme == 0)
                {
                    // 当前使用暗色主题
                    ThemeChangeButton.Content = "\xE708";
                    isDarkTheme = true;
                }
                else if (appsUseLightTheme == 1)
                {
                    // 当前使用亮色主题
                    ThemeChangeButton.Content = "\xE706";
                    isDarkTheme = false;
                }
                key.Close();
            }
            TranslationPage = new();
            SettingPage = new();
            ChatPage = new();
            MainFrame.Navigate(TranslationPage);
            Icon = isDarkTheme ? new BitmapImage(new Uri("pack://application:,,,/Icon_Black.png")) : new BitmapImage(new Uri("pack://application:,,,/Icon_White.png"));
        }

        private void ThemeChange(object sender, RoutedEventArgs e)
        {
            ThemeChangeButton.Content = isDarkTheme ? "\xE706" : "\xE708";
            Application.Current.ThemeMode = isDarkTheme ? ThemeMode.Light : ThemeMode.Dark;
            isDarkTheme = !isDarkTheme;
            Icon = isDarkTheme ? new BitmapImage(new Uri("pack://application:,,,/Icon_Black.png")) : new BitmapImage(new Uri("pack://application:,,,/Icon_White.png"));
        }

        private void OpenSettingPage(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(SettingPage);
        }

        private void OpenTranslationPage(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(TranslationPage);
        }

        private void OpenChatPage(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(ChatPage);
        }
    }
}