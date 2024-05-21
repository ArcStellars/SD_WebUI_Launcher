using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Awake.Models;
using Awake.Views.Pages;
using Wpf.Ui.Controls;
using static System.Net.Mime.MediaTypeNames;

namespace Awake.Views.Windows
{
    public class ExtTagItem
    {
        public string Ext { get; set; }
        public string Hash { get; set; }
        public string Message { get; set; }
        public string Date { get; set; }
        public string Tag { get; set; }
    }
    /// <summary>
    /// VerManager.xaml 的交互逻辑
    /// </summary>
    public partial class VerManager : UiWindow
    {
        private string currExt;
        private string currHash;
        private List<ExtTagItem> tags;
        public List<CommitItem> commits;
        public ObservableCollection<CommitItem> CommiteCollection = new();
        public ObservableCollection<CommitItem> CommiteTagCollection = new();
        private string giturl;
        public VerManager()
        {
            InitializeComponent();
        }

        public VerManager(string giturl, string ext)
        {
            this.currExt = ext;
            this.giturl = giturl;

            InitializeComponent();

            Process process1 = new Process();
            ProcessStartInfo startInfo1 = new ProcessStartInfo();
            startInfo1.FileName = initialize.gitPath_use + @"\mingw64\libexec\git-core\git.exe";
            startInfo1.Arguments = " fetch  --all"; //同步云端更新日志到本地
            startInfo1.UseShellExecute = false;
            startInfo1.RedirectStandardOutput = true;
            startInfo1.RedirectStandardError = false;
            startInfo1.CreateNoWindow = true;
            startInfo1.WorkingDirectory = ext;

            process1.StartInfo = startInfo1;
            process1.Start();
            process1.WaitForExit();

            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = initialize.gitPath_use + @"\mingw64\libexec\git-core\git.exe";
            startInfo.Arguments = " log --oneline --pretty=\"%h^^%s^^%cd\" --date=format:\"%Y-%m-%d %H:%M:%S\" -n 1";
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = false;
            startInfo.CreateNoWindow = true;
            startInfo.WorkingDirectory = ext;

            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();

            string msg = process.StandardOutput.ReadToEnd();

            process = new Process();
            startInfo = new ProcessStartInfo();
            startInfo.FileName = initialize.gitPath_use + @"\mingw64\libexec\git-core\git.exe";
            startInfo.Arguments = "  remote -v";
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = false;
            startInfo.CreateNoWindow = true;
            startInfo.WorkingDirectory = ext;

            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();

            string msg2 = process.StandardOutput.ReadToEnd();
            Debug.WriteLine(giturl);

            lblCurrHash.Content = msg.Split("^^")[0];
            lblCurrDate.Content = msg.Split("^^")[2];
            lblCurrMessage.Content = msg.Split("^^")[1];
            lblCurrGit.Content = msg2.Split("\\n")[0].Split(" ")[0];
            currHash = (string)lblCurrHash.Content;

            InitializeData(ext);

            commit.ItemsSource = CommiteCollection;
            commit2.ItemsSource = CommiteTagCollection;
        }

        private void InitializeData(string ext)
        {
            var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = initialize.gitPath_use + @"\mingw64\libexec\git-core\git.exe";
            startInfo.Arguments = "  --no-pager log main --pretty=\"%h^^%s^^%cd\" --date=format:\"%Y-%m-%d %H:%M:%S\" -n 150";
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.CreateNoWindow = true;
            startInfo.WorkingDirectory = ext;

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

            if (commits.Count <= 0)
            {
                process = new Process();
                startInfo = new ProcessStartInfo();
                startInfo.FileName = initialize.gitPath_use + @"\mingw64\libexec\git-core\git.exe";
                startInfo.Arguments = "  --no-pager log master --pretty=\"%h^^%s^^%cd\" --date=format:\"%Y-%m-%d %H:%M:%S\" -n 150";
                startInfo.UseShellExecute = false;
                startInfo.RedirectStandardOutput = true;
                startInfo.RedirectStandardError = true;
                startInfo.CreateNoWindow = true;
                startInfo.WorkingDirectory = ext;

                process.StartInfo = startInfo;

                idx = 0;
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

                    for (int j = 0; j < tags.Count(); j++)
                    {
                        if (currExt != tags[j].Ext) continue;
                        if (item1.Hash == tags[j].Hash)
                        {
                            item1.Tag = tags[j].Tag;
                        }
                    }

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
            else
            {
                for (int i = 0; i < commits.Count(); i++)
                {
                    CommiteCollection.Add(commits[i]);
                }
            }
        }

        private void setup_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button btn = (System.Windows.Controls.Button)sender;
            string hash = btn.Tag.ToString();

            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = initialize.gitPath_use + @"\mingw64\libexec\git-core\git.exe";
            startInfo.Arguments = " checkout " + hash;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.CreateNoWindow = true;
            startInfo.WorkingDirectory = currExt;

            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();


            currHash = hash;

            process = new Process();
            startInfo = new ProcessStartInfo();
            startInfo.FileName = initialize.gitPath_use + @"\mingw64\libexec\git-core\git.exe";
            startInfo.Arguments = " log --oneline --pretty=\"%h^^%s^^%cd\" --date=format:\"%Y-%m-%d %H:%M:%S\" -n 1";
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = false;
            startInfo.CreateNoWindow = true;
            startInfo.WorkingDirectory = currExt;

            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();

            string msg = process.StandardOutput.ReadToEnd();

            process = new Process();
            startInfo = new ProcessStartInfo();
            startInfo.FileName = initialize.gitPath_use + @"\mingw64\libexec\git-core\git.exe";
            startInfo.Arguments = " remote -v";
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = false;
            startInfo.CreateNoWindow = true;
            startInfo.WorkingDirectory = currExt;

            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();

            string msg2 = process.StandardOutput.ReadToEnd();

            lblCurrHash.Content = msg.Split("^^")[0];
            lblCurrDate.Content = msg.Split("^^")[2];
            lblCurrMessage.Content = msg.Split("^^")[1];
            lblCurrGit.Content = msg2.Split("\\n")[0].Split(" ")[0];
            currHash = (string)lblCurrHash.Content;

            CommiteCollection.Clear();
            CommiteTagCollection.Clear();

            InitializeData(currExt);

            commit.ItemsSource = CommiteCollection;
            commit2.ItemsSource = CommiteTagCollection;
            System.Windows.MessageBox.Show("安装完成,请继续操作");
        }

    }



}
