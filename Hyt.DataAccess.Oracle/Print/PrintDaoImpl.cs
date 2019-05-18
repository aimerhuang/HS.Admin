using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Print;
using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Model.Generated;

namespace Hyt.DataAccess.Oracle.Print
{
    /// <summary>
    /// 打印模块公共数据访问类
    /// </summary>
    /// <remarks>
    /// 2013-07-16 郑荣华 创建
    /// </remarks>
    public class PrintDaoImpl : IPrintDao
    {
        /// <summary>
        /// 获取配货(拣货，出库)单打印实体，只显示有效的出库单明细中 
        /// </summary>
        /// <param name="outStockSysNo">出库单系统编号</param>
        /// <returns>配货(拣货，出库)单打印实体</returns>
        /// <remarks>
        /// 2013-07-16 郑荣华 创建
        /// </remarks>
        public override CBPrtPicking GetPrintPicking(int outStockSysNo)
        {
            using (var context = Context.UseSharedConnection(true))
            {
                //主表            
                const string sql = @"select t.*,b.name ReceiveName,b.phonenumber,b.mobilephonenumber,b.areasysno,
                                 b.streetaddress,e.paymentname,c.remarks soremarks,d.name,f.warehousename,f.backwarehousename,
                                 c.createdate socreateddate,'' ShopName,g.DeliveryTypeName,c.FreightAmount
                                 from whstockout t
                                 left join soreceiveaddress b on t.receiveaddresssysno=b.sysno
                                 left join soorder c on t.ordersysno=c.sysno
                                 left join CrCustomer d on c.customersysno=d.sysno
                                 left join BsPaymentType e on c.paytypesysno=e.sysno
                                 left join whWarehouse f on t.warehousesysno=f.sysno
                                 left join lgdeliverytype g on t.deliverytypesysno=g.sysno
                                 where t.sysno=@0";
                var model = context.Sql(sql, outStockSysNo)
                                   .QuerySingle<CBPrtPicking>();
                if (null != model)
                {
                    const string subsql = @"select t.*,b.erpcode,b.Barcode,p.warehousepositionname from whstockoutitem t
                                    left join pdproduct b on t.productsysno=b.sysno
                                    left join whwarehouseposition p on t.warehousepositionsysno = p.sysno 
                                    where t.stockoutSysNo=@0 and t.status=@1";
                    model.List = context.Sql(subsql, outStockSysNo, (int)Hyt.Model.WorkflowStatus.WarehouseStatus.出库单明细状态.有效)
                                        .QueryMany<PrtSubPicking>();
                }
                return model;
            }

        }

        /// <summary>
        /// 批量打印捡货单实体
        /// </summary>
        /// <param name="outStockSysNos">出库单编号</param>
        /// <returns></returns>
        /// <remarks>
        /// 2014-07-11 朱成果 创建
        /// </remarks>
        public override List<CBPrtPicking> GetPrintPickingList(List<int> outStockSysNos)
        {
            string str = string.Empty;
            foreach (var i in outStockSysNos)
            {
                if(!string.IsNullOrEmpty(str))
                {
                    str += ",";
                }
                str += i;
            }
            str="("+str+")";
            using (var context = Context.UseSharedConnection(true))
            {
                //主表            
                const string sql = @"select t.*,b.name ReceiveName,b.phonenumber,b.mobilephonenumber,b.areasysno,
                                 b.streetaddress,e.paymentname,c.remarks soremarks,d.name,f.warehousename,
                                 c.createdate socreateddate,'' ShopName,g.DeliveryTypeName,c.FreightAmount
                                 from whstockout t
                                 left join soreceiveaddress b on t.receiveaddresssysno=b.sysno
                                 left join soorder c on t.ordersysno=c.sysno
                                 left join CrCustomer d on c.customersysno=d.sysno
                                 left join BsPaymentType e on c.paytypesysno=e.sysno
                                 left join whWarehouse f on t.warehousesysno=f.sysno
                                 left join lgdeliverytype g on t.deliverytypesysno=g.sysno
                                 where t.sysno in {0}";
                var lst = context.Sql(string.Format(sql,str)).QueryMany<CBPrtPicking>();
                if(lst!=null&&lst.Count>0)
                {

                    const string subsql = @"select t.*,b.erpcode from whstockoutitem t
                                            left join pdproduct b on t.productsysno=b.sysno
                                            where t.stockoutSysNo in {0} and t.status={1}";
                    var sublist = context.Sql(string.Format(subsql,str,(int)Hyt.Model.WorkflowStatus.WarehouseStatus.出库单明细状态.有效))
                                        .QueryMany<PrtSubPicking>();
                    lst.ForEach((item) => {

                        item.List = sublist.Where(m => m.StockOutSysNo == item.SysNo).ToList();
                    });
                }
                return lst;
            }
        }

        /// <summary>
        /// 获取分销配货(拣货，出库)单打印实体
        /// </summary>
        /// <param name="outStockSysNo">出库单系统编号</param>
        /// <returns>分销配货(拣货，出库)单打印实体</returns>
        /// <remarks>
        /// 2013-09-13 郑荣华 创建
        /// </remarks>
        public override PrtDsPicking GetPrintDsPicking(int outStockSysNo)
        {
            //主表            
            const string sql = @"select t.*,a.warehousename,a.backwarehousename,b.TransactionSysNo OrderTransactionSysNo,b.remarks SoRemarks,
                                 b.createdate SoCreatedDate,b.customersysno,c.name ReceiveName,c.phonenumber,
                                 c.mobilephonenumber,c.streetaddress,c.areasysno,d.paymentname,e.deliverytypename,           
                                 --f.MallOrderId,
                                 --g.shopname,g.shopaccount,
                                 b.FreightAmount
                                from WhStockOut t
                                left join whwarehouse a on t.warehousesysno=a.sysno
                                left join SoOrder b on t.ordersysno=b.sysno
                                left join Soreceiveaddress c on b.receiveaddresssysno=c.sysno
                                left join BsPaymentType d on b.paytypesysno=d.sysno                              
                                left join lgdeliverytype e on t.deliverytypesysno=e.sysno
                                --left join DsOrder f on b.transactionsysno=f.ordertransactionsysno
                                --left join DsDealerMall g on f.DealerMallSysNo=g.sysno
                                where t.sysno=@0";
            var model = Context.Sql(sql, outStockSysNo)
                               .QuerySingle<PrtDsPicking>();
            if (null != model)
            {
                //商品明细列表
                const string subsql = @"select t.*,b.erpcode,b.Barcode,p.warehousepositionname from whstockoutitem t
                                    left join pdproduct b on t.productsysno=b.sysno
                                    left join whwarehouseposition p on t.warehousepositionsysno = p.sysno 
                                    where t.stockoutSysNo=@0 and t.status=@1";
                model.List = Context.Sql(subsql, outStockSysNo, (int)Hyt.Model.WorkflowStatus.WarehouseStatus.出库单明细状态.有效)
                                    .QueryMany<PrtDsSubPicking>();

                //商品明细列表
//                const string subsql = @"select t.*,b.erpcode from whstockoutitem t
//                                    left join pdproduct b on t.productsysno=b.sysno
//                                    where t.stockoutSysNo=@0";
//                model.List = Context.Sql(subsql, outStockSysNo)
//                                    .QueryMany<PrtDsSubPicking>();
                //分销订单明细列表
                const string dssql = @"select t.*,a.shopname,a.shopaccount,a.IsSelfSupport from DsOrder t   
                                       left join DsDealerMall a on t.dealermallsysno=a.sysno  
                                       where t.ordertransactionsysno=@0";
                model.ListDs = Context.Sql(dssql, model.OrderTransactionSysNo)
                                    .QueryMany<CBDsOrder>();
            }
            return model;
        }

        /// <summary>
        /// 获取升舱出库单打印信息
        /// </summary>
        /// <param name="outStockSysNos">出库单列表</param>
        /// <returns></returns>
        /// <remarks>
        /// 2014-07-11 朱成果 创建
        /// </remarks>
        public override List<PrtDsPicking> GetPrintDsPickingList(List<int> outStockSysNos)
        {
            string str = string.Empty;
            foreach (var i in outStockSysNos)
            {
                if (!string.IsNullOrEmpty(str))
                {
                    str += ",";
                }
                str += i;
            }
            str = "(" + str + ")";
            using (var context = Context.UseSharedConnection(true))
            {
                //主表            
                const string sql = @"select t.*,a.warehousename,b.TransactionSysNo OrderTransactionSysNo,b.remarks SoRemarks,
                                b.createdate SoCreatedDate,b.customersysno,c.name ReceiveName,c.phonenumber,
                                c.mobilephonenumber,c.streetaddress,c.areasysno,d.paymentname,e.deliverytypename,           
                                f.MallOrderId,
                                g.shopname,
                                g.shopaccount,
                                g.IsSelfSupport,
                                b.FreightAmount
                                from WhStockOut t
                                left join whwarehouse a on t.warehousesysno=a.sysno
                                left join SoOrder b on t.ordersysno=b.sysno
                                left join Soreceiveaddress c on b.receiveaddresssysno=c.sysno
                                left join BsPaymentType d on b.paytypesysno=d.sysno                              
                                left join lgdeliverytype e on t.deliverytypesysno=e.sysno
                                inner join DsOrder f on b.transactionsysno=f.ordertransactionsysno
                                inner join DsDealerMall g on f.DealerMallSysNo=g.sysno
                                where t.sysno in {0}";
                var lst = Context.Sql(string.Format(sql, str)).QueryMany<PrtDsPicking>();
                if(lst!=null&&lst.Count>0)
                {
                    //商品明细列表
                    const string subsql = @"select t.*,b.erpcode,b.Barcode from whstockoutitem t
                                            left join pdproduct b on t.productsysno=b.sysno
                                            where t.stockoutSysNo in {0}";
                    var sublst = Context.Sql(string.Format(subsql,str)).QueryMany<PrtDsSubPicking>();
                    lst.ForEach((item) => {
                        item.List = sublst.Where(m => m.StockOutSysNo == item.SysNo).ToList();
                    });
                }
                return lst;                   
            }
        }

        /// <summary>
        /// 获取配送单打印实体
        /// </summary>
        /// <param name="sysNo">配送单号</param>
        /// <returns>配送单打印实体</returns>
        /// <remarks>
        /// 2013-07-16 郑荣华 创建
        /// </remarks>
        public override PrtDelivery GetPrintDelivery(int sysNo)
        {
            //主表            
            const string sql = "select * from lgdelivery where sysno=@0";
            var model = Context.Sql(sql, sysNo)
                               .QuerySingle<PrtDelivery>();
            if (null != model)
            {
                //明细
                const string subsql =
                    @"select t.*,b.name,b.streetaddress,b.phonenumber,b.mobilephonenumber,b.areasysno,c.ordersysno                                   
                                    from lgdeliveryitem t 
                                    left join soreceiveaddress b on t.addresssysno=b.sysno
                                    left join whstockout c on t.notesysno=c.sysno
                                    where t.deliverySysNo = @0";
                model.List = Context.Sql(subsql, sysNo)
                                    .QueryMany<PrtSubDelivery>();
            }
            return model;

        }

        /// <summary>
        /// 获取入库单打印实体
        /// </summary>
        /// <param name="sysNo">入库单号</param>
        /// <returns>入库单打印实体</returns>
        /// <remarks>
        /// 2013-07-16 郑荣华 创建
        /// </remarks>
        public override PrtInstock GetPrintInstock(int sysNo)
        {
            //主表            
            const string sql = @"select t.*,b.warehousename from whstockin t 
                                 left join whwarehouse b on t.warehousesysno=b.sysno
                                 where t.sysno=@0";
            //明细
            const string subsql = @"select t.*,b.erpcode,b.Barcode,r.OriginPrice,r.RefundProductAmount,r.RmaQuantity from whstockinitem t 
                                    left join pdproduct b on t.productsysno=b.sysno
                                    left join RcReturnItem r on t.SourceItemSysNo=r.SysNo
                                    where stockinsysno=@0";
            PrtInstock model = null;
            using (var context = Context.UseSharedConnection(true))
            {
                model = context.Sql(sql, sysNo)
                               .QuerySingle<PrtInstock>();
                if (null != model)
                {

                    model.List = context.Sql(subsql, sysNo)
                                        .QueryMany<PrtSubInstock>();
                    //保存订单系统编号
                    int orderSysNo = 0;
                    if (model.SourceType == (int)Model.WorkflowStatus.WarehouseStatus.入库单据类型.RMA单)
                    {
                        var rma = DataAccess.RMA.IRcReturnDao.Instance.GetEntity(model.SourceSysNO);
                        orderSysNo = rma.OrderSysNo;
                    }
                    if (model.SourceType == (int)Model.WorkflowStatus.WarehouseStatus.入库单据类型.出库单)
                    {
                        var stockOut = DataAccess.Warehouse.IOutStockDao.Instance.GetEntity(model.SourceSysNO);
                        orderSysNo = stockOut.OrderSysNO;
                    }
                    var order = Order.SoOrderDaoImpl.Instance.GetEntity(orderSysNo);
                    if (order != null)
                    {
                        var dsOrders =
                        DataAccess.MallSeller.IDsOrderDao.Instance.GetEntityByTransactionSysNo(
                            order.TransactionSysNo);
                        model.SoOrderSysNo = orderSysNo.ToString();
                        if (dsOrders != null)
                        {
                            var malls = new List<string>();
                            foreach (var dsOrder in dsOrders)
                            {
                                if (!malls.Contains(dsOrder.MallOrderId))
                                {
                                    malls.Add(dsOrder.MallOrderId);
                                }
                            }
                            model.TaoBaoSysNo = string.Join(",", malls);
                        }
                    }
                }
            }
            
            return model;
        }

        /// <summary>
        /// 获取借货单打印实体
        /// </summary>
        /// <param name="sysNo">借货单号</param>
        /// <returns>借货单打印实体</returns>
        /// <remarks>
        /// 2013-07-16 郑荣华 创建
        /// </remarks>
        public override PrtLend GetPrintLend(int sysNo)
        {
            //主表            
            const string sql = @"select t.*,b.warehousename from WhProductLend t 
                                 left join whwarehouse b on t.warehousesysno=b.sysno
                                 where t.sysno=@0";
            var model = Context.Sql(sql, sysNo)
                               .QuerySingle<PrtLend>();
            //明细 
            if (null != model)
            {
                const string subsql = @"select t.*,b.productshorttitle,b.erpcode
                                    from WhProductLendItem t
                                    left join pdproduct b on t.productsysno=b.sysno
                                    where t.ProductLendSysNo=@0";
                model.List = Context.Sql(subsql, sysNo)
                                    .QueryMany<PrtSubLend>();
            }
            return model;
        }

        /// <summary>
        /// 获取结算单打印实体
        /// </summary>
        /// <param name="sysNo">结算单号</param>
        /// <returns>结算单打印实体</returns>
        /// <remarks>
        /// 2013-07-16 郑荣华 创建
        /// </remarks>
        public override PrtSettlement GetPrintSettlement(int sysNo)
        {
            //主表            
            const string sql = "select * from lgsettlement where sysno=@0";
            var model = Context.Sql(sql, sysNo)
                               .QuerySingle<PrtSettlement>();
            if (null != model)
            {
                //明细
                const string subsql =
                    @"select t.*,b.receivable,c.name,c.areasysno,c.streetaddress,c.phonenumber,c.mobilephonenumber 
                                    from lgsettlementitem t
                                    left join whstockout b on t.stockoutsysno=b.sysno
                                    left join soreceiveaddress c on b.receiveaddresssysno=c.sysno
                                    where settlementsysno=@0";
                model.List = Context.Sql(subsql, sysNo)
                                    .QueryMany<PrtSubSettlement>();
            }
            return model;
        }

        /// <summary>
        /// 获取第三方快递包裹单打印实体
        /// </summary>
        /// <param name="outStockSysNo">出库单系统编号</param>
        /// <returns>快递包裹单打印实体</returns>
        /// <remarks>
        /// 2013-07-12 郑荣华 创建
        /// </remarks>
        public override PrtPack GetPrintPack(int outStockSysNo)
        {
            const string sql = @"select t.sysno,b.contact fromName,b.areasysno fromCity,b.streetaddress fromAddress,b.phone fromTel,b.WarehouseName,b.StreetAddress,
                                c.name toName,c.phonenumber toTel,c.mobilephonenumber toMobile,c.areasysno toCity,
                                c.streetaddress toAddress,t.orderSysno as OrderSysNo
                                from whstockout t left join whwarehouse b on t.warehousesysno=b.sysno
                                left join soreceiveaddress c on t.receiveaddresssysno=c.sysno 
                                where t.sysno=@0
                                ";
            return Context.Sql(sql, outStockSysNo)
                          .QuerySingle<PrtPack>();
        }
       
        /// <summary>
        /// 获取包裹单数据
        /// </summary>
        /// <param name="outStockSysNos">出库单编号列表</param>
        /// <returns></returns>
        /// <remarks>
        /// 2014-07-14 朱成果 创建
        /// </remarks>
        public override List<CBPrtPack> GetPrintPackList(List<int> outStockSysNos)
        {
            string str = string.Empty;
            foreach (var i in outStockSysNos)
            {
                if (!string.IsNullOrEmpty(str))
                {
                    str += ",";
                }
                str += i;
            }
            str = "(" + str + ")";
            const string sql = @"select t.sysno,b.contact fromName,b.areasysno fromCity,b.streetaddress fromAddress,b.phone fromTel,
                                c.name toName,c.phonenumber toTel,c.mobilephonenumber toMobile,c.areasysno toCity,
                                c.streetaddress toAddress,t.orderSysno as OrderSysNo,f.MallOrderId as DsOrderSysNo, g.ShopName,g.ServicePhone
                                from whstockout t 
                                left join whwarehouse b on t.warehousesysno=b.sysno
                                left join soreceiveaddress c on t.receiveaddresssysno=c.sysno 
                                left join DsOrder f on t.transactionsysno=f.ordertransactionsysno
                                left join DsDealerMall g on f.DealerMallSysNo=g.sysno
                                where t.sysno in {0}
                                ";
            return Context.Sql(string.Format(sql, str)).QueryMany<CBPrtPack>();
        }





        /// <summary>
        /// 获取分销配送结算单打印实体
        /// </summary>
        /// <param name="sysNo">配送单号</param>
        /// <returns>分销配送结算单打印实体</returns>
        /// <remarks>
        /// 2013-09-13 郑荣华 创建
        /// </remarks>
        public override PrtDsDelivery GetPrintDsDelivery(int sysNo)
        {
            using (var context = Context.UseSharedConnection(true))
            {
                //主表            
                const string sql = @"select t.*,a.warehousename,b.parentsysno from lgdelivery t 
                                    left join whwarehouse a on t.stocksysno=a.sysno 
                                    left join lgdeliverytype b on t.DeliveryTypeSysNo=b.sysno
                                    where t.sysno=@0";
                var model = context.Sql(sql, sysNo)
                                   .QuerySingle<PrtDsDelivery>();
                if (null != model)
                {
                    //明细
                    string subsql =
                        @"select t.*,b.name,b.streetaddress,b.phonenumber,b.mobilephonenumber,b.areasysno,dbo.func_getaereapath(b.areasysno) AreaAllName,
                          c.ordersysno,e.paymentname,f.deliverytypename,c.Remarks as stockOutRemarks,d.FreightAmount                                    
                                    from lgdeliveryitem t 
                                    left join soreceiveaddress b on t.addresssysno=b.sysno
                                    left join whstockout c on t.notesysno=c.sysno
                                    left join soorder d on c.ordersysno=d.sysno
                                    left join bspaymenttype e on d.paytypesysno=e.sysno
                                    left join lgdeliverytype f on c.deliverytypesysno=f.sysno
                                    where c.status != {0} and t.deliverySysNo = @0";

                    subsql = string.Format(subsql, (int) Model.WorkflowStatus.WarehouseStatus.出库单状态.作废);

                    model.List = context.Sql(subsql, sysNo)
                                        .QueryMany<PrtDsSubDelivery>();

                    foreach (var item in model.List)
                    {
                        var pdsql = "";
                        if (item.NoteType == (int)Hyt.Model.WorkflowStatus.LogisticsStatus.配送单据类型.取件单)
                        {
                            item.NoteTypeName = Hyt.Model.WorkflowStatus.LogisticsStatus.配送单据类型.取件单.ToString();
                            item.ShopName = "商城官方商城";
                            pdsql = @"select t.ProductSysNo,t.ProductName,t.ProductQuantity,b.erpcode from LgPickUpItem t 
                                         left join pdproduct b on t.productsysno=b.sysno 
                                         where t.sysno=@0";
                        }
                        else
                        {
                            #region 取分销商店铺名称和订单类型

                            var dsModel = context.Sql(@"select t.*,a.shopname from DsOrder t   
                                       left join DsDealerMall a on t.dealermallsysno=a.sysno  
                                       where t.ordertransactionsysno=@0", item.TransactionSysNo)
                                                 .QuerySingle<CBDsOrder>();
                            if (dsModel == null)
                            {
                                item.NoteTypeName = "商城订单";
                                item.ShopName = "商城官方商城";
                            }
                            else
                            {
                                item.NoteTypeName = "升舱订单";
                                item.ShopName = dsModel.ShopName;
                            }

                            #endregion

                            pdsql = @"select t.*
                                        --,'' as MallOrderId
                                        ,b.erpcode
                                        ,c.paytypesysno
                                        ,d.paymentname
                                        ,d.paymenttype 
                                      from WhStockOutItem t 
                                        --left join DsOrder a on t.transactionsysno=a.OrderTransactionSysNo
                                        left join pdproduct b on t.productsysno=b.sysno
                                        left join soorder c on t.ordersysno=c.sysno
                                        left join bspaymenttype d on c.paytypesysno=d.sysno
                                        where t.stockoutsysno=@0";//出库单主表系统编号=item.NoteSysNo
                        }
                        item.PdList = context.Sql(pdsql, item.NoteSysNo)
                                             .QueryMany<PrtDsWhStockOutItem>();

                        if (item.PdList != null)
                        {
                            string dsorderSql = @"select a.mallorderid
                                              from DsOrder a
                                             where a.sysno =
                                                   (select d.dsordersysno
                                                      from dsorderitem d
                                                     where d.sysno =
                                                           (select c.dsorderitemsysno
                                                              from dsorderitemassociation c
                                                             where c.soorderitemsysno = {0}))";
                            foreach (var outItem in item.PdList)
                            {
                                string tmpSql = string.Format(dsorderSql, outItem.OrderItemSysNo);
                                outItem.MallOrderId = context.Sql(tmpSql).QuerySingle<string>();
                            }
                        }
                    }
                }
                return model;
            }

        }


        public override int InsertPrtPrintTempel(Model.Generated.PrtPrintTempel prtPrint)
        {
            return Context.Insert("PrtPrintTempel", prtPrint).AutoMap(p => p.SysNo).ExecuteReturnLastId<int>("SysNo");
        }

        public override void UpdatePrtPrintTempel(Model.Generated.PrtPrintTempel prtPrint)
        {
            Context.Update("PrtPrintTempel", prtPrint).AutoMap(p => p.SysNo).Where(p=>p.SysNo).Execute();
        }

        public override Model.Generated.PrtPrintTempel GetPrtPrintTempel()
        {
            string sql = "select top 1 * from PrtPrintTempel ";
            return Context.Sql(sql).QuerySingle<PrtPrintTempel>();
        }
    }
}
