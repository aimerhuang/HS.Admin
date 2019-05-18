using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Infrastructure.Caching;
using Hyt.DataAccess;
using Hyt.Model.DouShabaoModel;
using Hyt.DataAccess.Order;

namespace Hyt.BLL.Web
{
    /// <summary>
    /// 网站订单逻辑
    /// </summary>
    /// <remarks>2013-08-15 唐永勤 创建</remarks>
    public class SoOrderBo : BOBase<SoOrderBo>
    {
        /// <summary>
        /// 获取订单项
        /// </summary>
        /// <param name="orderSysNo">订单编号</param>
        /// <returns></returns>
        /// <remarks>2016-3-22 杨浩 创建</remarks>
        public IList<CBSoOrderItem> GetOrderItemListByOrderSysNo(int orderSysNo)
        {
            return Hyt.DataAccess.Web.ISoOrderDao.Instance.GetOrderItemListByOrderSysNo(orderSysNo);
        }
        /// <summary>
        /// 获取我的订单列表
        /// </summary>
        /// <param name="pager">订单分页传输类</param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="unPay">是否查询待支付</param>
        /// <returns></returns>
        /// <remarks>2013-08-15 唐永勤 创建</remarks>
        public void GetMyOrderList(Pager<SoOrder> pager, DateTime? startTime, DateTime? endTime, bool unPay=false)
        {
            Hyt.DataAccess.Web.ISoOrderDao.Instance.GetMyOrderList(pager, startTime, endTime, unPay);
        }

        /// <summary>
        /// 获取订单收货地址
        /// </summary>
        /// <param name="receiveAddressSysno">订单收货地址编号</param>
        /// <returns>订单收货地址</returns>
        /// <remarks>2013-08-16 唐永勤 创建</remarks>
        public Hyt.Model.Transfer.CBCrReceiveAddress GetOrderReceiveAddress(int receiveAddressSysno)
        {
            return Hyt.DataAccess.Web.ISoOrderDao.Instance.GetOrderReceiveAddress(receiveAddressSysno);
        }

        /// <summary>
        /// 获取配送方式类型信息
        /// </summary>
        /// <param name="typeSysno">配送类型编号</param>
        /// <returns>配送方式信息</returns>
        /// <remarks>2013-08-16 唐永勤 创建</remarks>
        public CBLgDeliveryType GetDeliveryType(int typeSysno)
        {
            return Hyt.BLL.Logistics.DeliveryTypeBo.Instance.GetDeliveryType(typeSysno);

            //return CacheManager.Get(CacheKeys.Items.OrderType_, typeSysno.ToString(), delegate
            //{
            //    return Hyt.DataAccess.Logistics.ILgDeliveryTypeDao.Instance.GetLgDeliveryType(typeSysno);
            //});
        }

        /// <summary>
        /// 获取支付方式
        /// </summary>
        /// <param name="paymentSysno">支付方式编号</param>
        /// <returns>支付方式信息</returns>
        /// <remarks>2013-08-16 唐永勤 创建</remarks>
        public CBBsPaymentType GetPaymentType(int paymentSysno)
        {
            IList<CBBsPaymentType> list = CacheManager.Get(CacheKeys.Items.PaymentType, delegate{
                    return Hyt.BLL.Basic.PaymentTypeBo.Instance.GetAll();
            });
            return list.First<CBBsPaymentType>(x => x.SysNo == paymentSysno);
        }

        /// <summary>
        /// 获取订单详细信息
        /// </summary>
        /// <param name="orderSysno">订单编号</param>
        /// <returns>订单实体信息</returns>
        /// <remarks>2013-08-16 唐永勤 创建</remarks>
        public SoOrder GetEntity(int orderSysno)
        {
            return Hyt.DataAccess.Web.ISoOrderDao.Instance.GetEntity(orderSysno); 
        }

        /// <summary>
        /// 获取订单日志
        /// </summary>
        /// <param name="orderSysno">订单编号</param>
        /// <returns>订单日志列表</returns>
        /// <remarks>2013-08-19 唐永勤 创建</remarks>
        public Hyt.Infrastructure.Pager.PagedList<SoTransactionLog> GetOrderLogList(int orderSysno)
        {
            return CacheManager.Get(CacheKeys.Items.OrderLogList_, orderSysno.ToString(), delegate {
                return Hyt.BLL.Order.SoOrderBo.Instance.GetTransactionLogPageData(orderSysno, 0, 100);
            });
        }

        /// <summary>
        /// 是否可以退换货
        /// </summary>
        /// <param name="orderSysno">订单编号</param>
        /// <param name="productSysno">商品系统编号</param>
        /// <returns>可以退还货返回true，不可以退换货返回false</returns>
        /// <remarks>2013-08-23 唐永勤 创建</remarks>
        public bool IsOrderCanReturn(int orderSysno, int productSysno)
        {
            return Hyt.BLL.RMA.RmaBo.Instance.OrderRMARequest(orderSysno, productSysno, (int)Hyt.Model.WorkflowStatus.RmaStatus.RMA类型.售后换货);
        }

        /// <summary>
        /// 获取用户未处理的订单
        /// </summary>
        /// <param name="userSysno">用户编号</param>
        /// <returns>未处理的订单数</returns>
        /// <remarks>2013-08-19 唐永勤 创建</remarks>
        public int GetOrderUntreated(int userSysno)
        {
            return Hyt.DataAccess.Web.ISoOrderDao.Instance.GetOrderUntreated(userSysno);
           
        }

         /// <summary>
        /// 获取用户待评价的商品数
        /// </summary>
        /// <param name="userSysno">用户编号</param>
        /// <returns>待评价的商品数</returns>
        /// <remarks>2013-09-28 唐永勤 创建</remarks>
        public int GetUnValuation(int userSysno)
        {
            
            return CacheManager.Get(CacheKeys.Items.UserUnCommentNumber_, userSysno.ToString(), delegate
            {
                return Hyt.DataAccess.Web.ISoOrderDao.Instance.GetUnValuation(userSysno);
            });
            
        }
        /// <summary>
        /// 获取豆沙包签名需要的参数
        /// </summary>
        /// <param name="sysNo">订单号</param>
        /// <returns>2017-07-07 罗熙 创建</returns>
        public Signparameter GetSignparameter(int sysNo)
        {
            return ISoOrderDao.Instance.GetSignparameter(sysNo);
        }
        /// <summary>
        /// 获取配送方式，身份证，总价，(运费)，下单时间
        /// </summary>
        /// <param name="sysNo">订单号</param>
        /// <returns>2017-07-08 罗熙 创建</returns>
        public DouShabaoOrderParameter Getotherparameter(int sysNo)
        {
            return ISoOrderDao.Instance.Getotherparameter(sysNo);
        }
        /// <summary>
        /// 获取商品列表所需要的参数
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns>2017-07-10 罗熙 创建</returns>
        public ProductList GetProductlist(int sysNo)
        {
            return ISoOrderDao.Instance.GetProductlist(sysNo);
        }
    }
}
