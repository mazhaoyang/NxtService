using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolLibrary
{
    public class CustomRandom
    {
        private Random m_Rnd;
        private string m_Base = @"0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private static CustomRandom m_Single = null;
        private static readonly object SynObject = new object();
        public static CustomRandom GetInstance
        {
            get
            {
                if (m_Single == null)
                {
                    lock (SynObject)
                    {
                        return m_Single ?? (m_Single = new CustomRandom());
                    }
                }
                return m_Single;
            }
        }
        CustomRandom()
        {
            m_Rnd = new Random();
        }
        public int RInt()
        {
            return m_Rnd.Next();
        }
        public int RInt(int maxValue)
        {
            return m_Rnd.Next(maxValue);
        }
        public int RInt(int minValue,int maxValue)
        {
            return m_Rnd.Next(minValue,maxValue);
        }
        public string GetNumber()
        {
            return m_Rnd.Next(10).ToString();
        }
        public string GetLLetter()
        {
            return m_Base.Substring(10 + m_Rnd.Next(26), 1);
        }
        public string GetULetter()
        {
            return m_Base.Substring(36 + m_Rnd.Next(26), 1);
        }
        public string GetLULetter()
        {
            return m_Base.Substring(10 + m_Rnd.Next(52), 1);
        }
        public string GetNumberAndLLetter()
        {
            return m_Base.Substring(m_Rnd.Next(36), 1);
        }
        public string GetNumberAndULetter()
        {
            return GetNumberAndLLetter().ToUpper();
        }
        public string GetRandomString()
        {
            return m_Base.Substring(0 + m_Rnd.Next(61), 1);
        }
        public string GetRandomString(int length)
        {
            byte[] data = new byte[length];
            m_Rnd.NextBytes(data);
            return System.Text.Encoding.Default.GetString(data);
        }
        public double GetDouble()
        {
            return m_Rnd.NextDouble();
        }
        public char GetUpperChar()
        {
            // A-Z ASCII Value 65-90
            return (char)m_Rnd.Next(65, 91);
        }
        public char GetLowerChar()
        {
            // a-z ASCII Value 97-122
            return (char)m_Rnd.Next(97, 123);
        }
        public char GetChar()
        {
            char c = (char)m_Rnd.Next(65, 123);
            if (char.IsLetter(c))
                return c;
            else
                return GetChar();
        }
        public char GetLCharNumber()
        {
            char r = (char)m_Rnd.Next(0, 123);
            if(char.IsLower(r) || char.IsNumber(r))
                return r;
            else
                return GetLCharNumber();
        }
        public char GetUCharNumber()
        {
            char r = (char)m_Rnd.Next(0, 123);
            if (char.IsUpper(r) || char.IsNumber(r))
                return r;
            else
                return GetUCharNumber();
        }
        public char GetLUChar()
        {
            char r = (char)m_Rnd.Next(0, 123);
            if (char.IsUpper(r) || char.IsUpper(r))
                return r;
            else
                return GetLUChar();
        }
    }
}
