using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameWebAPI.Models
{
    /// <summary>
    /// 游戏model
    /// </summary>
    public class GameClass
    {

    }

    #region 游戏Model

    #region PNG MODEL 

    /// <summary>
    /// 通用model
    /// </summary>
    public class PNGEndpointBaseModel
    {

        /// <summary>
        /// 全球唯一的消息 id。
        /// </summary>
        public string MessageId { get; set; }
        /// <summary>
        /// 消息类型
        /// </summary>
        public int MessageType { get; set; }
        /// <summary>
        /// 消息添加到队列中的时
        /// </summary>
        public string MessageTimestamp { get; set; }
    }

    /// <summary>
    /// 玩家登录 （"MessageType": 1）
    /// </summary>
    public class PNGPlayerLogin : PNGEndpointBaseModel
    {
        /// <summary>
        /// 用户名或 ticket
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// 产品组
        /// </summary>
        public int ProductGroup { get; set; }
        /// <summary>
        /// 所连接的客户端 IP
        /// </summary>
        public string ClientIP { get; set; }
        /// <summary>
        /// 当开始游戏时指定的上下文id
        /// </summary>
        public int ContextId { get; set; }
        /// <summary>
        /// 语言代码
        /// </summary>
        public string Language { get; set; }
        /// <summary>
        /// 游戏的 Id
        /// </summary>
        public int GameId { get; set; }
        /// <summary>
        /// 客户端的用户代理
        /// </summary>
        public string AgentString { get; set; }
        /// <summary>
        /// 登录尝试的结果
        /// </summary>
        public string StatusCode { get; set; }
        /// <summary>
        /// 登录尝试的消息
        /// </summary>
        public string StatusMessage { get; set; }
        /// <summary>
        /// 用户外部 id
        /// </summary>
        public string ExternalUserId { get; set; }
        /// <summary>
        /// 用户昵称
        /// </summary>
        public string Nickname { get; set; }
        /// <summary>
        /// 玩游戏使用的货币
        /// </summary>
        public string Currency { get; set; }
        /// <summary>
        /// 用户的国家
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// 用户出生日期
        /// </summary>
        public string Birthdate { get; set; }
        /// <summary>
        /// 与用户相关的附属 id
        /// </summary>
        public string AffiliateId { get; set; }
        /// <summary>
        /// 用户注册时间
        /// </summary>
        public string Registration { get; set; }
        /// <summary>
        /// 用户性别
        /// </summary>
        public string Gender { get; set; }
    }


    /// <summary>
    /// 玩家登出（"MessageType": 2）
    /// </summary>
    public class PNGPlayerLogout : PNGEndpointBaseModel
    {

        /// <summary>
        /// 产品组
        /// </summary>
        public int ProductGroup { get; set; }
        /// <summary>
        /// 用户外部 id
        /// </summary>
        public string ExternalUserId { get; set; }
    }


    /// <summary>
    /// 交易保留（"MessageType": 3）
    /// </summary>
    public class PNGTransactionReserve: PNGEndpointBaseModel
    {
        /// <summary>
        /// 交易 id
        /// </summary>
        public long TransactionId { get; set; }
        /// <summary>
        /// 交易状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 添加到用户账户的金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 交易的 UTC 时间
        /// </summary>
        public string Time { get; set; }
        /// <summary>
        /// 产品组
        /// </summary>
        public int ProductGroup { get; set; }
        /// <summary>
        /// 用户外部 id
        /// </summary>
        public string ExternalUserId { get; set; }
        /// <summary>
        /// gamesession 的 Id
        /// </summary>
        public int GamesessionId { get; set; }
        /// <summary>
        /// 游戏的 Id
        /// </summary>
        public int GameId { get; set; }
        /// <summary>
        /// 该交易所属的回合
        /// </summary>
        public long RoundId { get; set; }
        /// <summary>
        /// 交易的货币
        /// </summary>
        public string Currency { get; set; }
        /// <summary>
        /// 外部交易 id
        /// </summary>
        public string ExternalTransactionId { get; set; }
        /// <summary>
        /// 交易完成之后的用户余额
        /// </summary>
        public decimal Balance { get; set; }

    }

    /// <summary>
    /// 赌场交易释放开启信息 （"MessageType": 4）
    /// </summary>
    public class PNGTransactionReleaseOpen : PNGEndpointBaseModel
    {
        /// <summary>
        /// 交易 id
        /// </summary>
        public long TransactionId { get; set; }
        /// <summary>
        /// 交易状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 添加到用户账户的金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 交易的 UTC 时间
        /// </summary>
        public string Time { get; set; }
        /// <summary>
        /// 产品组
        /// </summary>
        public int ProductGroup { get; set; }
        /// <summary>
        /// 用户外部 id
        /// </summary>
        public string ExternalUserId { get; set; }
        /// <summary>
        /// gamesession 的 Id
        /// </summary>
        public int GamesessionId { get; set; }
        /// <summary>
        /// gamesession 的状态
        /// </summary>
        public int GamesessionState { get; set; }
        /// <summary>
        /// 游戏的 Id
        /// </summary>
        public int GameId { get; set; }
        /// <summary>
        /// 该交易所属的回合
        /// </summary>
        public long RoundId { get; set; }
        /// <summary>
        /// 若经配置，会包含细节约整数据
        /// </summary>
        public string RoundData { get; set; }
        /// <summary>
        /// 该回合的总投注额
        /// </summary>
        public decimal RoundLoss { get; set; }
        /// <summary>
        /// 玩家输的/添加到任何彩池中的金额
        /// </summary>
        public decimal JackpotLoss { get; set; }
        /// <summary>
        /// 玩家获得/从任何彩池中赢得的金额
        /// </summary>
        public decimal JackpotGain { get; set; }
        /// <summary>
        /// 交易的货币
        /// </summary>
        public string Currency { get; set; }
        /// <summary>
        /// 外部交易 id
        /// </summary>
        public string ExternalTransactionId { get; set; }
        /// <summary>
        /// 交易完成之后的用户余额
        /// </summary>
        public decimal Balance { get; set; }
        /// <summary>
        /// 已玩的回合数
        /// </summary>
        public int NumRounds { get; set; }
        /// <summary>
        /// 游戏会话中的总投注额，包括任何从免费投注中的投注。
        /// </summary>
        public decimal TotalLoss { get; set; }
        /// <summary>
        /// 游戏会话中赢取的金额，包括任何从免费游戏中的赢取金额。
        /// </summary>
        public decimal TotalGain { get; set; }
        /// <summary>
        /// 外部免费游戏 id，若交易是为了一个免费游戏。
        /// </summary>
        public string ExternalFreegameId { get; set; }
    }

    /// <summary>
    /// 交易释放关闭 （MessageType: 5）
    /// </summary>
    public class PNGTransactionReleaseClosed : PNGEndpointBaseModel
    {

        /// <summary>
        /// 交易 id
        /// </summary>
        public long TransactionId { get; set; }
        /// <summary>
        /// 交易的 UTC 时间
        /// </summary>
        public string Time { get; set; }
        /// <summary>
        /// 产品组
        /// </summary>
        public int ProductGroup { get; set; }
        /// <summary>
        /// 用户外部 id
        /// </summary>
        public string ExternalUserId { get; set; }
        /// <summary>
        /// gamesession 的 Id
        /// </summary>
        public int GamesessionId { get; set; }
        /// <summary>
        /// 游戏的 Id
        /// </summary>
        public int GameId { get; set; }
        /// <summary>
        /// 交易的货币
        /// </summary>
        public string Currency { get; set; }
        /// <summary>
        /// 外部交易 id
        /// </summary>
        public string ExternalTransactionId { get; set; }
        /// <summary>
        /// 用户余额
        /// </summary>
        public decimal Balance { get; set; }
        /// <summary>
        /// 已玩的回合数
        /// </summary>
        public int NumRounds { get; set; }
        /// <summary>
        /// 游戏会话中的投注额，包括任何从免费游戏中的投注额
        /// </summary>
        public decimal TotalLoss { get; set; }
        /// <summary>
        /// 游戏会话中的赢取金额，包括任何从免费游戏中的赢取金额
        /// </summary>
        public decimal TotalGain { get; set; }
        /// <summary>
        /// gamession 开始的时间
        /// </summary>
        public string GamesessionStarted { get; set; }
        /// <summary>
        /// gamesession 结束的时间
        /// </summary>
        public string GamesessionFinished { get; set; }
        /// <summary>
        /// 使用的汇率
        /// </summary>
        public decimal ExchangeRate { get; set; }
        /// <summary>
        /// 当开始游戏时指定的上下文 id
        /// </summary>
        public int ContextId { get; set; }
        /// <summary>
        /// 所连接的客户端 IP
        /// </summary>
        public string ClientIP { get; set; }
        /// <summary>
        /// 发送到免费优惠 API 中的外部免费游戏 id，或者如果使用 GMT 则为内部优惠 id
        /// </summary>
        public int ExternalFreegameId { get; set; }
        /// <summary>
        /// 玩家输的/添加到任何彩池的金额
        /// </summary>
        public decimal JackpotLoss { get; set; }
        /// <summary>
        /// 玩家获取/从任何彩池中赢得的金额
        /// </summary>
        public decimal JackpotGain { get; set; }
        /// <summary>
        /// 已玩的免费游戏数
        /// </summary>
        public int FreegameRounds { get; set; }
        /// <summary>
        /// 免费游戏的总投注额
        /// </summary>
        public decimal FreegameBet { get; set; }
        /// <summary>
        /// 免费游戏期间的总获胜金额
        /// </summary>
        public decimal FreegameWin { get; set; }
    }

    /// <summary>
    /// 彩池释放（MessageType: 6）
    /// </summary>
    public class PNGJackpotRelease: PNGEndpointBaseModel
    {

        /// <summary>
        /// 用户外部 id
        /// </summary>
        public string ExternalUserId { get; set; }
        /// <summary>
        /// 产品组
        /// </summary>
        public int ProductGroup { get; set; }
        /// <summary>
        /// 赢得彩池的时间
        /// </summary>
        public string Time { get; set; }
        /// <summary>
        /// 赢得彩池的 Id
        /// </summary>
        public int JackpotId { get; set; }
        /// <summary>
        /// 赢得金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 交易货币
        /// </summary>
        public string Currency { get; set; }
        /// <summary>
        /// 游戏的 Id
        /// </summary>
        public int GameId { get; set; }
        /// <summary>
        /// 赢得彩池所在的 Gamesession
        /// </summary>
        public int GamesessionId { get; set; }
    }

    /// <summary>
    /// 免费游戏结束（MessageType: 7）
    /// </summary>
    public class PNGFreegameEnd: PNGEndpointBaseModel
    {

        /// <summary>
        /// 产品组
        /// </summary>
        public int ProductGroup { get; set; }
        /// <summary>
        /// 游戏的 Id
        /// </summary>
        public int GameId { get; set; }
        /// <summary>
        /// 玩家外部 id
        /// </summary>
        public string ExternalUserId { get; set; }
        /// <summary>
        /// 免费游戏优惠所包含的回合数
        /// </summary>
        public int Rounds { get; set; }
        /// <summary>
        /// 免费游戏优惠所包含的线数
        /// </summary>
        public int Lines { get; set; }
        /// <summary>
        /// 免费游戏所包含的币数
        /// </summary>
        public int Coins { get; set; }
        /// <summary>
        /// 免费游戏优惠的面值
        /// </summary>
        public decimal Denomination { get; set; }
        /// <summary>
        /// 与优惠相关的收益
        /// </summary>
        public int Turnover { get; set; }
        /// <summary>
        /// 免费游戏创建的时间
        /// </summary>
        public string Created { get; set; }
        /// <summary>
        /// 免费游戏结束的时间
        /// </summary>
        public string Finished { get; set; }
        /// <summary>
        /// 已使用的回合数
        /// </summary>
        public int RoundsUsed { get; set; }
        /// <summary>
        /// 总获胜金额
        /// </summary>
        public decimal Win { get; set; }
        /// <summary>
        /// 外部免费游戏 id
        /// </summary>
        public int ExternalFreegameId { get; set; }
        /// <summary>
        /// 免费游戏名称
        /// </summary>
        public string Name { get; set; }
    }


    #endregion

    #region 58Poker游戏MODEL


    /// <summary>
    /// 58Poker返回基础model
    /// </summary>
    public class PokerRetBase
    {
        /// <summary>
        /// 对应code值 （200：成功）
        /// </summary>
        public string code { get; set; } = "";
        /// <summary>
        /// 提示语
        /// </summary>
        public string msg { get; set; } = "";
        /// <summary>
        /// 时间搓
        /// </summary>
        public string time { get; set; } = "";
    }

    #region 58Poker登录model

    /// <summary>
    /// 58Poker验证数据
    /// </summary>
    public class PokerToken
    {
        /// <summary>
        /// token值
        /// </summary>
        public string access_token { get; set; } = "";
        /// <summary>
        /// token类型
        /// </summary>
        public string token_type { get; set; } = "";
        /// <summary>
        /// token过期时间
        /// </summary>
        public string expires_in { get; set; } = "";
    }
    /// <summary>
    /// 
    /// </summary>
    public class PokerLoginModel : PokerRetBase
    {
        /// <summary>
        /// json数据
        /// </summary>
        public PokerToken data { get; set; } = new PokerToken();

    }

    #endregion

    #region 58Poker新增model

    /// <summary>
    /// 注册
    /// </summary>
    public class PokerPalyesModel : PokerRetBase
    {

        /// <summary>
        /// json数据
        /// </summary>
        public Palyes58 data { get; set; } = new Palyes58();
    }

    /// <summary>
    /// 玩家信息
    /// </summary>
    public class Palyes58
    {
        /// <summary>
        /// 帐号
        /// </summary>
        public string account { get; set; } = "";
        /// <summary>
        /// 玩家点数
        /// </summary>
        public string balance { get; set; } = "";
        /// <summary>
        /// 电话
        /// </summary>
        public string phone { get; set; } = "";
        /// <summary>
        /// 昵称
        /// </summary>
        public string nickname { get; set; } = "";
        /// <summary>
        /// 大头贴
        /// </summary>
        public string head_url { get; set; } = "";
        /// <summary>
        /// 性别
        /// </summary>
        public string gender { get; set; } = "";

    }

    #endregion

    #region 58Poker储值/提现model

    /// <summary>
    /// 存储/提现
    /// </summary>
    public class PokerDepositModel : PokerRetBase
    {

        /// <summary>
        /// json数据
        /// </summary>
        public Deposit58 data { get; set; } = new Deposit58();
    }

    /// <summary>
    /// 存储返回参数
    /// </summary>
    public class Deposit58
    {

        /// <summary>
        /// 储值单号
        /// </summary>
        public string id { get; set; } = "";
        /// <summary>
        /// 玩家ip位址
        /// </summary>
        public string ip { get; set; } = "";
        /// <summary>
        /// 原始点数
        /// </summary>
        public string before { get; set; } = "";
        /// <summary>
        /// 变化后点数
        /// </summary>
        public string after { get; set; } = "";
        /// <summary>
        /// 创立时间
        /// </summary>
        public string create_at { get; set; } = "";
        /// <summary>
        /// 储值点数
        /// </summary>
        public string amount { get; set; } = "";
        /// <summary>
        /// 备注
        /// </summary>
        public string note { get; set; } = "";
    }

    #endregion

    #region 58Poker余额model

    /// <summary>
    /// 余额
    /// </summary>
    public class PokerBalanceModel : PokerRetBase
    {

        /// <summary>
        /// json数据
        /// </summary>
        public Balance58 data { get; set; } = new Balance58();
    }

    /// <summary>
    /// 余额
    /// </summary>
    public class Balance58
    {
        /// <summary>
        /// 余额值
        /// </summary>
        public string balance { get; set; } = "0";
    }
    #endregion

    #region 58Poker下注model

    /// <summary>
    /// 下注
    /// </summary>
    public class PokerOperatorsModel : PokerRetBase
    {

        /// <summary>
        /// json数据
        /// </summary>
        public Operators58 data { get; set; } = new Operators58();
    }

    public class Operators58
    {
        public statistics statistics { get; set; }
        public List<bet_results> bet_results { get; set; } = new List<bet_results>();
        public int count { get; set; }
    }

    /// <summary>
    /// 下注记录
    /// </summary>
    public class statistics
    {
        /// <summary>
        /// 余额值
        /// </summary>
        public string valid_amount { get; set; } = "0";
        /// <summary>
        /// 余额值
        /// </summary>
        public string bet_amount { get; set; } = "0";
        /// <summary>
        /// 余额值
        /// </summary>
        public string win_bet_result { get; set; } = "0";
        /// <summary>
        /// 余额值
        /// </summary>
        public string lose_bet_result { get; set; } = "0";
        /// <summary>
        /// 余额值
        /// </summary>
        public string commission { get; set; } = "0";
    }

    public class bet_results
    {

        /// <summary>
        /// 余额值
        /// </summary>
        public string id { get; set; } = "0";
        /// <summary>
        /// 余额值
        /// </summary>
        public string account { get; set; } = "0";
        /// <summary>
        /// 余额值
        /// </summary>
        public string table_id { get; set; } = "0";
        /// <summary>
        /// 余额值
        /// </summary>
        public string game_code { get; set; } = "0";
        /// <summary>
        /// 余额值
        /// </summary>
        public string round { get; set; } = "0";
        /// <summary>
        /// 余额值
        /// </summary>
        public string ante { get; set; } = "0";
        /// <summary>
        /// 余额值
        /// </summary>
        public string level { get; set; } = "0";
        /// <summary>
        /// 余额值
        /// </summary>
        public string valid_amount { get; set; } = "0";
        /// <summary>
        /// 余额值
        /// </summary>
        public string bet_amount { get; set; } = "0";
        /// <summary>
        /// 余额值
        /// </summary>
        public string bet_result { get; set; } = "0";
        /// <summary>
        /// 余额值
        /// </summary>
        public string commission { get; set; } = "0";
        /// <summary>
        /// 余额值
        /// </summary>
        public string resulted_at { get; set; } = "0";
        /// <summary>
        /// 余额值
        /// </summary>
        public string ip { get; set; } = "0";
        /// <summary>
        /// 
        /// </summary>
        public actions actions { get; set; }
    }

    public class actions
    {
        public List<string> hands { get; set; } = new List<string>();
        public string seat_no { get; set; }
    }
    #endregion

    #region 游戏列表model

    /// <summary>
    /// 游戏列表model
    /// </summary>
    public class PokerGameModel : PokerRetBase
    {

        /// <summary>
        /// json数据
        /// </summary>
        public List<GameModel58> data { get; set; } = new List<GameModel58>();
    }

    /// <summary>
    /// 游戏model
    /// </summary>
    public class GameModel58
    {
        /// <summary>
        /// 游戏类型
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 游戏状态 0:关闭 1:开启 2:维护中 3:即将上映
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// 游戏预设手续费比例
        /// </summary>
        public string commission_percent { get; set; }
    }

    #endregion

    #region 游戏登录model

    /// <summary>
    /// 游戏登录model
    /// </summary>
    public class PokerGameLoginModel : PokerRetBase
    {

        /// <summary>
        /// json数据
        /// </summary>
        public GameLogin58 data { get; set; } = new GameLogin58();
    }
    /// <summary>
    /// 
    /// </summary>
    public class GameLogin58
    {
        /// <summary>
        /// 登录地址
        /// </summary>
        public string url { get; set; } = "";
    }

    #endregion

    #endregion

    #region 老虎游戏平台model

    
    /// <summary>
    /// 
    /// </summary>
    public class TigerRetdata
    {
        /// <summary>
        /// 对应code值 （0：成功）
        /// </summary>
        public string errorCode { get; set; } = "";

        /// <summary>
        /// 返回数据（带解密）
        /// </summary>
        public List<Betdata> data { get; set; } = new List<Betdata>();

    }

    /// <summary>
    /// 資料清單， json array
    /// </summary>
    public class Betdata
    {
        /// <summary>
        /// 单号，即游戏局号，为唯一值
        /// </summary>
        public string SerialNumber { get; set; }
        /// <summary>
        /// 游戏类型
        /// </summary>
        public string GameType { get; set; }
        /// <summary>
        /// 记录时间，格式为 RFC3339
        /// </summary>
        public string LogTime { get; set; }
        /// <summary>
        /// 总下注，单位为现金点数
        /// </summary>
        public string BetMoney { get; set; }
        /// <summary>
        /// 总得分=主游戏得分+小游戏得分， 单位为现金点数
        /// </summary>
        public string MoneyWin { get; set; }
        /// <summary>
        /// 主游戏得分， 单位为现金点数
        /// </summary>
        public string NormalWin { get; set; }
        /// <summary>
        /// 小游戏得分， 单位为现金点数
        /// </summary>
        public string BonusWin { get; set; }
        /// <summary>
        /// Jp 得分， 单位为现金点数
        /// </summary>
        public string JackpotMoney { get; set; }
        /// <summary>
        /// 平台端帐号
        /// </summary>
        public string ThirdPartyAccount { get; set; }
    }
    #endregion

    #region HGDL彩票游戏

    /// <summary>
    /// 押注紀錄
    /// </summary>
    public class HGDLBetModel
    {
        /// <summary>
        /// betList unique id
        /// </summary>
        public string betListID { get; set; }
        /// <summary>
        /// 押注單狀狀態 1->未開獎 2->已結算 3->和局 4->刪除
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// 遊戲代號
        /// </summary>
        public string gameMainTypeID { get; set; }
        /// <summary>
        /// 遊戲項目代號
        /// </summary>
        public string gameSubTypeID { get; set; }
        /// <summary>
        /// 會員押注金額
        /// </summary>
        public string userBetMoney { get; set; }
        /// <summary>
        /// 會員 unique id
        /// </summary>
        public string userID { get; set; }
        /// <summary>
        /// 帳務⽇日
        /// </summary>
        public string billingDate { get; set; }
        /// <summary>
        /// 會員押注⾦額
        /// </summary>
        public string insertDatetime { get; set; }
        /// <summary>
        /// 修改日期時間
        /// </summary>
        public string motifyDatetime { get; set; }
        /// <summary>
        /// 會員帳號
        /// </summary>
        public string userAccount { get; set; }
        /// <summary>
        /// 遊戲押注期號
        /// </summary>
        public string pissue { get; set; }
        /// <summary>
        /// 押注盤⼝
        /// </summary>
        public string playType { get; set; }
        /// <summary>
        /// 會員押注值
        /// </summary>
        public string betValue { get; set; }
        /// <summary>
        /// 會員押注率
        /// </summary>
        public string betRate { get; set; }
        /// <summary>
        ///  盈虧⾦金金額
        /// </summary>
        public string resultMoney { get; set; }
        /// <summary>
        /// 盈虧时间
        /// </summary>
        public string resultDatetime { get; set; }
        /// <summary>
        /// 交易IP地址
        /// </summary>
        public string ip { get; set; }
    }

    #endregion

    #endregion

    #region 支付Model


    /// <summary>
    /// Z渠道支付
    /// </summary>
    public class ZPayModel
    {
        /// <summary>
        /// alipay: 支付宝；alipay_sm: 支付宝扫码；wxpay: 微信支付；wxpay_sm: 微信扫码；qqpay: QQ钱包；jdpay: 京东钱包；uppay：银联云闪付；quick：银联快捷
        /// </summary>
        public string PayType = "";
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId = "";
        /// <summary>
        /// 金额
        /// </summary>
        public string Amount = "";
    }


    #endregion
}