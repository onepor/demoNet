using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Utils
{
    public class HttpHelper
    {


        #region HttpWebReques


        /// <summary>
        /// OAuthHeaders构造
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="apiKey"></param>
        /// <param name="nonce"></param>
        /// <param name="timeStamp"></param>
        /// <param name="sig"></param>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public HttpWebRequest BaseDemo(Uri uri, string apiKey,string nonce,string timeStamp,string sig,string accessToken)
        {

            //构造OAuth头部 
            StringBuilder oauthHeader = new StringBuilder();
            oauthHeader.AppendFormat("OAuth realm=\"\", oauth_consumer_key={0}, ", apiKey);
            oauthHeader.AppendFormat("oauth_nonce={0}, ", nonce);
            oauthHeader.AppendFormat("oauth_timestamp={0}, ", timeStamp);
            oauthHeader.AppendFormat("oauth_signature_method={0}, ", "HMAC-SHA1");
            oauthHeader.AppendFormat("oauth_version={0}, ", "1.0");
            oauthHeader.AppendFormat("oauth_signature={0}, ", sig);
            oauthHeader.AppendFormat("oauth_token={0}", accessToken);

            //构造请求 
            StringBuilder requestBody = new StringBuilder("");
            Encoding encoding = Encoding.GetEncoding("utf-8");
            byte[] data = encoding.GetBytes(requestBody.ToString());

            // Http Request的设置 
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Headers.Set("Authorization", oauthHeader.ToString());
            //request.Headers.Add("Authorization", authorization); 
            request.ContentType = "application/atom+xml";
            request.Method = "GET";

            return request;
        }


        /// <summary>
        /// Post数据接口（json格式）
        /// </summary>
        /// <param name="postUrl">接口地址</param>
        /// <param name="paramData">提交json数据</param>
        /// <param name="dataEncode">编码方式(Encoding.UTF8)</param>
        /// <returns></returns>
        public static string PostWebRequest(string postUrl, string paramData, Encoding dataEncode)
        {
            string responseContent = string.Empty;
            try
            {
                byte[] byteArray = dataEncode.GetBytes(paramData); //转化
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(postUrl));
                webReq.Method = "POST";
                webReq.ContentType = "application/x-www-form-urlencoded";
                webReq.ContentLength = byteArray.Length;
                using (Stream reqStream = webReq.GetRequestStream())
                {
                    reqStream.Write(byteArray, 0, byteArray.Length);//写入参数
                    //reqStream.Close();
                }
                using (HttpWebResponse response = (HttpWebResponse)webReq.GetResponse())
                {
                    //在这里对接收到的页面内容进行处理
                    using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default))
                    {
                        responseContent = sr.ReadToEnd().ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return responseContent;
        }


        /// <summary>
        /// Get数据接口
        /// </summary>
        /// <param name="getUrl">接口地址</param>
        /// <returns></returns>
        public static string GetWebRequest(string getUrl)
        {
            string responseContent = "";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(getUrl);
            request.ContentType = "application/json";
            request.Method = "GET";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            //在这里对接收到的页面内容进行处理
            using (Stream resStream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(resStream, Encoding.UTF8))
                {
                    responseContent = reader.ReadToEnd().ToString();
                }
            }
            return responseContent;
        }


        /// <summary>
        /// HttpWebReques Post请求
        /// </summary>
        /// <param name="url">Url地址</param>
        /// <param name="body">body是要传递的参数,格式"roleId=1&uid=2"</param>
        /// <param name="contentType">post的cotentType填写:"application/x-www-form-urlencoded";soap填写:"text/xml; charset=utf-8"</param>
        /// <returns></returns>
        public static string PostHttp(string url, string body, string contentType)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);

            httpWebRequest.ContentType = contentType;
            httpWebRequest.Method = "POST";
            httpWebRequest.Timeout = 20000;

            byte[] btBodys = Encoding.UTF8.GetBytes(body);
            httpWebRequest.ContentLength = btBodys.Length;
            httpWebRequest.GetRequestStream().Write(btBodys, 0, btBodys.Length);

            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
            string responseContent = streamReader.ReadToEnd();

            httpWebResponse.Close();
            streamReader.Close();
            httpWebRequest.Abort();
            httpWebResponse.Close();

            return responseContent;
        }

        /// <summary>
        /// Get方式
        /// </summary>
        /// <param name="url"></param>
        /// <param name="httpContext">HTTPContext格式参数</param>
        /// <returns></returns>
        public static string GetHttp(string url, HttpContext httpContext)
        {
            string queryString = "?";

            foreach (string key in httpContext.Request.QueryString.AllKeys)
            {
                queryString += key + "=" + httpContext.Request.QueryString[key] + "&";
            }

            queryString = queryString.Substring(0, queryString.Length - 1);

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url + queryString);

            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "GET";
            httpWebRequest.Timeout = 20000;

            //byte[] btBodys = Encoding.UTF8.GetBytes(body);
            //httpWebRequest.ContentLength = btBodys.Length;
            //httpWebRequest.GetRequestStream().Write(btBodys, 0, btBodys.Length);

            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
            string responseContent = streamReader.ReadToEnd();

            httpWebResponse.Close();
            streamReader.Close();

            return responseContent;
        }


        /// <summary>
        /// 通过 WebRequest/WebResponse 类访问远程地址并返回结果，需要Basic认证；
        /// 调用端自己处理异常
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="timeout">访问超时时间，单位毫秒；如果不设置超时时间，传入0</param>
        /// <param name="encoding">如果不知道具体的编码，传入null</param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string Request_WebRequest(string uri, int timeout, Encoding encoding, string username, string password)
        {
            string result = string.Empty;

            WebRequest request = WebRequest.Create(new Uri(uri));

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                request.Credentials = GetCredentialCache(uri, username, password);
                request.Headers.Add("Authorization", GetAuthorization(username, password));
            }

            if (timeout > 0)
                request.Timeout = timeout;

            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader sr = encoding == null ? new StreamReader(stream) : new StreamReader(stream, encoding);

            result = sr.ReadToEnd();

            sr.Close();
            stream.Close();

            return result;
        }

        #endregion


        #region WebClient


        /// <summary>
        /// 通过WebClient类Post数据到远程地址，需要Basic认证；
        /// 调用端自己处理异常
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="paramStr">name=张三&age=20</param>
        /// <param name="encoding">请先确认目标网页的编码方式</param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string Request_WebClient(string uri, string paramStr, Encoding encoding, string username, string password)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;

            string result = string.Empty;

            WebClient wc = new WebClient();

            // 采取POST方式必须加的Header
            wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

            byte[] postData = encoding.GetBytes(paramStr);

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                wc.Credentials = GetCredentialCache(uri, username, password);
                wc.Headers.Add("Authorization", GetAuthorization(username, password));
            }

            byte[] responseData = wc.UploadData(uri, "POST", postData); // 得到返回字符流
            return encoding.GetString(responseData);// 解码                  
        }

        #endregion


        #region # 生成 Http Basic 访问凭证 #

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private static CredentialCache GetCredentialCache(string uri, string username, string password)
        {
            string authorization = string.Format("{0}:{1}", username, password);

            CredentialCache credCache = new CredentialCache();
            credCache.Add(new Uri(uri), "Basic", new NetworkCredential(username, password));

            return credCache;
        }

        private static string GetAuthorization(string username, string password)
        {
            string authorization = string.Format("{0}:{1}", username, password);

            return "Basic " + Convert.ToBase64String(new ASCIIEncoding().GetBytes(authorization));
        }

        #endregion


    }
}
