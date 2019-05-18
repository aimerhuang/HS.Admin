using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.DataAccess.BaseInfo;
using Hyt.DataAccess.CRM;
using Hyt.DataAccess.Logistics;
using Hyt.DataAccess.Order;
using Hyt.DataAccess.Sys;
using Hyt.Model;
using Hyt.Model.SystemPredefined;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.DataAccess.Warehouse;

namespace Hyt.DataAccess.Oracle.Logistics
{
    /// <summary>
    /// 配送单数据访问类
    /// </summary>
    /// <remarks>
    /// 2013/6/14 何方 创建
    /// </remarks>
    public class LgDeliveryDaoImpl : ILgDeliveryDao
    {
        /// <summary>
        /// 根据订单编号获取配送单
        /// </summary>
        /// <param name="sysNo">订单编号</param>
        /// <returns></returns>
        /// <remarks>2016-7-16 杨浩 创建</remarks>
        public override List<LgDeliveryItem> GetDeliveryItemByOrderSysNo(int sysNo)
        {
            return Context.Sql(@"SELECT di.* FROM LgSettlementItem AS si
								 INNER JOIN [WhStockOut] AS wout ON si.StockOutSysNo=wout.SysNo
								 INNER JOIN [LgDeliveryItem] AS di ON di.DeliverySysNo=si.DeliverySysNo WHERE wout.OrderSysNO=@0 and wout.Status>0 ", sysNo).QueryMany<LgDeliveryItem>();
        }
        /// <summary>
        /// 创建配送单
        /// </summary>
        /// <param name="model">配送单主表实体</param>
        /// <returns>返回系统编号</returns>
        /// <remarks>
        /// 2013-6-14 何方 创建
        /// </remarks>
        public override int CreateLgDelivery(LgDelivery model)
        {
            if (model.LastUpdateDate == DateTime.MinValue)
            {
                model.LastUpdateDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            if (model.Stamp == DateTime.MinValue)
            {
                model.Stamp = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            return Context.Insert<LgDelivery>("LgDelivery", model)
                .AutoMap(x => x.SysNo, x => x.LgDeliveryItemList)
                .ExecuteReturnLastId<int>("SysNo");
        }
        /// <summary>
        /// 获取配送单明细的物流信息
        /// </summary>
        /// <param name="TransactionSysNo">事务编号</param>
        /// <returns>配送物流内容的</returns>
        /// <remarks>2015-10-08 杨云奕 添加</remarks>
        public override IList<LgDeliveryItem> GetLgDeliveryItemListByTransactionSysNo(string TransactionSysNo)
        {
            string sql = " select * from LgDeliveryItem where TransactionSysNo = '" + TransactionSysNo + "' ";
            return Context.Sql(sql).QueryMany<LgDeliveryItem>();
        }
        /// <summary>
        /// 获取配送单集合
        /// </summary>
        /// <param name="pager">分页对象</param>
        /// <param name="currentUser">当前用户系统编号,若传入则查询当前用户用权限的仓库</param>        
        /// <param name="hasAllWarehouse">是否拥有所有仓库</param>
        /// <returns></returns>
        /// <remarks>2013-06-17 沈强 创建 2013-11-27 黄伟 修改 加入仓库权限</remarks>
        public override void GetLogisticsDeliveryItems(ref Pager<CBLgDelivery> pager, int currentUser,
            bool hasAllWarehouse)
        {
            CBLgDelivery cbLgDelivery = pager.Rows[0];

            //获取查询参数
            int sysNo = cbLgDelivery.SysNo;
            int stockSysNo = cbLgDelivery.StockSysNo;
            int deliveryUserSysNo = cbLgDelivery.DeliveryUserSysNo;
            int status = cbLgDelivery.Status;
            string noteSysNo = cbLgDelivery.StockOutNo;
            string createdName = cbLgDelivery.CreatedName;
            string createdStartDate = cbLgDelivery.CreatedStartDate;
            string createdEndDdate = cbLgDelivery.CreatedEndDate;
            DateTime start = DateTime.MinValue;
            DateTime end = DateTime.MinValue;
            if (!string.IsNullOrEmpty(createdStartDate))
            {
                start = DateTime.Parse(createdStartDate);
            }
            if (!string.IsNullOrEmpty(createdEndDdate))
            {
                end = DateTime.Parse(createdEndDdate).AddDays(1);
            }

            int? isThirdPartyExpress = null;
            if (!string.IsNullOrEmpty(cbLgDelivery.IsThirdPartyExpress))
            {
                isThirdPartyExpress = int.Parse(cbLgDelivery.IsThirdPartyExpress);
            }

            pager.Rows.Clear();

            #region 调试Sql

            /*
             select * from
                (
                       select sysno
                          , stocksysno
                          , DeliveryUserSysNo
                          , (select syuser.username from syuser where a.DeliveryUserSysNo = syuser.sysno) as DeliveryManName
                          , (select count(1) from LgDeliveryItem where a.sysno = LgDeliveryItem.DeliverySysNo and IsCOD = 0) as PaidNoteCount
                          , paidamount
                          , (select count(1) from LgDeliveryItem where a.sysno = LgDeliveryItem.DeliverySysNo and IsCOD = 1) as CODNoteCount
                          , codamount
                          , status
                          , isenforceallow
                          --, deliverymode
                          --, expressno
                          , createdby
                          , (select syuser.username from syuser where a.createdby = syuser.sysno) as CreatedName
                          , createddate
                          , (select count(1) from LgDeliveryItem where a.sysno = LgDeliveryItem.DeliverySysNo and LgDeliveryItem.Status != 10) as isCancel
                           from lgdelivery a
                ) t 
             */

            #endregion

            #region selectSql

            //DeliveryManName ： 配送员姓名
            //PaidNoteCount ： 预付单量
            //CODNoteCount ： 到付单量
            //CreatedName ： 创建者姓名
            //isCancel : 配送单是否可以作废，只有为 0 时可以作废
            //DeliveryTypeName : 配送方式名称
            //IsThirdPartyExpress ： 是否为三方快递 是(1),否(0)
            //string sqlInner=
            string selectSql = (currentUser != 0 && !hasAllWarehouse)
                ? @"(
                   select t.* from
                    (
                           select sysno
                              , stocksysno
                              , DeliveryUserSysNo
                              , (select syuser.username from syuser where a.DeliveryUserSysNo = syuser.sysno) as DeliveryManName
                              , (select count(1) from LgDeliveryItem where a.sysno = LgDeliveryItem.DeliverySysNo and IsCOD = {0}) as PaidNoteCount
                              , paidamount
                              , (select count(1) from LgDeliveryItem where a.sysno = LgDeliveryItem.DeliverySysNo and IsCOD = {1}) as CODNoteCount
                              , codamount
                              , status
                              , isenforceallow
                              , deliverytypesysno
                              , createdby
                              , (select syuser.username from syuser where a.createdby = syuser.sysno) as CreatedName
                              , createddate
                              , (select count(1) from LgDeliveryItem where a.sysno = LgDeliveryItem.DeliverySysNo and LgDeliveryItem.Status != {2}) as isCancel
                              ,(select DeliveryTypeName from LgDeliveryType where a.deliverytypesysno = LgDeliveryType.Sysno) as  DeliveryTypeName
                              ,(select IsThirdPartyExpress from LgDeliveryType where a.deliverytypesysno = LgDeliveryType.Sysno) as IsThirdPartyExpress
                               from lgdelivery a --where a.stocksysno in(@stocksysnos) //mex in list not supported --huangwei
                               where exists (select wh.sysno
                            from WHWAREHOUSE wh
                            inner join SYUSERWAREHOUSE swh on swh.warehousesysno=wh.sysno
                            and swh.usersysno={3} where wh.sysno=a.stocksysno) 
                    ) t  where 
                               (@SysNo = 0 or t.sysno = @SysNo)
                           and (@StockSysNo = 0 or t.stocksysno = @StockSysNo)
                           and (@DeliveryUserSysNo = 0 or t.DeliveryUserSysNo = @DeliveryUserSysNo)
                           and (@Status = 0 or t.status = @Status)
                           and (@NoteSysNo is null or exists(select 1 from LgDeliveryItem b where b.NoteSysNo = @NoteSysNo and b.DeliverySysNo = t.sysno))
                           and (@CreatedName is null or t.CreatedName like @CreatedName1)
                           and (@DateStart is null or t.createddate >= @DateStart)
                           and (@DateEnd is null or t.createddate < @DateEnd) 
                           and (@IsThirdPartyExpress is null or t.IsThirdPartyExpress = @IsThirdPartyExpress)
            ) CBLgDelivery"
                : @"(
                   select t.* from
                    (
                           select sysno
                              , stocksysno
                              , DeliveryUserSysNo
                              , (select syuser.username from syuser where a.DeliveryUserSysNo = syuser.sysno) as DeliveryManName
                              , (select count(1) from LgDeliveryItem where a.sysno = LgDeliveryItem.DeliverySysNo and IsCOD = {0}) as PaidNoteCount
                              , paidamount
                              , (select count(1) from LgDeliveryItem where a.sysno = LgDeliveryItem.DeliverySysNo and IsCOD = {1}) as CODNoteCount
                              , codamount
                              , status
                              , isenforceallow
                              , deliverytypesysno
                              , createdby
                              , (select syuser.username from syuser where a.createdby = syuser.sysno) as CreatedName
                              , createddate
                              , (select count(1) from LgDeliveryItem where a.sysno = LgDeliveryItem.DeliverySysNo and LgDeliveryItem.Status != {2}) as isCancel
                              ,(select DeliveryTypeName from LgDeliveryType where a.deliverytypesysno = LgDeliveryType.Sysno) as  DeliveryTypeName
                              ,(select IsThirdPartyExpress from LgDeliveryType where a.deliverytypesysno = LgDeliveryType.Sysno) as IsThirdPartyExpress
                               from lgdelivery a
                    ) t  where 
                               (@SysNo = 0 or t.sysno = @SysNo)
                           and (@StockSysNo = 0 or t.stocksysno = @StockSysNo)
                           and (@DeliveryUserSysNo = 0 or t.DeliveryUserSysNo = @DeliveryUserSysNo)
                           and (@Status = 0 or t.status = @Status)
                           and (@NoteSysNo is null or exists(select 1 from LgDeliveryItem b where b.NoteSysNo = @NoteSysNo and b.DeliverySysNo = t.sysno))
                           and (@CreatedName is null or t.CreatedName like @CreatedName1)
                           and (@DateStart is null or t.createddate >= @DateStart)
                           and (@DateEnd is null or t.createddate < @DateEnd) 
                           and (@IsThirdPartyExpress is null or t.IsThirdPartyExpress = @IsThirdPartyExpress)
            ) CBLgDelivery";

            #endregion

            selectSql = currentUser != 0
                ? string.Format(selectSql, (int)Hyt.Model.WorkflowStatus.LogisticsStatus.是否到付.否
                    , (int)Hyt.Model.WorkflowStatus.LogisticsStatus.是否到付.是
                    , (int)Hyt.Model.WorkflowStatus.LogisticsStatus.配送单明细状态.待签收, currentUser)
                : string.Format(selectSql, (int)Hyt.Model.WorkflowStatus.LogisticsStatus.是否到付.否
                    , (int)Hyt.Model.WorkflowStatus.LogisticsStatus.是否到付.是
                    , (int)Hyt.Model.WorkflowStatus.LogisticsStatus.配送单明细状态.待签收);

            var selectForm = Context.Select<CBLgDelivery>("CBLgDelivery.*").From(selectSql);
            var selectCount = Context.Select<int>("count(1)").From(selectSql);

            if (createdStartDate != null && Convert.ToDateTime(createdStartDate) == DateTime.MinValue)
            {
                createdStartDate = System.Data.SqlTypes.SqlDateTime.MinValue.ToString();
            }
            if (createdEndDdate != null && Convert.ToDateTime(createdEndDdate) == DateTime.MinValue)
            {
                createdEndDdate = System.Data.SqlTypes.SqlDateTime.MinValue.ToString();
            }
            if (start == DateTime.MinValue)
            {
                start = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            if (end == DateTime.MinValue)
            {
                end = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            selectForm.
                Parameter("SysNo", sysNo)
                .Parameter("StockSysNo", stockSysNo)
                .Parameter("DeliveryUserSysNo", deliveryUserSysNo)
                .Parameter("Status", status)
                .Parameter("NoteSysNo", noteSysNo)
                .Parameter("CreatedName", createdName)
                .Parameter("CreatedName1", "%" + createdName + "%")
                .Parameter("DateStart", createdStartDate)
                .Parameter("DateEnd", createdEndDdate)
                .Parameter("IsThirdPartyExpress", isThirdPartyExpress);

            selectCount.Parameter("SysNo", sysNo)
                .Parameter("StockSysNo", stockSysNo)
                .Parameter("DeliveryUserSysNo", deliveryUserSysNo)
                .Parameter("Status", status)
                .Parameter("NoteSysNo", noteSysNo)
                .Parameter("CreatedName", createdName)
                .Parameter("CreatedName1", "%" + createdName + "%")
                .Parameter("DateStart", createdStartDate)
                .Parameter("DateEnd", createdEndDdate)
                .Parameter("IsThirdPartyExpress", isThirdPartyExpress);

            var totalRows = selectCount.QuerySingle();
            //var fileredRowsByWh=selectForm.QueryMany().Where(p=>cbLgDelivery.StockSysNos.Contains(p.StockSysNo))
            var temps = selectForm.OrderBy("CBLgDelivery.createddate desc")
                .Paging(pager.CurrentPage, pager.PageSize)
                .QueryMany();

            pager.TotalRows = totalRows;
            pager.Rows = temps;
        }

        /// <summary>
        /// 获取配送单集合
        /// </summary>
        /// <param name="pagerFilter">查询过滤对象</param>
        /// <param name="currentUserSysNo">当前用户系统编号</param>
        /// <param name="hasAllWarehouse">是否拥有所有仓库</param>
        /// <returns>返回配送单集合</returns>
        /// <remarks>2013-10-18 沈强 创建</remarks>
        public override Pager<CBLgDelivery> GetLogisticsDeliveryItems(
            Pager<Hyt.Model.Parameter.ParaLogisticsFilter> pagerFilter, int currentUserSysNo, bool hasAllWarehouse)
        {

            Model.Parameter.ParaLogisticsFilter cbLgDelivery = pagerFilter.Rows[0];

            var pager = new Pager<CBLgDelivery>
            {
                CurrentPage = pagerFilter.CurrentPage,
                PageSize = pagerFilter.PageSize
            };

            #region 优化后的sql

            //DeliveryManName ： 配送员姓名
            //PaidNoteCount ： 预付单量
            //CODNoteCount ： 到付单量
            //CreatedName ： 创建者姓名
            //isCancel : 配送单是否可以作废，只有为 0 时可以作废
            //DeliveryTypeName : 配送方式名称
            //IsThirdPartyExpress ： 是否为三方快递 是(1),否(0)
            string sqlSelect = @"select tb.*
                           from (
                                           select sysno
                                              ,stocksysno
                                              ,DeliveryUserSysNo
                                              ,(select syuser.username from syuser where a.DeliveryUserSysNo = syuser.sysno) as DeliveryManName
                                              --,'' as DeliveryManName
                                              ,(select count(1) from LgDeliveryItem where a.sysno = LgDeliveryItem.DeliverySysNo and IsCOD = {0}) as PaidNoteCount
                                              ,paidamount
                                              ,(select count(1) from LgDeliveryItem where a.sysno = LgDeliveryItem.DeliverySysNo and IsCOD = {1}) as CODNoteCount
                                              ,codamount
                                              ,status
                                              ,isenforceallow
                                              ,deliverytypesysno
                                              ,createdby
                                              ,(select syuser.username from syuser where a.createdby = syuser.sysno) as CreatedName
                                              --,'' as CreatedName
                                              ,createddate
                                              ,(select count(1) from LgDeliveryItem where a.sysno = LgDeliveryItem.DeliverySysNo and LgDeliveryItem.Status != {2}) as isCancel
                                              ,(select DeliveryTypeName from LgDeliveryType where a.deliverytypesysno = LgDeliveryType.Sysno) as  DeliveryTypeName
                                              ,(select IsThirdPartyExpress from LgDeliveryType where a.deliverytypesysno = LgDeliveryType.Sysno) as IsThirdPartyExpress
                                              ,row_number() over (order by a.sysno desc) FLUENTDATA_ROWNUMBER 
                                               from lgdelivery a
                                               where 
                                                    (@0 = 0 or a.sysno = @0)
                                                and (@1 = 0 or a.stocksysno = @1)
                                                and (@2 = 0 or a.DeliveryUserSysNo = @2)
                                                and (@3 = 0 or a.status = @3)
                                                and (@4 is null or exists(select 1 from LgDeliveryItem b where b.NoteSysNo = @4 and b.DeliverySysNo = a.sysno))
                                                and (@5 is null or a.createddate >= @5)
                                                and (@6 is null or a.createddate < @6)
                                                and (@7 = 0 or (select count(1) from LgDeliveryItem c where a.sysno = c.deliverysysno and c.transactionsysno = (select TransactionSysNo from SoOrder d where d.sysno = @7)) > 0)
                                                and (@8 is null or exists(select 1 from syuser where a.createdby = syuser.sysno and syuser.username like @9))
                                                and (@10 is null or exists(select 1 from LgDeliveryType where a.deliverytypesysno = LgDeliveryType.Sysno and LgDeliveryType.IsThirdPartyExpress = @10))
                                                and (@11 is null or exists(select 1 from SyUserWarehouse e  where e.WarehouseSysNo = a.StockSysNo and e.UserSysNo = @12))
                        ) tb
                        where fluentdata_RowNumber between {3} and {4}
                        order by fluentdata_RowNumber";

            string countSelect = @"select count(1)
                                               from lgdelivery a
                                               where 
                                                    (@0 = 0 or a.sysno = @0)
                                                and (@1 = 0 or a.stocksysno = @1)
                                                and (@2 = 0 or a.DeliveryUserSysNo = @2)
                                                and (@3 = 0 or a.status = @3)
                                                and (@4 is null or exists(select 1 from LgDeliveryItem b where b.NoteSysNo = @4 and b.DeliverySysNo = a.sysno))
                                                and (@5 is null or a.createddate >= @5)
                                                and (@6 is null or a.createddate < @6)
                                                and (@7 = 0 or (select count(1) from LgDeliveryItem c where a.sysno = c.deliverysysno and c.transactionsysno = (select TransactionSysNo from SoOrder d where d.sysno = @7)) > 0)
                                                and (@8 is null or exists(select 1 from syuser where a.createdby = syuser.sysno and syuser.username like @9))
                                                and (@10 is null or exists(select 1 from LgDeliveryType where a.deliverytypesysno = LgDeliveryType.Sysno and LgDeliveryType.IsThirdPartyExpress = @10))
                                                and (@11 is null or exists(select 1 from SyUserWarehouse e  where e.WarehouseSysNo = a.StockSysNo and e.UserSysNo = @12))";

            #endregion

            #region 设置默认参数

            int beginNum = pager.PageSize * (pager.CurrentPage - 1) + 1;
            int endNum = beginNum + pager.PageSize - 1;

            sqlSelect = string.Format(sqlSelect, (int)LogisticsStatus.是否到付.否
                , (int)LogisticsStatus.是否到付.是
                , (int)LogisticsStatus.配送单明细状态.待签收
                , beginNum, endNum);

            #endregion

            using (var context = Context.UseSharedConnection(true))
            {
                #region 设置查询参数

                //获取查询参数
                string createdStartDate = cbLgDelivery.CreatedStartDate;
                string createdEndDdate = cbLgDelivery.CreatedEndDate;
                DateTime start = DateTime.MinValue;
                DateTime end = DateTime.MinValue;
                if (!string.IsNullOrEmpty(createdStartDate))
                {
                    start = DateTime.Parse(createdStartDate);
                }
                if (!string.IsNullOrEmpty(createdEndDdate))
                {
                    end = DateTime.Parse(createdEndDdate).AddDays(1);
                }

                int? isThirdPartyExpress = null;
                if (!string.IsNullOrEmpty(cbLgDelivery.IsThirdPartyExpress))
                {
                    isThirdPartyExpress = int.Parse(cbLgDelivery.IsThirdPartyExpress);
                }

                //查询所有仓库，还是只查询有权限的部分仓库
                string allWarehouse = hasAllWarehouse ? null : "part";
                var param = new object[]
                {
                    cbLgDelivery.SysNo,
                    cbLgDelivery.StockSysNo,
                    cbLgDelivery.DeliveryUserSysNo,
                    cbLgDelivery.Status,
                    cbLgDelivery.StockOutNo,
                    createdStartDate,
                    createdEndDdate,
                    cbLgDelivery.SoOrderSysNo,
                    cbLgDelivery.CreatedName, 
                    "%" + cbLgDelivery.CreatedName + "%",
                    isThirdPartyExpress,
                    allWarehouse,
                    currentUserSysNo
                };

                #endregion

                pager.TotalRows = context.Sql(countSelect).Parameters(param).QuerySingle<int>();
                pager.Rows = context.Sql(sqlSelect).Parameters(param).QueryMany<CBLgDelivery>();
            }
            return pager;
        }

        /// <summary>
        /// 读取配送单明细
        /// </summary>
        /// <param name="deliverySysNo">配送单sys no.</param>
        /// <returns>配送单明细组合实体列表</returns>
        /// <remarks>
        /// 2013-06-19 沈强 创建
        /// </remarks>
        public override IList<CBLgDeliveryItem> GetCBDeliveryItems(int deliverySysNo)
        {
            #region Sql

            const string sql = @"
                select a.*,
                    isnull(b.name,'找不到此人') as name,
                   isnull(b.phonenumber,'找不到电话号码') as phonenumber,
                    isnull(b.Mobilephonenumber,'找不到手机号') as Mobilephonenumber,
                    isnull(b.Streetaddress,'找不到地址') as  Streetaddress
                from  LgDeliveryItem a 
                  left join soreceiveaddress b                            
                    on a.addresssysno = b.sysno 
                where a.DeliverySysNo = @0
                ";

            #endregion

            return Context.Sql(sql, deliverySysNo).QueryMany<CBLgDeliveryItem>();
        }

        /// <summary>
        /// 读取配送单
        /// </summary>
        /// <param name="sysNo">配送单sys no.</param>
        /// <returns>配送单 实体</returns>
        /// <remarks>
        /// 2013-6-17 何方 创建
        /// </remarks>
        public override LgDelivery GetDelivery(int sysNo)
        {
            return Context.Sql(@"select * from LgDelivery  where sysno=@0  ", sysNo).QuerySingle<LgDelivery>();

        }

        /// <summary>
        /// 通过订单号读取配送单列表
        /// </summary>
        /// <param name="orderSysNo">订单号</param>
        /// <returns>配送单列表</returns>
        /// <remarks>
        /// 2013-12-06 余勇 创建
        /// </remarks>
        public override List<CBLgDelivery> GetDeliveryListByOrderSysNo(int orderSysNo)
        {
            return
                Context.Sql(@"select  a.SysNo,a.DeliveryUserSysNo,a.CreatedBy,a.CreatedDate,a.PaidAmount,a.CODAmount,a.Status,a.DeliveryTypeSysNo,
                (select count(1) from LgDeliveryItem where a.sysno = LgDeliveryItem.DeliverySysNo and IsCOD = 0) as PaidNoteCount, 
                (select count(1) from LgDeliveryItem where a.sysno = LgDeliveryItem.DeliverySysNo and IsCOD = 1) as CODNoteCount 
            from lgdelivery a inner join LgDeliveryItem c on  a.sysno = c.deliverysysno 
            inner join SoOrder d on c.transactionsysno=d.transactionsysno
            where d.sysno = @0", orderSysNo).QueryMany<CBLgDelivery>();
        }

        /// <summary>
        /// 获取当天指定的配送单列表
        /// </summary>
        /// <param name="sysNosExcluded">要排除的配送单系统编号集合</param>
        /// <param name="userSysNo">用户系统编号</param>
        /// <returns>当天指定的配送单列表</returns>
        /// <remarks>2013-06-21 黄伟 创建</remarks>
        public override Result<List<CBWCFLgDelivery>> GetDeliveryList(List<int> sysNosExcluded, int userSysNo)
        {

            //var result = new Result<List<CBWCFLgDelivery>>();
            var lst = new List<CBWCFLgDelivery>();

            var strSql =
                @"select * from LgDelivery where status=@status and deliveryusersysno=@deliveryusersysno and Convert(nvarchar(10),createddate,120)=Convert(nvarchar(10),sysdate,120)";
            // and sysno not in (7,8,9)";
            //select * from LgDelivery where to_char(createddate,'yyyy-mm-dd')=to_char(sysdate,'yyyy-mm-dd') and sysno not in(1,2,3)
            var strFilter = new StringBuilder();
            if (sysNosExcluded != null && sysNosExcluded.Count > 0)
            {
                strFilter.Append("(");
                sysNosExcluded.ForEach(p => strFilter.Append(p + ","));
                //remove last ','
                strFilter.Remove(strFilter.Length - 1, 1);
                strFilter.Append(")");
            }

            using (var context = Context.UseSharedConnection(true))
            {

                var lstDelivery = strFilter.Length == 0
                    ? context.Sql(strSql, (int)LogisticsStatus.配送单状态.配送在途, userSysNo)
                        .QueryMany<LgDelivery>()
                    : context.Sql(strSql + " and sysno not in" + strFilter,
                        (int)LogisticsStatus.配送单状态.配送在途, userSysNo)
                    //.Parameter("excluded", strFilter.ToString())
                        .QueryMany<LgDelivery>();

                lstDelivery.ForEach(p =>
                {
                    var lstDeliveryItem =
                        context.Sql("select * from LgDeliveryItem where deliverysysno=@0", p.SysNo)
                            .QueryMany<LgDeliveryItem>();
                    var lstLogisticsDeliveryItem = new List<LogisticsDeliveryItem>();
                    lstDeliveryItem.ForEach(i =>
                    {
                        var person =
                            context.Sql("select * from SoReceiveAddress where sysno=@0", i.AddressSysNo)
                                .QuerySingle<SoReceiveAddress>();
                        var user =
                            context.Sql("select * from LgDeliveryUserLocation where deliveryUsersysno=@0",
                                i.DeliverySysNo).QuerySingle<LgDeliveryUserLocation>();
                        //获取商品
                        var lstProducts = new List<Item>();
                        var stockOutItem = new List<WhStockOutItem>();
                        var lgpickupItem = new List<LgPickUpItem>();
                        int orderSysNo = 0;
                        decimal totalAmount = 0;

                        //出库单
                        if (i.NoteType == 10)
                        {
                            totalAmount = IOutStockDao.Instance.GetEntity(i.NoteSysNo).StockOutAmount;
                            orderSysNo = IOutStockDao.Instance.GetSoOrder(i.NoteSysNo).SysNo;
                            stockOutItem =
                                context.Sql("select * from WhStockOutItem where stockoutsysno=@0 and status=1",
                                    i.NoteSysNo)
                                    .QueryMany<WhStockOutItem>();
                            stockOutItem.ForEach(e => lstProducts.Add(new Item
                            {
                                ProductSysNo = e.ProductSysNo,
                                ProductName = e.ProductName,
                                OriginalPrice = double.Parse(e.OriginalPrice + ""),
                                Quantity = e.ProductQuantity,
                                OrderItemSysNo = e.OrderItemSysNo
                            }));

                        }
                        else
                        {
                            lgpickupItem =
                                context.Sql("select * from lgpickupItem where pickupsysno=@0", i.NoteSysNo)
                                    .QueryMany<LgPickUpItem>();
                            lgpickupItem.ForEach(e => lstProducts.Add(new Item
                            {
                                ProductSysNo = e.ProductSysNo,
                                ProductName = e.ProductName,
                                OriginalPrice =
                                    context.Sql("select * from pdprice where productsysno=@0", e.PickUpSysNo)
                                        .QuerySingle<double>(),
                                Quantity = e.ProductQuantity

                            }));
                        }

                        lstLogisticsDeliveryItem.Add(new LogisticsDeliveryItem
                        {
                            SysNo = i.SysNo,
                            NoteType = i.NoteType, //== 10 ? "出库单" : "取件单",
                            NoteSysNo = i.NoteSysNo,
                            AddrName = person == null ? null : person.Name,
                            StreetAddress = person == null ? null : person.StreetAddress,
                            MobilePhoneNumber = person == null ? null : person.MobilePhoneNumber,
                            PhoneNumber = person == null ? null : person.PhoneNumber,
                            Longitude = user == null ? 0 : user.Longitude,
                            Latitude = user == null ? 0 : user.Latitude,
                            //状态：待签收（10）、拒收（20）、未送达（30）、已签
                            Status = i.Status,
                            // == 10 ? "待签收" : (i.Status == 20 ? "拒收" : (i.Status == 30 ? "未送达" : "已签")),
                            PaymentType = i.PaymentType,
                            PaymentAmount = double.Parse(i.Receivable + ""), //支付金额
                            Items = lstProducts,
                            OrderSysNo = orderSysNo,
                            TotalAmount = totalAmount
                        });
                    });

                    lst.Add(new CBWCFLgDelivery
                    {
                        SysNo = p.SysNo,
                        NoteNum =
                            context.Sql("select count(sysno) from LgDeliveryItem where deliverysysno=@0",
                                p.SysNo)
                                .QuerySingle<int>(),
                        ShipAmount = double.Parse(p.CODAmount + ""),
                        ShipTime = p.CreatedDate, //创建日期
                        LogisticsDeliveryItems = lstLogisticsDeliveryItem
                    });

                });
                //LgDelivery for each end
            }
            var obj = new Result<List<CBWCFLgDelivery>>
            {
                Message = "1",
                Status = true,
                StatusCode = 0,
                Data = lst
            };

            return obj;
        }

        /// <summary>
        /// 根据配送单编号获取配送单明细
        /// </summary>
        /// <param name="sysNo">配送单系统编号</param>
        /// <returns>配送单明细列表</returns>
        /// <remarks>2013-12-30 黄伟 创建</remarks>
        public override List<LgDeliveryItem> GetItemListByDeliverySysNo(int sysNo)
        {

            var lstDeliveryItem =
                Context.Sql("select * from LgDeliveryItem where deliverysysno=@0", sysNo)
                    .QueryMany<LgDeliveryItem>();

            return lstDeliveryItem;
        }

        /// <summary>
        /// 获取配送人员所有配送单(配送在途)列表
        /// </summary>
        /// <param name="userSysNo">用户系统编号</param>
        /// <returns>配送单集合</returns>
        /// <remarks>
        /// 2013-12-30 黄伟 创建
        /// </remarks>   
        public override List<CBWCFLgDelivery> GetLgDeliveryListAll(int userSysNo)
        {
            var lst = new List<CBWCFLgDelivery>();
            //var strSql = "select * from LgDelivery where status!=:status and deliveryusersysno=:deliveryusersysno and to_char(createddate,'yyyy-mm-dd')=to_char(sysdate,'yyyy-mm-dd')";
            var strSql = "select * from LgDelivery where status=@status and deliveryusersysno=@deliveryusersysno";

            using (var context = Context.UseSharedConnection(true))
            {

                var lstDelivery = context.Sql(strSql, (int)LogisticsStatus.配送单状态.配送在途, userSysNo)
                    .QueryMany<LgDelivery>();
                //排除补单
                var lstDeliveryFilterd = new List<LgDelivery>();

                #region 补单filter

                lstDelivery.ForEach(p =>
                {
                    var lstDelItems = GetDeliveryItems(p.SysNo);

                    foreach (var lgDeliveryItem in lstDelItems)
                    {

                        if (lgDeliveryItem.NoteType == 10)
                        {
                            var stockOut = IOutStockDao.Instance.GetModel(lgDeliveryItem.NoteSysNo);
                            if (ISoOrderDao.Instance.GetEntity(stockOut.OrderSysNO).OrderSource ==
                                OrderStatus.销售单来源.业务员补单.GetHashCode())
                            {
                                continue;
                            }
                            if (!lstDeliveryFilterd.Contains(p))
                            {
                                lstDeliveryFilterd.Add(p);
                            }
                        }
                    }

                });

                #endregion


                lstDeliveryFilterd.ForEach(p => lst.Add(new CBWCFLgDelivery
                {
                    SysNo = p.SysNo,
                    NoteNum =
                        context.Sql("select count(sysno) from LgDeliveryItem where deliverysysno=@0",
                            p.SysNo)
                            .QuerySingle<int>(),
                    ShipAmount = double.Parse(p.CODAmount + ""),
                    ShipTime = p.CreatedDate, //创建日期
                    Status = p.Status
                }));
                //LgDelivery for each end
            }

            return lst;
        }

        /// <summary>
        /// 更新单据状态--------------------------------------------------------------------------
        /// </summary>
        /// <param name="lstStatus">需要更新的单据集合</param>
        /// <param name="user">当前登录用户</param>
        /// <returns>封装的响应实体(状态,状态编码,消息)</returns>
        /// <remarks>2013-06-24 黄伟 创建</remarks>
        /// <remarks>2014-01-14 沈强 修改</remarks>
        public override Result UpdateStatus(List<CBWCFStatusUpdate> lstStatus, SyUser user)
        {
            #region 之前的代码

            /*出库单签收60,出库单未能送达(待配送 = 40),出库单拒收70,取件单取货
             * i.NoteType == 10 ? "出库单" : "取件单"
            public enum 出库单状态
            {
                待出库 = 10,
                待拣货 = 20,
                待打包 = 30,
                待配送 = 40,
                配送中 = 50,
                已签收 = 60,
                拒收 = 70,
                部分退货 = 80,
                全部退货 = 90,
                作废 = -10,
            }
            */
            //if (lstStatus == null || !lstStatus.Any())
            //    return new Result
            //        {
            //            StatusCode = 0,
            //            Status = true,
            //            Message = "Nothing to update."
            //        };

            //var success = true;
            //do update
            //using (var context = Context.UseTransaction(true))
            //{
            //    try
            //    {
            //        lstStatus.ForEach(p =>
            //            {
            //                //出库单
            //                if (p.NoteType == 10)
            //                {
            //                    context.Sql(@"update WhStockOut set Status=:status where sysno=:sysno")
            //                           .Parameter("status", p.Status)
            //                           .Parameter("sysno", p.NoteSysNo)
            //                           .Execute();
            //                }
            //                //取件单
            //                else if (p.NoteType == 20)
            //                {
            //                    context.Sql(@"update LgPickUp set Status=:status where sysno=:sysno")
            //                           .Parameter("status", p.Status)
            //                           .Parameter("sysno", p.NoteSysNo)
            //                           .Execute();
            //                }

            //            });

            //        context.Commit();
            //    }
            //    catch 
            //    {
            //        context.Rollback();
            //        success = false;
            //    }

            //}

            //return new Result
            //    {
            //        Status = success,
            //        StatusCode = success == true ? 0 : -1,
            //        Message = success == true ? "Success" : "Error occurred during update,opertion cancelled."
            //    };

            #endregion

            if (lstStatus == null || !lstStatus.Any())
                return new Result
                {
                    Message = "Nothing to update."
                };

            using (var context = Context.UseSharedConnection(true))
            {
                lstStatus.ForEach(l =>
                {
                    var status = new LgAppSignStatus
                    {
                        CreatedBy = user.SysNo,
                        CreatedDate = DateTime.Now,
                        DeliverySysNo = l.DeliverySysNo,
                        NoteSysNo = l.NoteSysNo,
                        NoteType = l.NoteType,
                        Status = l.Status
                    };

                    var id =
                        context.Insert<LgAppSignStatus>("LgAppSignStatus", status)
                            .AutoMap(m => m.SysNo)
                            .ExecuteReturnLastId<int>("SysNo");

                    l.CbwcfStatusItems.ForEach(c =>
                    {
                        var item = new LgAppSignItem()
                        {
                            AppSignStatusSysNo = id,
                            NoteItemSysNo = c.NoteItemSysNo,
                            SignQuantity = c.SignQuantity
                        };
                        context.Insert<LgAppSignItem>("LgAppSignItem", item)
                            .AutoMap(m => m.SysNo).Execute();
                    });
                });
            }
            return new Result
            {
                Status = true,
                Message = "Success"
            };

        }

        /// <summary>
        /// App签收状态记录数
        /// </summary>
        /// <param name="deliverySysNo">配送单系统编号</param>
        /// <param name="noteSysNo">单据编号(出库单、取货单)</param>
        /// <returns>App签收状态记录数</returns>
        /// <remarks>2014-08-24 周唐炬 创建</remarks>
        public override int GetAppSignCount(int deliverySysNo, int noteSysNo)
        {
            return Context.Sql(
                "select count(1) from LgAppSignStatus where DeliverySysNo=@DeliverySysNo and NoteSysNo=@NoteSysNo")
                .Parameter("DeliverySysNo", deliverySysNo)
                .Parameter("NoteSysNo", noteSysNo)
                .QuerySingle<int>();
        }

        /// <summary>
        /// 读取配送单明细
        /// </summary>
        /// <param name="deliverySysNo">配送单sys no.</param>
        /// <returns>配送单列表列表</returns>
        /// <remarks>
        /// 2013-6-17 何方 创建
        /// </remarks>
        public override IList<LgDeliveryItem> GetDeliveryItems(int deliverySysNo)
        {
            return
                Context.Sql(@"select * from lgdeliveryitem where deliverysysno=@0  ", deliverySysNo)
                    .QueryMany<LgDeliveryItem>();
        }

        /// <summary>
        /// 读取单个配送单明细
        /// </summary>
        /// <param name="deliveryItemSysNo">配送单明细系统编号.</param>
        /// <returns>配送单明细实体</returns>
        /// <remarks>
        /// 2013-06-17 何方 创建
        /// </remarks>
        public override LgDeliveryItem GetDeliveryItem(int deliveryItemSysNo)
        {
            return
                Context.Sql(@"select * from LgDeliveryItem	where sysno=@0  ", deliveryItemSysNo)
                    .QuerySingle<LgDeliveryItem>();
        }

        /// <summary>
        /// 读取单个配送单明细
        /// </summary>
        /// <param name="deliverySysNo">配送单系统编号.</param>
        /// <param name="noteSysNo">配送单明细单据编号.</param>
        /// <param name="noteType">配送单明细单据类型.</param>
        /// <returns>
        /// 配送单明细实体
        /// </returns>
        /// <remarks>
        /// 2013-08-20 何方 创建
        /// </remarks>
        public override LgDeliveryItem GetDeliveryItem(int deliverySysNo, int noteSysNo, LogisticsStatus.配送单据类型 noteType)
        {
            return
                Context.Sql(@"select * from LgDeliveryItem where notetype=@0 and notesysno=@1  and deliverySysNo=@2 ",
                    (int)noteType, noteSysNo, deliverySysNo).QuerySingle<LgDeliveryItem>();
        }

        /// <summary>
        /// 读取单个配送单明细
        /// </summary>
        /// <param name="noteSysNo">配送明细单据系统编号.</param>
        /// <param name="noteType">配送明细单据类型.</param>
        /// <returns>
        /// 配送单明细实体
        /// </returns>
        /// <remarks>
        /// 2016-08-02 杨浩 创建
        /// </remarks>
        public override LgDeliveryItem GetDeliveryItem(int noteSysNo, LogisticsStatus.配送单据类型 noteType)
        {
            return
             Context.Sql(@"select top 1 * from LgDeliveryItem where notetype=@0 and notesysno=@1 and Status>0",
                 (int)noteType, noteSysNo).QuerySingle<LgDeliveryItem>();
        }

        /// <summary>
        /// 向配送单添加明细
        /// </summary>
        /// <param name="items">配送单明细集合</param>
        /// <returns>
        /// 添加成功：1；添加失败：0
        /// </returns>
        /// <remarks>
        /// 2013-06-18 何方 创建
        /// 2013-06-26 沈强 修改
        /// </remarks>
        public override int AddDeliveryItems(IList<LgDeliveryItem> items)
        {
            int i = 0;
            using (var trans = Context.UseTransaction(true))
            {
                foreach (var item in items)
                {
                    trans.Insert<LgDeliveryItem>("LgDeliveryItem", item)
                        .AutoMap(x => x.SysNo)
                        .Execute();
                }
                trans.Commit();
                i = 1;
            }
            return i;
        }

        /// <summary>
        /// 更新配送单状态
        /// </summary>
        /// <param name="deliverySysNo">配送单系统编号.</param>
        /// <param name="status">LogisticsStatus.配送单状态</param>
        /// <returns>
        /// 作废结果
        /// </returns>
        /// <remarks>
        /// 2013-06-18 何方 创建
        /// 2013-06-27 沈强 修改
        /// </remarks>
        public override bool UpdateStatus(int deliverySysNo, LogisticsStatus.配送单状态 status)
        {
            int result = Context.Update("lgdelivery")
                .Column("status", (int)status)
                .Where("sysno", deliverySysNo)
                .Execute();
            return result > 0;
        }

        /// <summary>
        /// 向配送单添加明细
        /// </summary>
        /// <param name="itemSysNos">配送单明细系统编号.</param>
        /// <returns>
        /// 删除结果
        /// </returns>
        /// <remarks>
        /// 2013-06-18 何方 创建
        /// </remarks>
        public override int RemoveDeliveryItems(int[] itemSysNos)
        {
            var i = 0;
            foreach (var sysNo in itemSysNos)
            {
                int del = Context.Delete("lgdelivery")
                    .Where("sysno", sysNo).Execute();
                ;
                i += del;
            }
            return i;
        }

        /// <summary>
        /// </summary>
        /// <param name="deliveryItemSysNo">配送单明细系统编号..</param>
        /// <param name="status">配送单明细状态.</param>
        /// <returns>返回操作状态</returns>
        /// <remarks>
        /// 2013-6-18 何方 创建
        /// </remarks>
        public override bool UpdateDeliveryItemStatus(int deliveryItemSysNo, LogisticsStatus.配送单明细状态 status)
        {
            int updateCount = Context.Update("lgdeliveryItem")
                .Column("status", (int)status)
                .Where("sysno", deliveryItemSysNo)
                .Execute();
            return updateCount == 1;
        }

        /// <summary>
        /// 更新单个配送单明细状态
        /// </summary>
        /// <param name="deliverySysNo">配送单系统编号.</param>
        /// <param name="noteType">配送单明细单据类型.</param>
        /// <param name="noteSysNo">配送单明细单据系统编号.</param>
        /// <param name="status">配送单明细状态.</param>
        /// <param name="operatorSysNo">操作人系统编号</param>
        /// <returns>返回操作状态</returns>
        /// <remarks>
        /// 2013-07-11 何方 创建
        /// </remarks>
        public override bool UpdateDeliveryItemStatus(int deliverySysNo, LogisticsStatus.配送单据类型 noteType, int noteSysNo,
            LogisticsStatus.配送单明细状态 status, int operatorSysNo = 0)
        {
            int updateCount = Context.Update("lgdeliveryItem")
                .Column("status", (int)status)
                .Column("LastUpdateBy", operatorSysNo)
                .Column("LastUpdateDate", DateTime.Now)
                .Where("deliverySysNo", deliverySysNo)
                .Where("noteType", (int)noteType)
                .Where("noteSysNo", noteSysNo)

                .Execute();
            return updateCount == 1;
        }

        /// <summary>
        /// 读取配送单明细
        /// </summary>
        /// <param name="syUserSysNo">用户系统编号.</param>
        /// <param name="status">配送单明细状态数组.</param>
        /// <returns>
        /// 配送单明细实体列表
        /// </returns>
        /// <remarks>
        /// 2013-06-17 何方 创建
        /// </remarks>
        public override IList<LgDeliveryItem> GetDeliveryItems(int syUserSysNo, int[] status)
        {
            return Context.Sql("select * from lgdeliveryitem  ").QueryMany<LgDeliveryItem>();
        }

        /// <summary>
        /// 更新单个配送单明细
        /// </summary>
        /// <param name="lgDeliveryItem">配送单明细实体</param>
        /// <returns>返回操作状态</returns>
        /// <remarks>
        /// 2013-06-20 沈强 创建
        /// </remarks>
        public override bool UpdateDeliveryItem(LgDeliveryItem lgDeliveryItem)
        {
            int result = Context.Update<LgDeliveryItem>("LgDeliveryItem", lgDeliveryItem)
                .AutoMap(x => x.SysNo)
                .Where(x => x.SysNo)
                .Execute();
            return result > 0;
        }

        /// <summary>
        /// 根据指定仓库系统编号和配送员系统编号获取配送单系统编号
        /// </summary>
        /// <param name="warehouseSysno">仓库系统编号</param>
        /// <param name="userSysNo">配送员系统编号</param>
        /// <param name="status">配送单状态</param>
        /// <returns>没有返回0；有则返回当前配送单系统编号</returns>
        /// <remarks>
        /// 2013-06-25 沈强 创建
        /// </remarks>
        public override int GetDeliveryNoteSysNo(int warehouseSysno, int userSysNo, LogisticsStatus.配送单状态 status)
        {
            #region sql

            var sql = @"select isnull(min(sysno),0) from  LgDelivery a 
                        where StockSysNo = @0 and DeliveryUserSysNo = @1 and Status = @2";

            #endregion

            return Context.Sql(sql, warehouseSysno, userSysNo, (int)status)
                .QuerySingle<int>();
        }

        /// <summary>
        /// 指定类型的单据是否在其他有效配送单明细中
        /// </summary>
        /// <param name="deliverSysNo">配送单系统编号</param>
        /// <param name="noteType">单据类型</param>
        /// <param name="noteNumber">单据编号</param>
        /// <returns>在有效配送单明细中：true；否则：false</returns>
        /// <remarks>
        /// 2013-06-25 沈强 创建
        /// </remarks>
        public override bool IsNoteInDeliveryItem(int deliverSysNo, LogisticsStatus.配送单据类型 noteType, int noteNumber)
        {
            int result =
                Context.Sql(
                    @"select count(1) from  LgDeliveryItem a  where sysno != @0 and NoteType = @1 and NoteSysNo = @2",
                    deliverSysNo, (int)noteType, noteNumber)
                    .QuerySingle<int>();

            return result > 0;
        }

        /// <summary>
        /// 指定类型的单据是否在其他有效配送单明细中
        /// </summary>
        /// <param name="noteType">单据类型</param>
        /// <param name="noteNumber">单据编号</param>
        /// <returns>在有效配送单明细中：true；否则：false</returns>
        /// <remarks>
        /// 2013-06-25 沈强 创建
        /// </remarks>
        public override bool IsNoteInDeliveryItem(LogisticsStatus.配送单据类型 noteType, int noteNumber)
        {
            var result = Context.Sql(@"select count(1) from LgDelivery d 
                  left join  LgDeliveryItem a  on d.sysno=a.deliverysysno
                    where d.status!=@0 and a.NoteType = @1 and a.NoteSysNo = @2",
                (int)Model.WorkflowStatus.LogisticsStatus.配送单状态.作废,
                (int)noteType, noteNumber)
                .QuerySingle<int>();

            return result > 0;
        }

        /// <summary>
        /// 更新指定配送单状态与预付、到付金额
        /// </summary>
        /// <param name="deliverSysNo">配送单系统编号</param>
        /// <param name="deliveryStatus">配送单状态</param>
        /// <param name="isForce">是否强制发货（true：是；false：否）</param>
        /// <returns>成功：true；失败：false</returns>
        /// <remarks>
        /// 2013-06-27 沈强 创建
        /// </remarks>
        public override bool UpdateDeliveryStatus(int deliverSysNo, LogisticsStatus.配送单状态 deliveryStatus, bool isForce)
        {
            #region updateSql

            string sql = @"update lgdelivery a
                               set 
                                   paidamount = (select isnull(sum(b.stockoutamount),0) from LgDeliveryItem b where b.deliverysysno = a.sysno and b.iscod = 0),
                                   codamount = (select isnull(sum(c.stockoutamount),0) from LgDeliveryItem c where c.deliverysysno = a.sysno and c.iscod = 1),
                                   isenforceallow = @0,
                                   status = @1
                             where sysno = @2";

            #endregion

            int tmpIsForce = isForce ? 1 : 0;

            int result = Context.Sql(sql, tmpIsForce, (int)deliveryStatus, deliverSysNo).Execute();

            return result > 0;
        }

        /// <summary>
        /// 获取配送单中，指定配送明细状态的数量
        /// </summary>
        /// <param name="deliverySysNo">配送单系统编号</param>
        /// <param name="deliveryItemStatus">配送单明细状态</param>
        /// <returns>返回指定状态数量</returns>
        /// <remarks>
        /// 2013-06-27 沈强 创建
        /// </remarks>
        public override int GetDeliveryItemStatusCount(int deliverySysNo, LogisticsStatus.配送单明细状态 deliveryItemStatus)
        {
            #region selectSql

            string sql = "select count(1) from LgDeliveryItem a where a.status = @0 and a.deliverysysno = @1";

            #endregion

            return Context.Sql(sql, (int)deliveryItemStatus, deliverySysNo).QuerySingle<int>();
        }

        /// <summary>
        /// 获取配送单中，指定配送明细状态的配送金额
        /// </summary>
        /// <param name="deliverySysNo">配送单系统编号</param>
        /// <param name="deliveryItemStatus">配送单明细状态</param>
        /// <returns>指定状态的配送金额</returns>
        /// <remarks>
        /// 2013-06-27 沈强 创建
        /// </remarks>
        public override decimal GetDeliveryAmount(int deliverySysNo, LogisticsStatus.配送单明细状态 deliveryItemStatus)
        {
            #region selectSql

            string sql =
                "select isnull(sum(a.stockoutamount),0) from LgDeliveryItem a where a.status = @0 and a.deliverysysno = @1";

            #endregion

            return Context.Sql(sql, (int)deliveryItemStatus, deliverySysNo).QuerySingle<decimal>();
        }

        /// <summary>
        /// 根据指定系统编号删除配送单明细
        /// </summary>
        /// <param name="deliveryItemSysNo">配送单明细系统编号</param>
        /// <returns>成功:true; 失败:false</returns>
        /// <remarks>
        /// 2013-06-27 沈强 创建
        /// </remarks> 
        public override bool DeleteDeliveryItem(int deliveryItemSysNo)
        {
            int result = Context.Delete("LgDeliveryItem").Where("SysNo", deliveryItemSysNo).Execute();
            return result > 0;
        }

        /// <summary>
        /// 检查一组配送单中，配送员是否相同
        /// </summary>
        /// <param name="deliverySysNos">配送单系统编号数组</param>
        /// <returns>相同：true；不同：false</returns>
        /// <remarks>
        /// 2013-06-28 沈强 创建
        /// </remarks>  
        public override bool CheckDeliveryUserIsRepeat(int[] deliverySysNos)
        {
            //保存配送单系统编号字符串
            string sysnoString = string.Empty;

            #region 将配送单系统编号，组装成字符串

            StringBuilder stringBuilder = new StringBuilder();
            foreach (var item in deliverySysNos)
            {
                stringBuilder.Append("," + item);
            }
            sysnoString = stringBuilder.ToString().Substring(1);

            #endregion

            #region check Sql 检测用sql

            string sql = @"select count(distinct deliveryusersysno) from LgDelivery a 
                           where exists(select 1 from splitstr(@sysnoString,',') b where b.col = a.sysno)";

            #endregion

            int result = Context.Sql(sql).Parameter("sysnoString", sysnoString).QuerySingle<int>();

            return result == 1;
        }

        /// <summary>
        /// 根据配送单系统编号数组，获取配送单集合
        /// </summary>
        /// <param name="deliverySysNo">配送单系统编号数组</param>
        /// <returns>配送单集合</returns>
        /// <remarks>
        /// 2013-07-04 沈强 创建
        /// </remarks>   
        public override IList<LgDelivery> GetLgDeliveryList(int[] deliverySysNo)
        {
            string sysnos = string.Join(",", deliverySysNo);

            string sql = @"select * from lgdelivery a where exists
                            (
                                select 1 from splitstr(@0,',') b where b.col = a.sysno
                            )";

            return Context.Sql(sql, sysnos).QueryMany<LgDelivery>();
        }

        /// <summary>
        /// 根据配送单系统编号数组，获取配送单明细集合
        /// </summary>
        /// <param name="deliverySysNo">配送单系统编号数组</param>
        /// <returns>配送单明细集合</returns>
        /// <remarks>
        /// 2013-07-04 沈强 创建
        /// </remarks>   
        public override IList<LgDeliveryItem> GetLgDeliveryItemList(int[] deliverySysNo)
        {
            string sysnos = string.Join(",", deliverySysNo);

            string sql = @"select * from LgDeliveryItem a where exists
                            (
                                select 1 from splitstr(@0,',') b where b.col = a.deliverysysno
                            )";

            return Context.Sql(sql, sysnos).QueryMany<LgDeliveryItem>();
        }

        /// <summary>
        /// 向配送单添加明细
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>返回系统编号</returns>
        /// <remarks>
        /// 2013-07-06 何方 创建
        /// </remarks>
        public override int AddDeliveryItem(LgDeliveryItem item)
        {
            if (item.LastUpdateDate == DateTime.MinValue)
            {
                item.LastUpdateDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            var sysno = Context.Insert<LgDeliveryItem>("LgDeliveryItem", item)
                .AutoMap(x => x.SysNo)
                .ExecuteReturnLastId<int>("SysNo");
            return sysno;
        }

        /// <summary>
        /// 更新配送单
        /// </summary>
        /// <param name="model">配送单实体</param>
        /// <returns>返回操作状态</returns>
        /// <remarks>
        /// 2013-7-14 何方 创建
        /// </remarks>
        public override bool Update(LgDelivery model)
        {
            var rowsAffected = Context.Update<LgDelivery>("LgDelivery", model)
                .AutoMap(x => x.SysNo, x => x.LgDeliveryItemList)
                .Where(x => x.SysNo)
                .Execute();
            return rowsAffected == 1;
        }

        /// <summary>
        /// 根据配送单据类型和单据编号，获取配送单为非作废状态的明细集合
        /// </summary>
        /// <param name="noteType">配送单据类型</param>
        /// <param name="noteSysNo">单据编号</param>
        /// <returns>配送单明细集合</returns>
        /// <remarks>
        /// 2013-07-30 沈强 创建
        /// </remarks>  
        public override IList<LgDeliveryItem> GetLgDeliveryItemList(LogisticsStatus.配送单据类型 noteType, int noteSysNo)
        {
            string sql = @"select a.* from LgDelivery d 
                  left join  LgDeliveryItem a  on d.sysno=a.deliverysysno
                    where d.status!=@0 and a.NoteType = @1 and a.NoteSysNo = @2";

            return
                Context.Sql(sql, (int)LogisticsStatus.配送单状态.作废, (int)noteType, noteSysNo).QueryMany<LgDeliveryItem>();
        }

        /// <summary>
        /// 根据用户编号来获取配送中的配送单数量
        /// </summary>
        /// <param name="customerSysNo">用户编号</param>
        /// <returns>配送中的配送单数量</returns>
        /// <remarks>2013-08-27 周瑜 创建</remarks>
        public override int GetDeliveryingCount(int customerSysNo)
        {
            const string sql =
                @"select count(distinct a.deliverysysno) from LgDeliveryItem a
inner join CrReceiveAddress b
on a.addresssysno = b.areasysno
where a.status in (10,30)
and b.customersysno= @customersysno";
            return Context.Sql(sql)
                .Parameter("customersysno", customerSysNo)
                .QuerySingle<int>();
        }

        /// <summary>
        /// 通过事务编号获取配送单列表
        /// </summary>
        /// <param name="transactionSysNo">事务编号</param>
        /// <returns>配送单</returns>
        /// <remarks>2013-09-18 周唐炬 创建</remarks>
        public override IList<LgDelivery> GetLgDeliveryList(string transactionSysNo)
        {
            const string sql =
                @"SELECT A.* FROM LgDelivery A INNER JOIN LgDeliveryItem B ON B.DELIVERYSYSNO=A.SYSNO AND B.TRANSACTIONSYSNO=@TransactionSysNo";
            return Context.Sql(sql).Parameter("TransactionSysNo", transactionSysNo).QueryMany<LgDelivery>();
        }

        /// <summary>
        /// 通过来源单据类型跟编号获取配送单
        /// </summary>
        /// <param name="noteType">单据类型</param>
        /// <param name="noteNumber">单据编号</param>
        /// <returns>配送单</returns>
        /// <remarks>2013-09-18 周唐炬 创建</remarks>
        public override LgDelivery GetDelivery(LogisticsStatus.配送单据类型 noteType, int noteNumber)
        {
            const string sql =
                @"SELECT d.* FROM LgDelivery d LEFT JOIN  LgDeliveryItem a ON d.sysno=a.deliverysysno WHERE d.status!=@0 AND a.NoteType = @1 AND a.NoteSysNo = @2";
            return
                Context.Sql(sql, (int)LogisticsStatus.配送单状态.作废, (int)noteType, noteNumber).QuerySingle<LgDelivery>();
        }


        /// <summary>
        /// 判断快递单号是否已经被使用
        /// </summary>
        /// <param name="deliverySysNo">快递方式</param>
        /// <param name="express_no">快递单号</param>
        /// <returns>是否存在</returns>
        /// <remarks>2014-04-14 朱成果 创建</remarks>
        public override bool IsExistsExpressNo(int deliverySysNo, string express_no)
        {
            const string sql = @"select count(0)
from  LgDeliveryItem   t1
inner join LgDelivery t2 on t1.deliverysysno=t2.sysno
where t1.ExpressNo=@ExpressNo and t2.Status<>@Status and t1.NoteType=@NoteType and t2.DeliveryTypeSysNo=@DeliverySysNo";
            return
                Context.Sql(sql)
                    .Parameter("ExpressNo", express_no)
                    .Parameter("Status", (int)LogisticsStatus.配送单状态.作废)
                    .Parameter("NoteType", (int)LogisticsStatus.配送单据类型.出库单)
                    .Parameter("DeliverySysNo", deliverySysNo)
                    .QuerySingle<int>() > 0;

        }

        #region 第三方快递配送单

        /// <summary>
        /// 创建第三方快递配送单
        /// </summary>
        /// <param name="model">rp_第三方快递发货量</param>
        /// <returns>返回系统编号</returns>
        /// <remarks>
        /// 2014-09-23 余勇 创建
        /// </remarks>
        public override void CreateExpressLgDelivery(rp_第三方快递发货量 model)
        {
            Context.Insert("rp_第三方快递发货量")
                .Column("StockSysNo", model.StockSysNo)
                .Column("StockName", model.StockName)
                .Column("ExpressNo", model.ExpressNo)
                .Column("Remarks", model.Remarks)
                .Column("CreateDate", model.CreateDate)
                .Column("CompanyName", model.CompanyName).Execute();
        }

        #endregion




    }
}
