using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;

namespace Hyt.DataAccess.Logistics
{

    /// <summary>
    /// 第三方快递快递信息抽象类
    /// </summary>
    /// <remarks>
    /// 2014-04-04 余勇 创建
    /// </remarks>
    public abstract class ILgExpressDao : DaoBase<ILgExpressDao>
    {
        #region 操作

        /// <summary>
        /// 插入物流信息
        /// </summary>
        /// <param name="model">物流信息实体</param>
        /// <remarks> 
        /// 2014-04-04 余勇 创建
        /// </remarks>
        public abstract int Insert(LgExpressInfo model);

        #endregion

        #region 查询

        /// <summary>
        /// 通过订单事务号查询物流信息
        /// </summary>
        /// <param name="transactionSysNo">订单事务号</param>
        /// <returns>物流信息</returns>
        /// <remarks> 
        /// 2014-04-04 余勇 创建
        /// </remarks>
        public abstract IList<LgExpressInfo> GetExpressInfo(string transactionSysNo);

        /// <summary>
        /// 通过物流配送信息编号查询物流配送日志
        /// </summary>
        /// <param name="expressInfoSysNo">物流配送信息系统编号</param>
        /// <returns>物流配送日志列表</returns>
        /// <remarks> 
        /// 2014-04-04 余勇 创建
        /// </remarks>
        public abstract IList<LgExpressLog> GetLgExpressLog(int expressInfoSysNo);

        #endregion
    }
}
