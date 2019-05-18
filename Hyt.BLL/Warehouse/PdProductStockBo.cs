using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.DataAccess.Warehouse;
using Hyt.Model.WorkflowStatus;
using System.IO;
using System.Data;
using Hyt.Util;
using Hyt.BLL.Product;
using Hyt.BLL.Log;
using Hyt.Model.Generated;
using Extra.Erp.Kis;
using Hyt.Model.ExcelTemplate;

namespace Hyt.BLL.Warehouse
{
    /// <summary>
    /// 库存
    /// </summary>
    /// <remarks>
    /// 2015-08-06 王耀发 创建
    /// </remarks>
    public class PdProductStockBo : BOBase<PdProductStockBo>
    {
        /// <summary>
        /// 检查产品指定仓库库存是否足够减（包含锁定库存）
        /// </summary>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <param name="productSysNo">产品系统编号</param>
        /// <param name="qty">待减的数量</param>
        /// <returns></returns>
        /// <remarks>2018-01-17 杨浩 创建</remarks>
        public bool HasProductStock(int warehouseSysNo, int productSysNo, int qty)
        {
            return IPdProductStockDao.Instance.HasProductStock(warehouseSysNo, productSysNo, qty);
        }
        /// <summary>
        /// 库存查询
        /// </summary>
        /// <param name="erpCode">商品编码</param>
        /// <param name="warehouseSysNo">仓库编号</param>
        /// <returns></returns>
        /// <remarks>2013-8-8 杨浩 添加</remarks>
        public IList<Inventory> GetInventory(string[] erpCode, int warehouseSysNo)
        {
            return IPdProductStockDao.Instance.GetInventory(erpCode, warehouseSysNo);
        }
        /// <summary>
        /// 获取产品所在的仓库列表(仓库按商品的库存降序排序，禁用的仓库不会显示)
        /// </summary>
        /// <param name="productSysNo">产品系统编号</param>
        /// <returns>仓库列表</returns>
        /// <remarks>2015-9-9 杨浩 创建</remarks>
        public IList<PdProductStock> GetProductStockListByProductSysNo(int productSysNo)
        {
            return IPdProductStockDao.Instance.GetProductStockListByProductSysNo(productSysNo);
        }
        /// <summary>
        /// 获取产品所在的仓库列表
        /// </summary>
        /// <param name="productSysNo">产品系统编号</param>
        /// <returns>仓库列表</returns>
        /// <remarks>2016-6-3 王耀发 创建</remarks>
        public IList<PdProductStockList> GetProStockListByProductSysNo(int productSysNo)
        {
            return IPdProductStockDao.Instance.GetProStockListByProductSysNo(productSysNo);
        }

        #region 库存商品
        /// <summary>
        /// 分页获取库存商品
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns>分页列表</returns>
        /// <remarks>2015-08-06 王耀发 创建</remarks>
        public Pager<CBPdProductStockList> GetPdProductStockList(ParaProductStockFilter filter)
        {
            return IPdProductStockDao.Instance.GetPdProductStockList(filter);
        }




        /// <summary>
        /// 获取库存商品
        /// </summary>
        /// <param name="WhInventoryStr">库存id</param>
        /// <param name="whereStr">筛选条件</param>
        /// <param name="BrandSysNo">品牌id</param>
        /// <param name="PdCategoryId">分类id</param>
        /// <returns></returns>
        /// <remarks>2017-08-08 吴琨 创建</remarks>
        public List<CBPdProductStockList> GetPdProductStockListData(string WhInventoryStr, string whereStr, string BrandSysNo, string PdCategoryId)
        {
            return IPdProductStockDao.Instance.GetPdProductStockListData(WhInventoryStr, whereStr, BrandSysNo, PdCategoryId);
        }


        /// <summary>
        /// 根据商品编号获取条码
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        public  string GetBarcode(string Code)
        {
            return IPdProductStockDao.Instance.GetBarcode(Code);
        }



        /// <summary>
        /// 保存入库商品到库存中
        /// </summary>
        /// <param name="model">商品模型</param>
        /// <param name="userSysNo">操作人系统编号</param>
        /// <returns></returns>
        /// <remarks>2016-06-21 杨浩 创建</remarks>
        public int SaveProductStock(PdProductStock model, int userSysNo)
        {
            int stockSysNo = 0;
            var entity = IPdProductStockDao.Instance.GetEntityByWP(model.WarehouseSysNo, model.PdProductSysNo);
            if (entity != null)
            {
                model.SysNo = entity.SysNo;
                model.StockQuantity = model.StockQuantity + entity.StockQuantity;
                model.LockStockQuantity = entity.LockStockQuantity;
                model.CreatedDate = entity.CreatedDate;
                model.CreatedBy = entity.CreatedBy;
                model.LastUpdateBy = userSysNo;
                model.LastUpdateDate = DateTime.Now;
                IPdProductStockDao.Instance.Update(model);
                stockSysNo = model.SysNo;

            }
            else
            {
                model.CreatedDate = DateTime.Now;
                model.CreatedBy = userSysNo;
                model.LastUpdateBy = userSysNo;
                model.LastUpdateDate = DateTime.Now;
                stockSysNo = IPdProductStockDao.Instance.Insert(model);
            }
            return stockSysNo;
        }

        /// <summary>
        /// 保存入库商品到库存中
        /// </summary>
        /// <param name="model">定制商品</param>
        /// <param name="user">操作人</param>
        /// <returns></returns>
        /// <remarks>2015-08-06 王耀发 创建</remarks>
        public Result SavePdProductStock(PdProductStock model, SyUser user)
        {
            var result = new Result()
            {
                Status = false
            };
            var entity = IPdProductStockDao.Instance.GetEntityByWP(model.WarehouseSysNo, model.PdProductSysNo);
            if (entity != null)
            {
                model.SysNo = entity.SysNo;
                model.StockQuantity = model.StockQuantity + entity.StockQuantity;
                model.LockStockQuantity = entity.LockStockQuantity;
                model.CreatedDate = entity.CreatedDate;
                model.CreatedBy = entity.CreatedBy;
                model.LastUpdateBy = user.SysNo;
                model.LastUpdateDate = DateTime.Now;
                IPdProductStockDao.Instance.Update(model);
                result.Status = true;
            }
            else
            {
                model.CreatedDate = DateTime.Now;
                model.CreatedBy = user.SysNo;
                model.LastUpdateBy = user.SysNo;
                model.LastUpdateDate = DateTime.Now;
                IPdProductStockDao.Instance.Insert(model);
                result.Status = true;
            }
            return result;
        }
        /// <summary>
        /// 根据仓库编号，产品编号
        /// </summary>
        /// <param name="WarehouseSysNo"></param>
        /// <param name="PdProductSysNo"></param>
        /// <returns></returns>
        /// <remarks>2015-12-26 王耀发 创建</remarks>
        public PdProductStock GetEntityByWP(int WarehouseSysNo, int PdProductSysNo)
        {
            return IPdProductStockDao.Instance.GetEntityByWP(WarehouseSysNo, PdProductSysNo);
        }
        /// <summary>
        /// 获得商品库存，根据商品编号字符串
        /// </summary>
        /// <param name="warehouseSysNo"></param>
        /// <param name="productSysNos"></param>
        /// <returns></returns>
        public IList<PdProductStock> GetPdProductStockList(int warehouseSysNo, int[] productSysNos)
        {
            return IPdProductStockDao.Instance.GetPdProductStockList(warehouseSysNo, productSysNos);
        }

        /// <summary>
        /// 更新库存数量
        /// </summary>
        /// <param name="WarehouseSysNo">仓库编号</param>
        /// <param name="PdProductSysNo">产品编号</param>
        /// <param name="Quantity">商品数</param>
        /// <remarks>2016-5-25 杨浩 添加注释</remarks>
        public void UpdateStockQuantity(int WarehouseSysNo, int PdProductSysNo, decimal Quantity)
        {
            IPdProductStockDao.Instance.UpdateStockQuantity(WarehouseSysNo, PdProductSysNo, Quantity);
        }

        /// <summary>
        /// 释放锁定库存数量
        /// </summary>
        /// <param name="WarehouseSysNo">仓库编号</param>
        /// <param name="PdProductSysNo">产品编号</param>
        /// <param name="Quantity">商品数</param>
        /// <remarks>2016-5-25 杨浩 添加注释</remarks>
        public void RollbackLockStockQuantity(int WarehouseSysNo, int PdProductSysNo, int  Quantity)
        {
            IPdProductStockDao.Instance.RollbackLockStockQuantity(WarehouseSysNo, PdProductSysNo, Quantity);
        }

        /// <summary>
        /// 更新锁定库存数，
        /// </summary>
        /// <param name="warehouseSysNo"></param>
        /// <param name="productSysNo"></param>
        /// <param name="quantity"></param>
        /// <remarks>2017-9-09 罗勤尧 创建</remarks>
        public void UpdateLockStockQuantity(int warehouseSysNo, int productSysNo, int quantity)
        {
            IPdProductStockDao.Instance.UpdateLockStockQuantity(warehouseSysNo, productSysNo, quantity);
        }
        /// <summary>
        /// 更新门店仓库库存数量
        /// </summary>
        /// <param name="WarehouseSysNo">仓库编号</param>
        /// <param name="PdProductSysNo">产品编号</param>
        /// <param name="Quantity">商品数</param>
        /// <remarks>2016-5-25 杨浩 添加</remarks>
        public void UpdateShopStockQuantity(int warehouseSysNo, int productSysNo, decimal quantity)
        {
            int row = IPdProductStockDao.Instance.UpdateStockQuantity(warehouseSysNo, productSysNo, quantity);

            //没更新则添加负的库存
            if (row <= 0)
            {
                var productStockInfo = new PdProductStock();
                productStockInfo.StockQuantity = -quantity;
                productStockInfo.WarehouseSysNo = warehouseSysNo;
                productStockInfo.PdProductSysNo = productSysNo;
                productStockInfo.LastUpdateDate = DateTime.Now;
                productStockInfo.LastUpdateBy = BLL.Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo;
                productStockInfo.CreatedBy = productStockInfo.LastUpdateBy;
                productStockInfo.CreatedDate = DateTime.Now;
                IPdProductStockDao.Instance.Insert(productStockInfo);
                //var stockInInfo=new WhStockIn();
                //stockInInfo.CreatedBy = productStockInfo.CreatedBy;
                //stockInInfo.CreatedDate = DateTime.Now;
                //stockInInfo.DeliveryType = 10;
                //stockInInfo.ItemList = new List<WhStockInItem>();
                //stockInInfo.LastUpdateBy = productStockInfo.CreatedBy;
                //stockInInfo.LastUpdateDate = DateTime.Now;
                //stockInInfo.Remarks = "门店扫描出库，系统自动补入库单！";

                //stockInInfo.SourceType = 10;
                //Hyt.BLL.Warehouse.InStockBo.Instance.CreateStockIn(stockInInfo);

            }
        }

        /// <summary>
        /// 更新商品库存信息
        /// </summary>
        /// <param name="models">商品信息列表</param>
        /// <returns>空</returns>
        /// <remarks>2015-09-10 王耀发 创建</remarks>
        public void UpdateProductStockInfo(PdProductStock model)
        {
            IPdProductStockDao.Instance.UpdateProductStockInfo(model);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>删除数据编号</returns>
        /// <remarks>2016-08-1  罗远康 创建</remarks>
        public void Delete(int sysNo)
        {
            IPdProductStockDao.Instance.Delete(sysNo);
        }
        #endregion

        #region 商品导入

        /// <summary>
        /// import cols mapping between DB and excel
        /// </summary>
        /// <remarks>2015-09-10 王耀发 创建</remarks>
        private static Dictionary<string, string> DicColsMapping = new Dictionary<string, string>
            {
                {"BackWarehouseName", "仓库名称"},
                {"ErpCode", "商品编码"},
                {"EasName", "后台显示名称"},
                {"Barcode", "条形码"},
                {"CustomsNo", "海关备案号"},
                {"CostPrice","采购价格"},
                {"StockQuantity","库存数量"},
                {"InStockTime", "日期"},
                {"Remark", "备注"}
            };

        /// <summary>
        /// import cols mapping between DB and excel
        /// </summary>
        /// <remarks>2017-06-23 罗勤尧 创建</remarks>
        private static Dictionary<string, string> DicColsMappingSn = new Dictionary<string, string>
            {
                {"BackWarehouseName", "后台仓库名称"},
                {"Barcode", "商品条码"},
                {"DateTime", "日期"},
                {"StockQuantity", "库存"},
                {"Remack", "备注"}
            };
        /// <summary>
        /// import cols mapping between DB and excel
        /// </summary>
        /// <remarks>2017-06-23 罗勤尧 创建</remarks>
        private static Dictionary<string, string> DicColsMappingSnDate = new Dictionary<string, string>
            {
                {"BackWarehouseName", "后台仓库名称"},
                {"ErpCode", "商品编码"},
                {"DateTime", "日期"},
            };
        private static readonly Dictionary<string, string> DicColsMappingYS = new Dictionary<string, string>
            {
                {"BackWarehouseName", "仓库名称"},
                {"ErpCode", "商品编码"},
                {"EasName", "后台显示名称"},
                {"Barcode", "条形码"},
                {"ProductSku","商品SKU"},
                {"CustomsNo", "海关备案号"},
                {"CostPrice","采购价格"},
                {"StockQuantity","库存数量"}
            };


        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <typeparam name="T">产品库存</typeparam>
        /// <param name="dt"></param>
        /// <remarks>2017-1-12 杨浩 创建</remarks>
        public void ExportExcel<T>(DataTable dt) where T : ITemplateBase
        {
            ExcelUtil.Export<T>(dt);
        }


        #region 库存导入导出

        /// <summary>
        /// 仓库库存excel导入
        /// </summary>
        /// <param name="stream">导入的excel stream format</param>
        /// <param name="operatorSysno">操作人员编号</param>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <returns>封装的泛型result对象</returns>
        /// <remarks>2015-09-10 杨浩 创建</remarks>
        public Result ImportExcel(Stream stream, int operatorSysno, int warehouseSysNo)
        {
            var result = new Result() { Status=false};           
            string warehouseNanme = BLL.Warehouse.WhWarehouseBo.Instance.GetWarehouseName(warehouseSysNo);
            if (warehouseNanme == "未知仓库")
            {
                result.Message = "未知仓库";
                return result;
            }

            var lstToInsert = new List<PdProductStock>();
            var lstToUpdate = new List<PdProductStock>();
            DataTable dt;
            var pager = new Pager<PdProduct>(){PageSize=99999999};
            var productList = BLL.Product.PdProductBo.Instance.GetPdProductList(pager);
           
            var cols = DicColsMapping.Select(p => p.Value).ToArray();
            try
            {
                dt = ExcelUtil.ImportExcel(stream, cols);
                if (dt == null)
                    throw new Exception();              
            }
            catch
            {
                //exception happened,some not caughted
                result.Message = string.Format("数据导入错误,请选择正确的excel文件");
                return result;
            }

            //仓库所有产品库存列表
            var warehouseProductStockList = DataAccess.Warehouse.IPdProductStockDao.Instance.GetProStockByWarehouseSysNo(warehouseSysNo);
            int excelRow = 0; //当前行号
            //循环excel数据
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                excelRow++;

                var erpCode = dt.Rows[i][DicColsMapping["ErpCode"]].ToString().Trim();
                //条形码
                var barcode = dt.Rows[i][DicColsMapping["Barcode"]].ToString().Trim();
                //海关备案号
                var customsNo = dt.Rows[i][DicColsMapping["CustomsNo"]].ToString().Trim();
                //采购价格
                var costPrice = dt.Rows[i][DicColsMapping["CostPrice"]].ToString().Trim();
                //库存数量
                var stockQuantity = dt.Rows[i][DicColsMapping["StockQuantity"]].ToString().Trim();

                var codeRows=dt.Select(DicColsMapping["ErpCode"] + "='" + erpCode + "'");
                if (codeRows.Length > 1)
                {
                    result.Message = "导入的数据商品编码【" + erpCode + "】必须是唯一的!";
                    return result;
                }
                var barcodeRows=dt.Select(DicColsMapping["Barcode"] + "='" + barcode + "'");
                if (barcodeRows.Length > 1)
                {
                    result.Message = "导入的数据条形码【" + barcode + "】必须是唯一的!";
                    return result;
                }


                var productInfo=pager.Rows.Where(x => x.ErpCode == erpCode).FirstOrDefault();
                if (productInfo == null)
                {
                    result.Message = string.Format("excel表第{0}行商品编号不存在", excelRow);
                    return result;
                }
             
                var outCostPrice = 0m;
                Decimal.TryParse(costPrice, out outCostPrice);
                //if (!Decimal.TryParse(costPrice, out outCostPrice))
                //{
                //    result.Message = string.Format("excel表第{0}行采购价格必须为数值", excelRow);
                //    return result;
                //}

                var outStockQuantity = 0;
                if (!int.TryParse(stockQuantity, out outStockQuantity))
                {
                    result.Message = string.Format("excel表第{0}行库存数量必须为数值", excelRow);
                    return result;               
                }

                var model = new PdProductStock
                {
                    WarehouseSysNo = warehouseSysNo,
                    PdProductSysNo = productInfo.SysNo,
                    Barcode = barcode,
                    CustomsNo = customsNo,
                    CostPrice = outCostPrice,
                    StockQuantity = outStockQuantity,
                    CreatedBy = operatorSysno,
                    CreatedDate = DateTime.Now,
                    LastUpdateBy = operatorSysno,
                    LastUpdateDate = DateTime.Now
                };


                var productStockInfo=warehouseProductStockList.Where(x => x.PdProductSysNo == model.PdProductSysNo&&x.WarehouseSysNo==model.WarehouseSysNo).FirstOrDefault();

                var productStockList = warehouseProductStockList.Where(x => x.Barcode == model.Barcode && x.WarehouseSysNo == model.WarehouseSysNo).ToList();

                if (productStockInfo == null)
                {                 
                    if (productStockList.Count > 0)
                    {
                        result.Message = string.Format("仓库中已经存在条码【" + model.Barcode+ "】,请修改再导入！", excelRow);
                        return result; 
                    }
                    lstToInsert.Add(model);
                }
                else
                {                  
                    if (productStockList.Count > 1)
                    {
                        result.Message = string.Format("仓库中产品条码【" + model.Barcode + "】出现过" + productStockList.Count + "次,请修改再导入！", excelRow);
                        return result;
                    }
                    lstToUpdate.Add(model);
                }
         
            }

            try
            {
                IPdProductStockDao.Instance.CreateExcelProductStock(lstToInsert);
                IPdProductStockDao.Instance.UpdateExcelProductStock(lstToUpdate);
            }
            catch (Exception ex)
            {
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "导入商品库存信息",
                                         LogStatus.系统日志目标类型.商品库存信息, 0, ex, null, operatorSysno);

                result.Message = string.Format("数据更新错误:{0}", ex.Message);
                return result;                   
               
            }

            var msg = lstToInsert.Count > 0 ? string.Format("成功导入{0}条数据!", lstToInsert.Count) : "";
            msg += lstToUpdate.Count > 0 ? string.Format("成功修改{0}条数据!", lstToUpdate.Count) : "";

            result.Status = true;
            result.Message = msg;
            return result;
        }


        /// <summary>
        /// 导入excel
        /// </summary>
        /// <param name="stream">导入的excel stream format</param>
        /// <param name="operatorSysno">操作人员编号</param>
        /// <returns>封装的泛型result对象</returns>
        /// <remarks>2015-09-10 王耀发 创建</remarks>
        public Result ImportExcel(Stream stream, int operatorSysno,IDictionary<string,string> dic=null)
        {
            #region Excel转DataTable
            DataTable dt = null;
            int warehouseSysNo = 0;
            IDictionary<string, string> _dic = DicColsMapping;
            string[] cols;
            if(dic==null)
                cols = DicColsMapping.Select(p => p.Value).ToArray();
            else
            {
                cols = dic.Values.ToArray();
                _dic = dic;
            }
                
            try
            {
                dt = ExcelUtil.ImportExcel(stream, cols);
            }
            catch (Exception ex)
            {
                //exception happened,some not caughted
                return new Result
                {
                    Message = ex.Message,
                    Status = false
                };
            }

            if (dt == null)
            {
                //not all the cols mapped
                return new Result
                {
                    Message = string.Format("请选择正确的excel文件!"),
                    Status = false
                };
            }
            #endregion

            var excellst = new List<PdProductStock>();
            var lstToInsert = new List<PdProductStock>();
            var lstToUpdate = new List<PdProductStock>();

            #region 遍历DataTable
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                int excelRow = i;
                for (var j = 0; j < dt.Columns.Count; j++)
                {
                    string columnName=dt.Columns[j].ColumnName;
                    //条形码、海关备案号。日期，备注可以为空
                    if (columnName != _dic["CustomsNo"] && columnName != _dic["Barcode"] && columnName != _dic["Remark"] && columnName != _dic["InStockTime"])                                 
                    {
                        if (string.IsNullOrWhiteSpace(dt.Rows[i][j].ToString()))
                        {
                            return new Result
                            {
                                Message = string.Format("excel表第{0}行{1}列数据不能有空值", (excelRow + 2),(j+1)),
                                Status = false
                            };
                        }
                    }
                }


                //仓库名称
                var backWarehouseName = dt.Rows[i][_dic["BackWarehouseName"]].ToString().Trim();
                WhWarehouse sEntity = WhWarehouseBo.GetWarehouseByName(backWarehouseName);
                warehouseSysNo = sEntity.SysNo;
                if (sEntity == null)
                {
                    return new Result
                    {
                        Message = string.Format("excel表第{0}行仓库名称不存在", (excelRow + 2)),
                        Status = false
                    };
                }
                //商品编号
                var ErpCode = dt.Rows[i][_dic["ErpCode"]].ToString().Trim();
                PdProduct pEntity = PdProductBo.Instance.GetProductByErpCode(ErpCode);
                if (pEntity == null)
                {
                    return new Result
                    {
                        Message = string.Format("excel表第{0}行商品编号不存在", (excelRow + 2)),
                        Status = false
                    };
                }
                //条形码
                var Barcode = dt.Rows[i][_dic["Barcode"]].ToString().Trim();
                //海关备案号
                var CustomsNo = dt.Rows[i][_dic["CustomsNo"]].ToString().Trim();
                //采购价格
                var CostPrice = dt.Rows[i][_dic["CostPrice"]].ToString().Trim();
                Decimal typeCostPrice = 0;
                if (!Decimal.TryParse(CostPrice, out typeCostPrice))
                {
                    return new Result
                    {
                        Message = string.Format("excel表第{0}行采购价格必须为数值", (excelRow + 2)),
                        Status = false
                    };
                }
                //库存数量
                var StockQuantity = dt.Rows[i][_dic["StockQuantity"]].ToString().Trim();
                Decimal typeStockQuantity = 0;
                if (!Decimal.TryParse(StockQuantity, out typeStockQuantity))
                {
                    return new Result
                    {
                        Message = string.Format("excel表第{0}行库存数量必须为数值", (excelRow +2)),
                        Status = false
                    };
                }

                //日期
                var Time = dt.Rows[i][_dic["InStockTime"]].ToString().Trim();
                //Decimal typeCostPrice = 0;
                //if (string.IsNullOrWhiteSpace(Time))
                //{
                //    return new Result
                //    {
                //        Message = string.Format("excel表第{0}日期不能为空", (excelRow + 2)),
                //        Status = false
                //    };
                //}
                //备注
                var Remack = dt.Rows[i][_dic["Remark"]].ToString().Trim();
                var model = new PdProductStock
                {
                    WarehouseSysNo = sEntity.SysNo,
                    PdProductSysNo = pEntity.SysNo,
                    Barcode = Barcode,
                    CustomsNo = CustomsNo,
                    CostPrice = decimal.Parse(CostPrice),
                    StockQuantity = decimal.Parse(StockQuantity),
                    CreatedBy = operatorSysno,
                    CreatedDate = DateTime.Now,
                    LastUpdateBy = operatorSysno,
                    LastUpdateDate = DateTime.Now,
                    InStockTime = Time,
                    Remark = Remack
                };
                excellst.Add(model);
            }
            #endregion

            #region 添加新增和修改到对应集合
            var lstExisted = DataAccess.Warehouse.IPdProductStockDao.Instance.GetAllProductStock();
            foreach (var excelModel in excellst)
            {
                if (lstExisted.Any(e => e.WarehouseSysNo == excelModel.WarehouseSysNo && e.PdProductSysNo == excelModel.PdProductSysNo))
                {
                    
                    lstToUpdate.Add(excelModel);
                }
                else //insert
                {
                    lstToInsert.Add(excelModel);
                }
            }
            #endregion

            #region 修改或更新到数据库
            try
            {
                var user = Hyt.BLL.Sys.SyUserBo.Instance.GetSyUser(operatorSysno);
                IPdProductStockDao.Instance.CreateExcelProductStock(lstToInsert);
                IPdProductStockDao.Instance.UpdateExcelProductStock(lstToUpdate);
                foreach(var mod in lstToInsert)
                {
                    WhWarehouseChangeLog log1 = new WhWarehouseChangeLog()
                    {
                        WareSysNo = mod.WarehouseSysNo,
                        ProSysNo = mod.PdProductSysNo,
                        ChageDate = DateTime.Now,
                        CreateDate = DateTime.Now,
                        ChangeQuantity = Convert.ToInt32(mod.StockQuantity),
                        BusinessTypes = "导EXCEL库存修改",
                        LogData = user.UserName + " 通过导入EXCEL修改仓库库存数据，将库存修改为" + mod.StockQuantity
                    };
                    WhWarehouseChangeLogBo.Instance.CreateMod(log1);
                }

                foreach (var mod in lstToUpdate)
                {
                    WhWarehouseChangeLog log1 = new WhWarehouseChangeLog()
                    {
                        WareSysNo = mod.WarehouseSysNo,
                        ProSysNo = mod.PdProductSysNo,
                        ChageDate = DateTime.Now,
                        CreateDate = DateTime.Now,
                        ChangeQuantity = Convert.ToInt32(mod.StockQuantity),
                        BusinessTypes = "导EXCEL库存修改",
                        LogData = user.UserName + " 通过导入EXCEL修改仓库库存数据，将库存修改为" + mod.StockQuantity
                    };
                    WhWarehouseChangeLogBo.Instance.CreateMod(log1);
                }
            }
            catch (Exception ex)
            {
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "导入商品信息",
                                         LogStatus.系统日志目标类型.商品基本信息, 0, ex, null, operatorSysno);
                return new Result
                {
                    Message = string.Format("数据更新错误:{0}", ex.Message),
                    Status = false
                };
            }
            #endregion


            var msg = lstToInsert.Count > 0 ? string.Format("成功导入{0}条数据!", lstToInsert.Count) : "";
            msg += lstToUpdate.Count > 0 ? string.Format("成功修改{0}条数据!", lstToUpdate.Count) : "";
            return new Result
            {
                Message = msg,
                Status = true
            };
        }


        /// <summary>
        /// 商品条形码导入excel
        /// </summary>
        /// <param name="stream">导入的excel stream format</param>
        /// <param name="operatorSysno">操作人员编号</param>
        /// <returns>封装的泛型result对象</returns>
        /// <remarks>2017-06-23 罗勤尧 创建</remarks>
        public Result ImportExcelSn(Stream stream, int operatorSysno, IDictionary<string, string> dic = null)
        {
            #region Excel转DataTable
            DataTable dt = null;
            int warehouseSysNo = 0;
            IDictionary<string, string> _dic = DicColsMappingSn;
            string[] cols;
            if (dic == null)
                cols = DicColsMappingSn.Select(p => p.Value).ToArray();
            else
            {
                cols = dic.Values.ToArray();
                _dic = dic;
            }

            try
            {
                dt = ExcelUtil.ImportExcel(stream, cols);
            }
            catch (Exception ex)
            {
                //exception happened,some not caughted
                return new Result
                {
                    Message = ex.Message,
                    Status = false
                };
            }

            if (dt == null)
            {
                //not all the cols mapped
                return new Result
                {
                    Message = string.Format("请选择正确的excel文件!"),
                    Status = false
                };
            }
            #endregion

            var excellst = new List<PdProductStock>();
            var lstToInsert = new List<PdProductStock>();
            var lstToUpdate = new List<PdProductStock>();

            #region 遍历DataTable
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                int excelRow = i;
                for (var j = 0; j < dt.Columns.Count; j++)
                {
                    string columnName = dt.Columns[j].ColumnName;
                    //备注,日期可以为空
                    if (columnName != _dic["Remack"] && columnName != _dic["DateTime"])
                    {
                        if (string.IsNullOrWhiteSpace(dt.Rows[i][j].ToString()))
                        {
                            return new Result
                            {
                                Message = string.Format("excel表第{0}行{1}列数据不能有空值", (excelRow + 2), (j + 1)),
                                Status = false
                            };
                        }
                    }
                }


                //仓库名称
                var backWarehouseName = dt.Rows[i][_dic["BackWarehouseName"]].ToString().Trim();
                WhWarehouse sEntity = WhWarehouseBo.GetWarehouseByName(backWarehouseName);
                warehouseSysNo = sEntity.SysNo;
                if (sEntity == null)
                {
                    return new Result
                    {
                        Message = string.Format("excel表第{0}行仓库名称不存在", (excelRow + 2)),
                        Status = false
                    };
                }
                //商品编号
                //var ErpCode = dt.Rows[i][_dic["ErpCode"]].ToString().Trim();
                //PdProduct pEntity = PdProductBo.Instance.GetProductByErpCode(ErpCode);
                //if (pEntity == null)
                //{
                //    return new Result
                //    {
                //        Message = string.Format("excel表第{0}行商品编号不存在", (excelRow + 2)),
                //        Status = false
                //    };
                //}
                //条形码
                var Barcode = dt.Rows[i][_dic["Barcode"]].ToString().Trim();
                var ProductInfo = BLL.Product.PdProductBo.Instance.GetProductByBarcode(Barcode);
                if (ProductInfo == null)
                {
                    return new Result
                    {
                        Message = string.Format("excel表第{0}行商品条码不存在", (excelRow + 2)),
                        Status = false
                    };
                }
                //日期
                var Time = dt.Rows[i][_dic["DateTime"]].ToString().Trim();
                //Decimal typeCostPrice = 0;
                //if (string.IsNullOrWhiteSpace(Time))
                //{
                //    return new Result
                //    {
                //        Message = string.Format("excel表第{0}日期不能为空", (excelRow + 2)),
                //        Status = false
                //    };
                //}
                //备注
                var Remack = dt.Rows[i][_dic["Remack"]].ToString().Trim();
               
                //库存数量
                var StockQuantity = dt.Rows[i][_dic["StockQuantity"]].ToString().Trim();
                Decimal typeStockQuantity = 0;
                if (!Decimal.TryParse(StockQuantity, out typeStockQuantity))
                {
                    return new Result
                    {
                        Message = string.Format("excel表第{0}行库存数量必须为数值", (excelRow + 2)),
                        Status = false
                    };
                }
                var model = new PdProductStock
                {
                    WarehouseSysNo = sEntity.SysNo,
                    PdProductSysNo = ProductInfo.SysNo,
                    Barcode = Barcode,
                    CustomsNo = string.Empty,
                    CostPrice = ProductInfo.CostPrice,
                    StockQuantity = decimal.Parse(StockQuantity),
                    CreatedBy = operatorSysno,
                    CreatedDate = DateTime.Now,
                    LastUpdateBy = operatorSysno,
                    LastUpdateDate = DateTime.Now,
                    InStockTime = Time,
                    Remark = Remack
                };
                excellst.Add(model);
            }
            #endregion

            #region 添加新增和修改到对应集合
            var lstExisted = DataAccess.Warehouse.IPdProductStockDao.Instance.GetAllProductStock();
            foreach (var excelModel in excellst)
            {
                if (lstExisted.Any(e => e.WarehouseSysNo == excelModel.WarehouseSysNo && e.PdProductSysNo == excelModel.PdProductSysNo))
                {
                    lstToUpdate.Add(excelModel);
                }
                else //insert
                {
                    lstToInsert.Add(excelModel);
                }
            }
            #endregion

            #region 修改或更新到数据库
            try
            {
                var user = Hyt.BLL.Sys.SyUserBo.Instance.GetSyUser(operatorSysno);
                IPdProductStockDao.Instance.CreateExcelProductStock(lstToInsert);
                IPdProductStockDao.Instance.UpdateExcelProductStock(lstToUpdate);
                foreach (var mod in lstToInsert)
                {
                    WhWarehouseChangeLog log1 = new WhWarehouseChangeLog()
                    {
                        WareSysNo = mod.WarehouseSysNo,
                        ProSysNo = mod.PdProductSysNo,
                        ChageDate = DateTime.Now,
                        CreateDate = DateTime.Now,
                        ChangeQuantity = Convert.ToInt32(mod.StockQuantity),
                        BusinessTypes = "导EXCEL库存修改",
                        LogData = user.UserName + " 通过导入EXCEL修改仓库库存数据，将库存修改为" + mod.StockQuantity
                    };
                    WhWarehouseChangeLogBo.Instance.CreateMod(log1);
                }

                foreach (var mod in lstToUpdate)
                {
                    WhWarehouseChangeLog log1 = new WhWarehouseChangeLog()
                    {
                        WareSysNo = mod.WarehouseSysNo,
                        ProSysNo = mod.PdProductSysNo,
                        ChageDate = DateTime.Now,
                        CreateDate = DateTime.Now,
                        ChangeQuantity = Convert.ToInt32(mod.StockQuantity),
                        BusinessTypes = "导EXCEL库存修改",
                        LogData = user.UserName + " 通过导入EXCEL修改仓库库存数据，将库存修改为" + mod.StockQuantity
                    };
                    WhWarehouseChangeLogBo.Instance.CreateMod(log1);
                }
            }
            catch (Exception ex)
            {
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "导入商品信息",
                                         LogStatus.系统日志目标类型.商品基本信息, 0, ex, null, operatorSysno);
                return new Result
                {
                    Message = string.Format("数据更新错误:{0}", ex.Message),
                    Status = false
                };
            }
            #endregion


            var msg = lstToInsert.Count > 0 ? string.Format("成功导入{0}条数据!", lstToInsert.Count) : "";
            msg += lstToUpdate.Count > 0 ? string.Format("成功修改{0}条数据!", lstToUpdate.Count) : "";
            return new Result
            {
                Message = msg,
                Status = true
            };
        }

        /// <summary>
        /// 商品条形码导入excel
        /// </summary>
        /// <param name="stream">导入的excel stream format</param>
        /// <param name="operatorSysno">操作人员编号</param>
        /// <returns>封装的泛型result对象</returns>
        /// <remarks>2017-06-23 罗勤尧 创建</remarks>
        public Result ImportExcelSnDate(Stream stream, int operatorSysno, IDictionary<string, string> dic = null)
        {
            #region Excel转DataTable
            DataTable dt = null;
            int warehouseSysNo = 0;
            IDictionary<string, string> _dic = DicColsMappingSnDate;
            string[] cols;
            if (dic == null)
                cols = DicColsMappingSnDate.Select(p => p.Value).ToArray();
            else
            {
                cols = dic.Values.ToArray();
                _dic = dic;
            }

            try
            {
                dt = ExcelUtil.ImportExcel(stream, cols);
            }
            catch (Exception ex)
            {
                //exception happened,some not caughted
                return new Result
                {
                    Message = ex.Message,
                    Status = false
                };
            }

            if (dt == null)
            {
                //not all the cols mapped
                return new Result
                {
                    Message = string.Format("请选择正确的excel文件!"),
                    Status = false
                };
            }
            #endregion

            var excellst = new List<PdProductStock>();
            var lstToInsert = new List<PdProductStock>();
            var lstToUpdate = new List<PdProductStock>();

            #region 遍历DataTable
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                int excelRow = i;
                for (var j = 0; j < dt.Columns.Count; j++)
                {
                    string columnName = dt.Columns[j].ColumnName;
                    //备注,日期可以为空
                    if (columnName != _dic["DateTime"])
                    {
                        if (string.IsNullOrWhiteSpace(dt.Rows[i][j].ToString()))
                        {
                            return new Result
                            {
                                Message = string.Format("excel表第{0}行{1}列数据不能有空值", (excelRow + 2), (j + 1)),
                                Status = false
                            };
                        }
                    }
                }


                //仓库名称
                var backWarehouseName = dt.Rows[i][_dic["BackWarehouseName"]].ToString().Trim();
                WhWarehouse sEntity = WhWarehouseBo.GetWarehouseByName(backWarehouseName);
                if (sEntity == null)
                {
                    return new Result
                    {
                        Message = string.Format("excel表第{0}行仓库名称不存在", (excelRow + 2)),
                        Status = false
                    };
                }
                warehouseSysNo = sEntity.SysNo;
               
                //商品编号
                var ErpCode = dt.Rows[i][_dic["ErpCode"]].ToString().Trim();
                PdProduct pEntity = PdProductBo.Instance.GetProductByErpCode(ErpCode);
                if (pEntity == null)
                {
                    return new Result
                    {
                        Message = string.Format("excel表第{0}行商品编号不存在", (excelRow + 2)),
                        Status = false
                    };
                }
                //条形码
                //var Barcode = dt.Rows[i][_dic["Barcode"]].ToString().Trim();
                //var ProductInfo = BLL.Product.PdProductBo.Instance.GetProductByBarcode(Barcode);
                //if (ProductInfo == null)
                //{
                //    return new Result
                //    {
                //        Message = string.Format("excel表第{0}行商品条码不存在", (excelRow + 2)),
                //        Status = false
                //    };
                //}
                //日期
                var Time = dt.Rows[i][_dic["DateTime"]].ToString().Trim();
                //Decimal typeCostPrice = 0;
                //if (string.IsNullOrWhiteSpace(Time))
                //{
                //    return new Result
                //    {
                //        Message = string.Format("excel表第{0}日期不能为空", (excelRow + 2)),
                //        Status = false
                //    };
                //}
                //备注
                //var Remack = dt.Rows[i][_dic["Remack"]].ToString().Trim();

                //库存数量
                //var StockQuantity = dt.Rows[i][_dic["StockQuantity"]].ToString().Trim();
                //Decimal typeStockQuantity = 0;
                //if (!Decimal.TryParse(StockQuantity, out typeStockQuantity))
                //{
                //    return new Result
                //    {
                //        Message = string.Format("excel表第{0}行库存数量必须为数值", (excelRow + 2)),
                //        Status = false
                //    };
                //}
                var model = new PdProductStock
                {
                    WarehouseSysNo = sEntity.SysNo,
                    PdProductSysNo = pEntity.SysNo,
                    Barcode = pEntity.Barcode,
                    CustomsNo = string.Empty,
                    CostPrice = pEntity.CostPrice,
                    StockQuantity = 0,
                    CreatedBy = operatorSysno,
                    CreatedDate = DateTime.Now,
                    LastUpdateBy = operatorSysno,
                    LastUpdateDate = DateTime.Now,
                    InStockTime = Time,
                    Remark = ""
                };
                excellst.Add(model);
            }
            #endregion

            #region 添加新增和修改到对应集合
            var lstExisted = DataAccess.Warehouse.IPdProductStockDao.Instance.GetAllProductStock();
            foreach (var excelModel in excellst)
            {
                if (lstExisted.Any(e => e.WarehouseSysNo == excelModel.WarehouseSysNo && e.PdProductSysNo == excelModel.PdProductSysNo))
                {
                    lstToUpdate.Add(excelModel);
                }
                else //insert
                {
                    lstToInsert.Add(excelModel);
                }
            }
            #endregion

            #region 修改或更新到数据库
            try
            {
                var user = Hyt.BLL.Sys.SyUserBo.Instance.GetSyUser(operatorSysno);
               // IPdProductStockDao.Instance.CreateExcelProductStock(lstToInsert);
                IPdProductStockDao.Instance.UpdateExcelProductStockDate(lstToUpdate);
                foreach (var mod in lstToInsert)
                {
                    WhWarehouseChangeLog log1 = new WhWarehouseChangeLog()
                    {
                        WareSysNo = mod.WarehouseSysNo,
                        ProSysNo = mod.PdProductSysNo,
                        ChageDate = DateTime.Now,
                        CreateDate = DateTime.Now,
                        ChangeQuantity = Convert.ToInt32(mod.StockQuantity),
                        BusinessTypes = "导EXCEL库存修改",
                        LogData = user.UserName + " 通过导入EXCEL修改仓库库存数据，将库存修改为" + mod.StockQuantity
                    };
                    WhWarehouseChangeLogBo.Instance.CreateMod(log1);
                }

                foreach (var mod in lstToUpdate)
                {
                    WhWarehouseChangeLog log1 = new WhWarehouseChangeLog()
                    {
                        WareSysNo = mod.WarehouseSysNo,
                        ProSysNo = mod.PdProductSysNo,
                        ChageDate = DateTime.Now,
                        CreateDate = DateTime.Now,
                        ChangeQuantity = Convert.ToInt32(mod.StockQuantity),
                        BusinessTypes = "导EXCEL库存修改",
                        LogData = user.UserName + " 通过导入EXCEL修改仓库库存数据，将库存修改为" + mod.StockQuantity
                    };
                    WhWarehouseChangeLogBo.Instance.CreateMod(log1);
                }
            }
            catch (Exception ex)
            {
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "导入商品信息",
                                         LogStatus.系统日志目标类型.商品基本信息, 0, ex, null, operatorSysno);
                return new Result
                {
                    Message = string.Format("数据更新错误:{0}", ex.Message),
                    Status = false
                };
            }
            #endregion


            //var msg = lstToInsert.Count > 0 ? string.Format("成功导入{0}条数据!", lstToInsert.Count) : "";
          var  msg = lstToUpdate.Count > 0 ? string.Format("成功修改{0}条数据!", lstToUpdate.Count) : "";
            return new Result
            {
                Message = msg,
                Status = true
            };
        }

        public Result ImportExcelYS(Stream stream, int operatorSysno)
        {
            DataTable dt = null;
            int WarehouseSysNo = 0;
            var cols = DicColsMappingYS.Select(p => p.Value).ToArray();

            try
            {
                dt = ExcelUtil.ImportExcel(stream, cols);
            }
            catch (Exception ex)
            {
                //exception happened,some not caughted
                return new Result
                {
                    Message = string.Format("数据导入错误,请选择正确的excel文件"),
                    Status = false
                };
            }
            if (dt == null)
            {
                //not all the cols mapped
                return new Result
                {
                    Message = string.Format("请选择正确的excel文件!"),
                    Status = false
                };
            }
            var excellst = new List<PdProductStock>();
            var lstToInsert = new List<PdProductStock>();
            var lstToUpdate = new List<PdProductStock>();
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                int excelRow = i;
                for (var j = 0; j < dt.Columns.Count; j++)
                {
                    //条形码、海关备案号可以为空
                    if (j != 3)
                    {
                        if (j != 5)
                        {
                            if ((dt.Rows[i][j] == null || string.IsNullOrEmpty(dt.Rows[i][j].ToString())))
                            {
                                return new Result
                                {
                                    Message = string.Format("excel表第{0}行第{1}列数据不能有空值", (excelRow + 1), j + 1),
                                    Status = false
                                };
                            }
                        }
                    }
                    
                }
                //仓库名称
                var BackWarehouseName = dt.Rows[i][DicColsMappingYS["BackWarehouseName"]].ToString().Trim();
                WhWarehouse sEntity = WhWarehouseBo.GetWarehouseByName(BackWarehouseName);
                WarehouseSysNo = 0;
                if (sEntity == null)
                {
                    return new Result
                    {
                        Message = string.Format("excel表第{0}行仓库名称不存在", (excelRow + 1)),
                        Status = false
                    };
                }
                else
                {
                    WarehouseSysNo = sEntity.SysNo;
                }
                //商品编号
                var ErpCode = dt.Rows[i][DicColsMappingYS["ErpCode"]].ToString().Trim();
                PdProduct pEntity = PdProductBo.Instance.GetProductByErpCode(ErpCode);
                if (pEntity == null)
                {
                    return new Result
                    {
                        Message = string.Format("excel表第{0}行商品编号不存在", (excelRow + 1)),
                        Status = false
                    };
                }
                //条形码
                var Barcode = dt.Rows[i][DicColsMappingYS["Barcode"]].ToString().Trim();
                //商品SKU
                var ProductSku = dt.Rows[i][DicColsMappingYS["ProductSku"]].ToString().Trim();
                //海关备案号
                var CustomsNo = dt.Rows[i][DicColsMappingYS["CustomsNo"]].ToString().Trim();
                //采购价格
                var CostPrice = dt.Rows[i][DicColsMappingYS["CostPrice"]].ToString().Trim();
                Decimal typeCostPrice = 0;
                if (!Decimal.TryParse(CostPrice, out typeCostPrice))
                {
                    return new Result
                    {
                        Message = string.Format("excel表第{0}行采购价格必须为数值", (excelRow + 1)),
                        Status = false
                    };
                }
                //库存数量
                var StockQuantity = dt.Rows[i][DicColsMappingYS["StockQuantity"]].ToString().Trim();
                Decimal typeStockQuantity = 0;
                if (!Decimal.TryParse(StockQuantity, out typeStockQuantity))
                {
                    return new Result
                    {
                        Message = string.Format("excel表第{0}行库存数量必须为数值", (excelRow + 1)),
                        Status = false
                    };
                }
                var model = new PdProductStock
                {
                    WarehouseSysNo = sEntity.SysNo,
                    PdProductSysNo = pEntity.SysNo,
                    Barcode = Barcode,
                    ProductSku=ProductSku,
                    CustomsNo = CustomsNo,
                    CostPrice = decimal.Parse(CostPrice),
                    StockQuantity = decimal.Parse(StockQuantity),
                    CreatedBy = operatorSysno,
                    CreatedDate = DateTime.Now,
                    LastUpdateBy = operatorSysno,
                    LastUpdateDate = DateTime.Now
                };
                excellst.Add(model);
            }
            var lstExisted = DataAccess.Warehouse.IPdProductStockDao.Instance.GetAllProductStock();
            foreach (var excelModel in excellst)
            {
                if (lstExisted.Any(e => e.WarehouseSysNo == excelModel.WarehouseSysNo && e.PdProductSysNo == excelModel.PdProductSysNo))
                {
                    lstToUpdate.Add(excelModel);
                }
                else //insert
                {
                    lstToInsert.Add(excelModel);
                }
            }
            try
            {
                IPdProductStockDao.Instance.CreateExcelProductStockYS(lstToInsert);
                IPdProductStockDao.Instance.UpdateExcelProductStockYS(lstToUpdate);
            }
            catch (Exception ex)
            {
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "导入商品信息",
                                         LogStatus.系统日志目标类型.商品基本信息, 0, ex, null, operatorSysno);
                return new Result
                {
                    Message = string.Format("数据更新错误:{0}", ex.Message),
                    Status = false
                };
            }
            var msg = lstToInsert.Count > 0 ? string.Format("成功导入{0}条数据!", lstToInsert.Count) : "";
            msg += lstToUpdate.Count > 0 ? string.Format("成功修改{0}条数据!", lstToUpdate.Count) : "";
            return new Result
            {
                Message = msg,
                Status = true
            };
        }
        #endregion

        #endregion

        /// <summary>
        /// 获取库房商品明细
        /// </summary>
        /// <param name="warehouseNo">库房编号</param>
        /// <returns></returns>
        public List<PdProductStock> GetPdProductStockList(int warehouseNo)
        {
            return IPdProductStockDao.Instance.GetPdProductStockList(warehouseNo);
        }

        public void SavePdProductStock(PdProductStock tempProduct)
        {
            IPdProductStockDao.Instance.Insert(tempProduct);
        }

        /// <summary>
        /// 查分销商绑定库存商品列表的分页数据列表
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        ///<remarks>
        /// 2016-03-14 杨云奕 添加
        /// </remarks>
        public Pager<PdProductStockList> DoDealerPdProductStockDetailQuery(ParaProductStockFilter filter)
        {
            return IPdProductStockDao.Instance.DoDealerPdProductStockDetailQuery(filter);
        }

        /// <summary>
        /// 导入盘点进度
        /// </summary>
        /// <param name="stream">导入excel流</param>
        /// <param name="warehouseSysNo">仓库编号</param>
        /// <param name="inventorySysNo">盘点单编号</param>
        /// <param name="operatorSysno">操作人</param>
        /// <returns></returns>
        ///<remarks>
        /// 2016-08-19 刘伟豪 创建
        /// 2016-8-26 杨浩 修改仓库产品库存为一次性读取完再用linq检查是否存在
        /// </remarks>
        public Result ImportInventoryDetail(Stream stream, int warehouseSysNo, int inventorySysNo, int operatorSysno)
        {
            var DicColsMapping = new Dictionary<string, string>
            {
                {"ProductSysNo", "商品编码"},
                {"Barcode", "商品条码"},
                {"EasName", "商品名称"},
                {"StockQuantity", "库存数量"},
                {"RealStock", "实盘数量"},
                {"WarehousePositionName","库位"}
            };

            DataTable dt = null;
            var cols = DicColsMapping.Select(p => p.Value).ToArray();

            try
            {
                dt = ExcelUtil.ImportExcel(stream, cols);
            }
            catch
            {
                //exception happened,some not caughted
                return new Result
                {
                    Message = string.Format("数据导入错误,请选择正确的excel文件"),
                    Status = false
                };
            }
            if (dt == null)
            {
                //not all the cols mapped
                return new Result
                {
                    Message = string.Format("请选择正确的excel文件!"),
                    Status = false
                };
            }

            //如果为创建盘点单，则新建
            if (inventorySysNo == 0)
            {
                var newInventory = new WhInventory()
                {
                    Status = 0,
                    TransactionSysNo = BLL.Basic.ReceiptNumberBo.Instance.GetCheckOrderNo(),
                    WarehouseSysNo = warehouseSysNo,
                    Remarks = "",
                    CreatedBy = operatorSysno,
                    StartDate = DateTime.Now,
                    EndDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                };
                newInventory = BLL.Warehouse.WhInventoryBo.Instance.CreateWhInventory(newInventory);
                inventorySysNo = newInventory.SysNo;
            }

            var excellst = new List<WhInventoryDetail>();
            var lstToInsert = new List<WhInventoryDetail>();
            var lstToUpdate = new List<WhInventoryDetail>();

            var productStockList=IPdProductStockDao.Instance.GetProStockByWarehouseSysNo(warehouseSysNo);

            if (productStockList == null)
            {
                return new Result
                {
                    Message = string.Format("仓库没有商品可盘点!"),
                    Status = false
                };
            }


            int errorNum = 0;
            foreach (DataRow row in dt.Rows)
            {
                //商品
                var ProductSysNo = 0;
                if (!int.TryParse(row[DicColsMapping["ProductSysNo"]].ToString().Trim(), out ProductSysNo))
                {
                    errorNum++;
                    continue;
                }

                //实盘数量
                var realStock = 0;
                if (!int.TryParse(row[DicColsMapping["RealStock"]].ToString().Trim(), out realStock))
                {
                    errorNum++;
                    continue;
                }               

                //检查仓库是否有入库过
                var productStockInfo = productStockList.FirstOrDefault(x => x.PdProductSysNo == ProductSysNo);
                if (productStockInfo==null)
                {
                    errorNum++;
                    continue;
                }

                var model = new WhInventoryDetail
                {
                    InventorySysNo = inventorySysNo,
                    ProductStockSysNo = productStockInfo.SysNo,
                    RealStock = realStock,
                    WarehouseSysNo = warehouseSysNo,
                    ProductSysNo = ProductSysNo
                };
                excellst.Add(model);
            }

            var lstExisted = IWhInventoryDao.Instance.GetWhInventoryProducts(inventorySysNo);
            foreach (var excelModel in excellst)
            {
                var oModel = lstExisted.FirstOrDefault(e => e.InventorySysNo == excelModel.InventorySysNo && e.WarehouseSysNo == excelModel.WarehouseSysNo && e.ProductSysNo == excelModel.ProductSysNo);

                if (oModel != null)
                {
                    excelModel.SysNo = oModel.SysNo;
                    lstToUpdate.Add(excelModel);
                }
                else //insert
                {
                    lstToInsert.Add(excelModel);
                }
            }
            try
            {
                IWhInventoryDao.Instance.InsertWhInventoryDetail(lstToInsert);
                IWhInventoryDao.Instance.UpdateWhInventoryDetail(lstToUpdate);
            }
            catch (Exception ex)
            {
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "导入盘点进度信息",
                                         LogStatus.系统日志目标类型.商品基本信息, 0, ex, null, operatorSysno);
                return new Result
                {
                    Message = string.Format("数据更新错误:{0}", ex.Message),
                    Status = false
                };
            }
            var msg = string.Format("共{0}条数据：", dt.Rows.Count);
            msg += lstToInsert.Count > 0 ? string.Format("成功导入{0}条数据!", lstToInsert.Count) : "";
            msg += lstToUpdate.Count > 0 ? string.Format("成功修改{0}条数据!", lstToUpdate.Count) : "";
            msg += string.Format("失败{0}条!", errorNum);
            return new Result
            {
                Message = msg,
                Status = true,
                StatusCode = inventorySysNo,
            };
        }

        public IList<PdProductStock> GetAllStockList()
        {
            return IPdProductStockDao.Instance.GetAllProductStock();
        }

        public PdProductStock GetPdProductStockBySysNo(int SysNo)
        {
            return IPdProductStockDao.Instance.GetPdProductStockBySysNo(SysNo);
        }

        public IList<PdProductStock> GetAllStockList(int warehouseSysNo , IList<int> proSysNos)
        {
            return IPdProductStockDao.Instance.GetAllStockList(warehouseSysNo,proSysNos);
        }
        /// <summary>
        /// 同步ERP仓库库存
        /// </summary>
        /// <param name="wwarehouseSysNo">仓库系统编号</param>
        /// <param name="pids">产品系统编号（多个逗号分隔）</param>
        /// <returns></returns>
        /// <remarks>2017-1-10 杨浩 创建</remarks>
        public bool SynchronizeErpStock(int warehouseSysNo,string pids="")
        {
            var pager=new Pager<PdProduct>()
            {
                PageSize=99999,
                CurrentPage=1,
               
            };

            var productList=BLL.Product.PdProductBo.Instance.GetPdProductList(pager).Rows;
            pids = pids.Trim().Trim(',');
            if (pids != "")
            {
                pids = "," + pids + ",";
                productList=productList.Where(x => pids.Contains(string.Format(",{0},", x.SysNo))).ToList();
                if (productList == null || productList.Count <= 0)
                    return false;
            }
          
            var warehouseInfo=BLL.Warehouse.WhWarehouseBo.Instance.GetWarehouse(warehouseSysNo);

            var instance=KisProviderFactory.CreateProvider();   
            int recCount = productList.Count;     
            var recordIndex =0;
            var returnValue = new Dictionary<string,int>();

            foreach (var item in productList)
            {       
               if (!returnValue.Keys.Contains(item.ErpCode))              
                   returnValue.Add(item.ErpCode,item.SysNo);
               //if (item.ErpCode == "04.01.11.01" || item.ErpCode == "04.01.19.02" || item.ErpCode == "05.02.02.10" || item.ErpCode == "05.02.02.09")
               // {
               //    int i =0;
               // }
               recordIndex++;

               //if (returnValue.Count>0&&(recordIndex % 20 == 0 || recordIndex >= recCount))
                   if (returnValue.Count > 0 &&recordIndex <= recCount)
               {               
                   var inventoryList = instance.GetInventory(null,returnValue.Keys.ToArray(),warehouseInfo.ErpCode, warehouseSysNo);
                   if (inventoryList.Status)
                   {
                       foreach (var _item in inventoryList.Data)
                       {
                           if (returnValue.Keys.Contains(_item.MaterialNumber))
                           {
                               int row = IPdProductStockDao.Instance.SynchronizeStock(warehouseSysNo, returnValue[_item.MaterialNumber], _item.Quantity);
                               if (row <= 0)
                               {
                                   var model = new PdProductStock();
                                   model.CreatedDate = DateTime.Now;
                                   model.CreatedBy = BLL.Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo;
                                   model.LastUpdateBy = model.CreatedBy;
                                   model.LastUpdateDate = DateTime.Now;
                                   model.PdProductSysNo = returnValue[_item.MaterialNumber];
                                   model.StockQuantity = _item.Quantity;
                                   model.WarehouseSysNo = warehouseSysNo;
                                   IPdProductStockDao.Instance.Insert(model);
                               }                     
                           }

                       }
                   }
                   returnValue.Clear();                                       
               }            
            }         
         
            return true;
        }

        /// <summary>
        /// 获取无效库存的产品系统编号列表
        /// </summary>
        /// <returns></returns>
        /// <remarks>2017-3-23 杨浩 创建</remarks>
        public IList<int> GetInvalidInventoryProductSysNoList()
        {
            return IPdProductStockDao.Instance.GetInvalidInventoryProductSysNoList();
        }
    }
}