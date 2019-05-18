using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Logistics;
using Hyt.Model;

namespace Hyt.DataAccess.Oracle.Logistics
{
    /// <summary>
    /// 配送物流公司编码信息数据访问类
    /// </summary>
    /// <remarks>
    /// 2014-04-08 余勇 创建
    /// </remarks>
    public class LgDeliveryCompanyCodeDaoImpl : ILgDeliveryCompanyCodeDao
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
        public override int Create(LgDeliveryCompanyCode model)
        {
            return Context.Insert("LgDeliveryCompanyCode", model)
                          .AutoMap(x => x.SysNo)
                          .ExecuteReturnLastId<int>("SysNo");
        }

        /// <summary>
        /// 更新配送物流公司编码信息
        /// </summary>
        /// <param name="model">配送物流公司编码信息实体</param>
        /// <returns>受影响的行数</returns>
        /// <remarks> 
        /// 2014-04-08 余勇 创建
        /// </remarks>
        public override int Update(LgDeliveryCompanyCode model)
        {
            return Context.Update("LgDeliveryCompanyCode", model)
                          .AutoMap(x => x.SysNo)
                          .Where(x => x.SysNo)
                          .Execute();
        }

        /// <summary>
        /// 删除配送物流公司编码信息
        /// </summary>
        /// <param name="sysNo">要删除的配送物流公司编码信息系统编号</param>
        /// <returns>受影响的行数</returns>
        /// <remarks> 
        /// 2014-04-08 余勇 创建
        /// </remarks>
        public override int Delete(int sysNo)
        {
            return Context.Delete("LgDeliveryCompanyCode")
                          .Where("SysNo", sysNo)
                          .Execute();
        }

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
        public override LgDeliveryCompanyCode GetLgDeliveryCompanyCode(int deliveryTypeSysNo)
        {
            const string sql = "select * from LgDeliveryCompanyCode where DeliveryTypeSysNo=@0";

            return Context.Sql(sql, deliveryTypeSysNo)
                          .QuerySingle<LgDeliveryCompanyCode>();
        }


        #endregion
    }
}
