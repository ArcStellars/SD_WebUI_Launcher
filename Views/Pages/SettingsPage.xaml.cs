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

        private void 晴岚Horizon的观测枢_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://pd.qq.com/s/g4et2xo0m") { UseShellExecute = true });

        }

        private void 反馈群_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("") { UseShellExecute = true });
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