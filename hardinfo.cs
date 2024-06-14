using System;
using System.Management;
namespace Awake
{
    internal class hardinfo
    {
        public static string GetCpuName()//获得计算机CPU名字
        {
            var CPUName = "";
            var management = new ManagementObjectSearcher("Select * from Win32_Processor");
            foreach (var baseObject in management.Get())
            {
                var managementObject = (ManagementObject)baseObject;
                CPUName = managementObject["Name"].ToString();
            }
            return CPUName;
        }
        public static string GetComputerName()//获得计算机名称
        {
            return Environment.MachineName;
        }

        public static string GetSystemType()//获得系统类型
        {
            var sysTypeStr = "";
            ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementBaseObject o in moc)
            {
                ManagementObject mo = (ManagementObject)o;
                sysTypeStr = mo["SystemType"].ToString();
            }
            return sysTypeStr;
        }
        public static float GetPhysicalMemory()//读取内存大小
        {
            float memoryCount = 0;
            var mc = new ManagementClass("Win32_ComputerSystem");
            var moc = mc.GetInstances();
            foreach (var o in moc)
            {
                var mo = (ManagementObject)o;
                string str = mo["TotalPhysicalMemory"].ToString();//单位为 B
                float a = long.Parse(str);
                memoryCount = a / 1024 / 1024 / 1024;//单位换成GB
            }
            return memoryCount;
        }
        public static int MemoryNumberCount()//获取内存条数量
        {
            ManagementClass m = new ManagementClass("Win32_PhysicalMemory");
            ManagementObjectCollection mn = m.GetInstances();
            int count = mn.Count;
            return count;
        }
        public static string GPUName()//获得显卡名称
        {
            string DisplayName = "";
            ManagementClass m = new ManagementClass("Win32_VideoController");
            ManagementObjectCollection mn = m.GetInstances();
            DisplayName = "显卡数量：" + mn.Count.ToString() + "  " + "\n";
            ManagementObjectSearcher mos = new ManagementObjectSearcher("Select * from Win32_VideoController");//Win32_VideoController 显卡
            int count = 0;
            foreach (ManagementObject mo in mos.Get())
            {
                count++;
                DisplayName += "显卡型号：" + count.ToString() + " " + mo["Name"].ToString() + "   " + "\n"; ;
            }
            mn.Dispose();
            m.Dispose();
            return DisplayName;
        }
        public static string getGPUNamelist()//获得显卡名称list，用于做多卡选择
        {
            string DisplayName = "";
            string 显卡名称 = "";
            ManagementClass m = new ManagementClass("Win32_VideoController");
            ManagementObjectCollection mn = m.GetInstances();
            DisplayName = "显卡数量：" + mn.Count.ToString() + "  " + "\n";
            ManagementObjectSearcher mos = new ManagementObjectSearcher("Select * from Win32_VideoController");//Win32_VideoController 显卡
            int count = 0;
            foreach (ManagementObject mo in mos.Get())
            {
                count++;
                DisplayName += "显卡型号：" + count.ToString() + " " + mo["Name"].ToString() + "   " + "\n";
                显卡名称 = mo["Name"].ToString();
                initialize.显卡列表.Add(显卡名称);
            }
            mn.Dispose();
            m.Dispose();
            return DisplayName;
        }
    }
}
