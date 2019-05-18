using Hyt.DataAccess.FinancialStatistics;
using Hyt.Model.FinancialStatistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.FinancialStatistics
{
    public class FnStatisticsDaoImpl : IFnStatisticsDao
    {
         //Hyt.Model.SyFrontConfig syFrontConfig = Hyt.DataAccess.Oracle.Sys.SyFrontConfigDaoImpl.Instance.GetSyFrontConfigMod();
        #region 财务统计表主表
        /// <summary>
        /// 添加财务统计表内容
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public override int InsertFnStatistics(Model.FinancialStatistics.FnStatistics mod)
        {
            return Context.Insert("FnStatistics", mod).AutoMap(p => p.SysNo).ExecuteReturnLastId<int>("SysNo");
        }
        /// <summary>
        /// 更新财务统计表数据
        /// </summary>
        /// <param name="mod"></param>
        public override void UpdateFnStatistics(Model.FinancialStatistics.FnStatistics mod)
        {
            Context.Update("FnStatistics", mod).AutoMap(p => p.SysNo).Where(p => p.SysNo).Execute();
        }
        /// <summary>
        /// 删除财务统计表数据
        /// </summary>
        /// <param name="SysNo"></param>
        public override void DeleteFnStatistics(int SysNo)
        {
            string sql = " delete from FnStatistics where SysNo='"+SysNo+"'";
            Context.Sql(sql).Execute();
        }
        /// <summary>
        /// 通过年份获取财务统计数据列表
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public override List<Model.FinancialStatistics.CBFnStatistics> GetCBFnStatisticList(int year)
        {
            string sql = "select * from FnStatistics where year(BindTime)='" + year + "'";
            return Context.Sql(sql).QueryMany<Hyt.Model.FinancialStatistics.CBFnStatistics>();
        }
        /// <summary>
        /// 通过编号获取财务统计表明细
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public override Model.FinancialStatistics.CBFnStatistics GetCBFnStatisticMod(int SysNo)
        {
            string sql = "select * from FnStatistics where SysNo = '"+SysNo+"'";
            Hyt.Model.FinancialStatistics.CBFnStatistics cbStatisticMod = 
                Context.Sql(sql).QuerySingle<Model.FinancialStatistics.CBFnStatistics>();
            cbStatisticMod.SaleOrSpendList = GetCBFnSalesOrSpendStatistics(SysNo);
            return cbStatisticMod;
        }
        /// <summary>
        /// 获取所有统计信息
        /// </summary>
        /// <param name="year"></param>
        /// <param name="mouth"></param>
        /// <returns></returns>
        public override AllStatistics GetAllStatistics(int year, int mouth)
        {
            string sql = @" ---网购
                            select SUM(CashPay) as CashPay,BsPaymentType.PaymentName from soorder Inner join BsPaymentType on soorder.PayTypeSysNo=BsPaymentType.SysNo
                            where (ShopWarehouseNo is null or ShopWarehouseNo=0) and soorder.Status>=30 and year(soorder.CreateDate)="+year+@" and MONTH(soorder.CreateDate)="+mouth+@"
                            group by BsPaymentType.PaymentName
                            union
                            ---实体店下单
                            select SUM(CashPay) as CashPay,'实体店进货' as PaymentName from soorder Inner join BsPaymentType on soorder.PayTypeSysNo=BsPaymentType.SysNo
                            where  ShopWarehouseNo>0  and soorder.Status>=30 and year(soorder.CreateDate)=" + year + @" and MONTH(soorder.CreateDate)=" + mouth + @"
                            union
                            ---保税商品
                            select SUM(CashPay) as CashPay,'保税商品收入' as PaymentName from soorder inner join WhWarehouse on soorder.DefaultWarehouseSysNo=WhWarehouse.SysNo
                            where  soorder.Status>=30 and WhWarehouse.WarehouseType=30 and year(soorder.CreateDate)=" + year + @" and MONTH(soorder.CreateDate)=" + mouth + @"
                            ---退货单 
                            union
                            select sum(RefundTotalAmount) as CashPay, '退货费用' as PaymentName from RcReturn
                            where [Status]=50 and year(RcReturn.CreateDate)=" + year + @" and MONTH(RcReturn.CreateDate)=" + mouth + @"
                            union
                            select sum(soorder.FreightAmount) as CashPay ,'德邦物流' as PaymentName
                            from soorder inner join LgDeliveryType on soorder.DeliveryTypeSysNo=LgDeliveryType.SysNo
                            where soorder.Status>=30 and LgDeliveryType.DeliveryTypeName like '%德邦%'
                             and year(soorder.CreateDate)=" + year + @" and MONTH(soorder.CreateDate)=" + mouth + @"
                            union
                            select sum(soorder.FreightAmount) as CashPay ,'顺丰物流' as PaymentName
                            from soorder inner join LgDeliveryType on soorder.DeliveryTypeSysNo=LgDeliveryType.SysNo
                            where soorder.Status>=30 and LgDeliveryType.DeliveryTypeName like '%顺丰%'
                             and year(soorder.CreateDate)=" + year + @" and MONTH(soorder.CreateDate)=" + mouth + @"
                            union
                            select sum(soorder.FreightAmount) as CashPay ,'心怡物流' as PaymentName
                            from soorder inner join LgDeliveryType on soorder.DeliveryTypeSysNo=LgDeliveryType.SysNo
                            where soorder.Status>=30 and LgDeliveryType.DeliveryTypeName like '%心怡%'
                             and year(soorder.CreateDate)=" + year + @" and MONTH(soorder.CreateDate)=" + mouth + @"
                            union
                            select sum(soorder.TaxFee) as CashPay ,'行邮税费' as PaymentName
                            from soorder inner join LgDeliveryType on soorder.DeliveryTypeSysNo=LgDeliveryType.SysNo
                            where soorder.Status>=30  and soorder.TaxFee>=0
                            and year(soorder.CreateDate)=" + year + @" and MONTH(soorder.CreateDate)=" + mouth + @"
                        ";
            ///获取金额
            List<StatisticsType> typeList = Context.Sql(sql).QueryMany<StatisticsType>();
            AllStatistics mod = new AllStatistics();
            mod.AliPay =
                typeList.Find(p => p.PaymentName == "支付宝") == null ? 0 : 
                typeList.Find(p => p.PaymentName == "支付宝").CashPay;

            mod.Bank = 
                typeList.Find(p => p.PaymentName == "网银")==null? 0: 
                typeList.Find(p => p.PaymentName == "网银").CashPay;

            mod.BaoShui =
                typeList.Find(p => p.PaymentName == "保税商品收入") == null ? 0 : 
                typeList.Find(p => p.PaymentName == "保税商品收入").CashPay;

            mod.Debang =
                typeList.Find(p => p.PaymentName == "德邦物流") == null ? 0 : 
                typeList.Find(p => p.PaymentName == "德邦物流").CashPay;

            mod.HaiGuanPostTax =
                typeList.Find(p => p.PaymentName == "行邮税费") == null ? 0 : 
                typeList.Find(p => p.PaymentName == "行邮税费").CashPay;

            mod.RetPrice =
                typeList.Find(p => p.PaymentName == "退货费用") == null ? 0 : 
                typeList.Find(p => p.PaymentName == "退货费用").CashPay;

            mod.SF =
                typeList.Find(p => p.PaymentName == "顺丰物流") == null ? 0 : 
                typeList.Find(p => p.PaymentName == "顺丰物流").CashPay;

            mod.StoreStock =
                typeList.Find(p => p.PaymentName == "实体店进货") == null ? 0 : 
                typeList.Find(p => p.PaymentName == "实体店进货").CashPay;

            mod.WebXin =
                typeList.Find(p => p.PaymentName == "微信支付") == null ? 0 : 
                typeList.Find(p => p.PaymentName == "微信支付").CashPay;

            mod.XinYiLogistics =
                typeList.Find(p => p.PaymentName == "心怡物流") == null ? 0 : 
                typeList.Find(p => p.PaymentName == "心怡物流").CashPay;

            mod.Cash =
                typeList.Find(p => p.PaymentName == " 现金") == null ? 0 :
                typeList.Find(p => p.PaymentName == " 现金").CashPay;

            return mod;
        }

        public override List<StatisticsDataMod> GetStatisticsDataMod(string type, int year, int month)
        {
            List<StatisticsDataMod> modList = new List<StatisticsDataMod>();
            string sql = "";
            switch(type)
            {
                case "微信支付":
                case "支付宝":
                case "网银":
                case "现金":
                    sql = @"select  CashPay as Amount,BsPaymentType.PaymentName as DataInfo,soorder.SysNo from soorder Inner join BsPaymentType on soorder.PayTypeSysNo=BsPaymentType.SysNo
                            where (ShopWarehouseNo is null or ShopWarehouseNo=0) and soorder.Status>=30 and year(soorder.CreateDate)=" + year + " and MONTH(soorder.CreateDate)=" + month + "";
                    sql += " and BsPaymentType.PaymentName like '%" + type + "%' ";
                    modList = Context.Sql(sql).QueryMany<StatisticsDataMod>();
                    break;
                case "实体店进货":
                    sql = @"select  CashPay as Amount,'实体店进货'+ BsPaymentType.PaymentName as DataInfo,soorder.SysNo from soorder Inner join BsPaymentType on soorder.PayTypeSysNo=BsPaymentType.SysNo
                            where (ShopWarehouseNo > 0) and soorder.Status>=30 and year(soorder.CreateDate)=" + year + " and MONTH(soorder.CreateDate)=" + month + "";
                    //sql += " and BsPaymentType.PaymentName='" + type + "' ";
                    modList = Context.Sql(sql).QueryMany<StatisticsDataMod>();
                    break;
                case "保税商品收入":
                    sql = @"select  CashPay as Amount ,'保税商品收入' as DataInfo, soorder.SysNo  from soorder inner join WhWarehouse on soorder.DefaultWarehouseSysNo=WhWarehouse.SysNo
                            where  soorder.Status>=30 and WhWarehouse.WarehouseType=30 and ";
                    sql += @" year(soorder.CreateDate)=" + year + " and MONTH(soorder.CreateDate)=" + month + "";
                    modList = Context.Sql(sql).QueryMany<StatisticsDataMod>();
                    break;
                case "退货费用":
                    sql = @"select RefundTotalAmount as Amount, '退货费用' as DataInfo,RcReturn.SysNo from RcReturn
                            where [Status]=50 and ";
                    sql += @" year(RcReturn.CreateDate)=" + year + " and MONTH(RcReturn.CreateDate)=" + month + "";
                    modList = Context.Sql(sql).QueryMany<StatisticsDataMod>();
                    break;
                case "德邦物流":
                    sql = @"select soorder.FreightAmount as Amount ,'德邦物流' as DataInfo, soorder.SysNo
                            from soorder inner join LgDeliveryType on soorder.DeliveryTypeSysNo=LgDeliveryType.SysNo
                            where soorder.Status>=30 and LgDeliveryType.DeliveryTypeName like '%德邦%' ";
                    sql += " and year(soorder.CreateDate)=" + year + @" and MONTH(soorder.CreateDate)=" + month + @" ";
                    modList = Context.Sql(sql).QueryMany<StatisticsDataMod>();
                    break;
                case "顺丰物流":
                    sql = @"select soorder.FreightAmount as Amount ,'顺丰物流' as DataInfo, soorder.SysNo
                            from soorder inner join LgDeliveryType on soorder.DeliveryTypeSysNo=LgDeliveryType.SysNo
                            where soorder.Status>=30 and LgDeliveryType.DeliveryTypeName like '%顺丰%' ";
                    sql += " and year(soorder.CreateDate)=" + year + @" and MONTH(soorder.CreateDate)=" + month + @" ";
                    modList = Context.Sql(sql).QueryMany<StatisticsDataMod>();
                    break;
                case "心怡物流":
                    sql = @"select soorder.FreightAmount as Amount ,'心怡物流' as DataInfo, soorder.SysNo
                            from soorder inner join LgDeliveryType on soorder.DeliveryTypeSysNo=LgDeliveryType.SysNo
                            where soorder.Status>=30 and LgDeliveryType.DeliveryTypeName like '%心怡%' ";
                    sql += " and year(soorder.CreateDate)=" + year + @" and MONTH(soorder.CreateDate)=" + month + @" ";
                    modList = Context.Sql(sql).QueryMany<StatisticsDataMod>();
                    break;
                case "行邮税费":
                    sql = @" select (soorder.TaxFee)  as Amount ,'行邮税费' as DataInfo, soorder.SysNo
                            from soorder inner join LgDeliveryType on soorder.DeliveryTypeSysNo=LgDeliveryType.SysNo
                            where soorder.Status>=30  and soorder.TaxFee>=0 ";
                    sql += " and year(soorder.CreateDate)=" + year + @" and MONTH(soorder.CreateDate)=" + month + @" ";
                    modList = Context.Sql(sql).QueryMany<StatisticsDataMod>();
                    break;
            }

            return modList;
        }
        public override CBFnStatistics GetCBFnStatisticMod(DateTime dateTime)
        {
            string sql = "select * from FnStatistics where year(BindTime) = " + dateTime.Year
                + " and  MONTH(BindTime) = " + dateTime.Month + " ";
            Hyt.Model.FinancialStatistics.CBFnStatistics cbStatisticMod =
                Context.Sql(sql).QuerySingle<Model.FinancialStatistics.CBFnStatistics>();

            if (cbStatisticMod!=null)
            {
                cbStatisticMod.SaleOrSpendList = GetCBFnSalesOrSpendStatistics(cbStatisticMod.SysNo);
            }
            
            return cbStatisticMod;
        }
        #endregion
        #region 财务统计表明细
        public override int InsertFnSalesOrSpendStatistics
            (Model.FinancialStatistics.FnSalesOrSpendStatistics SaleOrSpendMod)
        {
            return Context.Insert("FnSalesOrSpendStatistics", SaleOrSpendMod).AutoMap(p => p.SysNo)
                .ExecuteReturnLastId<int>("SysNo");
        }

        public override void UpdateFnSalesOrSpendStatistics(Model.FinancialStatistics.FnSalesOrSpendStatistics SaleOrSpendMod)
        {
            Context.Update("FnSalesOrSpendStatistics", SaleOrSpendMod).AutoMap(p => p.SysNo)
                .Where(p => p.SysNo).Execute();
        }

        public override void DeleteFnSalesOrSpendStatistics(int SysNo)
        {
            string sql = "delete from FnSalesOrSpendStatistics where SysNo = '" + SysNo + "'";
            Context.Sql(sql).Execute();
        }

        public override List<Model.FinancialStatistics.CBFnSalesOrSpendStatistics> GetCBFnSalesOrSpendStatistics(int PSysNo)
        {
            string sql = "select * from FnSalesOrSpendStatistics inner join FnStatisticsType on FnSalesOrSpendStatistics.PTSysNo=FnStatisticsType.SysNo where PSysNo='" + PSysNo + "' ";
            return Context.Sql(sql).QueryMany<CBFnSalesOrSpendStatistics>();
        }

        #endregion
        #region 财务统计表类型
        public override int InsertFnStatisticsType(Model.FinancialStatistics.FnStatisticsType type)
        {
            return Context.Insert("FnStatisticsType", type).AutoMap(p=>p.SysNo).ExecuteReturnLastId<int>();
        }

        public override void UpdateFnStatisticsType(Model.FinancialStatistics.FnStatisticsType type)
        {
            Context.Update("FnStatisticsType", type).AutoMap(p => p.SysNo).Where(p => p.SysNo).Execute();
        }

        public override void DeleteFnStatisticsType(int SysNo)
        {
            string sql = " delete from FnStatisticsType where SysNo = '" + SysNo + "' ";
            Context.Sql(sql).Execute();
        }

        public override List<Model.FinancialStatistics.FnStatisticsType> GetFnStatisticsTypeList()
        {
            string sql = " select * from FnStatisticsType ";
            return Context.Sql(sql).QueryMany<FnStatisticsType>();
        }
        #endregion
    }
}
