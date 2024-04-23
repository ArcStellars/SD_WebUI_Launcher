using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Numerics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Wpf.Ui.Appearance;
using static Awake.initialize;

namespace Awake.Views.Windows
{
    /// <summary>
    /// model_content.xaml 的交互逻辑
    /// </summary>
    public partial class Model_content

    {
        string _UUID = "";
        string _模型名称 = "";
        string _模型封面地址 = "";
        string _模型作者名称 = "";
        string _模型作者头像地址 = "";
        string _模型种类名称 = "";

        string _modelType = "";
        string _modelname = "";
        string _modelVersionId = "";
        string _modelSource = "";
        string _modelSourceName = "";
        string _modelSourceSize = "";
        string _modelSourceHash = "";
        string _versionDesc = "";


        public Model_content(string uuid, string nickname, string avatar, string modelType, string imageURL)
        {


            InitializeComponent();
            if (initialize.背景颜色 == "Mica")
            {
                modelwindow.WindowBackdropType = BackgroundType.Mica;
                图片亮度.Value = initialize.图片亮度;
                主题背景图.Opacity = 图片亮度.Value / 100;


            }
            if (initialize.背景颜色 == "Acrylic")
            {
                modelwindow.WindowBackdropType = BackgroundType.Acrylic;
                图片亮度.Value = initialize.图片亮度;
                主题背景图.Opacity = 图片亮度.Value / 100;


            }
            if (initialize.背景颜色 == "Tabbed")
            {
                modelwindow.WindowBackdropType = BackgroundType.Tabbed;
                图片亮度.Value = initialize.图片亮度;
                主题背景图.Opacity = 图片亮度.Value / 100;
            }


            if (initialize.背景颜色 == "Auto")
            {
                modelwindow.WindowBackdropType = BackgroundType.Auto;
                图片亮度.Value = initialize.图片亮度;
                主题背景图.Opacity = 图片亮度.Value / 100;

            }
            if (initialize.背景颜色 == "None")
            {
                modelwindow.WindowBackdropType = BackgroundType.None;
                图片亮度.Value = initialize.图片亮度;
                主题背景图.Opacity = 图片亮度.Value / 100;

            }
            if (initialize.背景颜色 == "Picture")
            {
                modelwindow.WindowBackdropType = BackgroundType.None;
                图片亮度.Value = initialize.图片亮度;
                主题背景图.Opacity = 图片亮度.Value / 100;
                string imagepath = initialize.背景图片; // 获取选择的文件路径+文件名  
                ImageSource imageSource = new BitmapImage(new Uri(imagepath)); // 设置Image的ImageSource为选择的图片  
                主题背景图.ImageSource = imageSource; // 将选择的图片显示在Image控件中  


            }
            //从拿到的UUID创建post请求
            _UUID = uuid;
            _模型作者名称 = nickname;
            _模型封面地址 = imageURL;
            _模型作者头像地址 = avatar;
            _模型种类名称 = modelType;
            _modelType = modelType;
            作者名称.Text = "模型作者：" + _模型作者名称;
            模型类型.Text = "模型类型：" + _模型种类名称;
            _GetModelDetals(_UUID);//开始执行异步任务
            LoadImageFromUrlAsync(imageURL, avatar);
        }
        async Task LoadImageFromUrlAsync(string imageUrl, string avatar)//开始加载模型封面图和作者头像
        {
            try
            {
                HttpClient client1 = new HttpClient();

                var imageBytes1 = await client1.GetByteArrayAsync(imageUrl);
                var image1 = new BitmapImage();
                image1.BeginInit();
                image1.CacheOption = BitmapCacheOption.OnLoad;
                image1.CreateOptions = BitmapCreateOptions.DelayCreation;
                image1.StreamSource = new MemoryStream(imageBytes1);
                image1.EndInit();
                模型封面.ImageSource = image1;


            }
            catch (Exception ex)
            {

            }
            try
            {
                HttpClient client1 = new HttpClient();

                var imageBytes1 = await client1.GetByteArrayAsync(avatar);
                var image1 = new BitmapImage();
                image1.BeginInit();
                image1.CacheOption = BitmapCacheOption.OnLoad;
                image1.CreateOptions = BitmapCreateOptions.DelayCreation;
                image1.StreamSource = new MemoryStream(imageBytes1);
                image1.EndInit();
                作者头像.ImageSource = image1;
            }
            catch (
            Exception ex)
            {

            }
        }
        async Task _GetModelDetals(string uuid)
        {
            try//避免404等情况崩掉，顺便catch error
            {
                string _Uri = "https://liblib-api.vibrou.com/api/www/model/getByUuid/" + uuid;
                var client = new HttpClient();
                var request = new HttpRequestMessage();
                request.RequestUri = new Uri(_Uri);
                request.Method = HttpMethod.Post;

                request.Headers.Add("Accept", "*/*");
                request.Headers.Add("User-Agent", "Open-SD-WebUI-Launcher");

                var response = await client.SendAsync(request);
                var result = await response.Content.ReadAsStringAsync();
                //返回值.Text = result;

                //开始解析返回值
                var JObjectlist = JObject.Parse(result)["data"] as JObject;//这个用来读取模型的一些基础信息
                _模型名称 = JObjectlist["name"].ToString();

                模型名称.Text = _模型名称.ToString();

                string 模型富文本简介内容 = JObjectlist["remark"].ToString();
                if (模型富文本简介内容 == "")
                {
                    简介.Visibility = Visibility.Visible;
                    模型富文本简介内容 = "作者暂未介绍模型";
                    string utf8text = "meta charset =\"UTF-8\"";//使用UTF8格式化富文本
                    string htmlContent = "<" + utf8text + ">" + 模型富文本简介内容;
                    富文本简介.Source = new Uri("data:text/html," + htmlContent);
                    简介.Visibility = Visibility.Visible;

                }
                else
                {

                    string utf8text = "meta charset =\"UTF-8\"";//使用UTF8格式化富文本
                    string htmlContent = "<" + utf8text + ">" + 模型富文本简介内容;
                    富文本简介.Source = new Uri("data:text/html," + htmlContent);
                    简介.Visibility = Visibility.Visible;
                }


                //开始解析模型的版本有多少个，并分别展示
                var JArraylist = JObject.Parse(result)["data"]["versions"] as JArray;//这个用来解析模型的多个版本

                string json = JArraylist.ToString();

                List<Dictionary<string, object>> infolist = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(json);
                try
                {
                    for (int i = 0; i < infolist.Count; i++)
                    {

                        _modelname = infolist[i]["name"].ToString();
                        _versionDesc = infolist[i]["versionDesc"].ToString();
                        string _Attachment = infolist[i]["attachment"].ToString();//这是一个嵌套的{}对象，这样解析


                        var JObjectlist3 = JObject.Parse(_Attachment) as JObject;//这个用来读取模型的一些基础信息
                        _modelVersionId = JObjectlist3["modelVersionId"].ToString();
                        _modelSourceName = JObjectlist3["modelSourceName"].ToString();
                        _modelSource = JObjectlist3["modelSource"].ToString();
                        string size = JObjectlist3["modelSourceSize"].ToString();
                        BigInteger numsize = BigInteger.Parse(size);//单位化为MB
                        if (numsize < 1024)
                        {
                            _modelSourceSize = numsize.ToString() + "B";
                        }
                        if (numsize < 1024 * 1024 && numsize > 1024)
                        {
                            _modelSourceSize = (numsize / 1024).ToString() + "KB";
                        }
                        if (numsize < 1024 * 1024 * 1024 && numsize > 1024 * 1024)
                        {
                            _modelSourceSize = (numsize / 1024 / 1024).ToString() + "MB";
                        }
                        if (numsize > 1024 * 1024 * 1024)
                        {
                            _modelSourceSize = (numsize / 1024 / 1024 / 1024).ToString() + "GB";
                        }
                        _modelSourceHash = JObjectlist3["modelSourceHash"].ToString();
                        model_download_list model_Download_List = new model_download_list(_versionDesc, _UUID, _模型名称, _modelname, _modelVersionId, _modelSourceName, _modelSource, _modelSourceSize, _modelSourceHash, _modelType);
                        模型下载列表.Children.Add(model_Download_List);
                    }
                }
                catch
                {
                    报错指示器.Visibility = Visibility.Visible;


                }

            }
            catch (Exception ex)//捕获异常拿去调试或者控制404一类的UI动画逻辑
            {
                System.Windows.MessageBox.Show(ex.Message);//开发中调试窗口
            }
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            modelCardshow.允许打开模型 = true;
            介绍.Child = null;
        }

    }

}
