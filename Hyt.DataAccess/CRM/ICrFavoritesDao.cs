using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Transfer;

namespace Hyt.DataAccess.CRM
{
    /// <summary>
    /// 顾客关注商品抽象类
    /// </summary>
    /// <remarks>
    /// 2013-08-26 郑荣华 创建
    /// </remarks>
    public abstract class ICrFavoritesDao : DaoBase<ICrFavoritesDao>
    {
        /// <summary>
        /// 添加商品关注
        /// </summary>
        /// <param name="model">商品关注实体</param>        
        /// <returns>已添加则返回-1，未添加则返回新增的系统编号</returns>
        /// <remarks>
        /// 2013-08-26 郑荣华 创建
        /// </remarks>
        public abstract int Create(CrFavorites model);

        /// <summary>
        /// 删除商品关注
        /// </summary>
        /// <param name="crSysNo">顾客系统编号</param>
        /// <param name="pdSysNo">商品系统编号</param>
        /// <returns>受影响行数</returns>
        /// <remarks>
        /// 2013-10-31 郑荣华 创建
        /// </remarks>
        public abstract int Delete(int crSysNo, int pdSysNo);

        /// <summary>
        /// 删除商品关注
        /// </summary>
        /// <param name="sysNo">商品关注系统编号</param>
        /// <returns>受影响行数</returns>
        /// <remarks>
        /// 2013-08-26 郑荣华 创建
        /// </remarks>
        public abstract int Delete(int sysNo);

        /// <summary>
        /// 获取顾客关注的商品
        /// </summary>
        /// <param name="crSysNo">顾客系统编号</param>
        /// <param name="pdSysNo">商品编号</param>
        /// <param name="pager">分页对象</param>
        /// <remarks>
        /// 2013-08-26 郑荣华 创建
        /// </remarks>
        public abstract void GetCrFavoritesList(int? crSysNo, int? pdSysNo, ref Pager<CBCrFavorites> pager);

        /// <summary>
        /// 获取关注商品
        /// </summary>
        /// <param name="customerSysNo">会员系统编号</param>
        /// <param name="productSysNo">商品系统编号</param>
        /// <returns>关注实体</returns>
        /// <remarks>2013-09-12 邵斌 创建</remarks>
        public abstract CrFavorites GetFavorites(int customerSysNo, int productSysNo);

        /// <summary>
        /// 获取收藏
        /// </summary>
        /// <param name="sysNo">收藏的系统编号</param>
        /// <returns>收藏实体</returns>
        /// <remarks>2013-09-12 杨晗 创建</remarks>
        public abstract CrFavorites GetFavorites(int sysNo);

        /// <summary>
        /// 是否已关注
        /// </summary>
        /// <param name="customerSysNo">顾客系统编号</param>
        /// <param name="productSysNo">商品编号</param>
        /// <remarks> 2013-10-28 杨浩 创建 </remarks>
        public abstract bool IsAttention(int customerSysNo, int productSysNo);

        /// <summary>
        /// 获取已关注数量
        /// </summary>
        /// <param name="customerSysNo">顾客系统编号</param>
        /// <remarks> 2013-10-28 杨浩 创建 </remarks>
        public abstract int GetAttentionCount(int customerSysNo);
    }
}
