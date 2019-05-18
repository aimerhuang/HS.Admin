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
    /// <summary>
    /// 转运系统商品档案
    /// </summary>
    /// <remarks>
    /// 2016-5-17 杨云奕 添加
    /// </remarks>
    public class DsWhProductBo : BOBase<DsWhProductBo>
    {
        /// <summary>
        /// 添加商品档案
        /// </summary>
        /// <param name="Mod"></param>
        /// <returns></returns>
        public  int InsertMod(DsWhProduct Mod)
        {
            return IDsWhProductDao.Instance.InsertMod(Mod);
        }
        /// <summary>
        /// 更新商品档案
        /// </summary>
        /// <param name="Mod"></param>
        public void UpdateMod(DsWhProduct Mod)
        {
            IDsWhProductDao.Instance.UpdateMod(Mod);
        }
        /// <summary>
        /// 删除商品档案数据
        /// </summary>
        /// <param name="SysNo"></param>
        public void DeleteBySysNo(int SysNo)
        {
            IDsWhProductDao.Instance.DeleteBySysNo(SysNo);
        }
        /// <summary>
        /// 通过自动编号获取商品档案
        /// </summary>
        /// <param name="SysNo">自动编号</param>
        /// <returns></returns>
        public DsWhProduct GetProductModBySysNo(int SysNo)
        {
            return IDsWhProductDao.Instance.GetProductModBySysNo(SysNo);
        }
        /// <summary>
        /// 通过商品编号获取商品档案数据
        /// </summary>
        /// <param name="GoodsCode"></param>
        /// <returns></returns>
        public DsWhProduct GetProductModByGoodsCode(string GoodsCode)
        {
            return IDsWhProductDao.Instance.GetProductModByGoodsCode(GoodsCode);
        }
        /// <summary>
        /// 通过自动编号集合获取商品档案集合
        /// </summary>
        /// <param name="SysNos"></param>
        /// <returns></returns>
        public List<DsWhProduct> GetProductModBySysNos(List<int> SysNos)
        {
            return IDsWhProductDao.Instance.GetProductModBySysNos(SysNos);
        }
        /// <summary>
        /// 通过货品编号获取商品档案集合
        /// </summary>
        /// <param name="GoodsCodes"></param>
        /// <returns></returns>
        public List<DsWhProduct> GetProductModByGoodsCodes(List<string> GoodsCodes)
        {
            return IDsWhProductDao.Instance.GetProductModByGoodsCodes(GoodsCodes);
        }
        /// <summary>
        /// 分页搜索商品数据
        /// </summary>
        /// <param name="pageProList"></param>
        public void DoDsWhProductQuery(ref Model.Pager<CBDsWhProduct> pageProList)
        {
            IDsWhProductDao.Instance.DoDsWhProductQuery(ref pageProList);
        }



        /// <summary>
        /// import cols mapping between DB and excel
        /// </summary>
        /// <remarks>2015-09-10 王耀发 创建</remarks>
        private static readonly Dictionary<string, string> DicColsMapping = new Dictionary<string, string>
            {
                {"ProCode", "商品编码"},
                {"HsCode", "HS编码"},
                {"ProductName", "中文商品名称"},
                {"ProductEName", "英文商品名称"},
                {"BrandName", "品牌"},
                {"ProSpec", "规格"},
                {"ProUnit","单位"},
                {"ProWeight","重量"},
                {"ProOrigin","产地"},
                {"ProPrice","价格"},
                {"Currency","货币"}
            };
        /// <summary>
        /// 导入商品档案
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="SyUserNo"></param>
        /// <param name="DealerSysNo"></param>
        /// <param name="CusCode"></param>
        /// <returns></returns>
        public Model.Result ImportExcel(System.IO.Stream stream, int SyUserNo,int DealerSysNo, string CusCode)
        {
            DataTable dt = null;

            var cols = DicColsMapping.Select(p => p.Value).ToArray();
            var existlst = new List<DsWhProduct>();
            try
            {
                dt = ExcelUtil.ImportExcel(stream, cols);
            }
            catch (Exception ex)
            {
                //exception happened,some not caughted
                return new Model.Result
                {
                    Message = string.Format("数据导入错误,请选择正确的excel文件"),
                    Status = false
                };
            }
            if (dt == null)
            {
                //not all the cols mapped
                return new Model.Result
                {
                    Message = string.Format("请选择正确的excel文件!"),
                    Status = false
                };
            }

            List<DsWhProduct> productList = DsWhProductBo.Instance.GetProductModByDsSysNo(DealerSysNo);
            List<DsWhProduct> saveOrupdateList = new List<DsWhProduct>();
            foreach (DataRow row in dt.Rows)
            {
                decimal price = 0;
                decimal.TryParse(row[DicColsMapping["ProPrice"]].ToString().Trim(),out price);
                decimal weight = 0;
                decimal.TryParse(row[DicColsMapping["ProWeight"]].ToString().Trim(), out weight);
                int SysNo=0;
                DsWhProduct temppro=productList.Find(p=>p.ProCode== row[DicColsMapping["ProCode"]].ToString().Trim());
                if(temppro!=null)
                {
                    SysNo=temppro.SysNo;
                }
                DsWhProduct pro = new DsWhProduct() {
                    Currency = row[DicColsMapping["Currency"]].ToString().Trim(),
                    BrandName = row[DicColsMapping["BrandName"]].ToString().Trim(),
                    CustomerCode = CusCode,
                    DsSysNo = DealerSysNo,
                    HsCode = row[DicColsMapping["HsCode"]].ToString().Trim(),
                    ProCode = row[DicColsMapping["ProCode"]].ToString().Trim(),
                    ProductEName = row[DicColsMapping["ProductEName"]].ToString().Trim(),
                    ProductName = row[DicColsMapping["ProductName"]].ToString().Trim(),
                    ProOrigin = row[DicColsMapping["ProOrigin"]].ToString().Trim(),
                    ProSpec = row[DicColsMapping["ProSpec"]].ToString().Trim(),
                    ProUnit = row[DicColsMapping["ProUnit"]].ToString().Trim(),
                    ProPrice = price,
                    ProWeight = weight,
                    SysNo = SysNo
                };
                saveOrupdateList.Add(pro);
            }

            foreach (var pro in saveOrupdateList)
            {
                if (pro.SysNo > 0)
                {
                    UpdateMod(pro);
                }
                else
                {
                    InsertMod(pro);
                }
            }

            return new Model.Result
            {
                Message = "操作成功，通过商品编码检查导入的商品新增更新与否",
                Status = true
            };
        }

        private List<DsWhProduct> GetProductModByDsSysNo(int DealerSysNo)
        {
            return IDsWhProductDao.Instance.GetProductModByDsSysNo(DealerSysNo);
        }

        public List<DsWhProduct> GetList(int DealerSysNo)
        {
            return IDsWhProductDao.Instance.GetList(DealerSysNo);
        }
    }
}
