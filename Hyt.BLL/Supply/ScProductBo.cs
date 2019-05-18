using Hyt.DataAccess.Supply;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.Supply
{
    /// <summary>
    /// 供应链产品
    /// </summary>
    /// <remarks>2016-3-17 杨浩 创建</remarks>
    public class ScProductBo : BOBase<ScProductBo>
    {
        /// <summary>
        /// 检测产品sku是否已存在
        /// </summary>
        /// <param name="sku">sku</param>
        /// <param name="supplyCode">供应链代码</param>
        /// <returns></returns>
        /// <remarks> 2016-3-18  杨浩 创建</remarks>
        public bool CheckProductSku(string sku, int supplyCode)
        {
            return IScProductDao.Instance.CheckProductSku(sku, supplyCode);
        }
        /// <summary>
        /// 添加供应链产品
        /// </summary>
        /// <param name="model">供应链商品实体</param>
        /// <returns></returns>
        /// <remarks> 2013-6-25 杨浩 创建</remarks>
        public int AddScProduct(ScProduct model)
        {
            model.CreateDate = DateTime.Now;
            model.UpdateDate = model.CreateDate;
            return IScProductDao.Instance.AddScProduct(model);
        }
        /// <summary>
        /// 更新供应链产品
        /// </summary>
        /// <param name="model">供应链商品实体</param>
        /// <returns></returns>
        /// <remarks> 2013-6-25 杨浩 创建</remarks>
        public bool UpdateScProduct(ScProduct model)
        {
            model.UpdateDate = DateTime.Now;
            return IScProductDao.Instance.UpdateScProduct(model) > 0;
        }
        /// <summary>
        /// 更新供应链商品
        /// </summary>
        /// <param name="SKU"></param>
        /// <param name="ProTitle"></param>
        /// <param name="Receipt"></param>
        /// <returns></returns>
        public bool UpdateScProduct(string SKU, string ProTitle, string Receipt)
        {
            return IScProductDao.Instance.UpdateScProduct(SKU, ProTitle, Receipt);
        }
        /// <summary>
        /// 删除供应链产品
        /// </summary>
        /// <param name="sysNo">供应商产品编号</param>
        /// <returns></returns>
        /// <remarks> 2013-6-25 杨浩 创建</remarks>
        public bool DeleteScProduct(int sysNo)
        {
            return IScProductDao.Instance.DeleteScProduct(sysNo) > 0;
        }
        /// <summary>
        /// 获取供应商产品详情
        /// </summary>
        /// <param name="sysNo">供应商产品编号</param>
        /// <returns></returns>
        /// <remarks> 2013-6-25 杨浩 创建</remarks>
        public ScProduct GetScProductInfo(int sysNo)
        {
            return IScProductDao.Instance.GetScProductInfo(sysNo);
        }
        /// <summary>
        /// 获取供应商产品详情
        /// </summary>
        /// <param name="sku">sku</param>
        /// <param name="supplyCode">供应链代码</param>
        /// <returns></returns>
        /// <remarks> 2016-3-17  杨浩 创建</remarks>
        public ScProduct GetScProductInfo(string sku, int supplyCode)
        {
            return IScProductDao.Instance.GetScProductInfo(sku, supplyCode);
        }
        /// <summary>
        /// 获取供应商产品详情，根据平台商品编号
        /// </summary>
        /// <param name="productSysNo">平台商品编号</param>
        /// <param name="supplyCode">供应链代码</param>
        /// <remarks> 2016-5-5 刘伟豪 创建</remarks>
        public ScProduct GetScProductInfo(int productSysNo, int supplyCode)
        {
            return IScProductDao.Instance.GetScProductInfo(productSysNo, supplyCode);
        }
        /// <summary>
        /// 获取供应链所有产品
        /// </summary>
        /// <param name="supplyCode">供应链代码</param>
        /// <returns></returns>
        /// <remarks> 2016-3-18  杨浩 创建</remarks>
        public IList<ScProduct> GetScProductList(int supplyCode)
        {
            return IScProductDao.Instance.GetScProductList(supplyCode);
        }
        /// <summary>
        /// 查询供应链产品
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        /// <remarks> 2013-6-25 杨浩 创建</remarks>
        /// <remarks> 2016-3-21 刘伟豪 修改</remarks>
        public void Seach(ref Pager<CBScProduct> pager, ParaSupplyProductFilter condition)
        {
            IScProductDao.Instance.Seach(ref pager, condition);
        }

        /// <summary>
        /// 更新表中的商品编号
        /// </summary>
        /// <param name="SysNo"></param>
        /// <param name="ProductSysNo"></param>
        /// <returns></returns>
        /// <remarks> 2016-4-25 王耀发 创建</remarks>
        public bool UpdateProductSysNo(int SysNo, int ProductSysNo)
        {
            var result = IScProductDao.Instance.UpdateProductSysNo(SysNo, ProductSysNo);
            return result;
        }
        /// <summary>
        /// 同步供应链商品到库存
        /// 王耀发 2016-5-5 创建
        /// </summary>
        /// <param name="Supply">供应链类型</param>
        /// <param name="PdProductSysNo">商品编号</param>
        /// <param name="StockQuantity">库存数量</param>
        /// <param name="CreatedBy">创建用户系统编号</param>
        /// <returns></returns>
        public int ProCreateSupplyStock(int Supply, int PdProductSysNo, decimal StockQuantity, int CreatedBy)
        {
            return IScProductDao.Instance.ProCreateSupplyStock(Supply, PdProductSysNo, StockQuantity, CreatedBy);
        }
        /// <summary>
        /// 根据商品ID更新商品详情
        /// </summary>
        /// <param name="ProductSysNo"></param>
        /// <param name="ProductDesc"></param>
        /// <returns></returns>
        public bool UpdatePdBySysNo(int ProductSysNo, string ProductDesc)
        {
            return IScProductDao.Instance.UpdatePdBySysNo(ProductSysNo, ProductDesc);
        }
    }
}