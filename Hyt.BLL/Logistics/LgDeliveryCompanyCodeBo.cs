using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Logistics;
using Hyt.Model;

namespace Hyt.BLL.Logistics
{
    /// <summary>
    /// 配送物流公司编码
    /// </summary>
    /// <remarks>
    /// 2014-04-04 余勇 创建
    /// </remarks>
    public class LgDeliveryCompanyCodeBo : BOBase<LgDeliveryCompanyCodeBo>
    {
        /// <summary>
        /// 插入物流信息
        /// </summary>
        /// <param name="model"></param>
        public void Insert(LgDeliveryCompanyCode model)
        {

        }

        /// <summary>
        /// 根据配送方式系统编号获取配送物流公司编码信息
        /// </summary>
        /// <param name="deliveryTypeSysNo">配送方式系统编号</param>
        /// <returns>配送物流公司编码信息</returns>
        /// <remarks> 
        /// 2014-04-08 余勇 创建
        /// </remarks>
        public LgDeliveryCompanyCode GetLgDeliveryCompanyCode(int deliveryTypeSysNo)
        {
          return ILgDeliveryCompanyCodeDao.Instance.GetLgDeliveryCompanyCode(deliveryTypeSysNo);
        }
    }
}
