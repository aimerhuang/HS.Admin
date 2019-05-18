using Hyt.DataAccess.Transport;
using Hyt.Model.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.Transport
{
    /// <summary>
    /// 货物档案数据列表
    /// </summary>
    /// <remarks>
    /// 2015-5-17 杨云奕 添加
    /// </remarks>
    public class DsWhGoodsManagementDaoImpl : IDsWhGoodsManagementDao
    {
        /// <summary>车辆
        /// 添加主表数据
        /// </summary>
        /// <param name="mod">实体</param>
        /// <returns></returns>
        public override int InsertMod(Model.Transport.DsWhGoodsManagement mod)
        {
            return Context.Insert<DsWhGoodsManagement>("DsWhGoodsManagement", mod).AutoMap(p => p.SysNo).ExecuteReturnLastId<int>("SysNo");
        }
        /// <summary>
        /// 更新主表数据
        /// </summary>
        /// <param name="mod">实体</param>
        public override void UpdateMod(Model.Transport.DsWhGoodsManagement mod)
        {
            Context.Update<DsWhGoodsManagement>("DsWhGoodsManagement", mod).AutoMap(p => p.SysNo).Where(p => p.SysNo).Execute();
        }
        /// <summary>
        /// 删除主表数据
        /// </summary>
        /// <param name="SysNo">自动编号</param>
        public override void DeleteMod(int SysNo)
        {
            string sql = " delete from DsWhGoodsManagement where SysNo = '"+SysNo+"' ";
            Context.Sql(sql).Execute();
                   sql = " delete from DsWhGoodsManagementList where PSysNo = '" + SysNo + "' ";
            Context.Sql(sql).Execute();
        }
        /// <summary>
        /// 获取实体数据
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public override Model.Transport.DsWhGoodsManagement GetModBySysNo(int SysNo)
        {
            string sql = "select * from DsWhGoodsManagement where SysNo ='" + SysNo + "' ";
            return Context.Sql(sql).QuerySingle<DsWhGoodsManagement>();
        }
        /// <summary>
        /// 获取实体数据
        /// </summary>
        /// <param name="SysNo">自动编号</param>
        /// <returns></returns>
        public override Model.Transport.CBDsWhGoodsManagement GetExtendsModBySysNo(int SysNo)
        {
            string sql = "select * from DsWhGoodsManagement where SysNo ='" + SysNo + "' ";
            CBDsWhGoodsManagement cbManagement =  Context.Sql(sql).QuerySingle<CBDsWhGoodsManagement>();
            cbManagement.ModList = GetModListByPSysNo(SysNo);
            return cbManagement;
        }
        /// <summary>
        /// 添加货物档案明细
        /// </summary>
        /// <param name="mod">货物档案明细</param>
        /// <returns></returns>
        public override int InsertModList(Model.Transport.DsWhGoodsManagementList mod)
        {
            return Context.Insert<DsWhGoodsManagementList>("DsWhGoodsManagementList", mod).AutoMap(p => p.SysNo).ExecuteReturnLastId<int>("SysNo");
        }
        /// <summary>
        /// 更新货物档案明细
        /// </summary>
        /// <param name="mod">货物档案明细</param>
        public override void UpdateModList(Model.Transport.DsWhGoodsManagementList mod)
        {
            Context.Update<DsWhGoodsManagementList>("DsWhGoodsManagementList", mod).AutoMap(p => p.SysNo).Where(p => p.SysNo).Execute();
        }
        /// <summary>
        /// 删除货物档案明细
        /// </summary>
        /// <param name="SysNo">自动编号</param>
        public override void DeleteModList(int SysNo)
        {
            string sql = "delete from DsWhGoodsManagementList where SysNo = '" + SysNo + "'";
            Context.Sql(sql).Execute();
        }
        /// <summary>
        /// 获取某一数据实体
        /// </summary>
        /// <param name="SysNo">自动编号</param>
        /// <returns></returns>
        public override Model.Transport.DsWhGoodsManagementList GetModListBySysNo(int SysNo)
        {
            string sql = " select * from DsWhGoodsManagementList where SysNo = '"+SysNo+"' ";
            return Context.Sql(sql).QuerySingle<DsWhGoodsManagementList>();
        }
        /// <summary>
        /// 通过父编号获取商品明细集合
        /// </summary>
        /// <param name="PSysNo">父id编号</param>
        /// <returns></returns>
        public override List<Model.Transport.DsWhGoodsManagementList> GetModListByPSysNo(int PSysNo)
        {
            string sql = " select * from DsWhGoodsManagementList where PSysNo = '" + PSysNo + "' ";
            return Context.Sql(sql).QueryMany<DsWhGoodsManagementList>();
        }

        public override List<DsWhGoodsManagement> GetModListByBatchNumber(string batchNumber, 
            bool IsBindAllDealer, bool IsBindDealer, bool IsCustomer, int DsSysNo, string CusCode)
        {
            string where = " BatchNumber = '" + batchNumber + "' and StatusCode='0' ";
            if (IsBindAllDealer)
            {
                where += " ";
            }
            else if(IsBindDealer)
            {
                where += " and DsWhCustomer.DsSysNo = '" + DsSysNo + "' ";
            }
            else if (IsCustomer)
            {
                where += " and CustomerCode = '" + CusCode + "' ";
            }
            string sql = " select * from "+
                " DsWhGoodsManagement inner join DsWhCustomer on DsWhCustomer.CusCode=DsWhGoodsManagement.CustomerCode ";
            sql += " where " + where;
            return Context.Sql(sql).QueryMany<DsWhGoodsManagement>();
        }

        public override void GoodsManagePager(ref Model.Pager<CBDsWhGoodsManagement> pageCusList, 
            bool IsBindAllDealer, bool IsBindDealer,
            bool IsCustomer, int DsSysNo, string CusCode, 
            string OrderByKey, string OrderbyType)
        {
            #region sql条件
            string sqlWhere = @"   ";
            if (IsBindAllDealer)
            {
                sqlWhere = " 1=1 ";
            }
            else if (IsBindDealer)
            {
                sqlWhere = "  DsWhCustomer.DsSysNo = '" + DsSysNo + "' ";
            }
            else if (IsCustomer)
            {
                sqlWhere = " CustomerCode = '" + CusCode + "' ";
            }
            if (pageCusList.PageFilter != null)
            {

                if (string.IsNullOrEmpty(pageCusList.PageFilter.StatusCode) || pageCusList.PageFilter.StatusCode.Trim() == "0")
                {
                    if (!string.IsNullOrEmpty(sqlWhere.Trim()))
                    {
                        sqlWhere += " and ";
                    }
                    sqlWhere += " DsWhGoodsManagement.StatusCode  > 0 ";
                }
                else
                {
                    if (!string.IsNullOrEmpty(sqlWhere.Trim()))
                    {
                        sqlWhere += " and ";
                    }
                    sqlWhere += " DsWhGoodsManagement.StatusCode  = " + pageCusList.PageFilter.StatusCode.Trim() + " ";
                }

                if (!string.IsNullOrEmpty(pageCusList.PageFilter.PayStatus))
                {
                   
                        if (!string.IsNullOrEmpty(sqlWhere.Trim()))
                        {
                            sqlWhere += " and ";
                        }
                        sqlWhere += " DsWhGoodsManagement.StatusCode  > 10 and DsWhGoodsManagement.PayStatus = " + pageCusList.PageFilter.PayStatus.Trim() + " ";
                    
                }

                if (!string.IsNullOrEmpty(pageCusList.PageFilter.ServiceType))
                {
                    if (!string.IsNullOrEmpty(sqlWhere.Trim()))
                    {
                        sqlWhere += " and ";
                    }
                    sqlWhere += " DsWhGoodsManagement.ServiceType = '" + pageCusList.PageFilter.ServiceType.Trim() + "' ";
                }

                if (!string.IsNullOrEmpty(pageCusList.PageFilter.CustomerCode) && !IsCustomer)
                {
                    if (!string.IsNullOrEmpty(sqlWhere.Trim()))
                    {
                        sqlWhere += " and ";
                    }
                    sqlWhere += " DsWhGoodsManagement.CustomerCode = '" + pageCusList.PageFilter.CustomerCode.Trim() + "' ";
                }

                if (!string.IsNullOrEmpty(pageCusList.PageFilter.AssNumber))
                {
                    if (!string.IsNullOrEmpty(sqlWhere.Trim()))
                    {
                        sqlWhere += " and  ";
                    }
                    sqlWhere += " ( DsWhGoodsManagement.AssNumber = '" + pageCusList.PageFilter.AssNumber.Trim() + "' ";
                    sqlWhere += " or  DsWhGoodsManagement.CourierNumber = '" + pageCusList.PageFilter.CourierNumber.Trim() + "' ";
                    sqlWhere += " or  DsWhGoodsManagement.Receipter = '" + pageCusList.PageFilter.Receipter.Trim() + "' )";
                }

                if (pageCusList.PageFilter.StartTime != null)
                {
                    if (!string.IsNullOrEmpty(sqlWhere.Trim()))
                    {
                        sqlWhere += " and ";
                    }
                    sqlWhere += " DsWhGoodsManagement.CreateTime >= '" + pageCusList.PageFilter.StartTime.Value.ToString("yyyy-MM-dd") + " 00:00:00' ";
                }

                if (pageCusList.PageFilter.EndTime != null)
                {
                    if (!string.IsNullOrEmpty(sqlWhere.Trim()))
                    {
                        sqlWhere += " and ";
                    }
                    sqlWhere += " DsWhGoodsManagement.CreateTime <= '" + pageCusList.PageFilter.EndTime.Value.ToString("yyyy-MM-dd") + " 23:59:59' ";
                }
            }
            #endregion



            using (var context = Context.UseSharedConnection(true))
            {
                pageCusList.Rows = context.Select<CBDsWhGoodsManagement>(" DsWhGoodsManagement.* ,LgDeliveryType.DeliveryTypeName as ServiceName,DsWhRecipienter.UploadFile as  IDCardImg")
                           .From(@"  DsWhGoodsManagement inner join DsWhCustomer on 
                                    DsWhCustomer.CusCode=DsWhGoodsManagement.CustomerCode inner join LgDeliveryType 
                                    on  LgDeliveryType.OverseaCarrier=DsWhGoodsManagement.ServiceType left join (select  distinct name,IDCard,UploadFile  from DsWhRecipienter) as  DsWhRecipienter  on DsWhRecipienter.IDCard=DsWhGoodsManagement.IDCard and DsWhRecipienter.Name=DsWhGoodsManagement.Receipter  ")
                           .Where(sqlWhere)
                           .Paging(pageCusList.CurrentPage, pageCusList.PageSize)
                           .OrderBy(" DsWhGoodsManagement." + (string.IsNullOrEmpty(OrderByKey) ? "SysNo" : OrderByKey) + " " + (string.IsNullOrEmpty(OrderbyType) ? "ASC" : OrderbyType) + " ")
                           .QueryMany();
                pageCusList.TotalRows = context.Select<int>("count(1)")
                           .From(@"  DsWhGoodsManagement inner join DsWhCustomer on DsWhCustomer.CusCode=DsWhGoodsManagement.CustomerCode
                                    inner join LgDeliveryType 
                                    on  LgDeliveryType.OverseaCarrier=DsWhGoodsManagement.ServiceType left join  (select  distinct name,IDCard,UploadFile  from DsWhRecipienter) as  DsWhRecipienter on DsWhRecipienter.IDCard=DsWhGoodsManagement.IDCard and DsWhRecipienter.Name=DsWhGoodsManagement.Receipter   ")
                           .Where(sqlWhere)
                           .QuerySingle();
            }
        }

        public override DsWhGoodsManagement GetModListByCourierNumber(string CourierNumber)
        {
            string where = " CourierNumber = '" + CourierNumber + "' or AssNumber = '" + CourierNumber + "' ";
            
            string sql = " select * from " +
                " DsWhGoodsManagement ";
            sql += " where " + where;
            return Context.Sql(sql).QuerySingle<DsWhGoodsManagement>();
        }

        public override List<DsWhGoodsManagement> GetDsWhGoodsManagementByCourierNumbers(List<string> packNumList)
        {
            string sql = " select * from DsWhGoodsManagement where CourierNumber in ('" + string.Join("','", packNumList.ToArray()) + "') ";
            return Context.Sql(sql).QueryMany<DsWhGoodsManagement>();
        }

        public override void UpdateModByWaybill(string MawbNumber,string StatusCode, List<string> waybillnumberList)
        {
            string sql = "update DsWhGoodsManagement set TotalWaybillNum='" + MawbNumber + "',StatusCode='" + StatusCode + "' where BagNumber in ('" + string.Join("','", waybillnumberList.ToArray()) + "')";
            Context.Sql(sql).Execute();
        }

        public override List<CBDsWhGoodsManagement> GetAllGoodsManageBySearch(CBDsWhGoodsManagement cbGoodsMan)
        {
            string sql = " select DsWhGoodsManagement.*,ToTalValue = (select sum(Quantiyt*GoodsPrice) from DsWhGoodsManagementList where PSysNo=DsWhGoodsManagement.SysNo) from  DsWhGoodsManagement  ";
            if (!string.IsNullOrEmpty(cbGoodsMan.TotalWaybillNum))
            {
                sql += " where DsWhGoodsManagement.TotalWaybillNum='" + cbGoodsMan.TotalWaybillNum + "' ";
            }
            return Context.Sql(sql).QueryMany<CBDsWhGoodsManagement>();
        }

        public override List<CBDsWhGoodsManagement> GetDsWhGoodsManagementByTotalWaybillNum(string WayWillNum)
        {
            string sql = "select DsWhGoodsManagement.* ,DsWhOutStockList.CalculateWeight from DsWhGoodsManagement left join DsWhOutStockList on  DsWhGoodsManagement.CourierNumber=DsWhOutStockList.GMCourierNumber";
            sql += " where TotalWaybillNum='" + WayWillNum + "' ";
            return Context.Sql(sql).QueryMany<CBDsWhGoodsManagement>();
        }

        public override void OrderManagerGroupPager(Model.Pager<WhGoodsManagementGroup> pageCusList)
        {
            #region sql条件
            string sqlWhere = @"  1=1 ";
            
            #endregion
            string typeList = pageCusList.PageFilter.CusCode;
            switch (typeList.Split('_')[0])
            {
                case "ALL":
                    sqlWhere = " 1=1 ";
                    break;
                case "Dealer":
                    sqlWhere = "( DsSysNo = '" + typeList.Split('_')[2] + "'   )";
                    
                    break;
                case "Customer":
                    sqlWhere = " ( CustomerCode = '" + typeList.Split('_')[1] + "' ) ";

                    break;
            }


            using (var context = Context.UseSharedConnection(true))
            {
                pageCusList.Rows = context.Select<WhGoodsManagementGroup>(" tab.*  ")
                           .From(@"  (select distinct 
	                                 CONVERT(varchar(10),CreateTime,120) as CreateTime, 'E'+ CONVERT(varchar(10),CreateTime,112) as OrderBatchNum ,
                                    DsWhCustomer.DsSysNo,
	                                 DsWhGoodsManagement.CustomerCode,
	                                 OrderCount=(select count(1) from DsWhGoodsManagement a where 
		                                CONVERT(varchar(10),a.CreateTime,120)=CONVERT(varchar(10),DsWhGoodsManagement.CreateTime,120)
		                                and
		                                DsWhGoodsManagement.CustomerCode=a.CustomerCode
                                     ) from DsWhGoodsManagement inner join DsWhCustomer on  DsWhGoodsManagement.CustomerCode=DsWhCustomer.CusCode ) tab ")
                           .Where(sqlWhere)
                           .Paging(pageCusList.CurrentPage, pageCusList.PageSize)
                           .OrderBy(" tab.CreateTime ASC ")
                           .QueryMany();
                pageCusList.TotalRows = context.Select<int>("count(1)")
                           .From(@"   (select distinct 
	                                 CONVERT(varchar(10),CreateTime,120) as CreateTime, 'E'+ CONVERT(varchar(10),CreateTime,112) as OrderBatchNum ,
                                        DsWhCustomer.DsSysNo,
	                                 DsWhGoodsManagement.CustomerCode,
	                                 OrderCount=(select count(1) from DsWhGoodsManagement a where 
		                                CONVERT(varchar(10),a.CreateTime,120)=CONVERT(varchar(10),DsWhGoodsManagement.CreateTime,120)
		                                and
		                                DsWhGoodsManagement.CustomerCode=a.CustomerCode
                                     ) from DsWhGoodsManagement inner join DsWhCustomer on  DsWhGoodsManagement.CustomerCode=DsWhCustomer.CusCode ) tab  ")
                           .Where(sqlWhere)
                           .QuerySingle();
            }
        }

        public override List<CBDsWhGoodsManagement> GetAllGoodsManageByCreateTime(string crateTime)
        {
            string sql = " select * from DsWhGoodsManagement where CONVERT(varchar(10),CreateTime,120)='" + crateTime + "'  ";
            return Context.Sql(sql).QueryMany<CBDsWhGoodsManagement>();
        }

        public override List<CBDsWhGoodsManagement> GetExtendsModBySysNos(string gmSysNos)
        {
            string sql = " select * from DsWhGoodsManagement where SysNo in  (" + gmSysNos + ")  ";
            return Context.Sql(sql).QueryMany<CBDsWhGoodsManagement>();
        }

        public override List<CBDsWhGoodsManagement> GetGoodsManageBySearchText(bool cb_Select, string ipt_Batch, string ipt_StartOrder, string ipt_EndOrder, 
            string ipt_OutStockDate, string sel_Stautus)
        {
            string sql = "select * from DsWhGoodsManagement where ";
            string where = " 1 = -1 ";
            if (!string.IsNullOrEmpty(ipt_Batch.Trim()))
            { 
                if(where==" 1 = -1 ")
                {
                    where = "BatchNumber='" + ipt_Batch + "'";
                }
                else
                {

                    where += " and  BatchNumber='" + ipt_Batch + "'";
                }
            }

            if (!string.IsNullOrEmpty(ipt_StartOrder.Trim()) && !string.IsNullOrEmpty(ipt_EndOrder.Trim()))
            {
                if (where == " 1 = -1 ")
                {
                    where = " (Convert(decimal(18,0), REPLACE(HTLogistics,'HTL',''))>=" + ipt_StartOrder.Replace("HTL", "") + " and  Convert(decimal(18,0), REPLACE(HTLogistics,'HTL',''))<=" + ipt_EndOrder.Replace("HTL", "") + " )";
                }
                else
                {

                    where += " and  (Convert(decimal(18,0), REPLACE(HTLogistics,'HTL',''))>=" + ipt_StartOrder.Replace("HTL", "") + " and  Convert(decimal(18,0), REPLACE(HTLogistics,'HTL',''))<=" + ipt_EndOrder.Replace("HTL", "") + " )";
                }
            }

            //if (!string.IsNullOrEmpty(ipt_OutStockDate.Trim()))
            //{
            //    if (where == " 1 = -1 ")
            //    {
            //        where = " (HTLogistics>='" + ipt_StartOrder + "' and  HTLogistics<='" + ipt_EndOrder + "' )";
            //    }
            //    else
            //    {

            //        where += " and  (HTLogistics>='" + ipt_StartOrder + "' and  HTLogistics<='" + ipt_EndOrder + "' )";
            //    }
            //}

            if (!string.IsNullOrEmpty(sel_Stautus.Trim()))
            {
                if (where == " 1 = -1 ")
                {
                    where = " (StatusCode='" + sel_Stautus + "' ";
                }
                else
                {

                    where += " and  (StatusCode='" + sel_Stautus + "' )";
                }
            }

            return Context.Sql(sql + where).QueryMany<CBDsWhGoodsManagement>();
        }

        public override DsWhGMReport GetDsWhReportData()
        {
            string sql = @" select
                    AllNumber=(select count(*) from DsWhGoodsManagement where StatusCode> 0 ),
                    AuditNumber=(select count(*) from DsWhGoodsManagement where StatusCode='1' ),
                    NotPass=(select count(*) from DsWhGoodsManagement where StatusCode='-10' ),
                    WaitCalculation=(select count(*) from DsWhGoodsManagement where StatusCode>'1' and PayStatus ='0' ),
                    WaitPay=(select count(*) from DsWhGoodsManagement where StatusCode>'1' and PayStatus ='1' ),
                    WaitOutStock=(select count(*) from DsWhGoodsManagement where StatusCode='50'  ),
                    HaveOutStock=(select count(*) from DsWhGoodsManagement where StatusCode='60'  ),
                    HaveGet=(select count(*) from DsWhGoodsManagement where StatusCode='70'  )
            ";
            return Context.Sql(sql).QuerySingle<DsWhGMReport>();
        }
    }
}
