using Awake.Views.Windows;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Awake
{
    /// <summary>
    /// modelCardshow.xaml 的交互逻辑
    /// </summary>
    public partial class modelCardshow : UserControl
    {
        public static bool 允许打开模型 = true;

        private bool check_url;
        string 模型_UUID = "";
        string _nickname = "";
        string _avatar = "";
        string _modelType = "";
        string _imageURL = "";
        public modelCardshow(string modelname, string imageUrl, string nickname, string avatar, string modelTypeName, string uuid)
        {
            InitializeComponent();

            try
            {
                HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(avatar);
                myHttpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko";
                myHttpWebRequest.Method = "GET";
                HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
            }
            catch
            { }
            try
            {
                HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(imageUrl);
                myHttpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko";
                myHttpWebRequest.Method = "GET";
                HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
            }
            catch
            { }

            LoadImageFromUrlAsync(imageUrl, avatar);

            async Task LoadImageFromUrlAsync(string imageUrl, string avatar)
            {
                // 将图片设置为Image控件的Source
                try
                {
                    HttpClient client2 = new HttpClient();
                    var imageBytes2 = await client2.GetByteArrayAsync(imageUrl);
                    var image2 = new BitmapImage();
                    image2.BeginInit();
                    image2.CacheOption = BitmapCacheOption.OnLoad;
                    image2.CreateOptions = BitmapCreateOptions.DelayCreation;
                    image2.StreamSource = new MemoryStream(imageBytes2);
                    image2.EndInit();
                    if (check_url == true)
                    {
                        File.WriteAllText(@".\logs\error.txt", check_url.ToString());
                        模型封面.ImageSource = image2;
                    }
                }
                catch (Exception error)
                {
                    File.WriteAllText(@".\logs\error.txt", error.Message.ToString());
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
                catch (Exception error)
                {
                    File.WriteAllText(@".\logs\error.txt", error.Message.ToString());
                    check_url = false;
                }

                模型名称.Text = modelname;
                模型类型.Text = modelTypeName;
                作者名称.Text = nickname;
                模型_UUID = uuid;
                _nickname = nickname;
                _avatar = avatar;
                _modelType = modelTypeName;
                _imageURL = imageUrl;

            }

        }
        private void _modelCard_Click(object sender, RoutedEventArgs e)
        {
            Model_content model_Content = new Model_content(模型_UUID, _nickname, _avatar, _modelType, _imageURL);//继续传递模型的UUID参数，以便在详情页中继续请求简介/下载地址等
            model_Content.Show();
        }
    }
}
