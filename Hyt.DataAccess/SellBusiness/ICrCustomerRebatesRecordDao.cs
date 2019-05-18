using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Model.Parameter;

namespace Hyt.DataAccess.SellBusiness
{
    /// <summary>
    /// 返利记录信息
    /// </summary>
    /// <param name="filter">返利记录信息</param>
    /// <returns>返回返利记录信息</returns>
    /// <remarks>2015-09-15 王耀发 创建</remarks>
    public abstract class ICrCustomerRebatesRecordDao : Hyt.DataAccess.Base.DaoBase<ICrCustomerRebatesRecordDao>
    {
        /// <summary>
        /// 获取返利记录列表
        /// </summary>
        /// <param name="sysNo">返利记录系统编号</param>
        /// <returns>返利记录列表</returns>
        /// <remarks>2015-08-30 王耀发 创建</remarks>
        public abstract Pager<CBCrCustomerRebatesRecord> GetCrCustomerRebatesRecordList(ParaCustomerRebatesRecordFilter filter);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="WarehouseSysNo"></param>
        /// <param name="PdProductSysNo"></param>
        /// <returns></returns>
        /// <remarkss>2015-08-06 王耀发 创建</remarks>
        public abstract CrCustomerRebatesRecord GetEntity(int SysNo);

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2015-08-06 王耀发 创建</remarks>
        public abstract int Insert(CrCustomerRebatesRecord entity);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>修改记录编号</returns>
        /// <remarks>2015-08-21  王耀发 创建</remarks>
        public abstract int Update(CrCustomerRebatesRecord entity);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>删除记录</returns>
        /// <remarks>2015-08-30 王耀发 创建</remarks>
        public abstract int Delete(int sysNo);


        /// <summary>
        /// 获得返利记录列表(按订单号升序排列)排除没有完成的退换货订单
        /// </summary>
        /// <param name="delayDay">获取可以执行返利的记录的天数</param>
        /// <param name="dealerSysNo">分销商编号</param>
        /// <param name="orderSysNo">订单系统编号</param>
        /// <remarks>
        /// 2015-10-19 杨云奕 添加
        /// 2016-1-13 杨浩 修改
        /// </remarks>
        public abstract List<CrCustomerRebatesRecord> GetRebatesRecordList(int delayDay, int dealerSysNo = 0, int orderSysNo=0);

        /// <summary>
        /// 设置返利佣金操作
        /// </summary>
        /// <param name="SysNo">返利佣金列表</param>
        /// <remarks>2015-10-19 杨云奕 添加</remarks>
        public abstract void SetCrCustomerRebatesRecordToCustomerBrokerage(int SysNo);
        /// <summary>
        /// 获取订单可返点的状态
        /// </summary>
        /// <param name="orderSysNo">订单编号</param>
        /// <returns></returns>
        /// <remarks>2016-5-13 杨浩 创建</remarks>
        public abstract int GetOrderRebatesStatus(int orderSysNo);
    }
}

