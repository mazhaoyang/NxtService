using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;

namespace ToolLibrary
{
    public class SmsManager : Singleton<SmsManager>
    {
        string mobilnumber;//上行手机号码
        public bool SendMessage(string phoneNumber, string content)
        {
            mobilnumber = phoneNumber;
            string msg = System.Web.HttpUtility.UrlEncode(content, System.Text.Encoding.GetEncoding("GBK"));
            string username = System.Web.HttpUtility.UrlEncode("hebidx", System.Text.Encoding.GetEncoding("GBK"));
            string password = MD5("hebidxhebidx@nxt" + mobilnumber, 16).ToString();
            StringBuilder url = new StringBuilder();
            url.Append("http://202.127.45.87/12316/api/SendSMS.jsp?");
            url.Append("spnumber=12316002011&");
            url.Append("password=" + password + "&");
            url.Append("msg=" + msg + "&");
            url.Append("username=" + username + "&");
            url.Append("mobilephones=" + mobilnumber);
            try
            {
                WebClient wClient = new WebClient();
                byte[] buffer = wClient.DownloadData(url.ToString());
                string result = System.Text.Encoding.UTF8.GetString(buffer);
                if (RegexHtml(result.ToLower()).Equals("ok"))
                {
                    return true;
                }
                return false;
            }
            catch { return false; }
        }

        private string MD5(string str, int code)
        {
            if (code == 16) //小16位MD5加密（取32位加密的9~25字符）  
            {
                return System.Web.Security.FormsAuthentication.
                    HashPasswordForStoringInConfigFile(str, "MD5").ToLower().Substring(8, 16);
            }
            else//小32位加密  
            {
                return System.Web.Security.FormsAuthentication.
                    HashPasswordForStoringInConfigFile(str, "MD5").ToLower();
            }
        }
        public static string RegexHtml(string content)
        {
            string regexText = @"(?<=<body>)[\S\s]*?(?=</body>)";
            Regex regex = new Regex(regexText);
            Match match = regex.Match(content);
            return match.Value.Trim();
        }
    }
}
