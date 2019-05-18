using Hyt.DataAccess.Base;
using Hyt.Model.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Order
{
    /// <summary>
    /// 订单同步物流日志
    /// </summary>
    /// <remarks>2016-7-29 杨浩 创建</remarks>
    public abstract class ISoOrderSyncLogisticsLogDao : DaoBase<ISoOrderSyncLogisticsLogDao>
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns></returns>
        public abstract int Insert(SoOrderSyncLogisticsLog model);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="orderSysNo">订单编号</param>
        /// <param name="code">物流代码</param>
        /// <returns></returns>
        public abstract int DeleteByOrderSysNoAndCode(int orderSysNo,int code);
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns></returns>
        public abstract int Update(SoOrderSyncLogisticsLog model);
        /// <summary>
        /// 获取订单同步物流日志实体
        /// </summary>
        /// <param name="orderSysNo">订单编号</param>
        /// <param name="code">物流代码</param>
        /// <returns></returns>
        public abstract SoOrderSyncLogisticsLog GetModel(int orderSysNo, int code);

        /// <summary>
        /// 获取订单同步物流对象集合
        /// </summary>
        /// <param name="orderSysno">订单编号</param>
        /// <returns>物流对象集合</returns>
        /// <remarks>2016-08-27 杨云奕 添加</remarks>
        public abstract List<SoOrderSyncLogisticsLog> GetModelList(int orderSysno);
    }
}
