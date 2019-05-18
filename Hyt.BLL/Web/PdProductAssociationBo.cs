using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Infrastructure.Caching;
using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.BLL.Product;

namespace Hyt.BLL.Web
{

    /// <summary>
    /// 关联商品
    /// </summary>
    /// <remarks>2013-08-07 邵斌 创建</remarks>
    public class PdProductAssociationBo : BOBase<PdProductAssociationBo>
    {
        /// <summary>
        /// 获取商品的关联商品
        /// </summary>
        /// <param name="relationCode">关联关系码</param>
        /// <param name="productSysNo">商品系统编号</param>
        /// <returns>返回 关联商品</returns>
        /// <remarks>2013-08-07 邵斌 创建</remarks>
        public IList<CBProductAssociation> GetProductAssociation(string relationCode,int productSysNo)
        {

            return CacheManager.Get(CacheKeys.Items.ProductAssociation_, relationCode, delegate
                {

                    return Hyt.DataAccess.Product.IPdProductAssociationDao.Instance.ProductList(productSysNo);

                });
        }
    }
}
