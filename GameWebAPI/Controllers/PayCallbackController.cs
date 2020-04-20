using log4net;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;

namespace GameWebAPI.Controllers
{
    /// <summary>
    /// 支付回调
    /// </summary>
    [RoutePrefix("PayCallback")]
    public class PayCallbackController : ApiController
    {

        /// <summary>
        /// 支付回调地址（Z渠道）
        /// </summary>
        /// <param name="objData">回调传入对象</param>
        /// <returns></returns>
        [HttpPost]
        [Route("ZPayResult")]
        public HttpResponseMessage ZPayResult([FromBody]JObject objData)
        {

            log4net.Config.XmlConfigurator.Configure();
            ILog log = LogManager.GetLogger("test");
            log.Info("Z支付回调地址：" + objData.ToString());
            log.Info("Z支付回调地址："+ Newtonsoft.Json.JsonConvert.SerializeObject(objData));
            log.Info("Z支付回调地址：" + objData["PayStatus"].ToObject<string>());

            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                if (string.IsNullOrEmpty(objData["PayStatus"].ToObject<string>()))
                {
                    response = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent("回调数据为空", Encoding.GetEncoding("UTF-8"), "application/json")
                    };
                    return response;
                }
                else
                {
                    if (objData["PayStatus"].ToObject<string>() == "success")
                    {
                        response = new HttpResponseMessage(HttpStatusCode.OK)
                        {
                            Content = new StringContent("success", Encoding.GetEncoding("UTF-8"), "application/json")
                        };
                        return response;
                    }
                    else
                    {

                        response = new HttpResponseMessage(HttpStatusCode.OK)
                        {
                            Content = new StringContent("error", Encoding.GetEncoding("UTF-8"), "application/json")
                        };
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(ex.Message, Encoding.GetEncoding("UTF-8"), "application/json")
                };
                return response;
            }
        }

        /// <summary>
        /// 善付支付回调接口
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GoodPayResult")]
        public HttpResponseMessage GoodPayResult([FromBody]JObject data)
        {

            log4net.Config.XmlConfigurator.Configure();
            ILog log = LogManager.GetLogger("test");
            log.Info("善付支付回调地址：" + data.ToString());
            log.Info("善付支付回调地址：" + Newtonsoft.Json.JsonConvert.SerializeObject(data));
            log.Info("善付支付回调地址：" + data["PayStatus"].ToObject<string>());

            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                if (string.IsNullOrEmpty(data.ToString()))
                {
                    response = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent("回调数据为空", Encoding.GetEncoding("UTF-8"), "application/json")
                    };
                    return response;
                }
                else
                {
                    //注：已成功支付的订单才会通知，为避免重复通知导致重复上单，请商户务必做好一次性验证
                    if (!string.IsNullOrEmpty(data["out_trade_no"].ToString()))
                    {
                        //string content = data["content"].ToString();
                        //string signdata = data["sign"].ToString();

                        ////对返回的数据进行解码
                        //content = HttpUtility.UrlDecode(content);

                        response = new HttpResponseMessage(HttpStatusCode.OK)
                        {
                            Content = new StringContent("success", Encoding.GetEncoding("UTF-8"), "application/json")
                        };
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                //Log4Helper.WriteErrLog("HttpPost - api/MengMaPay/MengMaPaymentResult - err ", ex.Message);
            }
            response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("error", Encoding.GetEncoding("UTF-8"), "application/json")
            };
            return response;
        }

    }
}