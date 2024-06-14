using Awake.Views.Pages;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;
using static Awake.initialize;
using Application = System.Windows.Application;

namespace Awake.Views.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : INavigationWindow
    {
        public ViewModels.MainWindowViewModel ViewModel
        {
            get;
        }

        public MainWindow(ViewModels.MainWindowViewModel viewModel, IPageService pageService, INavigationService navigationService)
        {
            try
            {
                ViewModel = viewModel;
                DataContext = this;

                InitializeComponent();
                Loaded += (sender, args) =>
                {
                    Wpf.Ui.Appearance.Watcher.Watch(
                        this,                                  // Window class
                        Wpf.Ui.Appearance.BackgroundType.Mica, // Background type
                        true                                   // Whether to change accents automatically
                    );
                };
                GetSystemInfo();
                CheckStartPathFile();//从log读取工作路径

                async void GetSystemInfo()
                {

                    string gpuname = await Task.Run(() => hardinfo.getGPUNamelist());//这个函数内部对GPU写入了List,仅return GPUname供展示
                    try
                    {
                        _UseGPUindex = initialize.显卡列表.Count - 1;//默认启动后选最后一张GPU，核显什么的排在前面
                    }
                    catch (Exception error)
                    {
                        File.WriteAllText(@".\logs\error.txt", error.Message.ToString());
                    }

                    //计算机名称类型.Text = "系统名称：" + Machinename + "   系统类型：" + systemType;
                    //计算机内存信息.Text = "内存信息：" + memorynum + " 插槽" + "  共计" + memorysize + " GB";
                    //计算机显卡信息.Text = "显卡信息：" + gpuname;

                    Read_setting();    //读取配置

                    // 读取工作路径然后展示
                    initialize.CheckStartPathFile();
                    initialize.CheckgitPathFile();
                    initialize.CheckVENVPathFile();
                    initialize.Checkstartpath_local();

                }


                if (File.Exists(@".AI_launther_log\UIpicture.txt"))
                {
                    // 如果文件存在，读取其中的内容到UI全局变量中
                    initialize.背景颜色 = File.ReadAllText(@".AI_launther_log\UI.txt");
                }
                else
                {
                    initialize.背景颜色 = "None";
                    basewindow.WindowBackdropType = BackgroundType.None;
                    Theme.Apply(ThemeType.Dark);
                }
                if (File.Exists(@".AI_launther_log\UIpicture.txt"))
                {
                    // 如果文件存在，读取其中的内容到UI全局变量中
                    initialize.背景图片 = File.ReadAllText(@".AI_launther_log\UIpicture.txt");
                }
                else
                {
                    initialize.背景图片 = "";
                }
                if (File.Exists(@".AI_launther_log\UIalpha.txt"))
                {
                    string alphaStringent = File.ReadAllText(@".AI_launther_log\UIalpha.txt");
                    initialize.背景亮度 = int.Parse(alphaStringent);
                    initialize.图片亮度 = int.Parse(alphaStringent);
                    图片亮度.Value = int.Parse(alphaStringent);
                }
                else
                {
                    string alphaStringent = "100";
                    initialize.背景亮度 = int.Parse(alphaStringent);
                    initialize.图片亮度 = int.Parse(alphaStringent);
                    图片亮度.Value = int.Parse(alphaStringent);
                }

                if (背景颜色 == "Mica")
                {
                    if (File.Exists(@".AI_launther_log\UIalpha.txt"))
                    {
                        basewindow.WindowBackdropType = BackgroundType.Mica;
                        string alphaStringent = File.ReadAllText(@".AI_launther_log\UIalpha.txt");
                        图片亮度.Value = int.Parse(alphaStringent);
                        主题背景图.Opacity = 图片亮度.Value / 100;
                        Mica.IsChecked = true;
                    }
                }
                else if (背景颜色 == "Acrylic")
                {
                    if (File.Exists(@".AI_launther_log\UIalpha.txt"))
                    {
                        basewindow.WindowBackdropType = BackgroundType.Acrylic;
                        string alphaStringent = File.ReadAllText(@".AI_launther_log\UIalpha.txt");
                        图片亮度.Value = int.Parse(alphaStringent);
                        主题背景图.Opacity = 图片亮度.Value / 100;
                        Acrylic.IsChecked = true;
                    }
                }
                else if (背景颜色 == "Tabbed")
                {
                    if (File.Exists(@".AI_launther_log\UIalpha.txt"))
                    {
                        basewindow.WindowBackdropType = BackgroundType.Tabbed;
                        string alphaStringent = File.ReadAllText(@".AI_launther_log\UIalpha.txt");
                        图片亮度.Value = int.Parse(alphaStringent);
                        主题背景图.Opacity = 图片亮度.Value / 100;
                        Tabbed.IsChecked = true;
                    }
                }
                else if (背景颜色 == "Auto")
                {
                    if (File.Exists(@".AI_launther_log\UIalpha.txt"))
                    {
                        basewindow.WindowBackdropType = BackgroundType.Auto;
                        string alphaStringent = File.ReadAllText(@".AI_launther_log\UIalpha.txt");
                        图片亮度.Value = int.Parse(alphaStringent);
                        主题背景图.Opacity = 图片亮度.Value / 100;
                        Auto.IsChecked = true;
                    }
                }
                else if (背景颜色 == "None")
                {
                    if (File.Exists(@".AI_launther_log\UIalpha.txt"))
                    {
                        basewindow.WindowBackdropType = BackgroundType.None;
                        string alphaStringent = File.ReadAllText(@".AI_launther_log\UIalpha.txt");
                        图片亮度.Value = int.Parse(alphaStringent);
                        主题背景图.Opacity = 图片亮度.Value / 100;
                        None.IsChecked = true;
                    }
                }
                else if (背景颜色 == "Picture")
                {
                    if (File.Exists(@".AI_launther_log\UIalpha.txt"))
                    {
                        basewindow.WindowBackdropType = BackgroundType.None;
                        None.IsChecked = true;
                        string alphaStringent = File.ReadAllText(@".AI_launther_log\UIalpha.txt");
                        图片亮度.Value = int.Parse(alphaStringent);
                        主题背景图.Opacity = 图片亮度.Value / 100;
                        string imagepath = initialize.背景图片; // 获取选择的文件路径+文件名  
                        ImageSource imageSource = new BitmapImage(new Uri(imagepath)); // 设置Image的ImageSource为选择的图片  
                        主题背景图.ImageSource = imageSource; // 将选择的图片显示在Image控件中  

                    }
                }



                SetPageService(pageService);

                navigationService.SetNavigationControl(RootNavigation);

                _loadpage();
                //在启动的时候判断是否下载与安装SDwebUI
                initialize.已下载WebUI = initialize.CheckWebUIdownloaded();
                initialize.已安装WebUI = initialize.CheckWebUIinstelled();
                initialize.已解压WebUI = initialize.CheckWebUIunzip();
            }
            catch (Exception error)
            {
                File.WriteAllText(@".\logs\error.txt", error.Message.ToString());
            }
        }
        async Task _loadpage()
        {
            try
            {

                // 加载页的背景图计时器
                await Task.Delay(TimeSpan.FromSeconds(2.5));

                // 在UI线程中更改grid的visibility  
                Dispatcher.Invoke(() =>
                {
                    // 创建动画  
                    DoubleAnimation doubleAnimation = new DoubleAnimation();
                    doubleAnimation.From = 1; // 从100%开始  
                    doubleAnimation.To = 0; // 结束时为0%  
                    doubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(1)); // 动画时间为1秒  
                    doubleAnimation.AutoReverse = false; // 动画结束后自动反转回原来的状态  

                    // 将动画应用到grid的透明度属性上  
                    开屏画面.BeginAnimation(Grid.OpacityProperty, doubleAnimation);
                });
                主显示区.Visibility = Visibility.Visible;
                // 在UI线程中更改grid的visibility  
                Dispatcher.Invoke(() =>
                {
                    // 创建动画  
                    DoubleAnimation doubleAnimation = new DoubleAnimation();
                    doubleAnimation.From = 0; // 从0%开始  
                    doubleAnimation.To = 1; // 结束时为100%  
                    doubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(1)); // 动画时间为1秒  
                    doubleAnimation.AutoReverse = false; // 动画结束后自动反转回原来的状态  

                    // 将动画应用到grid的透明度属性上  
                    主显示区.BeginAnimation(Grid.OpacityProperty, doubleAnimation);
                });
                开屏画面.Visibility = Visibility.Collapsed;
            }
            catch (Exception error)
            {
                File.WriteAllText(@".\logs\error.txt", error.Message.ToString());
            }

        }

        #region INavigationWindow methods

        public System.Windows.Controls.Frame GetFrame()
            => RootFrame;

        public INavigation GetNavigation()
            => RootNavigation;

        public bool Navigate(Type pageType)
            => RootNavigation.Navigate(pageType);

        public void SetPageService(IPageService pageService)
            => RootNavigation.PageService = pageService;

        public void ShowWindow()
            => Show();

        public void CloseWindow()
            => Close();

        #endregion INavigationWindow methods
        protected override void OnClosed(EventArgs e)
        {

            base.OnClosed(e);
            File.WriteAllText(@".AI_launther_log\UIalpha.txt", initialize.图片亮度.ToString());
            File.WriteAllText(@".AI_launther_log\UI.txt", initialize.背景颜色);
            File.WriteAllText(@".AI_launther_log\UIpicture.txt", initialize.背景图片);
            string 参数设置 = (
                initialize.浏览器启动 + "\n" +
                initialize.启动api + "\n" +
                initialize.分享WebUI到公网 + "\n" +
                initialize.使用CPU进行推理 + "\n" +
                initialize.关闭模型hash计算 + "\n" +
                initialize.冻结设置 + "\n" +
                initialize.快速启动 + "\n" +
                initialize.启用自定义路径 + "\n" +

                initialize.上投采样 + "\n" +
                initialize.关闭半精度计算 + "\n" +

                initialize.启用InvokeAI + "\n" +
                initialize.内存优化 + "\n" +

                initialize.SDP优化 + "\n" +
                initialize.缩放点积 + "\n" +
                initialize.启用xformers + "\n" +
                initialize.启用替代布局 + "\n" +

                initialize.显卡类型名 + "\n" +
                initialize._显卡类型 + "\n" +
                initialize._WebUI显存压力优化设置 + "\n" +
                initialize._WebUI主题颜色 + "\n" +

                initialize.Doggettx优化 + "\n"



                );


            File.WriteAllText(@".AI_launther_log\setting.txt", 参数设置);
            Application.Current.Shutdown();
        }

        private void 一键启动按钮_Click(object sender, RoutedEventArgs e)
        {
            //检查WebUI安装状态
            initialize.已下载WebUI = initialize.CheckWebUIdownloaded();
            initialize.已安装WebUI = initialize.CheckWebUIinstelled();
            initialize.已解压WebUI = initialize.CheckWebUIunzip();

            //通过对是否安装webUI已安装的判断做出提示安装或直接启动的操作
            if (initialize.已安装WebUI == false)
            {
                //探出flyout提示用户去安装
                安装提示.Show();
            }
            if (initialize.已安装WebUI == true)
            {
                //AI,启动！
                shell Shell = new shell();//shell会自动读取initilize中的参数变量
                Shell.Show();
            }
        }
        private void 主题切换_Click(object sender, RoutedEventArgs e)
        {
            主题设置区.Show();
        }
        //下面是主题效果
        private void Mica_Click(object sender, RoutedEventArgs e)
        {
            try//Win10不怎么支持Win11的先进特效
            {
                basewindow.WindowBackdropType = BackgroundType.Mica;
                主题背景图.Opacity = 图片亮度.Value / 100;
                背景颜色 = "Mica";
            }
            catch { }
        }
        private void Acrylic_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                basewindow.WindowBackdropType = BackgroundType.Acrylic;
                主题背景图.Opacity = 图片亮度.Value / 100;
                背景颜色 = "Acrylic";
            }
            catch { }

        }

        private void Tabbed_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                basewindow.WindowBackdropType = BackgroundType.Tabbed;
                主题背景图.Opacity = 图片亮度.Value / 100;
                背景颜色 = "Tabbed";
            }
            catch { }

        }
        private void Auto_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                basewindow.WindowBackdropType = BackgroundType.Auto;
                主题背景图.Opacity = 图片亮度.Value / 100;
                背景颜色 = "Auto";
            }
            catch { }
        }

        private void None_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                basewindow.WindowBackdropType = BackgroundType.None;
                主题背景图.Opacity = 图片亮度.Value / 100;
                背景颜色 = "None";
            }
            catch { }

        }

        private void Picture_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                basewindow.WindowBackdropType = BackgroundType.None;
                主题背景图.Opacity = 图片亮度.Value / 100;
                string imagepath = 背景图片; // 获取选择的文件路径+文件名  
                ImageSource imageSource = new BitmapImage(new Uri(imagepath)); // 设置Image的ImageSource为选择的图片  
                主题背景图.ImageSource = imageSource; // 将选择的图片显示在Image控件中
                背景颜色 = "Picture";
            }
            catch
            {

                System.Windows.MessageBox.Show("请选择一张图片");

            }
        }

        private void 图片亮度_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            主题背景图.Opacity = 图片亮度.Value / 100;
            initialize.图片亮度 = (int)图片亮度.Value;
            背景亮度 = (int)图片亮度.Value;
        }

        private void 图片亮度_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            主题背景图.Opacity = 图片亮度.Value / 100;
            initialize.图片亮度 = (int)图片亮度.Value;
            背景亮度 = (int)图片亮度.Value;
        }

        private void 图片亮度_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            主题背景图.Opacity = 图片亮度.Value / 100;
            initialize.图片亮度 = (int)图片亮度.Value;
            背景亮度 = (int)图片亮度.Value;
        }

        private void 选择背景图片_Click(object sender, RoutedEventArgs e)
        {

            initialize.图片亮度 = (int)图片亮度.Value;
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp; *.png)|*.jpg; *.jpeg; *.gif; *.bmp; *.png"; // 只允许选择图片文件  
            if (ofd.ShowDialog() == true) // 用户选择了一个文件并点击了“确定”按钮  
            {
                背景图片 = ofd.FileName;
                string imagepath2 = ofd.FileName; // 获取选择的文件路径+文件名  
                ImageSource imageSource = new BitmapImage(new Uri(imagepath2)); // 设置Image的ImageSource为选择的图片  
                主题背景图.ImageSource = imageSource; // 将选择的图片显示在Image控件中  
            }
            basewindow.WindowBackdropType = BackgroundType.None;
            None.IsChecked = true;
            主题背景图.Opacity = 图片亮度.Value / 100;
            背景颜色 = "Picture";
            string imagepath = 背景图片; // 获取选择的文件路径+文件名  
            try
            {
                ImageSource imageSource = new BitmapImage(new Uri(imagepath)); // 设置Image的ImageSource为选择的图片  
                主题背景图.ImageSource = imageSource; // 将选择的图片显示在Image控件中  

            }
            catch
            {
                System.Windows.MessageBox.Show("请先选择一张图片");

            }
        }

        private void Dark_Click(object sender, RoutedEventArgs e)
        {
            Theme.Apply(ThemeType.Dark);
        }

        private void Light_Click(object sender, RoutedEventArgs e)
        {
            Theme.Apply(ThemeType.Light);
        }

        private void 明暗切换_Click(object sender, RoutedEventArgs e)
        {
            if (Theme.GetAppTheme() == ThemeType.Dark)
            {
                Theme.Apply(ThemeType.Light);
            }

            else
            {
                Theme.Apply(ThemeType.Dark);
            }

        }

        private void SD启动_Click(object sender, RoutedEventArgs e)
        {
            if (_SD启动 == false)
            {
                _SD启动 = true;
                SD启动.IsChecked = true;
            }
            else
            {
                _SD启动 = false;
                SD启动.IsChecked = false;
            }

        }
    }


}