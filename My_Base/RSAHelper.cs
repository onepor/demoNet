using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace My_Base
{
    /// <summary>
    /// RSA  Tools
    /// </summary>
    public class RSAHelper
    {
        /// <summary>
        /// 生成密钥
        /// <param name="PrivateKey">私钥</param>
        /// <param name="PublicKey">公钥</param>
        /// <param name="KeySize">密钥长度：512,1024,2048，4096，8192</param>
        /// </summary>
        public void Generator(out string PrivateKey, out string PublicKey, int KeySize = 2048)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(KeySize);
            PrivateKey = rsa.ToXmlString(true); //将RSA算法的私钥导出到字符串PrivateKey中 参数为true表示导出私钥 true 表示同时包含 RSA 公钥和私钥；false 表示仅包含公钥。
            PublicKey = rsa.ToXmlString(false); //将RSA算法的公钥导出到字符串PublicKey中 参数为false表示不导出私钥 true 表示同时包含 RSA 公钥和私钥；false 表示仅包含公钥。
        }
        /// <summary>
        /// RSA加密 将公钥导入到RSA对象中，准备加密
        /// </summary>
        /// <param name="PublicKey">公钥</param>
        /// <param name="encryptstring">待加密的字符串</param>
        public string RSAEncrypt(string PublicKey, string encryptstring)
        {
            byte[] PlainTextBArray;
            byte[] CypherTextBArray;
            string Result;
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(PublicKey);
            PlainTextBArray = (new UnicodeEncoding()).GetBytes(encryptstring);
            CypherTextBArray = rsa.Encrypt(PlainTextBArray, false);
            Result = Convert.ToBase64String(CypherTextBArray);
            return Result;
        }
        /// <summary>
        /// RSA解密 将私钥导入RSA中，准备解密
        /// </summary>
        /// <param name="PrivateKey">私钥</param>
        /// <param name="decryptstring">待解密的字符串</param>
        public string RSADecrypt(string PrivateKey, string decryptstring)
        {
            byte[] PlainTextBArray;
            byte[] DypherTextBArray;
            string Result;
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(PrivateKey);
            PlainTextBArray = Convert.FromBase64String(decryptstring);
            DypherTextBArray = rsa.Decrypt(PlainTextBArray, false);
            Result = (new UnicodeEncoding()).GetString(DypherTextBArray);
            return Result;
        }
    }
}
