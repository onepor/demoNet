using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;

namespace My_Base
{
    public class DesHelpercs: InterfaceEnDecryp
    {
        private static string Key = "wangrocs";//key值必须为8位(des),
        /// <summary>
        /// 生成key
        /// </summary>
        public string Generator()
        {
            //var flag = "BigRoc";
            DESCryptoServiceProvider des = (DESCryptoServiceProvider)DESCryptoServiceProvider.Create();
            var key = ASCIIEncoding.ASCII.GetString(des.Key);
            return key;
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="password"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string DESEncrypt(string password, string key = null)
        {
            if (key == null)
            {
                key = Key;
            }
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(password);
                DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
                DES.Key = ASCIIEncoding.ASCII.GetBytes(key);
                DES.IV = ASCIIEncoding.ASCII.GetBytes(key);
                ICryptoTransform desEncrypt = DES.CreateEncryptor();
                byte[] result = desEncrypt.TransformFinalBlock(data, 0, data.Length);
                return BitConverter.ToString(result);
            }
            catch (Exception ex)
            {
                return "DES加密失败：" + ex.Message + "";
                //throw ex;
            }
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="password"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string DESDecrypt(string password, string key = null)
        {
            if (key == null)
            {
                key = Key;
            }
            try
            {
                string[] sinput = password.Split("-".ToCharArray());
                byte[] data = new byte[sinput.Length];
                for (int i = 0; i < sinput.Length; i++)
                {
                    data[i] = byte.Parse(sinput[i], NumberStyles.HexNumber);
                }
                DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
                DES.Key = ASCIIEncoding.ASCII.GetBytes(key);
                DES.IV = ASCIIEncoding.ASCII.GetBytes(key);
                ICryptoTransform desencrypt = DES.CreateDecryptor();
                byte[] result = desencrypt.TransformFinalBlock(data, 0, data.Length);
                return Encoding.UTF8.GetString(result);
            }
            catch (Exception ex)
            {
                return "DES解密失败："+ex.Message+"";
                //throw;
            }
        }

        /// <summary>   
        /// 加密数据   MD5方式
        /// </summary>   
        /// <param name="str"></param>   
        /// <param name="key">加密密钥</param>   
        /// <returns></returns>   
        public string DESEncryptMD5(string str, string key = null)
        {
            if (key == null)
            {
                key = Key;
            }
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            StringBuilder ret = new StringBuilder();
            try
            {
                byte[] inputByteArray;
                inputByteArray = Encoding.Default.GetBytes(str);
                des.Key = ASCIIEncoding.ASCII.GetBytes(FormsAuthentication.HashPasswordForStoringInConfigFile(key, "md5").Substring(0, 8));
                des.IV = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(key, "md5").Substring(0, 8));
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                foreach (byte b in ms.ToArray())
                    ret.AppendFormat("{0:X2}", b);
            }
            catch (Exception ex)
            {
                return "MD5加密失败：" + ex.Message+"";
                //throw ex;
            }
            return ret.ToString();
        }

        /// <summary>   
        /// 解密数据   MD5方式
        /// </summary>   
        /// <param name="str"></param>   
        /// <param name="key">加密密钥</param>   
        /// <returns></returns>   
        public string DESDecryptMD5(string str, string key = null)
        {
            if (key == null)
            {
                key = Key;
            }
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            try
            {
                int len;
                len = str.Length / 2;
                byte[] inputByteArray = new byte[len];
                int x, i;
                for (x = 0; x < len; x++)
                {
                    i = Convert.ToInt32(str.Substring(x * 2, 2), 16);
                    inputByteArray[x] = (byte)i;
                }
                des.Key = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(key, "md5").Substring(0, 8));
                des.IV = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(key, "md5").Substring(0, 8));
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
            }
            catch (Exception ex)
            {
                return "MD5解密失败："+ex.Message+"";
                //throw ex;
            }
            return Encoding.Default.GetString(ms.ToArray());
        }
    }
}
