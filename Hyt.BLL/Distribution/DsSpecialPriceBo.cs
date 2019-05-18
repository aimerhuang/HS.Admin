using System.Collections.Generic;
using Hyt.BLL.Log;
using System.Linq;
using Hyt.DataAccess.Distribution;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Model.Parameter;
using System.Data;
using Hyt.Util;
namespace Hyt.BLL.Distribution
{
    /// <summary>
    /// 分销商产品特殊价格
    /// </summary>
    /// <remarks>
    /// 2013-08-19 周唐炬 创建
    /// </remarks>
    public class DsSpecialPriceBo : BOBase<DsSpecialPriceBo>
    {
        /// <summary>
        /// 创建分销商产品特殊价格
        /// </summary>
        /// <param name="model">分销商产品特殊价格实体</param>
        /// <returns>系统编号</returns>
        /// <remarks>2013-09-04 周瑜 创建</remarks>
        public int Create(DsSpecialPrice model)
        {
            var sysNo = IDsSpecialPriceDao.Instance.Create(model);
            SysLog.Instance.Info(LogStatus.系统日志来源.后台, "创建分销商产品特殊价格", LogStatus.系统日志目标类型.分销商特殊价格, sysNo);
            return sysNo;
        }

        /// <summary>
        /// 更新分销商产品特殊价格
        /// </summary>
        /// <param name="model">分销商产品特殊价格实体</param>
        /// <returns>系统编号</returns>
        /// <remarks>2013-09-04 周瑜 创建</remarks>
        public int Update(DsSpecialPrice model)
        {
            SysLog.Instance.Info(LogStatus.系统日志来源.后台, "更新分销商产品特殊价格", LogStatus.系统日志目标类型.分销商特殊价格, model.SysNo);
            return IDsSpecialPriceDao.Instance.Update(model);
        }

        /// <summary>
        /// 删除特殊价格信息
        /// </summary>
        /// <param name="sysNo">特殊价格编号</param>
        /// <returns>删除特殊价格信息</returns>
        /// <remarks>2015-12-16 王耀发 创建</remarks>
        public Result Delete(int sysNo)
        {
            var res = new Result();
            var r = IDsSpecialPriceDao.Instance.Delete(sysNo);
            if (r > 0) res.Status = true;
            return res;
        }

        /// <summary>
        /// 修改特殊价格状态: 禁用/启用
        /// </summary>
        /// <param name="model">分销商产品特殊价格实体</param>
        /// <returns></returns>
        /// <remarks>2013-09-04 周瑜 创建</remarks>
        public int UpdateStatus(DsSpecialPrice model)
        {
            SysLog.Instance.Info(LogStatus.系统日志来源.后台, "修改特殊价格状态", LogStatus.系统日志目标类型.分销商特殊价格, model.SysNo);
            return IDsSpecialPriceDao.Instance.UpdateStatus(model);
        }

        /// <summary>
        /// 修改价格
        /// </summary>
        /// <param name="model">分销商产品特殊价格实体</param>
        /// <returns>返回影响行</returns>
        /// <remarks>2013-09-06 周瑜 创建</remarks>
        public int UpdatePrice(DsSpecialPrice model)
        {
            SysLog.Instance.Info(LogStatus.系统日志来源.后台, "修改价格", LogStatus.系统日志目标类型.分销商特殊价格, model.SysNo);
            return IDsSpecialPriceDao.Instance.UpdatePrice(model);
        }

        /// <summary>
        /// 快速搜索
        /// </summary>
        /// <param name="condition">搜索条件实体</param>
        /// <param name="pageIndex">页索引</param>
        /// <returns>符合搜索条件的实体集合</returns>
        /// <remarks>2013-09-04 周瑜 创建</remarks>
        public PagedList<CBDsSpecialPrice> QuickSearch(DsSpecialPriceSearchCondition condition, int pageIndex)
        {
            var model = new PagedList<CBDsSpecialPrice>();
            if (condition == null)
            {
                return model;
            }

            var result = IDsSpecialPriceDao.Instance.QuickSearch(condition, pageIndex, model.PageSize);
            model.TData = result.Rows;
            model.TotalItemCount = result.TotalRows;
            model.CurrentPageIndex = pageIndex;
            return model;
        }
        /// <summary>
        /// 获取商品详细信息列表
        /// </summary>
        /// <param name="pager">商品详细信息查询列表</param>
        /// <param name="dealerMallSysNo">分销商商城系统编号</param>
        /// <param name="condition">动态条件，CBPdProductDetail里不包含的条件</param>
        /// <returns></returns>
        /// <remarks>2015-12-10 王耀发 创建</remarks>
        public void GetSpecialPriceProductList(ref Pager<CBPdProductDetail> pager, ParaProductFilter condition)
        {
            IDsSpecialPriceDao.Instance.GetSpecialPriceProductList(ref pager,condition);
        }
        /// <summary>
        /// 获取经销商升舱信息
        /// </summary>
        /// <param name="pager">升舱信息查询列表</param>
        /// <param name="dsDetail">动态条件</param>
        /// <remarks>2017-8-23 罗熙 创建</remarks>
        public void GetDealerOrder(ref Pager<DsOrder> pager, ParaDsOrderFilter dsDetail)
        {
            IDsSpecialPriceDao.Instance.GetDealerOrder(ref pager, dsDetail);
        }
        /// <summary>
        /// 获取经销商退换货订单
        /// </summary>
        /// <param name="pager"></param>
        /// <param name="dsRMADetail">2017-8-29 罗熙 创建</param>
        public void GetDsRMAorder(ref Pager<DsReturn> pager, ParaDsReturnFilter dsRMADetail)
        {
            IDsSpecialPriceDao.Instance.GetDsRMAorder(ref pager, dsRMADetail);
        }
        
        /// <summary>
        /// 商城名称
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        public string GetmallName(int sysNo)
        {
            return IDsSpecialPriceDao.Instance.GetmallName(sysNo);
        }
        /// <summary>
        /// 获取分销商商城名称
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        public string GetdealerName(int sysNo)
        {
            return IDsSpecialPriceDao.Instance.GetdealerName(sysNo);
        }
        /// <summary>
        /// 获取升舱订单号
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns>2017-8-23 罗熙 创建</returns>
        public int GetorderSysNo(int sysNo)
        {
            return IDsSpecialPriceDao.Instance.GetorderSysNo(sysNo);
        }
        
        public string GetdealerSysNo(int sysNo)
        {
            return IDsSpecialPriceDao.Instance.GetdealerSysNo(sysNo);
        }
        /// <summary>
        /// 查看分销商详情
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns>2017-8-23 罗熙 创建</returns>
        public CBDsDealer GetDealerInfo(int sysNo)
        {
            return IDsSpecialPriceDao.Instance.GetDealerInfo(sysNo);
        }
        /// <summary>
        /// 查看经销商退换货订单信息
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns>2017-8-29 罗熙 创建</returns>
        public DsReturnItem GetRMADealerInfo(int sysNo)
        {
            return IDsSpecialPriceDao.Instance.GetRMADealerInfo(sysNo);
        }
        
        /// <summary>
        /// 升舱订单商品
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns>2017-8-24 罗熙 创建</returns>
        public List<DsOrderItem> GetDealerOrderPdInfo(int sysNo)
        {
            return IDsSpecialPriceDao.Instance.GetDealerOrderPdInfo(sysNo);
        }
        //public List<DsOrderItem> GetDsRMAorderPdInfo(int sysNo)
        //{
        //    return IDsSpecialPriceDao.Instance.GetDealerOrderPdInfo(sysNo);
        //}
        
        /// <summary>
        /// 查看升舱订单明细
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns>2017-8-23 罗熙 创建</returns>
        public CBDsOrder GetUpOrderInfo(int sysNo)
        {
            return IDsSpecialPriceDao.Instance.GetUpOrderInfo(sysNo);
        }
        /// <summary>
        /// 获取升舱订单
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns>2017-8-24 罗熙 创建</returns>
        public DsOrder GetUpOrderModel(int sysNo)
        {
            return IDsSpecialPriceDao.Instance.GetUpOrderModel(sysNo);
        }
        /// <summary>
        /// 2015-12-31 王耀发 创建
        /// </summary>
        /// <param name="ProductSysNo">商品编号</param>
        public Result DeleteByProSysNo(int ProductSysNo)
        {
            var res = new Result();
            var r = IDsSpecialPriceDao.Instance.DeleteByProSysNo(ProductSysNo);
            if (r > 0) res.Status = true;
            return res;
        }
        /// <summary>
        /// 2015-12-31 王耀发 创建
        /// </summary>
        /// <param name="DealerSysNo">分销商系统编号</param>
        /// <param name="ProductSysNo">商品编号</param>
        public Result DeleteDealerByProSysNo(int DealerSysNo,int ProductSysNo)
        {
            var res = new Result();
            var r = IDsSpecialPriceDao.Instance.DeleteDealerByProSysNo(DealerSysNo, ProductSysNo);
            if (r > 0) res.Status = true;
            return res;
        }
        /// <summary>
        /// 获取特殊价格信息
        /// </summary>
        /// <param name="dealerSysNo">分销商系统编号</param>
        /// <param name="productSysNo">商品编号</param>
        /// <returns>特殊价格信息</returns>
        /// <remarks>2016-1-3 王耀发 创建</remarks>
        public DsSpecialPrice GetEntityByDPSysNo(int dealerSysNo, int productSysNo)
        {
            return IDsSpecialPriceDao.Instance.GetEntityByDPSysNo(dealerSysNo, productSysNo);
        }
        /// <summary>
        /// 更新商品状态
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="sysNo">商品编号</param>
        /// <returns>更新行数</returns>
        /// <remarks>2016-1-3 王耀发 创建</remarks>
        public Result UpdateSSPriceStatus(int status, int sysNo)
        {
            Result result = new Result();
            //更新状态
            result = IDsSpecialPriceDao.Instance.UpdateSSPriceStatus(status, sysNo);
            return result;
        }
        /// <summary>
        /// 更新经销商商品状态
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="ProductSysNo">商品编号</param>
        /// <returns>更新行数</returns>
        /// <remarks>2016-1-12 王耀发 创建</remarks>
        public Result UpdatePriceStatusByPro(int status, int productSysNo)
        {
            Result result = new Result();
            //更新状态
            result = IDsSpecialPriceDao.Instance.UpdatePriceStatusByPro(status, productSysNo);
            return result;
        }

        /// <summary>
        /// 更新经销商商品状态
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="ProductSysNo">商品编号</param>
        ///  <param name="DealerSysNo">分销商编号编号</param>s
        /// <returns>更新行数</returns>
        /// <remarks>2017-9-12 罗勤尧 创建</remarks>
        public Result UpdatePriceStatusByPro(int status, int productSysNo, int DealerSysNo)
        {
            Result result = new Result();
            //更新状态
            result = IDsSpecialPriceDao.Instance.UpdatePriceStatusByPro(status, productSysNo, DealerSysNo);
            return result;
        }

        /// <summary>
        /// 更新商品状态
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="sysNo">商品编号</param>
        /// <returns>更新行数</returns>
        /// <remarks>2016-1-3 王耀发 创建</remarks>
        public Result UpdatePriceStatus(decimal price, int status, int sysNo)
        {
            Result result = new Result();
            //更新状态
            result = IDsSpecialPriceDao.Instance.UpdatePriceStatus(price, status, sysNo);
            return result;
        }
        /// <summary>
        /// 未选中时更新全部分销商商品状态
        /// </summary>
        /// <param name="DealerSysNo">分销商编号</param>
        /// <param name="status">状态</param>
        /// <returns>更新行数</returns>
        /// <remarks>2016-9-8 罗远康 创建</remarks>
        public Result UpdateAllPriceStatus(int DealerSysNo, int status)
        {
            Result result = new Result();
            //更新状态
            result = IDsSpecialPriceDao.Instance.UpdateAllPriceStatus(DealerSysNo, status);
            return result;
        }
        /// <summary>
        /// 更新商品状态
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="sysNo">系统编号编号</param>
        /// <returns>更新行数</returns>
        /// <remarks>2016-1-3 杨云奕 创建</remarks>
        public Result UpdatePriceStatus(decimal price, decimal shopPrice, int status, int sysNo)
        {
            Result result = new Result();
            //更新状态
            result = IDsSpecialPriceDao.Instance.UpdatePriceStatus(price,shopPrice, status, sysNo);
            return result;
        }
        /// <summary>
        /// 更新商品状态
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="sysNo">商品编号</param>
        /// <returns>更新行数</returns>
        /// <remarks>2016-11-14 杨云奕 创建</remarks>
        public Result UpdatePriceStatus(decimal price, decimal shopPrice, decimal wholesalePrice, int status, int sysNo)
        {
            Result result = new Result();
            //更新状态
            result = IDsSpecialPriceDao.Instance.UpdatePriceStatus(price, shopPrice,wholesalePrice, status, sysNo);
            return result;
        }
        /// <summary>
        /// 同步总部已上架商品到分销商商品表中
        /// 王耀发 2016-1-5 创建
        /// </summary>
        /// <param name="DealerSysNo">分销商系统编号</param>
        /// <param name="CreatedBy">创建用户系统编号</param>
        /// <returns></returns>
        public int ProCreateSpecialPrice(int DealerSysNo, int CreatedBy)
        {
            return IDsSpecialPriceDao.Instance.ProCreateSpecialPrice(DealerSysNo, CreatedBy);
        }

        /// <summary>
        /// 更新分销商商品价格
        /// </summary>
        /// <param name="ProductSysNos">分销商选中商品组</param>
        /// <param name="DealerSysNo">分销商系统编号</param>
        /// <param name="Percentage">修改价格百分比（传入值为除以100的值）</param>
        /// <returns>2016-09-06 罗远康 创建</returns>
        public int ProUpdateSpecialPrice(string ProductSysNos, int DealerSysNo, decimal Percentage)
        {
            return IDsSpecialPriceDao.Instance.ProUpdateSpecialPrice(ProductSysNos, DealerSysNo, Percentage);
        }
        /// <summary>
        /// 获取特殊价格信息
        /// </summary>
        /// <param name="SysNo">分销商商品系统编号</param>
        /// <returns>特殊价格信息</returns>
        /// <remarks>2016-2-24 王耀发 创建</remarks>
        public DsSpecialPrice GetEntityBySysNo(int SysNo)
        {
            return IDsSpecialPriceDao.Instance.GetEntityBySysNo(SysNo);
        }

        #region 商品特殊价格导入
        /// <summary>
        /// import cols mapping between DB and excel
        /// </summary>
        /// <remarks>2015-09-10 王耀发 创建</remarks>
        private static readonly Dictionary<string, string> DisColsMapping = new Dictionary<string, string>
            {
                {"ErpCode", "商品编码"},
                {"ProductName", "前台显示名称"},
                {"Barcode","条形码"},
                {"DsDealerName","分销商名称"},
                {"DsUserPrice","分销商会员价"},
                {"ShopPrice","门店销售价"},
                {"WholesalePrice","线下批发价"}
            };

        public Result ImportSpecialPriceExcel(System.IO.Stream stream, int operatorSysno)
        {
            var result = new Result();

            DataTable dt = null;

            var cols = DisColsMapping.Select(p => p.Value).ToArray();
            var existlst = new List<PdProduct>();
            try
            {
                dt = ExcelUtil.ImportExcel(stream, cols);
            }
            catch (System.Exception ex)
            {
                //exception happened,some not caughted
                result.Message = string.Format("数据导入错误,请选择正确的excel文件");
                result.Status = false;
                return result;
            }
            if (dt == null)
            {
                //not all the cols mapped             
                result.Message = string.Format("请选择正确的excel文件!");
                result.Status = false;
                return result;
            }
            string msg="";
            var excellst = new List<PdProductList>();
            var lstToInsert = new List<PdProductList>();
            var lstToUpdate = new List<PdProductList>();
            List<PdProduct> productList = Hyt.BLL.Product.PdProductBo.Instance.GetAllProductDataBase();
            List<DsSpecialPrice> specProductList = DsSpecialPriceBo.Instance.GetAllProductDsSpecialPrice();
            IList<DsDealer> dealerList = DsDealerBo.Instance.GetAllDealerList();
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                int excelRow = i + 2;

                for (var j = 0; j < dt.Columns.Count; j++)
                {
                    if ((dt.Rows[i][j] == null || string.IsNullOrEmpty(dt.Rows[i][j].ToString().Trim())))
                    {
                        result.Message = string.Format("excel表第{0}行第{1}列数据不能有空值", excelRow, (j + 1));
                        result.Status = false;
                        return result;
                    }
                }
                //商品编号
                var ErpCode = dt.Rows[i][DisColsMapping["ErpCode"]].ToString().Trim();
                //条形码.
                var Barcode = dt.Rows[i][DisColsMapping["Barcode"]].ToString().Trim();
                var DsDealerName = dt.Rows[i][DisColsMapping["DsDealerName"]].ToString().Trim();
                var DsUserPrice = dt.Rows[i][DisColsMapping["DsUserPrice"]].ToString().Trim();
                var ShopPrice = dt.Rows[i][DisColsMapping["ShopPrice"]].ToString().Trim();
                var WholesalePrice = dt.Rows[i][DisColsMapping["WholesalePrice"]].ToString().Trim();

                PdProduct tempProduct = productList.Find(p => p.ErpCode == ErpCode);
                List<DsDealer> tempDealer = dealerList.Where(p => p.DealerName == DsDealerName).ToList();
                if (tempProduct != null && tempDealer.Count > 0 && !string.IsNullOrEmpty(DsUserPrice) && !string.IsNullOrEmpty(ShopPrice))
                {
                    DsSpecialPrice tempSpecialMod = specProductList.Find(p => p.DealerSysNo == tempDealer[0].SysNo && p.ProductSysNo == tempProduct.SysNo);
                    DsSpecialPrice specialMod;
                    try
                    {
                        specialMod = new DsSpecialPrice()
                        {
                            ProductSysNo = tempProduct.SysNo,
                            DealerSysNo = tempDealer[0].SysNo,
                            CreatedBy = 1,
                            CreatedDate = System.DateTime.Now,
                            LastUpdateBy = 1,
                            LastUpdateDate = System.DateTime.Now,
                            Price = decimal.Parse(DsUserPrice),
                            ShopPrice = decimal.Parse(ShopPrice),
                            WholesalePrice = decimal.Parse(WholesalePrice),
                            Status = 1
                        };
                    }
                    catch(System.Exception e) {
                        if (!string.IsNullOrEmpty(msg))
                        {
                            msg += ",";
                        }
                        msg += "商品编码:" + Barcode + "出现异常-" + e.Message;
                        continue;
                    }
                    if(tempSpecialMod==null)
                    {
                        DsSpecialPriceBo.Instance.Create(specialMod);
                    }
                    else
                    {
                        specialMod.SysNo = tempSpecialMod.SysNo;
                        DsSpecialPriceBo.Instance.Update(specialMod);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(msg))
                    {
                        msg += ",";
                    }
                    if (tempProduct == null)
                    {
                        msg += "商品编码:" + Barcode + "出现异常-档案中无当前编码商品";
                    }
                    if (tempDealer.Count==0)
                    {
                        msg += "商品编码:" + Barcode + "出现异常-无指定分销商存在";
                    }
                    if (string.IsNullOrEmpty(DsUserPrice))
                    {
                        msg += "商品编码:" + Barcode + "出现异常-分销商会员价格不能为空";
                    }
                    if (string.IsNullOrEmpty(ShopPrice))
                    {
                        msg += "商品编码:" + Barcode + "出现异常-分销商线下门店价格不能为空";
                    }
                   
                }
            }
            if(string.IsNullOrEmpty(msg))
            {
                result.Message = "操作成功";
            }
            else
            {
                result.Message = "操作成功-其中有部分商品有异常" + msg;
            }
         
            result.Status = true;
            return result;
        }

        private List<DsSpecialPrice> GetAllProductDsSpecialPrice()
        {
            return IDsSpecialPriceDao.Instance.GetAllProductDsSpecialPrice();
        }

        #endregion


    }
}
