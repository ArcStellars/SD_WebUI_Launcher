using System.Diagnostics;
using Wpf.Ui.Common.Interfaces;

namespace Awake.Views.Pages
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : INavigableView<ViewModels.SettingsViewModel>
    {
        public ViewModels.SettingsViewModel ViewModel
        {
            get;
        }

        public SettingsPage(ViewModels.SettingsViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
        }

        private void 光源的魔法小镇_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://pd.qq.com/s/g4et2xo0m") { UseShellExecute = true });

        }

        private void 光源的AI魔法小镇_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("http://qm.qq.com/cgi-bin/qm/qr?_wv=1027&k=ir983BIAaQzt3CzQkel_NmJ5wQ1VAoBQ&authKey=U8Dv%2F8YlLk7mAvGQmRxaWjUxn%2FlNvpWdEk%2Bz43SpBwjh2GhnsjHg5ett%2B2%2Bdopbl&noverify=0&group_code=227356139") { UseShellExecute = true });
        }

        private void AIGC炼丹技术交流群_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("http://qm.qq.com/cgi-bin/qm/qr?_wv=1027&k=5Do89k8ZdV67sJcNp-XkhOFg_DguHWP3&authKey=UJ2uCai5vDW75rFvcoLfdjt93FHElFIAn4aHDizgrxza4uTXOARhuLfpcA3rutff&noverify=0&group_code=720697178") { UseShellExecute = true });
        }

        private void NovelAI中文频道_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://pd.qq.com/s/eqo0vw7yi") { UseShellExecute = true });
        }

        private void 参与建设_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://github.com/Ray-Source-X/Open-SD-WebUI-Launcher") { UseShellExecute = true });
        }

        private void 支持光源盒子开发_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://afdian.net/a/Ray_Source") { UseShellExecute = true });
        }

        private void 元素法典_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://docs.qq.com/doc/DWGh4QnZBVlJYRkly") { UseShellExecute = true });
        }

        private void 解构原典_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://docs.qq.com/doc/DR1Z4VkFEZGl4Sk9S") { UseShellExecute = true });
        }
    }
}