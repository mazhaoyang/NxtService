using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolLibrary
{
    public abstract class Singleton<T> where T : new()
    {
        private static T m_Single;
        private static readonly object SynObject = new object();
        public static T GetInstance
        {
            get
            {
                if (m_Single == null)
                {
                    lock (SynObject)
                    {
                        if (null == m_Single)
                            m_Single = new T();

                        return m_Single;  
                    }
                }
                return m_Single;
            }
        }
    }
}
