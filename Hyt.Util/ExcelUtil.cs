using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using NPOI.HSSF.UserModel;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using System.Net;
using NPOI.XSSF.UserModel;
namespace Hyt.Util
{
    /// <summary>
    /// Excel导入导出工具类
    /// </summary>
    /// <remarks>2013-7-3 杨浩 添加</remarks>
    public class ExcelUtil
    {
        private static readonly Dictionary<string, IDictionary<string, string>> dicColsMappingList = new Dictionary<string, IDictionary<string, string>>(); //缓存池
        private static readonly object Sync = new object();

        /// <summary>
        /// 获取Excel导入导出
        /// </summary>
        /// <param name="model">对应Execl表头模板实体类</param>
        /// <returns></returns>
        /// <remarks>2016-11-28 杨浩 创建</remarks>
        public static IDictionary<string, string> GetDicColsMapping<T>()
        {
            Type type = typeof(T);

            lock (Sync)
            {
                if (!dicColsMappingList.ContainsKey(type.Name))
                {
                    var dicColsMapping = new Dictionary<string, string>();

                    PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(type);

                    for (int i = 0; i < properties.Count; i++)
                    {
                        dicColsMapping.Add(properties[i].Name, properties[i].Description);
                    }

                    if (!dicColsMappingList.ContainsKey(type.Name))
                        dicColsMappingList.Add(type.Name, dicColsMapping);
                }
            }


            return dicColsMappingList[type.Name];
        }


        /// <summary>
        /// 导出到Excel(web版)
        /// </summary>
        /// <param name="sourceData">源数据</param>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        /// <remarks>2013-7-3 杨浩 添加</remarks>
        public static void Export(DataTable sourceData, string fileName = null)
        {
            if (sourceData == null)
                throw new ArgumentNullException("sourceData");

            IWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = null;
            IRow row = null;
            int count = 0;
            sheet = workbook.CreateSheet("Sheet1");
            row = sheet.CreateRow(0);
            //如果没有自定义的行首,那么采用反射集合的属性名做行首

            foreach (DataColumn value in sourceData.Columns) //生成sheet第一行列名 
            {
                ICell cell = row.CreateCell(count++);
                cell.SetCellValue(value.ColumnName);
            }
            //将数据导入到excel表中
            for (int i = 0; i < sourceData.Rows.Count; i++)
            {
                row = sheet.CreateRow(i + 1);
                count = 0;

                object value = null;
                foreach (DataColumn key in sourceData.Columns)
                {
                    ICell cell = row.CreateCell(count++);

                    value = sourceData.Rows[i][key.ColumnName].ToString();
                    //日期格式导出修改 huangwei 2013-12-23 yyyy-MM-dd HH:ss
                    //格式应该是"yyyy-MM-dd HH:mm"
                    cell.SetCellValue(value == null ? String.Empty
                        : (value is DateTime ? ((DateTime)value).ToString("yyyy-MM-dd HH:mm") : value.ToString()));
                }
            }

            //定义文件名
            if (string.IsNullOrEmpty(fileName))
                fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            else
                fileName = fileName + ".xls";

            //当前http头信息
            var response = HttpContext.Current.Response;
            response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", fileName));
            response.Clear();

            //Write the stream data of workbook to the root directory
            MemoryStream file = new MemoryStream();
            workbook.Write(file);
            file.WriteTo(response.OutputStream);
            response.End();
        }

        /// <summary>
        /// 导出到Excel(web版)
        /// </summary>
        /// <param name="sourceData">源数据</param>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        /// <remarks>2013-7-3 杨浩 添加</remarks>
        public static void Export<T>(DataTable sourceData, string fileName = null)
        {
            if (sourceData == null)
                throw new ArgumentNullException("sourceData");

            IWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = null;
            IRow row = null;
            int count = 0;
            sheet = workbook.CreateSheet("Sheet1");

            var dicColsMapping = ExcelUtil.GetDicColsMapping<T>();

            row = sheet.CreateRow(0);
            //如果没有自定义的行首,那么采用反射集合的属性名做行首

            foreach (var value in dicColsMapping.Values) //生成sheet第一行列名 
            {
                ICell cell = row.CreateCell(count++);
                cell.SetCellValue(value);
            }
            //将数据导入到excel表中
            for (int i = 0; i < sourceData.Rows.Count; i++)
            {
                row = sheet.CreateRow(i + 1);
                count = 0;

                object value = null;
                foreach (var key in dicColsMapping.Keys)
                {
                    ICell cell = row.CreateCell(count++);

                    value = sourceData.Rows[i][key].ToString();
                    //日期格式导出修改 huangwei 2013-12-23 yyyy-MM-dd HH:ss
                    //格式应该是"yyyy-MM-dd HH:mm"
                    cell.SetCellValue(value == null ? String.Empty
                        : (value is DateTime ? ((DateTime)value).ToString("yyyy-MM-dd HH:mm") : value.ToString()));
                }
            }

            //定义文件名
            if (string.IsNullOrEmpty(fileName))
                fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            else
                fileName = fileName + ".xls";

            //当前http头信息
            var response = HttpContext.Current.Response;
            response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", fileName));
            response.Clear();

            //Write the stream data of workbook to the root directory
            MemoryStream file = new MemoryStream();
            workbook.Write(file);
            file.WriteTo(response.OutputStream);
            response.End();
        }


        /// <summary>
        /// 导出到Excel(web版)
        /// </summary>
        /// <param name="sourceData">源数据</param>
        /// <param name="headerList">列名</param>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        /// <remarks>2013-7-3 杨浩 添加</remarks>
        public static void Export<T>(IList<T> sourceData, IList<String> headerList = null, string fileName = null)
        {
            if (sourceData == null)
                throw new ArgumentNullException("sourceData");

            IWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = null;
            IRow row = null;
            int count = 0;
            sheet = workbook.CreateSheet("Sheet1");

            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            row = sheet.CreateRow(0);
            //如果没有自定义的行首,那么采用反射集合的属性名做行首

            int headerCount = headerList != null ? headerList.Count : properties.Count;
            for (int i = 0; i < headerCount; i++) //生成sheet第一行列名 
            {
                ICell cell = row.CreateCell(count++);
                if (headerList == null)
                {
                    cell.SetCellValue(String.IsNullOrEmpty(properties[i].Description)
                                          ? properties[i].Name
                                          : properties[i].Description);
                }
                else
                    cell.SetCellValue(headerList[i]);
            }

            //将数据导入到excel表中
            for (int i = 0; i < sourceData.Count; i++)
            {
                row = sheet.CreateRow(i + 1);
                count = 0;

                object value = null;
                for (int j = 0; j < properties.Count; j++)
                {
                    ICell cell = row.CreateCell(count++);
                    value = properties[j].GetValue(sourceData[i]);
                    //日期格式导出修改 huangwei 2013-12-23 yyyy-MM-dd HH:ss
                    //格式应该是"yyyy-MM-dd HH:mm"
                    cell.SetCellValue(value == null ? String.Empty
                        : (value is DateTime ? ((DateTime)value).ToString("yyyy-MM-dd HH:mm") : value.ToString()));
                }
            }

            //每列宽度自适应
            //for (Int32 i = 0; i < workbook.NumberOfSheets; i++)
            //{
            //    sheet = workbook.GetSheetAt(i);
            //    for (Int32 h = 0; h < headerCount; h++)
            //    {
            //        sheet.AutoSizeColumn(h);
            //    }
            //}

            //定义文件名
            if (string.IsNullOrEmpty(fileName))
                fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            else
                fileName = fileName + ".xls";

            //当前http头信息
            var response = HttpContext.Current.Response;
            response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", fileName));
            response.Clear();

            //Write the stream data of workbook to the root directory
            MemoryStream file = new MemoryStream();
            workbook.Write(file);
            file.WriteTo(response.OutputStream);
            response.End();
        }

        /// <summary>
        /// 模板导出Excel
        /// </summary>
        /// <param name="sourceData">源数据</param>
        /// <param name="filePath">模板文件路径(\Templates\Excel\xxx.xls)</param>
        /// <param name="startRowNum">数据开始行数(从0开始)</param>
        /// <param name="fileName">文件名</param>
        /// <param name="dateFormat">日期格式</param>
        /// <param name="isInsertName">是否在表格中插入文件名(默认为true)</param>
        /// <returns>空</returns>
        /// <remarks>
        /// 黄伟 2013-12-17 创建
        /// 2013-12-25 周唐炬 修改日期数据格式
        /// </remarks>
        public static void ExportFromTemplate<T>(IList<T> sourceData, string filePath, int startRowNum = 1, string fileName = null, string dateFormat = "yyyy-MM-dd HH:mm", bool isInsertName = true)
        {
            if (sourceData == null)
                throw new ArgumentNullException("sourceData");

            //read the template via FileStream, it is suggested to use FileAccess.Read to prevent file lock.
            //book1.xls is an Excel-2007-generated file, so some new unknown BIFF records are added.
            var path = System.Web.HttpContext.Current.Server.MapPath(filePath);
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);

            HSSFWorkbook hssfworkbook = new HSSFWorkbook(fs);
            HSSFSheet sheet = hssfworkbook.GetSheetAt(0) as HSSFSheet;  //黄志勇 2013-12-20 修改
            IRow row = null;
            var count = 0;
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));

            //将数据导入到excel表中
            //set the name and date at the first row
            if (isInsertName)
            {
                sheet.GetRow(0).GetCell(0).SetCellValue(fileName);
            }
            for (int i = 0; i < sourceData.Count; i++)
            {
                row = sheet.CreateRow(i + startRowNum);//the num start
                count = 0;

                object value = null;
                for (int j = 0; j < properties.Count; j++)
                {
                    ICell cell = row.CreateCell(count++);
                    value = properties[j].GetValue(sourceData[i]);
                    //日期格式导出修改 huangwei 2013-12-23 yyyy-MM-dd HH:ss. //日期格式通过dateFormat参数设置 余勇
                    cell.SetCellValue(value == null ? String.Empty
                        : (value is DateTime ? ((DateTime)value).ToString(dateFormat) : value.ToString()));
                }
            }

            //Force excel to recalculate all the formula while open
            sheet.ForceFormulaRecalculation = true;

            //每列宽度自适应
            //for (Int32 i = 0; i < workbook.NumberOfSheets; i++)
            //{
            //    sheet = workbook.GetSheetAt(i);
            //    for (Int32 h = 0; h < headerCount; h++)
            //    {
            //        sheet.AutoSizeColumn(h);
            //    }
            //}

            //定义文件名
            if (string.IsNullOrEmpty(fileName))
                fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            else
                fileName = fileName + ".xls";

            //当前http头信息
            var response = HttpContext.Current.Response;
            response.ContentType = "application/vnd.ms-excel";

            //文件名兼容性 2014-05-27 朱家宏 修改
            if (HttpContext.Current.Request.UserAgent != null && HttpContext.Current.Request.UserAgent.ToLower().IndexOf("firefox", System.StringComparison.Ordinal) > -1)
            {
                response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", fileName));
            }
            else
            {
                response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8)));
            }
            response.Clear();

            //Write the stream data of workbook to the root directory
            MemoryStream ms = new MemoryStream();
            hssfworkbook.Write(ms);
            ms.WriteTo(response.OutputStream);
            response.End();
        }
        /// <summary>
        /// excel import
        /// </summary>
        /// <param name="stream">Stream instance</param>
        /// <param name="cols">the cols to import from</param>
        /// <returns>datatable(null indicate at least one of the cols specified cannot be found )</returns>
        /// <remarks>2013-10-11 huangwei created</remarks>
        public static DataTable ImportExcel(Stream stream, int sheetIndex = 0, HSSFWorkbook hssfwb = null, params string[] cols)
        {

            //using (var file = new FileStream(path, FileMode.Open, FileAccess.Read))
            //{
            //    hssfwb = new HSSFWorkbook(file);
            //}

            /*
             * xlsx(2007+) The supplied data appears to be in the Office 2007+ XML. You are calling the part of POI that deals with OLE2 Office Documents. You need to call a different part of POI to process this data (eg XSSF instead of HSSF)"
             */
            if (hssfwb == null)
            {
                hssfwb = new HSSFWorkbook(stream);
            }
            ISheet sheet = hssfwb.GetSheetAt(sheetIndex);
            var dt = new DataTable();
            var lstColIndexes = new List<int>();

            #region read data
            var flagColHeader = true;
            for (int i = 0; i <= sheet.LastRowNum; i++)
            {
                var row = sheet.GetRow(i);

                if (row == null)
                    continue;

                if (i == 0 || (!flagColHeader && i == 1)) //header
                {
                    if (dt.Columns.Count != 0)
                        continue;//already find the cols specified in the first cols
                    row.Cells.ForEach(c => dt.Columns.Add(c.StringCellValue));
                    if (cols.Any())
                    {
                        //check
                        if (cols.Any(col => dt.Columns.IndexOf(col) == -1))
                        {
                            flagColHeader = false;
                            dt = new DataTable();

                        }
                        else
                        {
                            flagColHeader = true;
                        }
                        if (flagColHeader)
                        {
                            cols.ForEach(col => lstColIndexes.Add(dt.Columns.IndexOf(col)));
                        }
                    }
                    //cols required not found in first row,
                    if (i == 1 && !flagColHeader)
                    {
                        return null;
                    }
                    continue;
                }

                //pick the required cols

                DataRow dataRow = dt.NewRow();
                if (lstColIndexes.Any())
                {
                    //判断row列中的值是否都为空，若都为空则不插入tb 余勇 2014-01-16
                    if (row.Cells.Any(col => col != null && !string.IsNullOrEmpty(col.ToString())))
                    {
                        lstColIndexes.ForEach(colIndex =>
                        {
                            //为避免超出索引错误，需判断row列数是否大于索引
                            if (row.Cells.Count > colIndex)
                            {
                                if (row.Cells[colIndex].CellType == CellType.Numeric)
                                {
                                    dataRow[colIndex] = row.Cells[colIndex].NumericCellValue;
                                }
                                else
                                    dataRow[colIndex] = row.Cells[colIndex].StringCellValue;
                            }

                        });
                        dt.Rows.Add(dataRow);
                    }
                }
                else //pick all
                {

                    row.Cells.ForEach(cell =>
                    {

                        if (cell.CellType == CellType.Numeric)
                            dataRow[cell.ColumnIndex] = cell.NumericCellValue;
                        else
                            dataRow[cell.ColumnIndex] = cell.StringCellValue;

                    });
                    dt.Rows.Add(dataRow);
                }
            }
            #endregion

            return dt;
        }
        /// <summary>
        /// excel import
        /// </summary>
        /// <param name="stream">Stream instance</param>
        /// <param name="cols">the cols to import from</param>
        /// <returns>datatable(null indicate at least one of the cols specified cannot be found )</returns>
        /// <remarks>2013-10-11 huangwei created</remarks>
        public static DataTable ImportExcel(Stream stream, ref HSSFWorkbook thssfwb, params string[] cols)
        {
            HSSFWorkbook hssfwb;
            //using (var file = new FileStream(path, FileMode.Open, FileAccess.Read))
            //{
            //    hssfwb = new HSSFWorkbook(file);
            //}

            /*
             * xlsx(2007+) The supplied data appears to be in the Office 2007+ XML. You are calling the part of POI that deals with OLE2 Office Documents. You need to call a different part of POI to process this data (eg XSSF instead of HSSF)"
             */
            hssfwb = new HSSFWorkbook(stream);
            thssfwb = hssfwb;
            ISheet sheet = hssfwb.GetSheetAt(0);
            var dt = new DataTable();
            var lstColIndexes = new List<int>();

            #region read data
            var flagColHeader = true;
            for (int i = 0; i <= sheet.LastRowNum; i++)
            {
                var row = sheet.GetRow(i);

                if (row == null)
                    continue;

                if (i == 0 || (!flagColHeader && i == 1)) //header
                {
                    if (dt.Columns.Count != 0)
                        continue;//already find the cols specified in the first cols
                    row.Cells.ForEach(c => dt.Columns.Add(c.StringCellValue));
                    if (cols.Any())
                    {
                        //check
                        if (cols.Any(col => dt.Columns.IndexOf(col) == -1))
                        {
                            flagColHeader = true;
                            dt = new DataTable();
                            row.Cells.ForEach(c => dt.Columns.Add(c.StringCellValue));
                        }
                        else
                        {
                            flagColHeader = true;
                        }
                        if (flagColHeader)
                        {
                            cols.ForEach(col => lstColIndexes.Add(dt.Columns.IndexOf(col)));
                        }
                    }
                    //cols required not found in first row,
                    if (i == 1 && !flagColHeader)
                    {
                        return null;
                    }
                    continue;
                }

                //pick the required cols

                DataRow dataRow = dt.NewRow();
                if (lstColIndexes.Any())
                {
                    //判断row列中的值是否都为空，若都为空则不插入tb 余勇 2014-01-16
                    if (row.Cells.Any(col => col != null && !string.IsNullOrEmpty(col.ToString())))
                    {
                        //string txt=row.GetCell()
                        lstColIndexes.ForEach(colIndex =>
                        {
                            if (colIndex >= 0)
                            {

                                HSSFCell tempCell = row.GetCell(colIndex) as HSSFCell;
                                if (tempCell == null)
                                {
                                    dataRow[colIndex] = "";
                                }
                                else
                                {
                                    //为避免超出索引错误，需判断row列数是否大于索引
                                    if (tempCell.CellType == CellType.Numeric)
                                    {
                                        dataRow[colIndex] = tempCell.NumericCellValue;
                                    }
                                    else
                                        dataRow[colIndex] = tempCell.StringCellValue;
                                }
                            }
                        });
                        dt.Rows.Add(dataRow);
                    }
                }
                else //pick all
                {

                    row.Cells.ForEach(cell =>
                    {

                        if (cell.CellType == CellType.Numeric)
                            dataRow[cell.ColumnIndex] = cell.NumericCellValue;
                        else
                            dataRow[cell.ColumnIndex] = cell.StringCellValue;

                    });
                    dt.Rows.Add(dataRow);
                }
            }
            #endregion

            return dt;
        }

        /// <summary>
        /// excel import
        /// </summary>
        /// <param name="stream">Stream instance</param>
        /// <param name="cols">the cols to import from</param>
        /// <returns>datatable(null indicate at least one of the cols specified cannot be found )</returns>
        /// <remarks>2013-10-11 huangwei created</remarks>
        /// <remarks>2016-7-25 杨云奕 修改 excel内容，和导入的字段内容有差异的时候数据混乱的问题</remarks>
        public static DataTable ImportExcel(Stream stream, params string[] cols)
        {
            HSSFWorkbook hssfwb;
            //using (var file = new FileStream(path, FileMode.Open, FileAccess.Read))
            //{
            //    hssfwb = new HSSFWorkbook(file);
            //}

            /*
             * xlsx(2007+) The supplied data appears to be in the Office 2007+ XML. You are calling the part of POI that deals with OLE2 Office Documents. You need to call a different part of POI to process this data (eg XSSF instead of HSSF)"
             */
            hssfwb = new HSSFWorkbook(stream);
            ISheet sheet = hssfwb.GetSheetAt(0);
            var dt = new DataTable();
            var lstColIndexes = new List<int>();

            #region read data
            var flagColHeader = true;
            Dictionary<string, int> dicKeyPostion = new Dictionary<string, int>();
            for (int i = 0; i <= sheet.LastRowNum; i++)
            {
                var row = sheet.GetRow(i);

                if (row == null)
                    continue;

                if (i == 0 || (!flagColHeader && i == 1)) //header
                {
                    if (dt.Columns.Count != 0)
                        continue;//already find the cols specified in the first cols

                    string strCols = string.Join(",", cols);
                    int postionIndex = 0;
                    foreach (ICell StringCellValue in row.Cells)
                    {

                        if (StringCellValue != null)
                            StringCellValue.SetCellType(CellType.String);

                        if ((("," + strCols + ",").IndexOf("," + StringCellValue.StringCellValue + ",") != -1))
                        {
                            dt.Columns.Add(StringCellValue.StringCellValue);
                            ///保存取值游标存入位置字段集合
                            dicKeyPostion.Add(StringCellValue.StringCellValue, postionIndex);
                        }
                        postionIndex++;
                    }
                    //row.Cells.ForEach(c => (((","+strCols+",").IndexOf(","+c.StringCellValue+",")!=-1)?dt.Columns.Add(c.StringCellValue):""));

                    if (cols.Any())
                    {
                        //check
                        if (cols.Any(col => dt.Columns.IndexOf(col) == -1))
                        {
                            flagColHeader = false;
                            dt = new DataTable();

                        }
                        else
                        {
                            flagColHeader = true;
                        }
                        if (flagColHeader)
                        {
                            cols.ForEach(col => lstColIndexes.Add(dt.Columns.IndexOf(col)));
                        }
                    }
                    //cols required not found in first row,
                    if (i == 1 && !flagColHeader)
                    {
                        return null;
                    }
                    continue;
                }

                //pick the required cols

                DataRow dataRow = dt.NewRow();

                if (lstColIndexes.Any())
                {
                    //判断row列中的值是否都为空，若都为空则不插入tb 余勇 2014-01-16
                    if (row.Cells.Any(col => col != null && !string.IsNullOrEmpty(col.ToString())))
                    {
                        for (int colIndex = 0; colIndex < lstColIndexes.Count(); colIndex++)
                        {
                            //if (row.Cells.Count > colIndex)
                            //{
                                if (row.GetCell(dicKeyPostion[cols[colIndex]]) == null)
                                {
                                    dataRow[cols[colIndex]] = "";
                                }
                                else
                                    if (row.GetCell(dicKeyPostion[cols[colIndex]]).CellType == CellType.Numeric)
                                    {//修改取值游标，通过变态获取inedx
                                        dataRow[cols[colIndex]] = row.GetCell(dicKeyPostion[cols[colIndex]]).NumericCellValue;
                                    }
                                    else if (row.GetCell(dicKeyPostion[cols[colIndex]]).CellType == CellType.Formula)
                                    {//EXCEL导入，出现公式的话以数值取值
                                        dataRow[cols[colIndex]] = row.GetCell(dicKeyPostion[cols[colIndex]]).NumericCellValue;
                                    }
                                    else///修改取值游标，通过变态获取inedx
                                        dataRow[cols[colIndex]] = row.GetCell(dicKeyPostion[cols[colIndex]]).StringCellValue;
                            //}
                        }
                        //lstColIndexes.ForEach(colIndex =>
                        //{
                        //    //为避免超出索引错误，需判断row列数是否大于索引
                            

                        //});
                        dt.Rows.Add(dataRow);
                    }
                }
                else //pick all
                {

                    row.Cells.ForEach(cell =>
                    {

                        if (cell.CellType == CellType.Numeric)
                            dataRow[cell.ColumnIndex] = cell.NumericCellValue;
                        else
                            dataRow[cell.ColumnIndex] = cell.StringCellValue;

                    });
                    dt.Rows.Add(dataRow);
                }
            }
            #endregion

            return dt;
        }


        /// <summary>
        /// excel import
        /// </summary>
        /// <param name="stream">Stream instance</param>
        /// <param name="cols">the cols to import from</param>
        /// <param name="err">error message</param>
        /// <returns>datatable(null indicate at least one of the cols specified cannot be found )</returns> 
        /// <remarks>2018-3-28 李法政created  重载excel导入方法，返回错误消息，并对excel版本做了适配</remarks>
        public static DataTable ImportExcel(Stream stream, string[] cols, out string err)
        {
            err = "";
            IWorkbook hssfwb;
            //using (var file = new FileStream(path, FileMode.Open, FileAccess.Read))
            //{
            //    hssfwb = new HSSFWorkbook(file);
            //}

            /*
             * xlsx(2007+) The supplied data appears to be in the Office 2007+ XML. You are calling the part of POI that deals with OLE2 Office Documents. You need to call a different part of POI to process this data (eg XSSF instead of HSSF)"
             */
            try//需要在NuGet管理中搜索NPOI 并添加引用，目前引用的版本为2.3.0.0
            {
                //把xlsx文件中的数据写入wk中
                hssfwb = new XSSFWorkbook(stream);
            }
            catch (Exception)
            {
                //把xls文件中的数据写入wk中
                hssfwb = new HSSFWorkbook(stream);
            }

            ISheet sheet = hssfwb.GetSheetAt(0);
            var dt = new DataTable();
            var lstColIndexes = new List<int>();

            #region read data
            var flagColHeader = true;
            Dictionary<string, int> dicKeyPostion = new Dictionary<string, int>();
            for (int i = 0; i <= sheet.LastRowNum; i++)
            {
                var row = sheet.GetRow(i);

                if (row == null)
                    continue;

                if (i == 0 || (!flagColHeader && i == 1)) //header
                {
                    if (dt.Columns.Count != 0)
                        continue;//already find the cols specified in the first cols

                    string strCols = string.Join(",", cols);
                    int postionIndex = 0;
                    foreach (ICell StringCellValue in row.Cells)
                    {

                        if (StringCellValue != null)
                            StringCellValue.SetCellType(CellType.String);

                        if ((("," + strCols + ",").IndexOf("," + StringCellValue.StringCellValue + ",") != -1))
                        {
                            if (dt.Columns.IndexOf(StringCellValue.StringCellValue) > -1)
                            {
                                err = StringCellValue.StringCellValue + "重复列";
                                return null;
                            }
                            dt.Columns.Add(StringCellValue.StringCellValue);
                            ///保存取值游标存入位置字段集合
                            dicKeyPostion.Add(StringCellValue.StringCellValue, postionIndex);
                        }
                        postionIndex++;
                    }
                    //row.Cells.ForEach(c => (((","+strCols+",").IndexOf(","+c.StringCellValue+",")!=-1)?dt.Columns.Add(c.StringCellValue):""));

                    if (cols.Any())
                    {
                        //check
                        if (cols.Any(col => dt.Columns.IndexOf(col) == -1))
                        {
                            flagColHeader = false;
                            dt = new DataTable();

                            string abnormalColMess = "";
                            cols.ForEach(a =>
                            {
                                if (!dicKeyPostion.ContainsKey(a))
                                {
                                    if (abnormalColMess.IsNullOrEmpty())
                                    {
                                        abnormalColMess = "未找到";
                                    }
                                    abnormalColMess += "【" + a + "】";
                                }
                            });
                            if (!abnormalColMess.IsNullOrEmpty())
                            {
                                err = abnormalColMess;
                            }
                        }
                        else
                        {
                            flagColHeader = true;
                        }
                        if (flagColHeader)
                        {
                            cols.ForEach(col => lstColIndexes.Add(dt.Columns.IndexOf(col)));
                        }
                    }
                    //cols required not found in first row,
                    if (i == 1 && !flagColHeader)
                    {
                        return null;
                    }
                    continue;
                }

                //pick the required cols

                DataRow dataRow = dt.NewRow();

                if (lstColIndexes.Any())
                {
                    //判断row列中的值是否都为空，若都为空则不插入tb 余勇 2014-01-16
                    if (row.Cells.Any(col => col != null && !string.IsNullOrEmpty(col.ToString())))
                    {
                        lstColIndexes.ForEach(colIndex =>
                        {
                            //为避免超出索引错误，需判断row列数是否大于索引
                            if (row.Cells.Count >= colIndex)
                            {
                                if (row.GetCell(dicKeyPostion[cols[colIndex]]) == null)
                                {
                                    dataRow[cols[colIndex]] = "";
                                }
                                else
                                    if (row.GetCell(dicKeyPostion[cols[colIndex]]).CellType == CellType.Numeric)
                                    {//修改取值游标，通过变态获取inedx
                                        dataRow[cols[colIndex]] = row.GetCell(dicKeyPostion[cols[colIndex]]).NumericCellValue;
                                    }
                                    else if (row.GetCell(dicKeyPostion[cols[colIndex]]).CellType == CellType.Formula)
                                    {//EXCEL导入，出现公式的话以数值取值
                                        dataRow[cols[colIndex]] = row.GetCell(dicKeyPostion[cols[colIndex]]).NumericCellValue;
                                    }
                                    else///修改取值游标，通过变态获取inedx
                                        dataRow[cols[colIndex]] = row.GetCell(dicKeyPostion[cols[colIndex]]).StringCellValue;
                            }

                        });
                        dt.Rows.Add(dataRow);
                    }
                }
                else //pick all
                {

                    row.Cells.ForEach(cell =>
                    {

                        if (cell.CellType == CellType.Numeric)
                            dataRow[cell.ColumnIndex] = cell.NumericCellValue;
                        else
                            dataRow[cell.ColumnIndex] = cell.StringCellValue;

                    });
                    dt.Rows.Add(dataRow);
                }
            }
            #endregion

            return dt;
        }


        /// <summary>
        /// 模板导出Excel(多个工作表)
        /// </summary>
        /// <param name="sourceData">源数据</param>
        /// <param name="filePath">模板文件路径(\Templates\Excel\xxx.xls)</param>
        /// <param name="startRowNum">数据开始行数(从0开始)</param>
        /// <param name="fileName">文件名</param>
        /// <param name="dateFormat">日期格式</param>
        /// <param name="isInsertName">是否在表格中插入文件名(默认为true)</param>
        /// <returns>空</returns>
        /// <remarks>2014-05-27 朱家宏 创建</remarks>
        public static void ExportLargeDataFromTemplate<T>(IList<T> sourceData, string filePath, int startRowNum = 1, string fileName = null, string dateFormat = "yyyy-MM-dd HH:mm", bool isInsertName = true)
        {
            if (sourceData == null)
                throw new ArgumentNullException("sourceData");

            const int maxIndex = 65000; //sheet 最大支持行数

            //var dataCount = sourceData.Count();
            //var isLarge = dataCount > maxIndex;
            //var sheetCount = 0;
            //if (isLarge)
            //{
            //    var num = (double)dataCount / (double)maxIndex;
            //    sheetCount = (int) (Math.Ceiling(num)) - 1;
            //}

            //read the template via FileStream, it is suggested to use FileAccess.Read to prevent file lock.
            //book1.xls is an Excel-2007-generated file, so some new unknown BIFF records are added.
            var path = System.Web.HttpContext.Current.Server.MapPath(filePath);
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);

            HSSFWorkbook hssfworkbook = new HSSFWorkbook(fs);
            ISheet sheet = hssfworkbook.GetSheetAt(0) as HSSFSheet;  //黄志勇 2013-12-20 修改
            IRow row = null;
            var count = 0;
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));

            //将数据导入到excel表中
            //set the name and date at the first row
            if (isInsertName)
            {
                sheet.GetRow(0).GetCell(0).SetCellValue(fileName);
            }

            var k = 0;
            var sheetIndex = 1;
            for (int i = 0; i < sourceData.Count; i++)
            {
                if (i != 0 && (i % maxIndex) == 0)
                {
                    //创建sheet
                    sheetIndex++;
                    sheet = hssfworkbook.CreateSheet("sheet" + sheetIndex);
                    startRowNum = 0;
                    k = 0;
                }

                row = sheet.CreateRow(k + startRowNum);//the num start
                count = 0;

                object value = null;
                for (int j = 0; j < properties.Count; j++)
                {
                    ICell cell = row.CreateCell(count++);
                    value = properties[j].GetValue(sourceData[i]);
                    //日期格式导出修改 huangwei 2013-12-23 yyyy-MM-dd HH:ss. //日期格式通过dateFormat参数设置 余勇
                    cell.SetCellValue(value == null ? String.Empty
                        : (value is DateTime ? ((DateTime)value).ToString(dateFormat) : value.ToString()));
                }

                k++;
            }

            //Force excel to recalculate all the formula while open
            sheet.ForceFormulaRecalculation = true;

            //每列宽度自适应
            //for (Int32 i = 0; i < workbook.NumberOfSheets; i++)
            //{
            //    sheet = workbook.GetSheetAt(i);
            //    for (Int32 h = 0; h < headerCount; h++)
            //    {
            //        sheet.AutoSizeColumn(h);
            //    }
            //}

            //定义文件名
            if (string.IsNullOrEmpty(fileName))
                fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            else
                fileName = fileName + ".xls";

            //当前http头信息
            var response = HttpContext.Current.Response;
            response.ContentType = "application/vnd.ms-excel";
            if (HttpContext.Current.Request.UserAgent != null && HttpContext.Current.Request.UserAgent.ToLower().IndexOf("firefox", System.StringComparison.Ordinal) > -1)
            {
                response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", fileName));
            }
            else
            {
                response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8)));
            }
            response.Clear();

            //Write the stream data of workbook to the root directory
            MemoryStream ms = new MemoryStream();
            hssfworkbook.Write(ms);
            ms.WriteTo(response.OutputStream);
            response.End();
        }

        /// <summary>
        /// 导出到Excel(非模板、多个工作表)
        /// </summary>
        /// <param name="sourceData">源数据</param>
        /// <param name="headerList">列名</param>
        /// <param name="fileName">文件名</param>
        /// <param name="dateFormat">日期格式</param>
        /// <returns>空</returns>
        /// <remarks>2014-8-4 余勇 添加</remarks>
        public static void ExportLargeData<T>(IList<T> sourceData, IList<String> headerList = null, string fileName = null, string dateFormat = "yyyy-MM-dd HH:mm")
        {
            if (sourceData == null)
                throw new ArgumentNullException("sourceData");
            const int maxIndex = 65000; //sheet 最大支持行数

            IWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = null;
            IRow row = null;
            int count = 0;
            int startRowNum = 1;

            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            //如果没有自定义的行首,那么采用反射集合的属性名做行首
            var k = 0;
            var sheetIndex = 1;
            int pagecount = sourceData.Count;
            var totalRow = 0;
            var pageSize = 5000;
            ICell cell;
            object value;
            IList<T> list;
            int rows = (int)Math.Ceiling(pagecount / (double)pageSize);
            //先取5000条数据导入
            for (var p = 0; p < rows; p++)
            {
                list = sourceData.Skip(p * pageSize).Take(pageSize).ToList();

                for (int i = 0; i < list.Count; i++)
                {
                    //当为总记录为0或65000的位数时新增sheet
                    if ((totalRow % maxIndex) == 0)
                    {
                        //创建sheet
                        sheet = workbook.CreateSheet("sheet" + sheetIndex);
                        row = sheet.CreateRow(0);
                        count = 0;
                        int headerCount = headerList != null ? headerList.Count : properties.Count;
                        for (int j = 0; j < headerCount; j++) //生成sheet第一行列名 
                        {
                            cell = row.CreateCell(count++);
                            if (headerList == null)
                            {
                                cell.SetCellValue(String.IsNullOrEmpty(properties[j].Description)
                                                      ? properties[j].Name
                                                      : properties[j].Description);
                            }
                            else
                                cell.SetCellValue(headerList[j]);
                        }

                        startRowNum = 1;
                        sheetIndex++;
                        k = 0;
                    }

                    row = sheet.CreateRow(k + startRowNum);//the num start
                    count = 0;

                    value = null;
                    for (int j = 0; j < properties.Count; j++)
                    {
                        cell = row.CreateCell(count++);
                        value = properties[j].GetValue(list[i]);
                        //日期格式导出修改 huangwei 2013-12-23 yyyy-MM-dd HH:ss. //日期格式通过dateFormat参数设置 余勇
                        cell.SetCellValue(value == null ? String.Empty
                            : (value is DateTime ? ((DateTime)value).ToString(dateFormat) : value.ToString()));
                    }
                    totalRow++;
                    k++;
                }
            }


            //每列宽度自适应
            //for (Int32 i = 0; i < workbook.NumberOfSheets; i++)
            //{
            //    sheet = workbook.GetSheetAt(i);
            //    for (Int32 h = 0; h < headerCount; h++)
            //    {
            //        sheet.AutoSizeColumn(h);
            //    }
            //}

            //定义文件名
            if (string.IsNullOrEmpty(fileName))
                fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            else
                fileName = fileName + ".xls";

            //当前http头信息
            var response = HttpContext.Current.Response;
            response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", fileName));
            response.Clear();

            //Write the stream data of workbook to the root directory
            MemoryStream file = new MemoryStream();
            workbook.Write(file);
            file.WriteTo(response.OutputStream);
            response.End();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sourceData"></param>
        /// <param name="headerList"></param>
        /// <param name="fileName"></param>
        /// <param name="dateFormat"></param>
        public static void ExportSoOrders<T>(IList<T> sourceData, IList<String> headerList = null, string fileName = null, string dateFormat = "yyyy-MM-dd HH:mm")
        {
            if (sourceData == null)
                throw new ArgumentNullException("sourceData");
            const int maxIndex = 65000; //sheet 最大支持行数

            IWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = null;
            IRow row = null;
            int count = 0;
            int startRowNum = 1;

            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            //如果没有自定义的行首,那么采用反射集合的属性名做行首
            var k = 0;
            var sheetIndex = 1;
            int pagecount = sourceData.Count;
            var totalRow = 0;
            var pageSize = 5000;
            ICell cell;
            object value;
            IList<T> list;
            int rows = (int)Math.Ceiling(pagecount / (double)pageSize);
            //先取5000条数据导入
            for (var p = 0; p < rows; p++)
            {
                list = sourceData.Skip(p * pageSize).Take(pageSize).ToList();

                for (int i = 0; i < list.Count; i++)
                {
                    //当为总记录为0或65000的位数时新增sheet
                    if ((totalRow % maxIndex) == 0)
                    {
                        //创建sheet
                        sheet = workbook.CreateSheet("sheet" + sheetIndex);
                        row = sheet.CreateRow(0);
                        count = 0;
                        int headerCount = headerList != null ? headerList.Count : properties.Count;
                        for (int j = 0; j < headerCount; j++) //生成sheet第一行列名 
                        {
                            cell = row.CreateCell(count++);
                            if (headerList == null)
                            {
                                cell.SetCellValue(String.IsNullOrEmpty(properties[j].Description)
                                                      ? properties[j].Name
                                                      : properties[j].Description);
                            }
                            else
                                cell.SetCellValue(headerList[j]);
                        }

                        startRowNum = 1;
                        sheetIndex++;
                        k = 0;
                    }

                    row = sheet.CreateRow(k + startRowNum);//the num start
                    count = 0;

                    value = null;
                    for (int j = 0; j < properties.Count; j++)
                    {
                        cell = row.CreateCell(count++);

                        if (properties[j].GetValue(list[i]) != null && (properties[j].GetValue(list[i]).GetType() == typeof(List<int>) || properties[j].GetValue(list[i]).GetType() == typeof(List<string>)))
                        {
                            if (properties[j].GetValue(list[i]).GetType() == typeof(List<string>))
                            {
                                value = ((List<string>)properties[j].GetValue(list[i]))[0];
                            }
                            else
                            {
                                value = ((List<int>)properties[j].GetValue(list[i]))[0];
                            }

                            //日期格式导出修改 huangwei 2013-12-23 yyyy-MM-dd HH:ss. //日期格式通过dateFormat参数设置 余勇
                            cell.SetCellValue(value == null ? String.Empty
                                : (value is DateTime ? ((DateTime)value).ToString(dateFormat) : value.ToString()));
                        }
                        else
                        {

                            value = properties[j].GetValue(list[i]);
                            DateTime time = DateTime.MinValue;
                            var isDataTime = (value != null ? DateTime.TryParse(value.ToString(), out time) : false);
                            //日期格式导出修改 huangwei 2013-12-23 yyyy-MM-dd HH:ss. //日期格式通过dateFormat参数设置 余勇
                            cell.SetCellValue(value == null ? String.Empty
                                : (isDataTime ? time.ToString(dateFormat) : value.ToString()));
                        }
                    }
                    if (properties[1].GetValue(list[i]) != null && (properties[1].GetValue(list[i]).GetType() == typeof(List<int>) || properties[1].GetValue(list[i]).GetType() == typeof(List<string>)))
                    {
                        List<string> _tempProductSysno = (List<string>)properties[1].GetValue(list[i]);
                        List<string> _tempProductName = (List<string>)properties[2].GetValue(list[i]);
                        List<int> _tempProductQuantity = (List<int>)properties[3].GetValue(list[i]);
                        for (int cIndex = 1; cIndex < _tempProductSysno.Count; cIndex++)
                        {
                            row = sheet.CreateRow((++k) + startRowNum);
                            value = _tempProductSysno[cIndex];
                            cell = row.CreateCell(1);// 商品编码
                            cell.SetCellValue(value.ToString());

                            value = _tempProductName[cIndex];
                            cell = row.CreateCell(2);//商品名称
                            cell.SetCellValue(value.ToString());

                            value = _tempProductQuantity[cIndex];
                            cell = row.CreateCell(3);//商品数量
                            cell.SetCellValue(value.ToString());
                        }
                    }
                    totalRow++;
                    k++;
                }
            }


            //每列宽度自适应
            //for (Int32 i = 0; i < workbook.NumberOfSheets; i++)
            //{
            //    sheet = workbook.GetSheetAt(i);
            //    for (Int32 h = 0; h < headerCount; h++)
            //    {
            //        sheet.AutoSizeColumn(h);
            //    }
            //}

            //定义文件名
            if (string.IsNullOrEmpty(fileName))
                fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            else
                fileName = fileName + ".xls";

            //当前http头信息
            var response = HttpContext.Current.Response;
            response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", fileName));
            response.Clear();

            //Write the stream data of workbook to the root directory
            MemoryStream file = new MemoryStream();
            workbook.Write(file);
            file.WriteTo(response.OutputStream);
            response.End();
        }

        /// <summary>
        ///  导出带商品图片Excel表（用于信营）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sourceData">导出数据订单</param>
        /// <param name="headerList">第一行头数据</param>
        /// <param name="fileName">导出Excel文件名</param>
        /// <param name="dateFormat">日期格式</param>
        /// <param name="Filepath">配置图片服务器链接</param> 
        /// <remarks>2016-8-4 罗远康 添加</remarks>
        public static void ExportSoOrdersPic<T>(IList<T> sourceData, IList<String> headerList = null, string fileName = null, string dateFormat = "yyyy-MM-dd HH:mm", string Filepath = null)
        {
            if (sourceData == null)
                throw new ArgumentNullException("sourceData");
            const int maxIndex = 65000; //sheet 最大支持行数

            IWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = null;
            IRow row = null;
            int count = 0;
            int startRowNum = 1;

            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            //如果没有自定义的行首,那么采用反射集合的属性名做行首
            var k = 0;
            var sheetIndex = 1;
            int pagecount = sourceData.Count;
            var totalRow = 0;
            var pageSize = 5000;
            ICell cell;
            object value;
            IList<T> list;
            int rows = (int)Math.Ceiling(pagecount / (double)pageSize);
            //先取5000条数据导入
            for (var p = 0; p < rows; p++)
            {
                list = sourceData.Skip(p * pageSize).Take(pageSize).ToList();

                for (int i = 0; i < list.Count; i++)
                {
                    //当为总记录为0或65000的位数时新增sheet
                    if ((totalRow % maxIndex) == 0)
                    {
                        //创建sheet
                        sheet = workbook.CreateSheet("sheet" + sheetIndex);
                        row = sheet.CreateRow(0);
                        count = 0;
                        int headerCount = headerList != null ? headerList.Count : properties.Count;
                        for (int j = 0; j < headerCount; j++) //生成sheet第一行列名 
                        {
                            cell = row.CreateCell(count++);
                            if (headerList == null)
                            {
                                cell.SetCellValue(String.IsNullOrEmpty(properties[j].Description)
                                                      ? properties[j].Name
                                                      : properties[j].Description);
                            }
                            else
                                cell.SetCellValue(headerList[j]);
                        }

                        startRowNum = 1;
                        sheetIndex++;
                        k = 0;
                    }
                    row = sheet.CreateRow(k + startRowNum);//the num start
                    row.HeightInPoints = 20;//设置行高 
                    count = 0;
                    value = null;
                    for (int j = 0; j < properties.Count; j++)
                    {
                        cell = row.CreateCell(count++);

                        if (properties[j].GetValue(list[i]) != null && (properties[j].GetValue(list[i]).GetType() == typeof(List<int>) || properties[j].GetValue(list[i]).GetType() == typeof(List<string>)))
                        {
                            if (properties[j].GetValue(list[i]).GetType() == typeof(List<string>))
                            {
                                value = ((List<string>)properties[j].GetValue(list[i]))[0];
                            }
                            else
                            {
                                value = ((List<int>)properties[j].GetValue(list[i]))[0];
                            }




                            //日期格式导出修改 huangwei 2013-12-23 yyyy-MM-dd HH:ss. //日期格式通过dateFormat参数设置 余勇
                            cell.SetCellValue(value == null ? String.Empty
                                : (value is DateTime ? ((DateTime)value).ToString(dateFormat) : value.ToString()));
                        }
                        //else if (j == 17)//商品图片
                        //{
                        //    sheet.SetColumnWidth(j, 12 * 256);//设置宽
                        //    string imgUrl = properties[j].GetValue(list[i]).ToString();
                        //    string Fileurl = "Small";
                        //    string FileName = string.Format(imgUrl, Filepath, Fileurl);
                        //    WebClient w = new WebClient();
                        //    byte[] bytes = w.DownloadData(FileName);//下载网络图片生成BYTE
                        //    //byte[] bytes = System.IO.File.ReadAllBytes(FileName);//只能打开本地图片
                        //    if (!string.IsNullOrEmpty(FileName))
                        //    {
                        //        int pictureIdx = workbook.AddPicture(bytes, NPOI.SS.UserModel.PictureType.JPEG);
                        //        HSSFPatriarch patriarch = (HSSFPatriarch)sheet.CreateDrawingPatriarch();
                        //        HSSFClientAnchor anchor = new HSSFClientAnchor(0, 0, 50, 25, j, i + 1, j + 1, i + 2);
                        //        //##处理照片位置，左上角（X轴，Y轴）；右下角（X轴，Y轴）；列1，行1，列2，行2

                        //        HSSFPicture pict = (HSSFPicture)patriarch.CreatePicture(anchor, pictureIdx);
                        //        // pict.Resize();用于图片原始大小来显示
                        //    }
                        //}
                        else
                        {
                            value = properties[j].GetValue(list[i]);
                            //日期格式导出修改 huangwei 2013-12-23 yyyy-MM-dd HH:ss. //日期格式通过dateFormat参数设置 余勇
                            cell.SetCellValue(value == null ? String.Empty
                                : (value is DateTime ? ((DateTime)value).ToString(dateFormat) : value.ToString()));
                        }
                    }
                    if (properties[1].GetValue(list[i]) != null && (properties[1].GetValue(list[i]).GetType() == typeof(List<int>) || properties[1].GetValue(list[i]).GetType() == typeof(List<string>)))
                    {
                        List<string> _tempProductSysno = (List<string>)properties[1].GetValue(list[i]);
                        List<string> _tempProductName = (List<string>)properties[2].GetValue(list[i]);
                        List<int> _tempProductQuantity = (List<int>)properties[3].GetValue(list[i]);
                        for (int cIndex = 1; cIndex < _tempProductSysno.Count; cIndex++)
                        {
                            row = sheet.CreateRow((++k) + startRowNum);
                            value = _tempProductSysno[cIndex];
                            cell = row.CreateCell(1);// 商品编码
                            cell.SetCellValue(value.ToString());

                            value = _tempProductName[cIndex];
                            cell = row.CreateCell(2);//商品名称
                            cell.SetCellValue(value.ToString());

                            value = _tempProductQuantity[cIndex];
                            cell = row.CreateCell(3);//商品数量
                            cell.SetCellValue(value.ToString());
                        }
                    }
                    totalRow++;
                    k++;
                }
            }
            //定义文件名
            if (string.IsNullOrEmpty(fileName))
                fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            else
                fileName = fileName + ".xls";

            //当前http头信息
            var response = HttpContext.Current.Response;
            response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", fileName));
            response.Clear();

            //Write the stream data of workbook to the root directory
            MemoryStream file = new MemoryStream();
            workbook.Write(file);
            file.WriteTo(response.OutputStream);
            response.End();
        }

        //public static void ExportDealerRebatesRecord<T1>(List<global::Hyt.Model.Transfer.CBOutputDealerRebatesRecord> exportOrders, List<string> list, string fileName)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
