using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hyt.Model;
using Hyt.DataAccess.Base;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;

namespace Hyt.DataAccess.Web
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>2013-08-06 苟治国 创建</remarks>
    public abstract class IComplaintDao : DaoBase<IComplaintDao>
    {
        /// <summary>
        /// 获取指定会员订单列表
        /// </summary>
        /// <param name="pager">订单查询条件</param>
        /// <returns>会员订单列表</returns>
        /// <remarks>2013-08-06 苟治国 创建</remarks>
        public abstract Pager<SoOrder> GetOrder(Pager<SoOrder> pager, ParaOrderFilter orderFilter);

        /// <summary>
        /// 根据条件获取会员投诉的列表
        /// </summary>
        /// <param name="pager">会员投诉查询条件</param>
        /// <returns>会员投诉列表</returns>
        /// <remarks>2013-08-06 苟治国 创建</remarks>
        public abstract Pager<Model.CBCrComplaint> GetComplaintList(Pager<CBCrComplaint> pager);

        /// <summary>
        /// 根据订单号获取产品图片
        /// </summary>
        /// <param name="ordersysNo">订单编号</param>
        /// <returns>订单所属图片集</returns>
        /// <remarks>2013-08-06 苟治国 创建</remarks>
        public abstract IList<Model.PdProduct> GetProductImage(int ordersysNo);

        /// <summary>
        /// 插入会员投诉
        /// </summary>
        /// <param name="model"></param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－11-19 苟治国 创建</remarks>
        public abstract int Insert(Model.CrComplaint model);

        /// <summary>
        /// 更新会员投诉
        /// </summary>
        /// <param name="model"></param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－11-19 苟治国 创建</remarks>
        public abstract int Update(Model.CrComplaint model);
    }
}
