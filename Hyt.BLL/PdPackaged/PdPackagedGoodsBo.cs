using Hyt.BLL.Warehouse;
using Hyt.BLL.Web;
using Hyt.DataAccess.PdPackaged;
using Hyt.Model;
using Hyt.Model.PdPackaged;
using Hyt.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Hyt.BLL.PdPackaged
{
    /// <summary>
    /// 商品套装
    /// </summary>
    public class PdPackagedGoodsBo : BOBase<PdPackagedGoodsBo>
    {
        /// <summary>
        /// 分页获取盘点作业单
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        /// 2017-8-25
        public Pager<PdPackagedGoods> GetPageList(Pager<PdPackagedGoods> pager, int? GetType)
        {
            return IPdPackagedGoodsDao.Instance.GetPageList(pager, GetType);
        }

        /// <summary>
        /// 获取套装商品
        /// </summary>
        /// <param name="pager"></param>
        /// <param name="GetType">是否获取商品明细 1是 0否</param>
        /// <returns></returns>
        /// 2017-8-25 吴琨 创建
        public PdPackagedGoods GetPageModels(int SysNo, int? GetType = 0)
        {
            return IPdPackagedGoodsDao.Instance.GetPageModels(SysNo, GetType);
        }

        /// <summary>
        /// 创建套装商品
        /// </summary>
        /// <param name="model">套装商品表</param>
        /// <param name="listModel">套装商品商品明细表</param>
        /// <returns></returns>
        /// 2017-8-25 吴琨 创建
        public  bool Add(PdPackagedGoods model, List<PdPackagedGoodsEntry> listModel)
        {
            return IPdPackagedGoodsDao.Instance.Add(model, listModel);
        }

        /// <summary>
        /// 更改套装商品状态
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <param name="status">状态值</param>
        /// <returns></returns>
        /// 吴琨 创建
        public bool UpdateStatus(int sysNo, int status, int Auditor, string AuditorName)
        {
            return IPdPackagedGoodsDao.Instance.UpdateStatus(sysNo, status, Auditor, AuditorName);
        }


        /// <summary>
        /// 生成单据编号
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <param name="status">状态值</param>
        /// <returns></returns>
        /// 吴琨 2017/8/29 创建
        public string GetSetCode()
        {
            string sysno = IPdPackagedGoodsDao.Instance.GetModelSysNo().ToString();
            switch (sysno.Length)
            {
                case 1:
                    return "TZSP000" + sysno;
                case 2:
                    return "TZSP00" + sysno;
                case 3:
                    return "TZSP0" + sysno;
                case 4:
                    return "TZSP" + sysno;
                default:
                    return "TZSP" + sysno;
            }
        }


        #region 导入Excel
        #region 套装商品数据导入Excel 2017-08-25 吴琨 创建
        /// <summary>
        /// 套装商品数据导入Excel
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        ///  2017-08-25 吴琨 创建
        public Resuldt<PdPackagedGoodsEntry> ImportExcel(System.IO.Stream stream, int SysNo)
        {
            DataTable dt = null;
            var cols = DicColsMapping.Select(p => p.Value).ToArray();
            #region 基础验证
            try
            {
                dt = ExcelUtil.ImportExcel(stream, cols);
            }
            catch (Exception ex)
            {
                return new Resuldt<PdPackagedGoodsEntry>
                {
                    Message = string.Format("数据导入错误,请选择正确的excel文件"),
                    Status = false
                };
            }
            if (dt == null)
            {
                //not all the cols mapped
                return new Resuldt<PdPackagedGoodsEntry>
                {
                    Message = string.Format("请选择正确的excel文件!"),
                    Status = false
                };
            }
            if (dt.Rows.Count == 0)
            {
                return new Resuldt<PdPackagedGoodsEntry>
                {
                    Message = "导入的数据为空!",
                    Status = false
                };
            }
            #endregion
            Resuldt<PdPackagedGoodsEntry> run = new Resuldt<PdPackagedGoodsEntry>();
            List<PdPackagedGoodsEntry> listModel = new List<PdPackagedGoodsEntry>();
            int fail = 0;//失败记录数
            int success = 0; //成功记录数
            string failstr = ""; //失败条数记录
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                success++;
                PdPackagedGoodsEntry model = new PdPackagedGoodsEntry();
                model.PdCode = dt.Rows[i]["商品代码"].ToString();
                if (!string.IsNullOrEmpty(model.PdCode))
                {
                    var product = PdProductBo.Instance.GetProductErpCode(dt.Rows[i]["商品代码"].ToString().Trim(), null);
                    if (product != null)
                    {
                        model.PdSysNo = product.SysNo;
                        model.PdName = product.EasName;
                    }
                    else
                    {
                        fail++;
                        failstr += (i + 2) + "、";
                        dt.Rows.Remove(dt.Rows[i]);
                        continue;
                    }
                }
                model.Company = dt.Rows[i]["单位"].ToString();
                model.UnitPrice = Convert.ToDecimal(dt.Rows[i]["单价"]);
                model.Count = Convert.ToDecimal(dt.Rows[i]["用量"]);
                #region 发料仓库信息
                model.WarehouseCode = dt.Rows[i]["发料仓库"].ToString();
                if (!string.IsNullOrEmpty(model.WarehouseCode))
                {
                    var WhWarehouse = Hyt.BLL.Web.WhWarehouseBo.Instance.GetModelErpCode(model.WarehouseCode);
                    if (WhWarehouse != null)
                    {
                        model.WarehouseSysNo = WhWarehouse.SysNo;
                        model.WarehouseName = WhWarehouse.BackWarehouseName;
                    }
                    else
                    {
                        fail++;
                        failstr += (i + 2) + "、";
                        dt.Rows.Remove(dt.Rows[i]);
                        continue;
                    }
                }
                #endregion
                model.Remarks = dt.Rows[i]["备注"].ToString();
                if (model.PdSysNo > 0 && model.WarehouseSysNo > 0)
                {
                    listModel.Add(model);
                }
            }
            if (success > 0 && dt.Rows.Count > 0) run.Data = null;
            if (success > 0 && listModel != null) run.listModel = listModel;
            run.Message = "导入成功" + success + "件商品,失败" + fail + "件商品;";
            if (fail > 0) run.Message += "失败原因为:产品编码或仓库erp编码有误,不存在此件商品。失败条数为第" + failstr.Trim('、') + "条。";
            run.Status = true;
            return run;
        }

        /// <summary>
        /// import cols mapping between DB and excel
        /// </summary>
        /// <remarks>2017-08-15 吴琨 创建</remarks>
        private static readonly Dictionary<string, string> DicColsMapping = new Dictionary<string, string>
        {
            {"PdCode", "商品代码"},
            {"PdName", "商品名称"},
            {"Company", "单位"},
            {"UnitPrice", "单价"},
            {"Count", "用量"},
            {"WarehouseCode", "发料仓库"},
            {"Remarks", "备注"}
        };
        #endregion
        #endregion
    }
}
