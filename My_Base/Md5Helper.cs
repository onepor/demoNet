using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace My_Base
{
    /// <summary>
    /// Md5 、 SHA  Irreversible   
    /// </summary>
    public class Md5Helper
    {
        
        #region Md5 ---------------------------------------------------------------------------------------------------

        /// <summary>
        /// MD5 16位加密，不可逆
        /// </summary>
        /// <param name="password"></param> 
        public string Encrypt16Bit(string password)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string str = BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(password)), 4, 8);
            str = str.Replace("-", "");
            return str;
        }

        /// <summary>
        ///  MD5 32位加密，不可逆
        /// </summary>
        /// <param name="password"></param> 
        public string Encrypt32Bit(string password)
        {
            string pwd = "";
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] str = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
            for (int i = 0; i < str.Length; i++)         // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
                pwd = pwd + str[i].ToString("X");        // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符 
            return pwd;
        }

        /// <summary>
        ///  MD5 64位加密，不可逆
        /// </summary>
        /// <param name="password"></param> 
        public string Encrypt64Bit(string password)
        {
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] str = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(str);
        }

        #endregion

        #region SHA  ----------------------------------------------------------------------------------------------

        /// <summary>
        /// SHA-1
        /// </summary>
        /// <param name="str"></param> 
        public string SHA_1(string str)
        {
            System.Security.Cryptography.SHA1CryptoServiceProvider SHA1CSP = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            byte[] bytValue = System.Text.Encoding.UTF8.GetBytes(str);
            byte[] bytHash = SHA1CSP.ComputeHash(bytValue);
            SHA1CSP.Clear();
            string hashstr = "", tempstr = "";
            for (int counter = 0; counter < bytHash.Count(); counter++)
            {
                long i = bytHash[counter] / 16;
                if (i > 9)
                    tempstr = ((char)(i - 10 + 0x41)).ToString();
                else
                    tempstr = ((char)(i + 0x30)).ToString();
                i = bytHash[counter] % 16;
                if (i > 9)
                    tempstr += ((char)(i - 10 + 0x41)).ToString();
                else
                    tempstr += ((char)(i + 0x30)).ToString();
                hashstr += tempstr;
            }
            return hashstr;
        }

        /// <summary>
        /// SHA-256
        /// </summary>
        /// <param name="str"></param> 
        public string SHA_256(string str)
        {
            System.Security.Cryptography.SHA256CryptoServiceProvider SHA256CSP = new System.Security.Cryptography.SHA256CryptoServiceProvider();
            byte[] bytValue = System.Text.Encoding.UTF8.GetBytes(str);
            byte[] bytHash = SHA256CSP.ComputeHash(bytValue);
            SHA256CSP.Clear();
            string hashstr = "", tempstr = "";
            for (int counter = 0; counter < bytHash.Count(); counter++)
            {
                long i = bytHash[counter] / 16;
                if (i > 9)
                    tempstr = ((char)(i - 10 + 0x41)).ToString();
                else
                    tempstr = ((char)(i + 0x30)).ToString();
                i = bytHash[counter] % 16;
                if (i > 9)
                    tempstr += ((char)(i - 10 + 0x41)).ToString();
                else
                    tempstr += ((char)(i + 0x30)).ToString();
                hashstr += tempstr;
            }
            return hashstr;
        }

        /// <summary>
        /// SHA-384
        /// </summary>
        /// <param name="str"></param> 
        public string SHA_384(string str)
        {
            System.Security.Cryptography.SHA384CryptoServiceProvider SHA384CSP = new System.Security.Cryptography.SHA384CryptoServiceProvider();
            byte[] bytValue = System.Text.Encoding.UTF8.GetBytes(str);
            byte[] bytHash = SHA384CSP.ComputeHash(bytValue);
            SHA384CSP.Clear();
            string hashstr = "", tempstr = "";
            for (int counter = 0; counter < bytHash.Count(); counter++)
            {
                long i = bytHash[counter] / 16;
                if (i > 9)
                    tempstr = ((char)(i - 10 + 0x41)).ToString();
                else
                    tempstr = ((char)(i + 0x30)).ToString();
                i = bytHash[counter] % 16;
                if (i > 9)
                    tempstr += ((char)(i - 10 + 0x41)).ToString();
                else
                    tempstr += ((char)(i + 0x30)).ToString();
                hashstr += tempstr;
            }
            return hashstr;
        }

        /// <summary>
        /// SHA-512
        /// </summary>
        /// <param name="str"></param> 
        public string SHA_512(string str)
        {
            System.Security.Cryptography.SHA512CryptoServiceProvider SHA512CSP = new System.Security.Cryptography.SHA512CryptoServiceProvider();
            byte[] bytValue = System.Text.Encoding.UTF8.GetBytes(str);
            byte[] bytHash = SHA512CSP.ComputeHash(bytValue);
            SHA512CSP.Clear();
            string hashstr = "", tempstr = "";
            for (int counter = 0; counter < bytHash.Count(); counter++)
            {
                long i = bytHash[counter] / 16;
                if (i > 9)
                    tempstr = ((char)(i - 10 + 0x41)).ToString();
                else
                    tempstr = ((char)(i + 0x30)).ToString();
                i = bytHash[counter] % 16;
                if (i > 9)
                    tempstr += ((char)(i - 10 + 0x41)).ToString();
                else
                    tempstr += ((char)(i + 0x30)).ToString();
                hashstr += tempstr;
            }
            return hashstr;
        }
        #endregion
    }
}
