using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Pipes;

namespace ToolLibrary
{
    public class PipeStream
    {
        public enum PipeType
        {
            PipeServer,
            PipeClient
        }

        NamedPipeServerStream m_PipeServer;
        NamedPipeClientStream m_PipeClient;
        StreamReader m_PipeServerStream;
        StreamWriter m_PipeClientStream;
        public PipeStream(string name,PipeType type)
        {
            m_PipeServer = null;
            m_PipeClient = null;
            if (type == PipeType.PipeClient)
            {
                m_PipeClient = new NamedPipeClientStream(name);
                m_PipeClientStream = new StreamWriter(m_PipeClient);

                m_PipeClient.Connect();
            }
            if (type == PipeType.PipeServer)
            {
                m_PipeServer = new NamedPipeServerStream(name);
                m_PipeServerStream = new StreamReader(m_PipeServer);
            }
        }
        ~PipeStream()
        {
            if (m_PipeClient != null)
                m_PipeClient.Close();
            if (m_PipeServerStream != null)
                m_PipeServerStream.Close();
        }
        public string Read()
        {
            return m_PipeServerStream.ReadLine();
        }
        public void Write(string buff)
        {
            m_PipeClientStream.WriteLine(buff);
        }
    }
}
