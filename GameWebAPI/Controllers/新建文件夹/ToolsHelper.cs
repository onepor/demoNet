using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Xml;

namespace GameWebAPI.Utils
{
    /// <summary>
    /// 工具帮助类
    /// </summary>
    public class ToolsHelper
    {

        #region Json 格式



        /// <summary>
        /// JsonHttp（post）
        /// </summary>
        /// <param name="serviceAddress"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string JsonHttpClient(string serviceAddress, string content)
        {
            string retData = string.Empty;
            try
            {
                //string serviceAddress = HTTPAdress + methodname + "";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(serviceAddress);

                request.Method = "POST";
                request.ContentType = "application/json";
                string strContent = content;
                using (StreamWriter dataStream = new StreamWriter(request.GetRequestStream()))
                {
                    dataStream.Write(strContent);
                    dataStream.Close();
                }
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                string encoding = response.ContentEncoding;
                if (encoding == null || encoding.Length < 1)
                {
                    encoding = "UTF-8"; //默认编码  
                }
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding));
                string retString = reader.ReadToEnd();
                //解析josn
                retData = retString;
            }
            catch (Exception ex)
            {
                retData = ex.Message;
            }
            return retData;
        }



        #region Http调用 （Habanero游戏）

        //private string HTTPAdress = ConfigurationManager.AppSettings["HBDefaultHTTPAdress"];

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="method"></param>
        ///// <param name="data"></param>
        ///// <returns></returns>
        //public string httpPostMethod(string method, string data)
        //{
        //    string result = string.Empty;
        //    string url = HTTPAdress + method;
        //    try
        //    {
        //        var httpRequest = (HttpWebRequest)WebRequest.Create(url);
        //        var postData = Encoding.ASCII.GetBytes(data);
        //        httpRequest.KeepAlive = false;
        //        httpRequest.Method = "POST";
        //        httpRequest.ContentType = "application/json";
        //        var newStream = httpRequest.GetRequestStream();
        //        newStream.Write(postData, 0, data.Length);

        //        var httpResponse = (HttpWebResponse)httpRequest.GetResponse();

        //        var responseStream = httpResponse.GetResponseStream();
        //        var reader = new StreamReader(responseStream, Encoding.UTF8);

        //        result = reader.ReadToEnd();
        //        responseStream.Close();
        //        reader.Close();
        //        httpResponse.Close();

        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return result;
        //}


        ///// <summary>
        ///// 接口统一调用方法
        ///// </summary>
        ///// <param name="methodname"></param>
        ///// <param name="content"></param>
        ///// <returns></returns>
        //public string PostFunction(string methodname, string content)
        //{
        //    string retData = string.Empty;
        //    try
        //    {
        //        string serviceAddress = HTTPAdress + methodname + "";
        //        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(serviceAddress);

        //        request.Method = "POST";
        //        request.ContentType = "application/json";
        //        string strContent = content;
        //        using (StreamWriter dataStream = new StreamWriter(request.GetRequestStream()))
        //        {
        //            dataStream.Write(strContent);
        //            dataStream.Close();
        //        }
        //        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        //        string encoding = response.ContentEncoding;
        //        if (encoding == null || encoding.Length < 1)
        //        {
        //            encoding = "UTF-8"; //默认编码  
        //        }
        //        StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding));
        //        string retString = reader.ReadToEnd();
        //        //解析josn
        //        retData = retString;
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return retData;
        //}

        #endregion

        #endregion

        #region XML格式

        private static string PNGURL = ConfigurationManager.AppSettings["PNGDefaultHTTPAdress"];

        /// <summary>
        /// soapHttp（账号验证）
        /// </summary>
        /// <param name="body"></param>
        /// <param name="Method"></param>
        /// <param name="httpStatusCode"></param>
        /// <returns></returns>
        public static string PostHttpConnectXML(string body, string Method,out int httpStatusCode)
        {
            httpStatusCode = 200;

            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(PNGURL);

                httpWebRequest.ContentType = "text/xml; charset=utf-8";
                httpWebRequest.Method = "POST";
                httpWebRequest.Timeout = 20000;

                byte[] btBodys = Encoding.UTF8.GetBytes(body);
                httpWebRequest.ContentLength = btBodys.Length;
                httpWebRequest.GetRequestStream().Write(btBodys, 0, btBodys.Length);
                NetworkCredential nc = new NetworkCredential("qaiapi ", "zjKGJUzgDscqdagnyYbmxoWGM");
                httpWebRequest.Credentials = nc;
                httpWebRequest.Headers.Add("SOAPAction", "http://playngo.com/v1/" + Method);
                HttpWebResponse httpWebResponse;
                try
                {
                    httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    //httpStatusCode = Convert.ToInt32(httpWebRequest.StatusCode);
                }
                catch (WebException ex)
                {
                    httpWebResponse = (HttpWebResponse)ex.Response;
                    httpStatusCode = Convert.ToInt32(ex.Status);
                }
                StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.UTF8);
                string responseContent = streamReader.ReadToEnd();
                
                httpWebResponse.Close();
                streamReader.Close();
                httpWebRequest.Abort();
                httpWebResponse.Close();

                return responseContent;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// XML格式解析成json文件
        /// </summary>
        /// <param name="strxml"></param>
        /// <returns></returns>
        public static string XMLToJson(string strxml)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                if (!isXmlDocument(xmlDoc, strxml))
                {
                    return strxml;
                }
                xmlDoc.LoadXml(strxml);//Load加载XML文件，LoadXML加载XML字符串
                string jsonText = Newtonsoft.Json.JsonConvert.SerializeXmlNode(xmlDoc, Newtonsoft.Json.Formatting.None, true);
                return jsonText;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// 判断xml格式
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <param name="rtnMsg"></param>
        /// <returns></returns>
        private static bool isXmlDocument(XmlDocument xmlDoc, string rtnMsg)
        {
            bool flag = true;
            try
            {
                xmlDoc.LoadXml(rtnMsg);
            }
            catch (Exception e)
            {
                flag = false;
            }
            return flag;
        }


        #region Soap协议调用（PNG游戏）

        //private static string PNGURL = ConfigurationManager.AppSettings["PNGDefaultHTTPAdress"];

        ///// <summary>
        ///// Soap协议Post方法
        ///// </summary>
        ///// <param name="URL">WebService地址</param>
        ///// <param name="str">传入Soap协议格式数据</param>
        ///// <returns></returns>
        //public static string HttpConnectToServer(string str, string Method)//byte[]
        //{
        //    byte[] dataArray;

        //    //var xml = Encoding.UTF8.GetString(str, 0, str.Length);
        //    dataArray = Encoding.UTF8.GetBytes(str);

        //    //创建请求
        //    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(PNGURL);
        //    request.Method = "POST";
        //    request.ContentLength = dataArray.Length;
        //    request.ContentType = "text/xml; charset=utf-8";//"text/xml; charset=utf-8";
        //    NetworkCredential nc = new NetworkCredential("qaiapi ", "zjKGJUzgDscqdagnyYbmxoWGM");//  --账号权限问题
        //    request.Credentials = nc;
        //    request.Headers.Add("SOAPAction", "http://playngo.com/v1/CasinoGameService/" + Method);

        //    //创建输入流
        //    Stream dataStream = null;
        //    try
        //    {
        //        dataStream = request.GetRequestStream();
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message.ToString();//连接服务器失败
        //    }

        //    //发送请求
        //    dataStream.Write(dataArray, 0, dataArray.Length);
        //    dataStream.Close();
        //    //读取返回消息
        //    string res = string.Empty;
        //    HttpWebResponse response;
        //    try
        //    {
        //        response = (HttpWebResponse)request.GetResponse();
        //    }
        //    catch (WebException ex)
        //    {
        //        response = (HttpWebResponse)ex.Response;
        //        //return ex.ToString();//连接服务器失败
        //    }
        //    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
        //    res = reader.ReadToEnd();
        //    reader.Close();
        //    return res;

        //    ////发送请求
        //    //dataStream.Write(dataArray, 0, dataArray.Length);
        //    //dataStream.Close();
        //    ////读取返回消息
        //    //string res = string.Empty;
        //    //try
        //    //{
        //    //    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        //    //    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
        //    //    res = reader.ReadToEnd();
        //    //    reader.Close();
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    return ex.Message.ToString();//连接服务器失败
        //    //}
        //    //return res;
        //}

        //public static string PostHttp(string body, string Method)
        //{
        //    try
        //    {
        //        HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(PNGURL);

        //        httpWebRequest.ContentType = "text/xml; charset=utf-8";
        //        httpWebRequest.Method = "POST";
        //        httpWebRequest.Timeout = 20000;

        //        byte[] btBodys = Encoding.UTF8.GetBytes(body);
        //        httpWebRequest.ContentLength = btBodys.Length;
        //        httpWebRequest.GetRequestStream().Write(btBodys, 0, btBodys.Length);
        //        NetworkCredential nc = new NetworkCredential("qaiapi ", "zjKGJUzgDscqdagnyYbmxoWGM");
        //        httpWebRequest.Credentials = nc;
        //        httpWebRequest.Headers.Add("SOAPAction", "http://playngo.com/v1/CasinoGameService/" + Method);
        //        HttpWebResponse httpWebResponse;
        //        try
        //        {
        //            httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        //        }
        //        catch (WebException ex)
        //        {
        //            httpWebResponse = (HttpWebResponse)ex.Response;
        //        }
        //        StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.UTF8);
        //        string responseContent = streamReader.ReadToEnd();

        //        httpWebResponse.Close();
        //        streamReader.Close();
        //        httpWebRequest.Abort();
        //        httpWebResponse.Close();

        //        return responseContent;
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;
        //    }
        //}

        ///// <summary>
        ///// 传入xml格式的字符串，解析获取其中的数据
        ///// </summary>
        ///// <param name="strxml"></param>
        ///// <returns></returns>
        //public static string ReturnValue(string strxml)
        //{
        //    string value = "";
        //    try
        //    {
        //        XmlDocument xmlDoc = new XmlDocument();
        //        if (!isXmlDocument(xmlDoc, strxml))
        //        {
        //            return strxml;
        //        }
        //        xmlDoc.LoadXml(strxml);//Load加载XML文件，LoadXML加载XML字符串
        //        XmlElement root = xmlDoc.DocumentElement;
        //        XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);

        //        XmlNode xnode = root.FirstChild;
        //        nsmgr.AddNamespace("s", "http://schemas.xmlsoap.org/soap/envelope/");

        //        value = xnode.SelectSingleNode("//s:Envelope", nsmgr).InnerText;
        //        return value;
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;
        //    }
        //}

        ///// <summary>
        ///// XML格式解析成json文件
        ///// </summary>
        ///// <param name="strxml"></param>
        ///// <returns></returns>
        //public static string XMLToJson(string strxml)
        //{
        //    try
        //    {
        //        XmlDocument xmlDoc = new XmlDocument();
        //        if (!isXmlDocument(xmlDoc, strxml))
        //        {
        //            return strxml;
        //        }
        //        xmlDoc.LoadXml(strxml);//Load加载XML文件，LoadXML加载XML字符串
        //        string jsonText = Newtonsoft.Json.JsonConvert.SerializeXmlNode(xmlDoc, Newtonsoft.Json.Formatting.None, true);
        //        return jsonText;
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;
        //    }
        //}

        ///// <summary>
        ///// 判断xml格式
        ///// </summary>
        ///// <param name="xmlDoc"></param>
        ///// <param name="rtnMsg"></param>
        ///// <returns></returns>
        //private static bool isXmlDocument(XmlDocument xmlDoc, string rtnMsg)
        //{
        //    bool flag = true;
        //    try
        //    {
        //        xmlDoc.LoadXml(rtnMsg);
        //    }
        //    catch (Exception e)
        //    {
        //        flag = false;
        //    }
        //    return flag;
        //}

        #endregion


        #endregion

        #region PayTools

        private static string SignKey = ConfigurationManager.AppSettings["SignKey"];//签名key值

        /// <summary>
        /// 生成签名值
        /// </summary>
        /// <param name="Str"></param>
        /// <returns></returns>
        public static string GetStrSign(string Str)
        {
            Str += SignKey;
            return MD5(Str, "UTF-8").ToUpper();//对stringSignTemp进行MD5运算，再将得到的字符串所有字符转换为大写，得到sign值signValue。 
        }

        /// <summary>
        /// 签名
        /// </summary>
        /// <param name="dicParams">签名参数</param>
        /// <param name="key">加密key值</param>
        /// <returns></returns>
        public static string GetSign(Dictionary<string, string> dicParams, string key)
        {
            //将字典中按ASCII码升序排序
            Dictionary<string, string> dicDestSign = new Dictionary<string, string>();
            dicDestSign = AsciiDictionary(dicParams);
            var sb = new StringBuilder();
            int i = 0;
            foreach (var sA in dicDestSign)//参数名ASCII码从小到大排序（字典序）；
            {
                if (string.IsNullOrEmpty(sA.Value) || string.Compare(sA.Key, "sign", true) == 0)
                {
                    continue;// 参数中为签名的项，不参加计算//参数的值为空不参与签名；
                }
                string value = sA.Value.ToString();

                i++;
                sb.Append(sA.Key).Append("=").Append(sA.Value);
                if (i != dicDestSign.Count)
                {
                    sb.Append("&");
                }
            }
            var string1 = sb.ToString();
            sb.Append(key);//"key=" +在stringA最后拼接上key=(API密钥的值)得到stringSignTemp字符串
            var stringSignTemp = sb.ToString();
            var sign = MD5Encrypt(stringSignTemp);//MD5(stringSignTemp, "UTF-8").ToUpper();//对stringSignTemp进行MD5运算，再将得到的字符串所有字符转换为大写，得到sign值signValue。 

            //string str = string1 + "sign=" + sign;
            //return str;

            return sign;
        }


        #region 未用

        ///// <summary>
        ///// 签名(个码支付需要)
        ///// </summary>
        ///// <param name="dicParams">签名参数</param>
        ///// <returns></returns>
        //public static string GetSignGM(Dictionary<string, string> dicParams, string key)
        //{
        //    //将字典中按ASCII码升序排序
        //    Dictionary<string, string> dicDestSign = new Dictionary<string, string>();
        //    dicDestSign = AsciiDictionary(dicParams);
        //    var sb = new StringBuilder();
        //    foreach (var sA in dicDestSign)//参数名ASCII码从小到大排序（字典序）；
        //    {
        //        if (string.IsNullOrEmpty(sA.Value) || string.Compare(sA.Key, "sign", true) == 0)
        //        {
        //            continue;// 参数中为签名的项，不参加计算//参数的值为空不参与签名；
        //        }
        //        string value = sA.Value.ToString();

        //        sb.Append(sA.Key).Append("=").Append(sA.Value).Append("&");

        //    }
        //    var string1 = sb.ToString();
        //    sb.Append(key);//在stringA最后拼接上 密钥的值 得到stringSignTemp字符串
        //    var stringSignTemp = sb.ToString();
        //    var sign = MD5(stringSignTemp, "UTF-8").ToLower();//对stringSignTemp进行MD5运算，再将得到的字符串所有字符转换为小写，得到sign值signValue。 

        //    string str = string1 + "sign=" + sign;

        //    return str;

        //    //return sign;
        //}
        ///// <summary>
        ///// 签名(四方支付需要)
        ///// </summary>
        ///// <param name="dicParams"></param>
        ///// <param name="key"></param>
        ///// <returns></returns>
        //public static string GetSignSquare(Dictionary<string, string> dicParams, string key)
        //{
        //    //将字典中按ASCII码升序排序
        //    Dictionary<string, string> dicDestSign = new Dictionary<string, string>();
        //    dicDestSign = AsciiDictionary(dicParams);
        //    var sb = new StringBuilder();
        //    foreach (var sA in dicDestSign)//参数名ASCII码从小到大排序（字典序）；
        //    {
        //        if (string.IsNullOrEmpty(sA.Value) || string.Compare(sA.Key, "sign", true) == 0)
        //        {
        //            continue;// 参数中为签名的项，不参加计算//参数的值为空不参与签名；
        //        }
        //        string value = sA.Value.ToString();

        //        sb.Append(sA.Key).Append("=").Append(sA.Value).Append("&");

        //    }
        //    string string1 = sb.ToString();

        //    //sb.Append("key=" + key);//在stringA最后拼接上 密钥的值 得到stringSignTemp字符串
        //    var stringSignTemp = string1.ToLower() + "key=" + key;//将得到的字符串所有字符转换为小写
        //    var sign = MD5(stringSignTemp, "UTF-8");//对stringSignTemp进行MD5运算，得到sign值signValue。 

        //    string str = string1 + "sign=" + sign.ToLower();

        //    return str;

        //    //return sign;
        //}


        ///// <summary>
        ///// 签名（网关2）
        ///// </summary>
        ///// <param name="dicParams">签名参数</param>
        ///// <returns></returns>
        //public static string GetSignPay(Dictionary<string, string> dicParams, string key)
        //{
        //    //将字典中按ASCII码升序排序
        //    Dictionary<string, string> dicDestSign = new Dictionary<string, string>();
        //    dicDestSign = AsciiDictionary(dicParams);
        //    var sb = new StringBuilder();
        //    foreach (var sA in dicDestSign)//参数名ASCII码从小到大排序（字典序）；
        //    {
        //        if (string.IsNullOrEmpty(sA.Value) || string.Compare(sA.Key, "sign", true) == 0)
        //        {
        //            continue;// 参数中为签名的项，不参加计算//参数的值为空不参与签名；
        //        }
        //        string value = sA.Value.ToString();

        //        sb.Append(sA.Key).Append("=").Append(sA.Value).Append("&");

        //    }
        //    var string1 = sb.ToString();
        //    sb.Append("key=" + key);//在stringA最后拼接上key=(API密钥的值)得到stringSignTemp字符串
        //    var stringSignTemp = sb.ToString();
        //    var sign = MD5(stringSignTemp, "UTF-8").ToUpper();//对stringSignTemp进行MD5运算，再将得到的字符串所有字符转换为大写，得到sign值signValue。 

        //    string str = string1 + "pay_md5sign=" + sign;

        //    return str;
        //}

        #endregion


        /// <summary>
        /// 用MD5加密字符串
        /// </summary>
        /// <param name="strobject">待加密的字符串</param>
        /// <returns></returns>
        public static string MD5Encrypt(string strobject)
        {
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            byte[] hashedDataBytes;
            hashedDataBytes = md5Hasher.ComputeHash(Encoding.GetEncoding("gb2312").GetBytes(strobject));
            StringBuilder tmp = new StringBuilder();
            foreach (byte i in hashedDataBytes)
            {
                tmp.Append(i.ToString("x2"));
            }
            return tmp.ToString();
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="encypStr">需要md5加密的字符串</param>
        /// <param name="charset">编码</param>
        /// <returns>返回加密后的MD5字符串</returns>
        public static string MD5(string encypStr, string charset = "UTF-8")
        {
            string retStr = string.Empty;
            MD5CryptoServiceProvider m5 = new MD5CryptoServiceProvider();

            //创建md5对象
            byte[] inputBye;
            byte[] outputBye;

            //使用GB2312编码方式把字符串转化为字节数组．
            try
            {
                inputBye = Encoding.GetEncoding(charset).GetBytes(encypStr);
            }
            catch
            {
                inputBye = Encoding.GetEncoding("GB2312").GetBytes(encypStr);
            }
            outputBye = m5.ComputeHash(inputBye);

            retStr = BitConverter.ToString(outputBye);
            retStr = retStr.Replace("-", "").ToUpper();
            return retStr;
        }

        /// <summary>
        /// 将集合key以ascii码从小到大排序
        /// </summary>
        /// <param name="sArray">源数组</param>
        /// <returns>目标数组</returns>
        public static Dictionary<string, string> AsciiDictionary(Dictionary<string, string> sArray)
        {
            Dictionary<string, string> asciiDic = new Dictionary<string, string>();
            string[] arrKeys = sArray.Keys.ToArray();
            Array.Sort(arrKeys, string.CompareOrdinal);
            foreach (var key in arrKeys)
            {
                string value = sArray[key];
                asciiDic.Add(key, value);
            }
            return asciiDic;
        }


        #region 请求方式


        /// <summary>
        /// 创建http请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string HttpGetWBGJPay(string url)
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";

            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                string retString = myStreamReader.ReadToEnd();
                return retString;
            }
        }

        /// <summary>
        /// HTTP_Post请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">请求数据</param>
        /// <param name="contentType">请求内容数据（可以不用传）</param>
        /// <returns></returns>
        public static string HttpPost(string url, string postData, string contentType = "application/x-www-form-urlencoded")
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = contentType;
            request.Method = "POST";
            request.Timeout = 300000;

            byte[] bytes = Encoding.UTF8.GetBytes(postData);
            request.ContentLength = bytes.Length;
            Stream writer = request.GetRequestStream();
            writer.Write(bytes, 0, bytes.Length);
            writer.Close();
            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch ( WebException ex)
            {
                response = (HttpWebResponse)ex.Response;
            }
            StreamReader reader = new StreamReader(response.GetResponseStream() ?? throw new InvalidOperationException(), Encoding.UTF8);
            string result = reader.ReadToEnd();
            response.Close();
            return result;
        }

        /// <summary>
        /// 调用接口（支付返回HTML）
        /// </summary>
        /// <param name="url"></param>
        /// <param name="datas">提交的参数</param>
        /// <returns></returns>
        public static string HTMLPost(string url, System.Text.StringBuilder datas)
        {
            string result = "";

            byte[] postData = System.Text.Encoding.UTF8.GetBytes(datas.ToString());

            System.Net.HttpWebRequest _webRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
            _webRequest.Method = "POST";
            _webRequest.ContentType = "application/json";
            //内容类型  
            //_webRequest.ContentType = "application/x-www-form-urlencoded";
            _webRequest.Timeout = 1000 * 30;
            _webRequest.ContentLength = postData.Length;

            using (System.IO.Stream reqStream = _webRequest.GetRequestStream())
            {
                reqStream.Write(postData, 0, postData.Length);
                reqStream.Close();
            }

            System.Net.HttpWebResponse resp = (System.Net.HttpWebResponse)_webRequest.GetResponse();
            System.IO.Stream stream = resp.GetResponseStream();
            //获取响应内容
            using (System.IO.StreamReader reader = new System.IO.StreamReader(stream, System.Text.Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }

            return result;
        }

        #endregion

        #endregion


        #region 路虎游戏


        /// <summary>
        /// 加密的密码
        /// </summary>
        public const string iKey = "bBgnFfM/aV4YXkX7cpLnw9VNIVnYN2yCgD7uYFgss9Y=";//

        /// <summary>
        /// 密钥
        /// </summary>
        public const string iIv = "puTVVsex4Qf2Ls0b0iW2UQ==";//

        /// <summary>
        /// Generate a private key
        /// From : www.chapleau.info/blog/2011/01/06/usingsimplestringkeywithaes256encryptioninc.html
        /// <param name="iKeySize">key值长度</param>
        /// </summary>
        public static string GenerateKey(int iKeySize=256)
        {
            RijndaelManaged aesEncryption = new RijndaelManaged();
            aesEncryption.KeySize = iKeySize;
            aesEncryption.BlockSize = 128;
            aesEncryption.Mode = CipherMode.CBC;
            aesEncryption.Padding = PaddingMode.PKCS7;
            aesEncryption.GenerateIV();
            string ivStr = Convert.ToBase64String(aesEncryption.IV);
            aesEncryption.GenerateKey();
            string keyStr = Convert.ToBase64String(aesEncryption.Key);
            string completeKey = ivStr + "," + keyStr;

            return Convert.ToBase64String(ASCIIEncoding.UTF8.GetBytes(completeKey));
        }


        /// <summary>
        /// AES加密 
        /// </summary>
        /// <param name="iPlainStr">加密字符</param>
        /// <param name="iKeySize">key值长度</param>
        /// <returns></returns>
        public static string AESEncrypt(string iPlainStr, int iKeySize = 256)
        {
            RijndaelManaged aesEncryption = new RijndaelManaged();
            aesEncryption.KeySize = iKeySize;
            aesEncryption.BlockSize = 128;
            aesEncryption.Mode = CipherMode.CBC;
            aesEncryption.Padding = PaddingMode.PKCS7;
            aesEncryption.IV = Convert.FromBase64String(iIv);
            aesEncryption.Key = Convert.FromBase64String(iKey);
            byte[] plainText = ASCIIEncoding.UTF8.GetBytes(iPlainStr);
            ICryptoTransform crypto = aesEncryption.CreateEncryptor();
            byte[] cipherText = crypto.TransformFinalBlock(plainText, 0, plainText.Length);
            return Convert.ToBase64String(cipherText);

        }
        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="iEncryptedText"></param>
        /// <param name="iKeySize"></param>
        /// <returns></returns>
        public static string AESDecrypt(string iEncryptedText, int iKeySize = 256)
        {
            RijndaelManaged aesEncryption = new RijndaelManaged();
            aesEncryption.KeySize = iKeySize;
            aesEncryption.BlockSize = 128;
            aesEncryption.Mode = CipherMode.CBC;
            aesEncryption.Padding = PaddingMode.PKCS7;
            aesEncryption.IV = Convert.FromBase64String(iIv);
            aesEncryption.Key = Convert.FromBase64String(iKey);
            ICryptoTransform decrypto = aesEncryption.CreateDecryptor();
            byte[] encryptedBytes = Convert.FromBase64CharArray(iEncryptedText.ToCharArray(), 0, iEncryptedText.Length);
            return ASCIIEncoding.UTF8.GetString(decrypto.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length));

        }

        #endregion




        /// <summary> 
        /// 获取时间戳 
        /// </summary> 
        /// <returns></returns> 
        public static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        /// <summary>
        /// 把字符串加密（返回）
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetMd5(string str)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string a = BitConverter.ToString(md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(str)));
            a = a.Replace("-", "");
            return a;
        }
    }
}