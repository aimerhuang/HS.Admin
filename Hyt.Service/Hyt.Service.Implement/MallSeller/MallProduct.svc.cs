using Hyt.Service.Contract.MallSeller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Hyt.Infrastructure.Pager;
using Hyt.Model.Parameter;
using Hyt.Model;
using Hyt.Model.UpGrade;

namespace Hyt.Service.Implement.MallSeller
{
    /// <summary>
    ///商城商品编码维护实现类
    /// </summary>
    /// <remarks>2013-8-28 陶辉 创建</remarks>
    public class MallProduct : BaseService, IMallProduct
    {
        /// <summary>
        /// 搜索商城商品信息
        /// </summary>
        /// <param name="keyword">第三方商品名称关键字</param>
        /// <returns>商品列表</returns>
        /// <remarks>2013-8-28 陶辉 创建</remarks>
        public Result<List<UpGradeProduct>> GetProductList(string keyword)
        {
            Result<List<UpGradeProduct>> r = new Result<List<UpGradeProduct>>()
            {
                Status = true,
                StatusCode = 0,
                Data = new List<UpGradeProduct>()

            };
            if (string.IsNullOrEmpty(keyword))
            {
                return r;
            }
            //返回结果
            var pager = new Hyt.Model.Pager<ParaProductSearchFilter>();
            pager.CurrentPage = 1;
            pager.PageSize = 10;
            //设置查询条件
            pager.PageFilter = new ParaProductSearchFilter
            {
                ProductName = keyword,
                ErpCode = keyword
            };

            PagedList<ParaProductSearchFilter> list; //分页结果集
            Hyt.BLL.Product.PdProductBo.Instance.ProductSelectorProductSearch(ref pager, out list); //执行查询
            if (list != null && list.TData != null)
            {
                r.Data = list.TData.Select(m => new UpGradeProduct()
                {
                    HytProductCode = m.ErpCode,
                    HytProductSysNo = m.SysNo,
                    HytProductName = m.ProductName,
                    HytPrice = m.Price

                }).ToList();
            }
            return r;
        }

        /// <summary>
        /// 匹配商品编码
        /// </summary>
        /// <param name="dealerSysNo">分销商系统编号</param>
        /// <param name="mallProductCode">商城商品编码</param>
        /// <param name="mallProductAttrs">商城商品属性，多属性用英文半角分号隔开</param>
        /// <param name="productId">商城商品编号</param>
        /// <returns>处理结果</returns>
        /// <remarks>2013-8-28 陶辉 创建</remarks>
        public Result MapProductCode(int dealerSysNo, string mallProductCode, string mallProductAttrs, string productId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取分销商所有已匹配的商品编码
        /// </summary>
        /// <param name="param">查询参数</param>
        /// <returns>商品列表</returns>
        /// <remarks>2013-8-28 陶辉 创建</remarks>
        public Result<PagedList<UpGradeProduct>> GetMapProductList(MapProductParameters param)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 根据第三方商品编码和商品属性匹配商城商品编码
        /// </summary>
        /// <param name="dealerSysNo">分销商系统编号</param>
        /// <param name="mallProductCode">第三方商品编码</param>
        /// <param name="mallProductAttrs">第三方商品属性</param>
        /// <returns>商城商品编码</returns>
        /// <remarks>2013-8-28 陶辉 创建</remarks>
        public Result<List<string>> GetHytProductCode(int dealerSysNo, string mallProductCode, string mallProductAttrs)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取产品分销商价格
        /// </summary>
        /// <param name="dealerSysNo">分销商编号</param>
        /// <param name="hytProductID">产品编号</param>
        /// <returns>产品分销商价格</returns>
        /// <remarks>2013-09-11 朱成果 创建</remarks> 
        public Result<decimal> GetHytPrice(int dealerSysNo, int hytProductID)
        {
            Result<decimal> r = new Result<decimal>()
            {
                Status = true,
                Data = 0
            };
            try
            {
                r.Data = Hyt.BLL.MallSeller.DsOrderBo.Instance.GetDsSpecialPrice(dealerSysNo, hytProductID);
            }
            catch (Exception ex)
            {
                r.Message = ex.Message;
                r.Status = false;
            }
            return r;
        }

        /// <summary>
        /// 获取关联的商城产品详情
        /// </summary>
        /// <param name="dealerMallSysNo">分销商商城系统编号</param>
        /// <param name="mallProductId">商城商品编码</param>
        /// <returns>关联的商城产品详情</returns>
        /// <remarks>2013-09-13  朱成果 创建</remarks>
        public Result<UpGradeOrderItem> MapHytProduct(int dealerMallSysNo, string mallProductId)
        {
            Result<UpGradeOrderItem> r = new Result<UpGradeOrderItem>()
            {
                Status = true
            };
            try
            {
                var data = Hyt.BLL.MallSeller.DsOrderBo.Instance.GetAssociationHytProduct(dealerMallSysNo, mallProductId);
                if (data != null)
                {
                    r.Data = new UpGradeOrderItem()
                    {
                        MallProductCode = mallProductId,
                        HytProductName = data.HytProductName,
                        HytProductSysNo = data.HytProductSysNo,
                        MallProductAttrs = data.MallProductAttr,
                        HytProductErpCode = data.HytProductErpCode,
                        HytPrice = data.SpecialPrice > 0 ? data.SpecialPrice: data.PdPrice //优先取分销商特殊价格
                    };
                }
                else
                {
                    r.Status = false;
                }
            }
            catch (Exception ex)
            {
                r.Message = ex.Message;
                r.Status = false;
            }
            return r;

        }
    }
}
