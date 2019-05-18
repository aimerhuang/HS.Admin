using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model.ExpressList;
using Hyt.DataAccess.ExpressList;
using Hyt.Model;

namespace Hyt.DataAccess.Oracle.ExpressList
{
    public class IExpressListImpl : IExpressListDao
    {
        /// <summary>
        /// 判断是否已包裹
        /// </summary>
        /// <param name="WhStockInId">出库单</param>
        /// <returns></returns>
        /// <remarks> 2017-11-24 廖移凤</remarks>
        public override int GetExpressList(int WhStockInId)
        {
            return Context.Sql("select count(1) from ExpressList where WhStockInId=@0 ", WhStockInId).QuerySingle<int>();
        }


        /// <summary>
        /// 查询快递单号
        /// </summary>
        /// <param name="WhStockInId">出库单号</param>
        /// <returns></returns>
        ///  <remarks> 2017-12-01 廖移凤</remarks>
        public override int GetKuaiDiNo(int WhStockInId)
        {
            //int query = Context.Sql("select ExpressListNo from ExpressList where WhStockInId=@0 ", WhStockInId).QuerySingle<int>();

            return Context.Sql("select ExpressListNo from ExpressList where WhStockInId=@0 ", WhStockInId).QuerySingle<int>();
        }

        /// <summary>
        /// 保存快递单
        /// </summary>
        /// <param name="el">快递单实体</param>
        /// <returns></returns>
        /// <remarks>2017-11-24 廖移凤</remarks>
        public override int AddExpressList(ExpressLists el)
        {
            return Context.Sql("insert ExpressList values(@WhStockInId,@OrderSysNo,@ExpressListNo)").
                               Parameter("WhStockInId", el.WhStockInId).
                               Parameter("OrderSysNo", el.OrderSysNo).
                               Parameter("ExpressListNo", el.ExpressListNo).Execute();
        }

        /// <summary>
        /// 保存快递100接口 返回的数据
        /// </summary>
        /// <param name="kn"></param>
        /// <returns></returns>
        ///  <remarks>2017-11-28 廖移凤</remarks>
        public override int AddKdOrderNums(KdOrderNums kn)
        {
            return Context.Insert("KdOrderNum", kn).AutoMap(l => l.sysNo).Execute();
        }
        /// <summary>
        /// 查询接口参数
        /// </summary>
        /// <param name="StockOutSysNo"></param>
        /// <returns></returns>
        public override KdOrderParam GetKdOrderParam(int StockOutSysNo)
        {
            return Context.Sql(" select w.ProductQuantity [count],w.ProductName cargo,w.[Weight] [weight],w.Measurement volumn,w.Remarks remark,B.PaymentName payType,l.DeliveryTypeName expType,s.SysNo orderId " +
                                  " from WhStockOutItem w,SoOrder s,BsPaymentType B,LgDeliveryType l " +
                                  " where s.SysNo=w.OrderSysNo and s.PayTypeSysNo=B.SysNo  and s.DeliveryTypeSysNo=l.SysNo " +
                                  " and StockOutSysNo=@0 ", StockOutSysNo).QuerySingle<KdOrderParam>();
        }
        /// <summary>
        /// 查询发货人
        /// </summary>
        /// <param name="StockOutSysNo"></param>
        /// <returns></returns>
        public override RecMan GetRecMan(int StockOutSysNo)
        {

            return Context.Sql(" select b.areasysno zipCode,b.streetaddress printAddr,b.phone mobile,b.WarehouseName name " +
                              "  from whstockout t left join whwarehouse b on t.warehousesysno=b.sysno " +
                              "  left join soreceiveaddress c on t.receiveaddresssysno=c.sysno " +
                              " where t.sysno=@0 ", StockOutSysNo).QuerySingle<RecMan>();
        }
        /// <summary>
        /// 查询收货人
        /// </summary>
        /// <param name="StockOutSysNo"></param>
        /// <returns></returns>
        public override RecMan GetSRecMan(int StockOutSysNo)
        {
            
            return Context.Sql(" select  c.name name ,c.phonenumber tel,c.mobilephonenumber mobile,c.areasysno zipCode, " +
                               " c.streetaddress printAddr from whstockout t left join whwarehouse b on t.warehousesysno=b.sysno " +
                               " left join soreceiveaddress c on t.receiveaddresssysno=c.sysno  " +
                               " where t.sysno=@0 ", StockOutSysNo).QuerySingle<RecMan>();
        }


        /// <summary>
        /// 快递单查询
        /// </summary>
        /// <returns></returns>
        public override List<KuaiDiNumQuery> GetKuaiDiNumQuery() {
            return Context.Sql(@" select e.ExpressListNo as KuaidiNo,w.SysNo  as WhStocklnNo, s.SysNo as  OrderNo, srd.Name as Name 
                                 ,s.OrderAmount as Money,CASe   w.Status when 10 then '待出库'  when 20 then '待拣货'  when 30 then '待打包'  
						    	when 40 then '待配送'  when 50 then '配送中'when 60 then '已签收' when 70 then '拒收'   when 80 then '部分退货'  
								when 90 then '全部退货'  when -10 then '作废' end as Status,l.DeliveryTypeName as DeliveryTypeName 
                                from  ExpressList e,SoOrder s,WhStockOut w,SoReceiveAddress srd ,LgDeliveryType l 
                                where e.OrderSysNo=s.SysNo and e.WhStockInId=w.SysNo and s.ReceiveAddressSysNo=srd.SysNo and w.DeliveryTypeSysNo=l.SysNo ").QueryMany<KuaiDiNumQuery>();
        
        }


        /// <summary>
        /// 删除快递单
        /// </summary>
        /// <returns></returns>
        public override int DeleteKuaiDiNum(int KuaiDiNum)
        {

            return Context.Sql("delete ExpressList where ExpressListNo=@0", KuaiDiNum).Execute();
        }


        /// <summary>
        /// 搜索快递单
        /// </summary>
        /// <returns></returns>
        public override KuaiDiNumQuery SelectKuaiDiNum(int KuaiDiNum)
        {

            return Context.Sql(@" select e.ExpressListNo as KuaidiNo,w.SysNo  as WhStocklnNo, s.SysNo as  OrderNo, srd.Name as Name 
                                 ,s.OrderAmount as Money,CASe   w.Status when 10 then '待出库'  when 20 then '待拣货'  when 30 then '待打包'  
                                 when 40 then '待配送'  when 50 then '配送中'when 60 then '已签收' when 70 then '拒收'   when 80 then '部分退货'  
                                 when 90 then '全部退货'  when -10 then '作废' end as Status,l.DeliveryTypeName as DeliveryTypeName 
                                 from  ExpressList e,SoOrder s,WhStockOut w,SoReceiveAddress srd ,LgDeliveryType l 
                                 where e.OrderSysNo=s.SysNo and e.WhStockInId=w.SysNo and s.ReceiveAddressSysNo=srd.SysNo and w.DeliveryTypeSysNo=l.SysNo and 
                                 e.ExpressListNo=@0 ",KuaiDiNum).QuerySingle<KuaiDiNumQuery>();
        }


        /// <summary>
        /// 分页查询快递单
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        public override Pager<KuaiDiNumQuery> GetPage(Pager<KuaiDiNumQuery> pager)
        {
           
                const string sql = @"(select e.ExpressListNo as KuaidiNo,w.SysNo  as WhStocklnNo, s.SysNo as  OrderNo, srd.Name as Name 
                                    ,s.OrderAmount as Money,CASe   w.Status when 10 then '待出库'  when 20 then '待拣货'  when 30 then '待打包'  
                                    when 40 then '待配送'  when 50 then '配送中'when 60 then '已签收' when 70 then '拒收'   when 80 then '部分退货'  
                                    when 90 then '全部退货'  when -10 then '作废' end as Status,l.DeliveryTypeName as DeliveryTypeName 
                                    from  ExpressList e,SoOrder s,WhStockOut w,SoReceiveAddress srd ,LgDeliveryType l 
                                    where e.OrderSysNo=s.SysNo and e.WhStockInId=w.SysNo and s.ReceiveAddressSysNo=srd.SysNo and w.DeliveryTypeSysNo=l.SysNo ) tb";

                pager.Rows = Context.Select<KuaiDiNumQuery>("tb.*").From(sql).OrderBy("KuaidiNo desc")                       
                              .Paging(pager.CurrentPage, pager.PageSize)
                              .QueryMany();

                pager.TotalRows = Context.Select<int>("count(1)").From(sql).QuerySingle();
          
            return pager;
        }
    }
}
