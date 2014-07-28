using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace ToolLibrary
{
    public abstract class CommonBase
    {
        public CommonBase()
        { }
        ~CommonBase()
        { }
        public abstract bool Init();
        public abstract string GetState();
        public abstract bool Start();
        public abstract bool End();
        public abstract bool Loop();
    }

    public delegate int CommandDelegate(byte[] buff, int length, EndPoint ip);
}
