using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model.Generated;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;

namespace Hyt.DataAccess.MallSeller
{
    public abstract class IDsMallSyncLogDao : DaoBase<IDsMallSyncLogDao>
    {
        /// <summary>
        /// 创建升舩商城同步日志表
        /// </summary>
        /// <param name="model">升舩商城同步日志表</param>
        /// <returns>创建的升舩商城同步日志表sysNo</returns>
        /// <remarks> 
        /// 2014-07-28 杨文兵 创建
        /// </remarks>
        public abstract int Create(DsMallSyncLog model);


        #region 获取日志中需要同步的订单
        /// <summary>
        /// 获取日志中需要同步的订单
        /// </summary>
        /// <returns>订单日志</returns>
        /// <remarks>
        /// 吴琨 2017-8-30 创建
        /// 2017-11-02 杨浩 增加sysno参数，为0代表忽略此参数 否则加入查询条件
        /// </remarks>
        /// 
        public abstract List<DsMallSyncLog> GetSynchroOrder(int sysno=0);


        /// <summary>
        /// 获取商城订单号和商城类型
        /// </summary>
        /// <returns>商城订单号</returns>
        /// 吴琨 2017-8-30 创建
        public abstract SynchroMallSyncLog GetOrder(string orderTransactionSysNo);


        /// <summary>
        /// 获取物流公司名称
        /// </summary>
        /// <returns>商城订单号</returns>
        /// 吴琨 2017-8-30 创建
        public abstract string GetDeliveryName(int deliveryType);


        /// <summary>
        /// 更新同步日志状态
        /// </summary>
        /// <param name="Message">修改状态备注</param>
        /// <param name="Status">状态  等待(0),成功(10),失败(20),作废(-10)</param>
        /// <param name="SysNo">修改的系统编号</param>
        /// <param name="elapsedTime">执行接口时间</param>
        /// <returns></returns>
        /// 吴琨 2017-8-31 创建
        public abstract bool UpdateDsMallSyncLogStatus(string message, int status, int sysNo, int elapsedMilliseconds);


        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>日志分页</returns>
        /// <remarks>2017-11-1 杨浩 创建</remarks>
        public abstract Pager<CBDsMallSyncLog> GetList(ParaDsMallSyncLogFilter filter);
        #endregion


        
    }
}
