using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolLibrary
{
    /*
     * DeviceLog类用来输出程序要记录的log信息
     * 
     * 备注：
     * 主进程和dll之间是一个程序集空间，单实例对象只有一个
     */
    public class DeviceLog
    {
        private string m_LogFileName;
        private System.IO.FileStream m_File;
        private static DeviceLog m_Single = null;
        private static readonly object SynObject = new object();
        private bool m_bLogConsole;
        private DateTime m_CreateTime;
        private string m_DeviceID;
        public static DeviceLog GetInstance
        {
            get
            {
                if (m_Single == null)
                {
                    lock (SynObject)
                    {
                        return m_Single ?? (m_Single = new DeviceLog());
                    }
                }
                return m_Single;
            }
        }
        public bool LogConsole
        {
            get { return m_bLogConsole; }
            set { 
                m_bLogConsole = value;
                if (m_bLogConsole) 
                    WriteLog("Start out system log to screen");
            }
        }
        public string DeviceID
        {
            set { m_DeviceID = value; }
        }
        public string GetDataTimeString()
        {
            StringBuilder time = new StringBuilder();
            DateTime now = DateTime.Now;
            time.Append(now.Year); time.Append('_');
            time.Append(now.Month); time.Append('_');
            time.Append(now.Day); time.Append('_');
            time.Append(now.Hour); time.Append('_');
            time.Append(now.Minute); time.Append('_');
            time.Append(now.Second); time.Append('_');
            time.Append(now.Millisecond);
            return time.ToString();
        }
        public DeviceLog()
        {
            m_bLogConsole = true;
            m_DeviceID = "";
            InitLog();
        }
        ~DeviceLog()
        {
            if(m_File != null)
                m_File.Close();
        }
        public bool InitLog()
        {
            string path = System.Environment.CurrentDirectory + "\\Log\\";
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
            m_LogFileName = path + "LogFile_" + GetDataTimeString() + ".log";
            try
            {
                m_File = new System.IO.FileStream(m_LogFileName, System.IO.FileMode.OpenOrCreate);
                m_CreateTime = DateTime.Now;
                return true;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.ToString());
                return false;
            }
        }
        public void TimeCheck()
        {
            while (true)
            {
                if (m_CreateTime.Day != DateTime.Now.Day)
                {
                    if (m_File != null)
                        m_File.Close();
                    lock (m_File)
                    {
                        InitLog();
                    }
                }
                System.Threading.Thread.Sleep(2000);
            }
        }
        public bool WriteLog(string strStream, bool newLine = true, bool debug = false)
        {
            string time = "[" + System.DateTime.Now.ToString() + "]:";
            if (newLine)
                strStream = strStream + System.Environment.NewLine;
            byte[] buff = System.Text.Encoding.Default.GetBytes(time + strStream);

            if (m_bLogConsole == true)
            {
                if (m_DeviceID.Length > 0)
                {
                    if (strStream.Contains(m_DeviceID) == true)
                        Console.Write(time + strStream);
                }
                else
                {
                    Console.Write(time + strStream);
                }
            }

            if (m_CreateTime.Day != DateTime.Now.Day)
            {
                if (m_File != null)
                    m_File.Close();
                InitLog();
            }
            if (m_File != null && debug == false)
            {
                m_File.Write(buff, 0, buff.Length);
                m_File.Flush();
                return true;
            }
            return false;
        }
        private void LogMemery()
        {
            System.Diagnostics.Process process = System.Diagnostics.Process.GetCurrentProcess();
            string str = "ProcessName:" + process.ProcessName; WriteLog(str);
            str = "Threads:" + process.Threads; WriteLog(str);
            str = "BasePriority:" + process.BasePriority; WriteLog(str);
            str = "TotalProcessorTime:" + process.TotalProcessorTime; WriteLog(str);
            str = "HandleCount:" + process.HandleCount; WriteLog(str);
            str = "PrivilegedProcessorTime" + process.PrivilegedProcessorTime; WriteLog(str);
            str = "UserProcessorTime:" + process.UserProcessorTime; WriteLog(str);
            str = "WorkingSet64:" + process.WorkingSet64 / 1024 + "K"; WriteLog(str);
            str = "PagedMemorySize64:" + process.PagedMemorySize64 / 1024 + "K"; WriteLog(str);
            str = "PagedSystemMemorySize64:" + process.PagedSystemMemorySize64 / 1024 + "K"; WriteLog(str);
            str = "PeakPagedMemorySize64:" + process.PeakPagedMemorySize64 / 1024 + "K"; WriteLog(str);
            str = "PeakVirtualMemorySize64:" + process.PeakVirtualMemorySize64 / 1024 + "K"; WriteLog(str);
            str = "PeakWorkingSet64:" + process.PeakWorkingSet64 / 1024 + "K"; WriteLog(str);
            str = "PrivateMemorySize64:" + process.PrivateMemorySize64 / 1024 + "K"; WriteLog(str);
            str = "VirtualMemorySize64:" + process.VirtualMemorySize64 / 1024 + "K"; WriteLog(str);
            process.Close();
        }
    }
}
