using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
namespace Hyt.DataAccess.Logistics
{
    /// <summary>
    /// 配送员信用额度维护 抽象类
    /// </summary>
    /// <remarks>2013-06-09 沈强 创建</remarks>
    public abstract class ILgDeliveryUserCreditDao : Hyt.DataAccess.Base.DaoBase<ILgDeliveryUserCreditDao>
    {
        #region 操作

        /// <summary>
        /// 添加配送员(业务员)信用配额
        /// </summary>
        /// <param name="model">配送员(业务员)信用配额</param>
        /// <returns>添加是否成功</returns>
        /// <remarks>2013-06-19 郑荣华 创建</remarks>
        public abstract bool Create(LgDeliveryUserCredit model);

        /// <summary>
        /// 更新配送员(业务员)信用配额
        /// </summary>
        /// <param name="model">配送员(业务员)信用配额</param>
        /// <returns>更新是否成功</returns>
        /// <remarks>2013-06-09 沈强 创建</remarks>
        public abstract bool Update(LgDeliveryUserCredit model);

        /// <summary>
        /// 删除配送员信用信息
        /// </summary>
        /// <param name="deliveryUserSysNo">配送员系统编号</param>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks>2013-06-21 郑荣华 创建</remarks>
        public abstract bool Delete(int deliveryUserSysNo, int warehouseSysNo);

        #endregion

        #region 查询

        /// <summary>
        /// 获取配送员(业务员)信用配额集合
        /// </summary>
        /// <param name="pager">配送员(业务员)信用配额集合分页对象</param>
        /// <returns></returns>
        /// <remarks>2013-06-20 郑荣华 创建</remarks>
        public abstract void GetLgDeliveryUserCreditList(ref Pager<CBLgDeliveryUserCredit> pager);

        /// <summary>
        /// 查询配送员(业务员)信用配额集合
        /// </summary>
        /// <param name="pager">配送员(业务员)信用配额集合分页对象</param>
        /// <param name="filter">配送员信用查询条件</param>
        /// <returns></returns>
        /// <remarks>2013-06-21 郑荣华 创建</remarks>
        public abstract void GetLgDeliveryUserCreditList(ref Pager<CBLgDeliveryUserCredit> pager, ParaDeliveryUserCreditFilter filter);

        /// <summary>
        /// 根据配送员系统编号获取信用配额信息
        /// </summary>
        /// <param name="deliveryUserSysNo">配送员系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-06-09 沈强 创建</remarks>
        public abstract IList<CBLgDeliveryUserCredit> GetLgDeliveryUserCredit(int deliveryUserSysNo);

        /// <summary>
        /// 根据配送员和仓库系统编号获取信用配额信息
        /// </summary>
        /// <param name="deliveryUserSysNo">配送员系统编号</param>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <returns>配送员信用额度组合实体</returns>
        /// <remarks>2013-07-17 郑荣华 创建</remarks>
        public abstract CBLgDeliveryUserCredit GetLgDeliveryUserCredit(int deliveryUserSysNo, int warehouseSysNo);

        #endregion
    }
}
