using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.DataAccess.Logistics;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Util;
namespace Hyt.DataAccess.Oracle.Logistics
{
    /// <summary>
    /// 取件单数据访问类
    /// </summary>
    /// <remarks>
    /// 2013-07-05 郑荣华 创建
    /// </remarks>
    public class LgPickUpDaoImpl : ILgPickUpDao
    {
        /// <summary>
        /// 查询取件单
        /// </summary>
        /// <param name="filter">查询条件实体</param>
        /// <returns>返回取件单列表</returns>
        /// <remarks>2013-08-12 周唐炬 创建</remarks>
        public override Pager<LgPickUp> GetPickUpList(ParaPickUpFilter filter)
        {
            #region 原调试SQL
            //(SELECT A.*,
            //                               B.WAREHOUSENAME AS WarehouseName,
            //                               C.Streetaddress as StreetAddress,
            //                               D.PICKUPTYPENAME AS PiceupTypeName
            //                          FROM LGPICKUP A
            //                          LEFT JOIN WhWarehouse B
            //                            ON B.SysNo = A.WarehouseSysNo
            //                          LEFT JOIN SORECEIVEADDRESS C
            //                            ON C.SYSNO = A.PICKUPADDRESSSYSNO
            //                          LEFT JOIN LgPickupType D
            //                            ON D.SYSNO=A.PICKUPTYPESYSNO
            //                        WHERE 1=1
            //                            AND (:WarehouseSysNoList IS NULL OR EXISTS
            //                                    (SELECT 1
            //                                        FROM table(splitstr(:WarehouseSysNoList, ',')) tmp
            //                                    WHERE tmp.column_value = A.WarehouseSysNo))
            //                            AND (:Status IS NULL OR A.Status=:Status)
            //                            AND (:PickupTypeSysNo IS NULL OR A.PickupTypeSysNo=:PickupTypeSysNo)
            //                       ) tb
            #endregion
            const string sql = @"(SELECT A.*
                                      FROM LGPICKUP A                                     
                                    WHERE 1=1
                                        AND (@0 IS NULL OR EXISTS
                                                (SELECT 1
                                                    FROM splitstr(@0, ',') tmp
                                                WHERE tmp.col = A.WarehouseSysNo))
                                        AND (@1 IS NULL OR A.Status=@1)
                                        AND (@2 IS NULL OR A.PickupTypeSysNo=@2)
                                        AND (@3 IS NULL OR A.SysNo=@3 OR A.StockInSysNo=@3)
                                   ) tb";
            var dataList = Context.Select<LgPickUp>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(1)").From(sql);
            var warehouseSysNoList = string.Empty;
            if (null != filter.WarehouseSysNoList)
            {
                warehouseSysNoList = string.Join(",", filter.WarehouseSysNoList);
            }
            var paras = new object[]
                {
                    warehouseSysNoList,
                    filter.Status,
                    filter.PickupTypeSysNo,
                    filter.SysNo
                };
            dataList.Parameters(paras);
            dataCount.Parameters(paras);

            var pager = new Pager<LgPickUp>
            {
                Rows = dataList.OrderBy(@"tb.SysNo desc").Paging(filter.CurrentPage, filter.PageSize).QueryMany(),
                TotalRows = dataCount.QuerySingle()
            };

            return pager;
        }

        /// <summary>
        /// 查询取件单
        /// </summary>
        /// <param name="pager">分页对象</param>
        /// <param name="filter">查询条件实体</param>
        /// <remarks>
        /// 2013-07-05 郑荣华 创建
        /// </remarks>
        public override void GetPickUpList(ref Pager<CBLgPickUp> pager, ParaPickUpFilter filter)
        {
            using (var context = Context.UseSharedConnection(true))
            {
                #region sqlcount

                const string sqlCount = @"
                                select count(1) from lgpickup t
                                where (@warehousesysno is null or warehousesysno=@warehousesysno)
                                and (@status is null or status=@status)
                                and (@SysNoFilter is null or not exists(select 1 from splitstr(@SysNoFilter,',') z where z.col = t.sysno))                       
                                ";

                #endregion

                pager.TotalRows = context.Sql(sqlCount)
                                         .Parameter("warehousesysno", filter.WarehouseSysNo)
                                         .Parameter("status", filter.Status)
                                         .Parameter("SysNoFilter", filter.SysNoFilter)
                                         .QuerySingle<int>();

                const string sqlFrom = @"
                                lgpickup t left join Soreceiveaddress b
                                on t.pickupAddressSysNo=b.sysno
                                ";

                const string sqlWhere = @"
                                (@warehousesysno is null or t.warehousesysno=@warehousesysno)
                                and (@status is null or t.status=@status)
                                and (@SysNoFilter is null or not exists(select 1 from splitstr(@SysNoFilter,',') z where z.col = t.sysno))                  
                                ";

                pager.Rows = context.Select<CBLgPickUp>("t.*,b.mobilephonenumber,b.streetaddress,b.phoneNumber")
                                    .From(sqlFrom)
                                    .Where(sqlWhere)
                                    .Parameter("warehousesysno", filter.WarehouseSysNo)
                                    .Parameter("status", filter.Status)
                                    .Parameter("SysNoFilter", filter.SysNoFilter)
                                    .OrderBy("t.sysno desc")
                                    .Paging(pager.CurrentPage, pager.PageSize)
                                    .QueryMany();
            }

        }

        /// <summary>
        /// 获取取件单
        /// </summary>
        /// <param name="pickUpSysNo">The pick up sys no.</param>
        /// <returns>取件单</returns>
        /// <remarks>
        /// 2013/7/6 何方 创建
        /// </remarks>
        public override LgPickUp GetPickUp(int pickUpSysNo)
        {
            return Context.Sql(@"select * from LgPickUp  where sysno=@0  ", pickUpSysNo).QuerySingle<LgPickUp>();
        }

        /// <summary>
        /// 获取取件单商品项
        /// </summary>
        /// <param name="pickUpSysNo">取件单系统编号</param>
        /// <returns>取件单商品项</returns>
        /// <remarks>2013-08-13 周唐炬 创建</remarks>
        public override List<LgPickUpItem> GetLgPickUpItem(int pickUpSysNo)
        {
            return Context.Sql(@"SELECT * FROM LgPickUpItem WHERE PickUpSysNo=@0", pickUpSysNo).QueryMany<LgPickUpItem>();
        }

        public override IList<LgPickUp> GetPickUp(int[] pickUpSysNos)
        {
            //保存系统编号字符串
            string sysNosString = string.Join(",", pickUpSysNos);

            #region select sql

            string sql = @"select * from LgPickUp a 
                            where exists(
                                            select 1 from splitstr(@0,',') b where b.col = a.sysno
                                        )";

            #endregion

            return Context.Sql(sql, sysNosString).QueryMany<LgPickUp>();
        }

        /// <summary>创建取件单
        /// </summary>
        /// <param name="model">实体.</param>
        /// <returns>返回取件单系统编号</returns>
        /// <remarks>
        /// 2013-07-12 何方 创建
        /// </remarks>
        public override int Create(LgPickUp model)
        {
            int sysNo = 0;
            using (var context = Context.UseTransaction(true))
            {
                try
                {
                    sysNo = context.Insert<LgPickUp>("LgPickUp", model)
                   .AutoMap(x => x.SysNo, x => x.Items)
                   .ExecuteReturnLastId<int>("SysNo");
                    if (model.Items != null)
                    {
                        foreach (var item in model.Items)
                        {
                            item.PickUpSysNo = sysNo;
                            ILgPickUpItemDao.Instance.Create(item);
                        }
                    }
                    context.Commit();
                }
                catch (Exception)
                {
                    //回滚
                    context.Rollback();
                }
            }
            return sysNo;
        }

        /// <summary>
        /// 修改取件单状态
        /// </summary>
        /// <param name="pickUpSysNo">取件单系统编号.</param>
        /// <param name="status">状态.</param>
        /// <returns>返回操作状态</returns>
        /// <remarks>
        /// 2013-07-14 何方 创建
        /// </remarks>
        public override bool UpdateStatus(int pickUpSysNo, LogisticsStatus.取件单状态 status)
        {
            int result = Context.Update("lgpickup")
                .Column("status", (int)status)
                .Where("sysno", pickUpSysNo)
                .Execute();
            return result > 0;
        }

        /// <summary>
        /// 获取取件单列表
        /// </summary>
        /// <param name="arrSysNos">取件单系统编号</param>
        /// <returns>取件单列表</returns>
        /// <remarks>黄伟 2013-11-22 创建</remarks>
        public override List<LgPickUp> GetLgPickUpList(int[] arrSysNos)
        {
            return
                Context.Sql("select * from LgPickUp  where sysno in (@sysno)")
                       .Parameter("sysno", arrSysNos)
                       .QueryMany<LgPickUp>();
        }
    }
}
