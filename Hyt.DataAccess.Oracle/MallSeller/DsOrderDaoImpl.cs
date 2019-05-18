using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.MallSeller;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using System.Data;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Oracle.MallSeller
{
    /// <summary>
    /// 分销商升舱订单数据访问
    /// </summary>
    /// <remarks>2013-09-03 朱家宏 创建</remarks>
    public class DsOrderDaoImpl : IDsOrderDao
    {
        /// <summary>
        /// 根据开始日期获取指定状态的升舱订单
        /// </summary>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="dearerMallSysNo">商城系统编号</param>
        /// <param name="status">订单状态</param>
        /// <returns>订单列表</returns>
        /// <reamrks>2014-04-08 黄波 创建</reamrks>
        public override List<CBDsOrder> GetSuccessedOrder(DateTime startDate, DateTime endDate, int dearerMallSysNo, Hyt.Model.WorkflowStatus.DistributionStatus.升舱订单状态 status)
        {
            string sql = @"select t1.*,t6.DeliveryTypeName,t4.ExpressNo
 from dsorder t1
inner join SoOrder   t2
on t1.ordertransactionsysno=t2.transactionsysno
left join WhStockOut   t3
on t3.ordersysno=t2.sysno  and t3.Status<>-10
left join LgDeliveryItem t4
on t4.notetype=10 and t4.notesysno=t3.sysno and t4.Status<>-10
left join  LgDelivery  t5
on t5.sysno=t4.deliverysysno
left join LgDeliveryType t6
on t6.sysno=t5.deliverytypesysno 
where t1.dealerMallSysNo=@dealerMallSysNo and t1.status=@status and t1.upgradetime>=@startDate and t1.upgradetime<@endDate";
            return Context.Sql(sql)
                    .Parameter("dealerMallSysNo", dearerMallSysNo)
                    .Parameter("status", (int)status)
                    .Parameter("startDate", startDate)
                    .Parameter("endDate", endDate)
                    .QueryMany<CBDsOrder>();
        }


        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>分页</returns>
        /// <remarks>2013-09-03 朱家宏 创建</remarks>
        /// <remarks>2013-10-15 黄志勇 修改</remarks>
        public override Pager<CBDsOrder> Query(ParaDsOrderFilter filter)
        {
            string sql = @"DsOrder a 
                            left join soOrder b on a.OrderTransactionSysNo = b.transactionsysno
                            left join SoReceiveAddress c on b.receiveaddresssysno=c.sysno
                            where {0} ";

            #region 构造sql

            var paras = new ArrayList();
            var where = "1=1 ";
            int i = 0;
            if (filter.DealerMallSysNo != null)
            {
                where += " and a.DealerMallSysNo=@p0p" + i;
                paras.Add(filter.DealerMallSysNo);
                i++;
            }
            if (!string.IsNullOrWhiteSpace(filter.BuyerNick))
            {
                where += " and charindex(a.buyernick,@p0p" + i + ")>0";
                paras.Add(filter.BuyerNick);
                i++;
            }

            if (!string.IsNullOrWhiteSpace(filter.MallOrderId))
            {
                where += " and a.MallOrderID=@p0p" + i;
                paras.Add(filter.MallOrderId);
                i++;
            }
            if (filter.BeginDate.HasValue)
            {
                where += " and a.UpgradeTime>=@p0p" + i;
                paras.Add(filter.BeginDate);
                i++;
            }
            if (filter.EndDate.HasValue)
            {
                where += " and a.UpgradeTime<@p0p" + i;
                paras.Add(filter.EndDate);
                i++;
            }
            if (!string.IsNullOrWhiteSpace(filter.MallProductName))
            {
                //商品名称
                where += @" and exists (
                                select 1 from DsOrderItem tmp where tmp.DsOrderSysNo=a.sysNo and charindex(tmp.MallProductName,:MallProductName)>0
                                union all
                                select 1 from DsOrderItem t1
                                inner join DsOrderItemAssociation t2 on t1.SysNo = t2.DsOrderItemSysNo
                                inner join SoOrderItem t3 on t2.SoOrderItemSysNo = t3.SysNo
                                where t1.DsOrderSysNo=a.sysNo and charindex(t3.ProductName,@p0p" + i + @")>0
                          )";
                paras.Add(filter.MallProductName);
                i++;
            }
            if (!string.IsNullOrWhiteSpace(filter.MallProductId))
            {
                where +=
                    " and exists (select 1 from DsOrderItem tmp where tmp.DsOrderSysNo=a.sysNo and tmp.MallProductId=@p0p" + i + ")";
                paras.Add(filter.MallProductId);
                i++;
            }
            if (filter.HytOrderStatus != null)
            {
                where +=
                    " and a.Status=@p0p" + i;
                paras.Add(filter.HytOrderStatus);
                i++;
            }
            if (filter.OrderSysNo != null)
            {
                where += " and b.Sysno=@p0p" + i;
                paras.Add(filter.OrderSysNo);
                i++;
            }
            if (!string.IsNullOrWhiteSpace(filter.ReceiveName))
            {
                where += " and c.Name like @p0p" + i;
                paras.Add("%" + filter.ReceiveName + "%");
                i++;
            }
            if (!string.IsNullOrWhiteSpace(filter.MobilePhoneNumber))
            {
                where += " and c.MobilePhoneNumber= @p0p" + i;
                paras.Add(filter.MobilePhoneNumber);
                i++;
            }

            sql = string.Format(sql, where);

            #endregion

            var dataList = Context.Select<CBDsOrder>("a.*,b.PayStatus,b.Sysno as OrderSysNo").From(sql);
            var dataCount = Context.Select<int>("count(0)").From(sql);

            dataList.Parameters(paras);
            dataCount.Parameters(paras);

            var pager = new Pager<CBDsOrder>
            {
                PageSize = filter.PageSize,
                CurrentPage = filter.PageIndex,
                TotalRows = dataCount.QuerySingle(),
                Rows = dataList.OrderBy("a.sysNo desc").Paging(filter.PageIndex, filter.PageSize).QueryMany()
            };

            return pager;
        }

        /// <summary>
        /// 查询可退换货升舱订单
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>分页</returns>
        /// <remarks>2013-09-26 朱成果 创建</remarks>
        public override Pager<CBDsOrder> QueryForRma(ParaDsOrderFilter filter)
        {
            const string sql = @"(
                                select    a.*,b.PayStatus 
                                from DsOrder a 
                                inner join soOrder b on a.OrderTransactionSysNo = b.transactionsysno
                                inner join   
                                (      
                                    select distinct  m.ordersysno
                                    from WhStockOutItem m
                                    inner  join WhStockOut p
                                    on p.sysno=m.stockoutsysno
                                    left outer  join
                                    (
                                      select n.stockoutitemsysno,n.rmaquantity
                                      from RcReturn k
                                      inner join
                                       RcReturnItem n
                                      on n.returnsysno=k.sysno
                                      where  k.status<>-10
                                    ) r
                                    on m.sysno=r.stockoutitemsysno
                                    where  p.Status=60 
                                    group by m.ordersysno,m.sysno,m.productquantity
                                    having m.productquantity-isnull(sum(r.rmaquantity),0)>0
                                ) rtb
                                on rtb.ordersysno=b.sysno
                                where DealerMallSysNo=@0 and
                                (@1 is null or charindex(a.buyernick,@1)>0) and 
                                (@2 is null or a.MallOrderID=@2) and 
                                (@3 is null or a.UpgradeTime>=@3) and                                                                                   --日期(起)
                                (@4 is null or a.UpgradeTime<@4) and                                                                                        --日期(止) 
                                (@5 is null or exists (select 1 from DsOrderItem tmp where tmp.DsOrderSysNo=a.sysNo and charindex(tmp.MallProductName,@5)>0
                                union all
                                    select 1 from DsOrderItem t1
                                    inner join DsOrderItemAssociation t2 on t1.SysNo = t2.DsOrderItemSysNo
                                    inner join SoOrderItem t3 on t2.SoOrderItemSysNo = t3.SysNo
                                    where t1.DsOrderSysNo=a.sysNo and charindex(t3.ProductName,@5)>0
)) and       --商品名称
                                (@6 is null or exists (select 1 from DsOrderItem tmp where tmp.DsOrderSysNo=a.sysNo and tmp.MallProductId=@6)) and                         --商品编号
                                (@7 is null or b.Status=@7)  
                                ) tb";

            var paras = new object[]
                {
                    filter.DealerMallSysNo,
                    filter.BuyerNick,
                    filter.MallOrderId,
                    filter.BeginDate,
                    filter.EndDate,
                    filter.MallProductName,
                    filter.MallProductId,
                    filter.HytOrderStatus
                };

            var dataList = Context.Select<CBDsOrder>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(0)").From(sql);

            dataList.Parameters(paras);
            dataCount.Parameters(paras);

            var pager = new Pager<CBDsOrder>
            {
                PageSize = filter.PageSize,
                CurrentPage = filter.PageIndex,
                TotalRows = dataCount.QuerySingle(),
                Rows = dataList.OrderBy("tb.sysNo desc").Paging(filter.PageIndex, filter.PageSize).QueryMany()
            };
            return pager;
        }

        /// <summary>
        /// 获取升舱订单实体
        /// </summary>
        /// <param name="mallOrderId">淘宝订单编号</param>
        /// <returns>实体</returns>
        /// <remarks>2013-09-05 朱家宏 创建</remarks>
        public override DsOrder SelectByMallOrderId(string mallOrderId)
        {
            return Context.Sql("select * from dsOrder where MallOrderID=@0 order by sysNo desc", mallOrderId).QuerySingle<DsOrder>();
        }

        /// <summary>
        /// 根据升舱订单编号获取
        /// </summary>
        /// <param name="sysNo">升舱订单编号</param>
        /// <returns>分销商升舱订单</returns>
        /// <remarks>2013-09-26 朱成果 创建</remarks>
        public override DsOrder SelectBySysNo(int sysNo)
        {
            return Context.Sql("select * from dsOrder where SysNo=@0", sysNo).QuerySingle<DsOrder>();
        }

        /// <summary>
        /// 获取升舱订单明细
        /// </summary>
        /// <param name="dsOrderSysNo">升舱编号</param>
        /// <returns>明细列表</returns>
        /// <remarks>2013-09-05 朱家宏 创建</remarks>
        public override IList<DsOrderItem> SelectItems(int dsOrderSysNo)
        {
            var items =
                Context.Select<DsOrderItem>("*")
                       .From("dsorderitem")
                       .Where("dsordersysno=@dsordersysno")
                       .Parameter("dsordersysno", dsOrderSysNo)
                       .QueryMany();
            return items;
        }

        /// <summary>
        /// 获取升舱订单明细关联
        /// </summary>
        /// <param name="dsOrderItemSysNo">升舱明细编号</param>
        /// <returns>明细关联列表</returns>
        /// <remarks>2013-09-05 余勇 创建</remarks>
        public override IList<DsOrderItemAssociation> SelectItemAssociations(int dsOrderItemSysNo)
        {
            var items =
                Context.Select<DsOrderItemAssociation>("*")
                       .From("DsOrderItemAssociation")
                       .Where("DsOrderItemSysNo=@DsOrderItemSysNo")
                       .Parameter("DsOrderItemSysNo", dsOrderItemSysNo)
                       .QueryMany();
            return items;
        }

        /// <summary>
        /// 根据出库单明细获取关联
        /// </summary>
        /// <param name="outStockItemNo">出库单明细编号</param>
        /// <returns>关联列表</returns>
        /// <remarks>2013-09-05 朱家宏 创建</remarks>
        public override List<DsOrderItemAssociation> GetDsOrderItemAssociationByOutStockItemNo(int outStockItemNo)
        {
            return Context.Sql(@"select t1.* from DsOrderItemAssociation t1 
                                      inner join WhStockOutItem  t2 
                                      on t1.soorderitemsysno=t2.orderitemsysno 
                                      where t2.SysNo=@SysNo")
                                        .Parameter("SysNo", outStockItemNo)
                                        .QueryMany<DsOrderItemAssociation>();

        }

        /// <summary>
        /// 插入分销商升舱订单数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2013-09-06  余勇 创建</remarks>
        public override int InsertOrder(DsOrder entity)
        {
            //entity.SysNo = Context.Insert("DsOrder", entity)
            //                          .AutoMap(o => o.SysNo)
            //                          .ExecuteReturnLastId<int>("SysNo");
            //return entity.SysNo;
            //再sql中判断重复升舱
            string sql = @"
                             declare @v_cnt int                        
                              select @v_cnt=count(1) from  DsOrder where DealerMallSysNo=@A and MallOrderId=@B and Status<>@C
                              if @v_cnt>0 
                              begin
                                  RAISERROR('重复升舱.',16,1,'-20001')
                                  --RAISE_APPLICATION_ERROR(-20001,'重复升舱.');  
                              end 
                              else
                              begin
                              insert into 
                              DsOrder(DealerMallSysNo,MallOrderId,OrderTransactionSysNo,ReturnTransactionSysNo,SellerNick,BuyerNick,Province,City,County,StreetAddress,PostFee,ServiceFee,DiscountAmount,Payment,PayTime,UpgradeTime,DeliveryTime,SignTime,IsCallback,LastCallbackTime,Status) 
                              values(@DealerMallSysNo,@MallOrderId,@OrderTransactionSysNo,@ReturnTransactionSysNo,@SellerNick,@BuyerNick,@Province,@City,@County,@StreetAddress,@PostFee,@ServiceFee,@DiscountAmount,@Payment,@PayTime,@UpgradeTime,@DeliveryTime,@SignTime,@IsCallback,@LastCallbackTime,@Status)
                              set @newId=@@Identity
                              end
                          ";
            try
            {
                var cmd = Context.Sql(sql)
                       .Parameter("A", entity.DealerMallSysNo)
                       .Parameter("B", entity.MallOrderId)
                       .Parameter("C", (int)DistributionStatus.升舱订单状态.失败)
                       .Parameter("DealerMallSysNo", entity.DealerMallSysNo)
                       .Parameter("MallOrderId", entity.MallOrderId)
                       .Parameter("OrderTransactionSysNo", entity.OrderTransactionSysNo)
                       .Parameter("ReturnTransactionSysNo", entity.ReturnTransactionSysNo)
                       .Parameter("SellerNick", entity.SellerNick)
                       .Parameter("BuyerNick", entity.BuyerNick)
                       .Parameter("Province", entity.Province)
                       .Parameter("City", entity.City)
                       .Parameter("County", entity.County)
                       .Parameter("StreetAddress", entity.StreetAddress)
                       .Parameter("PostFee", entity.PostFee)
                       .Parameter("ServiceFee", entity.ServiceFee)
                       .Parameter("DiscountAmount", entity.DiscountAmount)
                       .Parameter("Payment", entity.Payment)
                       .Parameter("PayTime", entity.PayTime)
                       .Parameter("UpgradeTime", entity.UpgradeTime)
                       .Parameter("DeliveryTime", entity.DeliveryTime)
                       .Parameter("SignTime", entity.SignTime)
                       .Parameter("IsCallback", entity.IsCallback)
                       .Parameter("LastCallbackTime", entity.LastCallbackTime)
                       .Parameter("Status", entity.Status)
                       .ParameterOut("newId", Base.DataTypes.Int32);
                cmd.Execute();//执行Sql
                entity.SysNo = cmd.ParameterValue<int>("newId");//接收返回值
            }
            catch (Exception ex)
            {
                if (ex.Message.IndexOf("重复升舱") > -1)
                {
                    throw new HytException("不允许重复升舱.");
                }
                else
                {
                    throw ex;
                }
            }
            return entity.SysNo;
        }

        /// <summary>
        /// 更新分销商升舱订单数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>影响行数</returns>
        /// <remarks>2013-09-06  余勇 创建</remarks>
        public override int UpdateOrder(DsOrder entity)
        {
            return Context.Update("DsOrder", entity)
                .AutoMap(o => o.SysNo)
                .Where("SysNo", entity.SysNo)
                .Execute();

        }

        /// <summary>
        /// 删除分销商升舱订单数据
        /// </summary>
        /// <param name="sysNo">升舱编号</param>
        /// <returns>空</returns>
        /// <remarks>2013-09-06  余勇 创建</remarks>
        public override void DeleteOrder(int sysNo)
        {
            Context.Sql("Delete from DsOrder where SysNo=@SysNo")
                .Parameter("SysNo", sysNo)
           .Execute();
        }

        /// <summary>
        /// 插入分销商升舱订单明细数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2013-09-06  余勇 创建</remarks>
        public override int InsertOrderItem(DsOrderItem entity)
        {
            entity.SysNo = Context.Insert("DsOrderItem", entity)
                                      .AutoMap(o => o.SysNo)
                                      .ExecuteReturnLastId<int>("SysNo");
            return entity.SysNo;
        }

        /// <summary>
        /// 更新分销商升舱订单明细数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>影响行数</returns>
        /// <remarks>2013-09-06  余勇 创建</remarks>
        public override int UpdateOrderItem(DsOrderItem entity)
        {
            return Context.Update("DsOrderItem", entity)
                 .AutoMap(o => o.SysNo)
                 .Where("SysNo", entity.SysNo)
                 .Execute();
        }

        /// <summary>
        /// 删除分销商升舱订单明细数据
        /// </summary>
        /// <param name="dsOrderSysNo">升舱编号</param>
        /// <returns>空</returns>
        /// <remarks>2013-09-06  余勇 创建</remarks>
        public override void DeleteItemByOrderSysNo(int dsOrderSysNo)
        {
            Context.Sql("Delete from DsOrderItem where DsOrderSysNo=@DsOrderSysNo")
                .Parameter("DsOrderSysNo", dsOrderSysNo)
           .Execute();
        }

        /// <summary>
        /// 通过升舱明细编号删除数据
        /// </summary>
        /// <param name="dsOrderSysNo">升舱明细编号</param>
        /// <remarks>2014-08-18  朱成果 创建</remarks>
        public override void DeleteItemByItemSysNo(int dsorderitemsysno)
        {
            Context.Sql("Delete from DsOrderItem where sysno=@sysno")
              .Parameter("sysno", dsorderitemsysno)
             .Execute();
        }


        /// <summary>
        /// 插入升舱订单明细关联数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2013-09-06  余勇 创建</remarks>
        public override int InsertAssociation(DsOrderItemAssociation entity)
        {
            entity.SysNo = Context.Insert("DsOrderItemAssociation", entity)
                                     .AutoMap(o => o.SysNo)
                                     .ExecuteReturnLastId<int>("SysNo");
            return entity.SysNo;
        }

        /// <summary>
        /// 更新升舱订单明细关联数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>影响行数</returns>
        /// <remarks>2013-09-06  余勇 创建</remarks>
        public override int UpdateAssociation(DsOrderItemAssociation entity)
        {
            return Context.Update("DsOrderItemAssociation", entity)
                .AutoMap(o => o.SysNo)
                .Where("SysNo", entity.SysNo)
                .Execute();
        }

        /// <summary>
        /// 删除升舱订单明细关联数据
        /// </summary>
        /// <param name="dsOrderItemSysNo">升舱编号</param>
        /// <returns>空</returns>
        /// <remarks>2013-09-06  余勇 创建</remarks>
        public override void DeleteAssociationByItmeSysNo(int dsOrderItemSysNo)
        {
            Context.Sql("Delete from DsOrderItemAssociation where DsOrderItemSysNo=@DsOrderItemSysNo")
              .Parameter("DsOrderItemSysNo", dsOrderItemSysNo)
         .Execute();
        }

        /// <summary>
        /// 根据商城订单事物编号获取分销商订单信息
        /// </summary>
        /// <param name="orderTransactionSysNo">订单事物编号</param>
        /// <returns>分销商订单信息列表</returns>
        /// <remarks>2013-09-12 朱成果 创建</remarks>
        public override List<DsOrder> GetEntityByTransactionSysNo(string orderTransactionSysNo)
        {
            return
               Context.Sql("select * from DsOrder where OrderTransactionSysNo=@OrderTransactionSysNo")
               .Parameter("OrderTransactionSysNo", orderTransactionSysNo)
               .QueryMany<DsOrder>();
        }

        /// <summary>
        /// 根据商城订单编号获取分销商订单信息
        /// </summary>
        /// <param name="hytOrderID">商城订单编号</param>
        /// <returns>分销商订单信息列表</returns>
        /// <remarks>2013-09-12 朱成果 创建</remarks>
        public override List<DsOrder> GetEntityByHytOrderID(int hytOrderID)
        {
            return Context.Sql(@"select 
                        t1.* 
                        from DsOrder t1 
                        inner join SoOrder t2
                        on t1.ordertransactionsysno=t2.transactionsysno
                        where t2.sysno=@sysno
                                     ")
             .Parameter("sysno", hytOrderID)
             .QueryMany<DsOrder>();
        }

        /// <summary>
        /// 获取商城订单对应的分销商编号
        /// </summary>
        /// <param name="hytOrderID">商城订单编号</param>
        /// <returns>商城订单对应的分销商编号</returns>
        /// <remarks>2013-09-12 朱成果 创建</remarks>
        public override int GetHytOrderDealerSysno(int hytOrderID)
        {
            return Context.Sql(@"select distinct
                                        t3.dealersysno
                                        from DsOrder t1 
                                        inner join SoOrder t2
                                        on t1.ordertransactionsysno=t2.transactionsysno
                                        inner join DsDealerMall t3
                                        on t3.sysno=t1.dealermallsysno
                                        where t2.sysno=@sysno          
                                     ")
           .Parameter("sysno", hytOrderID)
           .QuerySingle<int>();

        }

        /// <summary>
        /// 是否已经成功升舱
        /// </summary>
        /// <param name="dealerMallSysNo">分销商商城系统编号</param>
        /// <param name="mallOrderId">商城订单号</param>
        /// <returns>是否已经成功升舱</returns>
        /// <remarks>2013-9-17 朱成果 创建</remarks>
        public override bool ExistsDsOrder(int dealerMallSysNo, string mallOrderId)
        {

            return Context.Sql(@"select count(1) from DsOrder where DealerMallSysNo=@DealerMallSysNo and MallOrderId=@MallOrderId")
           .Parameter("DealerMallSysNo", dealerMallSysNo)
           .Parameter("MallOrderId", mallOrderId)
           .QuerySingle<int>() > 0;

            //return Context.Sql(@"select count(1) from DsOrder where DealerMallSysNo=@DealerMallSysNo and MallOrderId=@MallOrderId and Status<>@Status")
            // .Parameter("DealerMallSysNo", dealerMallSysNo)
            // .Parameter("MallOrderId", mallOrderId)
            // .Parameter("Status", (int)Hyt.Model.WorkflowStatus.DistributionStatus.升舱订单状态.失败)
            // .QuerySingle<int>() > 0;

        }


        /// <summary>
        /// 根据商城订单明细获取升仓明细
        /// </summary>
        /// <param name="soOrderItemSysNo">事物编号</param>
        /// <returns>获取升仓明细</returns>
        /// <remarks>2014-07-04 朱成果 创建</remarks>
        public override DsOrderItem GetDsOrderItemsByHytItems(int soOrderItemSysNo)
        {
            return Context.Sql(@"select a.* from DsOrderItem a
                                            inner join DsOrderItemAssociation b
                                            on a.sysno=b.dsorderitemsysno where b.SoOrderItemSysNo=@SoOrderItemSysNo")
                    .Parameter("SoOrderItemSysNo", soOrderItemSysNo).QuerySingle<DsOrderItem>();
        }

        #region 登录
        /// <summary>
        /// 根据店铺账号获取分销商商城
        /// </summary>
        /// <param name="shopAccount">店铺账号</param>
        /// <param name="mallTypeSysNo">分销商城类型系统编号</param>
        /// <returns>分销商信息</returns>
        /// <remarks>2013-09-05 黄志勇 创建</remarks>
        public override DsDealerMall GetDsDealerMallByShopAccount(string shopAccount, int mallTypeSysNo)
        {
            return Context.Sql("select * from DsDealerMall where ShopAccount=@shopAccount and MallTypeSysNo=@mallTypeSysNo")
                          .Parameter("shopAccount", shopAccount)
                          .Parameter("mallTypeSysNo", mallTypeSysNo)
                          .QuerySingle<DsDealerMall>();
        }

        /// <summary>
        /// 根据分销商系统编号获取授权账号绑定表
        /// </summary>
        /// <param name="dealerSysNo">分销商系统编号</param>
        ///<returns>授权账号绑定表</returns>
        /// <remarks>2013-09-13 黄志勇 创建</remarks>
        public override List<DsDealerMall> GetDsAuthorizations(int dealerSysNo)
        {
            var sql = @"SELECT * FROM DSDEALERMALL WHERE DEALERSYSNO=@DEALERSYSNO";
            return Context.Sql(sql)
                          .Parameter("DEALERSYSNO", dealerSysNo)
                          .QueryMany<DsDealerMall>();
        }

        /// <summary>
        /// 通过名称得到分销商商城旗舰店
        /// </summary>
        /// <returns>分销商商城旗舰店列表</returns>
        /// <remarks>2014-05-20 余勇 创建</remarks>
        public override List<DsDealerMall> GetAllFlagShip()
        {
            var sql = @"SELECT * FROM DSDEALERMALL where MallTypeSysNo=1 and IsSelfSupport=1 and ShopName like '%旗舰%'";
            return Context.Sql(sql)
                          .QueryMany<DsDealerMall>();
        }

        /// <summary>
        /// 更新分销商商城
        /// </summary>
        /// <param name="model">分销商商城</param>
        ///<returns>受影响行数</returns>
        /// <remarks>2013-09-05 黄志勇 创建</remarks>
        public override int UpdateDsAuthorization(DsDealerMall model)
        {
            return Context.Update("DsDealerMall", model)
                          .AutoMap(x => x.SysNo, x => x.CreatedBy, x => x.CreatedDate)
                          .Where(x => x.SysNo)
                          .Execute();
        }

        #endregion

        #region 账户管理
        /// <summary>
        /// 获取账户信息
        /// </summary>
        /// <param name="shopAccount">店铺账号</param>
        /// <param name="mallTypeSysNo">分销商城类型系统编号</param>
        /// <returns>账户信息</returns>
        /// <remarks>2013-09-06 黄志勇 创建</remarks>
        public override CBAccountInfo GetAccountInfo(string shopAccount, int mallTypeSysNo)
        {
            const string sql = @"select a.ShopAccount,b.MobilePhoneNumber,c.AvailableAmount,c.TotalPrestoreAmount,c.FrozenAmount,c.SysNo PrePaymentSysNo,b.DealerName,c.AlertAmount  
                                from DsDealerMall a 
                                left join DsDealer b on a.DealerSysNo = b.SysNo
                                left join DsPrePayment c on b.SysNo = c.DealerSysNo
                                where a.ShopAccount=@shopAccount and a.MallTypeSysNo=@mallTypeSysNo";
            return Context.Sql(sql)
                          .Parameter("shopAccount", shopAccount)
                          .Parameter("mallTypeSysNo", mallTypeSysNo)
                          .QuerySingle<CBAccountInfo>();
        }

        /// <summary>
        /// 分页查询分销商预存款往来账明细
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>分页</returns>
        /// <remarks>2013-09-06 黄志勇 创建</remarks>
        public override Pager<DsPrePaymentItem> QueryPrePaymentItem(ParaDsPrePaymentItemFilter filter)
        {
            const string sql = @"(select a.* from DsPrePaymentItem a   
                                inner join DsPrePayment b on a.PrePaymentSysNo = b.SysNo 
                                where                               
                                (@DealerSysNo is null or b.DealerSysNo=@DealerSysNo) and 
                                (@BeginDate is null or a.CreatedDate>=@BeginDate) and                                                                                   --日期(起)
                                (@EndDate is null or a.CreatedDate<@EndDate)                                                                                            --日期(止) 
                                ) tb";

            var paras = new object[]
                {
                    filter.DealerSysNo,
                    filter.BeginDate,
                    filter.EndDate
                };

            var dataList = Context.Select<DsPrePaymentItem>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(0)").From(sql);

            dataList.Parameters(paras);
            dataCount.Parameters(paras);

            var pager = new Pager<DsPrePaymentItem>
            {
                PageSize = filter.PageSize,
                CurrentPage = filter.PageIndex,
                TotalRows = dataCount.QuerySingle(),
                Rows = dataList.OrderBy("tb.CreatedDate desc").Paging(filter.PageIndex, filter.PageSize).QueryMany()
            };

            return pager;
        }

        /// <summary>
        /// 根据分销商系统编号获取授权账号绑定表
        /// </summary>
        /// <param name="dealerSysNo">分销商系统编号</param>
        /// <returns>授权账号绑定表</returns>
        /// <remarks>2013-09-13 朱家宏 创建</remarks>
        public override DsDealer GetDealer(int dealerSysNo)
        {
            return Context.Sql("select * from DsDealer where SysNo=@dealerSysNo")
                          .Parameter("dealerSysNo", dealerSysNo)
                          .QuerySingle<DsDealer>();
        }

        /// <summary>
        /// 获取分销商商城
        /// </summary>
        /// <param name="value">分销商商城系统编号</param>
        /// <returns>实体</returns>
        /// <remarks>2013-9-17 朱家宏 创建</remarks>
        public override DsDealerMall SelectDsDealerMall(int value)
        {
            return Context.Sql("select * from DsDealerMall where sysNo=@dealerSysNo")
                          .Parameter("dealerSysNo", value)
                          .QuerySingle<DsDealerMall>();
        }

        /// <summary>
        /// 获取分销商升舱订单
        /// </summary>
        /// <param name="shopAccount">店铺账号</param>
        /// <param name="mallTypeSysNo">分销商城类型系统编号</param>
        /// <param name="top">取前几条</param>
        /// <param name="isFinish">升舱完成</param>
        /// <returns>分销商升舱订单列表</returns>
        /// <remarks>2013-09-10 黄志勇 创建</remarks>
        public override List<DsOrder> GetDsOrderInfo(string shopAccount, int mallTypeSysNo, int top, bool? isFinish)
        {
            var sql = string.Format(@"select * from (select a.* from DsOrder a
                                inner join DsDealerMall b
                                on a.DealerMallSysNo = b.SysNo
                                where b.ShopAccount = @shopAccount and b.MallTypeSysNo = @mallTypeSysNo {0}
                                order by a.UpgradeTime desc) where rownum <= @top", !isFinish.HasValue ? "" : (isFinish.Value ? "and a.Status = 30" : "and a.Status != 30"));
            return Context.Sql(sql)
                          .Parameter("shopAccount", shopAccount)
                          .Parameter("mallTypeSysNo", mallTypeSysNo)
                          .Parameter("top", top)
                          .QueryMany<DsOrder>();
        }

        /// <summary>
        /// 分销商预存款主表
        /// </summary>
        /// <param name="shopAccount">店铺账号</param>
        /// <param name="mallTypeSysNo">分销商城类型系统编号</param>
        /// <returns>分销商预存款主表</returns>
        /// <remarks>2013-09-10 黄志勇 创建</remarks>
        public override DsPrePayment GetPrePayment(string shopAccount, int mallTypeSysNo)
        {
            const string sql = @"select c.* from DsDealerMall a
                                left join DsDealer b
                                on a.DealerSysNo = b.SysNo
                                left join DsPrePayment c
                                on b.SysNo = c.DealerSysNo
                                where a.ShopAccount = @shopAccount and a.MallTypeSysNo = @mallTypeSysNo";
            return Context.Sql(sql)
                          .Parameter("shopAccount", shopAccount)
                          .Parameter("mallTypeSysNo", mallTypeSysNo)
                          .QuerySingle<DsPrePayment>();
        }

        /// <summary>
        /// 设置登录信息
        /// </summary>
        /// <param name="shopAccount">店铺账号</param>
        /// <param name="mallTypeSysNo">分销商城类型系统编号</param>
        /// <returns>登录信息</returns>
        /// <remarks>2013-9-11 黄志勇 创建</remarks>
        public override MallSellerAuthorization SetLoginInfo(string shopAccount, int mallTypeSysNo)
        {
            const string sql = @"select a.DealerSysNo HytDealerSysNo,a.SysNo DealerMallSysNo,a.ShopAccount ShopAccount,a.ShopName,a.IsSelfSupport,
                                c.SysNo PrePaymentSysNo,b.DealerName HytDealerName,c.AvailableAmount HytLeftAmount, c.FrozenAmount HytFrozenAmount,
                                a.MallTypeSysNo MallType,d.IsPreDeposit IsPreDeposit,a.AuthCode MallAuthorizationCode
                                  from DsDealerMall a
                                left join DsDealer b on a.DealerSysNo=b.SysNo
                                left join DsPrePayment c on c.DealerSysNo = b.SysNo
                                left join DsMallType d on a.MallTypeSysNo=d.SysNo
                                where a.ShopAccount = @shopAccount and a.MallTypeSysNo = @mallTypeSysNo
";
            return Context.Sql(sql)
                          .Parameter("shopAccount", shopAccount)
                          .Parameter("mallTypeSysNo", mallTypeSysNo)
                          .QuerySingle<MallSellerAuthorization>();
        }

        /// <summary>
        /// 根据系统用户系统编号获取分销商商城
        /// </summary>
        /// <param name="userId">系统用户系统编号</param>
        /// <returns>分销商商城实体</returns>
        /// <remarks>2013-9-11 黄志勇 创建</remarks>
        public override DsDealerMall GetAuthorizationByUserID(int userId)
        {
            return Context.Sql("SELECT DDM.* FROM DSDEALERMALL DDM LEFT JOIN DSDEALER DD ON DDM.DEALERSYSNO=DD.SYSNO WHERE DD.USERSYSNO=@userId ORDER BY MALLTYPESYSNO")
                          .Parameter("userId", userId)
                          .QuerySingle<DsDealerMall>();
        }

        /// <summary>
        /// 获取商城订单和商城类型
        /// </summary>
        /// <param name="orderTransactionSysNos">OrderTransactionSysNo</param>
        /// <returns>商城订单和商城类型列表</returns>
        /// <remarks>2013-9-11 黄志勇 创建</remarks>
        public override List<CBOrderMallType> GetMallType(string orderTransactionSysNos)
        {
            var sql = string.Format(@"select a.OrderTransactionSysNo,b.MallTypeSysNo
from DsOrder a 
inner join DsDealerMall b 
on a.DealerMallSysNo=b.SysNo
where a.OrderTransactionSysNo in ({0})
", orderTransactionSysNos);
            return Context.Sql(sql).QueryMany<CBOrderMallType>();
        }

        #endregion
    }
}
