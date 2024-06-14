using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
namespace Awake
{
    /// <summary>
    //不使用第三方库写一个简单的.NET6 C# WPF程序，由一个“下载”按钮，一个“取消”按钮与一个“进度条”组成，在按下“下载”按钮时开始下载，开始下载后按下“下载”按钮暂停，暂停后按下“下载”按钮继续下载之前暂停的同一任务，按下取消按钮后取消之前的下载任务并删除下载缓存.在下载过程中将下载进度显示到进度条
    /// </summary>
    public partial class model_download_list : UserControl
    {

        public delegate void ProgressBarSetter(double value);
        public void SetProgressBar(double value)
        {
            progressBar.Value = value;

        }

        public string speedInfo = "";

        string downloadUrl = string.Empty;
        string modelNme = string.Empty;
        DateTime startTime = DateTime.Now;
        long totalBytesRead = 0;
        string 模型保存路径 = string.Empty;
        string 模型保存名称 = string.Empty;
        string _uuid = string.Empty;
        string _Hash = "";
        private string 下载路径;

        public model_download_list(string _versionDesc, string uuid, string _模型名, string _modelname, string _modelVersionId, string _modelSourceName, string _modelSource, string _modelSourceSize, string _modelSourceHash, string _modelType)
        {
            InitializeComponent();
            _uuid = uuid;
            _Hash = GenerateHash(initialize.工作路径);

            if (initialize.工作路径.Length == 3)
            {
                下载路径 = initialize.工作路径.Substring(0, initialize.工作路径.Length - 1);
            }
            else
            {
                下载路径 = initialize.工作路径;
            }

            if (_modelType == "Checkpoint")
            {
                模型保存路径 = 下载路径 + "\\models\\Stable-diffusion";
                模型保存名称 = _模型名 + _modelname + ".safetensors";
            }
            if (_modelType == "LORA")
            {
                模型保存路径 = 下载路径 + "\\models\\Lora";
                模型保存名称 = _模型名 + _modelname + ".safetensors";

            }

            if (_modelType == "Textual Inversion")
            {
                模型保存路径 = 下载路径 + "\\embeddings";
                模型保存名称 = _模型名 + _modelname + ".pt";

            }
            if (_modelType == "LyCORIS")
            {
                模型保存路径 = 下载路径 + "\\models\\LyCORIS";
                模型保存名称 = _模型名 + _modelname + ".safetensors";

            }
            if (_modelType == "Controlnet")
            {
                模型保存路径 = 下载路径 + "\\models\\ControlNet";
                模型保存名称 = _模型名 + _modelname + ".safetensors";

            }
            if (_modelType == "Hypernetwork")
            {
                模型保存路径 = 下载路径 + "\\models\\ControlNet";
                模型保存名称 = _模型名 + _modelname + ".safetensors";

            }
            try
            {

                模型名称.Text = _modelname;
                模型版本ID.Text = "模型版本ID：" + _modelVersionId;
                模型文件名称.Text = "模型文件名称：" + _modelSourceName;
                模型下载源.Text = "模型下载地址：" + _modelSource;
                模型大小.Text = "模型文件大小：" + _modelSourceSize;
                模型hash.Text = "模型文件Hash：" + _modelSourceHash;
            }
            catch
            {
                模型名称.Text = "null";
                模型版本ID.Text = "null";
                模型文件名称.Text = "null";
                模型下载源.Text = "null";
                模型大小.Text = "null";
                模型hash.Text = "null";
            }

            if (模型名称.Text == "")
            {
                模型名称.Text = "未知";
            }
            if (模型版本ID.Text == "模型版本ID：")
            {
                模型版本ID.Text = "模型版本ID：" + "未知";
            }
            if (模型文件名称.Text == "模型文件名称：")
            {
                模型文件名称.Text = "模型文件名称：" + "未知";
            }
            if (模型下载源.Text == "模型下载地址：")
            {
                模型下载源.Text = "模型下载地址：" + "未知";
            }
            if (模型大小.Text == "模型文件大小：")
            {
                模型大小.Text = "模型文件大小：" + "未知";
            }
            if (模型hash.Text == "模型文件Hash：")
            {
                模型hash.Text = "模型文件Hash：" + "未知\n\n\n";
            }
            downloadUrl = _modelSource;
            modelNme = _modelname + _modelSourceName;
        }


        bool is_downloaded = false;

        //下面是一些与下载有关的code
        private async void Download_Click(object sender, RoutedEventArgs e)
        {
            //由于liblib的下载服务器不支持下载计数，为避免异常访问被404，需要报告下载的是什么
            var client = new HttpClient();
            var request2 = new HttpRequestMessage();
            request2.RequestUri = new Uri("https://liblib-api.vibrou.com/api/www/log/acceptor/f");
            request2.Method = HttpMethod.Post;

            request2.Headers.Add("Accept", "*/*");
            request2.Headers.Add("User-Agent", "Open-SD-WebUI-Launcher");

            var bodyString = "{\"cid\":\"" + _Hash + "\",\"e\":\"model.view.download\",  \"pageUrl\":\"Open-SD-WebUI-Launcher\", \"sys\":\"Windows\",  \"ua\":\"Open-SD-WebUI-Launcher\",   \"var\":{},    \"var.download\":\"start\",     \"var.model_id\":\"" + _uuid + "\",     \"var.version_id\":\"1\"}";
            var content = new StringContent(bodyString, Encoding.UTF8, "application/json");
            request2.Content = content;

            var response = await client.SendAsync(request2);
            var result = await response.Content.ReadAsStringAsync();


            if (is_downloaded == false)
            {
                try
                {
                    if (Directory.Exists(模型保存路径))
                    {

                    }
                    else
                    {
                        Directory.CreateDirectory(模型保存路径);
                    }
                    下载按钮.IsEnabled = false;
                    string 模型特殊名称过滤 = 模型保存名称.Replace("[", "").Replace("]", "").Replace("(", "").Replace(")", "").Replace("!", "").Replace("@", "").Replace("#", "").Replace("%", "").Replace("^", "").Replace("&", "").Replace("*", "").Replace("(", "").Replace(")", "").Replace("+", "").Replace("|", "").Replace("~", "");
                    string modelpath = System.IO.Path.Combine(模型保存路径, 模型特殊名称过滤);
                    WebRequest request = WebRequest.Create(downloadUrl);
                    WebResponse respone = request.GetResponse();
                    progressBar.Maximum = respone.ContentLength;
                    try
                    {
                        ThreadPool.QueueUserWorkItem((obj) =>
                        {
                            try
                            {
                                Stream netStream = respone.GetResponseStream();

                                Stream fileStream = new FileStream(modelpath, FileMode.Create);

                                byte[] read = new byte[1024];
                                long progressBarValue = 0;
                                int realReadLen = netStream.Read(read, 0, read.Length);
                                while (realReadLen > 0)
                                {
                                    fileStream.Write(read, 0, realReadLen);
                                    progressBarValue += realReadLen;
                                    progressBar.Dispatcher.BeginInvoke(new ProgressBarSetter(SetProgressBar), progressBarValue);
                                    realReadLen = netStream.Read(read, 0, read.Length);

                                    // 计算下载速度和已下载的文件大小/文件总大小
                                    DateTime currentTime = DateTime.Now;
                                    double elapsedSeconds = (currentTime - startTime).TotalSeconds;
                                    if (elapsedSeconds > 0)
                                    {
                                        double downloadSpeed = totalBytesRead / elapsedSeconds / 1024; // KB/s
                                        if (downloadSpeed > 0)
                                        {
                                            speedInfo = $" 当前速度：{downloadSpeed:F2}  KB/s";
                                            下载按钮.Dispatcher.BeginInvoke(new Action(() => 下载按钮.Content = speedInfo), null);

                                        }


                                    }
                                    totalBytesRead += realReadLen;

                                }
                                netStream.Close();
                                fileStream.Close();

                                // 释放线程
                                ThreadPool.QueueUserWorkItem((obj) => { }, null);
                                // 文件下载完成后，释放下载按钮的Dispatcher
                                下载按钮.Dispatcher.BeginInvoke(new Action(() => 下载按钮.Content = speedInfo), null);
                                下载按钮.Dispatcher.BeginInvoke(new Action(() =>
                                {
                                    下载按钮.Content = speedInfo;
                                    is_downloaded = true;
                                    下载按钮.Content = "下载完成,点击打开";
                                    下载按钮.IsEnabled = true;

                                }
                                ), null);
                            }
                            catch (Exception error)
                            {
                                File.WriteAllText(@".\logs\error.txt", error.Message.ToString());
                            }
                        }, null);
                    }
                    catch (Exception error)
                    {
                        File.WriteAllText(@".\logs\error.txt", error.Message.ToString());
                    }
                }
                catch (Exception error)
                {
                    File.WriteAllText(@".\logs\error.txt", error.Message.ToString());
                }
            }
            if (is_downloaded == true)
            {
                //用文件资源管理器打开模型
                System.Diagnostics.Process.Start("explorer.exe", 模型保存路径);
            }

        }

        private async void cancelButton_Click(object sender, RoutedEventArgs e)
        {

        }

        public static string GenerateHash(string input)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
            }
        }

        private void progressBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }
    }
}
