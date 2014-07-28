using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using System.Threading;

namespace ToolLibrary
{
    public class MemoryShare
    {
        public enum MFileAccess
        {
            ReadWrite = MemoryMappedFileAccess.ReadWrite,
            Read = MemoryMappedFileAccess.Read,
            Write = MemoryMappedFileAccess.Write,
            CopyOnWrite = MemoryMappedFileAccess.CopyOnWrite,
            ReadExecute = MemoryMappedFileAccess.ReadExecute,
            ReadWriteExecute = MemoryMappedFileAccess.ReadWriteExecute,
        }
        MemoryMappedFile m_MemoryFile;
        Mutex m_Mutex;
        public MemoryShare(string name,long size,MFileAccess mfa)
        {
            try
            {
                m_Mutex = new Mutex(false, "ParamControlMemoryShare");
                m_MemoryFile = MemoryMappedFile.CreateOrOpen(name, size, (MemoryMappedFileAccess)mfa);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        public void Write(string buff)
        {
            try
            {
                m_Mutex.WaitOne();
                using (MemoryMappedViewStream stream = m_MemoryFile.CreateViewStream()) //创建文件内存视图流
                {
                    var writer = new BinaryWriter(stream);
                    writer.Write(buff);
#if DEBUG
                    Console.WriteLine("写入控制流:{0}", buff);
#endif
                }
                m_Mutex.ReleaseMutex();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }            
        }
        public string Read()
        {
            string buff = null;
            try 
            {
                m_Mutex.WaitOne();
                using (MemoryMappedViewStream stream = m_MemoryFile.CreateViewStream(0,32)) //创建文件内存视图流
                {
                    var reader = new StreamReader(stream);
                    buff = reader.ReadLine();
#if DEBUG
                    if(buff != null)
                        Console.WriteLine("读取控制流:{0}", buff);
#endif
                }
                m_Mutex.ReleaseMutex();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            
            return buff;
        }
    }
}
