using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
namespace Awake
{
    class initialize//这里负责整个启动器的初始化，也是回复默认设置
    {

        public static bool CheckDirectory()//首先查找启动器所在目录是否有工作目录，在启动器所在目录创建工作目录
        {

            if (Directory.Exists(@".AI_launther_log"))
            {
                return true;
            }
            else
            {
                Directory.CreateDirectory(@".AI_launther_log");
                return false;
            }

        }

        public static List<string> 显卡列表 = new List<string>();
        public static string 背景颜色 = "";
        public static string 背景图片 = "";
        public static int 背景亮度 = 100;
        public static int 图片亮度 = 100;
        public static bool _SD启动 = false;//SD,启动的白屏动画效果开关!

        //下面确定SD_WebUI是否已经安装
        public static bool 已下载WebUI = false;
        public static bool 已解压WebUI = false;
        public static bool 已安装WebUI = false;

        //下面是一些启动选项的具体操作
        public static bool 浏览器启动 = false;
        public static bool 启动api = false;
        public static bool 分享WebUI到公网 = false;
        public static bool 使用CPU进行推理 = false;
        public static bool 关闭模型hash计算 = false;
        public static bool 冻结设置 = false;
        public static bool 快速启动;
        internal static bool 启用自定义路径;

        //优化
        public static bool 上投采样 = false;
        public static bool 关闭半精度计算 = false;
        public static bool 内存优化 = false;

        public static bool 启用InvokeAI = false;

        public static bool SDP优化 = false;
        public static bool 缩放点积 = false;
        public static bool 启用xformers = false;
        public static bool 启用替代布局 = false;
        public static bool Doggettx优化 = false;

        //下面是一些参数的字符串预设
        public static string 命令列表 = "";
        public static string 显卡类型名 = "";
        public static string _显卡类型 = "";
        public static string _WebUI显存压力优化设置 = " ";
        public static string _WebUI主题颜色 = "";

        //下面是一些路径管理的具体实现
        public static string 工作路径 = "";
        public static string gitPath = "";
        public static string venvPath = "";
        public static string 程序所在目录 = "";
        public static string 本地路径 = "";

        //下面是全局硬件判断
        public static string _cpuname = "";
        public static string _GPUname = "";
        public static int _UseGPUindex = 0;
        public static string 相册图片数量 = "";
        public static string 参数列表 = "";//所有启动时传递的参数挂到这里，全局可编辑与访问
        public static string 加载路径;
        public static string gitPath_use;

        public static void Read_setting()
        {

            string filePath = @".AI_launther_log\setting.txt"; // 文本文件路径

            try
            {
                List<string> lines_setting = new List<string>();

                // 使用 File.ReadAllLines 方法读取文本文件的所有行
                lines_setting.AddRange(File.ReadAllLines(filePath));

                // 在这里使用列表
                if (lines_setting[0] == "True")
                {
                    initialize.浏览器启动 = true;


                }
                if (lines_setting[1] == "True")
                {
                    initialize.启动api = true;

                }
                if (lines_setting[2] == "True")
                {
                    initialize.分享WebUI到公网 = true;

                }
                if (lines_setting[3] == "True")
                {
                    initialize.使用CPU进行推理 = true;

                }
                if (lines_setting[4] == "True")
                {
                    initialize.关闭模型hash计算 = true;

                }
                if (lines_setting[5] == "True")
                {
                    initialize.冻结设置 = true;

                }
                if (lines_setting[6] == "True")
                {
                    initialize.快速启动 = true;

                }

                if (lines_setting[7] == "True")
                {
                    initialize.启用自定义路径 = true;
                }

                if (lines_setting[8] == "True")
                {
                    initialize.上投采样 = true;


                }
                if (lines_setting[9] == "True")
                {
                    initialize.关闭半精度计算 = true;

                }
                if (lines_setting[10] == "True")
                {
                    initialize.启用InvokeAI = true;

                }
                if (lines_setting[11] == "True")
                {
                    initialize.内存优化 = true;

                }
                if (lines_setting[12] == "True")
                {
                    initialize.SDP优化 = true;

                }
                if (lines_setting[13] == "True")
                {
                    initialize.缩放点积 = true;

                }
                if (lines_setting[14] == "True")
                {
                    initialize.启用xformers = true;

                }
                if (lines_setting[15] == "True")
                {
                    initialize.启用替代布局 = true;
                }
                initialize.显卡类型名 = lines_setting[16];
                initialize._显卡类型 = lines_setting[17];
                initialize._WebUI显存压力优化设置 = lines_setting[18];
                initialize._WebUI主题颜色 = lines_setting[19];
                if (lines_setting[20] == "True")
                {
                    initialize.Doggettx优化 = true;
                }

            }
            catch
            {

            }
        }

        public static void 选择工作路径()
        {
            FolderBrowserDialog folder = new FolderBrowserDialog();
            folder.Description = "请选择WebUI工作目录，至少保留25GB硬盘空间";
            if (folder.ShowDialog() == DialogResult.OK)
            {
                initialize.工作路径 = folder.SelectedPath;
                if (initialize.工作路径.Length == 3)
                {
                    initialize.工作路径 = initialize.工作路径.Substring(0, initialize.工作路径.Length - 1);
                }
                File.WriteAllText(@".AI_launther_log\startpath.txt", initialize.工作路径);
            }
        }
        public static void 选择Git路径()
        {
            FolderBrowserDialog folder = new FolderBrowserDialog();
            folder.Description = "请选择Git.exe所在目录";
            if (folder.ShowDialog() == DialogResult.OK)
            {
                initialize.gitPath = folder.SelectedPath;
                File.WriteAllText(@".AI_launther_log\gitpath.txt", initialize.gitPath);

            }
        }
        public static void 选择VENV路径()
        {
            FolderBrowserDialog folder = new FolderBrowserDialog();
            folder.Description = "请选择虚拟环境所在目录";
            if (folder.ShowDialog() == DialogResult.OK)
            {
                initialize.venvPath = folder.SelectedPath;
                File.WriteAllText(@".AI_launther_log\venvpath.txt", initialize.venvPath);

            }
        }

        public static void 本地运行路径()
        {
            FolderBrowserDialog folder = new FolderBrowserDialog();
            folder.Description = "请选择自定义SD所在目录";
            if (folder.ShowDialog() == DialogResult.OK)
            {
                initialize.本地路径 = folder.SelectedPath;
                if (initialize.本地路径.Length == 3)
                {
                    initialize.本地路径 = initialize.本地路径.Substring(0, initialize.本地路径.Length - 1);
                }
                File.WriteAllText(@".AI_launther_log\startpath_local.txt", initialize.本地路径);

            }
        }

        public static void 获取程序同目录路径()
        {
            // 获取程序所在目录
            程序所在目录 = Directory.GetCurrentDirectory();

        }
        public static bool CheckWebUIdownloaded() //检查WebUI是否下载
        {
            string instellPath = initialize.工作路径 + @"\WebUIpack.7z";
            if (File.Exists(instellPath))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool CheckWebUIunzip() //检查WebUI是否解压
        {
            string instellPath = initialize.工作路径 + @"\2.0.9\stable-diffusion-webui\webui-user.bat";
            if (File.Exists(instellPath))
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public static bool CheckWebUIinstelled() //检查WebUI是否安装
        {

            if (File.Exists(initialize.工作路径 + @"\webui-user.bat"))
            {
                return true;
            }
            else
            {
                if (File.Exists(initialize.本地路径 + @"\webui-user.bat"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }
        public static void CheckCommandline()//这里在初始化后从log里读取保存的args
        {
            if (File.Exists(@".AI_launther_log\argspath.txt"))
            {
                // 如果文件存在，读取其中的内容到startpath全局变量中
                initialize.命令列表 = File.ReadAllText(@".AI_launther_log\argspath.txt");

            }

        }
        public static void CheckStartPathFile()//这里在初始化后从log里读取工作路径
        {
            if (File.Exists(@".AI_launther_log\startpath.txt"))
            {
                // 如果文件存在，读取其中的内容到startpath全局变量中
                initialize.工作路径 = File.ReadAllText(@".AI_launther_log\startpath.txt");
              
                if (initialize.工作路径.Length == 3)
                {
                    initialize.工作路径 = initialize.工作路径.Substring(0, initialize.工作路径.Length - 1);
                }              
            }


        }
        public static void CheckgitPathFile()//这里在初始化后从log里读取git路径
        {
            if (File.Exists(@".AI_launther_log\gitpath.txt"))
            {
                // 如果文件存在，读取其中的内容到gitpath全局变量中
                initialize.gitPath = File.ReadAllText(@".AI_launther_log\gitpath.txt");   
            }
        }
        public static void CheckVENVPathFile()//这里在初始化后从log里读取VENV的路径
        {
            string filePath = @".AI_launther_log\venvpath.txt";
            if (File.Exists(filePath))
            {
                // 如果文件存在，读取其中的内容到venvpath全局变量中
                initialize.venvPath = File.ReadAllText(filePath);
            }
        }
        public static void Checkstartpath_local()//这里在初始化后从log里读取VENV的路径
        {
            if (File.Exists(@".AI_launther_log\startpath_local.txt"))
            {
                // 如果文件存在，读取其中的内容到venvpath全局变量中
                initialize.本地路径 = File.ReadAllText(@".AI_launther_log\startpath_local.txt");
                if (initialize.本地路径.Length == 3)
                {
                    initialize.本地路径 = initialize.本地路径.Substring(0, initialize.本地路径.Length - 1);
                }
            }
        }
        public static void 相册计数()
        {
            string 相册路径 = 工作路径 + "\\outputs";
            if (!Directory.Exists(相册路径))
            {
                // 如果不存在，将"未找到相册"赋值给A1
                initialize.相册图片数量 = "未找到相册";
            }
            else//相册计数功能
            {
                int imageFileCount = 0;
                // 遍历outputs文件夹及其子文件夹
                foreach (string directory in Directory.GetDirectories(相册路径, "*", SearchOption.AllDirectories))
                {
                    // 遍历当前目录下的所有文件
                    foreach (string file in Directory.GetFiles(directory))
                    {
                        // 获取文件后缀名
                        string extension = Path.GetExtension(file);
                        // 判断是否为图片文件
                        if (extension == ".png" || extension == ".jpg" || extension == ".jpeg" || extension == ".gif")
                        {
                            // 如果是图片文件，图片文件数量加1
                            imageFileCount++;
                        }
                    }
                }
                // 将图片文件数量写入变量AI
                int AI = imageFileCount;
                initialize.相册图片数量 = AI.ToString();

            }
        }











    }

}
