using Hyt.DataAccess.Transport;
using Hyt.Model.Transport;
using Hyt.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Hyt.BLL.Transport
{
    public class DsWhLogisticsNumberBo : BOBase<DsWhLogisticsNumberBo>
    {

        public  int InsertMod(Model.Transport.DsWhLogisticsNumber mod)
        {
            return IDsWhLogisticsNumberDao.Instance.InsertMod(mod);
        }

        public  void UpdataMod(Model.Transport.DsWhLogisticsNumber mod)
        {
            IDsWhLogisticsNumberDao.Instance.UpdataMod(mod);
        }

        public Model.Transport.DsWhLogisticsNumber GetLogisticsNumberByNotUsed(string type)
        {
            return IDsWhLogisticsNumberDao.Instance.GetLogisticsNumberByNotUsed(type);
        }

        public  List<Model.Transport.DsWhLogisticsNumber> GetAllLogisticsNumberByServiceType(string type)
        {
            return IDsWhLogisticsNumberDao.Instance.GetAllLogisticsNumberByServiceType(type);
        }

        public List<Model.Transport.DsWhLogisticsNumber> GetLogisticsNumberByNotUsed(Dictionary<string,int> dicType)
        {
            return IDsWhLogisticsNumberDao.Instance.GetLogisticsNumberByNotUsed(dicType);
        }

        public List<Model.Transport.CBDsWhLogisticsNumber> GetLogisticsNumberListByCodeList(List<string> listNumber)
        {
            return IDsWhLogisticsNumberDao.Instance.GetLogisticsNumberListByCodeList(listNumber);
        }

        /// <summary>
        /// import cols mapping between DB and excel
        /// </summary>
        /// <remarks>2015-09-10 王耀发 创建</remarks>
        private static readonly Dictionary<string, string> DicColsMapping = new Dictionary<string, string>
            {
                {"ProCode", "物流编码"}
            };
        /// <summary>
        /// 导入商品档案
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="SyUserNo"></param>
        /// <param name="DealerSysNo"></param>
        /// <param name="CusCode"></param>
        /// <returns></returns>
        public Model.Result<List<string>> ImportExcel(System.IO.Stream stream, int CurrentSysNo, int DsSysNo, string CusCode)
        {
            DataTable dt = null;

            var cols = DicColsMapping.Select(p => p.Value).ToArray();
            
            try
            {
                dt = ExcelUtil.ImportExcel(stream, cols);
            }
            catch (Exception ex)
            {
                //exception happened,some not caughted
                return new Model.Result<List<string>>
                {
                    Message = string.Format("数据导入错误,请选择正确的excel文件"),
                    Status = false
                };
            }
            if (dt == null)
            {
                //not all the cols mapped
                return new Model.Result<List<string>>
                {
                    Message = string.Format("请选择正确的excel文件!"),
                    Status = false
                };
            }
            Model.Result<List<string>> modResult = new Model.Result<List<string>>();
            modResult.Status=true;
            modResult.Message = "成功！";
            modResult.Data = new List<string>();
            foreach (DataRow row in dt.Rows)
            {
                modResult.Data.Add(row[DicColsMapping["ProCode"]].ToString().Trim());
            }
            return modResult;
        }
    }
}
