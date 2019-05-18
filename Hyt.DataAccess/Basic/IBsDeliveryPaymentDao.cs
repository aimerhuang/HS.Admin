using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;

namespace Hyt.DataAccess.Basic
{
    /// <summary>
    /// 配送方式支付方式关联抽象类
    /// </summary>
    /// <remarks>
    /// 2013-08-01 郑荣华 创建
    /// </remarks>
    public abstract class IBsDeliveryPaymentDao : DaoBase<IBsDeliveryPaymentDao>
    {
        #region 操作

        /// <summary>
        /// 创建配送方式支付方式信息
        /// </summary>
        /// <param name="model">配送方式支付方式信息实体</param>
        /// <returns>创建的配送方式支付方式信息sysNo</returns>
        /// <remarks> 
        /// 2013-08-02 郑荣华 创建
        /// </remarks>
        public abstract int Create(BsDeliveryPayment model);

        /// <summary>
        /// 更新配送方式支付方式信息
        /// </summary>
        /// <param name="model">配送方式支付方式信息实体，根据sysno</param>
        /// <returns>受影响的行数</returns>
        /// <remarks> 
        /// 2013-08-02 郑荣华 创建
        /// </remarks>
        public abstract int Update(BsDeliveryPayment model);

        /// <summary>
        /// 删除配送方式支付方式信息
        /// </summary>
        /// <param name="sysNo">要删除的配送方式支付方式信息系统编号</param>
        /// <returns>受影响的行数</returns>
        /// <remarks> 
        /// 2013-08-02 郑荣华 创建
        /// </remarks>
        public abstract int Delete(int sysNo);

        /// <summary>
        /// 删除配送方式支付方式关联信息
        /// </summary>
        /// <param name="paymentSysNo">支付方式系统编号</param>
        /// <param name="deliverySysNo">配送方式系统编号</param>
        /// <returns>受影响的行数</returns>
        /// <remarks> 
        /// 2013-08-02 郑荣华 创建
        /// </remarks>
        public abstract int Delete(int paymentSysNo, int deliverySysNo);
       
        #endregion

        #region 查询

        /// <summary>
        /// 查询配送方式支付方式关联信息
        /// </summary>
        /// <param name="pager">配送方式支付方式关联列表分页对象</param>
        /// <param name="filter">配送方式支付方式关联查询条件</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-08-02 郑荣华 创建
        /// </remarks>
        public abstract void GetBsDeliveryPaymentList(ref Pager<CBBsDeliveryPayment> pager,
                                                      ParaBsDeliveryPaymentFilter filter);

        /// <summary>
        /// 根据支付方式查询配送方式支付方式关联列表信息
        /// </summary>
        /// <param name="paymentSysNo">支付方式系统编号</param>
        /// <returns>配送方式支付方式关联列表</returns>
        /// <remarks>
        /// 2013-08-02 郑荣华 创建
        /// </remarks>
        public abstract IList<CBBsDeliveryPayment> GetListByPayment(int paymentSysNo);

        /// <summary>
        /// 根据配送方式查询配送方式支付方式关联列表信息
        /// </summary>
        /// <param name="deliverySysNo">配送方式系统编号</param>
        /// <returns>配送方式支付方式关联列表</returns>
        /// <remarks>
        /// 2013-08-02 郑荣华 创建
        /// </remarks>
        public abstract IList<CBBsDeliveryPayment> GetListByDelivery(int deliverySysNo);

        /// <summary>
        /// 配送和支付方式匹配条数
        /// </summary>
        /// <param name="paymentSysNo">支付方式编号</param>
        /// <param name="deliverySysNo">配送方式系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-09-12 黄志勇 创建</remarks>
        public abstract int GetBsDeliveryPaymentCount(int paymentSysNo, int deliverySysNo);

        #endregion
    }
}
