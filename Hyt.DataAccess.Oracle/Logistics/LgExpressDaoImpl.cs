using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Logistics;
using Hyt.Model;

namespace Hyt.DataAccess.Oracle.Logistics
{
    /// <summary>
    /// 物流配送信息数据访问类
    /// </summary>
    /// <remarks>
    /// 2014-04-04 余勇 创建
    /// </remarks>
    public class LgExpressDaoImpl : ILgExpressDao
    {
        #region 操作

        /// <summary>
        /// 插入物流信息
        /// </summary>
        /// <param name="model">物流信息实体</param>
        /// <returns>物流信息sysNo</returns>
        /// <remarks> 
        /// 2014-04-04 余勇 创建
        /// </remarks>
        public override int Insert(LgExpressInfo model)
        {
            return Context.Insert("LgExpressInfo", model)
                         .AutoMap(x => x.SysNo)
                         .ExecuteReturnLastId<int>("SysNo");
        }

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
        public override IList<LgExpressInfo> GetExpressInfo(string transactionSysNo)
        {
            return Context.Sql("select * from LgExpressInfo where TransactionSysNo=@0 order by ExpressNo", transactionSysNo)
                          .QueryMany<LgExpressInfo>();
        }

        /// <summary>
        /// 通过物流配送信息编号查询物流配送日志
        /// </summary>
        /// <param name="expressInfoSysNo">物流配送信息系统编号</param>
        /// <returns>物流配送日志列表</returns>
        /// <remarks> 
        /// 2014-04-04 余勇 创建
        /// </remarks>
        public override IList<LgExpressLog> GetLgExpressLog(int expressInfoSysNo)
        {
            return Context.Sql("select * from LgExpressLog where ExpressInfoSysNo=@0 order by LogTime ", expressInfoSysNo)
                          .QueryMany<LgExpressLog>();
        }
        #endregion
    }
}
