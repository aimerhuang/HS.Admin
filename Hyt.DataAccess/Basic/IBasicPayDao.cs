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
    /// 支付Dao
    /// </summary>
    /// <remarks>2013-09-06 周唐炬 创建</remarks>
    public abstract class IBasicPayDao : DaoBase<IBasicPayDao>
    {
        /// <summary>
        /// 添加新支付方式
        /// </summary>
        /// <param name="model">支付方式实体</param>
        /// <returns>受影响行</returns>
        /// <remarks>2013-09-06 周唐炬 创建</remarks>
        public abstract int PaymentTypeCreate(BsPaymentType model);

        /// <summary>
        /// 修改支付方式
        /// </summary>
        /// <param name="model">支付方式实体</param>
        /// <returns>受影响行</returns>
        /// <remarks>2013-09-06 周唐炬 创建</remarks>
        public abstract int PaymentTypeUpdate(BsPaymentType model);
        
        /// <summary>
        /// 获取所有支付方式
        /// </summary>
        /// <returns>配送方式</returns>
        /// <remarks>
        /// 2013-06-13 黄志勇 创建
        /// </remarks>
        public abstract IList<BsPaymentType> LoadAllPayType();

       /// <summary>
       /// 根据配送方式获取对应的支付方式列表
       /// </summary>
       /// <param name="deliverySysNo">配送方式</param>
        /// <returns>根据配送方式获取对应的支付方式列表</returns>
        /// <remarks>
        /// 2013-06-17 朱成果 创建
        /// </remarks>
        public abstract IList<BsPaymentType> LoadPayTypeListByDeliverySysNo(int deliverySysNo);

        /// <summary>
        /// 根据主键获取BsPaymentType实体
        /// </summary>
        /// <param name="sysNo">id</param>
        /// <returns>实体</returns>
        /// <remarks>2013-06-20 朱家宏 创建</remarks>
        public abstract BsPaymentType GetPaymentType(int sysNo);

        /// <summary>
        /// 获取所有支付方式
        /// </summary>
        /// <returns>配送方式</returns>
        /// <remarks>
        /// 2013-08-08 郑荣华 创建
        /// </remarks>
        public abstract IList<CBBsPaymentType> GetAll();

        /// <summary>
        /// 获取支付方式列表
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns>支付方式列表</returns>
        /// <remarks>
        /// 2013-08-20 郑荣华 创建
        /// </remarks>
        public abstract IList<CBBsPaymentType> GetPaymentTypeList(ParaPaymentTypeFilter filter);

        /// <summary>
        /// 支付名称验证
        /// </summary>
        /// <param name="paymentName">支付名称</param>
        /// <param name="sysNo">支付方式系统编号</param>
        /// <returns>验证结果</returns>
        /// <remarks>2013-09-06 周唐炬 创建</remarks>
        public abstract int PaymentTypeVerify(string paymentName,int? sysNo);
    }
}
