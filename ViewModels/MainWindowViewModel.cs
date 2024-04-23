using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;

namespace Awake.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private bool _isInitialized = false;

        [ObservableProperty]
        private string _applicationTitle = String.Empty;

        [ObservableProperty]
        private ObservableCollection<INavigationControl> _navigationItems = new();

        [ObservableProperty]
        private ObservableCollection<INavigationControl> _navigationFooter = new();

        [ObservableProperty]
        private ObservableCollection<MenuItem> _trayMenuItems = new();

        public MainWindowViewModel(INavigationService navigationService)
        {
            initialize.获取程序同目录路径();
            initialize.CheckDirectory();//对类库中的创建工作目录进行初始化
            initialize.CheckCommandline();//对类库中的检查保存的的路径进行加载
            initialize.CheckStartPathFile();//对读取设置的启动目录路径进行初始化
            initialize.相册计数();//获取相册数量
            if (!_isInitialized)
                InitializeViewModel();

        }

        private void InitializeViewModel()
        {
            ApplicationTitle = "光源AI绘画盒子    开源AI绘画辅助工具";

            NavigationItems = new ObservableCollection<INavigationControl>
            {
                new NavigationItem()
                {
                    Width = 140,
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                    Content = "盒子首页",
                    PageTag = "dashboard",
                    Icon = SymbolRegular.Desktop24,
                    PageType = typeof(Views.Pages.DashboardPage)
                },
                //                new NavigationItem()
                //{
                //    Width = 140,
                //    HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                //    Content = "作品灵感",
                //    PageTag = "a2",
                //    Icon = SymbolRegular.DeveloperBoard24,
                //    PageType = typeof(Views.Pages.a2)
                //},

                new NavigationItem()
                {
                    Width = 140,
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                    Content = "高级选项",
                    PageTag = "data",
                    Icon = SymbolRegular.Wrench24,
                    PageType = typeof(Views.Pages.DataPage)
                },
                new NavigationItem()
                {
                    Width = 140,
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                    Content = "安装中心",
                    PageTag = "web",
                    Icon = SymbolRegular.WrenchScrewdriver24,
                    PageType = typeof(Views.Pages.webpp)

                }
            };
            //NavigationItem _change_ui_color = new NavigationItem()
            //{
            //    Width = 140,
            //    HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
            //    Content = "主题切换",
            //    PageTag = "Toggle theme",
            //    Icon = SymbolRegular.WeatherSunny16,

            //};


            NavigationFooter = new ObservableCollection<INavigationControl>
            {


                new NavigationItem()
                {
                    Width = 140,
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                    Content = "关于盒子",
                    PageTag = "settings",
                    Icon = SymbolRegular.Info24,
                    PageType = typeof(Views.Pages.SettingsPage)
                }
            };

            TrayMenuItems = new ObservableCollection<MenuItem>
            {
                new MenuItem
                {
                    Header = "Home",
                    Tag = "tray_home"
                }
            };

            _isInitialized = true;


        }


    }
}
