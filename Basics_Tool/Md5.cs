using System;

namespace Basics_Tool
{
    public class Md5
    {
        /// <summary>
        /// 获取MD5加密字符串
        /// </summary>
        /// <param name="dataStr">待加密字符串</param>
        /// <param name="codeType">编码格式</param>
        /// <returns></returns>
        public static string GetMD5(string dataStr, string codeType)
        {
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(System.Text.Encoding.GetEncoding(codeType).GetBytes(dataStr));
            System.Text.StringBuilder sb = new System.Text.StringBuilder(32);
            for (int i = 0; i < t.Length; i++)
            {
                sb.Append(t[i].ToString("x").PadLeft(2, '0'));
            }
            return sb.ToString();
        }
        /// <summary>
        /// 获取MD5加密字符串 默认中文编码
        /// </summary>
        /// <param name="dataStr">待加密字符串</param>
        /// <returns></returns>
        public static string GetMD5(string dataStr)
        {
            return GetMD5(dataStr, "gb2312");
        }
    }
}
