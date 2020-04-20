using GameWebAPI.Models;
using GameWebAPI.Utils;
using HtmlAgilityPack;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Caching;
using System.Web.Http;
using System.Xml;

namespace GameWebAPI.Controllers
{
    /// <summary>
    /// 游戏接口
    /// </summary>
    //[AuthAttributes.ApiAuthorize]
    public class GameController : ApiController
    {

        #region Habanero游戏

        private static string HTTPAdress = ConfigurationManager.AppSettings["HBDefaultHTTPAdress"];
        private Guid BrandId = new Guid(ConfigurationManager.AppSettings["BrandId"]);
        private string APIKey = ConfigurationManager.AppSettings["APIKey"];


        /// <summary>
        /// 加载游戏列表
        /// </summary>
        /// <returns>
        /// </returns>
        [HttpPost]
        [Route("Game/HB_GetGames")]
        public string HB_GetGames()
        {
            dynamic data = new System.Dynamic.ExpandoObject();

            data.BrandId = BrandId;
            data.Apikey = APIKey;

            return ToolsHelper.JsonHttpClient(HTTPAdress + "getgames", Newtonsoft.Json.JsonConvert.SerializeObject(data));

        }

        /// <summary>
        /// 启动游戏
        /// </summary>
        /// <param name="gameType">游戏ID值</param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Game/HB_PlayerGame")]
        public string HB_PlayerGame(string gameType, string token)
        {
            //mode：fun or real 试玩/真钱
            string url = string.Format("https://app-test.insvr.com/play?brandid={0}&keyname={1}&token={2}&mode={3}&locale={4}&lobbyurl={5}"
               , BrandId, gameType, token, "real", "zh-CN", "");
            return url;
        }

        /// <summary>
        /// 登录或注册
        /// </summary>
        /// <param name="Name">登录账号</param>
        /// <param name="PassWord">登录密码</param>
        /// <param name="UserHostAddress">IP地址</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Game/HB_LoginPlayer")]
        public string HB_LoginPlayer(string Name, string PassWord, string UserHostAddress)
        {
            string RetData = string.Empty;

            try
            {
                dynamic data = new System.Dynamic.ExpandoObject();
                data.BrandId = BrandId;//玩家类型ID值
                data.Apikey = APIKey;//
                data.PlayerHostAddress = UserHostAddress;//玩家IP地址
                data.UserAgent = Name;        //玩家浏览器代理字符串,string UserAgent
                data.KeepExistingToken = true;//默认为True: 如果玩家已经登录，不要创建一个新的会话token。如果要使其他会话无效，请设置为False
                data.Username = Name;//账号
                data.Password = PassWord;//密码
                data.CurrencyCode = "CNY";//货币代码
                data.LanguageCode = "zh-CN";

                //可选参数
                //    //CountryCode = "VI",
                //    //FirstName = "john",
                //    //LastName = "mustard",
                //    //Locale = "vi",
                //    //Address1
                //    //Address2
                //    //EmailAddress
                //    //Gender
                //    //IdentityNumber //护照，IC等
                //    //PostalCode
                //    //TelNumber
                //    //City

                //RetData = PostFunction("LoginOrCreatePlayer",Newtonsoft.Json.JsonConvert.SerializeObject(data));
                RetData = ToolsHelper.JsonHttpClient(HTTPAdress + "LoginOrCreatePlayer", Newtonsoft.Json.JsonConvert.SerializeObject(data));

                //返回字段（）
                //  Authenticated      指示如果用户在请求之后进行登入或是验证。如果是False表示密码不正确
                //  PlayerId       在HB电游数据库内部的<GUID>主键（可使用）
                //  BrandId        (玩家的brandid) 
                //  BrandName      (玩家的brandid)
                //  Token      对于玩家的会话Token。使用这个来加载游戏
                //  RealBalance    玩家们真实货币余额
                //  CurrencyCode   玩家货币代码
                //  CurrencySymbol     玩家货币符号
                //  PlayerCreated      如果IsNewPlayer是True，否则False（玩家已经存在）
                //  HasBonus       表示玩家有一个活跃的奖金
                //  BonusBalance       奖励余额为活跃中奖金的呈现
                //  BonusSpins     如果奖金是免费转的类型，那么这是剩下多少免费转 
                //  BonusGameKeyName       在游戏中拥有奖励的游戏Keyname
                //  BonusPercentage        奖金进行投注完成的百分比 
                //  BonusWagerRemaining    距离完成奖金投注要求还差多少
                //  Message        参考消息
                //  PointBalance       显示玩家点数的小数

            }
            catch (Exception ex)
            {
                RetData = ex.Message;
            }

            return RetData;
        }

        /// <summary>
        /// 查询玩家余额（查询一个玩家返回玩家的当前余额、token和奖金信息）
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="PassWord"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Game/HB_QueryPlayer")]
        public string HB_QueryPlayer(string Name, string PassWord)
        {

            string RetData = string.Empty;
            try
            {
                dynamic data = new System.Dynamic.ExpandoObject();

                data.BrandId = BrandId;//玩家类型ID值
                data.Apikey = APIKey;//
                data.Username = Name;//账号
                data.Password = PassWord;//密码

                RetData = ToolsHelper.JsonHttpClient(HTTPAdress + "QueryPlayer", Newtonsoft.Json.JsonConvert.SerializeObject(data));

                // 返回字段（）
                // Found   Boolean值，表示如果找到记录（玩家存在）。如果token是空的，玩家没有登录，
                // PlayerId    在HB数据库内部的<guid>主键（可使用）在HB数据库内部，
                // BrandId     玩家的brandid
                // BrandName   品牌名称
                // Token       对于玩家的会话Token。用于加载 游戏。如果空的，则玩家没有登录 
                // RealBalance     玩家真实货币余额 
                // CurrencyCode    玩家的货币代码 
                // CurrencySymbol      玩家的货币符号
                // HasBonus        表示玩家有一个活跃的奖金
                // BonusBalance    奖金余额
                // BonusSpins      如果奖金是免费转的类型，那么这是表示剩余的免费转次数 
                // BonusGameKeyName    游戏keyname表示里面有奖励游戏
                // BonusPercentage     表示距离奖金完成投注要求的百分比 
                // BonusWagerRemaining     需要多少就这个奖金，直到完成要下注 
                // Message     如果信息显示Found = false 

            }
            catch (Exception ex)
            {
                RetData = ex.Message;
            }
            return RetData;
        }

        /// <summary>
        /// 玩家存款
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="PassWord"></param>
        /// <param name="Amount"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Game/HB_DepositPlayerMoney")]
        public string HB_DepositPlayerMoney(string Name, string PassWord, decimal Amount)
        {
            string RetData = string.Empty;
            try
            {
                dynamic data = new System.Dynamic.ExpandoObject();
                data.BrandId = BrandId;//玩家类型ID值
                data.Apikey = APIKey;//
                data.Username = Name;//账号
                data.Password = PassWord;//密码
                data.Amount = Amount;//金额
                data.CurrencyCode = "CNY";
                data.RequestId = "DepositPlayerMoney" + Name;

                RetData = ToolsHelper.JsonHttpClient(HTTPAdress + "DepositPlayerMoney", Newtonsoft.Json.JsonConvert.SerializeObject(data));

                //返回字段（）
                // Success:Indicates if Deposit Successful 指示是否成功存款
                //Amount: Amount deposited  已存款金额
                //RealBalance:Player’s balance after Deposit 存款后，玩家的余额
                //TransactionId:   The unique Habanero<guid> representing the deposit  独特的HB<guid> 代表存款
                //CurrencyCode: The player’s Currency Code  玩家的货币代码
                //Message: Informational message  参考消息

            }
            catch (Exception ex)
            {
                RetData = ex.Message;
            }
            return RetData;
        }

        /// <summary>
        /// 玩家取现
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="PassWord"></param>
        /// <param name="Amount">(值为负数)</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Game/HB_WithdrawPlayerMoney")]
        public string HB_WithdrawPlayerMoney(string Name, string PassWord, decimal Amount)
        {
            string RetData = string.Empty;
            try
            {
                dynamic data = new System.Dynamic.ExpandoObject();
                data.BrandId = BrandId;//玩家类型ID值
                data.Apikey = APIKey;//
                data.Username = Name;//账号
                data.Password = PassWord;//密码
                data.Amount = Amount;//金额
                data.CurrencyCode = "CNY";
                //data.WithdrawAll = true;
                data.RequestId = "WithdrawPlayerMoney" + Name;

                RetData = ToolsHelper.JsonHttpClient(HTTPAdress + "WithdrawPlayerMoney", Newtonsoft.Json.JsonConvert.SerializeObject(data));

                //返回字段（）
                // Success  指示是否取款成功
                // Amount  （ 可以使用全部取款WithdrawAll=true）取款的金额
                // RealBalance     玩家取款后的余额
                // TransactionId   独特的HB<guid>表示提款 
                // CurrencyCode    玩家的货币代码
                // Message     参考消息

            }
            catch (Exception ex)
            {
                RetData = ex.Message;
            }
            return RetData;
        }

        /// <summary>
        /// 查询投注记录
        /// </summary>
        /// <param name="starTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Game/HB_GetBrandCompletedGameResults")]
        public string HB_GetBrandCompletedGameResults(string starTime, string endTime)
        {
            string RetData = string.Empty;
            try
            {
                dynamic data = new System.Dynamic.ExpandoObject();
                data.BrandId = BrandId;//玩家类型ID值
                data.Apikey = APIKey;//
                data.DtStartUTC = starTime;//UTC开始日期范围 - yyyyMMddHHmmss。此字段是包容性（> =）
                data.DtEndUTC = endTime;//UTC结束日期范围 - yyyyMMddHHmmss。这个字段是独占（<）

                RetData = ToolsHelper.JsonHttpClient(HTTPAdress + "GetBrandCompletedGameResults", Newtonsoft.Json.JsonConvert.SerializeObject(data));

                // 返回列表字段：
                // PlayerId 玩家ID，BrandId 品牌ID，Username 用户名，BrandGameId 品牌游戏ID，GameKeyName 游戏名，
                // GameTypeId 游戏类型ID，DtStarted 开始日期，DtCompleted 结束日期，FriendlyGameInstanceId 游戏编号，
                // GameInstanceId 注单号，Stake 投注，Payout 派彩，JackpotWin 奖池奖金，JackpotContribution，
                // CurrencyCode 货币代码，ChannelTypeId 渠道类型ID，BalanceAfter 馀额

            }
            catch (Exception ex)
            {
                RetData = ex.Message;
            }

            return RetData;

        }

        #endregion


        #region PNG 游戏(Play’n Go 游戏)

        /// <summary>
        /// 登入或创建玩家
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("Game/PNG_LoginOrCreatePlayer")]
        public string PNG_LoginOrCreatePlayer(string userName)
        {
            try
            {
                string strxml = @"<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/'
                                    xmlns:v1='http://playngo.com/v1'>
                                    <soapenv:Header/>
                                    <soapenv:Body>
                                    <v1:RegisterUser>
                                    <v1:UserInfo>
                                    <v1:ExternalUserId>" + userName + @"</v1:ExternalUserId>
                                    <v1:Username>" + userName + @"</v1:Username>
                                    <v1:Nickname>" + userName + @"</v1:Nickname>
                                    <v1:Currency>CNY</v1:Currency>
                                    <v1:Country>CN</v1:Country>
                                    <v1:Birthdate>1990-07-07</v1:Birthdate>
                                    <v1:Registration>" + DateTime.Now.ToString("yyyy-MM-dd") + @"</v1:Registration>
                                    <v1:BrandId>" + userName + @"</v1:BrandId>
                                    <v1:Language>zh_CN</v1:Language>
                                    <v1:IP>127.0.1</v1:IP>
                                    <v1:Locked>false</v1:Locked>
                                    <v1:Gender>m</v1:Gender>
                                    </v1:UserInfo>
                                    </v1:RegisterUser>
                                    </soapenv:Body>
                                    </soapenv:Envelope>";
                int PostStatusCode;
                string data = ToolsHelper.PostHttpConnectXML(strxml, "CasinoGameService/RegisterUser", out PostStatusCode).ToString();//Encoding.UTF8.GetBytes(strxml)
                return ToolsHelper.XMLToJson(data)+ "【StatusCode："+ PostStatusCode + "】";
            }
            catch (Exception ex)
            {
                return ex.Message;//"false";
            }
        }

        /// <summary>
        /// 启动游戏
        /// </summary>
        /// <param name="gid">Excel中游戏对应的GID</param>
        /// <param name="ticket"></param>
        /// <param name="showType">1：PC 2：APP</param>
        /// <param name="practice">1：免费玩，0：真钱游戏</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Game/PNG_PlayerGame")]
        public string PNG_PlayerGame(string gid, string ticket, int showType, string practice)
        {
            //如果practice=1（这个是前提），如果是手机端，则不传递参数ticket，如果是web端，则不传递参数username
            string url = "";
            if (showType == 1)//PC
            {
                int star = Request.RequestUri.ToString().IndexOf("/G");
                url = Request.RequestUri.ToString().Substring(0, star + 1) + "PNGGame.html";
                url = string.Format(url + "?pid=495&gid={0}&username={1}&lang=zh_CN&practice={2}&height=100%&width=100%", gid, ticket, practice);
                //if (practice == "1")
                //{
                //    url = string.Format("https://csistage.playngonetwork.com/casino/js?pid=495&gid={0}&lang=zh_CN&practice=1&height=100%&width=100%", gid);
                //}
                //else
                //{
                //    url = string.Format("https://csistage.playngonetwork.com/casino/js?pid=495&gid={0}&lang=zh_CN&practice=0&username={1}&height=100%&width=100%", gid, ticket);
                //}
            }
            else//APP
            {
                if (practice == "1")
                {
                    url = string.Format("https://csistage.playngonetwork.com/casino/PlayMobile?pid=495&gid={0}&lang=zh_CN&practice=1&height=100%&width=100%"
                                      , gid);
                }
                else
                {
                    url = string.Format("https://csistage.playngonetwork.com/casino/PlayMobile?pid=495&gid={0}&lang=zh_CN&practice=0&ticket={1}&height=100%&width=100%"
                                      , gid, ticket);
                }
            }
            return url;
        }

        /// <summary>
        /// 获取游戏Ticket
        /// </summary>
        /// <param name="userName">游戏名称</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Game/PNG_GetTicket")]
        public string PNG_GetTicket(string userName)
        {
            string strxml = @"<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:v1='http://playngo.com/v1'>
                              <soapenv:Header/>
                             <soapenv:Body>
                                <v1:GetTicket>
                                   <v1:ExternalUserId>" + userName + @"</v1:ExternalUserId>
                              </v1:GetTicket>
                            </soapenv:Body>
                          </soapenv:Envelope>";

            int PostStatusCode;
            string data = ToolsHelper.PostHttpConnectXML(strxml, "CasinoGameService/GetTicket", out PostStatusCode).ToString();//Encoding.UTF8.GetBytes(strxml)
            return ToolsHelper.XMLToJson(data);
        }

        /// <summary>
        /// 获取玩家余额
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("Game/PNG_GetUserBalance")]
        public string PNG_GetUserBalance(string userName)
        {
            try
            {
                string strxml = @"<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:v1='http://playngo.com/v1'>
                              <soapenv:Header/>
                             <soapenv:Body>
                                <v1:Balance>
                                   <v1:ExternalUserId>" + userName + @"</v1:ExternalUserId>
                              </v1:Balance>
                            </soapenv:Body>
                          </soapenv:Envelope>";

                int PostStatusCode;
                string data = ToolsHelper.PostHttpConnectXML(strxml, "CasinoGameService/Balance", out PostStatusCode).ToString();//Encoding.UTF8.GetBytes(strxml)
                return ToolsHelper.XMLToJson(data);
            }
            catch (Exception exp)
            {
                return exp.Message;
            }
        }

        /// <summary>
        /// 玩家提款
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("Game/PNG_GetUserTransferWithdraw")]
        public string PNG_GetUserTransferWithdraw(string userName, decimal money)
        {
            try
            {
                string strxml = @"<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:v1='http://playngo.com/v1'>
                              <soapenv:Header/>
                             <soapenv:Body>
                                <v1:Debit>
                                   <v1:ExternalUserId>" + userName + @"</v1:ExternalUserId>
                                  <v1:Amount>" + Convert.ToDecimal(money) + @"</v1:Amount>
                                  <v1:Currency>CNY</v1:Currency>
                              </v1:Debit>
                            </soapenv:Body>
                          </soapenv:Envelope>";

                int PostStatusCode;
                string data = ToolsHelper.PostHttpConnectXML(strxml, "CasinoGameService/Debit", out PostStatusCode).ToString();//Encoding.UTF8.GetBytes(strxml)
                return ToolsHelper.XMLToJson(data);

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// 玩家存款
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("Game/PNG_GetUserTransferDeposit")]
        public string PNG_GetUserTransferDeposit(string userName, decimal money)
        {
            try
            {
                string strxml = @"<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:v1='http://playngo.com/v1'>
                              <soapenv:Header/>
                             <soapenv:Body>
                                <v1:Credit>
                                   <v1:ExternalUserId>" + userName + @"</v1:ExternalUserId>
                                  <v1:Amount>" + Convert.ToDecimal(money) + @"</v1:Amount>
                                  <v1:Currency>CNY</v1:Currency>
                              </v1:Credit>
                            </soapenv:Body>
                          </soapenv:Envelope>";

                int PostStatusCode;
                string data = ToolsHelper.PostHttpConnectXML(strxml, "CasinoGameService/Credit", out PostStatusCode).ToString();//Encoding.UTF8.GetBytes(strxml)
                return ToolsHelper.XMLToJson(data);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// 获取未完成的游戏
        /// </summary>
        /// <param name="userName">用户</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Game/PNG_GetUnfinishedGames")]
        public string PNG_GetUnfinishedGames(string userName)
        {
            try
            {
                string strxml = @"<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/'
                                xmlns:v1='http://playngo.com/v1'>
                                <soapenv:Header/>
                                <soapenv:Body>
                                <v1:GetUnfinishedGames>
                                <v1:ExternalId>" + userName + @"</v1:ExternalId>
                                </v1:GetUnfinishedGames>
                                </soapenv:Body>
                                </soapenv:Envelope>";

                int PostStatusCode;
                string data = ToolsHelper.PostHttpConnectXML(strxml, "CasinoGameService/GetUnfinishedGames", out PostStatusCode).ToString();//Encoding.UTF8.GetBytes(strxml)
                return ToolsHelper.XMLToJson(data);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        /// <summary>
        /// 用来向免费游戏活动中一次添加一位玩家 （测试交易记录）
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Game/PNG_AddFreeGameOffers")]
        public string PNG_AddFreeGameOffers(string userName = "wp_123")
        {
            try
            {
                string strxml = @"<soapenv:Envelope
                                    xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/'
                                    xmlns:v1='http://playngo.com/v1'
                                    xmlns:arr='http://schemas.microsoft.com/2003/10/Serialization/Arrays'>
                                    <soapenv:Header/>
                                    <soapenv:Body>
                                    <v1:AddFreegameOffers>
                                    <v1:UserId>" + userName + @"</v1:UserId>
                                    <!--<v1:GameId></v1:GameId>:-->
                                    <v1:Lines>10</v1:Lines>
                                    <v1:Coins>1</v1:Coins>
                                    <v1:Denomination>0.01</v1:Denomination>
                                    <v1:Rounds>10</v1:Rounds>
                                    <v1:ExpireTime>2019-12-08T00:01:02</v1:ExpireTime>
                                    <v1:Turnover>0</v1:Turnover>
                                    <v1:FreegameExternalId>45678924</v1:FreegameExternalId>
                                    <v1:RequestId>BL2012</v1:RequestId>
                                    <!--<v1:TriggerId></v1:TriggerId>:-->
                                    <v1:AllGamesVariants>0</v1:AllGamesVariants>
                                    <v1:GameIdList>
                                    <arr:int>243</arr:int>
                                    <arr:int>288</arr:int>
                                    </v1:GameIdList>
                                    </v1:AddFreegameOffers>
                                    </soapenv:Body>
                                    </soapenv:Envelope>";
                int PostStatusCode;
                string data = ToolsHelper.PostHttpConnectXML(strxml, "CasinoGameService/AddFreegameOffers", out PostStatusCode).ToString();//
                return ToolsHelper.XMLToJson(data) + "【StatusCode：" + PostStatusCode + "】";
            }
            catch (Exception ex)
            {
                return ex.Message;//"false";
            }
        }

        /// <summary>
        /// PNG 游戏端回发数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Game/PNG_GetGamesBetRecord")]
        public HttpResponseMessage PNG_GetGamesBetRecord([FromBody]JObject data)
        {

            HttpResponseMessage response = new HttpResponseMessage();

            //测试日志
            ILog log = LogManager.GetLogger("PNGgame：");
            log.Info("PNG 游戏端回发数据：" + data.ToString());

            //string jsondata = "{\"Messages\": [{\"TransactionId\": 2056842,\"Status\": 1,\"Balance\": 985.0,\"MessageId\": \"3377#2\",\"MessageType\": 3,\"MessageTimestamp\": \"2019-09-04T05:19:13\"},{\"TransactionId\": 3110993,\"Status\": 1,\"Amount\": 3.52,\"Time\": \"2016-01-11T14:20:41.3736\",\"ProductGroup\": 2,\"ExternalUserId\": \"testuser\",\"GamesessionId\": 27056,\"GamesessionState\": 0,\"GameId\": 251,\"RoundId\": 1568391,\"RoundData\": null,\"RoundLoss\": 0.76,\"JackpotLoss\": 0.0,\"JackpotGain\": 0.0,\"Currency\": \"EUR\",\"ExternalTransactionId\": \"160111021945-p-10\",\"Balance\": 9999.72,\"NumRounds\": 5,\"TotalLoss\": 3.8,\"TotalGain\": 3.52,\"ExternalFreegameId\": null,\"MessageId\": \"3377#3\", \"MessageType\": 4, \"MessageTimestamp\": \"2019-09-04T05:19:13\" } ]}";
            //var jobj = JObject.Parse(jsondata);

            try
            {

                //赌场交易释放开启（返回的交易信息）
                List<PNGTransactionReleaseOpen> TransactionReleaseOpenlist = new List<PNGTransactionReleaseOpen>();
                //赌场交易保留（返回的交易信息）
                List<PNGTransactionReserve> TransactionReservelist = new List<PNGTransactionReserve>();
                for (int i = 0; i < data["Messages"].Count(); i++)
                {
                    //消息类型：（1：赌场玩家登录；2：赌场玩家登出；3：赌场交易保留；4：赌场交易释放开启；5：赌场交易释放关闭；6：赌场彩池释放；7：赌场免费游戏结束）
                    if (data["Messages"][i]["MessageType"].ToString() == "3")
                    {
                        TransactionReservelist.Add(Newtonsoft.Json.JsonConvert.DeserializeObject<PNGTransactionReserve>(data["Messages"][i].ToString()));
                    }
                    if (data["Messages"][i]["MessageType"].ToString() == "4")
                    {
                        TransactionReleaseOpenlist.Add(Newtonsoft.Json.JsonConvert.DeserializeObject<PNGTransactionReleaseOpen>(data["Messages"][i].ToString()));
                    }
                }

                log.Info("PNG 游戏端回发数据（TransactionReservelist）：" + Newtonsoft.Json.JsonConvert.SerializeObject(TransactionReservelist));
                log.Info("PNG 游戏端回发数据（TransactionReleaseOpenlist）：" + Newtonsoft.Json.JsonConvert.SerializeObject(TransactionReleaseOpenlist));

                response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("Success", Encoding.GetEncoding("UTF-8"), "application/x-www-form-urlencoded")
                };
            }
            catch (Exception ex)
            {
                response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("Error:"+ ex.Message, Encoding.GetEncoding("UTF-8"), "application/x-www-form-urlencoded")
                };
            }

            return response;
        }


        #region 未用

        ///// <summary>
        ///// 获取投注记录
        ///// </summary>
        ///// <param name="userId">玩家ID</param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("Game/PNG_GetGamesBetList")]
        //public DataTable PNG_GetGamesBetList(string userId = "wp_123")
        //{
        //    string url = "https://csistage.playngonetwork.com/casinohistory?userId=" + userId + "&pid=495&lang=zh_CN";
        //    //var sss = JsonAothHttpClient(url,"","","get");

        //    WebClient client = new WebClient();
        //    client.Encoding = System.Text.Encoding.UTF8;
        //    string html = client.DownloadString(url).Replace("\r\n", "").Replace("\t", "");

        //    return ParsingWeb(html, "/html/body/table");
        //}

        ////HtmlString 获取的html页面的字符串
        ////XmlPath 解析元素在html中的位置,像:XmlPath = "/html/body/div[3]/div[3]/div[1]/table"
        //public static DataTable ParsingWeb(string HtmlString, string XmlPath)
        //{
        //    try
        //    {
        //        //HtmlWeb web = new HtmlWeb();
        //        //HtmlDocument doc = web.Load(WebUrl);
        //        var doc = new HtmlDocument();
        //        doc.LoadHtml(HtmlString);
        //        DataTable htTable = new DataTable();
        //        var tablehtml = doc.DocumentNode.SelectSingleNode(XmlPath);

        //        if (tablehtml == null)
        //        {
        //            return null;
        //        }
        //        var TrSelected = tablehtml.SelectNodes(".//tr");
        //        foreach (HtmlNode row in TrSelected)
        //        {
        //            var Index = TrSelected.IndexOf(row);
        //            if (TrSelected.IndexOf(row) == 0)
        //            {
        //                foreach (HtmlNode cell in row.SelectNodes("th|td"))  //有些table 表头是写在 td中的
        //                {

        //                    htTable.Columns.Add(cell.InnerText, typeof(string));
        //                }
        //            }
        //            else
        //            {
        //                DataRow TempRow = htTable.NewRow();
        //                foreach (HtmlNode cell in row.SelectNodes("th|td"))
        //                {

        //                    var position = row.SelectNodes("th|td").IndexOf(cell);
        //                    TempRow[htTable.Columns[position].ColumnName] = cell.InnerText;
        //                }
        //                htTable.Rows.Add(TempRow);
        //            }
        //        }
        //        return htTable;
        //    }
        //    catch (Exception e)
        //    {
        //        return null;
        //    }
        //}


        #endregion

        #endregion


        #region 58 Poker游戏


        /// <summary>
        /// 接口地址
        /// </summary>
        private const string PokerApiUrl = "https://api.58poker.net";//测试
        private const string site = "wbtest";//站台
        private const string ApiUser = "wp123";//"admin";//接口账号
        private const string ApiUserPass = "123456";//"test1234";//接口账号密码
        /// <summary>
        /// 缓存（token存放）
        /// </summary>
        Cache cache = new Cache();


        /// <summary>
        /// Poker登录接口获取token值
        /// </summary>
        /// <param name="account">账号</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Game/PokerLoginApi")]
        public string PokerLoginApi(string account, string password)
        {
            string RetData = string.Empty;
            try
            {
                //dynamic data = new System.Dynamic.ExpandoObject();
                //data.account = "58qp123"; //admin
                //data.password = "aa123456"; //test1234
                //data.site = "bar";
                string jsonparm = "{\"account\": \"" + account + "\",\"site\": \"" + site + "\",\"password\": \"" + password + "\"}";

                RetData = ToolsHelper.JsonHttpClient(PokerApiUrl + "/v1/auth/login", jsonparm);
                if (IsJSON(RetData))
                {
                    PokerLoginModel retBase = Newtonsoft.Json.JsonConvert.DeserializeObject<PokerLoginModel>(RetData);
                    if (retBase.code == "200")
                    {
                        cache.Insert("access_token", retBase.data.access_token);
                        cache.Insert("access_tokenOutTime", DateTime.Now.AddSeconds(Convert.ToDouble(retBase.data.expires_in)));
                        //PokerToken = retBase.data.access_token;
                        //var a2 = Convert.ToDouble(retBase.data.expires_in);
                        //var a1 = DateTime.Now.AddSeconds(Convert.ToDouble(retBase.data.expires_in));
                        //PokerTokenOutTime = DateTime.Now.AddSeconds(Convert.ToDouble(retBase.data.expires_in));
                    }
                }
            }
            catch (Exception ex)
            {
                RetData = ex.Message;
            }
            return RetData;
        }

        /// <summary>
        /// Poker更新token值
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("Game/PokerUpdateToken")]
        public string PokerUpdateToken()
        {
            string RetData = string.Empty;
            try
            {
                if (cache.Get("access_token") == null)
                {
                    return "token为空！";
                }
                RetData = JsonAothHttpClient(PokerApiUrl + "/v1/auth/refresh", "", cache.Get("access_token").ToString());
                if (IsJSON(RetData))
                {
                    PokerLoginModel retBase = Newtonsoft.Json.JsonConvert.DeserializeObject<PokerLoginModel>(RetData);
                    if (retBase.code == "200")
                    {
                        //Newtonsoft.Json.Linq.JObject jObject = Newtonsoft.Json.Linq.JObject.Parse(retBase.data);
                        //PokerToken = jObject["access_token"].ToString();
                        //PokerTokenOutTime = DateTime.Now.AddSeconds(Convert.ToDouble(jObject["expires_in"]));
                        cache.Insert("access_token", retBase.data.access_token);
                        cache.Insert("access_tokenOutTime", DateTime.Now.AddSeconds(Convert.ToDouble(retBase.data.expires_in)));
                    }
                }
            }
            catch (Exception ex)
            {
                RetData = ex.Message;
            }
            return RetData;
        }

        /// <summary>
        /// Poker新增玩家帐号
        /// </summary>
        /// <param name="account">玩家账号</param>
        /// <param name="password">密码</param>
        /// <param name="phone">手机</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Game/PokerPlayers")]
        public string PokerPlayers(string account, string password, string phone = "")
        {
            string RetData = string.Empty;
            try
            {
                StringBuilder str = new StringBuilder();
                str.Append("{");
                str.AppendFormat("\"account\":\"{0}\",", account);
                str.AppendFormat("\"password\":\"{0}\",", password);
                str.AppendFormat("\"site\":\"{0}\",", site);
                str.AppendFormat("\"phone\":\"{0}\"", phone);
                str.Append("}");
                if (cache.Get("access_token") == null)
                {
                    PokerLoginApi(ApiUser, ApiUserPass);
                }
                else if (string.IsNullOrEmpty(cache.Get("access_tokenOutTime").ToString()) || Convert.ToDateTime(cache.Get("access_tokenOutTime").ToString()) <= DateTime.Now)
                {
                    //更新token值
                    PokerUpdateToken();
                }
                RetData = JsonAothHttpClient(PokerApiUrl + "/v1/players", str.ToString(), cache.Get("access_token").ToString());
                if (IsJSON(RetData))
                {
                    PokerPalyesModel retBase = Newtonsoft.Json.JsonConvert.DeserializeObject<PokerPalyesModel>(RetData);
                    if (retBase.code == "200")
                    {
                        return retBase.msg;//balance
                    }
                }
            }
            catch (Exception ex)
            {
                RetData = ex.Message;
            }
            return RetData;
        }

        /// <summary>
        /// Poker玩家储值
        /// </summary>
        /// <param name="player">玩家名称</param>
        /// <param name="money">金额</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Game/PokerDeposit")]
        public string PokerDeposit(string player, string money)
        {
            string RetData = string.Empty;
            try
            {
                StringBuilder str = new StringBuilder();
                str.Append("{");
                str.AppendFormat("\"money\":\"{0}\"", money);
                str.Append("}");
                if (cache.Get("access_token") == null)
                {
                    PokerLoginApi(ApiUser, ApiUserPass);
                }
                else if (cache.Get("access_tokenOutTime") == null || Convert.ToDateTime(cache.Get("access_tokenOutTime").ToString()) <= DateTime.Now)
                {
                    //更新token值
                    PokerUpdateToken();
                }
                //v1/players/{{player}}/deposit
                RetData = JsonAothHttpClient(PokerApiUrl + "/v1/players/" + player + "@" + site + "/deposit", str.ToString(), cache.Get("access_token").ToString());
                if (IsJSON(RetData))
                {
                    PokerDepositModel retBase = Newtonsoft.Json.JsonConvert.DeserializeObject<PokerDepositModel>(RetData);
                    if (retBase.code == "200")
                    {
                        return retBase.msg;
                    }
                }
            }
            catch (Exception ex)
            {
                RetData = ex.Message;
            }
            return RetData;
        }

        /// <summary>
        /// Poker玩家提现
        /// </summary>
        /// <param name="player">玩家名称</param>
        /// <param name="money">金额</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Game/PokerWithdraw")]
        public string PokerWithdraw(string player, string money)
        {
            string RetData = string.Empty;
            try
            {
                StringBuilder str = new StringBuilder();
                str.Append("{");
                str.AppendFormat("\"money\":\"{0}\"", money);
                str.Append("}");
                if (cache.Get("access_token") == null)
                {
                    PokerLoginApi(ApiUser, ApiUserPass);
                }
                else if (string.IsNullOrEmpty(cache.Get("access_tokenOutTime").ToString()) || Convert.ToDateTime(cache.Get("access_tokenOutTime").ToString()) <= DateTime.Now)
                {
                    //更新token值
                    PokerUpdateToken();
                }
                //v1/players/{{player}}/withdraw
                RetData = JsonAothHttpClient(PokerApiUrl + "/v1/players/" + player + "@" + site + "/withdraw", str.ToString(), cache.Get("access_token").ToString());
                if (IsJSON(RetData))
                {
                    PokerDepositModel retBase = Newtonsoft.Json.JsonConvert.DeserializeObject<PokerDepositModel>(RetData);
                    if (retBase.code == "200")
                    {
                        return retBase.msg;
                    }
                }
            }
            catch (Exception ex)
            {
                RetData = ex.Message;
            }
            return RetData;
        }

        /// <summary>
        /// Poker查询玩家余额
        /// </summary>
        /// <param name="player">玩家名称</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Game/PokerBalance")]
        public string PokerBalance(string player)
        {
            string RetData = string.Empty;
            try
            {
                if (cache.Get("access_token") == null)
                {
                    PokerLoginApi(ApiUser, ApiUserPass);
                }
                else if (string.IsNullOrEmpty(cache.Get("access_tokenOutTime").ToString()) || Convert.ToDateTime(cache.Get("access_tokenOutTime").ToString()) <= DateTime.Now)
                {
                    //更新token值
                    PokerUpdateToken();
                }
                //v1/players/{{player}}/balance
                RetData = JsonAothHttpClient(PokerApiUrl + "/v1/players/" + player + "@" + site + "/balance", "", cache.Get("access_token").ToString(), "GET");
                if (IsJSON(RetData))
                {
                    PokerBalanceModel retBase = Newtonsoft.Json.JsonConvert.DeserializeObject<PokerBalanceModel>(RetData);
                    if (retBase.code == "200")
                    {
                        return retBase.data.balance;//balance
                    }
                }
            }
            catch (Exception ex)
            {
                RetData = ex.Message;
            }
            return RetData;
        }

        /// <summary>
        /// Poker查询玩家下注记录
        /// </summary>
        /// <param name="operators">操作人员</param>
        /// <param name="starTime">开始时间（Y-m-d H:i:s）</param>
        /// <param name="endTime">结束时间（Y-m-d H:i:s）</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Game/PokerOperators")]
        public string PokerOperators(string operators, string starTime, string endTime)
        {
            string RetData = string.Empty;
            try
            {
                StringBuilder str = new StringBuilder();
                str.AppendFormat("start_at={0}", starTime);
                str.AppendFormat("&end_at={0}", endTime);
                if (cache.Get("access_token") == null)
                {
                    PokerLoginApi(ApiUser, ApiUserPass);
                }
                else if (string.IsNullOrEmpty(cache.Get("access_tokenOutTime").ToString()) || Convert.ToDateTime(cache.Get("access_tokenOutTime").ToString()) <= DateTime.Now)
                {
                    //更新token值
                    PokerUpdateToken();
                }
                //v1/bet-results/operators/{{operator}}
                RetData = JsonAothHttpClient(PokerApiUrl + "/v1/bet-results/operators/" + operators + "@" + site + "?" + str.ToString(), "", cache.Get("access_token").ToString(), "GET");
                if (IsJSON(RetData))
                {
                    PokerOperatorsModel retBase = Newtonsoft.Json.JsonConvert.DeserializeObject<PokerOperatorsModel>(RetData);
                    if (retBase.code != "200")
                    {
                        return retBase.msg;
                    }
                }
            }
            catch (Exception ex)
            {
                RetData = ex.Message;
            }
            return RetData;
        }

        /// <summary>
        /// Poker获取游戏列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("Game/PokerGamesList")]
        public string PokerGamesList()
        {
            string RetData = string.Empty;
            try
            {

                if (cache.Get("access_token") == null)
                {
                    PokerLoginApi(ApiUser, ApiUserPass);
                }
                else if (string.IsNullOrEmpty(cache.Get("access_tokenOutTime").ToString()) || Convert.ToDateTime(cache.Get("access_tokenOutTime").ToString()) <= DateTime.Now)
                {
                    //更新token值
                    PokerUpdateToken();
                }

                RetData = JsonAothHttpClient(PokerApiUrl + "/v1/games", "", cache.Get("access_token").ToString(), "GET");
                if (IsJSON(RetData))
                {
                    PokerGameModel retBase = Newtonsoft.Json.JsonConvert.DeserializeObject<PokerGameModel>(RetData);
                    if (retBase.code != "200")
                    {
                        return retBase.msg;
                    }
                }
            }
            catch (Exception ex)
            {
                RetData = ex.Message;
            }
            return RetData;
        }

        /// <summary>
        /// Poker游戏登录
        /// </summary>
        /// <param name="player">玩家</param>
        /// <param name="password">玩家密码</param>
        /// <param name="game">游戏code值</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Game/PokerGamesLogin")]
        public string PokerGamesLogin(string player, string password, string game = "")
        {
            string RetData = string.Empty;
            try
            {

                StringBuilder str = new StringBuilder();
                str.Append("{");
                str.AppendFormat("\"password\":\"{0}\",", password);
                str.AppendFormat("\"lang\":\"{0}\"", "cn");//语系 cn: 简体中文 tw : 繁體中文 en : 英文
                str.Append("}");

                if (cache.Get("access_token") == null)
                {
                    PokerLoginApi(ApiUser, ApiUserPass);
                }
                else if (string.IsNullOrEmpty(cache.Get("access_tokenOutTime").ToString()) || Convert.ToDateTime(cache.Get("access_tokenOutTime").ToString()) <= DateTime.Now)
                {
                    //更新token值
                    PokerUpdateToken();
                }
                //if (!string.IsNullOrEmpty(game))
                //{
                //    game = "/" + game;
                //}
                RetData = JsonAothHttpClient(PokerApiUrl + "/v1/players/" + player + "@" + site + "/wanna-play/" + game, str.ToString(), cache.Get("access_token").ToString());
                if (IsJSON(RetData))
                {
                    PokerGameLoginModel retBase = Newtonsoft.Json.JsonConvert.DeserializeObject<PokerGameLoginModel>(RetData);
                    if (retBase.code == "200")
                    {
                        return retBase.data.url;
                    }
                }
            }
            catch (Exception ex)
            {
                RetData = ex.Message;
            }
            return RetData;
        }


        /// <summary>
        /// HTTP请求
        /// </summary>
        /// <param name="serviceAddress">请求地址</param>
        /// <param name="content">数据内容</param>
        /// <param name="token">token值</param>
        /// <param name="Method">请求方式</param>
        /// <returns></returns>
        public static string JsonAothHttpClient(string serviceAddress, string content, string token, string Method = "POST")
        {
            string retData = string.Empty;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(serviceAddress);

                request.Method = Method;
                request.ContentType = "application/json";
                request.Headers.Add("Authorization", "Bearer " + token);
                if (!string.IsNullOrEmpty(content))
                {
                    string strContent = content;
                    using (StreamWriter dataStream = new StreamWriter(request.GetRequestStream()))
                    {
                        dataStream.Write(strContent);
                        dataStream.Close();
                    }
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

        /// <summary>
        /// 判读json格式
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsJSON(string str)
        {
            try
            {
                //Newtonsoft.Json.JsonConvert.DeserializeObject<object>(str);
                Newtonsoft.Json.Linq.JObject.Parse(str);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        #endregion


        #region 路虎 游戏

        /// <summary>
        /// 版本号
        /// </summary>
        private const string version = "1.0";

        /// <summary>
        /// 商户号
        /// </summary>
        private const int channelId = 27527;//80856

        /// <summary>
        /// 新增玩家账户
        /// </summary>
        /// <param account="account"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Game/LandRoverCreateUser")]
        public async Task<string> LandRoverCreateUser(string account)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("version={0}&", version);
            stringBuilder.AppendFormat("channelId={0}&", channelId);
            string jsonData = ToolsHelper.AESEncrypt("{\"accountId\":\"" + account + "\",\"currency\":\"CNY\"}");//需进入AES设置加密密码和密钥
            stringBuilder.AppendFormat("data={0}", jsonData);
            string result = string.Empty;
            await Task.Run(() =>
            {
                result = ToolsHelper.AESDecrypt(ToolsHelper.HttpPost("http://dev.virtual-technology.com:8088/td_create_account", stringBuilder.ToString(), ""));
            });
            var retobj = JObject.Parse(result);
            string errorCode = retobj["errorCode"].ToString();
            if (errorCode == "0")
            {
                return "添加成功！";
            }
            else
            {
                return "添加失败！";
            }
        }

        /// <summary>
        /// 游戏页面登入
        /// </summary>
        /// <param name="token">由平台端自行产生 token</param>
        /// <param name="gametype">游戏类型（Game ID）</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Game/LandRoverLoginGame")]
        public async Task<string> LandRoverLoginGame(string token = "9de72c3c/5be74eb1b14a8cf58ca5/b6c4", string gametype = "BuffaloBlitz")
        {

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("version={0}&", version);
            stringBuilder.AppendFormat("language={0}&", "cn");
            stringBuilder.AppendFormat("channelId={0}&", channelId);
            string aseEncryptData = ToolsHelper.AESEncrypt("{\"token\":\""+ token + "\"}");//需进入AES设置加密密码和密钥  

            stringBuilder.AppendFormat("data={0}", System.Web.HttpUtility.UrlEncode(aseEncryptData));

            string result = string.Empty;
            await Task.Run(() =>
            {
                result = "http://test-client.virtual-technology.com/" + gametype + "?" + stringBuilder.ToString();
            });

            return result;
        }

        /// <summary>
        /// 根据token查询玩家信息（回调）
        /// </summary>
        /// <param name="data"></param>
        [HttpPost]
        [Route("Game/LandRoverGetLoginAccount")]
        public HttpResponseMessage LandRoverGetLoginAccount([FromBody]JObject data)
        {

            HttpResponseMessage response = new HttpResponseMessage();
            string retData = string.Empty;
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                ILog log = LogManager.GetLogger("game test");
                log.Info("根据token查询玩家信息（回调）：" + data.ToString());

                var dataobj = JObject.Parse(ToolsHelper.AESDecrypt(data["data"].ToString()));

                log.Info("根据token查询玩家信息：data：" + dataobj);

                string token = dataobj["token"].ToString();
                log.Info("根据token查询玩家信息： token：" + token);

                if (true)
                {
                    //根据token查询玩家信息返回
                     retData = ToolsHelper.AESEncrypt("{\"channelId\":\"" + channelId + "\",\"accountId\":\"test003\",\"nickName\":\"test003\",\"errorCode\":0}");//需进入AES设置加密密码和密钥
                    log.Info("根据token查询玩家信息： retData：" + retData);
                    //return retData;
                }
            }
            catch (Exception ex)
            {
                //
                ILog log = LogManager.GetLogger("game 错误日志");
                log.Info("根据token查询玩家信息： 错误日志：" + ex.Message);

                retData = ToolsHelper.AESEncrypt("{\"channelId\":\"" + channelId + "\",\"accountId\":\"\",\"nickName\":\"\",\"errorCode\":3}");//需进入AES设置加密密码和密钥
            }

            response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(retData, Encoding.GetEncoding("UTF-8"), "application/x-www-form-urlencoded")
            };
            return response;
        }


        /// <summary>
        /// VT 发送登入结果（回调）
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Game/LandRoverLoginResponse")]
        public void LandRoverLoginResponse([FromBody]JObject data)
        {
            log4net.Config.XmlConfigurator.Configure();
            ILog log = LogManager.GetLogger("game test");
            log.Info("VT 发送登入结果：" + data);

            var dataobj = JObject.Parse(ToolsHelper.AESDecrypt(data["data"].ToString()));
            string errorCode = dataobj["errorCode"].ToString();
            log.Info("VT 发送登入结果：accountId：" + dataobj["accountId"].ToString());
            log.Info("VT 发送登入结果：errorCode：" + dataobj["errorCode"].ToString());
            if (errorCode == "0")
            {
                //登录成功！
            }
        }


        /// <summary>
        /// 玩家充值、提款
        /// </summary>
        /// <param name="account">玩家唯一标识符(账号)</param>
        /// <param name="amount">转点金额，金额必须大于 0 且小于 等于[MaxTransactionAmount]，精确到小数点第二位</param>
        /// <param name="type">转点类型 (0:入款， 1:提款)</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Game/LandRoverTransaction")]
        public async Task<string> LandRoverTransaction(string account, string amount, int type = 0)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("version={0}&", version);
            stringBuilder.AppendFormat("channelId={0}&", channelId);
            string jsonData = ToolsHelper.AESEncrypt("{\"accountId\":\"" + account + "\",\"currency\":\"CNY\",\"serialNumber\":\"" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "\",\"type\":\"" + type + "\",\"amount\":\"" + amount + "\"}");//需进入AES设置加密密码和密钥

            stringBuilder.AppendFormat("data={0}", jsonData);
            string result = string.Empty;
            await Task.Run(() =>
            {
                result = ToolsHelper.HttpPost("http://dev.virtual-technology.com:8088/td_userwallet_transaction", stringBuilder.ToString(), "");
            });

            JObject objects = JObject.Parse(ToolsHelper.AESDecrypt(result));
            string errorCode = objects["errorCode"].ToString();

            if (errorCode == "0")
            {
                return "成功";
            }
            else if (errorCode == "10")
            {
                return "余额不足";
            }
            else if (errorCode == "14")
            {
                return "币值错误";
            }
            else
            {
                return "未知错误！";
            }
        }

        /// <summary>
        /// 查询玩家余额
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Game/LandRoverGetBalance")]
        public async Task<string> LandRoverGetBalance(string account)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendFormat("version={0}&", version);
            stringBuilder.AppendFormat("channelId={0}&", channelId);
            string jsonData = ToolsHelper.AESEncrypt("{\"accountId\":\"" + account + "\"}");//需进入AES设置加密密码和密钥

            stringBuilder.AppendFormat("data={0}", jsonData);
            string result = string.Empty;

            await Task.Run(() =>
            {
                result = ToolsHelper.HttpPost("http://dev.virtual-technology.com:8088/td_balance", stringBuilder.ToString(), "");
            });

            JObject objects = JObject.Parse(ToolsHelper.AESDecrypt(result));
            //objects["errorCode"].ToString() //成功(0) 、 其他情况请参考错误讯息代码说明
            
            return objects["balance"].ToString();

        }

        /// <summary>
        /// 获取投注记录
        /// </summary>
        /// <param name="starTime">开始时间（yyyyMMddHHmmss）</param>
        /// <param name="endTime">结束时间（yyyyMMddHHmmss）</param>
        /// <param name="offset">从第 n 笔资料开始抓取</param>
        /// <param name="limit">抓取资料的笔数，预设为10000，且不能超过 10000</param>
        /// <param name="method">data: 抓取數據，rows: 抓取數據的總筆數，overview: 抓取数据并提供统计资讯</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Game/LandRoverGetBet")]
        public async Task<List<Betdata>> LandRoverGetBet(string starTime, string endTime,int offset = 0,int limit = 1000)
        {
            List<Betdata> retList = new List<Betdata>();
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                string result = string.Empty;
                string method = "data";

                //stringBuilder.AppendFormat("version={0}&", version);
                stringBuilder.AppendFormat("channelId={0}&", channelId);

                if (!string.IsNullOrEmpty(starTime))
                {
                    starTime = DateTime.ParseExact(starTime, "yyyyMMddHHmmss", CultureInfo.CurrentCulture).ToString("yyyy-MM-dd'T'HH:mm:ss.fff+08:00");
                }
                if (!string.IsNullOrEmpty(endTime))
                {
                    endTime = DateTime.ParseExact(endTime, "yyyyMMddHHmmss", CultureInfo.CurrentCulture).ToString("yyyy-MM-dd'T'HH:mm:ss.fff+08:00");
                }

                string jsonData = ToolsHelper.AESEncrypt("{\"startTime\":\"" + starTime + "\",\"endTime\":\"" + endTime + "\",\"offset\":"+ offset + ",\"limit\":"+ limit + ",\"removeComma\":\"True\",\"method\":\"" + method + "\"}");//需进入AES设置加密密码和密钥

                stringBuilder.AppendFormat("data={0}", System.Web.HttpUtility.UrlEncode(jsonData));

                await Task.Run(() =>
                {
                    result = ToolsHelper.HttpPost("http://dev.virtual-technology.com/client/api/getGameRecord.php", stringBuilder.ToString());
                });

                var retobj = Newtonsoft.Json.JsonConvert.DeserializeObject<TigerRetdata>(ToolsHelper.AESDecrypt(result));

                if (retobj.errorCode == "0")
                {
                    retList = retobj.data;
                }
                else
                {
                    //错误或者暂无数据！
                }
            }
            catch (Exception ex)
            {
                //result = ex.Message;
            }
            return retList;
        }


        #endregion


        #region HGDL 彩票

        //接口地址
        private const string HgdlApiUrl = "http://admin.bb558899.com";


        /// <summary>
        /// 建立會員
        /// </summary>
        /// <param name="account">帳號</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Game/HGDLCreateMember")]
        public string HGDLCreateMember(string account)
        {
            string RetData = string.Empty;
            try
            {
                string jsonparm = "{\"account\": \"" + account + "\",\"lang\": \"cn\"}";

                RetData = ToolsHelper.JsonHttpClient(HgdlApiUrl + "/api/user/createMember", jsonparm);
                if (IsJSON(RetData))
                {
                    JObject retBase = JObject.Parse(RetData);
                    if (retBase["pass"].ToString() == "true"|| retBase["pass"].ToString() == "True")
                    {
                        return "添加成功！";
                    }
                    else
                    {
                        return "添加失败！";
                    }
                }
            }
            catch (Exception ex)
            {
                RetData = ex.Message;
            }
            return RetData;
        }

        /// <summary>
        /// 取得會員資訊
        /// </summary>
        /// <param name="account">帳號</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Game/HGDLGetMemberPoint")]
        public string HGDLGetMemberPoint(string account)
        {
            string RetData = string.Empty;
            try
            {
                string jsonparm = "{\"account\": \"" + account + "\"}";

                RetData = ToolsHelper.JsonHttpClient(HgdlApiUrl + "/api/user/getMemberInfo", jsonparm);
                if (IsJSON(RetData))
                {
                    JObject retBase = JObject.Parse(RetData);
                    if (retBase["pass"].ToString() == "true" || retBase["pass"].ToString() == "True")
                    {
                        return retBase["data"]["point"].ToString();
                    }
                    else
                    {
                        return "失败！"+ RetData;
                    }
                }
            }
            catch (Exception ex)
            {
                RetData = ex.Message;
            }
            return RetData;
        }

        /// <summary>
        /// 會員存（提）點數
        /// </summary>
        /// <param name="account">會員</param>
        /// <param name="point">點數（正：存；负：提）</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Game/HGDLAddPoint")]
        public string HGDLAddPoint(string account,decimal point)
        {
            string RetData = string.Empty;
            try
            {
                string jsonparm = "{\"account\": \"" + account + "\",\"point\": "+ point + "}";

                RetData = ToolsHelper.JsonHttpClient(HgdlApiUrl + "/api/point/addPoint", jsonparm);
                if (IsJSON(RetData))
                {
                    JObject retBase = JObject.Parse(RetData);
                    if (retBase["pass"].ToString() == "true" || retBase["pass"].ToString() == "True")
                    {
                        return "成功！";
                    }
                    else
                    {
                        return "失败！"+ RetData;
                    }
                }
            }
            catch (Exception ex)
            {
                RetData = ex.Message;
            }
            return RetData;
        }

        /// <summary>
        /// 取得會員登入Token，生成登录地址
        /// </summary>
        /// <param name="account">會員帳號 </param>
        /// <param name="type">0：返回桌面登录地址，1：返回APP登录地址（默认为：0） </param>
        /// <returns></returns>
        [HttpPost]
        [Route("Game/HGDLGetMemberToken")]
        public string HGDLGetMemberToken(string account,int type=0)
        {
            string RetData = string.Empty;
            try
            {
                string jsonparm = "{\"account\": \"" + account + "\"}";

                RetData = ToolsHelper.JsonHttpClient(HgdlApiUrl + "/api/login/getMemberToken", jsonparm);
                if (IsJSON(RetData))
                {
                    JObject retBase = JObject.Parse(RetData);
                    if (retBase["pass"].ToString() == "true" || retBase["pass"].ToString() == "True")
                    {
                        //token  mobileURL  desktopURL
                        if (type == 0)
                        {
                            return retBase["data"]["desktopURL"].ToString()+ "?token="+ retBase["data"]["token"].ToString(); //+"&toURL="
                        }
                        else
                        {
                            return retBase["data"]["mobileURL"].ToString() + "?token=" + retBase["data"]["token"].ToString(); //+"&toURL="
                        }
                    }
                    else
                    {
                        return "失败！"+ RetData;
                    }
                }
            }
            catch (Exception ex)
            {
                RetData = ex.Message;
            }
            return RetData;
        }

        /// <summary>
        /// 取得押注紀錄
        /// </summary>
        /// <param name="account">會員帳號</param>
        /// <param name="starTime">开始时间（格式：‘2018-07-01'）</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Game/HGDLGetBetList")]
        public List<HGDLBetModel> HGDLGetBetList(string account, string starTime,string endTime)
        {
            string RetData = string.Empty;
            List<HGDLBetModel> Betlist = new List<HGDLBetModel>();
            try
            {
                string jsonparm = "{\"memberAccount\": \"" + account + "\",\"sbtBillingDate\": " + starTime + ",\"ebtBillingDate\": " + endTime + "}";

                RetData = ToolsHelper.JsonHttpClient(HgdlApiUrl + "/api/betList/getBetList", jsonparm);
                
                if (IsJSON(RetData))
                {
                    //解析记录 
                    JObject retBase = JObject.Parse(RetData);
                    foreach (var item in retBase)
                    {
                        Betlist.Add(Newtonsoft.Json.JsonConvert.DeserializeObject<HGDLBetModel>(item.Value.ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                RetData = ex.Message;
            }
            return Betlist;
        }

        #endregion

        #region MW 游戏

        #region 配置


        // MW 提供
        static string MW_public_key = "<RSAKeyValue><Modulus>jqqRdDVHxPZptDDgfn84ca4icbfjf2tg6mk4zzwH0oQtL3IKh6+3G4Sf9BagiujVR+LS6ErHQ+Y2UnFRPKAo3hMcURyv0Q8NQMcB2U4YY7xMl1f8qDawlQbBbXpqgu3d89oxQFLAAUdtaWShm9FczotUDYsqbgF8MayRvFbpmW8=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
        // EC key pair 取得
        static string EC_private_key = "<RSAKeyValue><Modulus>kSOoUFt7nB64W+FvVDG97vZxlzOlvPtHspbfziLE8MP9F+mJeAB67B0BhZo24NAneT0uBXU7z7+Z\r\nhppnA9eN3B4PIreb+tYJ42s7jyezvuxCm4qpw9kvxl/tTHTLfEuHX4IvrkKEKZ6MQsaTHG0wYmAL\r\nQm7lrkeo/RkZ6KxGj2U=</Modulus><Exponent>AQAB</Exponent><P>0u4eHlvbs0mSxNsFwwZIZubSWGcGCukx+y6vTNTcLpxE4D/kDHv5kaNgRxoHW/+VyMoLKA4ADIiY\r\n9m1rpEPN+w==</P><Q>sCbHXCL/8TYMXePA/omp1kshgxVG9xSvXQN5z4RlZeLTfxnn41oAso59IKhQzbY683aR2fxvR93N\r\n3Ac8tgV6Hw==</Q><DP>jB7Cw0giWqe1aDBXU4cI8dLESRWnXfgry8DnkxKUciI9XvsSc3ioAaeWfaU83lCbIBeX8bPbNHhJ\r\npOprZ2PjoQ==</DP><DQ>DMGiKk/2jPdHCf2WWliJzhT+xkliLD55PJkl1xtVZH16p3euzU4VQtkCwrytrVgHCN6LTWf0fxXW\r\n9JopkVzwLQ==</DQ><InverseQ>Qsyo0HElHVrd9UnH640eTxP1Lx0Xv6kNNUClT5E5Sxx3Xn5pCPkbSrrPCg1/FGU+S8xUjOOXh4eC\r\nq7GPB6rStw==</InverseQ><D>JBv79yobgcb+1RUsLoVFnNtBfX3DAVZ/CwaQXee2mbQZFsbqhamQ64d35nZsqtf+yiIXfhIhtFkV\r\n6DAn6wPI7hdapLu8sBMSNFTYfr8yruzrNtzVcdiuOC4JOAz8NnFtJM2c6vbpESzoionhZMLpDqzy\r\ncN+5WFsLOPoQiQUc/40=</D></RSAKeyValue>";
        // EC key pair 取得
        static string EC_public_key = "<RSAKeyValue><Modulus>kSOoUFt7nB64W+FvVDG97vZxlzOlvPtHspbfziLE8MP9F+mJeAB67B0BhZo24NAneT0uBXU7z7+Z\r\nhppnA9eN3B4PIreb+tYJ42s7jyezvuxCm4qpw9kvxl/tTHTLfEuHX4IvrkKEKZ6MQsaTHG0wYmAL\r\nQm7lrkeo/RkZ6KxGj2U=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
        // 站點ID MW 提供
        static string siteId = "10019500";

        static Dictionary<string, object> dataMap = new Dictionary<string, object>();
        static string EC_AES_key = string.Empty;
        static string domainURL = string.Empty;
        static string apiURL = string.Empty;
        static string func = string.Empty;       //[1] 
        static string resultType = "json"; //[2] 
        static string lang = "cn";       //[3]
        static string data = string.Empty;       //[4]
        static string key = string.Empty;        //[5]

        #endregion

        /// <summary>
        /// 获取Domain 地址
        /// </summary>
        /// <returns></returns>
        public string MWGetdomain()
        {
            func = "domain";
            dataMap.Clear();
            dataMap["timestamp"] = ToolsHelper.GetTimeStamp(); //"1635247523365"

            try
            {
                JObject json = sendApi(dataMap);
                return json["domain"].ToString();
            }
            catch (Exception ex)
            {
                string defaultDomain = "https://www.666wins.com/as-lobby/";
                return defaultDomain;
            }
        }


        /// <summary>
        ///  授权 Oauth API （返回登录地址）
        /// </summary>
        /// <param name="Uid"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Game/MWOauth")]
        public string MWOauth(string Uid)
        {
            domainURL = MWGetdomain();

            func = "oauth";
            dataMap.Clear();
            dataMap["uid"] = Uid;// 区分大小写，总长度必须小于32个字符 (uid、utoken、merchantId 接入方自行生成與管理) 
            dataMap["utoken"] = ToolsHelper.GetMd5(Uid); // ec平台用户授权码，一次授权之后不可变更，长度必须为32个字符
            dataMap["timestamp"] = ToolsHelper.GetTimeStamp();
            dataMap["jumpType"] = "0"; //跳转页面类型（0： 游戏大厅； 1：查询页面； 2： APP 引导页面； 3：获取 APP启动信息；）
            dataMap["gameId"] = "";
            JObject jobjects = sendApi(dataMap);

            return domainURL+jobjects["interface"].ToString();
        }

        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <param name="Uid"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Game/MWGetUserInfo")]
        public string MWGetUserInfo(string Uid)
        {
            domainURL = MWGetdomain();

            func = "userInfo";
            dataMap.Clear();
            dataMap["uid"] = Uid;// 区分大小写，总长度必须小于32个字符 (uid、utoken、merchantId 接入方自行生成與管理) 
            dataMap["utoken"] = ToolsHelper.GetMd5(Uid); // ec平台用户授权码，一次授权之后不可变更，长度必须为32个字符
            dataMap["timestamp"] = ToolsHelper.GetTimeStamp();
            JObject jobjects = sendApi(dataMap);

            return jobjects.ToString();
        }

        /// <summary>
        /// 充值
        /// </summary>
        /// <param name="Uid"></param>
        /// <param name="Amount"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Game/MWRecharge")]
        public string MWRecharge(string Uid, decimal Amount)
        {
            func = "transferPrepare";
            string TimeStamp = ToolsHelper.GetTimeStamp();

            dataMap.Clear();
            dataMap["uid"] = Uid;// 区分大小写，总长度必须小于32个字符 (uid、utoken、merchantId 接入方自行生成與管理) 
            dataMap["utoken"] = ToolsHelper.GetMd5(Uid);// + "_12345678901234567890"; // ec平台用户授权码，一次授权之后不可变更，长度必须为32个字符
            dataMap["transferType"] = 0;
            dataMap["transferAmount"] = Amount;
            dataMap["transferOrderNo"] = TimeStamp;
            dataMap["transferOrderTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            dataMap["transferClientIp"] = "127.0.0.1";
            dataMap["timestamp"] = TimeStamp;

            var T_obj = sendApi(dataMap);


            func = "transferPay";

            dataMap.Clear();
            dataMap["uid"] = Uid;// 区分大小写，总长度必须小于32个字符 (uid、utoken、merchantId 接入方自行生成與管理) 
            dataMap["utoken"] = ToolsHelper.GetMd5(Uid); // ec平台用户授权码，一次授权之后不可变更，长度必须为32个字符
            dataMap["asinTransferOrderNo"] = T_obj["asinTransferOrderNo"];
            dataMap["asinTransferOrderTime"] = T_obj["asinTransferDate"];
            dataMap["transferOrderNo"] = TimeStamp;
            dataMap["transferAmount"] = Amount;
            dataMap["transferClientIp"] = "127.0.0.1";
            dataMap["timestamp"] = TimeStamp;
            JObject retobj = sendApi(dataMap);
            if (retobj["ret"].ToString() == "0000")
            {
                return "成功！";
            }
            else
            {
                return "失败！";
            }
            
        }


        /// <summary>
        /// 转出
        /// </summary>
        /// <param name="Uid"></param>
        /// <param name="Amount"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Game/MWTurnOut")]
        public string MWTurnOut(string Uid, decimal Amount)
        {
            func = "transferPrepare";
            string TimeStamp = ToolsHelper.GetTimeStamp();

            dataMap.Clear();
            dataMap["uid"] = Uid;// 区分大小写，总长度必须小于32个字符 (uid、utoken、merchantId 接入方自行生成與管理) 
            dataMap["utoken"] = ToolsHelper.GetMd5(Uid); // ec平台用户授权码，一次授权之后不可变更，长度必须为32个字符
            dataMap["transferType"] = 1;
            dataMap["transferAmount"] = Amount;
            dataMap["transferOrderNo"] = TimeStamp;
            dataMap["transferOrderTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            dataMap["transferClientIp"] = "127.0.0.1";
            dataMap["timestamp"] = TimeStamp;

            var T_obj = sendApi(dataMap);


            func = "transferPay";

            dataMap.Clear();
            dataMap["uid"] = Uid;// 区分大小写，总长度必须小于32个字符 (uid、utoken、merchantId 接入方自行生成與管理) 
            dataMap["utoken"] = ToolsHelper.GetMd5(Uid); // ec平台用户授权码，一次授权之后不可变更，长度必须为32个字符
            dataMap["asinTransferOrderNo"] = T_obj["asinTransferOrderNo"];
            dataMap["asinTransferOrderTime"] = T_obj["asinTransferDate"];
            dataMap["transferOrderNo"] = TimeStamp;
            dataMap["transferAmount"] = Amount;
            dataMap["transferClientIp"] = "127.0.0.1";
            dataMap["timestamp"] = TimeStamp;

            JObject retobj = sendApi(dataMap);

            if (retobj["ret"].ToString() == "0000")
            {
                return "成功！";
            }
            else
            {
                return "失败！";
            }
        }

        /// <summary>
        /// 获取玩家游戏记录
        /// </summary>
        /// <param name="Uid"></param>
        /// <param name="beginTime">（格式：yyyy-MM-dd HH:mm:ss）</param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Game/MWGameRecordList")]
        public Object MWGameRecordList(string Uid,string beginTime,string endTime)
        {
            func = "usersgminfo";

            dataMap.Clear();
            dataMap["uid"] = Uid;// 区分大小写，总长度必须小于32个字符 (uid、utoken、merchantId 接入方自行生成與管理) 
            dataMap["beginTime"] = beginTime; 
            dataMap["endTime"] = endTime;

            return sendApi(dataMap);
        }

        /// <summary>
        /// 站点流水日志查询
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Game/MWSiteGameRecordList")]
        public Object MWSiteGameRecordList(string beginTime, string endTime,int page =1)
        {
            func = "siteUsergamelog";

            dataMap.Clear();
            dataMap["beginTime"] = beginTime;
            dataMap["endTime"] = endTime;
            dataMap["page"] = page;

            return sendApi(dataMap);
        }

        #region MW_Tools


        /// <summary>
        /// 请求接口  Turn out
        /// </summary>
        /// <param name="dataDic"></param>
        /// <returns></returns>
        private JObject sendApi(Dictionary<string, object> dataDic)
        {
            if (func.Equals("siteUsergamelog"))
                domainURL = domainURL.Replace("as-lobby", "as-service");

            if (func.Equals("domain"))
                apiURL = "http://www.168at168.com/as-lobby/api/domain?"; // Domain 請求網址
            else
                apiURL = domainURL + "api/" + func + "?";

            EC_AES_key = getAesKey();
            string contentJson = SetDataContent(ref dataDic);

            data = AES_Encrypt(EC_AES_key, contentJson);
            key = RSA_Encrypt(MW_public_key, EC_AES_key);

            Dictionary<string, string> postParameters = new Dictionary<string, string>();
            postParameters.Add("func", func);
            postParameters.Add("resultType", resultType);
            postParameters.Add("siteId", siteId);
            postParameters.Add("lang", lang);
            postParameters.Add("data", data);
            postParameters.Add("key", key);

            string res = HttpPostRequest(apiURL, postParameters);
            JObject json = Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(res);

            return json;
        }


        public string getAesKey()
        {
            string aesKey = string.Empty;
            string strPool = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz";
            int max = strPool.Length - 1;
            int keySize = 16;
            int idx = 0;
            Random rand = new Random();

            for (int i = 0; i < keySize; i++)
            {
                idx = rand.Next(0, max);
                aesKey += strPool[idx];
            }

            return aesKey;
        }


        const int KEYSIZE = 1024;
        static string dataSign = string.Empty;
        static string contentSign = string.Empty;

        // 文檔 data 数据规则 step1~4 : 組合signContent 生成Sign 建構Json 回傳JsonContent
        private string SetDataContent(ref Dictionary<string, object> data)
        {
            var Dy_data = new JObject();
            contentSign = string.Empty;

            // step1. 将API 接口所需参数的参数名，按照字母字典顺序先后排序.
            data = data.OrderBy(o => o.Key).ToDictionary(o => o.Key, p => p.Value);

            // step2. 将排序后的参数名以及对应参数值按以下方式进行拼接：AA=11BB=22...
            foreach (KeyValuePair<string, object> pair in data)
            {
                string key = pair.Key;
                object obj = pair.Value;
                string value = (obj == null) ? string.Empty : obj.ToString();
                contentSign += (key + "=" + value);
                Dy_data[key] = value;
            }

            // step3. 将步骤2 获得的参数，使用EC Platform Private Key 进行RSA 签名，获得API 接口所需传递的参数sign
            dataSign = RSA_SignData(EC_private_key, contentSign);

            // step4. 将步骤3 获得的参数sign，和步骤1 涉及的参数，构建JSON 数据
            Dy_data["sign"] = dataSign;
            string jsonContent = JsonConvert.SerializeObject(Dy_data);

            return jsonContent;
        }

        /// <summary>
        /// HTTP 请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postParameters"></param>
        /// <returns></returns>
        private string HttpPostRequest(string url, Dictionary<string, string> postParameters)
        {
            string postData = "";

            foreach (string key in postParameters.Keys)
            {
                if (postData.Length != 0)
                    postData += "&";
                postData += WebUtility.UrlEncode(key) + "=" + WebUtility.UrlEncode(postParameters[key]);
            }

            string func = postParameters["func"];
            // 如果有请求有问题, 请把这串 请求串 提供给MW 技术检视
            Console.WriteLine(string.Format("[{0}_RequestURL]:{1}", func, postData));

            HttpWebRequest myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
            myHttpWebRequest.Method = "POST";

            byte[] data = Encoding.ASCII.GetBytes(postData);

            myHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
            myHttpWebRequest.ContentLength = data.Length;

            Stream requestStream = myHttpWebRequest.GetRequestStream();
            requestStream.Write(data, 0, data.Length);
            requestStream.Close();

            HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
            Stream responseStream = myHttpWebResponse.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(responseStream, Encoding.UTF8);

            string pageContent = myStreamReader.ReadToEnd();

            myStreamReader.Close();
            responseStream.Close();

            myHttpWebResponse.Close();

            Console.WriteLine(string.Format("[{0}_Response]:{1}", func, pageContent));

            return pageContent;
        }

        /// <summary>
        /// 执行异步通知 解开收到的 key & data
        /// </summary>
        /// <param name="ecPrivateKey"></param>
        /// <param name="notifierKey"></param>
        /// <param name="notifierData"></param>
        /// <returns></returns>
        private string DecryptNotifierData(string privateKey, string notifierKey, string notifierData)// 暫時只能用 mwPrivateKey, 正確要用 ecPrivateKey
        {
            notifierKey = WebUtility.UrlDecode(notifierKey);
            notifierData = WebUtility.UrlDecode(notifierData);

            string aesKey = RSADecrypt(privateKey, notifierKey);
            string decryptData = AESDecrypt(aesKey, notifierData);

            return decryptData;
        }

        /// <summary>
        /// 验证回调的讯息是否正确
        /// </summary>
        /// <param name="originalMessage">DecryptNotifierData 出來的資料 </param>
        /// <param name="mwPublicKey"></param>
        /// <returns></returns>
        public bool VerifyData(string originalMessage, string mwPublicKey)
        {
            Dictionary<string, string> resDic = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(originalMessage);
            string signedMessage = resDic["sign"];
            var dic = resDic.OrderBy(item => item.Key);

            string sortContent = string.Empty;
            foreach (var item in dic)
            {
                if (item.Key.Equals("sign"))
                    continue;
                sortContent += item.Key + "=" + item.Value;
            }
            originalMessage = sortContent;

            bool isVerify = false;
            using (var rsa = new RSACryptoServiceProvider())
            {
                var encoder = new UTF8Encoding();
                byte[] bytesToVerify = encoder.GetBytes(originalMessage);
                byte[] signedBytes = Convert.FromBase64String(signedMessage);
                try
                {
                    rsa.FromXmlString(mwPublicKey);
                    isVerify = rsa.VerifyData(bytesToVerify, CryptoConfig.MapNameToOID("SHA1"), signedBytes);
                }
                catch (CryptographicException e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }
            return isVerify;
        }

        /// <summary>
        /// RSA解密
        /// </summary>
        /// <param name="privateKey"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        private string RSADecrypt(string privateKey, string msg)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(privateKey);

            byte[] plainbytes = rsa.Decrypt(Convert.FromBase64String(msg), false);
            return System.Text.Encoding.UTF8.GetString(plainbytes);
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="aesKey"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private string AESDecrypt(string aesKey, string data)
        {
            AesManaged tdes = new AesManaged();
            tdes.Key = Encoding.UTF8.GetBytes(aesKey);
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;
            ICryptoTransform crypt = tdes.CreateDecryptor();
            byte[] plain = Convert.FromBase64String(data);
            byte[] cipher = crypt.TransformFinalBlock(plain, 0, plain.Length);
            return System.Text.Encoding.UTF8.GetString(cipher);
        }


        #region Cryptography 相關
        /// <summary>
        /// 將內容RSA簽章
        /// </summary>
        /// <param name="input">Key</param>
        /// <param name="message">內容</param>
        /// <returns></returns>
        private string RSA_SignData(string input, string message)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(KEYSIZE);
            rsa.FromXmlString(input);

            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            byte[] textBytes = encoding.GetBytes(message);
            byte[] encryptedOutput = rsa.SignData(textBytes, new SHA1CryptoServiceProvider());

            return Convert.ToBase64String(encryptedOutput);
        }

        /// <summary>
        /// 字串加密
        /// </summary>
        /// <param name="input">Key</param>
        /// <param name="message">內容</param>
        /// <returns></returns>
        private string RSA_Encrypt(string input, string message)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(KEYSIZE);
            rsa.FromXmlString(input);

            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            byte[] textBytes = encoding.GetBytes(message);
            byte[] encryptedOutput = rsa.Encrypt(textBytes, false);  /// rsa加密模式是  ecb pkcs1padding 

            return Convert.ToBase64String(encryptedOutput);
        }

        /// <summary>
        /// AES 加密
        /// </summary>
        /// <param name="input">Key</param>
        /// <param name="message">內容</param>
        /// <returns></returns>
        public string AES_Encrypt(string input, string message)
        {
            AesManaged tdes = new AesManaged();
            tdes.Key = Encoding.UTF8.GetBytes(input);
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;
            ICryptoTransform crypt = tdes.CreateEncryptor();
            byte[] plain = Encoding.UTF8.GetBytes(message);
            byte[] cipher = crypt.TransformFinalBlock(plain, 0, plain.Length);

            return Convert.ToBase64String(cipher);
        }

        #endregion


        #endregion

        #endregion


    }
}