using Awake.Views.Windows;
using System;
using System.IO;
using System.Net.Http;
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

        string 模型_UUID = "";
        string _nickname = "";
        string _avatar = "";
        string _modelType = "";
        string _imageURL = "";
        public modelCardshow(string modelname, string imageUrl, string nickname, string avatar, string modelTypeName, string uuid)
        {
            InitializeComponent();


            //模型名称.Text = modelName;
            LoadImageFromUrlAsync(imageUrl, avatar);
            async Task LoadImageFromUrlAsync(string imageUrl, string avatar)
            {


                // 将图片设置为Image控件的Source

                try
                {
                    HttpClient client1 = new HttpClient();
                    try
                    {
                        var imageBytes1 = await client1.GetByteArrayAsync(imageUrl);
                        var image1 = new BitmapImage();
                        image1.BeginInit();
                        image1.CacheOption = BitmapCacheOption.OnLoad;
                        image1.CreateOptions = BitmapCreateOptions.DelayCreation;
                        image1.StreamSource = new MemoryStream(imageBytes1);
                        image1.EndInit();
                        //BitmapImage 模型封面图 = new BitmapImage(new Uri(imageUrl));
                        模型封面.ImageSource = image1;
                    }
                    catch (Exception error)
                    {
                        string str1 = error.Message;
                        File.WriteAllText(@".\logs\error.txt", str1);
                        throw;
                    }


                }
                catch (System.IO.IOException ex)
                {
                    string str1 = ex.Message;
                    File.WriteAllText(@".\logs\error.txt", str1);
                    throw;

                }

                try
                {
                    HttpClient client1 = new HttpClient();

                    var imageBytes1 = await client1.GetByteArrayAsync(avatar);
                    try
                    {
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
                        string str1 = error.Message;
                        File.WriteAllText(@".\logs\error.txt", str1);
                        throw;
                    }


                    //BitmapImage _avatarimage= new BitmapImage(new Uri(avatar));


                }
                catch (
             System.IO.IOException ex)
                {
                    string str1 = ex.Message;
                    File.WriteAllText(@".\logs\error.txt", str1);
                    throw;
                }

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

        private void _modelCard_Click(object sender, RoutedEventArgs e)
        {
            Model_content model_Content = new Model_content(模型_UUID, _nickname, _avatar, _modelType, _imageURL);//继续传递模型的UUID参数，以便在详情页中继续请求简介/下载地址等
            model_Content.Show();
        }
    }
}
