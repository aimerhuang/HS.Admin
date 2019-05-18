using Hyt.BLL.Log;
using Hyt.BLL.Warehouse;
using Hyt.BLL.Web;
using Hyt.DataAccess.InventorySheet;
using Hyt.Model;
using Hyt.Model.Generated;
using Hyt.Model.InventorySheet;
using Hyt.Model.WorkflowStatus;
using Hyt.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Hyt.BLL.InventorySheet
{
    /// <summary>
    /// 盘点作业单
    /// </summary>
    public class WhInventoryBo : BOBase<WhInventoryBo>
    {

        /// <summary>
        /// 分页获取盘点作业单
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        /// 2017-8-07
        public Pager<Hyt.Model.InventorySheet.WhInventory> GetSoOrders(Pager<Hyt.Model.InventorySheet.WhInventory> pager)
        {
            return IWhInventoryDao.Instance.GetSoOrders(pager);
        }

        /// <summary>
        /// 创建盘点作业
        /// </summary>
        /// <param name="model">盘点实体</param>
        /// <param name="productModel">盘点商品实体</param>
        /// <returns></returns>
        public int AddWhInventory(Hyt.Model.InventorySheet.WhInventory model, List<WhInventoryProduct> productModel)
        {
            return IWhInventoryDao.Instance.AddWhInventory(model, productModel);
        }


        /// <summary>
        /// 查询当天的盘点单总数
        /// </summary>
        /// <returns></returns>
        public int GetWhInventoryCount()
        {
            return IWhInventoryDao.Instance.GetWhInventoryCount();
        }

        /// <summary>
        /// 根据盘点单系统编号获取明细
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        public Hyt.Model.InventorySheet.WhInventoryDetail GetWhInventoryDetail(int sysNo)
        {
            return IWhInventoryDao.Instance.GetWhInventoryDetail(sysNo);
        }

        /// <summary>
        /// 根据盘点单系统编号获取明细
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        public Pager<Hyt.Model.InventorySheet.WhInventoryDetail> GetWhInventoryDetail(int PageIndex, int sysNo)
        {
            return IWhInventoryDao.Instance.GetWhInventoryDetail(PageIndex, sysNo);
        }

        /// <summary>
        /// 根据仓库id查询仓库名称
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        public string GetWhWarehouseName(int sysNo)
        {
            return IWhInventoryDao.Instance.GetWhWarehouseName(sysNo);
        }

        /// <summary>
        /// 根据商品编号查询商品名称
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        public PdProduct GetProductName(int sysNo)
        {
            return IWhInventoryDao.Instance.GetProductName(sysNo);
        }

        /// <summary>
        /// 根据品牌编号查询品牌名称
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        public string GetBrandName(int sysNo)
        {
            return IWhInventoryDao.Instance.GetBrandName(sysNo);
        }


        /// <summary>
        /// 更新盘点库存
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        public bool UploadPDQuantity(int sysNo, decimal Quantity, decimal ZhangCunQuantity)
        {
            return IWhInventoryDao.Instance.UploadPDQuantity(sysNo, Quantity, ZhangCunQuantity);
        }


        /// <summary>
        /// 更新调整数量/实际库存
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        public bool UploadSJQuantity(int sysNo, decimal Quantity)
        {
            return IWhInventoryDao.Instance.UploadSJQuantity(sysNo, Quantity);
        }


        /// <summary>
        /// 更新盘点单状态
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        public bool UploadStatus(int sysNo, int status)
        {
            return IWhInventoryDao.Instance.UploadStatus(sysNo, status);
        }

        /// <summary>
        /// 生成盘点报告单
        /// </summary>
        /// <returns></returns>
        public bool AddWhInventoryRepor(WhInventoryRepor reporModl, List<WhIReporPrDetails> productModel)
        {
            return IWhInventoryDao.Instance.AddWhInventoryRepor(reporModl, productModel);
        }


        /// <summary>
        /// 分页获取盘点报告单
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        /// 2017-8-07
        public Pager<WhInventoryRepor> GetWhInventoryReporPage(Pager<WhInventoryRepor> pager)
        {
            return IWhInventoryDao.Instance.GetWhInventoryReporPage(pager);
        }


        /// <summary>
        /// 根据id获取盘点报告单
        /// </summary>
        /// <returns></returns>
        public WhInventoryRepor GetWhInventoryRepor(int sysNo)
        {
            return IWhInventoryDao.Instance.GetWhInventoryRepor(sysNo);
        }


        /// <summary>
        /// 根据id获取盘点报告单明细
        /// </summary>
        /// <returns></returns>
        public WhInventoryRepor GetWhInventoryReporModel(int sysNo, int PageType)
        {
            return IWhInventoryDao.Instance.GetWhInventoryReporModel(sysNo, PageType);
        }


        /// <summary>
        /// 根据id获取是否已生成了盈亏报告单
        /// </summary>
        /// <param name="sysNo">盘点Code</param>
        /// <param name="status">盈亏状态  1盈 2亏</param>
        /// <returns></returns>
        public bool GetIsWhInventoryRepor(string Code, int status)
        {
            return IWhInventoryDao.Instance.GetIsWhInventoryRepor(Code, status);
        }


        /// <summary>
        /// 更新盘点报告单状态
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        public bool UploadWhInventoryReporStatus(int sysNo, int status)
        {
            return IWhInventoryDao.Instance.UploadWhInventoryReporStatus(sysNo, status);
        }


        /// <summary>
        /// 更新盘点报告单
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        public bool UploadWhInventoryRepor(WhInventoryRepor model)
        {
            return IWhInventoryDao.Instance.UploadWhInventoryRepor(model);
        }


        /// <summary>
        /// 根据盘点报告单系统编号 查询盘点商品报告单列表
        /// </summary>
        /// <param name="sysNo">盘点单id</param>
        /// <returns></returns>
        public List<WhIReporPrDetails> GetWhIReporPrDetailsPid(int sysNo, int status)
        {
            return IWhInventoryDao.Instance.GetWhIReporPrDetailsPid(sysNo, status);
        }


        /// <summary>
        /// 更新产品库存
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdatePdProductStock(List<WhIReporPrDetails> model)
        {
            return IWhInventoryDao.Instance.UpdatePdProductStock(model);
        }


        /// <summary>
        /// 根据商品id获取商品和商品对应仓库信息
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public List<uditPdProductStock> GetuditPdProductStock(int SysNo, int? whSysId)
        {
            return IWhInventoryDao.Instance.GetuditPdProductStock(SysNo, whSysId);
        }



        #region 商品导入Excel 2017-08-15 吴琨 创建
        public Resuldt<uditPdProductStock> ImportExcel(System.IO.Stream stream, int SysNo)
        {
            DataTable dt = null;
            var cols = DicColsMapping.Select(p => p.Value).ToArray();
            try
            {
                dt = ExcelUtil.ImportExcel(stream, cols);
            }
            catch (Exception ex)
            {
                return new Resuldt<uditPdProductStock>
                {
                    Message = string.Format("数据导入错误,请选择正确的excel文件"),
                    Status = false
                };
            }
            if (dt == null)
            {
                //not all the cols mapped
                return new Resuldt<uditPdProductStock>
                {
                    Message = string.Format("请选择正确的excel文件!"),
                    Status = false
                };
            }

            if (dt.Rows.Count == 0)
            {
                return new Resuldt<uditPdProductStock>
                {
                    Message = "导入的数据为空!",
                    Status = false
                };
            }

            Resuldt<uditPdProductStock> run = new Resuldt<uditPdProductStock>();
            List<uditPdProductStock> listModel = new List<uditPdProductStock>();
            int fail = 0;//失败记录数
            int success = 0; //成功记录数
            string failstr = ""; //失败条数记录
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                uditPdProductStock model = IWhInventoryDao.Instance.GetuditPdProductStockTo(dt.Rows[i]["商品代码"].ToString().Trim(), dt.Rows[i]["仓库Erp编号"].ToString().Trim());
                if (model == null)
                {
                    model = new uditPdProductStock();
                    var product = PdProductBo.Instance.GetProductErpCode(dt.Rows[i]["商品代码"].ToString().Trim(), null);
                    var ware = Hyt.BLL.Web.WhWarehouseBo.Instance.GetModelErpCode(dt.Rows[i]["仓库Erp编号"].ToString().Trim());
                    if (product == null || ware == null)
                    {
                        fail++;
                        failstr += (i + 2) + "、";
                        dt.Rows.Remove(dt.Rows[i]);
                    }
                    else
                    {
                        #region 将商品插入至商品库存表
                        // WhInventoryBo
                        PdProductStock stockModel = new PdProductStock()
                        {
                            WarehouseSysNo = ware.SysNo,
                            PdProductSysNo = product.SysNo,
                            StockQuantity = 0,
                            LockStockQuantity=0,
                            CreatedDate = DateTime.Now,
                            Barcode = product.Barcode,
                            CostPrice = 0,
                            CustomsNo = "",
                            InStockTime = "",
                            Remark = "库存盘点自动入库"
                        };
                        var r = PdProductStockBo.Instance.SaveProductStock(stockModel, 0);
                        #endregion
                        if (r > 0)
                        {
                            model = IWhInventoryDao.Instance.GetuditPdProductStockTo(dt.Rows[i]["商品代码"].ToString().Trim(), dt.Rows[i]["仓库Erp编号"].ToString().Trim());
                            success++;
                            model.BeiZhu = dt.Rows[i]["备注"] == null ? "" : dt.Rows[i]["备注"].ToString().Trim();
                            model.ShiQuantity = dt.Rows[i]["实存数"] == null ? 0 : Convert.ToDecimal(dt.Rows[i]["实存数"].ToString().Trim());
                            model.WhCostPrice = dt.Rows[i]["单价"] == null ? 0 : Convert.ToDecimal(dt.Rows[i]["单价"].ToString().Trim());
                            listModel.Add(model);
                        }
                        else
                        {
                            fail++;
                            failstr += (i + 2) + "、";
                            dt.Rows.Remove(dt.Rows[i]);
                        }
                    }
                }
                else
                {
                    success++;
                    model.BeiZhu = dt.Rows[i]["备注"] == null ? "" : dt.Rows[i]["备注"].ToString().Trim();
                    model.ShiQuantity = dt.Rows[i]["实存数"] == null ? 0 : Convert.ToDecimal(dt.Rows[i]["实存数"].ToString().Trim());
                    model.WhCostPrice = dt.Rows[i]["单价"] == null ? 0 : Convert.ToDecimal(dt.Rows[i]["单价"].ToString().Trim());
                    listModel.Add(model);
                }
            }
            if (success > 0 && dt.Rows.Count > 0) run.Data = dt;
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
                {"PrErpCode", "商品代码"},
                {"PrEasName", "商品名称"},
                {"WhBackWarehouseName", "仓库名称"},
                {"WhErpCode", "仓库Erp编号"},
                {"WhStockQuantity", "帐存数"},
                {"ShiQuantity", "实存数"},
                {"WhCostPrice", "单价"},
                {"YingQuantity", "盈亏数量"},
                {"YingPrice", "盈亏金额"},
                {"BeiZhu", "备注"}
        };
        #endregion

        #region 盘点录入数据导出Excel
        public void ExportPurchaseData(Hyt.Model.InventorySheet.WhInventoryDetail list, string userIp, int operatorSysno)
        {
            try
            {
                // 查询商品
                List<WhInventoryProductDetailOutput> outputData = new List<WhInventoryProductDetailOutput>();
                foreach (var item in list.dataList)
                {
                    WhInventoryProductDetailOutput model = new WhInventoryProductDetailOutput();
                    model.WarehouseNameDate = item.WarehouseNameDate;
                    model.PrCode = item.ErpCode;
                    model.PrName = item.EasName;
                    model.ZhangCunQuantity = item.ZhangCunQuantity;
                    model.InventoryQuantity = item.InventoryQuantity;
                    model.Remarks = item.Remarks;
                    outputData.Add(model);
                }
                var fileName = string.Format("" + list.Code + "({0})", DateTime.Now.ToString("yyyyMMddHHmmss"));

                /*
                 * 仓库名称
                 * 商品编码
                 * 后台显示名称
                 * 条形码
                 * 海关备案号
                 * 采购价格
                 * 库存数量
                 */

                //导出Excel，并设置表头列名
                Util.ExcelUtil.Export<WhInventoryProductDetailOutput>(outputData,
                    new List<string> { "仓库名称", "商品编号", "商品名称", "账存数量", "盘点数量", "备注" },
                    fileName);
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "盘点数据录入商品导出excel",
                                         LogStatus.系统日志目标类型.ExcelExporting, 0, null, userIp, operatorSysno);
            }
            catch (Exception ex)
            {

                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "盘点数据录入商品导出excel",
                                         LogStatus.系统日志目标类型.ExcelExporting, 0, ex, userIp, operatorSysno);

            }
        }
        #endregion

        #region 盘点录入数据导入Excel 2017-08-15 吴琨 创建
        public Resuldt<WhInventoryProductDetailOutput> ImportExcelTo(System.IO.Stream stream, int SysNo)
        {
            DataTable dt = null;
            var cols = DicColsMappingTo.Select(p => p.Value).ToArray();
            #region 基础验证
            try
            {
                dt = ExcelUtil.ImportExcel(stream, cols);
            }
            catch (Exception ex)
            {
                return new Resuldt<WhInventoryProductDetailOutput>
                {
                    Message = string.Format("数据导入错误,请选择正确的excel文件"),
                    Status = false
                };
            }
            if (dt == null)
            {
                //not all the cols mapped
                return new Resuldt<WhInventoryProductDetailOutput>
                {
                    Message = string.Format("请选择正确的excel文件!"),
                    Status = false
                };
            }

            if (dt.Rows.Count == 0)
            {
                return new Resuldt<WhInventoryProductDetailOutput>
                {
                    Message = "导入的数据为空!",
                    Status = false
                };
            }
            #endregion

            Resuldt<WhInventoryProductDetailOutput> run = new Resuldt<WhInventoryProductDetailOutput>();
            List<WhInventoryProductDetailOutput> listModel = new List<WhInventoryProductDetailOutput>();
            int fail = 0;//失败记录数
            int success = 0; //成功记录数
            string failstr = ""; //失败条数记录
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                success++;
                WhInventoryProductDetailOutput model = new WhInventoryProductDetailOutput();
                model.WarehouseNameDate = dt.Rows[i]["仓库名称"].ToString();
                model.PrCode = dt.Rows[i]["商品编号"].ToString();
                model.PrName = dt.Rows[i]["商品名称"].ToString();
                model.ZhangCunQuantity = Convert.ToDecimal(dt.Rows[i]["账存数量"]);
                model.InventoryQuantity = Convert.ToDecimal(dt.Rows[i]["盘点数量"]);
                model.Remarks = dt.Rows[i]["备注"].ToString();
                listModel.Add(model);
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
        private static readonly Dictionary<string, string> DicColsMappingTo = new Dictionary<string, string>
        {
            {"WhBackWarehouseName", "仓库名称"},
            {"PrErpCode", "商品编号"},
            {"PrEasName", "商品名称"},
            {"WhStockQuantity", "账存数量"},
            {"ShiQuantity", "盘点数量"},
            {"BeiZhu", "备注"}
        };
        #endregion

        #region 其他出入库
        /// <summary>
        /// 分页获取其他出入库
        /// </summary>
        /// <param name="pager"></param>
        /// <param name="dataType">1查询全部 2查询其他出库 3查询其他入库</param>
        /// <returns></returns>
        /// 2017-8-07
        public Pager<OtherOutOfStorage> GetOtherOutOfStoragePage(Pager<OtherOutOfStorage> pager, int? dataTyp = 1)
        {
            return IWhInventoryDao.Instance.GetOtherOutOfStoragePage(pager, dataTyp);
        }



        #region 其他出入库商品导入Excel 2017-08-17 吴琨 创建
        public Resuldt<OtherOutOfStorageDetailed> OtherImportExcel(System.IO.Stream stream, int SysNo)
        {
            DataTable dt = null;
            var cols = OtherDicColsMapping.Select(p => p.Value).ToArray();
            try
            {
                dt = ExcelUtil.ImportExcel(stream, cols);
            }
            catch (Exception ex)
            {
                return new Resuldt<OtherOutOfStorageDetailed>
                {
                    Message = string.Format("数据导入错误,请选择正确的excel文件"),
                    Status = false
                };
            }
            if (dt == null)
            {
                //not all the cols mapped
                return new Resuldt<OtherOutOfStorageDetailed>
                {
                    Message = string.Format("请选择正确的excel文件!"),
                    Status = false
                };
            }

            if (dt.Rows.Count == 0)
            {
                return new Resuldt<OtherOutOfStorageDetailed>
                {
                    Message = "导入的数据为空!",
                    Status = false
                };
            }
            Resuldt<OtherOutOfStorageDetailed> run = new Resuldt<OtherOutOfStorageDetailed>();
            List<OtherOutOfStorageDetailed> listModel = new List<OtherOutOfStorageDetailed>();
            int fail = 0;//失败记录数
            int success = 0; //成功记录数
            string failstr = ""; //失败条数记录
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                OtherOutOfStorageDetailed model = new OtherOutOfStorageDetailed();
                var product = PdProductBo.Instance.GetProductErpCode(dt.Rows[i]["商品代码"].ToString(), dt.Rows[i]["商品条形码"].ToString());
                var ware = Hyt.BLL.Web.WhWarehouseBo.Instance.GetModelErpCode(dt.Rows[i][7].ToString());
                if (product == null||ware == null)
                {
                    fail++;
                    failstr += (i + 2) + "、";
                    dt.Rows.Remove(dt.Rows[i]);
                }
                else
                {
                    #region 判断商品是否已入库,若未入库将商品插入至商品库存表
                    var Stock = IWhInventoryDao.Instance.GetuditPdProductStockTo(dt.Rows[i]["商品代码"].ToString().Trim(), dt.Rows[i][7].ToString().Trim());
                    if (Stock == null)
                    {
                        // WhInventoryBo
                        PdProductStock stockModel = new PdProductStock()
                        {
                            WarehouseSysNo = ware.SysNo,
                            PdProductSysNo = product.SysNo,
                            StockQuantity = 0,
                            LockStockQuantity=0,
                            CreatedDate = DateTime.Now,
                            Barcode = product.Barcode,
                            CostPrice = 0,
                            CustomsNo = "",
                            InStockTime = "",
                            Remark = "库存盘点自动入库"
                        };
                        PdProductStockBo.Instance.SaveProductStock(stockModel, 0);
                    }
                    #endregion

                    //重新获取商品库存信息
                    Stock = IWhInventoryDao.Instance.GetuditPdProductStockTo(dt.Rows[i]["商品代码"].ToString().Trim(), dt.Rows[i][7].ToString().Trim());
                    success++;
                    model.ProductCode = product.ErpCode;
                    model.BarCode = product.Barcode;
                    model.ProductName = product.EasName;
                    model.Count = dt.Rows[i]["实收数量"] != DBNull.Value ? Convert.ToDecimal(dt.Rows[i]["实收数量"]) : Convert.ToDecimal(0.00);
                    model.UnitPrice = dt.Rows[i]["单价"] != DBNull.Value ? Convert.ToDecimal(dt.Rows[i]["单价"]) : Convert.ToDecimal(0.00);
                    model.Price = (model.Count * model.UnitPrice);
                    model.Remarks = dt.Rows[i]["备注"].ToString();
                    model.ProductSysNo = product.SysNo;
                    model.CollectWarehouseName = ware.BackWarehouseName;
                    model.CollectWarehouseCode = ware.ErpCode;
                    model.CollectWarehouseSysNo = ware.SysNo;
                    listModel.Add(model);
                }
            }
            if (success > 0 && dt.Rows.Count > 0) run.Data = dt;
            if (success > 0 && listModel != null) run.listModel = listModel;
            run.Message = "导入成功" + success + "件商品,失败" + fail + "件商品;";
            if (fail > 0) run.Message += "失败原因为:产品编码或仓库erp编码有误,不存在此件商品或仓库。失败条数为第" + failstr.Trim('、') + "条。";
            run.Status = true;
            return run;
        }

        /// <summary>
        /// import cols mapping between DB and excel
        /// </summary>
        /// <remarks>2017-08-15 吴琨 创建</remarks>
        private static readonly Dictionary<string, string> OtherDicColsMapping = new Dictionary<string, string>
        {
                {"ProductCode", "商品代码"},
                {"BarCode", "商品条形码"},
                {"ProductName", "商品名称"},
                {"Count", "实收数量"},
                {"UnitPrice", "单价"},
                {"Price", "金额"},
                {"CollectWarehouseName", "仓库名称"},
                {"CollectWarehouseCode", "仓库Erp编码"},
                {"BeiZhu", "备注"}
        };
        #endregion

        #endregion


        #region 创建其他入库
        /// <summary>
        /// 添加其他入库
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddOtherOutOfStorage(OtherOutOfStorage model)
        {
            return IWhInventoryDao.Instance.AddOtherOutOfStorage(model);
        }
        #endregion

        #region 根据id获取其他出入库明细
        /// <summary>
        /// 根据id获取其他出入库明细
        /// </summary>
        /// <returns></returns>
        public  OtherOutOfStorage GetOtherOutOfStorageModel(int sysNo)
        {
            return IWhInventoryDao.Instance.GetOtherOutOfStorageModel(sysNo);
        }
        #endregion
       
        #region 其他出入库更新库存
        /// <summary>
        /// 其他出入库更新库存
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public  bool UpdateOtherOutPdProductStock(OtherOutOfStorage model)
        {
            return IWhInventoryDao.Instance.UpdateOtherOutPdProductStock(model);
        }
        #endregion
    }
}
