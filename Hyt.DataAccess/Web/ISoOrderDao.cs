using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hyt.Model;
using Hyt.DataAccess.Base;
using Hyt.Model.Transfer;

namespace Hyt.DataAccess.Web
{
    /// <summary>
    /// 网站订单接口
    /// </summary>
    /// <remarks>2013-08-15 唐永勤 创建</remarks>
    public abstract class ISoOrderDao : DaoBase<ISoOrderDao>
    {
        /// <summary>
        /// 获取订单项
        /// </summary>
        /// <param name="orderSysNo">订单编号</param>
        /// <returns></returns>
        /// <remarks>2016-3-22 杨浩 创建</remarks>
        public abstract IList<CBSoOrderItem> GetOrderItemListByOrderSysNo(int orderSysNo);
        /// <summary>
        /// 获取我的订单列表
        /// </summary>
        /// <param name="pager">订单分页传输类</param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="unPay">是否查询待支付</param>
        /// <remarks>2013-08-15 唐永勤 创建</remarks>
        public abstract void GetMyOrderList(Pager<SoOrder> pager, DateTime? startTime, DateTime? endTime, bool unPay=false);

        /// <summary>
        /// 获取订单收货地址
        /// </summary>
        /// <param name="receiveAddressSysNo">订单收货地址编号</param>
        /// <returns>订单收货地址</returns>
        /// <remarks>2013-08-16 唐永勤 创建</remarks>
        public abstract Hyt.Model.Transfer.CBCrReceiveAddress GetOrderReceiveAddress(int receiveAddressSysNo);

        /// <summary>
        /// 获取订单详细信息
        /// </summary>
        /// <param name="orderSysNo">订单编号</param>
        /// <returns>订单实体信息</returns>
        /// <remarks>2013-08-16 唐永勤 创建</remarks>
        public abstract SoOrder GetEntity(int orderSysNo);

        /// <summary>
        /// 判断订单编号是否有效
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <param name="context">数据库操作上下文</param>
        /// <returns>返回 true:有效 false:无效</returns>
        /// <remarks>2013-08-27 邵斌 创建</remarks>
        public abstract bool Exist(int orderId,IDbContext context = null);

        /// <summary>
        /// 获取用户未处理的订单
        /// </summary>
        /// <param name="userSysNo">用户编号</param>
        /// <returns>未处理的订单数</returns>
        /// <remarks>2013-08-19 唐永勤 创建</remarks>
        public abstract int GetOrderUntreated(int userSysNo);

        /// <summary>
        /// 获取用户待评价的商品数
        /// </summary>
        /// <param name="userSysNo">用户编号</param>
        /// <returns>待评价的商品数</returns>
        /// <remarks>2013-09-28 唐永勤 创建</remarks>
        public abstract int GetUnValuation(int userSysNo);
    }
}
