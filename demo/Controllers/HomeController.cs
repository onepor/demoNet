using DemoTimers;
using My_Base;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Timers;
using System.Web;
using System.Web.Mvc;
using WaterCommon;

namespace demo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        

        public ActionResult retxls()
        {
            var name = "wp";
            var Des = new DesHelpercs();
            var key = Des.Generator();

            var desp = Des.DESEncrypt(name, null);
            var md5 = Des.DESEncryptMD5(name, "08B68FF09F0202CCD8D5585D661");

            var desname= Des.DESDecrypt(desp, null);
            var md5name = Des.DESDecryptMD5(md5, "08B68FF09F0202CCD8D5585D662");

            //ServiceReference1.iPaiWebServiceSoapClient db = new ServiceReference1.iPaiWebServiceSoapClient();
            //var json = db.GetBill("08B68FF09F0202CCD8D5585D661A492C9718E2A25C3B7266E34BB537BEA06B6268C9AD13669D4D55C1A771BCD49EBEA1", "10001");

            //var list = JsonConvert.DeserializeObject(json);

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
            HSSFWorkbook book = PutOutDataTable.HSSPutOutDataTableToExcel(dt, "aa");

            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel",  "aa.xls");
        }
        public string UploadUserAdd()
        {

            HttpPostedFileBase fostFile = Request.Files["file"];
            if (fostFile == null)
            {
                return "";
            }
            if (fostFile.ContentLength == 0)
            {
                return "";
            }
            Stream streamfile = fostFile.InputStream;
            HSSFWorkbook hssfworkbook = new HSSFWorkbook(streamfile);
            //long uuid = GetNewUUID();
            //drqf.Add(uuid, hssfworkbook);

            var sheetname = "";
            for (int i = 0; i < hssfworkbook.Count; i++)
            {
                sheetname += "<option value='" + i + "'>" + hssfworkbook.GetSheetAt(i).SheetName + "</option>";
            }

            //ViewBag.uuid = uuid;
            //ViewBag.sheetname = sheetname;

            return sheetname;
        }
        public ActionResult UploadUserAdd1()
        {
            HttpPostedFileBase fostFile = Request.Files["file"];
            if (fostFile == null)
            {
                return View();
            }
            if (fostFile.ContentLength == 0)
            {
                return View();
            }
            Stream streamfile = fostFile.InputStream;
            HSSFWorkbook hssfworkbook = new HSSFWorkbook(streamfile);
            //long uuid = GetNewUUID();
            //drqf.Add(uuid, hssfworkbook);

            var sheetname = "";
            for (int i = 0; i < hssfworkbook.Count; i++)
            {
                sheetname += "<option value='" + i + "'>" + hssfworkbook.GetSheetAt(i).SheetName + "</option>";
            }

            //ViewBag.uuid = uuid;
            ViewBag.sheetname = sheetname;

            return View();
        }



        #region Excel导入


        #region Excel导入
        /// <summary>  
        /// Excel导入  
        /// </summary>  
        /// <returns></returns>  
        public DataTable GetTableFromExcel( int sheetid, Dictionary<long, HSSFWorkbook> drexcel,long uuid = 0)
        {
            DataTable table = null;
            //var now = drexcel.Where(s => s.Key == uuid).ToList();
            var now = drexcel.ToList();
            if (now != null && now.Count > 0)
            {
                HSSFWorkbook hssfworkbook = now.First().Value;

                NPOI.SS.UserModel.ISheet sheet = hssfworkbook.GetSheetAt(sheetid);
                if (sheet != null)
                {
                    table = new DataTable();
                    IRow headerRow = sheet.GetRow(0);//第一行为标题行  
                    if (headerRow == null)
                    {
                        return null;
                    }
                    int cellCount = headerRow.LastCellNum;//LastCellNum = PhysicalNumberOfCells  
                    int rowCount = sheet.LastRowNum;//LastRowNum = PhysicalNumberOfRows - 1  
                                                    //handling header.  
                    for (int i = headerRow.FirstCellNum; i < cellCount; i++)
                    {
                        DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                        table.Columns.Add(column);
                    }
                    for (int i = (sheet.FirstRowNum + 1); i <= rowCount; i++)
                    {
                        IRow row = sheet.GetRow(i);
                        DataRow dataRow = table.NewRow();
                        if (row != null)
                        {
                            for (int j = row.FirstCellNum; j < cellCount; j++)
                            {
                                if (row.GetCell(j) != null)
                                    dataRow[j] = GetCellValue(row.GetCell(j));
                            }
                        }
                        table.Rows.Add(dataRow);
                    }
                }

            }
            return table;
        }

        #endregion

        #region 根据Excel列类型获取列的值
        /// <summary>  
        /// 根据Excel列类型获取列的值  
        /// </summary>  
        /// <param name="cell">Excel列</param>  
        /// <returns></returns>  
        private static string GetCellValue(ICell cell)
        {
            if (cell == null)
                return string.Empty;
            switch (cell.CellType)
            {
                case CellType.Blank:
                    return string.Empty;
                case CellType.Boolean:
                    return cell.BooleanCellValue.ToString();
                case CellType.Error:
                    return cell.ErrorCellValue.ToString();
                case CellType.Numeric:
                case CellType.Unknown:
                default:
                    return cell.ToString();
                case CellType.String:
                    return cell.StringCellValue;
                case CellType.Formula:
                    try
                    {
                        HSSFFormulaEvaluator e = new HSSFFormulaEvaluator(cell.Sheet.Workbook);
                        e.EvaluateInCell(cell);
                        return cell.ToString();
                    }
                    catch
                    {
                        return cell.NumericCellValue.ToString();
                    }
            }
        }

        #endregion

        #endregion

        /// <summary>
        /// 生成model
        /// </summary>
        /// <returns></returns>
        public int ProduceModel()
        {
            int ret = 0;
            try
            {
                
                Mysql_Conn.BusinessDBConn().DbFirst.CreateClassFile(" D:\\SqlSugar\\Model");//c:\\Demo\\1
                ret = 200;
            }
            catch (Exception ex)
            {
                ret = -200;
                throw ex;
            }
            return ret;
        }

    }
}