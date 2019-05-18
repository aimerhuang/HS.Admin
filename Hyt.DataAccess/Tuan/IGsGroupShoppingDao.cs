using System;
using System.Collections.Generic;
using Hyt.Model;
using Hyt.DataAccess.Base;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Tuan
{
    /// <summary>
    /// 团购数据访问 抽象类 
    /// </summary>
    /// <remarks>2013-09-03 苟治国 创建</remarks>
    public abstract class IGsGroupShoppingDao:DaoBase<IGsGroupShoppingDao>
    {
        /// <summary>
        /// 今日推荐团购列表
        /// </summary>
        /// <param name="pager">查询参数</param>
        /// <returns>分页</returns>
        /// <remarks>2013-09-03 苟治国 创建</remarks>
        public abstract Pager<GsGroupShopping> GetPagingList(Pager<GsGroupShopping> pager);

        /// <summary>
        /// 团购分页列表
        /// </summary>
        /// <param name="pager">查询参数</param>
        /// <returns>分页</returns>
        /// <remarks>2013-09-03 苟治国 创建</remarks>
        public abstract Pager<GsGroupShopping> GetList(Pager<GsGroupShopping> pager);

        /// <summary>
        /// 团购分页列表 已审核,未过期的
        /// </summary>
        /// <param name="pager">查询参数</param>
        /// <returns>分页</returns>
        /// <remarks>2013-09-16 苟治国 创建</remarks>
        public abstract Pager<GsGroupShopping> GetGroupShoppingList(Pager<GsGroupShopping> pager);

        /// <summary>
        /// 查询一条未过期的团购
        /// </summary>
        /// <param name="pager">查询参数</param>
        /// <returns>一条未过期的团购</returns>
        /// <remarks>2013-09-24 苟治国 创建</remarks>
        public abstract GsGroupShopping GetGroupShopping(Pager<GsGroupShopping> pager);

        /// <summary>
        /// 获取团购数量
        /// </summary>
        /// <param name="type">团购类型</param>
        /// <param name="status">团购状态</param>
        /// <param name="startTime">起始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>团购数量</returns>
        /// <remarks>2013-09-04 杨晗 创建</remarks>
        public abstract int GetCount(GroupShoppingStatus.团购类型 type, GroupShoppingStatus.团购状态 status, DateTime? startTime,
                                     DateTime? endTime);

        /// <summary>
        /// 通过商品系统编号获取团购系统编号
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <returns>返回商品系统编号 0：表示没有团购</returns>
        /// <remarks>2013-09-09 邵斌 创建</remarks>
        public abstract int GetGroupShoppingSysNoByProduct(int productSysNo);

        /// <summary>
        /// 为手机客户端查询最新的一条团购
        /// </summary>
        /// <returns>最新的一条团购信息</returns>
        /// <remarks>2013-09-24 周瑜 创建</remarks>
        public abstract GsGroupShopping GetNewGroupShoppingForApp();

        /// <summary>
        /// 根据商品系统编号 获取团购价格
        /// </summary>
        /// <param name="sysNo">商品系统编号</param>
        /// <returns>团购价格</returns>
        /// <remarks>2013-10-31 杨浩 创建</remarks>
        public abstract decimal GetGroupShoppingPrice(int sysNo);
    }
}
