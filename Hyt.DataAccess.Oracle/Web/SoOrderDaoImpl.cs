using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.DataAccess.Web;
using Hyt.Model.WorkflowStatus;
using Hyt.Model.Transfer;

namespace Hyt.DataAccess.Oracle.Web
{
    /// <summary>
    /// 网站订单接口实现
    /// </summary>
    /// <remarks>2013-08-15 唐永勤 创建</remarks>
    public class SoOrderDaoImpl : ISoOrderDao
    {
        /// <summary>
        /// 获取订单项
        /// </summary>
        /// <param name="orderSysNo">订单编号</param>
        /// <returns></returns>
        /// <remarks>2016-3-22 杨浩 创建</remarks>
        public override IList<CBSoOrderItem> GetOrderItemListByOrderSysNo(int orderSysNo)
        {
            #region sql query
            string sql = @"SELECT o.*,p.ErpCode,p.Barcode FROM [SoOrderItem] as o inner join [PdProduct] as p on p.SysNo=o.ProductSysNo WHERE o.OrderSysNo={0}";
            sql = string.Format(sql, orderSysNo);
            #endregion

            return Context.Sql(sql).QueryMany<CBSoOrderItem>();
        }
        /// <summary>
        /// 获取我的订单列表
        /// </summary>
        /// <param name="pager">订单分页传输类</param> 
        /// <param name="startTime">时间起</param>
        /// <param name="endTime">时间止</param>
        /// <param name="unPay">是否查询待支付</param>
        /// <returns></returns>
        /// <remarks>2013-08-15 唐永勤 创建</remarks>
        /// <remarks>2013-10-30 杨浩 修改</remarks>
        public override void GetMyOrderList(Pager<SoOrder> pager, DateTime? startTime, DateTime? endTime, bool unPay=false)
        {
            #region 条件、参数

            string condition = string.Format(@" so.CustomerSysno = @0 and (@Status=0 or so.Status =@1) 
                                 and (@2 is null or so.CreateDate >= @2) 
                                 and (@3 is null or so.CreateDate <= @3)
                                 and (1!={0} or (PayStatus = {1}  and  IsOnlinePay={2}  and (so.Status={3} or so.Status={4})))--杨浩 新增 待支付条件判断",
                                             Convert.ToInt32(unPay), (int)OrderStatus.销售单支付状态.未支付, (int)BasicStatus.支付方式是否网上支付.是,
                                             (int)OrderStatus.销售单状态.待审核, (int)OrderStatus.销售单状态.待支付);

            const string tempTable = " (soorder so inner join BsPaymentType py on so.PayTypeSysNo=py.sysno) ";

            var parameters = new object[]
                {
                    pager.PageFilter.CustomerSysNo,
                    pager.PageFilter.Status,
                    startTime,
                    endTime
                };

            #endregion

            using (var context = Context.UseSharedConnection(true))
            {
                pager.Rows = context.Select<SoOrder>("so.*")
                                    .From(tempTable)
                                    .Where(condition)
                                    .Parameters(parameters)
                                    .OrderBy("so.sysno desc ")
                                    .Paging(pager.CurrentPage, pager.PageSize)
                                    .QueryMany();

                pager.TotalRows = context.Select<int>("count(1)")
                                         .From(tempTable)
                                         .Where(condition)
                                         .Parameters(parameters)
                                         .QuerySingle();
                int i = 0;
                foreach (var item in pager.Rows)
                {
                    IList<SoOrderItem> items = context.Select<SoOrderItem>("*")
                                                       .From("SoOrderItem")
                                                       .Where("OrderSysno = @OrderSysno")
                                                       .Parameter("OrderSysno", item.SysNo)
                                                       .OrderBy("sysno desc")
                                                       .QueryMany();
                     pager.Rows[i].OrderItemList = items;
                     i++;
                }
            }
        }

        /// <summary>
        /// 获取订单收货地址
        /// </summary>
        /// <param name="receiveAddressSysNo">订单收货地址编号</param>
        /// <returns>订单收货地址</returns>
        /// <remarks>2013-08-16 唐永勤 创建</remarks>
        public override Hyt.Model.Transfer.CBCrReceiveAddress GetOrderReceiveAddress(int receiveAddressSysNo)
        {
            #region sql query
            string sql = @"select r.*,a.areaname as Region, a.sysno as RegionSysno, c.areaname as City, c.sysno as CitySysno, p.areaname as Province, p.sysno as ProvinceSysno
                        from (select * from SoReceiveAddress where sysno = {0}) r 
                            left join bsarea a on r.AreaSysno = a.Sysno
                            left join bsarea c on a.ParentSysno = c.Sysno
                            left join bsarea p on c.ParentSysno = p.Sysno
                            ";
            sql = string.Format(sql, receiveAddressSysNo);
            #endregion

            return Context.Sql(sql).QuerySingle<Hyt.Model.Transfer.CBCrReceiveAddress>();
        }

        /// <summary>
        /// 获取订单详细信息
        /// </summary>
        /// <param name="orderSysNo">订单编号</param>
        /// <returns>订单实体信息</returns>
        /// <remarks>2013-08-16 唐永勤 创建</remarks>
        /// <remarks>2013-08-29 邵  斌 优化 加入无效订单判断</remarks>
        public override SoOrder GetEntity(int orderSysNo)
        {
            SoOrder entity = new SoOrder();
            using (var context = Context.UseSharedConnection(true))
            {
                entity = context.Select<SoOrder>("so.*")
                                .From("SoOrder so")
                                .Where("sysno=@sysno")
                                .Parameter("sysno", orderSysNo)
                                .QuerySingle();

                //订单是否有效
                if (entity == null)
                {
                    return entity;
                }

                //获取订单商品详单
                entity.OrderItemList = context.Select<SoOrderItem>("*")
                                              .From("SoOrderItem")
                                              .Where("OrderSysno = @OrderSysno")
                                              .Parameter("OrderSysno", entity.SysNo)
                                              .OrderBy("sysno desc")
                                              .QueryMany();

            }
            return entity;
        }

        /// <summary>
        /// 判断订单编号是否有效
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <param name="context">数据库操作上下文</param>
        /// <returns>返回 true:有效 false:无效</returns>
        /// <remarks>2013-08-27 邵斌 创建</remarks>
        public override bool Exist(int orderId, IDbContext context = null)
        {
            context = context ?? Context;

            //判断订单编号是否存在
            return context.Sql("select count(sysno) from SoOrder where sysno=@0", orderId).QuerySingle<int>() > 0;
        }

        /// <summary>
        /// 获取用户未处理的订单
        /// </summary>
        /// <param name="userSysNo">用户编号</param>
        /// <returns>未处理的订单数</returns>
        /// <remarks>2013-08-19 唐永勤 创建</remarks>
        public override int GetOrderUntreated(int userSysNo)
        {
              return Context.Select<int>("count(1)")
                            .From("SoOrder")
                            .Where("CustomerSysno=@CustomerSysno and Status <> @Status and Paystatus=@PayStatus")
                            .Parameter("CustomerSysno", userSysNo)
                            .Parameter("Status", (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单状态.作废)
                            .Parameter("PayStatus", (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单支付状态.未支付)
                            .QuerySingle();
        }

        /// <summary>
        /// 获取用户待评价的商品数
        /// </summary>
        /// <param name="userSysNo">用户编号</param>
        /// <returns>待评价的商品数</returns>
        /// <remarks>2013-09-28 唐永勤 创建</remarks>
        public override int GetUnValuation(int userSysNo)
        {
            #region sql query

            //已完成的商品数sql查询
            string sqlOrderComplete = @"select count(1) 
                                        from( (select * from SoOrder where CustomerSysNo = @CustomerSysNo and Status = @Status) o
                                              left join SoOrderItem item
                                              on o.Sysno = item.OrderSysno)";
            //已评价的商品数
            string sqlProductHaveComment =
                @"select count(*) from (select OrderSysNo,ProductSysNo from FeProductComment where CustomerSysNo=@CustomerSysNo and IsComment=@IsComment and (CommentStatus=@CommentStatus1 or  CommentStatus=@CommentStatus2) group by OrderSysNo,ProductSysNo)";

            #endregion

            int totalNumber, commentNumber;
            using (var context = Context.UseSharedConnection(true))
            {
                totalNumber = context.Sql(sqlOrderComplete)
                                     .Parameter("CustomerSysNo", userSysNo)
                                     .Parameter("Status", (int) OrderStatus.销售单状态.已完成)
                                     .QuerySingle<int>();
                commentNumber = context.Sql(sqlProductHaveComment)
                                       .Parameter("CustomerSysNo", userSysNo)
                                       .Parameter("IsComment", (int) ForeStatus.是否评论.是)
                                       .Parameter("CommentStatus1", (int) ForeStatus.商品评论状态.待审)
                                       .Parameter("CommentStatus2", (int) ForeStatus.商品评论状态.已审)
                                       .QuerySingle<int>();
            }
            int unValuationNumber = totalNumber - commentNumber;
            return unValuationNumber >= 0 ? unValuationNumber : 0;
        }

    }
}