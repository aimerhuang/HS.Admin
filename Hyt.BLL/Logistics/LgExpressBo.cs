using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Logistics;
using Hyt.Model;

namespace Hyt.BLL.Logistics
{
    /// <summary>
    /// 物流配送信息
    /// </summary>
    /// <remarks>
    /// 2014-04-04 余勇 创建
    /// </remarks>
    public class LgExpressBo : BOBase<LgExpressBo>
    {
        /// <summary>
        /// 插入物流信息
        /// </summary>
        /// <param name="model"></param>
        /// <remarks> 
        /// 2014-04-04 余勇 创建
        /// </remarks>
        public void Insert(LgExpressInfo model)
        {
            ILgExpressDao.Instance.Insert(model);
        }

        /// <summary>
        /// 通过订单事务号查询物流信息
        /// </summary>
        /// <param name="transactionSysNo">订单事务号</param>
        /// <returns>物流信息</returns>
        /// <remarks> 
        /// 2014-04-10 余勇 创建
        /// </remarks>
        public IList<CBLgExpressInfo> GetExpressInfoByTransactionSysNo(string transactionSysNo)
        {
            var expressInfoList = ILgExpressDao.Instance.GetExpressInfo(transactionSysNo);
            return expressInfoList.Select(lgExpressInfo => new CBLgExpressInfo
            {
                LgExpressInfo = lgExpressInfo,
                LgExpressLog = ILgExpressDao.Instance.GetLgExpressLog(lgExpressInfo.SysNo)
            }).ToList();
        }

        /// <summary>
        /// 通过订单事务号查询物流配送日志
        /// </summary>
        /// <param name="transactionSysNo">订单事务号</param>
        /// <returns>物流配送日志列表</returns>
        /// <remarks> 
        /// 2014-04-10 余勇 创建
        /// </remarks>
        public IList<LgExpressLog> GetLgExpressLogByTransactionSysNo(string transactionSysNo)
        {
            var list = new List<LgExpressLog>();
            var expressInfoList = ILgExpressDao.Instance.GetExpressInfo(transactionSysNo);
            foreach (var expressInfo in expressInfoList)
            {
                list.AddRange(ILgExpressDao.Instance.GetLgExpressLog(expressInfo.SysNo));
            }
            return list;
        }


    }
}
