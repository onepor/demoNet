using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    /// <summary>
    /// 编码转换
    /// </summary>
    public class CodeConverUtils
    {

        #region Unicode 编码转换


        /// <summary>
        /// String to Unicode
        /// </summary>
        /// <param name="str">String</param>
        /// <returns></returns>
        public static StringBuilder StrToUnicode(string str)
        {
            StringBuilder strResult = new StringBuilder();
            for (int i = 0; i < str.Length; i++)
            {
                strResult.Append("\\u");
                strResult.Append(((int)str[i]).ToString("x"));
            }
            return strResult;
        }

        /// <summary>
        /// Unicode to String
        /// </summary>
        /// <param name="str">Unicode</param>
        /// <returns></returns>
        public static string UnicodeToStr(string str)
        {
            StringBuilder strResult = new StringBuilder();
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"(?i)\\[uU]([0-9a-f]{4})");
            return reg.Replace(str, delegate (System.Text.RegularExpressions.Match m)
            {
                return ((char)Convert.ToInt32(m.Groups[1].Value, 16)).ToString();
            });
        }


        #endregion

    }
}
