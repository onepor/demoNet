using GameWebAPI.Models;
using GameWebAPI.Utils;
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
    /// 支付接口
    /// </summary>
    [AuthAttributes.ApiAuthorize]
    //[RoutePrefix("api/Pay")]
    public class PayController : ApiController
    {

        #region Z渠道
        private string MerId = "90222";//商户唯一标识
        private string ZPayUrl = "http://zfb.ibosser.com:9860";//网关地址
        private string NotifyUrl = "http://rocking.ink/PayCallback/ZPayResult";//支付回调地址（需要自行配置）
        /// <summary>
        /// Z渠道支付
        /// </summary>
        /// <returns></returns>
        ///// <param name="PayType">支付类型：alipay:支付宝；wxpay:微信支付；</param>
        ///// <param name="UserId">用户</param>
        ///// <param name="Amount">金额（分）</param>
        [HttpPost]
        [Route("Pay/ZPayRequest")]
        public string ZPayRequest([FromBody]dynamic objects)
        {
            string retData = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(objects.PayType.ToString()) || string.IsNullOrEmpty(objects.UserId.ToString()) || string.IsNullOrEmpty(objects.Amount.ToString()))
                {
                    return "参数存在为空，（PayType、UserId、Amount）";
                }
                ////必填。验签信息，加密串Amount | MerId | MerOrderNo
                ////| MerOrderTime | SignKey
                ////的值用“|”连起来做MD5运算
                //dic.Add("Sign", ToolsHelper.GetStrSign(Amount+"|"+ MerId+"|"+ UserId + MerOrder + "|"+ MerOrder+"|"));

                string MerOrder = DateTime.Now.ToString("yyyyMMddhhmmss");//订单生成时间
                StringBuilder postData = new StringBuilder();
                postData.Append("MerId=" + MerId);
                postData.Append("&PayType=" + objects.PayType.ToString());
                postData.Append("&MerOrderNo=" + objects.UserId.ToString() + MerOrder);
                postData.Append("&MerOrderTime=" + MerOrder);
                postData.Append("&UserId=" + objects.UserId.ToString());
                postData.Append("&Amount=" + objects.Amount.ToString());
                postData.Append("&NotifyUrl=" + NotifyUrl);//支付回调地址
                //postData.Append("&SuccessUrl=" + "http://rocking.ink/swagger");//成功回跳地址
                postData.Append("&Version=2.1");
                postData.Append("&Sign=" + ToolsHelper.GetStrSign(objects.Amount.ToString() + "|" + MerId + "|" + objects.UserId.ToString() + MerOrder + "|" + MerOrder + "|"));

                retData = ToolsHelper.HttpPost(ZPayUrl + "/order/pay/", postData.ToString());
            }
            catch (Exception ex)
            {
                retData = ex.Message;
            }

            return retData;

            //            返回示例：
            //{ "code":"success","message":"下单成功","data":{ "PayOrderNo":"2019011517452600000005","PayOrderTime":"20190115174526","MerId":"8001","MerOrderNo":"20190115174425015","MerOrderTime":"20190115174425","PayType":"alipay","PayStatus":"accept","Amount":100, "AmountReal":100,"Fee":2,"Currency":"RMB","PayUrl":"http://39.98.177.232:42857/YST/Pay/2019011517452600000005","QrcodeUrl":"https://qr.95516.com/00010002/01043133953019401045844206133530"} }

        }

        /// <summary>
        /// 订单查询
        /// </summary>
        /// <param name="objects"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Pay/ZQueryOrder")]
        public string ZQueryOrder([FromBody]dynamic objects)
        {
            string retData = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(objects.OrderNo.ToString()) || string.IsNullOrEmpty(objects.Amount.ToString()))
                {
                    return "参数存在为空，（OrderNo、Amount）";
                }
                ////必填。验签信息，加密串Amount | MerId | MerOrderNo
                ////| MerOrderTime | SignKey
                ////的值用“|”连起来做MD5运算
                //dic.Add("Sign", ToolsHelper.GetStrSign(Amount+"|"+ MerId+"|"+ UserId + MerOrder + "|"+ MerOrder+"|"));

                StringBuilder postData = new StringBuilder();
                postData.Append("MerId=" + MerId);
                postData.Append("&PayOrderNo=" + objects.OrderNo.ToString());
                postData.Append("&Amount=" + objects.Amount.ToString());
                postData.Append("&Version=2.1");
                postData.Append("&Sign=" + ToolsHelper.GetStrSign(objects.Amount.ToString() + "|" + MerId + "|" + objects.OrderNo.ToString() + "|"));

                retData = ToolsHelper.HttpPost(ZPayUrl + "/order/payquery/", postData.ToString());
            }
            catch (Exception ex)
            {
                retData = ex.Message;
            }

            return retData;

            //            返回示例：
            //{"code":"success","message":"success","data":"20190115174533"}

        }

        ///// <summary>
        ///// 提现接口（银行卡）
        ///// </summary>
        ///// <param name="BankName">银行名称</param>
        ///// <param name="AccountNo">卡号</param>
        ///// <param name="PersonName">姓名</param>
        ///// <param name="Amount">金额</param>
        ///// <returns></returns>
        //        [HttpPost]
        //        [Route("ZWithdrawCar")]
        //        public string ZWithdrawCar(string BankName, string AccountNo, string PersonName, string Amount)
        //        {
        //            string retData = string.Empty;

        //            try
        //            {

        //                ////必填。验签信息，加密串Amount | MerId | MerOrderNo
        //                ////| MerOrderTime | SignKey
        //                ////的值用“|”连起来做MD5运算
        //                //dic.Add("Sign", ToolsHelper.GetStrSign(Amount+"|"+ MerId+"|"+ UserId + MerOrder + "|"+ MerOrder+"|"));

        //                string MerOrder = DateTime.Now.ToString("yyyyMMddhhmmss");//订单生成时间
        //                StringBuilder postData = new StringBuilder();
        //                postData.Append("MerId=" + MerId);
        //                postData.Append("&MerOrderNo=" + Amount + MerOrder);
        //                postData.Append("&MerOrderTime=" + MerOrder);
        //                //postData.Append("BankId=" + BankId);
        //                postData.Append("BankName=" + BankName);
        //                postData.Append("AccountNo=" + AccountNo);
        //                postData.Append("PersonName=" + PersonName);
        //                postData.Append("&Amount=" + Amount);
        //                postData.Append("&Version=2.1");
        //                postData.Append("&Sign=" + ToolsHelper.GetStrSign(Amount + "|" + MerId + "|" + Amount + MerOrder + "|" + MerOrder + "|"));

        //                retData = ToolsHelper.HttpPost(ZPayUrl + "/order/withdraw/", postData.ToString());
        ////                返回示例：
        ////{ "code":"success","message":"提现已受理","data":{ "WithdrawNo":"2019011517452600000005","WithdrawTime":"20190115174526","MerId":"8001","MerOrderNo":"20190115174425015","MerOrderTime":"20190115174425","PayStatus":"lock","Amount":100, "Fee":2 } }

        //            }
        //            catch (Exception ex)
        //            {
        //                retData = ex.Message;
        //            }

        //            return retData;

        //            //            返回示例：
        //            //{ "code":"success","message":"下单成功","data":{ "PayOrderNo":"2019011517452600000005","PayOrderTime":"20190115174526","MerId":"8001","MerOrderNo":"20190115174425015","MerOrderTime":"20190115174425","PayType":"alipay","PayStatus":"accept","Amount":100, "AmountReal":100,"Fee":2,"Currency":"RMB","PayUrl":"http://39.98.177.232:42857/YST/Pay/2019011517452600000005","QrcodeUrl":"https://qr.95516.com/00010002/01043133953019401045844206133530"} }

        //        }


        #endregion


        #region 善付
        private string GoodUrl = "https://sfpay8.com/api/gateway/index.html";
        private string notify_url = "http://rocking.ink/Pay/GoodPayResult";//支付回调地址
        private string SingKey = "aa2ef4a3ad2fcf1cf64950a0168b1ff7";//签名加密key值

        /// <summary>
        /// 善付支付接口（）
        /// </summary>
        /// <returns></returns>
        ///// <param name="Money">支付金额</param>
        ///// <param name="PayType">支付类型（微信扫码	2;支付宝扫码	3;网银快捷	6）</param>
        ///// <param name="PayTypeCode">支付代码值（微信扫码	WECHAT;支付宝扫码	ALIPAY;网银快捷	QUICK）</param>
        [HttpPost]
        [Route("Pay/GoodPayRequest")]
        public string GoodPayRequest([FromBody]dynamic data)//float Money,int PayType,string PayTypeCode
        {
            string retData = string.Empty;
            try
            {

                if (string.IsNullOrEmpty(data.PayType.ToString()) || string.IsNullOrEmpty(data.PayTypeCode.ToString()) || string.IsNullOrEmpty(data.Money.ToString()))
                {
                    return "参数存在为空，（PayType、PayTypeCode、Money）";
                }
                string MerOrder = DateTime.Now.ToString("yyyyMMddhhmmss");//订单生成时间
                StringBuilder postData = new StringBuilder();
                postData.Append("{\"partner_id\":" + 20162);//商户ID
                postData.Append(",\"pay_type\":" + data.PayType);//支付类型
                postData.Append(",\"pay_code\":\"" + data.PayTypeCode.ToString() + "\"");//支付代码
                postData.Append(",\"out_trade_no\":\"" + data.PayType.ToString() + DateTime.Now.ToString("yyyyMMddhhmmssffff") + "\"");//商户订单号，32位以内
                postData.Append(",\"notify_url\":\"" + notify_url + "\"");//异步通知地址，120位以内
                postData.Append(",\"return_url\":\"https://www.baidu.com/?tn=48021271_15_hao_pg\"");//同步通知地址，120位以内
                postData.Append(",\"money\":" + Convert.ToDecimal(data.Money));//支付提交金额，单位：元，两位小数

                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("partner_id", 20162.ToString());
                dic.Add("pay_type", data.PayType.ToString());
                dic.Add("pay_code", data.PayTypeCode.ToString());
                dic.Add("out_trade_no", data.PayType.ToString() + DateTime.Now.ToString("yyyyMMddhhmmssffff"));
                dic.Add("notify_url", notify_url);//
                dic.Add("return_url", "https://www.baidu.com/?tn=48021271_15_hao_pg");
                dic.Add("money", Convert.ToDecimal(data.Money).ToString("0.##"));
                string Sign = ToolsHelper.GetSign(dic, "aa2ef4a3ad2fcf1cf64950a0168b1ff7");

                postData.Append(",\"sign\":\"" + Sign + "\"");
                postData.Append(",\"sign_type\":\"md5\"}");
                retData = ToolsHelper.HTMLPost(GoodUrl, postData).Replace(@"\", "");
                //返回成功的数据（列子）
                //               {
                //                   "code": 200,
                //"message": "success",
                //"data": {
                //                       "partner_id": 20086,
                //	"pay_type": 3,
                //	"pay_code": "ALIPAY",
                //	"trade_no": "SF191114123319880616",
                //	"out_trade_no": "TEST1911141231599758",
                //	"money": 300,
                //	"sign_type": "md5",
                //	"pay_url": "http://www.baidu.com/xxxx",
                //	"sign": "19f48ff94cf6f9aeb6eeb91628e62d8e"
                //   }
                //               }
            }
            catch (Exception ex)
            {
                retData = ex.Message;
            }
            return retData;
        }

        /// <summary>
        /// 订单查询查询
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Pay/GoodQueryOrder")]
        public string QueryOrder([FromBody]dynamic data)
        {

            string retData = string.Empty;
            try
            {

                if (string.IsNullOrEmpty(data.OrderNo.ToString()))
                {
                    return "参数存在为空，（OrderNo）";
                }

                StringBuilder postData = new StringBuilder();

                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("partner_id", 20162.ToString());
                dic.Add("out_trade_no", data.OrderNo.ToString());
                string Sign = ToolsHelper.GetSign(dic, "aa2ef4a3ad2fcf1cf64950a0168b1ff7");

                //postData.Append("{\"partner_id\":" + 20162);
                //postData.Append(",\"out_trade_no\":\"" + data.OrderNo.ToString() + "\"");
                //postData.Append(",\"sign\":\"" + Sign + "\"");
                //postData.Append(",\"sign_type\":\"md5\"");

                postData.Append("partner_id=" + 20162);
                postData.Append("&out_trade_no=" + data.OrderNo.ToString());
                postData.Append("&sign=" + Sign);
                postData.Append("&sign_type=md5");

                retData = ToolsHelper.HttpPost("https://sfpay8.com/api/trade/query.html", postData.ToString());
                //返回数据
                //{ "code":200,"message":"查询订单成功","data":{ "partner_id":20086,"out_trade_no":"TEST1910292204367130","trade_no":"SF191102105045139860","money":300,"money_true":299.5,"fee":3.5,"state":1,"sign":"50da9ded1969c9659e317326e165c933","sign_type":"md5"} }
            }
            catch (Exception ex)
            {
                retData = ex.Message;
            }
            return retData;
        }


        #endregion

    }
}