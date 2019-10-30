using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_Base
{
    public interface InterfaceEnDecryp
    {
        /// <summary>
        /// 生成key
        /// </summary>
        /// <returns></returns>
        string Generator();
        /// <summary>
        /// 加密数据
        /// </summary>
        /// <param name="password"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        string DESEncrypt(string password, string key);
        /// <summary>
        /// 解密数据
        /// </summary>
        /// <param name="password"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        string DESDecrypt(string password, string key);
        /// <summary>
        /// 加密数据   MD5方式
        /// </summary>
        /// <param name="password"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        string DESEncryptMD5(string str, string key);
        /// <summary>
        /// 解密数据   MD5方式
        /// </summary>
        /// <param name="password"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        string DESDecryptMD5(string str, string key);

    }
}
