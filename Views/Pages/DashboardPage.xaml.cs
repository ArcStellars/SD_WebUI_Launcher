using Awake.Views.Windows;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using static Microsoft.Web.WebView2.Core.DevToolsProtocolExtension.CSS;

namespace Awake.Views.Pages
{
    public partial class DashboardPage/* : INavigableView<ViewModels.DashboardViewModel>*/
    {
        public ViewModels.DashboardViewModel ViewModel
        {
            get;
        }

        string _searchName = "";//搜索名称
        int 模型排列 = 0;//模型排列优先级状态量
        string 模型种类 = "[]";//默认所有模型种类
        int 加载页数 = 1;//第一页的模型
        private string 模型名称;
        private string 模型封面地址;
        private string 模型作者名称;
        private string 模型作者头像地址;
        private string 模型种类名称;
        private string 模型UUID;


        private readonly List<string> _imagePaths;
        private DispatcherTimer _timer;
        private Random _random = new Random();

        public DashboardPage(ViewModels.DashboardViewModel viewModel)
        {
            try
            {
                ViewModel = viewModel;
                InitializeComponent();
                _imagePaths = new List<string> { "pack://application:,,,/Views/Pages/001.png",
                "pack://application:,,,/Views/Pages/002.png",
                "pack://application:,,,/Views/Pages/003.png",
                "pack://application:,,,/Views/Pages/004.png",
                "pack://application:,,,/Views/Pages/005.png",
                "pack://application:,,,/Views/Pages/006.png",
                "pack://application:,,,/Views/Pages/007.png",
                "pack://application:,,,/Views/Pages/008.png",
                "pack://application:,,,/Views/Pages/009.png",
                "pack://application:,,,/Views/Pages/010.png",
                "pack://application:,,,/Views/Pages/011.png",
                "pack://application:,,,/Views/Pages/012.png"
            };
                _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(5) };
                _timer.Tick += TimerTick;
                _timer.Start();
                loatmodel(0, "", "[]", 1);
            }
            catch (Exception ex)
            {
            }

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // 随机选择一张图片作为初始的背景  
            SetRandomImage();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            // 每10秒更换一次背景图片  
            SetRandomImage();
        }

        private void SetRandomImage()
        {
            if (_imagePaths.Any())
            {
                var randomImageIndex = _random.Next(_imagePaths.Count);
                var randomImagePath = _imagePaths[randomImageIndex];
                背景图.ImageSource = new BitmapImage(new Uri(randomImagePath));
            }
        }



        //异步方法避免阻塞UI线程
        async Task loatmodel(int _modelClass, string _searchName, string _modelType, int _modelPage)
        {
            try//避免404等情况崩掉，顺便catch error
            {

                var client = new HttpClient();
                var request = new HttpRequestMessage();
                request.RequestUri = new Uri("https://liblib-api.vibrou.com/api/www/model/search");
                request.Method = HttpMethod.Post;
                //请求字段，由页面导航生成
                request.Headers.Add("Accept", "*/*");
                request.Headers.Add("User-Agent", "Open-SD-WebUI-Launcher");
                //请求热门模型

                var bodyString = "{\"cid\":\"Open-SD-WebUI-Launcher\",\"keyword\":\"" + _searchName + "\",\"time\":\"\",\"models\":" + _modelType + ",\"sort\":\"" + _modelClass + "\",\"page\": \"" + _modelPage + "\"}";
                var content = new StringContent(bodyString, Encoding.UTF8, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);
                var result = await response.Content.ReadAsStringAsync();

                // 获取json中"data""data"下的列表  
                var list = JObject.Parse(result)["data"]["data"] as JArray;
                // 过滤只是模型的元素 (盒子只是提供模型下载，不是提供liblib的运营广告
                var filteredList = list.Where(obj => obj["name"] != null).ToList();
                // 将过滤后的结果转为JSON字符串（如果需要）  
                string filteredJson = JArray.FromObject(filteredList).ToString();
                string json = filteredJson.ToString();
                List<Dictionary<string, object>> cardlist = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(json);
                for (int i = 0; i < cardlist.Count; i++) //for cardlist中的每一个
                {
                    if (cardlist[i]["name"] != null &&
                        cardlist[i]["imageUrl"] != null &&
                         cardlist[i]["nickname"] != null &&
                        cardlist[i]["avatar"] != null &&
                         cardlist[i]["modelTypeName"] != null &&
                         cardlist[i]["uuid"] != null
                        )
                    {
                        模型名称 = cardlist[i]["name"].ToString();//解析模型名称
                        模型封面地址 = cardlist[i]["imageUrl"].ToString();//解析模型封面地址
                        模型作者名称 = cardlist[i]["nickname"].ToString();//解析作者名称
                        模型作者头像地址 = cardlist[i]["avatar"].ToString();//解析作者头像地址
                        模型种类名称 = cardlist[i]["modelTypeName"].ToString();//解析模型种类名称
                        模型UUID = cardlist[i]["uuid"].ToString();
                    }
                    //WPF的bitmap显示机制疑似有点占内存了，所以希望活动在展示列表的模型不超过5次点击刷新(大概120个
                    if (模型资源列表.Children.Count > 120)
                    {
                        模型资源列表.Children.RemoveRange(0, 20);
                    }
                    modelCardshow modelCardshow = new modelCardshow(
          模型名称,
          模型封面地址,
          模型作者名称,
          模型作者头像地址,
          模型种类名称,
          模型UUID);

                    模型资源列表.Children.Add(modelCardshow);
                }


            }
            catch (Exception ex)//捕获异常拿去调试或者控制404一类的UI动画逻辑
            {


            }

        }


        private void 一键启动按钮_Click(object sender, RoutedEventArgs e)
        {

            shell WindowMain = new shell();//运行终端
            WindowMain.Show();

        }

        void 搜索框_GotFocus(object sender, RoutedEventArgs e)
        {
            _searchName = 搜索框.Text;
        }


        private void charListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void 搜素按钮_Click(object sender, RoutedEventArgs e)
        {
            if (搜索框.Text != null)
            {
                模型资源列表.Children.Clear();
                _searchName = 搜索框.Text;
                loatmodel(模型排列, _searchName, 模型种类, 1);
            }
        }
        private void 搜素按钮2_Click(object sender, RoutedEventArgs e)
        {
            if (搜索框.Text != null)
            {
                模型资源列表.Children.Clear();
                _searchName = 搜索框.Text;
                loatmodel(模型排列, _searchName, 模型种类, 1);
            }
        }


        private void 最新_Click(object sender, RoutedEventArgs e)
        {
            模型资源列表.Children.Clear();
            模型排列 = 1;
            loatmodel(模型排列, _searchName, 模型种类, 1);
        }

        private void 最热_Click(object sender, RoutedEventArgs e)
        {
            模型资源列表.Children.Clear();
            模型排列 = 2;
            loatmodel(模型排列, _searchName, 模型种类, 1);
        }


        private void 推荐_Click(object sender, RoutedEventArgs e)
        {
            模型资源列表.Children.Clear();
            模型排列 = 0;
            loatmodel(模型排列, _searchName, 模型种类, 1);
        }



        private void 模型类型选择器_DropDownClosed(object sender, EventArgs e)
        {
            if (模型类型选择器.SelectedIndex == 0)
            {
                模型资源列表.Children.Clear();
                模型种类 = "[]";
                loatmodel(模型排列, _searchName, 模型种类, 1);
            }
            if (模型类型选择器.SelectedIndex == 1)
            {
                模型资源列表.Children.Clear();
                模型种类 = "[5]";
                loatmodel(模型排列, _searchName, 模型种类, 1);
            }
            if (模型类型选择器.SelectedIndex == 2)
            {
                模型资源列表.Children.Clear();
                模型种类 = "[6]";
                loatmodel(模型排列, _searchName, 模型种类, 1);
            }
            if (模型类型选择器.SelectedIndex == 3)
            {
                模型资源列表.Children.Clear();
                模型种类 = "[7]";
                loatmodel(模型排列, "", 模型种类, 1);
            }
            if (模型类型选择器.SelectedIndex == 4)
            {
                模型资源列表.Children.Clear();
                模型种类 = "[1]";
                loatmodel(模型排列, _searchName, 模型种类, 1);
            }
            if (模型类型选择器.SelectedIndex == 5)
            {
                模型资源列表.Children.Clear();
                模型种类 = "[2]";
                loatmodel(模型排列, _searchName, 模型种类, 1);
            }

        }

        private void banner1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://www.aigodlike.com/") { UseShellExecute = true });
        }

        private void 搜索框_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            _searchName = 搜索框.Text;
        }

        private void 搜索框_LostFocus(object sender, RoutedEventArgs e)
        {
            _searchName = 搜索框.Text;
        }
        private void 搜索框2_LostFocus(object sender, RoutedEventArgs e)
        {
            _searchName = 搜索框.Text;
        }

        private void 加载更多_Click(object sender, RoutedEventArgs e)
        {

            loatmodel(模型排列, _searchName, 模型种类, 加载页数 += 1);

        }

        private void 搜索栏_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            搜索栏2.Visibility = Visibility.Visible;
        }

        //这里是希望在屏幕前的你翻模型列表时把滚动搜索栏滑动到不可见区域时new 一个悬浮搜索栏
        private void 总滚动列表_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (总滚动列表.VerticalOffset > 430)
            {
                // 如果  （搜索栏） 的高度大于  （总滚动列表）  的视口高度，则将搜索栏2的可见性设置为 Visible。  

                搜索栏2.Visibility = Visibility.Visible;


            }
            else
            {

                搜索栏2.Visibility = Visibility.Collapsed;

            }
        }

        private void 模型类型选择器2_DropDownClosed(object sender, EventArgs e)

        {
            if (模型类型选择器.SelectedIndex == 0)
            {
                模型资源列表.Children.Clear();
                模型种类 = "[]";
                loatmodel(模型排列, _searchName, 模型种类, 1);
            }
            if (模型类型选择器.SelectedIndex == 1)
            {
                模型资源列表.Children.Clear();
                模型种类 = "[5]";
                loatmodel(模型排列, _searchName, 模型种类, 1);
            }
            if (模型类型选择器.SelectedIndex == 2)
            {
                模型资源列表.Children.Clear();
                模型种类 = "[6]";
                loatmodel(模型排列, _searchName, 模型种类, 1);
            }
            if (模型类型选择器.SelectedIndex == 3)
            {
                模型资源列表.Children.Clear();
                模型种类 = "[7]";
                loatmodel(模型排列, "", 模型种类, 1);
            }
            if (模型类型选择器.SelectedIndex == 4)
            {
                模型资源列表.Children.Clear();
                模型种类 = "[1]";
                loatmodel(模型排列, _searchName, 模型种类, 1);
            }
            if (模型类型选择器.SelectedIndex == 5)
            {
                模型资源列表.Children.Clear();
                模型种类 = "[2]";
                loatmodel(模型排列, _searchName, 模型种类, 1);
            }

        }

        string programpath = System.Windows.Forms.Application.StartupPath.Substring(0, System.Windows.Forms.Application.StartupPath.IndexOf(':'));
        //获取执行文件所在盘符

        private void firstloded(object sender, RoutedEventArgs e)
        {
            搜索栏2.Visibility = Visibility.Collapsed;
        }
        private void 打开WebUI文件夹_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Process.Start("explorer.exe", initialize.工作路径);
        }

        private void 打开文生图文件夹_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (programpath.Length == 1)
            {
                string 文生图文件路径 = initialize.工作路径 + "outputs\\txt2img-images";
                Process.Start("explorer.exe", 文生图文件路径);
            }
            else
            {
                string 文生图文件路径 = initialize.工作路径 + "\\outputs\\txt2img-images";
                Process.Start("explorer.exe", 文生图文件路径);
            }
        }

        private void 打开图生图文件夹_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (programpath.Length == 1)
            {
                string 图生图文件路径 = initialize.工作路径 + "outputs\\img2img-images";
                Process.Start("explorer.exe", 图生图文件路径);
            }
            else
            {
                string 图生图文件路径 = initialize.工作路径 + "\\outputs\\img2img-images";
                Process.Start("explorer.exe", 图生图文件路径);
            }

        }

        private void 统计生成图片数量_MouseDown(object sender, MouseButtonEventArgs e)
        {
            initialize.相册计数();
            图片数量展示.Text = initialize.相册图片数量;
        }

        private void 打开SD模型文件夹_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (programpath.Length == 1)
            {
                string SD模型文件路径 = initialize.工作路径 + "models\\Stable-diffusion";
                Process.Start("explorer.exe", SD模型文件路径);
            }
            else
            {
                string SD模型文件路径 = initialize.工作路径 + "\\models\\Stable-diffusion";
                Process.Start("explorer.exe", SD模型文件路径);
            }

        }

        private void 打开lora模型文件夹_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (programpath.Length == 1)
            {
                string LORA模型文件路径 = initialize.工作路径 + "models\\Lora";
                Process.Start("explorer.exe", LORA模型文件路径);
            }
            else
            {
                string LORA模型文件路径 = initialize.工作路径 + "\\models\\Lora";
                Process.Start("explorer.exe", LORA模型文件路径);
            }

        }

        private void 打开VAE模型文件夹_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (programpath.Length == 1)
            {
                string VAE模型文件路径 = initialize.工作路径 + "models\\VAE";
                Process.Start("explorer.exe", VAE模型文件路径);
            }
            else
            {
                string VAE模型文件路径 = initialize.工作路径 + "\\models\\VAE";
                Process.Start("explorer.exe", VAE模型文件路径);
            }


        }

        private void 打开EMB模型文件夹_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (programpath.Length == 1)
            {
                string EMB模型文件路径 = initialize.工作路径 + "embeddings";
                Process.Start("explorer.exe", EMB模型文件路径);
            }
            else
            {
                string EMB模型文件路径 = initialize.工作路径 + "\\embeddings";
                Process.Start("explorer.exe", EMB模型文件路径);
            }


        }

        private void 打开HYP模型文件夹_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (programpath.Length == 1)
            {
                string HYP模型文件路径 = initialize.工作路径 + "models\\hypernetworks";
                Process.Start("explorer.exe", HYP模型文件路径);
            }
            else
            {
                string HYP模型文件路径 = initialize.工作路径 + "\\models\\hypernetworks";
                Process.Start("explorer.exe", HYP模型文件路径);
            }


        }

        private void 打开扩展模型文件夹_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (programpath.Length == 1)
            {
                string 扩展模型文件夹 = initialize.工作路径 + "extensions";
                Process.Start("explorer.exe", 扩展模型文件夹);
            }
            else
            {
                string 扩展模型文件夹 = initialize.工作路径 + "\\extensions";
                Process.Start("explorer.exe", 扩展模型文件夹);
            }


        }

        private void _02_MouseDown(object sender, MouseButtonEventArgs e)
        {


        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://www.aigodlike.com/") { UseShellExecute = true });
        }


    }

}