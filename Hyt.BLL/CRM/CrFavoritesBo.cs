using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.CRM;
using Hyt.Model;
using Hyt.Model.Transfer;

namespace Hyt.BLL.CRM
{
    /// <summary>
    /// 顾客关注商品业务类
    /// </summary>
    /// <remarks>
    /// 2013-08-26 郑荣华 创建
    /// </remarks>
    public class CrFavoritesBo : BOBase<CrFavoritesBo>
    {

        /// <summary>
        /// 添加商品关注
        /// </summary>
        /// <param name="crSysNo">顾客系统编号</param>
        /// <param name="pdSysNo">商品系统编号</param>
        /// <returns>已添加则返回-1，未添加则返回新增的系统编号</returns>
        /// <remarks>
        /// 2013-08-26 郑荣华 创建
        /// </remarks>
        public int Create(int crSysNo, int pdSysNo)
        {
            var model = new CrFavorites { CustomerSysNo = crSysNo, ProductSysNo = pdSysNo, CreateDate = DateTime.Now };
            var r = Create(model);
            //if (r != -1)
            //{
            //    //加商品静态统计数据                
            //    Hyt.BLL.Product.PdProductStatisticsBo.Instance.Update("Favorites");
            //}
            return r;
        }

        /// <summary>
        /// 添加商品关注
        /// </summary>
        /// <param name="model">商品关注实体</param>        
        /// <returns>已添加则返回-1，未添加则返回新增的系统编号</returns>
        /// <remarks>
        /// 2013-08-26 郑荣华 创建
        /// </remarks>
        public int Create(CrFavorites model)
        {
            return ICrFavoritesDao.Instance.Create(model);
        }

        /// <summary>
        /// 删除商品关注
        /// </summary>
        /// <param name="crSysNo">顾客系统编号</param>
        /// <param name="pdSysNo">商品系统编号</param>
        /// <returns>受影响行数</returns>
        /// <remarks>
        /// 2013-10-31 郑荣华 创建
        /// </remarks>
        public int Delete(int crSysNo, int pdSysNo)
        {
            return ICrFavoritesDao.Instance.Delete(crSysNo, pdSysNo);
        }

        /// <summary>
        /// 删除商品关注
        /// </summary>
        /// <param name="sysNo">商品关注系统编号</param>
        /// <returns>受影响行数</returns>
        /// <remarks>
        /// 2013-08-26 郑荣华 创建
        /// </remarks>
        public int Delete(int sysNo)
        {
            return ICrFavoritesDao.Instance.Delete(sysNo);
        }

        /// <summary>
        /// 获取顾客关注的商品
        /// </summary>
        /// <param name="crSysNo">顾客系统编号</param>
        /// <param name="pdSysNo">商品编号</param>
        /// <param name="pager">分页对象</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-08-26 郑荣华 创建
        /// </remarks>
        public void GetCrFavoritesList(int? crSysNo, int? pdSysNo, ref Pager<CBCrFavorites> pager)
        {
            ICrFavoritesDao.Instance.GetCrFavoritesList(crSysNo, pdSysNo, ref pager);
        }

        /// <summary>
        /// 是否已关注
        /// </summary>
        /// <param name="customerSysNo">顾客系统编号</param>
        /// <param name="productSysNo">商品编号</param>
        /// <returns>bool是否已关注</returns>
        /// <remarks> 2013-10-28 杨浩 创建 </remarks>
        public bool IsAttention(int customerSysNo, int productSysNo)
        {
           return ICrFavoritesDao.Instance.IsAttention(customerSysNo, productSysNo);
        }

        /// <summary>
        /// 获取已关注数量
        /// </summary>
        /// <param name="customerSysNo">顾客系统编号</param>
        /// <returns>关注数量</returns>
        /// <remarks> 2013-10-28 杨浩 创建 </remarks>
        public int GetAttentionCount(int customerSysNo)
        {
            return ICrFavoritesDao.Instance.GetAttentionCount(customerSysNo);
        }

        /// <summary>
        /// 获取收藏
        /// </summary>
        /// <param name="sysNo">收藏的系统编号</param>
        /// <returns>收藏实体</returns>
        /// <remarks>2013-09-12 杨晗 创建</remarks>
        public CrFavorites GetFavorites(int sysNo)
        {
            return ICrFavoritesDao.Instance.GetFavorites(sysNo);
        }

    }
}
