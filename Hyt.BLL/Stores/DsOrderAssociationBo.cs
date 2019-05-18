using Hyt.DataAccess.Stores;
using Hyt.Model.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.Stores
{
    /// <summary>
    /// 经销商订单与系统订单关联
    /// </summary>
    /// <remarks>2016-9-7 杨浩 创建</remarks>
    public class DsOrderAssociationBo : BOBase<DsOrderAssociationBo>
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public  bool Add(DsOrderAssociation model)
        {
            return IDsOrderAssociationDao.Instance.Add(model);       
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Update(DsOrderAssociation model)
        {
            return IDsOrderAssociationDao.Instance.Update(model);
        }
        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="orderSysNo">系统订单编号</param>
        /// <param name="dealerSysNo">经销商系统编号</param>
        /// <returns></returns>
        public DsOrderAssociation GetOrderAssociationInfo(int dealerSysNo, string dealerOrderNo)
        {
            return IDsOrderAssociationDao.Instance.GetOrderAssociationInfo(dealerSysNo, dealerOrderNo);
        }
    }
}
