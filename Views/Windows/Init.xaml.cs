using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
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
using Awake.Services;
using Awake.Views.Pages;
using Wpf.Ui.Controls;
using static System.Net.WebRequestMethods;

namespace Awake.Views.Windows
{
    /// <summary>
    /// Init.xaml 的交互逻辑
    /// </summary>
    public partial class Init : UiWindow
    {

        public static void InitExtData()
        {


            Store.extLocal = new System.Collections.ObjectModel.ObservableCollection<ExtItem>();

            Process process;
            ProcessStartInfo startInfo;


            process = new Process();
            startInfo = new ProcessStartInfo();
            startInfo.FileName = initialize.gitPath_use + @"\mingw64\libexec\git-core\git.exe";
            startInfo.Arguments = " config --global --add safe.directory *";
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = false;
            startInfo.CreateNoWindow = true;
            startInfo.WorkingDirectory = (initialize.加载路径 + @"\extensions");

            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();

            var httpClient = new HttpClient();
            var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            using (var request = new HttpRequestMessage(HttpMethod.Get,
                "https://raw.githubusercontent.com/fallingmeteorite/automatic111-webui-exts/master/exts_ver.json"))
            {
                var response = httpClient.Send(request);
                response.EnsureSuccessStatusCode();
                using var stream = response.Content.ReadAsStream();
                Store.extRemote = JsonSerializer.Deserialize<List<ExtRemote>>(stream, jsonOptions);
            }
            if (!Directory.Exists(initialize.加载路径 + @"\extensions"))
            {
                return;
            }
            List<string> extsDir = new List<string>(Directory.EnumerateDirectories(initialize.加载路径 + @"\extensions"));
            for (int i = 0; i < extsDir.Count(); i++)
            {
                process = new Process();
                startInfo = new ProcessStartInfo();
                startInfo.FileName = initialize.gitPath_use + @"\mingw64\libexec\git-core\git.exe";
                startInfo.Arguments = " remote -v ";
                startInfo.UseShellExecute = false;
                startInfo.RedirectStandardOutput = true;
                startInfo.CreateNoWindow = true;
                startInfo.WorkingDirectory = extsDir[i];

                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();

                string msg = process.StandardOutput.ReadToEnd();
                ExtItem item1 = new ExtItem();
                item1.Index = i;
                item1.Name = extsDir[i].Split("\\").LastOrDefault();
                item1.Path = extsDir[i];
                if (msg.Length > 0)
                {
                    if (msg.Split("\\n").Length <= 0) continue;
                    if (msg.Split("\\n")[0].Split(" ").Length <= 0) continue;
                    item1.GitUrl = msg.Split("\\n")[0].Split(" ")[0].Substring(7);

                    process = new Process();
                    startInfo = new ProcessStartInfo();
                    startInfo.FileName = initialize.gitPath_use + @"\mingw64\libexec\git-core\git.exe";
                    startInfo.Arguments = " log --oneline --pretty=\"%h^^%s^^%cd\" --date=format:\"%Y-%m-%d %H:%M:%S\" -n 1";
                    startInfo.UseShellExecute = false;
                    startInfo.RedirectStandardOutput = true;
                    startInfo.RedirectStandardError = false;
                    startInfo.CreateNoWindow = true;
                    startInfo.WorkingDirectory = extsDir[i];

                    process.StartInfo = startInfo;
                    process.Start();
                    process.WaitForExit();

                    msg = process.StandardOutput.ReadToEnd();
                    string[] data = msg.Split("^^");
                    item1.Hash = data[0];
                    item1.Date = data[2];

                    for (int j = 0; j < Store.extRemote.Count; j++)
                    {
                        try
                        {
                            if (item1.GitUrl.Split("//")[item1.GitUrl.Split("//").Length - 1].Split("/")[2].Replace(".git", "") == Store.extRemote[j].url.Split("//")[item1.GitUrl.Split("//").Length - 1].Split("/")[2].Replace(".git", ""))
                            {
                                item1.hasUpdate = true;

                                if (item1.Hash == Store.extRemote[j].hash)
                                {
                                    item1.hasUpdate = false;
                                }
                            }
                        }
                        catch
                        {

                        }
                    }
                }
                else
                {
                    item1.GitUrl = "异常";
                }
                Store.extLocal.Add(item1);
            }
        }
    }
}
