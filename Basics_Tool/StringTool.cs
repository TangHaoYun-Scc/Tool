using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basics_Tool
{
    /// <summary>
    /// 字符串工具类
    /// </summary>
    public class StringTool
    {
        /// <summary>
        /// 过滤重复的字符串
        /// eg: "aaa,bvv,ass,aaa,adf,bvv"
        /// -->"aaa,bvv,ass,adf"
        /// thy|2021年1月28日16:09:12
        /// </summary>
        /// <param name="Str"></param>
        /// <returns></returns>
        public static string FilterDoubleInStrList(string Str)
        {
            return FilterDoubleInStrList(Str, ",");
        }
        public static string FilterDoubleInStrList(string strX, string strSplit)
        {
            string strTemp = "", strRet = "";
            //string strSplit = ",";
            int intPos;
            if (strX == "")
                return "";

            while (strX != "")
            {

                intPos = strX.IndexOf(strSplit);
                if (intPos == -1)
                {
                    strRet = strRet + strSplit + strX;
                    strX = "";
                }
                else
                {
                    strTemp = strX.Substring(0, intPos);
                    strX = strX.Substring(intPos) + strSplit;
                    while (strX.IndexOf(strSplit + strTemp + strSplit) != -1)
                    {
                        strX = strX.Replace(strSplit + strTemp + strSplit, strSplit);
                    }
                    strX = strX.Substring(0, strX.Length - 1);
                    strRet = strRet + strSplit + strTemp;

                }
                if (strX.IndexOf(strSplit) == 0)
                {
                    strX = strX.Substring(1);
                }
            }
            if (strRet.IndexOf(strSplit) == 0)
            {
                strRet = strRet.Substring(1);
            }

            return strRet;
        }

        /// <summary>
        /// 统一的将日期转变为固定的字符串格式
        /// 统一格式： 2004年05月01日
        /// tht | 2021年1月28日16:09:03
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static string ToMyDateFormat(DateTime d)
        {
            string str = "";
            str = d.Year.ToString() + "年" + (((int)(100 + d.Month)).ToString()).Substring(1) + "月" + (((int)(100 + d.Day)).ToString()).Substring(1) + "日";
            return str;
        }

        /// <summary>
        /// 标签文本太长时的处理方法，截断一定长度，其余的用....代替
        /// 缺省长度为20
        /// thy | 2021年1月28日16:12:55
        /// </summary>
        /// <param name="strCaption"></param>		
        /// <returns></returns>
        public static string sfLongLableCaption(string strCaption)
        {
            return sfLongLableCaption(strCaption, 20);
        }
        public static string sfLongLableCaption(string strCaption, int length)
        {
            string strRet = strCaption;
            if (strCaption.Length > length)
            {
                strRet = strCaption.Substring(0, length) + "...";
            }
            return strRet;
        }

        /// <summary>
        /// 将字符串转化为Ascii串,各字的ASCII不足3位左边补零
        /// thy | 2021年1月28日16:12:32
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static string String2Ascii(string strValue)
        {
            string sAsc = "";
            ASCIIEncoding asci = new ASCIIEncoding();
            byte[] bts = asci.GetBytes(strValue);
            foreach (byte t in bts)
            {
                sAsc += t.ToString().PadLeft(3, '0');
            }
            return sAsc;
        }

        /// <summary>
        /// ASCII字符串转化为字符串
        /// thy | 2021年1月28日16:12:42
        /// </summary>
        /// <param name="strAsc">长度为3的整数倍</param>
        /// <returns></returns>
        public static string Ascii2String(string strAsc)
        {
            byte[] bts = new byte[strAsc.Length / 3];
            string schar = ""; int j = 0;
            ASCIIEncoding asci = new ASCIIEncoding();
            for (int i = 0; i < strAsc.Length; i = i + 3)
            {
                schar = strAsc.Substring(i, 3);
                bts[j] = byte.Parse(schar);
                j++;
            }
            string str = new String(asci.GetChars(bts));
            return str;
        }
    }
}

