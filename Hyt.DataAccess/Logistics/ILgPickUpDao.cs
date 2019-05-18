using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Logistics
{
    /// <summary>
    /// 取件单抽象类
    /// </summary>
    /// <remarks>
    /// 2013-07-05 郑荣华 创建
    /// </remarks>
    public abstract class ILgPickUpDao : DaoBase<ILgPickUpDao>
    {
        /// <summary>
        /// 查询取件单
        /// </summary>
        /// <param name="filter">查询条件实体</param>
        /// <returns>返回取件单列表</returns>
        /// <remarks>2013-08-12 周唐炬 创建</remarks>
        public abstract Pager<LgPickUp> GetPickUpList(ParaPickUpFilter filter);

        /// <summary>
        /// 查询取件单
        /// </summary>
        /// <param name="pager">分页对象</param>
        /// <param name="filter">查询条件实体</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-07-04 郑荣华 创建
        /// </remarks>
        public abstract void GetPickUpList(ref Pager<CBLgPickUp> pager, ParaPickUpFilter filter);

        /// <summary>
        /// 获取取件单
        /// </summary>
        /// <param name="pickUpSysNo">The pick up sys no.</param>
        /// <returns>返回取件单</returns>
        /// <remarks>
        /// 2013/7/6 何方 创建
        /// </remarks>
        public abstract LgPickUp GetPickUp(int pickUpSysNo);

        /// <summary>
        /// 获取取件单商品项
        /// </summary>
        /// <param name="pickUpSysNo">取件单系统编号</param>
        /// <returns>取件单商品项</returns>
        /// <remarks>2013-08-13 周唐炬 创建</remarks>
        public abstract List<LgPickUpItem> GetLgPickUpItem(int pickUpSysNo);

        /// <summary>创建取件单
        /// </summary>
        /// <param name="model">实体.</param>
        /// <returns>返回系统编号</returns>
        /// <remarks>
        /// 2013-07-12 何方 创建
        /// </remarks>
        public abstract int Create(LgPickUp model);

        /// <summary>
        /// 批量获取
        /// </summary>
        /// <param name="pickUpSysNos">取件单系统编号数组.</param>
        /// <returns>返回取件单列表</returns>
        /// <remarks>
        /// 2013/7/6 何方 创建
        /// </remarks>
        public abstract IList<LgPickUp> GetPickUp(int[] pickUpSysNos);

        /// <summary>
        /// 修改取件单状态
        /// </summary>
        /// <param name="pickUpSysNo">取件单系统编号.</param>
        /// <param name="status">状态.</param>
        /// <returns>返回修改状态</returns>
        /// <remarks>
        /// 2013-07-14 何方 创建
        /// </remarks>
        public abstract bool UpdateStatus(int pickUpSysNo, LogisticsStatus.取件单状态 status);

        /// <summary>
        /// 获取取件单列表
        /// </summary>
        /// <param name="arrSysNos">取件单系统编号</param>
        /// <returns>取件单列表</returns>
        /// <remarks>黄伟 2013-11-22 创建</remarks>
        public abstract List<LgPickUp> GetLgPickUpList(int[] arrSysNos);
    }
}
