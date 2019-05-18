using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hyt.BLL.Base;
using System.IO;
using Hyt.Util;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using Hyt.DataAccess.Base;
using Hyt.BLL.Product;
namespace Hyt.UnitTest
{
    [TestClass]
    public class ExcelTest : DaoBase<ExcelTest>
    {
        public ExcelTest()
        {
            DataProviderBo.Set(Activator.CreateInstance(Type.GetType("Hyt.DataAccess.Oracle.DataProvider,Hyt.DataAccess.Oracle")));
        }

        /// <summary>
        /// 导入商品测试
        /// </summary>
        [TestMethod]
        public void ExcelImport()
        {  
            var dicColsMapping = new Dictionary<string, string>
            {
                {"SysNo", "自动编码"},
                {"ErpCode", "商品编码"}            
            };
            string excelFileName = "商品new.xls";
            string fileName = Hyt.Util.WebUtil.GetMapPath("/excel/") + excelFileName;
            //Stream stream=new 
            
            var fs = new FileStream(fileName,FileMode.Open, FileAccess.Read, FileShare.Read);

            DataTable dt = ExcelUtil.ImportExcel(fs,dicColsMapping.Values.ToArray());

            foreach (DataRow row in dt.Rows)
            {
                int sysNo = 0;
                if (int.TryParse(row[dicColsMapping["SysNo"]].ToString(), out sysNo))
                {
                    string _sysNo=row[dicColsMapping["SysNo"]].ToString();
                    string erpCode = row[dicColsMapping["ErpCode"]].ToString().Trim();
                    //Context.Sql("update PdProduct set ErpCode='" + erpCode + "' where sysNo=" + sysNo).Execute();
                }
               
               
            }
            
        }
         /// <summary>
         /// 导入商品测试
         /// </summary>
        [TestMethod]
        public void ImportXinYingExcel()
        {
            string date = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string excelFileName = "商品(20170106160353).xls";
            string fileName = Hyt.Util.WebUtil.GetMapPath("/excel/") + excelFileName;
            //Stream stream=new 
            
            var fs = new FileStream(fileName,FileMode.Open, FileAccess.Read, FileShare.Read);
            PdProductBo.Instance.ImportXinYingExcel(fs,1);
        }
    }
}
