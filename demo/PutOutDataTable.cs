/***
 * ==================2013-06-20====================
 * 1.新增加一个将Excel文件导出到DataTable文件中的方法getExcelTable
 * 
 **/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using System.Reflection;
using System.Data;
using Microsoft.Office.Interop.Excel;
using System.IO;
using System.Data.SqlClient;

using System.Runtime.InteropServices;
using System.Data.OleDb;
using System.Net;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace WaterCommon
{
    public class PutOutDataTable
    {

        /// <summary>
        /// 根据文件路径获取工作簿
        /// </summary>
        /// <param name="fileurl"></param>
        /// <returns></returns>
        public static string getWorkBooklist(string fileurl)
        {
            string nkey = "";
            if (!string.IsNullOrEmpty(fileurl) && Convert.ToString(fileurl).ToString().Trim() != "")
            {
                nkey = WaterCommon.PutOutDataTable.getWorkBookList(fileurl);
                HttpContext.Current.Session["bindExcelNameList"] = nkey;
                HttpContext.Current.Session["myurl"] = fileurl;//批量结束后删除上次的临时批量文件
            }
            else
            {
                nkey = "参数错误！";
            }
            return nkey;
        }
       /// <summary>
        /// 得到Excel 文件中某一张工作表的表格(2013-06-20am11:42)
       /// </summary>
        /// <param name="filename">Excel 文件路径</param>
        /// <param name="sheetNum"> 工作表编号 </param>
        /// <returns>所有数据生成的表格</returns>
        public static System.Data.DataTable getExcelTable(string filename, int sheetNum)
        {
            Microsoft.Office.Interop.Excel.Application myExcel = new Application();
            object missing = Missing.Value;
            Workbook myBook = myExcel.Application.Workbooks.Open(filename, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing,missing, missing, missing, missing); //打开Excel 文件 
            if (myBook != null) //打开成功
            {
                myExcel.Visible = false;
                Worksheet mySheet = (Worksheet)myBook.Worksheets[sheetNum]; //得 到工作表 
                System.Data.DataTable dt = new System.Data.DataTable();
                for (int j = 1; j <= mySheet.Cells.CurrentRegion.Columns.Count; j++)
                    dt.Columns.Add();
                for (int i = 1; i <= mySheet.Cells.CurrentRegion.Rows.Count; i++) //把工作表导入DataTable 中
                {
                    DataRow myRow = dt.NewRow();
                    for (int j = 1; j <= mySheet.Cells.CurrentRegion.Columns.Count; j++)
                    {
                        Microsoft.Office.Interop.Excel.Range temp = (Microsoft.Office.Interop.Excel.Range)mySheet.Cells[i, j];
                        string strValue = temp.Text.ToString();
                        myRow[j - 1] = strValue;
                    }
                    dt.Rows.Add(myRow);
                }
                myExcel.Quit(); //退出Excel 文件 
                System.Diagnostics.Process[] myProcesses = System.Diagnostics.Process.GetProcessesByName("EXCEL");
                foreach (System.Diagnostics.Process instance in myProcesses)
                {
                    instance.Kill();
                }
                return dt;
            } //打开不成功 
            return null;
        }
        /// <summary>
        /// 导数数据
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="filename"></param>
        public static void PutOutDataTableToExcelUser(System.Data.DataTable dt, string filename)
        {
            var aa = HttpContext.Current;
            System.Web.HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.Default;//设置编码格式
            System.Web.HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";//设置输入类型为Excel文件，指定返回的是一个不能被客户端读取的流，必须被下载
            System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(filename) + "");//添加Http表头，将文件保存为Test.xls
            string columnHeader = "";//保存表头字符
            string columnContent = "";//保存每行的数据内容
            System.Data.DataTable dsTable = dt;
            for (int i = 0; i < dsTable.Columns.Count; i++)
            {
                if (i == dsTable.Columns.Count - 1)
                    columnHeader += dsTable.Columns[i].Caption.ToString() + "\n";//当当前列为最后一列时要换行
                else
                    columnHeader += dsTable.Columns[i].Caption.ToString() + "\t";
            }
            System.Web.HttpContext.Current.Response.Write(columnHeader);
            //添加每行的数据信息            
            foreach (DataRow dr in dsTable.Rows)
            {
                for (int j = 0; j < dsTable.Columns.Count; j++)
                {
                    if (j == dsTable.Columns.Count - 1)
                        columnContent += dr[j] + "\n";//当当前列为最后一列时换行
                    else
                        columnContent += dr[j] + "\t";
                }
                System.Web.HttpContext.Current.Response.Write(columnContent);
                columnContent = "";
            }
            System.Web.HttpContext.Current.Response.End();
        }
        /// <summary>
        /// 新写的从DataTable中导出XLS数据
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="filename"></param>
        public static void PutOutDataTableToExcel(System.Data.DataTable dt, string filename, string templetname)
        {

            string filePath = HttpContext.Current.Server.MapPath("~/UpLoad/" + Guid.NewGuid().ToString() + ".xls");
            File.Copy(HttpContext.Current.Server.MapPath("~/UpLoad/" + templetname + ""), filePath);
            // 使用OleDb驱动程序连接到副本
            OleDbConnection conn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=YES;IMEX=2'");
            using (conn)
            {
                conn.Open();
                // 增加记录
                string columnHeader = "";//保存表头字符
                string columnContent = "";//保存每行的数据内容
                System.Data.DataTable dsTable = dt;
                string titlestr = "";
                for (int i = 0; i < dsTable.Columns.Count; i++)
                {
                    if (i == dsTable.Columns.Count - 1)
                    {
                        titlestr += "''";
                        columnHeader += dsTable.Columns[i].Caption.ToString();//当当前列为最后一列时要换行
                    }
                    else
                    {
                        titlestr += "''" + ",";
                        columnHeader += dsTable.Columns[i].Caption.ToString() + ",";
                    }
                }
                //OleDbCommand cmd2 = new OleDbCommand("INSERT INTO [Sheet1$](" + titlestr + ") VALUES(" + columnHeader + ")", conn); 
                //cmd2.ExecuteNonQuery();
                //cmd2.Clone();
                //cmd2.Dispose();            
                //System.Web.HttpContext.Current.Response.Write(columnHeader);
                //添加每行的数据信息            
                foreach (DataRow dr in dsTable.Rows)
                {
                    for (int j = 0; j < dsTable.Columns.Count; j++)
                    {
                        if (j == dsTable.Columns.Count - 1)
                            columnContent += "'" + dr[j] + "'";//当当前列为最后一列时换行
                        else
                            columnContent += "'" + dr[j] + "',";
                    }
                    if (File.Exists(filePath))
                    {
                        FileInfo f = new FileInfo(filePath);
                        f.IsReadOnly = false;
                        OleDbCommand cmd = new OleDbCommand("INSERT INTO [Sheet1$](" + columnHeader + ") VALUES(" + columnContent + ")", conn);
                        //System.Web.HttpContext.Current.Response.Write(columnContent);                       
                       
                        cmd.ExecuteNonQuery();
                        columnContent = "";
                        cmd.Clone();
                        cmd.Dispose();
                    }
                }

                //OleDbCommand cmd = new OleDbCommand("INSERT INTO [Sheet1$](" + columnHeader + ") VALUES(" + columnContent + ")", conn);
                //cmd.Parameters.AddWithValue("@Id", "1");
                //cmd.Parameters.AddWithValue("@Name", "Hsu Yencheng");
                //cmd.Parameters.AddWithValue("@Birthday", "1981-10-13");               
            }
            // 输出副本的二进制字节流
            HttpContext.Current.Response.ContentType = "application/ms-excel";
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(filename) + "");
            HttpContext.Current.Response.BinaryWrite(File.ReadAllBytes(filePath));
            // 删除副本
            File.Delete(filePath);
            conn.Close();
            conn.Dispose();
            //System.Web.HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;//设置编码格式
            //System.Web.HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";//设置输入类型为Excel文件，指定返回的是一个不能被客户端读取的流，必须被下载
            //System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(filename) + "");//添加Http表头，将文件保存为Test.xls
            //string columnHeader = "";//保存表头字符
            //string columnContent = "";//保存每行的数据内容
            //System.Data.DataTable dsTable = dt;
            //for (int i = 0; i < dsTable.Columns.Count; i++)
            //{
            //    if (i == dsTable.Columns.Count - 1)
            //        columnHeader += dsTable.Columns[i].Caption.ToString() + "\n";//当当前列为最后一列时要换行
            //    else
            //        columnHeader += dsTable.Columns[i].Caption.ToString() + "\t";
            //}
            //System.Web.HttpContext.Current.Response.Write(columnHeader);
            ////添加每行的数据信息            
            //foreach (DataRow dr in dsTable.Rows)
            //{
            //    for (int j = 0; j < dsTable.Columns.Count; j++)
            //    {
            //        if (j == dsTable.Columns.Count - 1)
            //            columnContent += dr[j] + "\n";//当当前列为最后一列时换行
            //        else
            //            columnContent += dr[j] + "\t";
            //    }
            //    System.Web.HttpContext.Current.Response.Write(columnContent);
            //    columnContent = "";
            //}
            //System.Web.HttpContext.Current.Response.End();
        }
        //        Response.ContentType类型汇总
        //在ASP.NET中使用Response.ContentType="类型名";来确定输出格式 
        //'ez' => 'application/andrew-inset',  
        //'hqx' => 'application/mac-binhex40',  
        //'cpt' => 'application/mac-compactpro',  
        //'doc' => 'application/msword',  
        //'bin' => 'application/octet-stream',  
        //'dms' => 'application/octet-stream',  
        //'lha' => 'application/octet-stream',  
        //'lzh' => 'application/octet-stream',  
        //'exe' => 'application/octet-stream',  
        //'class' => 'application/octet-stream',  
        //'so' => 'application/octet-stream',  
        //'dll' => 'application/octet-stream',  
        //'oda' => 'application/oda',  
        //'pdf' => 'application/pdf',  
        //'ai' => 'application/postscript',  
        //'eps' => 'application/postscript',  
        //'ps' => 'application/postscript',  
        //'smi' => 'application/smil',  
        //'smil' => 'application/smil',  
        //'mif' => 'application/vnd.mif',  
        //'xls' => 'application/vnd.ms-excel',  
        //'ppt' => 'application/vnd.ms-powerpoint',  
        //'wbxml' => 'application/vnd.wap.wbxml',  
        //'wmlc' => 'application/vnd.wap.wmlc',  
        //'wmlsc' => 'application/vnd.wap.wmlscriptc',  
        //'bcpio' => 'application/x-bcpio',  
        //'vcd' => 'application/x-cdlink',  
        //'pgn' => 'application/x-chess-pgn',  
        //'cpio' => 'application/x-cpio',  
        //'csh' => 'application/x-csh',  
        //'dcr' => 'application/x-director',  
        //'dir' => 'application/x-director',  
        //'dxr' => 'application/x-director',  
        //'dvi' => 'application/x-dvi',  
        //'spl' => 'application/x-futuresplash',  
        //'gtar' => 'application/x-gtar',  
        //'hdf' => 'application/x-hdf',  
        //'js' => 'application/x-javascript',  
        //'skp' => 'application/x-koan',  
        //'skd' => 'application/x-koan',  
        //'skt' => 'application/x-koan',  
        //'skm' => 'application/x-koan',  
        //'latex' => 'application/x-latex',  
        //'nc' => 'application/x-netcdf',  
        //'cdf' => 'application/x-netcdf',  
        //'sh' => 'application/x-sh',  
        //'shar' => 'application/x-shar',  
        //'swf' => 'application/x-shockwave-flash',  
        //'sit' => 'application/x-stuffit',  
        //'sv4cpio' => 'application/x-sv4cpio',  
        //'sv4crc' => 'application/x-sv4crc',  
        //'tar' => 'application/x-tar',  
        //'tcl' => 'application/x-tcl',  
        //'tex' => 'application/x-tex',  
        //'texinfo' => 'application/x-texinfo',  
        //'texi' => 'application/x-texinfo',  
        //'t' => 'application/x-troff',  
        //'tr' => 'application/x-troff',  
        //'roff' => 'application/x-troff',  
        //'man' => 'application/x-troff-man',  
        //'me' => 'application/x-troff-me',  
        //'ms' => 'application/x-troff-ms',  
        //'ustar' => 'application/x-ustar',  
        //'src' => 'application/x-wais-source',  
        //'xhtml' => 'application/xhtml+xml',  
        //'xht' => 'application/xhtml+xml',  
        //'zip' => 'application/zip',  
        //'au' => 'audio/basic',  
        //'snd' => 'audio/basic',  
        //'mid' => 'audio/midi',  
        //'midi' => 'audio/midi',  
        //'kar' => 'audio/midi',  
        //'mpga' => 'audio/mpeg',  
        //'mp2' => 'audio/mpeg',  
        //'mp3' => 'audio/mpeg',  
        //'aif' => 'audio/x-aiff',  
        //'aiff' => 'audio/x-aiff',  
        //'aifc' => 'audio/x-aiff',  
        //'m3u' => 'audio/x-mpegurl',  
        //'ram' => 'audio/x-pn-realaudio',  
        //'rm' => 'audio/x-pn-realaudio',  
        //'rpm' => 'audio/x-pn-realaudio-plugin',  
        //'ra' => 'audio/x-realaudio',  
        //'wav' => 'audio/x-wav',  
        //'pdb' => 'chemical/x-pdb',  
        //'xyz' => 'chemical/x-xyz',  
        //'bmp' => 'image/bmp',  
        //'gif' => 'image/gif',  
        //'ief' => 'image/ief',  
        //'jpeg' => 'image/jpeg',  
        //'jpg' => 'image/jpeg',  
        //'jpe' => 'image/jpeg',  
        //'png' => 'image/png',  
        //'tiff' => 'image/tiff',  
        //'tif' => 'image/tiff',  
        //'djvu' => 'image/vnd.djvu',  
        //'djv' => 'image/vnd.djvu',  
        //'wbmp' => 'image/vnd.wap.wbmp',  
        //'ras' => 'image/x-cmu-raster',  
        //'pnm' => 'image/x-portable-anymap',  
        //'pbm' => 'image/x-portable-bitmap',  
        //'pgm' => 'image/x-portable-graymap',  
        //'ppm' => 'image/x-portable-pixmap',  
        //'rgb' => 'image/x-rgb',  
        //'xbm' => 'image/x-xbitmap',  
        //'xpm' => 'image/x-xpixmap',  
        //'xwd' => 'image/x-xwindowdump',  
        //'igs' => 'model/iges',  
        //'iges' => 'model/iges',  
        //'msh' => 'model/mesh',  
        //'mesh' => 'model/mesh',  
        //'silo' => 'model/mesh',  
        //'wrl' => 'model/vrml',  
        //'vrml' => 'model/vrml',  
        //'css' => 'text/css',  
        //'html' => 'text/html',  
        //'htm' => 'text/html',  
        //'asc' => 'text/plain',  
        //'txt' => 'text/plain',  
        //'rtx' => 'text/richtext',  
        //'rtf' => 'text/rtf',  
        //'sgml' => 'text/sgml',  
        //'sgm' => 'text/sgml',  
        //'tsv' => 'text/tab-separated-values',  
        //'wml' => 'text/vnd.wap.wml',  
        //'wmls' => 'text/vnd.wap.wmlscript',  
        //'etx' => 'text/x-setext',  
        //'xsl' => 'text/xml',  
        //'xml' => 'text/xml',  
        //'mpeg' => 'video/mpeg',  
        //'mpg' => 'video/mpeg',  
        //'mpe' => 'video/mpeg',  
        //'qt' => 'video/quicktime',  
        //'mov' => 'video/quicktime',  
        //'mxu' => 'video/vnd.mpegurl',  
        //'avi' => 'video/x-msvideo',  
        //'movie' => 'video/x-sgi-movie',  
        //'ice' => 'x-conference/x-cooltalk'
        #region 调用说明
        //方法ExportControl(System.Web.UI.Control source, string DocumentType,string filename)中
        //第一个参数source表示导出的页面或控件名,当为datagrid或dataList控件时，在导出Excel/word文件时，必须把控件的分页、排序等属性去除并重新绑定，
        //第二个参数DocumentType表示导出的文件类型word或excel
        //第三个参数filename表示需要导出的文件所取的文件名
        //调用方法：
        //ExportData export=new ExportData();
        //export.ExportControl(this, "Word","testfilename");//当为this时表示当前页面
        //这是将整个页面导出为Word,并命名为testfilename
        #endregion
        /**/
        /// <summary>
        /// 将Web控件或页面信息导出(带文件名参数)
        /// </summary>
        /// <param name="source">控件实例</param>        
        /// <param name="DocumentType">导出类型:Excel或Word</param>
        /// <param name="filename">保存文件名</param>
        public static void ExportControl(System.Web.UI.Control source, string DocumentType, string filename)
        {
            //设置Http的头信息,编码格式
            if (DocumentType == "Excel")
            {
                //Excel            
                HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(filename + ".xls", System.Text.Encoding.UTF8));
                HttpContext.Current.Response.ContentType = "application/ms-excel";
            }

            else if (DocumentType == "Word")
            {
                //Word
                HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(filename + ".doc", System.Text.Encoding.UTF8));
                HttpContext.Current.Response.ContentType = "application/ms-word";
            }

            HttpContext.Current.Response.Charset = "UTF-8";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;

            //关闭控件的视图状态
            source.Page.EnableViewState = false;

            //初始化HtmlWriter
            System.IO.StringWriter writer = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter htmlWriter = new System.Web.UI.HtmlTextWriter(writer);
            source.RenderControl(htmlWriter);

            //输出
            HttpContext.Current.Response.Write(writer.ToString());
            HttpContext.Current.Response.End();
        }

        /// <summary>
        /// 获取Excel工作簿名称列表（以下拉框的形式呈现）
        /// </summary>
        /// <param name="fileurl">Excel文件的全路径</param>   
        /// <returns></returns>
        public static string getWorkBookList(string fileurl)
        {
            string nkey = "";
            //绑定当前选择的XLS文件中所有的工作簿名称 
            #region 绑定当前选择的XLS文件中所有的工作簿名称

            //  if (HttpContext.Current.Request["fileurl"] != null)
            if (!string.IsNullOrEmpty(fileurl))
            {
                // string recieveurl = HttpContext.Current.Request["fileurl"].ToString();
                string recieveurl = fileurl.ToString();
                //if (context.Session["inputurl"] != null)
                //{
                //    recieveurl = context.Session["inputurl"].ToString();
                //}
                if (recieveurl == "")
                {
                    nkey = "尚未检测到路径,请选择路径！";
                }
                else
                {
                    if (recieveurl.ToString().ToLower().EndsWith(".xls"))
                    {
                        string newfileurl = recieveurl.ToString();
                        //string newfileurl = HttpContext.Current.Server.MapPath("../UpLoad/") + Water.Common.Utils.getOneRandNum() + ".xls";
                        #region 上传文件
                        if (recieveurl.Contains("fakepath"))
                        {
                            recieveurl = recieveurl.Replace("fakepath", @"Documents and Settings\Administrator\桌面");
                        }
                       // File.Copy(recieveurl, newfileurl, true);
                        ////HttpContext.Current.Response.Write("服务器地址：" + newfileurl+"<br/>");
                        //// HttpContext.Current.Response.Write("客户端地址：" + recieveurl + "<br/>");
                        ////WebClient client = new WebClient();
                        ////client.UploadFile(newfileurl,"PUT",recieveurl);
                        //File.Copy(recieveurl, newfileurl, true);
                        //HttpContext.Current.Response.Write(newfileurl);
                        //HttpContext.Current.Response.Write("<br>服务器地址：" + newfileurl + "<br/>");
                        
                        if (File.Exists(newfileurl))
                        {
                            #region 打开excel文件并获取工作簿名称列表，并进行异常捕捉
                            try
                            {
                                String[] name = WaterCommon.PutOutDataTable.One(newfileurl);
                                nkey = nkey + "表格名称：<select id=\"ddlway\" style=\"width:198px; height:26px; line-height:26px\">";
                                nkey = nkey + "<option value='-1' selected=\"selected\">--请选择--</option>";
                                for (int i = 0; i < name.Length; i++)
                                {
                                    string[] node = name[i].Split('$');  //获取字符串                             
                                    nkey = nkey + "<option value='" + i + "'>" + node[0] + "</option>";
                                }
                                nkey = nkey + "</select>";
                                // 读取出这些工作簿的名称之后对该对象进行销毁 
                                //System.IO.FileInfo objFI = new System.IO.FileInfo(newfileurl);
                                //if (File.Exists(newfileurl))
                                //{
                                //    objFI.IsReadOnly = false;
                                //    File.Delete(newfileurl);
                                //}
                            }
                            catch (Exception ex)
                            {
                                //出现错误的时候删除原先已经复制的新文件
                                if (ex.Message.ToString().Contains("的访问被拒绝"))
                                {
                                    nkey = "操作Excel文件时对其产生的副本文件的访问被拒绝！";
                                }
                                else
                                {
                                    //System.IO.FileInfo objFI = new System.IO.FileInfo(newfileurl);

                                    //if (File.Exists(newfileurl))
                                    //{
                                    //    objFI.IsReadOnly = false;
                                    //    File.Delete(newfileurl);
                                    //}
                                    nkey = "打开Excel文件失败！";
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            nkey = "未找到该文件！";
                        }
                        #endregion
                    }
                    else
                    {
                        nkey = "请选择正确的XLS文件！";
                    }
                }
            }
            else
            {
                nkey = "参数错误！";
            }
            #endregion
            return nkey;
        }
        /// <summary>
        /// 获取excel文件的所有的工作簿名称（2013-05-04pm18:26zhangquan）
        /// </summary>
        /// <param name="excelFile"></param>
        /// <returns></returns>
        public static String[] One(string excelFile)
        {
            System.Data.OleDb.OleDbConnection objConn = null;
            System.Data.DataTable dt = null;
            try
            {
                String connString = "Provider=Microsoft.Jet.OLEDB.4.0;" +
                    "Data Source=" + excelFile + ";Extended Properties=Excel 8.0;";
                objConn = new System.Data.OleDb.OleDbConnection(connString);
                int isyc = 0;//是否存在异常
                string ycmsg = "";//异常信息
                try
                {
                    objConn.Open();
                }
                catch (Exception msg)
                {
                    isyc = 1;
                    string m = msg.Message.ToString();
                    if (m.Contains("外部表不是预期的格式。"))
                    {
                        //该EXCEL文件保存的格式错误，请重新确保盖Excel文件格式正确
                        ycmsg="该EXCEL文件保存的格式错误，请重新确保该Excel文件格式正确";
                    }
                    else
                    {
                        //打开该Excel文件失败
                        ycmsg = "打开该Excel文件失败,"+msg.Message.ToString();
                    }                   
                }
                if (isyc == 0)
                {
                    dt = objConn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, null);

                    if (dt == null)
                    {
                        return null;
                    }
                    String[] excelSheets = new String[dt.Rows.Count];
                    int i = 0;
                    // Add the sheet name to the string array.
                    foreach (System.Data.DataRow row in dt.Rows)
                    {
                        excelSheets[i] = row["TABLE_NAME"].ToString();
                        i++;
                    }
                    // Loop through all of the sheets if you want too...
                    for (int j = 0; j < excelSheets.Length; j++)
                    {
                        //Query each excel sheet.
                    }
                    objConn.Close();
                    return excelSheets;
                }
                else
                {
                    string[] strarr = { ycmsg };
                    return strarr;
                }
            }
            catch (Exception ex)
            {
                string errstr = ex.Message.ToString();
                return null;
            }
            finally
            {
                // Clean up.
                if (objConn != null)
                {
                    objConn.Close();
                    objConn.Dispose();
                }
                if (dt != null)
                {
                    dt.Dispose();
                }
            }
        }
        /// <summary>
        /// 将DataTable中的数据导出到指定的Excel文件中
        /// </summary>
        /// <param name="page">Web页面对象</param>
        /// <param name="tab">包含被导出数据的DataTable对象</param>
        /// <param name="FileName">Excel文件的名称</param>
        public static void Export(System.Web.UI.Page page, System.Data.DataTable tab, string FileName)
        {
            System.Web.HttpResponse httpResponse = page.Response;
            System.Web.UI.WebControls.DataGrid dataGrid = new System.Web.UI.WebControls.DataGrid();
            dataGrid.DataSource = tab.DefaultView;
            dataGrid.AllowPaging = false;
            dataGrid.HeaderStyle.BackColor = System.Drawing.Color.Green;
            dataGrid.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            dataGrid.HeaderStyle.Font.Bold = true;
            dataGrid.DataBind();
            httpResponse.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(FileName, System.Text.Encoding.UTF8)); //filename="*.xls";
            httpResponse.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            httpResponse.ContentType = "application/ms-excel";
            System.IO.StringWriter tw = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
            dataGrid.RenderControl(hw);

            string filePath = page.Server.MapPath("..") + "\\Files\\" + FileName;
            System.IO.StreamWriter sw = System.IO.File.CreateText(filePath);
            sw.Write(tw.ToString());
            sw.Close();


            DownFile(httpResponse, FileName, filePath);

            httpResponse.End();
        }
        /// <summary>
        /// 文件下载
        /// </summary>
        /// <param name="Response"></param>
        /// <param name="fileName"></param>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public static void DownFile(System.Web.HttpResponse Response, string fileName, string fullPath)
        {
            try
            {
                Response.ContentType = "application/octet-stream";

                Response.AppendHeader("Content-Disposition", "attachment;filename=" +
                 HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8) + ";charset=GB2312");
                System.IO.FileStream fs = System.IO.File.OpenRead(fullPath);
                long fLen = fs.Length;
                int size = 102400;//每100K同时下载数据 
                byte[] readData = new byte[size];//指定缓冲区的大小 
                if (size > fLen) size = Convert.ToInt32(fLen);
                long fPos = 0;
                bool isEnd = false;
                while (!isEnd)
                {
                    if ((fPos + size) > fLen)
                    {
                        size = Convert.ToInt32(fLen - fPos);
                        readData = new byte[size];
                        isEnd = true;
                    }
                    fs.Read(readData, 0, size);//读入一个压缩块 
                    Response.BinaryWrite(readData);
                    fPos += size;
                }
                fs.Close();
                System.IO.FileInfo objFI = new System.IO.FileInfo(fullPath);

                if (File.Exists(fullPath))
                {
                    objFI.IsReadOnly = false;
                    File.Delete(fullPath);
                }
                //System.IO.File.Delete(fullPath);
                // return true;
            }
            catch
            {
                //return false;
            }
            HttpContext.Current.Response.End();
        }
        /// <summary>
        /// 为导出的DBF文件提供下载链接
        /// </summary>
        /// <param name="mFilePath">文件所属路径</param>
        /// <param name="mTableName">该DBF文件名称</param>
        public static void DownLoadDbfFile(string mFilePath, string mTableName)
        {
            #region 提供下载
            System.IO.FileStream fs = new System.IO.FileStream(mFilePath + "//" + mTableName + ".DBF", System.IO.FileMode.Open, System.IO.FileAccess.Read);

            byte[] b = new Byte[fs.Length];
            fs.Read(b, 0, b.Length);
            fs.Flush();
            fs.Close();

            //System.IO.File.Delete(SavePdfPath); 
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.Buffer = false;
            HttpContext.Current.Response.ContentType = "application/octet-stream";      //ContentType;           
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(mTableName + ".DBF", System.Text.Encoding.UTF8));//--如果需要弹出下载框则此处值需要取消该句注释即可
            //int port = DDL.DDLInterface.hs_state(5);
            //int isOK=DDL.DDLInterface.downfile(port, mFilePath + "//" + mTableName + ".DBF");//上传文件到抄表机中的“文件”这个菜单中            
            HttpContext.Current.Response.AppendHeader("Content-Length", b.Length.ToString());
            fs.Close();
            fs.Close();
            if (b.Length > 0)
            {
                HttpContext.Current.Response.OutputStream.Write(b, 0, b.Length);
            }
            HttpContext.Current.Response.Flush();
            //清除临时文件
            string newfileurl = mFilePath + "//" + mTableName + ".DBF";
            System.IO.FileInfo objFI = new System.IO.FileInfo(newfileurl);

            if (File.Exists(newfileurl))
            {
                objFI.IsReadOnly = false;
                File.Delete(newfileurl);
            }
            HttpContext.Current.Response.End();

            // Response.Clear();
            //Response.ClearHeaders();
            //Response.Clear();
            //Response.ClearHeaders();
            //Response.Buffer = false;
            //Response.ContentType = "application/octet-stream";      //ContentType;
            //Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(mFilePath + "//" + mTableName + ".DBF", System.Text.Encoding.UTF8));
            //Response.AppendHeader("Content-Length", b.Length.ToString());
            //fs.Close();
            //fs.Close();
            //if (b.Length > 0)
            //{
            //    Response.OutputStream.Write(b, 0, b.Length);
            //}
            //Response.Flush();
            //Response.End();
            #endregion 提供下载
        }

        /// <summary>
        /// HSS
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="xlsname"></param>
        /// <returns></returns>
        public static HSSFWorkbook HSSPutOutDataTableToExcel(System.Data.DataTable dt, string xlsname)
        {
            //创建Excel文件的对象
            HSSFWorkbook book = new HSSFWorkbook();
            //添加一个sheet
            ISheet sheet1 = book.CreateSheet(xlsname);
            if (dt != null && dt.Rows.Count > 0)
            {
                //给sheet1添加第一行的头部标题
                IRow row1 = sheet1.CreateRow(0);
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    row1.CreateCell(i, CellType.String).SetCellValue(dt.Columns[i].ColumnName);
                }
                //将数据逐步写入sheet1各个行
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    IRow rowtemp = sheet1.CreateRow(i + 1);
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        rowtemp.CreateCell(j).SetCellValue(dt.Rows[i][j].ToString());
                    }
                }
            }
            return book;
        }
    }
}
