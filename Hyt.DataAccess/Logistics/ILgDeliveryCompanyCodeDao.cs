using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;

namespace Hyt.DataAccess.Logistics
{

    /// <summary>
    /// 百城当日达区域信息抽象类
    /// </summary>
    /// <remarks>
    /// 2013-08-01 郑荣华 创建
    /// </remarks>
    public abstract class ILgDeliveryCompanyCodeDao : DaoBase<ILgDeliveryCompanyCodeDao>
    {
        #region 操作

        /// <summary>
        /// 创建配送物流公司编码信息
        /// </summary>
        /// <param name="model">配送物流公司编码信息实体</param>
        /// <returns>创建的配送物流公司编码信息sysNo</returns>
        /// <remarks> 
        /// 2014-04-08 余勇 创建
        /// </remarks>
        public abstract int Create(LgDeliveryCompanyCode model);

        /// <summary>
        /// 更新配送物流公司编码信息
        /// </summary>
        /// <param name="model">配送物流公司编码信息实体</param>
        /// <returns>受影响的行数</returns>
        /// <remarks> 
        /// 2014-04-08 余勇 创建
        /// </remarks>
        public abstract int Update(LgDeliveryCompanyCode model);

        /// <summary>
        /// 删除配送物流公司编码信息
        /// </summary>
        /// <param name="sysNo">要删除的配送物流公司编码信息系统编号</param>
        /// <returns>受影响的行数</returns>
        /// <remarks> 
        /// 2014-04-08 余勇 创建
        /// </remarks>
        public abstract int Delete(int sysNo);

        #endregion

        #region 查询

        /// <summary>
        /// 根据配送方式系统编号获取配送物流公司编码信息
        /// </summary>
        /// <param name="deliveryTypeSysNo">配送方式系统编号</param>
        /// <returns>配送物流公司编码信息</returns>
        /// <remarks> 
        /// 2014-04-08 余勇 创建
        /// </remarks>
        public abstract LgDeliveryCompanyCode GetLgDeliveryCompanyCode(int deliveryTypeSysNo);

        #endregion
    }
}
