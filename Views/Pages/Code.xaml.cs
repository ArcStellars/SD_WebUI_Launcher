using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Media;
using Awake.Models;
using Wpf.Ui.Common.Interfaces;
using Awake.Views.Windows;
using System.Windows.Controls;
using Awake.Services;


namespace Awake.Views.Pages
{
    public partial class Code
    {
        public string currHash;
        public List<CommitItem> commits;
        public List<CommitItem> commits2;
        public ObservableCollection<CommitItem> CommiteCollection = new();
        public ObservableCollection<CommitItem> CommiteCollection2 = new();

        public Code()
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

            Process process3 = new Process();
            ProcessStartInfo startInfo3 = new ProcessStartInfo();
            startInfo3.FileName = initialize.gitPath_use + @"\mingw64\libexec\git-core\git.exe";
            startInfo3.Arguments = " fetch  --all"; //同步云端更新日志到本地
            startInfo3.UseShellExecute = false;
            startInfo3.RedirectStandardOutput = true;
            startInfo3.RedirectStandardError = false;
            startInfo3.CreateNoWindow = true;
            startInfo3.WorkingDirectory = initialize.加载路径;

            Process process1 = new Process();
            ProcessStartInfo startInfo1 = new ProcessStartInfo();
            startInfo1.FileName = initialize.gitPath_use + @"\mingw64\libexec\git-core\git.exe";
            startInfo1.Arguments = " log --oneline --pretty=\"%h^^%s^^%cd\" --date=format:\"%Y-%m-%d %H:%M:%S\" -n 1";
            startInfo1.UseShellExecute = false;
            startInfo1.RedirectStandardOutput = true;
            startInfo1.RedirectStandardError = false;
            startInfo1.CreateNoWindow = true;
            startInfo1.WorkingDirectory = initialize.加载路径;

            process1.StartInfo = startInfo1;
            process1.Start();
            process1.WaitForExit();

            string msg1 = process1.StandardOutput.ReadToEnd();
            currHash = msg1.Split("^^")[0];
            //Debug.WriteLine(msg);

            process1 = new Process();
            startInfo1 = new ProcessStartInfo();
            startInfo1.FileName = initialize.gitPath_use + @"\mingw64\libexec\git-core\git.exe";
            startInfo1.Arguments = " remote -v";
            startInfo1.UseShellExecute = false;
            startInfo1.RedirectStandardOutput = true;
            startInfo1.RedirectStandardError = false;
            startInfo1.CreateNoWindow = true;
            startInfo1.WorkingDirectory = initialize.加载路径;

            process1.StartInfo = startInfo1;
            process1.Start();
            process1.WaitForExit();

            string msg12 = process1.StandardOutput.ReadToEnd();

            InitializeData();
            InitializeComponent();

            lblCurrHash.Content = currHash;
            lblCurrDate.Content = msg1.Split("^^")[2];
            lblCurrMessage.Content = msg1.Split("^^")[1];
            lblCurrGit.Content = msg12.Split("\\n")[0].Split(" ")[0];

            process1 = new Process();
            startInfo1 = new ProcessStartInfo();
            startInfo1.FileName = initialize.gitPath_use + @"\mingw64\libexec\git-core\git.exe";
            startInfo1.Arguments = " log --oneline origin master --pretty=\"%h^^%s^^%cd\" --date=format:\"%Y-%m-%d %H:%M:%S\" -n 1";
            startInfo1.UseShellExecute = false;
            startInfo1.RedirectStandardOutput = true;
            startInfo1.RedirectStandardError = false;
            startInfo1.CreateNoWindow = true;
            startInfo1.WorkingDirectory = initialize.加载路径;

            process1.StartInfo = startInfo1;
            process1.Start();
            process1.WaitForExit();

            msg1 = process1.StandardOutput.ReadToEnd();
            currHash = msg1.Split("^^")[0];

            commit2.ItemsSource = CommiteCollection;


            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = initialize.gitPath_use + @"\mingw64\libexec\git-core\git.exe";
            startInfo.Arguments = " log --oneline --pretty=\"%h^^%s^^%cd\" --date=format:\"%Y-%m-%d %H:%M:%S\" -n 1";
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = false;
            startInfo.CreateNoWindow = true;
            startInfo.WorkingDirectory = initialize.加载路径;

            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();

            string msg = process.StandardOutput.ReadToEnd();
            currHash = msg.Split("^^")[0];
            //Debug.WriteLine(msg);

            process = new Process();
            startInfo = new ProcessStartInfo();
            startInfo.FileName = initialize.gitPath_use + @"\mingw64\libexec\git-core\git.exe";
            startInfo.Arguments = " remote -v";
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = false;
            startInfo.CreateNoWindow = true;
            startInfo.WorkingDirectory = initialize.加载路径;

            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();

            string msg2 = process.StandardOutput.ReadToEnd();

            InitializeData();
            InitializeComponent();

            lblCurrHash.Content = currHash;
            lblCurrDate.Content = msg.Split("^^")[2];
            lblCurrMessage.Content = msg.Split("^^")[1];
            lblCurrGit.Content = msg2.Split("\\n")[0].Split(" ")[0];

            process = new Process();
            startInfo = new ProcessStartInfo();
            startInfo.FileName = initialize.gitPath_use + @"\mingw64\libexec\git-core\git.exe";
            startInfo.Arguments = " log --oneline origin dev --pretty=\"%h^^%s^^%cd\" --date=format:\"%Y-%m-%d %H:%M:%S\" -n 1";
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = false;
            startInfo.CreateNoWindow = true;
            startInfo.WorkingDirectory = initialize.加载路径;

            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();

            msg = process.StandardOutput.ReadToEnd();
            currHash = msg.Split("^^")[0];

            commit.ItemsSource = CommiteCollection;
        }
        private void InitializeData()
        {
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = initialize.gitPath_use + @"\mingw64\libexec\git-core\git.exe";
            startInfo.Arguments = " --no-pager log origin/master --pretty=\"%h^^%s^^%cd\" --date=format:\"%Y-%m-%d %H:%M:%S\" -n 1000";
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.CreateNoWindow = true;
            startInfo.WorkingDirectory = initialize.加载路径;

            process.StartInfo = startInfo;

            int idx = 0;
            commits = new List<CommitItem>();
            process.ErrorDataReceived += new DataReceivedEventHandler(delegate (object sender, DataReceivedEventArgs e)
            {

            });
            process.OutputDataReceived += new DataReceivedEventHandler(delegate (object sender, DataReceivedEventArgs e)
            {
                if (e.Data == null) return;
                CommitItem item1 = new CommitItem();
                string[] itemarr = e.Data.Split("^^");
                if (itemarr.Length < 3)
                {
                    return;
                }
                item1.Hash = itemarr[0];
                item1.Message = itemarr[1];
                item1.Date = itemarr[2];
                item1.Id = idx++;
                item1.Enable = true;
                item1.Checked = false;

                if (currHash == item1.Hash)
                {
                    item1.Enable = false;
                    item1.Checked = true;
                }

                commits.Add(item1);
            });

            process.Start();
            process.BeginErrorReadLine();
            process.BeginOutputReadLine();
            process.WaitForExit();

            for (int i = 0; i < commits.Count(); i++)
            {
                CommiteCollection.Add(commits[i]);
            }
        }
        private void setup_Click(object sender, RoutedEventArgs e)
        {

            Button btn = (Button)sender;
            string hash = btn.Tag.ToString();

            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = initialize.gitPath_use + @"\mingw64\libexec\git-core\git.exe";
            startInfo.Arguments = " reset --hard"; //回退版本到
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.CreateNoWindow = true;
            startInfo.WorkingDirectory = initialize.加载路径;

            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();

            process = new Process();
            startInfo = new ProcessStartInfo();
            startInfo.FileName = initialize.gitPath_use + @"\mingw64\libexec\git-core\git.exe";
            startInfo.Arguments = " checkout " + hash; //切换到指定分支
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.CreateNoWindow = true;
            startInfo.WorkingDirectory = initialize.加载路径;

            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();

            string msg = process.StandardOutput.ReadToEnd();
            Debug.WriteLine(msg);

            CommiteCollection.Clear();

            process = new Process();
            startInfo = new ProcessStartInfo();
            startInfo.FileName = initialize.gitPath_use + @"\mingw64\libexec\git-core\git.exe";
            startInfo.Arguments = " log --oneline --pretty=\"%h^^%s^^%cd\" --date=format:\"%Y-%m-%d %H:%M:%S\" -n 1";
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = false;
            startInfo.CreateNoWindow = true;
            startInfo.WorkingDirectory = initialize.加载路径;

            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();

            msg = process.StandardOutput.ReadToEnd();
            currHash = msg.Split("^^")[0];

            process = new Process();
            startInfo = new ProcessStartInfo();
            startInfo.FileName = initialize.gitPath_use + @"\mingw64\libexec\git-core\git.exe";
            startInfo.Arguments = "  remote -v";
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = false;
            startInfo.CreateNoWindow = true;
            startInfo.WorkingDirectory = initialize.加载路径;

            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();

            string msg2 = process.StandardOutput.ReadToEnd();

            InitializeData();

            lblCurrHash.Content = currHash;
            lblCurrDate.Content = msg.Split("^^")[2];
            lblCurrMessage.Content = msg.Split("^^")[1];
            lblCurrGit.Content = msg2.Split("\\n")[0].Split(" ")[0];

            commit.ItemsSource = CommiteCollection;
            System.Windows.MessageBox.Show("安装完成,请继续操作");
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CommiteCollection.Clear();

            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = initialize.gitPath_use + @"\mingw64\libexec\git-core\git.exe";
            startInfo.Arguments = " log --oneline --pretty=\"%h^^%s^^%cd\" --date=format:\"%Y-%m-%d %H:%M:%S\" -n 1";
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = false;
            startInfo.CreateNoWindow = true;
            startInfo.WorkingDirectory = initialize.加载路径;

            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();

            string msg = process.StandardOutput.ReadToEnd();
            currHash = msg.Split("^^")[0];
            //Debug.WriteLine(msg);

            process = new Process();
            startInfo = new ProcessStartInfo();
            startInfo.FileName = initialize.gitPath_use + @"\mingw64\libexec\git-core\git.exe";
            startInfo.Arguments = " remote -v";
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = false;
            startInfo.CreateNoWindow = true;
            startInfo.WorkingDirectory = initialize.加载路径;

            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();

            string msg2 = process.StandardOutput.ReadToEnd();

            InitializeData();
            InitializeComponent();

            lblCurrHash.Content = currHash;
            lblCurrDate.Content = msg.Split("^^")[2];
            lblCurrMessage.Content = msg.Split("^^")[1];
            lblCurrGit.Content = msg2.Split("\\n")[0].Split(" ")[0];

            process = new Process();
            startInfo = new ProcessStartInfo();
            startInfo.FileName = initialize.gitPath_use + @"\mingw64\libexec\git-core\git.exe";
            startInfo.Arguments = " log --oneline origin master --pretty=\"%h^^%s^^%cd\" --date=format:\"%Y-%m-%d %H:%M:%S\" -n 1";
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = false;
            startInfo.CreateNoWindow = true;
            startInfo.WorkingDirectory = initialize.加载路径;

            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();

            msg = process.StandardOutput.ReadToEnd();
            currHash = msg.Split("^^")[0];

            commit.ItemsSource = CommiteCollection;
        }
        private void UpdateCode_Click(object sender, RoutedEventArgs e)
        {
            Process process2 = new Process();
            ProcessStartInfo startInfo2 = new ProcessStartInfo();
            startInfo2.FileName = initialize.gitPath_use + @"\mingw64\libexec\git-core\git.exe";
            startInfo2.Arguments = " pull ";
            startInfo2.UseShellExecute = true;
            startInfo2.RedirectStandardOutput = false;
            startInfo2.CreateNoWindow = false;
            startInfo2.WorkingDirectory = initialize.加载路径;

            process2.StartInfo = startInfo2;
            process2.Start();
            process2.WaitForExit();

            btnUpdateCode.IsEnabled = false;

            CommiteCollection.Clear();

            InitializeData();

            commit.ItemsSource = CommiteCollection;
        }

    }
    public class TagItem
    {
        public string Hash { get; set; }

        public string Message { get; set; }

        public string Date { get; set; }
    }

}
