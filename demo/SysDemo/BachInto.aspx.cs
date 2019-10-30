using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;

namespace demo.SysDemo
{
    public partial class BachInto
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Session["st"] == null)
            //{
            //    Response.Write("<script>window.parent.parent.location.href='../Login.aspx';</script>");
            //    Response.End();
            //}
            //if (!IsPostBack)
            //{
            //    message.InnerHtml = "导入收费的Excel表格必须和批量未收导出的格式一致！";
            //    T_TableStaff ts = Session["st"] as T_TableStaff;
            //    hfcss.Value = ts.F_fontSize;

            //    int no = new LogBLL().GetRecordCount("F_CompanyId=" + ts.F_CompanyId + " and F_DealStfId=" + ts.F_TableStaffId, "T_RcvblFlowTemp");
            //    if (no > 0)
            //    {
            //        step01.Visible = false;
            //        step02.Visible = true;
            //    }
            //    else
            //    {
            //        step01.Visible = true;
            //        step02.Visible = false;
            //    }
            //}
        }

        #region 导入临时数据

        public DataSet leDs(string filenameurl, string table, string sheetname)
        {
            DataSet ds = new DataSet();
            string strConn = "Provider=Microsoft.Jet.OleDb.4.0;" + "data source=" + filenameurl + ";Extended Properties=\"Excel 8.0; HDR=YES; IMEX=1\""; //此连接可以操纵.xls与.xlsx文件 （支撑Excel2003 和 Excel2007 的连接字符串）
            //备注： "HDR=yes;"是说Excel文件的第一行是列名而不是数据，"HDR=No;"正好与前面的相反。//      "IMEX=1 "若是列中的数据类型不一致，应用"IMEX=1"可必免数据类型冲突。 
            OleDbConnection conn = new OleDbConnection(strConn);
            try
            {
                //string strConn = "Provider=Microsoft.Jet.OleDb.4.0;" + "data source=" + filenameurl + ";Extended Properties=""Excel 8.0; HDR=YES; IMEX=1""";
                conn.Open();
                OleDbDataAdapter odda = new OleDbDataAdapter(string.Format(" select * from [{0}$]", sheetname), conn);
                odda.Fill(ds, table);
                return ds;
            }
            catch
            {
                return ds;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// 导入数据
        /// </summary>
        protected void btnInto_Click(object sender, EventArgs e)
        {
            bool tjjg = false;
            //msg.InnerHtml = "";
            //            if (Session["st"] != null)
            //            {
            //                T_TableStaff ts = Session["st"] as T_TableStaff;
            //                if (ts.F_RoleName != "超级管理员")
            //                {
            //                    //判断是否有权限删除
            //                    string sql = " F_TableStaffId=" + ts.F_TableStaffId + " and F_OperatId=15 and F_CompanyId=" + ts.F_CompanyId;
            //                    //获取此用户使用权限
            //                    if (new TableStaffBLL().GetBtMode(sql) == 0)
            //                    {
            //                        MessageBox("对不起，你没有操作权限");
            //                        return;
            //                    }
            //                }
            //                string geturl = HiddenField1.Value.ToString();// hid_hidpicpath.Value.ToString();//获取上传文件的全路径
            //                if (string.IsNullOrEmpty(geturl) && FileUpload1.HasFile)
            //                {
            //                    msg.InnerHtml = "请选择文件!";
            //                }
            //                else
            //                {
            //                    #region 有文件的时候
            //                    // Response.Write("目前获取的上传文件的全路径为：\n\t" + geturl);
            //                    string fileurl = geturl;// HttpContext.Current.Server.MapPath("~/upload/");//文件路径
            //                    string filename = DateTime.Now.ToShortDateString().Replace("-", "").Replace("//", "") + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();//.ToLongDateString() + DateTime.Now.ToLongTimeString();//FileUpload1.PostedFile.FileName.ToString();//文件名
            //                    DataSet out_Ds = new DataSet();
            //                    if (File.Exists(fileurl))
            //                    {
            //                        PutInDataSets obj = new PutInDataSets(fileurl, filename, PutInDataSets.TableType.XLS);//调用PutInDataSet类
            //                        if (!string.IsNullOrEmpty(HiddenFieldWorkBook.Value) && Convert.ToString(HiddenFieldWorkBook.Value.ToString()).Trim() != "--请选择--")
            //                        {
            //                            try
            //                            {
            //                                out_Ds = leDs(fileurl, filename, Convert.ToString(HiddenFieldWorkBook.Value.ToString()).Trim());
            //                                #region 对每条记录进行修改
            //                                //对每条记录进行修改
            //                                Water.BLL.UserStfBLL userstfbll = new Water.BLL.UserStfBLL();
            //                                if (out_Ds != null && out_Ds.Tables[0].Rows.Count > 0)
            //                                {
            //                                    int errorcount = 0;
            //                                    StringBuilder sberror = new StringBuilder();
            //                                    if (!string.IsNullOrEmpty(HiddenFieldWorkBook.Value) && Convert.ToString(HiddenFieldWorkBook.Value).Trim() != "")
            //                                    {
            //                                        string sheet = HiddenFieldWorkBook.Value;// ddlway.SelectedValue;
            //                                        if (dldway.SelectedValue == "2")
            //                                        {
            //                                            #region 银行账号方式

            //                                            Session["inputurl"] = HiddenField1.Value;
            //                                            //检查当前批量添加excel模板格式是否符合标准模板格式
            //                                            #region 检查当前批量添加excel模板格式是否符合标准模板格式
            //                                            StringBuilder sbFilds = new StringBuilder();
            //                                            StringBuilder sbFildsError = new StringBuilder();

            //                                            string tmpFilds = "银行账号-户号-实收金额";//定义模板Excel标题字段

            //                                            var nowFild = tmpFilds.Split('-');
            //                                            bool goonflag = true;//标示是否继续执行剩余代码
            //                                            for (int j = 0; j < nowFild.Length; j++)
            //                                            {
            //                                                if (!out_Ds.Tables[0].Columns.Contains(nowFild[j]))
            //                                                {
            //                                                    goonflag = false;
            //                                                    sbFildsError.Append(string.Format("\n当前选择的Excel文件中不包含【{0}】字段！请仔细检查！", nowFild[j]));
            //                                                }
            //                                            }
            //                                            #endregion
            //                                            //判断当前批量添加的Excel文件中是否也包含这些标题
            //                                            #region 判断是否继续执行
            //                                            string tmpstrGet = "";//临时字符串（获取）

            //                                            if (goonflag == true)
            //                                            {
            //                                                List<T_RcvblFlowTemp> list = new List<T_RcvblFlowTemp>();
            //                                                LogBLL logbll = new LogBLL();
            //                                                DataTable dd = logbll.GetWaterRate("F_CompanyId=" + ts.F_CompanyId, "", "", "T_RcvblFlowTemp");
            //                                                var listtemp = ListDtDeal.DataTableToList<T_RcvblFlowTemp>(dd);
            //                                                string LogName = DateTime.Now.ToString("yyyyMMddHHmmss");
            //                                                string input = "\r\n";
            //                                                input += DateTime.Now.ToString("批量收费|yyyy/MM/dd HH:mm:ss") + "|修改方式:银行账号导入收费|总数量:" + out_Ds.Tables[0].Rows.Count + "\r\n\r\n";
            //                                                #region 组装标题栏
            //                                                for (int j = 0; j < out_Ds.Tables[0].Columns.Count; j++)
            //                                                {
            //                                                    input += out_Ds.Tables[0].Columns[j] + "\r\t";
            //                                                }
            //                                                input += "\r\n";
            //                                                #endregion

            //                                                for (int i = 0; i < out_Ds.Tables[0].Rows.Count; i++)
            //                                                {
            //                                                    #region 组装写入的内容
            //                                                    for (int j = 0; j < out_Ds.Tables[0].Columns.Count; j++)
            //                                                    {
            //                                                        input += out_Ds.Tables[0].Rows[i][j] + "\r\t";
            //                                                    }
            //                                                    input += "\r\n";
            //                                                    #endregion
            //                                                    decimal truemonery = 0;//实收金额
            //                                                    #region 循环体
            //                                                    string F_BankNum = out_Ds.Tables[0].Rows[i]["银行账号"].ToString();
            //                                                    string F_Userid = out_Ds.Tables[0].Rows[i]["户号"].ToString();
            //                                                    if (string.IsNullOrEmpty(F_Userid) || F_Userid == "")
            //                                                    {
            //                                                        errorcount++;
            //                                                        sberror.Append("第" + (i + 1) + "条数据错误!户号数据格式错误！<br/>");
            //                                                        //该用户户号的数据格式错误
            //                                                        continue;
            //                                                    }
            //                                                    if (F_BankNum != "")   //获取磁卡本来的数据
            //                                                    {
            //                                                        if (F_BankNum.Contains("'"))  //包含
            //                                                        {
            //                                                            F_BankNum = F_BankNum.Substring(1, F_BankNum.Length - 1);
            //                                                        }
            //                                                    }
            //                                                    else
            //                                                    {
            //                                                        errorcount++;
            //                                                        sberror.Append("户号为(" + F_Userid + ")的数据错误!银行账号不能为空！<br/>");
            //                                                        //该用户户号的数据格式错误
            //                                                        continue;
            //                                                    }
            //                                                    var temp = listtemp.Where(s => s.F_UserId == F_Userid).ToList();
            //                                                    if (temp != null && temp.Count > 0)
            //                                                    {
            //                                                        errorcount++;
            //                                                        sberror.Append("户号为(" + F_Userid + ")的用户，已经导入数据！<br/>");
            //                                                        continue;
            //                                                    }
            //                                                    tmpstrGet = out_Ds.Tables[0].Rows[i]["实收金额"].ToString();
            //                                                    decimal m = 0;
            //                                                    if (decimal.TryParse(tmpstrGet, out m) && decimal.Parse(tmpstrGet) > 0)
            //                                                    {
            //                                                        truemonery = decimal.Parse(tmpstrGet);//实收金额
            //                                                    }
            //                                                    else
            //                                                    {
            //                                                        continue;   //为0的时候继续下一个循环
            //                                                    }

            //                                                    //添加临时数据
            //                                                    list.Add(new T_RcvblFlowTemp()
            //                                                    {
            //                                                        F_TempID = GetTimestamp(),
            //                                                        F_CompanyId = ts.F_CompanyId,
            //                                                        F_UserId = F_Userid,
            //                                                        F_UserName = "",
            //                                                        F_CardNo = "",
            //                                                        F_BankAccount = "",
            //                                                        F_Address = "",
            //                                                        F_MobilePhone = "",
            //                                                        F_Guilei = "",
            //                                                        F_TableName = "",
            //                                                        F_ManName = "",
            //                                                        F_HallName = "",
            //                                                        F_StreetName = "",
            //                                                        F_BankUserName = "",
            //                                                        F_BankNum = F_BankNum,
            //                                                        F_TotalBalance = 0,
            //                                                        F_RcvblMoney = 0,
            //                                                        F_RcvblPenalty = 0,
            //                                                        F_TotalMoney = 0,
            //                                                        F_RcvedMoney = truemonery,
            //                                                        F_DealStfId = ts.F_TableStaffId,
            //                                                        F_DealStfName = ts.F_TableStaffName,
            //                                                        F_DealDate = DateTime.Now,
            //                                                        F_IntoType = 2,
            //                                                        F_IsDeal = 0

            //                                                    });

            //                                                    #endregion
            //                                                }

            //                                                if (list != null && list.Count > 0)
            //                                                {
            //                                                    string conn = getconctionString();
            //                                                    DataTable dt = ListDtDeal.ListToDataTable<T_RcvblFlowTemp>(list);
            //                                                    new RcvblFlowBLL().SqlBulkCopyByDatatable(conn, "T_RcvblFlowTemp", dt);
            //                                                    tjjg = true;
            //                                                }
            //                                                //记录日志
            //                                                input += "\r\n==========================================================【导入完成】========================================================\r\n";
            //                                                Water.Common.Log.WriteLog(input, LogName, ts.F_CompanyId.ToString());


            //                                                #region 页面最后初始化
            //                                                if (Session["inputurl"] != null)
            //                                                {
            //                                                    ddlway1.InnerHtml = WaterCommon.PutOutDataTable.getWorkBookList(Session["inputurl"].ToString());
            //                                                    HiddenField1.Value = Session["inputurl"].ToString();
            //                                                }
            //                                                FileInfo f = new FileInfo(Session["myurl"].ToString());
            //                                                f.IsReadOnly = false;
            //                                                if (File.Exists(Session["myurl"].ToString()))
            //                                                {
            //                                                    File.Delete(Session["myurl"].ToString());
            //                                                }
            //                                                #endregion
            //                                            }
            //                                            else
            //                                            {
            //                                                #region 将不再继续执行判断和显示页面代码，Excel文件格式此处已经不符合模板格式
            //                                                //MessageBox(sbFildsError.ToString());
            //                                                msg.InnerHtml = sbFildsError.ToString();
            //                                                #endregion
            //                                            }
            //                                            #endregion
            //                                            #endregion
            //                                        }
            //                                        else if (dldway.SelectedValue == "3")
            //                                        {
            //                                            #region 户号（明细模式）

            //                                            Session["inputurl"] = HiddenField1.Value;
            //                                            //检查当前批量添加excel模板格式是否符合标准模板格式
            //                                            #region 检查当前批量添加excel模板格式是否符合标准模板格式
            //                                            StringBuilder sbFilds = new StringBuilder();
            //                                            StringBuilder sbFildsError = new StringBuilder();
            //                                            //户号-户名-磁卡号-历史编号-地址-电话-归类-所属人员-所属手册-所属片区-营业厅-开户名称-银行账号-总余额-应收金额-滞纳金-合计金额-欠费数量-实收金额

            //                                            string tmpFilds = "户号-户名-磁卡号-历史编号-地址-电话-归类-所属人员-所属手册-所属片区-营业厅-开户名称-银行账号-起数-止数-用量-抄表时间-总余额-应收金额-滞纳金-合计金额-实收金额";//定义模板Excel标题字段
            //                                            T_Company com = new CompanyBLL().GetById(ts.F_CompanyId);
            //                                            if (com.F_VerSionId == 1)
            //                                            {
            //                                                out_Ds.Tables[0].Columns["违约金"].ColumnName = "滞纳金";
            //                                            }

            //                                            var tmpFildses = tmpFilds.Split('-');
            //                                            var cols = out_Ds.Tables[0].Columns;
            //                                            bool goonflag = true;//标示是否继续执行剩余代码
            //                                            for (int j = 0; j < tmpFildses.Length; j++)
            //                                            {
            //                                                if (!cols.Contains(tmpFildses[j]))
            //                                                {
            //                                                    goonflag = false;
            //                                                    sbFildsError.Append(string.Format("\n当前选择的Excel文件中不包含【{0}】字段！请仔细检查！", tmpFildses[j]));
            //                                                }
            //                                            }
            //                                            #endregion
            //                                            //判断当前批量添加的Excel文件中是否也包含这些标题
            //                                            #region 判断是否继续执行
            //                                            string tmpstrGet = "";//临时字符串（获取）

            //                                            if (goonflag == true)
            //                                            {
            //                                                List<T_RcvblFlowTemp> list = new List<T_RcvblFlowTemp>();
            //                                                LogBLL logbll = new LogBLL();
            //                                                DataTable dd = logbll.GetWaterRate("F_CompanyId=" + ts.F_CompanyId, "", "", "T_RcvblFlowTemp");
            //                                                var listtemp = ListDtDeal.DataTableToList<T_RcvblFlowTemp>(dd);
            //                                                string LogName = DateTime.Now.ToString("yyyyMMddHHmmss");
            //                                                string input = "\r\n";
            //                                                int rightnum = 0;
            //                                                input += DateTime.Now.ToString("批量收费|yyyy/MM/dd HH:mm:ss") + "|修改方式:银行账号导入收费|总数量:" + out_Ds.Tables[0].Rows.Count + "\r\n\r\n";
            //                                                #region 组装标题栏
            //                                                for (int j = 0; j < out_Ds.Tables[0].Columns.Count; j++)
            //                                                {
            //                                                    input += out_Ds.Tables[0].Columns[j] + "\r\t";
            //                                                }
            //                                                input += "\r\n";
            //                                                #endregion

            //                                                for (int i = 0; i < out_Ds.Tables[0].Rows.Count; i++)
            //                                                {
            //                                                    #region 组装写入的内容
            //                                                    for (int j = 0; j < out_Ds.Tables[0].Columns.Count; j++)
            //                                                    {
            //                                                        input += out_Ds.Tables[0].Rows[i][j] + "\r\t";
            //                                                    }
            //                                                    input += "\r\n";
            //                                                    #endregion

            //                                                    #region 循环体
            //                                                    /*
            //户号-户名-磁卡号-历史编号-地址-电话-归类-所属人员-所属手册-所属片区-营业厅-开户名称-银行账号-总余额-应收金额-滞纳金-合计金额-欠费数量-实收金额
            //                                                         */
            //                                                    string UserId = out_Ds.Tables[0].Rows[i]["户号"].ToString();
            //                                                    if (string.IsNullOrEmpty(UserId) || UserId == "")
            //                                                    {
            //                                                        errorcount++;
            //                                                        sberror.Append("第" + (i + 1) + "条数据错误!户号数据格式错误！<br/>");
            //                                                        //该用户户号的数据格式错误
            //                                                        continue;
            //                                                    }
            //                                                    var temp = listtemp.Where(s => s.F_UserId == UserId).ToList();
            //                                                    if (temp != null && temp.Count > 0)
            //                                                    {
            //                                                        errorcount++;
            //                                                        sberror.Append("户号为(" + UserId + ")的用户，已经导入数据！<br/>");
            //                                                        continue;
            //                                                    }

            //                                                    string UserName = out_Ds.Tables[0].Rows[i]["户名"].ToString().Trim();
            //                                                    string CardNo = out_Ds.Tables[0].Rows[i]["磁卡号"].ToString();
            //                                                    string BankAccount = out_Ds.Tables[0].Rows[i]["历史编号"].ToString();
            //                                                    string Address = out_Ds.Tables[0].Rows[i]["地址"].ToString();
            //                                                    string MobilePhone = out_Ds.Tables[0].Rows[i]["电话"].ToString();
            //                                                    string Guilei = out_Ds.Tables[0].Rows[i]["归类"].ToString();
            //                                                    string TableName = out_Ds.Tables[0].Rows[i]["所属人员"].ToString();
            //                                                    string ManName = out_Ds.Tables[0].Rows[i]["所属手册"].ToString();
            //                                                    string HallName = out_Ds.Tables[0].Rows[i]["所属片区"].ToString();
            //                                                    string StreetName = out_Ds.Tables[0].Rows[i]["营业厅"].ToString();
            //                                                    string BankUserName = out_Ds.Tables[0].Rows[i]["开户名称"].ToString();
            //                                                    string BankNum = out_Ds.Tables[0].Rows[i]["银行账号"].ToString();

            //                                                    #region 去单引号
            //                                                    if (BankAccount.Contains("'"))
            //                                                    {
            //                                                        BankAccount = BankAccount.Substring(1, BankAccount.Length - 1);
            //                                                    }
            //                                                    if (Address.Contains("'"))
            //                                                    {
            //                                                        Address = Address.Substring(1, Address.Length - 1);
            //                                                    }
            //                                                    if (MobilePhone.Contains("'"))
            //                                                    {
            //                                                        MobilePhone = MobilePhone.Substring(1, MobilePhone.Length - 1);
            //                                                    }
            //                                                    if (BankNum.Contains("'"))
            //                                                    {
            //                                                        BankNum = BankNum.Substring(1, BankNum.Length - 1);
            //                                                    }
            //                                                    if (CardNo.Contains("'"))
            //                                                    {
            //                                                        CardNo = CardNo.Substring(1, CardNo.Length - 1);
            //                                                    }
            //                                                    if (UserName.Contains("'"))
            //                                                    {
            //                                                        UserName = UserName.Substring(1, UserName.Length - 1);
            //                                                    }
            //                                                    if (ManName.Contains("'"))
            //                                                    {
            //                                                        ManName = ManName.Substring(1, ManName.Length - 1);
            //                                                    }
            //                                                    if (BankUserName.Contains("'"))
            //                                                    {
            //                                                        BankUserName = BankUserName.Substring(1, BankUserName.Length - 1);
            //                                                    }
            //                                                    if (HallName.Contains("'"))
            //                                                    {
            //                                                        HallName = HallName.Substring(1, HallName.Length - 1);
            //                                                    }
            //                                                    #endregion


            //                                                    decimal TotalBalance, RcvblMoney, RcvblPenalty, TotalMoney, RcvedMoney = 0.00M;

            //                                                    tmpstrGet = out_Ds.Tables[0].Rows[i]["总余额"].ToString();
            //                                                    if (!decimal.TryParse(tmpstrGet, out TotalBalance))
            //                                                    {
            //                                                        continue;
            //                                                    }
            //                                                    tmpstrGet = out_Ds.Tables[0].Rows[i]["应收金额"].ToString();
            //                                                    if (!decimal.TryParse(tmpstrGet, out RcvblMoney))
            //                                                    {
            //                                                        continue;
            //                                                    }
            //                                                    tmpstrGet = out_Ds.Tables[0].Rows[i]["滞纳金"].ToString();
            //                                                    if (!decimal.TryParse(tmpstrGet, out RcvblPenalty))
            //                                                    {
            //                                                        continue;
            //                                                    }
            //                                                    tmpstrGet = out_Ds.Tables[0].Rows[i]["合计金额"].ToString();
            //                                                    if (!decimal.TryParse(tmpstrGet, out TotalMoney))
            //                                                    {
            //                                                        continue;
            //                                                    }

            //                                                    #region 实收金额
            //                                                    tmpstrGet = out_Ds.Tables[0].Rows[i]["实收金额"].ToString();
            //                                                    decimal m = 0;
            //                                                    if (decimal.TryParse(tmpstrGet, out m) && decimal.Parse(tmpstrGet) > 0)
            //                                                    {
            //                                                        RcvedMoney = decimal.Parse(tmpstrGet);//实收金额
            //                                                    }
            //                                                    else
            //                                                    {
            //                                                        continue;   //为0的时候继续下一个循环
            //                                                    }
            //                                                    #endregion

            //                                                    var v = list.Where(s => s.F_UserId == UserId).ToList();
            //                                                    if (v != null && v.Count > 0)
            //                                                    {
            //                                                        v.First().F_RcvblMoney += RcvblMoney;
            //                                                        v.First().F_RcvblPenalty += RcvblPenalty;
            //                                                        v.First().F_TotalMoney += TotalMoney;
            //                                                        v.First().F_RcvedMoney += RcvedMoney;
            //                                                    }
            //                                                    else
            //                                                    {
            //                                                        list.Add(new T_RcvblFlowTemp()
            //                                                        {
            //                                                            F_TempID = GetTimestamp(),
            //                                                            F_CompanyId = ts.F_CompanyId,
            //                                                            F_UserId = UserId,
            //                                                            F_UserName = UserName,
            //                                                            F_CardNo = CardNo,
            //                                                            F_BankAccount = BankAccount,
            //                                                            F_Address = Address,
            //                                                            F_MobilePhone = MobilePhone,
            //                                                            F_Guilei = Guilei,
            //                                                            F_TableName = TableName,
            //                                                            F_ManName = ManName,
            //                                                            F_HallName = HallName,
            //                                                            F_StreetName = StreetName,
            //                                                            F_BankUserName = BankUserName,
            //                                                            F_BankNum = BankNum,
            //                                                            F_TotalBalance = TotalBalance,
            //                                                            F_RcvblMoney = RcvblMoney,
            //                                                            F_RcvblPenalty = RcvblPenalty,
            //                                                            F_TotalMoney = TotalMoney,
            //                                                            F_RcvedMoney = RcvedMoney,
            //                                                            F_DealStfId = ts.F_TableStaffId,
            //                                                            F_DealStfName = ts.F_TableStaffName,
            //                                                            F_DealDate = DateTime.Now,
            //                                                            F_IntoType = 3,
            //                                                            F_IsDeal = 0
            //                                                        });
            //                                                    }
            //                                                    rightnum++;

            //                                                    #endregion
            //                                                }

            //                                                if (list != null && list.Count > 0)
            //                                                {
            //                                                    string conn = getconctionString();
            //                                                    DataTable dt = ListDtDeal.ListToDataTable<T_RcvblFlowTemp>(list);
            //                                                    new RcvblFlowBLL().SqlBulkCopyByDatatable(conn, "T_RcvblFlowTemp", dt);
            //                                                    tjjg = true;
            //                                                }
            //                                                //记录日志
            //                                                input += "\r\n==========================================================【导入完成】========================================================\r\n";
            //                                                Water.Common.Log.WriteLog(input, LogName, ts.F_CompanyId.ToString());
            //                                                T_Log log = new T_Log();
            //                                                log.F_TableName = ts.F_TableStaffName;
            //                                                log.F_Userip = Water.Common.Utils.getIp();
            //                                                log.F_TableStaffId = ts.F_TableStaffId;
            //                                                log.F_CompanyId = ts.F_CompanyId;
            //                                                log.F_ParentName = "收费管理";
            //                                                log.F_NowProment = "批量收费";
            //                                                if (string.IsNullOrEmpty(sberror.ToString()) && sberror.ToString() == "")
            //                                                {
            //                                                    msg.InnerHtml = "导入成功！";
            //                                                    log.F_Contents = "批量收费临时数据导入成功";
            //                                                }
            //                                                else
            //                                                {
            //                                                    if (rightnum > 0)
            //                                                    {
            //                                                        msg.InnerHtml = "本次部分导入失败！错误信息如下：\n\t" + sberror.ToString();
            //                                                        log.F_Contents = "部分导入成功";
            //                                                    }
            //                                                    else
            //                                                    {
            //                                                        msg.InnerHtml = "本次导入出现错误！错误信息如下：\n\t" + sberror.ToString();
            //                                                        log.F_Contents = "导入失败";
            //                                                    }
            //                                                    // MessageBox("本次修改出现错误！错误信息如下：\n\t" + sberror.ToString()); 
            //                                                }
            //                                                log.F_Contents += "<a onclick=\"DWLog(" + ts.F_CompanyId + "," + LogName + ")\">详细日志</a>";
            //                                                //Water.Common.Log.WriteLog("【导入收费】执行收费完毕|" + ts.F_CompanyId, "批量收费");
            //                                                logbll.Add(log);
            //                                                #region 页面最后初始化
            //                                                if (Session["inputurl"] != null)
            //                                                {
            //                                                    ddlway1.InnerHtml = WaterCommon.PutOutDataTable.getWorkBookList(Session["inputurl"].ToString());
            //                                                    HiddenField1.Value = Session["inputurl"].ToString();
            //                                                }
            //                                                FileInfo f = new FileInfo(Session["myurl"].ToString());
            //                                                f.IsReadOnly = false;
            //                                                if (File.Exists(Session["myurl"].ToString()))
            //                                                {
            //                                                    File.Delete(Session["myurl"].ToString());
            //                                                }
            //                                                #endregion
            //                                            }
            //                                            else
            //                                            {
            //                                                #region 将不再继续执行判断和显示页面代码，Excel文件格式此处已经不符合模板格式
            //                                                //MessageBox(sbFildsError.ToString());
            //                                                msg.InnerHtml = sbFildsError.ToString();
            //                                                #endregion
            //                                            }
            //                                            #endregion

            //                                            #endregion
            //                                        }
            //                                        else
            //                                        {
            //                                            #region 户号（合并模式）

            //                                            Session["inputurl"] = HiddenField1.Value;
            //                                            //检查当前批量添加excel模板格式是否符合标准模板格式
            //                                            #region 检查当前批量添加excel模板格式是否符合标准模板格式
            //                                            StringBuilder sbFilds = new StringBuilder();
            //                                            StringBuilder sbFildsError = new StringBuilder();
            //                                            //户号-户名-磁卡号-历史编号-地址-电话-归类-所属人员-所属手册-所属片区-营业厅-开户名称-银行账号-总余额-应收金额-滞纳金-合计金额-欠费数量-实收金额

            //                                            string tmpFilds = "户号-户名-磁卡号-历史编号-地址-电话-归类-所属人员-所属手册-所属片区-营业厅-开户名称-银行账号-总余额-应收金额-滞纳金-合计金额-欠费数量-实收金额";//定义模板Excel标题字段
            //                                            T_Company com = new CompanyBLL().GetById(ts.F_CompanyId);
            //                                            if (com.F_VerSionId == 1)
            //                                            {
            //                                                out_Ds.Tables[0].Columns["违约金"].ColumnName = "滞纳金";
            //                                            }

            //                                            var tmpFildses = tmpFilds.Split('-');
            //                                            var cols = out_Ds.Tables[0].Columns;
            //                                            bool goonflag = true;//标示是否继续执行剩余代码
            //                                            for (int j = 0; j < tmpFildses.Length; j++)
            //                                            {
            //                                                if (!cols.Contains(tmpFildses[j]))
            //                                                {
            //                                                    goonflag = false;
            //                                                    sbFildsError.Append(string.Format("\n当前选择的Excel文件中不包含【{0}】字段！请仔细检查！", tmpFildses[j]));
            //                                                }
            //                                            }
            //                                            #endregion
            //                                            //判断当前批量添加的Excel文件中是否也包含这些标题
            //                                            #region 判断是否继续执行
            //                                            string tmpstrGet = "";//临时字符串（获取）

            //                                            if (goonflag == true)
            //                                            {
            //                                                List<T_RcvblFlowTemp> list = new List<T_RcvblFlowTemp>();
            //                                                LogBLL logbll = new LogBLL();
            //                                                DataTable dd = logbll.GetWaterRate("F_CompanyId=" + ts.F_CompanyId, "", "", "T_RcvblFlowTemp");
            //                                                var listtemp = ListDtDeal.DataTableToList<T_RcvblFlowTemp>(dd);
            //                                                string LogName = DateTime.Now.ToString("yyyyMMddHHmmss");
            //                                                string input = "\r\n";
            //                                                int rightnum = 0;
            //                                                input += DateTime.Now.ToString("批量收费|yyyy/MM/dd HH:mm:ss") + "|修改方式:银行账号导入收费|总数量:" + out_Ds.Tables[0].Rows.Count + "\r\n\r\n";
            //                                                #region 组装标题栏
            //                                                for (int j = 0; j < out_Ds.Tables[0].Columns.Count; j++)
            //                                                {
            //                                                    input += out_Ds.Tables[0].Columns[j] + "\r\t";
            //                                                }
            //                                                input += "\r\n";
            //                                                #endregion

            //                                                for (int i = 0; i < out_Ds.Tables[0].Rows.Count; i++)
            //                                                {
            //                                                    #region 组装写入的内容
            //                                                    for (int j = 0; j < out_Ds.Tables[0].Columns.Count; j++)
            //                                                    {
            //                                                        input += out_Ds.Tables[0].Rows[i][j] + "\r\t";
            //                                                    }
            //                                                    input += "\r\n";
            //                                                    #endregion

            //                                                    #region 循环体
            //                                                    /*
            //户号-户名-磁卡号-历史编号-地址-电话-归类-所属人员-所属手册-所属片区-营业厅-开户名称-银行账号-总余额-应收金额-滞纳金-合计金额-欠费数量-实收金额
            //                                                         */
            //                                                    string UserId = out_Ds.Tables[0].Rows[i]["户号"].ToString();
            //                                                    if (string.IsNullOrEmpty(UserId) || UserId == "")
            //                                                    {
            //                                                        errorcount++;
            //                                                        sberror.Append("第" + (i + 1) + "条数据错误!户号数据格式错误！<br/>");
            //                                                        //该用户户号的数据格式错误
            //                                                        continue;
            //                                                    }
            //                                                    var temp = listtemp.Where(s => s.F_UserId == UserId).ToList();
            //                                                    if (temp != null && temp.Count > 0)
            //                                                    {
            //                                                        errorcount++;
            //                                                        sberror.Append("户号为(" + UserId + ")的用户，已经导入数据！<br/>");
            //                                                        continue;
            //                                                    }

            //                                                    string UserName = out_Ds.Tables[0].Rows[i]["户名"].ToString().Trim();
            //                                                    string CardNo = out_Ds.Tables[0].Rows[i]["磁卡号"].ToString();
            //                                                    string BankAccount = out_Ds.Tables[0].Rows[i]["历史编号"].ToString();
            //                                                    string Address = out_Ds.Tables[0].Rows[i]["地址"].ToString();
            //                                                    string MobilePhone = out_Ds.Tables[0].Rows[i]["电话"].ToString();
            //                                                    string Guilei = out_Ds.Tables[0].Rows[i]["归类"].ToString();
            //                                                    string TableName = out_Ds.Tables[0].Rows[i]["所属人员"].ToString();
            //                                                    string ManName = out_Ds.Tables[0].Rows[i]["所属手册"].ToString();
            //                                                    string HallName = out_Ds.Tables[0].Rows[i]["所属片区"].ToString();
            //                                                    string StreetName = out_Ds.Tables[0].Rows[i]["营业厅"].ToString();
            //                                                    string BankUserName = out_Ds.Tables[0].Rows[i]["开户名称"].ToString();
            //                                                    string BankNum = out_Ds.Tables[0].Rows[i]["银行账号"].ToString();

            //                                                    #region 去单引号
            //                                                    if (BankAccount.Contains("'"))
            //                                                    {
            //                                                        BankAccount = BankAccount.Substring(1, BankAccount.Length - 1);
            //                                                    }
            //                                                    if (Address.Contains("'"))
            //                                                    {
            //                                                        Address = Address.Substring(1, Address.Length - 1);
            //                                                    }
            //                                                    if (MobilePhone.Contains("'"))
            //                                                    {
            //                                                        MobilePhone = MobilePhone.Substring(1, MobilePhone.Length - 1);
            //                                                    }
            //                                                    if (BankNum.Contains("'"))
            //                                                    {
            //                                                        BankNum = BankNum.Substring(1, BankNum.Length - 1);
            //                                                    }
            //                                                    if (CardNo.Contains("'"))
            //                                                    {
            //                                                        CardNo = CardNo.Substring(1, CardNo.Length - 1);
            //                                                    }
            //                                                    if (UserName.Contains("'"))
            //                                                    {
            //                                                        UserName = UserName.Substring(1, UserName.Length - 1);
            //                                                    }
            //                                                    if (ManName.Contains("'"))
            //                                                    {
            //                                                        ManName = ManName.Substring(1, ManName.Length - 1);
            //                                                    }
            //                                                    if (BankUserName.Contains("'"))
            //                                                    {
            //                                                        BankUserName = BankUserName.Substring(1, BankUserName.Length - 1);
            //                                                    }
            //                                                    if (HallName.Contains("'"))
            //                                                    {
            //                                                        HallName = HallName.Substring(1, HallName.Length - 1);
            //                                                    }
            //                                                    #endregion


            //                                                    decimal TotalBalance, RcvblMoney, RcvblPenalty, TotalMoney, RcvedMoney = 0.00M;

            //                                                    tmpstrGet = out_Ds.Tables[0].Rows[i]["总余额"].ToString();
            //                                                    if (!decimal.TryParse(tmpstrGet, out TotalBalance))
            //                                                    {
            //                                                        continue;
            //                                                    }
            //                                                    tmpstrGet = out_Ds.Tables[0].Rows[i]["应收金额"].ToString();
            //                                                    if (!decimal.TryParse(tmpstrGet, out RcvblMoney))
            //                                                    {
            //                                                        continue;
            //                                                    }
            //                                                    tmpstrGet = out_Ds.Tables[0].Rows[i]["滞纳金"].ToString();
            //                                                    if (!decimal.TryParse(tmpstrGet, out RcvblPenalty))
            //                                                    {
            //                                                        continue;
            //                                                    }
            //                                                    tmpstrGet = out_Ds.Tables[0].Rows[i]["合计金额"].ToString();
            //                                                    if (!decimal.TryParse(tmpstrGet, out TotalMoney))
            //                                                    {
            //                                                        continue;
            //                                                    }

            //                                                    #region 实收金额
            //                                                    tmpstrGet = out_Ds.Tables[0].Rows[i]["实收金额"].ToString();
            //                                                    decimal m = 0;
            //                                                    if (decimal.TryParse(tmpstrGet, out m) && decimal.Parse(tmpstrGet) > 0)
            //                                                    {
            //                                                        RcvedMoney = decimal.Parse(tmpstrGet);//实收金额
            //                                                    }
            //                                                    else
            //                                                    {
            //                                                        continue;   //为0的时候继续下一个循环
            //                                                    }
            //                                                    #endregion

            //                                                    list.Add(new T_RcvblFlowTemp()
            //                                                    {
            //                                                        F_TempID = GetTimestamp(),
            //                                                        F_CompanyId = ts.F_CompanyId,
            //                                                        F_UserId = UserId,
            //                                                        F_UserName = UserName,
            //                                                        F_CardNo = CardNo,
            //                                                        F_BankAccount = BankAccount,
            //                                                        F_Address = Address,
            //                                                        F_MobilePhone = MobilePhone,
            //                                                        F_Guilei = Guilei,
            //                                                        F_TableName = TableName,
            //                                                        F_ManName = ManName,
            //                                                        F_HallName = HallName,
            //                                                        F_StreetName = StreetName,
            //                                                        F_BankUserName = BankUserName,
            //                                                        F_BankNum = BankNum,
            //                                                        F_TotalBalance = TotalBalance,
            //                                                        F_RcvblMoney = RcvblMoney,
            //                                                        F_RcvblPenalty = RcvblPenalty,
            //                                                        F_TotalMoney = TotalMoney,
            //                                                        F_RcvedMoney = RcvedMoney,
            //                                                        F_DealStfId = ts.F_TableStaffId,
            //                                                        F_DealStfName = ts.F_TableStaffName,
            //                                                        F_DealDate = DateTime.Now,
            //                                                        F_IntoType = 1,
            //                                                        F_IsDeal = 0

            //                                                    });
            //                                                    rightnum++;

            //                                                    #endregion
            //                                                }

            //                                                if (list != null && list.Count > 0)
            //                                                {
            //                                                    string conn = getconctionString();
            //                                                    DataTable dt = ListDtDeal.ListToDataTable<T_RcvblFlowTemp>(list);
            //                                                    new RcvblFlowBLL().SqlBulkCopyByDatatable(conn, "T_RcvblFlowTemp", dt);
            //                                                    tjjg = true;
            //                                                }
            //                                                //记录日志
            //                                                input += "\r\n==========================================================【导入完成】========================================================\r\n";
            //                                                Water.Common.Log.WriteLog(input, LogName, ts.F_CompanyId.ToString());
            //                                                T_Log log = new T_Log();
            //                                                log.F_TableName = ts.F_TableStaffName;
            //                                                log.F_Userip = Water.Common.Utils.getIp();
            //                                                log.F_TableStaffId = ts.F_TableStaffId;
            //                                                log.F_CompanyId = ts.F_CompanyId;
            //                                                log.F_ParentName = "收费管理";
            //                                                log.F_NowProment = "批量收费";
            //                                                if (string.IsNullOrEmpty(sberror.ToString()) && sberror.ToString() == "")
            //                                                {
            //                                                    msg.InnerHtml = "导入成功！";
            //                                                    log.F_Contents = "批量收费临时数据导入成功";
            //                                                }
            //                                                else
            //                                                {
            //                                                    if (rightnum > 0)
            //                                                    {
            //                                                        msg.InnerHtml = "本次部分导入失败！错误信息如下：\n\t" + sberror.ToString();
            //                                                        log.F_Contents = "部分导入成功";
            //                                                    }
            //                                                    else
            //                                                    {
            //                                                        msg.InnerHtml = "本次导入出现错误！错误信息如下：\n\t" + sberror.ToString();
            //                                                        log.F_Contents = "导入失败";
            //                                                    }
            //                                                    // MessageBox("本次修改出现错误！错误信息如下：\n\t" + sberror.ToString()); 
            //                                                }
            //                                                log.F_Contents += "<a onclick=\"DWLog(" + ts.F_CompanyId + "," + LogName + ")\">详细日志</a>";
            //                                                //Water.Common.Log.WriteLog("【导入收费】执行收费完毕|" + ts.F_CompanyId, "批量收费");
            //                                                logbll.Add(log);
            //                                                #region 页面最后初始化
            //                                                if (Session["inputurl"] != null)
            //                                                {
            //                                                    ddlway1.InnerHtml = WaterCommon.PutOutDataTable.getWorkBookList(Session["inputurl"].ToString());
            //                                                    HiddenField1.Value = Session["inputurl"].ToString();
            //                                                }
            //                                                FileInfo f = new FileInfo(Session["myurl"].ToString());
            //                                                f.IsReadOnly = false;
            //                                                if (File.Exists(Session["myurl"].ToString()))
            //                                                {
            //                                                    File.Delete(Session["myurl"].ToString());
            //                                                }
            //                                                #endregion
            //                                            }
            //                                            else
            //                                            {
            //                                                #region 将不再继续执行判断和显示页面代码，Excel文件格式此处已经不符合模板格式
            //                                                //MessageBox(sbFildsError.ToString());
            //                                                msg.InnerHtml = sbFildsError.ToString();
            //                                                #endregion
            //                                            }
            //                                            #endregion

            //                                            #endregion
            //                                        }

            //                                        if (tjjg)
            //                                        {
            //                                            //跳转第二步执行收费
            //                                            step01.Visible = false;
            //                                            step02.Visible = true;
            //                                        }
            //                                        if (string.IsNullOrEmpty(sberror.ToString()) && sberror.ToString() == "")
            //                                        {
            //                                            MessageBox("导入成功！");
            //                                        }
            //                                        else
            //                                        {
            //                                            MessageBox("本次部分导入失败！错误信息如下：\\n\\t" + sberror.Replace("<br/>", "\\n").ToString());
            //                                        }
            //                                    }
            //                                    else
            //                                    {
            //                                        // MessageBox("请选择Excel工作簿！");                        
            //                                        msg.InnerHtml = "请选择Excel工作簿！";
            //                                    }
            //                                }
            //                                else
            //                                {
            //                                    //MessageBox("暂无数据需要处理！");
            //                                    //return;
            //                                    msg.InnerHtml = "暂无数据需要处理！";
            //                                }
            //                                #endregion
            //                            }
            //                            catch (Exception)
            //                            {
            //                                msg.InnerHtml = "选中的Excel工作簿文件打开失败!";
            //                            }
            //                        }
            //                        else
            //                        {
            //                            msg.InnerHtml = "请选中一个有效的Excel工作簿!";
            //                        }

            //                    }
            //                    else
            //                    {
            //                        msg.InnerHtml = "请重新选择一个正确格式的Excel文件！";
            //                    }
            //                    #endregion
            //                }
            //            }
            //            else
            //            {
            //                Response.Write("<script>window.parent.parent.location.href='../Login.aspx';</script>");
            //                Response.End();
            //            }
            //            isgoing.InnerHtml = "";
        }
        //Button8_Click\Button9_Click
        protected void Button8_Click()
        {
            DataTable dt = new DataTable("cart");
            DataColumn dc1 = new DataColumn("prizename", Type.GetType("System.String"));
            DataColumn dc2 = new DataColumn("point", Type.GetType("System.Int16"));
            DataColumn dc3 = new DataColumn("number", Type.GetType("System.Int16"));
            DataColumn dc4 = new DataColumn("totalpoint", Type.GetType("System.Int64"));
            DataColumn dc5 = new DataColumn("prizeid", Type.GetType("System.String"));
            dt.Columns.Add(dc1);
            dt.Columns.Add(dc2);
            dt.Columns.Add(dc3);
            dt.Columns.Add(dc4);
            dt.Columns.Add(dc5);
            for (int i = 0; i < 10; i++)
            {
                DataRow dr = dt.NewRow();
                dr["prizename"] = "娃娃";
                dr["point"] = 10;
                dr["number"] = 1;
                dr["totalpoint"] = 10;
                dr["prizeid"] = "001";
                dt.Rows.Add(dr);
            }
            //Export()
            WaterCommon.PutOutDataTable.PutOutDataTableToExcelUser(dt, "aa");
        }

        protected void Button6_Click(object sender, EventArgs e)
        {
            //#region 代码处理区
            //if (FileUpload1.HasFile)
            //{
            //    string filename = FileUpload1.PostedFile.FileName;
            //    if (filename.ToString().ToLower().Contains("xls"))
            //    {
            //        //文件格式正确
            //        //Response.Write("文件格式正确");
            //        string newfilename = Water.Common.Utils.getOneRandNum();
            //        if (Session["st"] != null)
            //        {
            //            T_TableStaff ts = Session["st"] as T_TableStaff;
            //            string manageid = ts.F_TableStaffId.ToString();
            //            string dirpath = Server.MapPath("../Files/" + manageid + "");
            //            if (Directory.Exists(dirpath))
            //            {

            //            }
            //            else
            //            {
            //                DirectoryInfo dir3 = Directory.CreateDirectory(dirpath);
            //                // Response.Write("目录不存在");
            //            }
            //            //MessageBox("路径：" + dirpath);
            //            Directory.Delete(dirpath, true);
            //            //Response.Write("路径为：" + dirpath);
            //            DirectoryInfo dir = Directory.CreateDirectory(dirpath);
            //            if (dir.Exists)
            //            {
            //                if (!Directory.Exists(dirpath))
            //                {
            //                    DirectoryInfo dir2 = Directory.CreateDirectory(dirpath);
            //                }

            //                FileUpload1.SaveAs(dirpath + "\\" + newfilename + ".xls");
            //                if (File.Exists(dirpath + "\\" + newfilename + ".xls"))
            //                {
            //                    HiddenField1.Value = dirpath + "\\" + newfilename + ".xls";
            //                    ddlway1.InnerHtml = WaterCommon.PutOutDataTable.getWorkBooklist(dirpath + "\\" + newfilename + ".xls");
            //                }
            //                else
            //                {
            //                    // Response.Write("生成临时文件失败");
            //                    MessageBox("生成临时文件失败");
            //                }
            //            }
            //            else
            //            {
            //                MessageBox("生成专用临时文件失败");
            //                // Response.Write("生成专用临时文件失败");
            //            }
            //        }
            //        else
            //        {
            //            Response.Write("<script>window.parent.parent.location.href='../Login.aspx';</script>");
            //            Response.End();
            //        }
            //    }
            //    else
            //    {
            //        //Response.Write("文件格式错误");
            //        MessageBox("文件格式错误");
            //    }
            //}
            //#endregion

            //Chodr
            //  ClientScript.RegisterStartupScript(this.GetType(), "do", "ChooseMssss(Chodr);", true);  //调用JS方法 
        }

        #endregion

        #region 方法
        SqlConnection conn = new SqlConnection(getconctionString());

        private class SendSMSMsg
        {
            public string Phone { get; set; }
            public Double PayCostMoney { get; set; }
            public Double strUserBalance { get; set; }
            public Double thischg { get; set; }
            public string name { get; set; }
            //public T_UserStf stf { get; set; }
            public string ReadingDate { get; set; }
            public string Qi { get; set; }
            public string Zhi { get; set; }
        }

        private class MeterInfo
        {
            public string F_OpenID { get; set; }
            public string F_UserId { get; set; }

            public int F_MembersId { get; set; }
        }
        private class MaxAcctNo
        {
            public string F_StreetId { get; set; }
            public string F_AcctNo { get; set; }
            public string NowAcctNo { get; set; }
        }

        private class PayFlowId
        {
            public int CBDate { get; set; }
            public int rcvid { get; set; }
        }

        private static string getconctionString()
        {
            string _connectionString = "";
            string dbSource = ConfigurationManager.AppSettings["dbSource"].ToString();
            string dbId = ConfigurationManager.AppSettings["dbId"].ToString();
            string dbpass = ConfigurationManager.AppSettings["dbpass"].ToString();
            string dbname = ConfigurationManager.AppSettings["dbname"].ToString();
            _connectionString = "Data Source=" + dbSource + ";User ID=" + dbId + ";Password=" + dbpass + ";Initial Catalog=" + dbname + ";Pooling=false";
            return _connectionString;
        }

        private int UpdateString(string SQLString, SqlTransaction tran)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(SQLString, conn);
                cmd.Transaction = tran;
                int rows = cmd.ExecuteNonQuery();
                return rows;
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 批量添加数据
        /// </summary>
        /// <param name="connectionString">目标连接字符</param>
        /// <param name="TableName">目标表</param>
        /// <param name="dt">源数据</param>
        private void SqlBulkCopyByDatatable(string TableName, DataTable dt)
        {
            //string connectionString = getconctionString();
            //Stopwatch sw = new Stopwatch();
            //sw.Start();
            //using (SqlBulkCopy sqlbulkcopy = new SqlBulkCopy(connectionString, SqlBulkCopyOptions.Default))
            //{
            //    try
            //    {
            //        sqlbulkcopy.DestinationTableName = TableName;
            //        for (int i = 0; i < dt.Columns.Count; i++)
            //        {
            //            sqlbulkcopy.ColumnMappings.Add(dt.Columns[i].ColumnName, dt.Columns[i].ColumnName);
            //        }
            //        sqlbulkcopy.WriteToServer(dt);

            //    }
            //    catch (System.Exception ex)
            //    {
            //        //double d = sw.Elapsed.Seconds;
            //        Water.Common.Log.WriteLog("异常：" + ex.Message + "|connectionString:" + connectionString + "|TableName:" + TableName + "|" + sw.Elapsed.TotalMilliseconds, "批量收费SqlBulkCopy", "0");
            //        throw ex;
            //    }
            //    finally
            //    {
            //        //Water.Common.Log.WriteLog("正常完成|connectionString:" + connectionString + "|TableName:" + TableName + "|" + sw.Elapsed.TotalMilliseconds, "抄表入库SqlBulkCopy");
            //        sw.Stop();
            //    }



            //}
        }
        #endregion

        /// <summary>
        /// 批量收费
        /// </summary>
        protected void Button1_Click(object sender, EventArgs e)
        {
            //    if (Session["st"] == null)
            //    {
            //        Response.Redirect("../Login.aspx");
            //    }

            //    LogBLL logbll = new LogBLL();
            //    T_TableStaff ts = Session["st"] as T_TableStaff;


            //    var dt = logbll.GetWaterRate("F_CompanyId=" + ts.F_CompanyId + " and F_DealStfId=" + ts.F_TableStaffId, "", "", "T_RcvblFlowTemp");
            //    if (dt != null && dt.Rows.Count > 0)
            //    {
            //        RcvblFlowBLL rcvbll = new RcvblFlowBLL();

            //        #region 提取未收数据
            //        T_Company com = new CompanyBLL().GetById(ts.F_CompanyId);
            //        List<T_RcvblFlowTemp> listtemps = ListDtDeal.DataTableToList<T_RcvblFlowTemp>(dt);
            //        var dtuser = logbll.GetWaterRate("F_CompanyId=" + ts.F_CompanyId, "", "", "T_UserStf");
            //        List<T_UserStf> listuser = ListDtDeal.DataTableToList<T_UserStf>(dtuser);
            //        List<T_UserStf> listuser1 = new List<T_UserStf>();
            //        var list = new List<T_RcvblFlow>();
            //        string where = "F_CompanyId=" + ts.F_CompanyId + " and F_Status=1 ";
            //        int beginyear = 2016;
            //        if (com.F_CreateDate.Year > 2016)
            //        {
            //            beginyear = com.F_CreateDate.Year;
            //        }
            //        int year = logbll.GetNowYear();
            //        for (int i = beginyear; i <= year; i++)
            //        {
            //            var table = "T_RcvblFlow";
            //            if (i > 2016) { table = "T_RcvblFlow" + i; }
            //            if (logbll.GetRecordCount(where, table) > 0)
            //            {
            //                var dtrcv1 = logbll.GetWaterRate(where, "", "", table);
            //                var list1 = ListDtDeal.DataTableToList<T_RcvblFlow>(dtrcv1);
            //                list = list.Concat(list1).ToList();
            //            }
            //        }
            //        dt = logbll.GetWaterRate("RF.F_CompanyId=S.F_CompanyId and RF.F_CompanyId=" + ts.F_CompanyId + " and RF.F_StreetId=S.F_StreetId and charindex(S.F_StreetSys+'" + DateTime.Now.ToString("yy") + "',F_AcctNo)=1 group by RF.F_StreetId", "RF.F_StreetId,max(F_AcctNo) F_AcctNo,'' NowAcctNo", "", "T_Street S,T_PayFlow" + DateTime.Now.Year + " RF");
            //        List<MaxAcctNo> listacc = WaterCommon.ListDtDeal.DataTableToList<MaxAcctNo>(dt);
            //        List<MaxAcctNo> listacc1 = new List<MaxAcctNo>();
            //        dt = logbll.GetWaterRate("F_CompanyId=" + ts.F_CompanyId, "", "", "T_Street");
            //        List<T_Street> liststreet = WaterCommon.ListDtDeal.DataTableToList<T_Street>(dt);
            //        List<T_Street> liststreet1 = new List<T_Street>();
            //        var listuserp = new List<T_UserStf>();
            //        var listrcv = new List<T_RcvblFlow>();
            //        T_UserStf stf = null;
            //        string NowAcctNo = "";
            //        string num = "";
            //        int rightnum = 0;//正确的数量
            //        dt = logbll.GetListWeb("M.F_MembersId=U.F_MembersId and F_CompanyId=" + ts.F_CompanyId, "T_MembersInfo M,T_MembersAndUser U", "M.F_MembersId,F_OpenID,F_UserId").Tables[0];
            //        List<MeterInfo> listmem = WaterCommon.ListDtDeal.DataTableToList<MeterInfo>(dt);
            //        List<MeterInfo> listmem1 = new List<MeterInfo>();
            //        T_SysConfig sys = new CompanyBLL().GetAll("F_CompanyId=" + ts.F_CompanyId);
            //        var dtf = logbll.GetWaterRate("F_CompanyId=" + ts.F_CompanyId, "", "", "T_Fines");
            //        List<T_Fines> fines = WaterCommon.ListDtDeal.DataTableToList<T_Fines>(dtf);
            //        var dtuf = logbll.GetWaterRate("F_CompanyId=" + ts.F_CompanyId, "", "", "T_UserFines");
            //        List<T_UserFines> userfines = WaterCommon.ListDtDeal.DataTableToList<T_UserFines>(dtuf);
            //        var dtwt = logbll.GetWaterRate("F_CompanyId=" + ts.F_CompanyId, "", "", "T_WaterType");
            //        List<T_WaterType> listwt = WaterCommon.ListDtDeal.DataTableToList<T_WaterType>(dtwt);

            //        #endregion

            //        #region //需要执行修改的数据
            //        StringBuilder UpdateRcv = new StringBuilder(2000);
            //        StringBuilder UpdateUser = new StringBuilder(2000);
            //        StringBuilder addpayflow = new StringBuilder(2000);
            //        StringBuilder DelRcvblFlowTemp = new StringBuilder(2000);

            //        List<T_UserStf> listuseredit = new List<T_UserStf>();
            //        List<Common.WeiXinTemplateMsg> listwxmsg = new List<Common.WeiXinTemplateMsg>();
            //        Common.WeiXinTemplateMsg wxmsg = null;
            //        List<SendSMSMsg> listsmsmsg = new List<SendSMSMsg>();
            //        SendSMSMsg smsmsg = null;
            //        List<PayFlowId> listpayid = new List<PayFlowId>();
            //        List<T_RcvblFlow> listpay = new List<T_RcvblFlow>();
            //        List<T_KouPre> listkou = new List<T_KouPre>();
            //        #endregion

            //        StringBuilder sberror = new StringBuilder(2000);
            //        var error = 0;
            //        #region 循环处理临时表中的用户
            //        foreach (var temp in listtemps)
            //        {
            //            listuserp = listuser.Where(s => s.F_UserId == temp.F_UserId && s.F_ParentId == 0).ToList();
            //            var uids = listuser.Where(s => s.F_UserId == temp.F_UserId && s.F_ParentId != 0).Select(s => s.F_Id).ToList();
            //            if (listuserp != null && listuserp.Count > 0 && uids != null && uids.Count > 0)
            //            {
            //                listuser1 = listuseredit.Where(u => u.F_Id == listuserp[0].F_Id).ToList();
            //                if (listuser1 != null && listuser1.Count > 0)
            //                {
            //                    stf = listuseredit[0];
            //                    listuseredit.Remove(stf);
            //                }
            //                else
            //                { stf = listuserp.First(); }
            //                liststreet1 = liststreet.Where(o => o.F_StreetId == stf.F_StreetId).ToList();

            //                #region 处理未收数据
            //                if (list != null && list.Count > 0)
            //                {
            //                    listrcv = list.Where(s => uids.Contains(s.F_Id)).ToList();
            //                    if (listrcv != null && listrcv.Count > 0)
            //                    {
            //                        foreach (var rcv in listrcv)
            //                        {
            //                            if (temp.F_RcvedMoney > 0 || (stf.F_Balance + stf.F_PrepayOrther) > 0)
            //                            {
            //                                #region //计算流水编号
            //                                if (listacc != null && listacc.Count > 0)
            //                                {
            //                                    listacc1 = listacc.Where(a => a.F_StreetId == stf.F_StreetId.ToString()).ToList();
            //                                }
            //                                if (listacc1 != null && listacc1.Count > 0)
            //                                {
            //                                    if (listacc1[0].NowAcctNo == "")
            //                                    {
            //                                        num = listacc1[0].F_AcctNo;
            //                                    }
            //                                    else
            //                                    {
            //                                        num = listacc1[0].NowAcctNo;
            //                                    }
            //                                }
            //                                if (num != "")
            //                                {
            //                                    NowAcctNo = liststreet1[0].F_StreetSys + DateTime.Now.ToString("yy") + (Convert.ToDecimal(num.Substring(num.Length - 8, 8)) + 1).ToString().PadLeft(8, '0');
            //                                }
            //                                else   //当前没有用户信息
            //                                {
            //                                    NowAcctNo = liststreet1[0].F_StreetSys + DateTime.Now.ToString("yy") + "00000001";
            //                                }
            //                                if (listacc1 != null && listacc1.Count > 0)
            //                                {
            //                                    listacc1[0].NowAcctNo = NowAcctNo;
            //                                }
            //                                else
            //                                {
            //                                    MaxAcctNo acc = new MaxAcctNo();
            //                                    acc.F_AcctNo = NowAcctNo;
            //                                    acc.NowAcctNo = NowAcctNo;
            //                                    acc.F_StreetId = stf.F_StreetId.ToString();
            //                                    listacc.Add(acc);
            //                                }
            //                                #endregion

            //                                #region 计算滞纳金
            //                                T_Fines fine = null;
            //                                if (fines != null && fines.Count > 0)
            //                                {
            //                                    var fines1 = fines.Where(s => s.F_WaterTypeId.ToString() == rcv.F_MonWaterId).ToList();
            //                                    if (fines1 != null && fines1.Count > 0)
            //                                    {
            //                                        fine = fines1.First();
            //                                    }
            //                                }

            //                                T_UserFines userfine = null;
            //                                if (userfines != null && userfines.Count > 0)
            //                                {
            //                                    var userfines1 = userfines.Where(s => s.F_Id == rcv.F_Id).ToList();
            //                                    if (userfines1 != null && userfines1.Count > 0)
            //                                    {
            //                                        userfine = userfines1.First();
            //                                    }
            //                                }

            //                                rcv.F_RcvblPenalty = rcvbll.GenerateRcvblPenalty(rcv, sys, fine, userfine);
            //                                #endregion

            //                                rcv.F_RcvedMoney = temp.F_RcvedMoney;
            //                                rcv.F_LastChg = stf.F_Balance;
            //                                rcv.F_ThisChg = stf.F_Balance + stf.F_PrepayOrther + temp.F_RcvedMoney - (rcv.F_RcvblMoney + rcv.F_RcvblPenalty);
            //                                temp.F_RcvedMoney = 0;


            //                                UpdateRcv.Append("update " + (rcv.F_CBDate > 2016 ? "T_RcvblFlow" + rcv.F_CBDate : "T_RcvblFlow") + " set F_RcvedAll=" + rcv.F_RcvedMoney + ",F_RcvedMoney=" + rcv.F_RcvedMoney + ",F_RcvblPenalty=" + rcv.F_RcvblPenalty + ",F_Status=2,F_LastChg=" + rcv.F_LastChg + ",F_ThisChg=" + rcv.F_ThisChg + ",F_AcctNo='" + NowAcctNo + "',F_PreMoney=" + stf.F_PrepayOrther + ",F_TimeShou=getdate(),F_ShouName='" + ts.F_TableStaffName + "',F_ShouId=" + ts.F_TableStaffId + ",F_WayId=5,F_Wayname='批量',F_NoteNo='否',F_Piliang=" + rcv.F_RcvedMoney + " where F_CompanyId=" + ts.F_CompanyId + " and F_RcvblId=" + rcv.F_RcvblId + ";");
            //                                if (stf.F_PrepayOrther > 0)
            //                                {
            //                                    //预存结余扣款数据
            //                                    T_KouPre koupre = new T_KouPre();
            //                                    koupre.F_AccNum = rcv.F_AcctNo;
            //                                    koupre.F_CompanyId = stf.F_CompanyId;
            //                                    koupre.F_Id = rcv.F_Id.ToString();
            //                                    koupre.F_Jieyu = "0";
            //                                    koupre.F_koukuan = stf.F_PrepayOrther.ToString();
            //                                    koupre.F_yucun = "0";
            //                                    koupre.F_StfId = "0";
            //                                    koupre.F_StfName = "系统收费";
            //                                    koupre.F_StreetId = stf.F_StreetId.ToString();
            //                                    koupre.F_StreetName = stf.F_StreetName;
            //                                    koupre.F_UserId = stf.F_UserId;
            //                                    koupre.F_UserName = stf.F_UserName;
            //                                    koupre.F_AcctNo = rcv.F_CalcId;
            //                                    koupre.Ststau = 2;
            //                                    koupre.F_Time = DateTime.Now;
            //                                    koupre.F_beiyong = "";
            //                                    listkou.Add(koupre);
            //                                }

            //                                stf.F_Balance = rcv.F_ThisChg;
            //                                stf.F_PrepayOrther = 0;
            //                                listuseredit.Add(stf);

            //                                listpayid.Add(new PayFlowId() { CBDate = rcv.F_CBDate, rcvid = rcv.F_RcvblId });

            //                                rightnum += 1;

            //                                #region 微信消息集合
            //                                if (sys.F_IsSendWeiXin == 1)
            //                                {
            //                                    listmem1 = listmem.Where(u => u.F_UserId == stf.F_UserId).ToList();
            //                                    if (listmem1 != null && listmem1.Count > 0)
            //                                    {
            //                                        for (int j = 0; j < listmem1.Count; j++)
            //                                        {
            //                                            if (!string.IsNullOrEmpty(listmem1[j].F_OpenID))
            //                                            {
            //                                                wxmsg = new Common.WeiXinTemplateMsg();
            //                                                wxmsg.MemberId = listmem1[j].F_MembersId.ToString();
            //                                                wxmsg.openId = listmem1[j].F_OpenID;
            //                                                wxmsg.BusNo = stf.F_CompanyId;
            //                                                wxmsg.UserId = stf.F_UserId;
            //                                                wxmsg.UserName = stf.F_UserName;
            //                                                if (string.IsNullOrEmpty(liststreet1[0].F_StreetTitle))
            //                                                {
            //                                                    wxmsg.CompanyName = com.F_CompanyName;
            //                                                }
            //                                                else
            //                                                {
            //                                                    wxmsg.CompanyName = liststreet1[0].F_StreetTitle;
            //                                                }
            //                                                wxmsg.Time = DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss");
            //                                                wxmsg.SendType = 2;
            //                                                if (sys.F_WaterWay == "300")
            //                                                {
            //                                                    wxmsg.PayUse = "缴纳气费";
            //                                                }
            //                                                else if (sys.F_WaterWay == "200")
            //                                                {
            //                                                    wxmsg.PayUse = "缴纳电费";
            //                                                }
            //                                                else
            //                                                {
            //                                                    wxmsg.PayUse = "缴纳水费";
            //                                                }
            //                                                wxmsg.PayWay = "批量";
            //                                                wxmsg.Money = rcv.F_RcvedMoney.ToString("F2");
            //                                                wxmsg.Money1 = (stf.F_Balance + stf.F_PrepayOrther).ToString("F2");
            //                                                wxmsg.LastNum = "";
            //                                                wxmsg.ThisRead = "";
            //                                                listwxmsg.Add(wxmsg);
            //                                            }
            //                                        }
            //                                    }
            //                                }
            //                                #endregion

            //                                #region 短信推送
            //                                if (sys.F_SendPLMode == 1 && stf.F_MobilePhone.Length == 11)
            //                                {
            //                                    smsmsg = new SendSMSMsg();
            //                                    smsmsg.Phone = stf.F_MobilePhone;
            //                                    smsmsg.PayCostMoney = Convert.ToDouble(rcv.F_RcvedMoney);
            //                                    smsmsg.strUserBalance = Convert.ToDouble(stf.F_Balance + stf.F_PrepayOrther);
            //                                    smsmsg.name = stf.F_UserName;
            //                                    smsmsg.stf = stf;
            //                                    smsmsg.ReadingDate = rcv.F_ReadingDataDate.ToString("yyyy年MM月");
            //                                    smsmsg.Qi = Convert.ToDecimal(rcv.F_Monqi).ToString("F0");
            //                                    smsmsg.Zhi = Convert.ToDecimal(rcv.F_Monzhi).ToString("F0");
            //                                    listsmsmsg.Add(smsmsg);
            //                                }
            //                                #endregion
            //                            }
            //                        }
            //                    }
            //                }
            //                #endregion

            //                #region 处理用户结余负数数据
            //                if (temp.F_RcvedMoney > 0)
            //                {
            //                    if (stf.F_Balance < 0)
            //                    {
            //                        #region 用户结余为负，需要执行冲抵的
            //                        #region //计算流水编号
            //                        if (listacc1 != null && listacc1.Count > 0)
            //                        {
            //                            if (listacc1[0].NowAcctNo == "")
            //                            {
            //                                num = listacc1[0].F_AcctNo;

            //                            }
            //                            else
            //                            {
            //                                num = listacc1[0].NowAcctNo;
            //                            }
            //                        }
            //                        if (num != "")
            //                        {
            //                            NowAcctNo = liststreet1[0].F_StreetSys + DateTime.Now.ToString("yy") + (Convert.ToDecimal(num.Substring(num.Length - 8, 8)) + 1).ToString().PadLeft(8, '0');
            //                        }
            //                        else   //当前没有用户信息
            //                        {
            //                            NowAcctNo = liststreet1[0].F_StreetSys + DateTime.Now.ToString("yy") + "00000001";
            //                        }
            //                        if (listacc1 != null && listacc1.Count > 0)
            //                        {
            //                            listacc1[0].NowAcctNo = NowAcctNo;
            //                        }
            //                        else
            //                        {
            //                            MaxAcctNo acc = new MaxAcctNo();
            //                            acc.F_AcctNo = NowAcctNo;
            //                            acc.NowAcctNo = NowAcctNo;
            //                            acc.F_StreetId = stf.F_StreetId.ToString();
            //                            listacc.Add(acc);
            //                        }
            //                        #endregion
            //                        var user = listuser.Where(s => s.F_Id == uids.First()).First();
            //                        var water = new T_WaterType();
            //                        if (listwt != null && listwt.Count > 0)
            //                        {
            //                            var wt = listwt.Where(s => s.F_WaterTypeId == user.F_WaterTypeId).ToList();
            //                            if (wt != null && wt.Count > 0)
            //                            {
            //                                water = wt.First();
            //                            }
            //                        }
            //                        #region 冲抵记录数据
            //                        T_RcvblFlow rcv = new T_RcvblFlow();
            //                        rcv.F_CalcId = GetTimestamp();
            //                        rcv.F_Id = user.F_Id;
            //                        rcv.F_CompanyId = ts.F_CompanyId;
            //                        rcv.F_AmtType = 1;
            //                        rcv.F_PayMode = 5;
            //                        rcv.F_WaterPQ = 0;
            //                        rcv.F_RcvblMoney = 0.00M;
            //                        rcv.F_RcvedMoney = temp.F_RcvedMoney;
            //                        rcv.F_Status = 2;
            //                        rcv.F_RcvblWaterPrice = 0.00M;
            //                        rcv.F_RcvblAddMoney = 0.00M;
            //                        rcv.F_RcvblPenalty = 0.00M;
            //                        rcv.F_ReliefMoney = 0.00M;
            //                        rcv.F_LastChg = stf.F_Balance;
            //                        rcv.F_ThisChg = stf.F_Balance + temp.F_RcvedMoney + stf.F_PrepayOrther;
            //                        rcv.F_AcctNo = NowAcctNo;
            //                        rcv.F_NoteNo = "否";
            //                        rcv.F_PreMoney = stf.F_PrepayOrther;
            //                        rcv.F_HomeRmb = 0.00M;
            //                        rcv.F_ClearRmb = 0.00M;
            //                        rcv.F_Mondeparent = user.F_DepartName;
            //                        rcv.F_MonDeparentId = user.F_DepartId.ToString();
            //                        rcv.F_Monwatername = user.F_WaterTypeName;
            //                        rcv.F_MonWaterId = user.F_WaterTypeId.ToString();
            //                        rcv.F_Monqi = user.F_Last.ToString();
            //                        rcv.F_Monzhi = user.F_Last.ToString();
            //                        rcv.F_Monprice = water.F_WaterTypePrice.ToString();
            //                        rcv.F_Monpq = "0.00 ";
            //                        rcv.F_Monjia = "0.00 ";
            //                        rcv.F_Monstfname = user.F_TableName;
            //                        rcv.F_MonstfId = user.F_TableId.ToString();
            //                        rcv.F_MonhallId = user.F_HallId.ToString();
            //                        rcv.F_Monhallname = user.F_HallName;
            //                        rcv.F_Monqianfei = "0";
            //                        rcv.F_MonNameId = user.F_ReadingManualsId.ToString();
            //                        rcv.F_MonNameshou = user.F_ReadmanualName;
            //                        rcv.F_MonUserName = user.F_UserName;
            //                        rcv.F_Xianjin = "0";
            //                        rcv.F_Piliang = temp.F_RcvedMoney.ToString("F2");
            //                        rcv.F_Zhipiao = "0";
            //                        rcv.F_Zhuanzhang = "0";
            //                        rcv.F_Shishou = temp.F_RcvedMoney.ToString("F2"); ;
            //                        rcv.F_Jieyu = "0";
            //                        rcv.F_ShouName = ts.F_TableStaffName;
            //                        rcv.F_ShouId = ts.F_TableStaffId.ToString();
            //                        rcv.F_WayId = "5";
            //                        rcv.F_Wayname = "批量";
            //                        rcv.F_StrName = user.F_StreetName;
            //                        rcv.F_StreetId = user.F_StreetId.ToString();
            //                        rcv.F_GuiId = user.F_GuileiId.ToString();
            //                        rcv.F_GuiName = user.F_GuileiName;
            //                        rcv.F_Shared = user.F_Share.ToString();
            //                        rcv.F_NoStatus = "";
            //                        rcv.F_RcvMupl = user.F_Multiply.ToString();
            //                        rcv.F_RcvShare = user.F_Share.ToString();
            //                        rcv.F_ChaoBiaoRenName = user.F_TableName;
            //                        rcv.F_RcvedAll = temp.F_RcvedMoney;
            //                        rcv.F_AliPay = 0.00M;
            //                        rcv.F_WeiXin = 0.00M;
            //                        rcv.F_Remark = "批量收费冲抵用户结余";
            //                        rcv.F_ReadingDataDate = DateTime.Now;
            //                        rcv.F_PenaltyBeginDate = DateTime.Now;
            //                        rcv.F_TimeShou = DateTime.Now;
            //                        rcv.F_CancelDate = DateTime.Now;
            //                        rcv.F_CBDate = DateTime.Now.Year;
            //                        rcv.F_AddStatus = 2;
            //                        listpay.Add(rcv);
            //                        #endregion


            //                        #region   //预存结余扣款数据
            //                        if (stf.F_PrepayOrther > 0)
            //                        {
            //                            T_KouPre koupre = new T_KouPre();
            //                            koupre.F_AccNum = rcv.F_AcctNo;
            //                            koupre.F_CompanyId = user.F_CompanyId;
            //                            koupre.F_Id = user.F_Id.ToString();
            //                            koupre.F_Jieyu = "0";
            //                            koupre.F_koukuan = stf.F_PrepayOrther.ToString();
            //                            koupre.F_yucun = "0";
            //                            koupre.F_StfId = "0";
            //                            koupre.F_StfName = "系统收费";
            //                            koupre.F_StreetId = user.F_StreetId.ToString();
            //                            koupre.F_StreetName = user.F_StreetName;
            //                            koupre.F_UserId = user.F_UserId;
            //                            koupre.F_UserName = user.F_UserName;
            //                            koupre.F_AcctNo = rcv.F_CalcId;
            //                            koupre.Ststau = 2;
            //                            koupre.F_Time = DateTime.Now;
            //                            koupre.F_beiyong = "";
            //                            listkou.Add(koupre);
            //                        }
            //                        #endregion


            //                        temp.F_RcvedMoney = 0;
            //                        stf.F_Balance = rcv.F_ThisChg;
            //                        stf.F_PrepayOrther = 0;
            //                        listuseredit.Add(stf);
            //                        #endregion
            //                    }
            //                    else
            //                    {
            //                        #region 跳过，提示不存在欠费
            //                        sberror.Append("用户[" + stf.F_UserId + "]" + stf.F_UserName + "不存在欠费，导入金额(" + temp.F_RcvedMoney + ")失败<br/>");
            //                        error++;
            //                        #endregion
            //                    }
            //                }
            //                #endregion
            //            }
            //        }
            //        #endregion

            //        #region 执行处理数据的方法
            //        this.conn.Open();
            //        SqlTransaction tran = this.conn.BeginTransaction(IsolationLevel.ReadUncommitted);
            //        try
            //        {
            //            //修改应收
            //            if (UpdateRcv.Length > 0)
            //            {
            //                UpdateString(UpdateRcv.ToString(), tran);
            //            }
            //            //添加已收
            //            if (listpayid != null && listpayid.Count > 0)
            //            {
            //                string col = rcvbll.GetColumnsName("T_RcvblFlow", " is_identity=0");
            //                var listcbyear = listpayid.Select(s => s.CBDate).Distinct().ToList();
            //                foreach (var cbyear in listcbyear)
            //                {
            //                    var listrcvids = listpayid.Where(s => s.CBDate == cbyear).Select(s => s.rcvid).ToList();
            //                    var rcvids = string.Join(",", listrcvids);
            //                    addpayflow.Append("insert into T_PayFlow" + DateTime.Now.Year + " select " + col + " from " + (cbyear > 2016 ? "T_RcvblFlow" + cbyear : "T_RcvblFlow") + " where F_CompanyId=" + ts.F_CompanyId + " and F_Status=2 and F_RcvblId in (" + rcvids + ");");
            //                }
            //                UpdateString(addpayflow.ToString(), tran);
            //            }
            //            //添加冲抵记录
            //            if (listpay != null && listpay.Count > 0)
            //            {
            //                var dd = ListDtDeal.ListToDataTable<T_RcvblFlow>(listpay);
            //                SqlBulkCopyByDatatable("T_PayFlow" + DateTime.Now.Year, dd);
            //            }
            //            //添加扣款记录
            //            if (listkou != null && listkou.Count > 0)
            //            {
            //                var dd = ListDtDeal.ListToDataTable<T_KouPre>(listkou);
            //                SqlBulkCopyByDatatable("T_KouPre", dd);
            //            }
            //            //修改用户余额
            //            if (listuseredit.Count > 0)
            //            {
            //                foreach (var v in listuseredit)
            //                {
            //                    UpdateUser.Append("update T_UserStf set F_Balance=" + v.F_Balance + ",F_PrepayOrther=" + v.F_PrepayOrther + " where F_CompanyId=" + ts.F_CompanyId + " and F_Id='" + v.F_Id + "';");
            //                }
            //                if (UpdateUser.Length > 0)
            //                {
            //                    UpdateString(UpdateUser.ToString(), tran);
            //                }
            //            }
            //            //删除临时表中的数据
            //            DelRcvblFlowTemp.Append("delete T_RcvblFlowTemp where F_CompanyId=" + ts.F_CompanyId + " and F_DealStfId=" + ts.F_TableStaffId);
            //            if (DelRcvblFlowTemp.Length > 0)
            //            {
            //                UpdateString(DelRcvblFlowTemp.ToString(), tran);
            //            }

            //            tran.Commit();
            //            conn.Close();
            //        }
            //        catch (Exception)
            //        {
            //            tran.Rollback();
            //            conn.Close();
            //            throw;
            //        }
            //        #endregion

            //        #region 微信消息推送
            //        if (sys.F_IsSendWeiXin == 1)
            //        {
            //            if (listwxmsg != null && listwxmsg.Count > 0)
            //            {
            //                foreach (var wmsg in listwxmsg)
            //                {
            //                    WaterWeb.Common.WeiXinHelper.SendWeiXinTemplateMsg(wmsg);
            //                }
            //            }
            //        }
            //        #endregion

            //        #region 短信消息推送
            //        if (sys.F_SendPLMode == 1)//短信收费
            //        {
            //            if (listsmsmsg != null && listsmsmsg.Count > 0)
            //            {
            //                string name = "";
            //                if (Request.Cookies["IpaiClientCompanyName"] != null)
            //                {
            //                    name = HttpUtility.UrlDecode(Request.Cookies["IpaiClientCompanyName"].Value.Trim().ToString());
            //                }
            //                //只要注册企业了就自动添加了一个参数设置
            //                SqlParameter[] parameters = {
            //                new SqlParameter("@companyname", SqlDbType.NVarChar,120)};
            //                parameters[0].Value = name;
            //                string retval = WaterDBUtility.HelperSQL.ReturnValue("IsExistCompanyProcdianzifapiao", parameters, 1).ToString();
            //                string[] arr = retval.Split('★');
            //                if (arr[12].ToString().Trim() != "")
            //                {
            //                    if (Convert.ToInt32(arr[12].ToString()) > 0)
            //                    {
            //                        WaterWeb.SMS_Module.SMS_User sms_user = new WaterWeb.SMS_Module.SMS_User(ts.F_CompanyId);
            //                        int smscount = Convert.ToInt32(arr[12].ToString());

            //                        SqlParameter[] parameters2 = { new SqlParameter("@SMSWay", SqlDbType.Int, 5) };
            //                        parameters2[0].Value = 300;
            //                        string url = WaterDBUtility.HelperSQL.ReturnValue("proc_SMSWay", parameters2, 1).ToString();

            //                        foreach (var v in listsmsmsg)
            //                        {
            //                            if (smscount > 0)
            //                            {
            //                                var bcsl = sms_user.SendPayCostRemind(v.Phone, v.PayCostMoney, v.strUserBalance, v.name, name, sms_user, ts.F_CompanyId, v.stf, url, v.ReadingDate, v.Qi, v.Zhi);
            //                                smscount -= bcsl;
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //        #endregion

            //        #region 记录日志
            //        T_Log log = new T_Log();
            //        log.F_TableName = ts.F_TableStaffName;
            //        log.F_Userip = Water.Common.Utils.getIp();
            //        log.F_TableStaffId = ts.F_TableStaffId;
            //        log.F_CompanyId = ts.F_CompanyId;
            //        log.F_ParentName = "收费管理";
            //        log.F_NowProment = "批量收费";
            //        log.F_Contents = "批量收费【" + listtemps.Count + "】个用户成功";
            //        logbll.Add(log);
            //        #endregion

            //        if (sberror != null && sberror.Length > 0)
            //        {
            //            ShowTips.Visible = true;
            //            ShowCount.InnerHtml = "批量收费成功【" + (listtemps.Count - error) + "】个用户,导入失败【" + error + "】个";
            //            ShowError.InnerHtml = sberror.ToString();
            //            MessageBox("批量收费成功【" + (listtemps.Count - error) + "】个用户,导入失败【" + error + "】个");
            //        }
            //        else
            //        {
            //            step01.Visible = true;
            //            step02.Visible = false;

            //            MessageBox("批量收费【" + listtemps.Count + "】个用户成功");
            //        }
            //    }
            //    else
            //    {
            //        step01.Visible = true;
            //        step02.Visible = false;
            //        MessageBox("没有临时数据需要处理！");
            //    }
            //}
        }
    }
}