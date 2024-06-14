using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Awake.Models;
using Awake.Services;
using Awake.Views.Windows;
using static System.Net.Mime.MediaTypeNames;

namespace Awake.Views.Pages
{
    public partial class Exts
    {
        public Exts()
        {
            if (initialize.启用自定义路径)
            {
                initialize.加载路径 = initialize.本地路径;
                initialize.gitPath_use = initialize.gitPath;

            }
            else
            {
                initialize.加载路径 = initialize.工作路径;
                initialize.gitPath_use = initialize.工作路径 + @"\GIT";
            }

            if (initialize.启用自定义路径)
            {
                if (!File.Exists(initialize.gitPath_use + @"\mingw64\libexec\git-core\git.exe"))
                {
                    System.Windows.MessageBox.Show("自定义GIT路径错误或未选择，程序错误即将关闭！");
                    Process.GetCurrentProcess().Kill();
                }
            }
            else
            {
                if (!File.Exists(initialize.工作路径 + @"\GIT\mingw64\libexec\git-core\git.exe"))
                {
                    System.Windows.MessageBox.Show("工作路径下即整合包未存在GIT，程序错误即将关闭！");
                    Process.GetCurrentProcess().Kill();
                }

            }

            InitializeComponent();
            Init.InitExtData();
            exts.ItemsSource = Store.extLocal;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("Explorer.exe", initialize.加载路径 + @"\extensions");
        }
        private void checkUpdateExt_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;

            Process process1 = new Process();
            ProcessStartInfo startInfo1 = new ProcessStartInfo();
            startInfo1.FileName = initialize.gitPath_use + @"\mingw64\libexec\git-core\git.exe";
            startInfo1.Arguments = " fetch  --all"; //同步云端更新日志到本地
            startInfo1.UseShellExecute = false;
            startInfo1.RedirectStandardOutput = true;
            startInfo1.RedirectStandardError = false;
            startInfo1.CreateNoWindow = true;
            startInfo1.WorkingDirectory = (string)btn.Tag;

            process1.StartInfo = startInfo1;
            process1.Start();
            process1.WaitForExit();

            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = initialize.gitPath_use + @"\mingw64\libexec\git-core\git.exe";
            startInfo.Arguments = " reset --hard";
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.CreateNoWindow = true;
            startInfo.WorkingDirectory = (string)btn.Tag;
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();

            process = new Process();
            startInfo = new ProcessStartInfo();
            startInfo.FileName = initialize.gitPath_use + @"\mingw64\libexec\git-core\git.exe";
            startInfo.Arguments = " pull";
            startInfo.UseShellExecute = true;
            startInfo.RedirectStandardOutput = false;
            startInfo.CreateNoWindow = false;
            startInfo.WorkingDirectory = (string)btn.Tag;

            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();

            Init.InitExtData();
            exts.ItemsSource = Store.extLocal;
            System.Windows.MessageBox.Show("安装完成,请继续操作");

        }
        private void openExt_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            string ext = btn.Tag.ToString();
            Process.Start("Explorer.exe", ext);
        }
        private void Setup_Click(object sender, RoutedEventArgs e)
        {
            Awake.Views.Windows.ExtManager ext = new Awake.Views.Windows.ExtManager();
            ext.Show();
        }
        private void verManager_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button btn = (Button)sender;
                int idx = int.Parse(btn.Tag.ToString());

                VerManager ma = new VerManager(Store.extLocal[idx].GitUrl, Store.extLocal[idx].Path);
                ma.Show();
            }
            catch
            {
                System.Windows.MessageBox.Show("获取失败,可能是插件问题");
            }

        }
        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            Init.InitExtData();
            exts.ItemsSource = Store.extLocal;
        }
    }
    public class ExtRemote
    {
        public string name { get; set; }
        public string hash { get; set; }
        public string date { get; set; }
        public string url { get; set; }
    }
}
