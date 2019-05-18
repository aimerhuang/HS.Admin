using System;
using System.Collections.Generic;
using Hyt.Model;

namespace Hyt.DataAccess.Oracle.Sys
{
    /// <summary>
    /// 网站购物状态管理
    /// </summary>
    /// <remarks>2016-07-01 周 创建</remarks>
    public class TdWebsiteDaolmpl: DataAccess.Sys.TdWebsiteDao
    {
        /// <summary>
        /// select
        /// </summary>
        /// <param name="sysNo">sysNo</param>
        /// <returns>entity</returns>
        /// <remarks>2016-07-01 周 创建</remarks>
        public override TdWebsiteManagement SelectWebsiteManagement(int sysNo)
        {
            return Context.Sql("select * from TdWebsiteManagement where SysNo=@0", sysNo)
                          .QuerySingle<TdWebsiteManagement>();
        }

        /// <summary>
        /// update
        /// </summary>
        /// <param name="model">model</param>
        /// <returns>boolean</returns>
        /// <remarks>2016-07-01 周 创建</remarks>
        public override bool UpdateWebsiteManagement(TdWebsiteManagement model)
        {
            var r = Context.Update("TdWebsiteManagement", model)
                           .AutoMap(o => o.SysNo)
                           .Where("SysNo", model.SysNo).Execute();
            return r > 0;
        }

        /// <summary>
        /// select
        /// </summary>
        /// <param name="sysNo">sysNo</param>
        /// <returns>entity</returns>
        /// <remarks>2016-07-01 周 创建</remarks>
        public override TdWebsiteState SelectWebsiteState(int sysNo)
        {
            return Context.Sql("select * from TdWebsiteState where SysNo=@0", sysNo)
                          .QuerySingle<TdWebsiteState>();
        }
        /// <summary>
        /// insert
        /// </summary>
        /// <param name="model">model</param>
        /// <returns>sysNo</returns>
        /// <remarks>2016-07-01 周海鹏 创建</remarks>
        public override int InsertWebsiteState(TdWebsiteState model)
        {
            return Context.Insert("TdWebsiteState", model)
                                        .AutoMap(o => o.SysNo)
                                        .ExecuteReturnLastId<int>("SysNo");
        }
        /// <summary>
        /// update
        /// </summary>
        /// <param name="model">model</param>
        /// <returns>boolean</returns>
        /// <remarks>2016-07-01 周 创建</remarks>
        public override bool UpdateWebsiteState(TdWebsiteState model)
        {
            var r = Context.Update("TdWebsiteState", model)
                           .AutoMap(o => o.SysNo)
                           .Where("SysNo", model.SysNo).Execute();
            return r > 0;
        }
        /// <summary>
        /// list
        /// </summary>
        /// <returns>IList</returns>
        /// <remarks>2016-07-01 周 创建</remarks>
        public override IList<TdWebsiteState> SelectAllWebsiteState()
        {
            const string sql = @"select * from TdWebsiteState where Isdelete=0 order by SysNo asc";
            return Context.Sql(sql).QueryMany<TdWebsiteState>();
        }
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="currentPage">当前页号</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="status">状态</param>
        /// <returns>分页</returns>
        /// <remarks>2016-07-01 周 创建</remarks>
        public override Pager<TdWebsiteState> SelectAllWebsiteState(int currentPage, int pageSize, int? status, int? ProvinceSysno, int? CitySysno, int? AreaSysNo, int? WarehouseSysNo, string WarehouseName)
        {
            string sql = "", where = " Isdelete=0 ";

            if (ProvinceSysno > 0)
                where += " and b2.ParentSysNo=" + ProvinceSysno + "";
            if (CitySysno > 0)
                where += " and b.ParentSysNo=" + CitySysno + "";
            if (AreaSysNo > 0)
                where += " and w.AreaSysNo=" + AreaSysNo + "";
            if (WarehouseSysNo > 0)
                where += " and t.WarehouseSysNo=" + WarehouseSysNo + "";
            if(WarehouseName!=""||WarehouseName!=null)
                where += " and w.WarehouseName like'%" + WarehouseName + "%'";
            if(status>-1)
                where += " and t.Status=" + status + "";
            sql = "(select t.*,w.WarehouseName,w.AreaSysNo,b.ParentSysNo as CitySysno,b2.ParentSysNo as ProvinceSysno from TdWebsiteState t left join WhWarehouse w on t.WarehouseSysNo=w.SysNo left join BsArea b on w.AreaSysNo=b.SysNo left join BsArea b2 on b2.sysno=b.ParentSysNo where "+where+") tb";

            var dataList = Context.Select<TdWebsiteState>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(0)").From(sql);

            var pager = new Pager<TdWebsiteState>
            {
                PageSize = pageSize,
                CurrentPage = currentPage,
                TotalRows = dataCount.QuerySingle(),
                Rows = dataList.OrderBy("tb.SysNo asc").Paging(currentPage, pageSize).QueryMany()
            };

            return pager;
        }
        /// <summary>
        /// delete
        /// </summary>
        /// <param name="sysNo">sysNo</param>
        /// <returns>boolean</returns>
        /// <remarks>2016-07-01 周 创建</remarks>
        public override bool DeleteWebsiteState(int sysNo)
        {
            int rowsAffected = Context.Sql("Update TdWebsiteState set Isdelete = 1 where sysno = @sysno")
                                       .Parameter("sysno", sysNo)
                                       .Execute();
            return rowsAffected > 0;
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="currentPage">当前页号</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="status">状态</param>
        /// <returns>分页</returns>
        /// <remarks>2016-07-01 周 创建</remarks>
        public override Pager<TdWebsiteManagement> SelectWebsiteManagement(int currentPage, int pageSize, int? status, int? ProvinceSysno, int? CitySysno, int? AreaSysNo, int? WarehouseSysNo, string WarehouseName)
        {
            string sql = "", where = " 1=1 ";

            if (ProvinceSysno > 0)
                where += " and b2.ParentSysNo=" + ProvinceSysno + "";
            if (CitySysno > 0)
                where += " and b.ParentSysNo=" + CitySysno + "";
            if (AreaSysNo > 0)
                where += " and w.AreaSysNo=" + AreaSysNo + "";
            if (WarehouseSysNo > 0)
                where += " and t.WarehouseSysNo=" + WarehouseSysNo + "";
            if (WarehouseName != "" || WarehouseName != null)
                where += " and w.WarehouseName like'%" + WarehouseName + "%'";
            if (status > -1)
                where += " and t.WebsiteState=" + status + "";

            sql = "(select t.*,w.WarehouseName,w.AreaSysNo,b.ParentSysNo as CitySysno,b2.ParentSysNo as ProvinceSysno from TdWebsiteManagement t left join WhWarehouse w on t.WarehouseSysNo=w.SysNo left join BsArea b on w.AreaSysNo=b.SysNo left join BsArea b2 on b2.sysno=b.ParentSysNo where " + where + ") tb";

            var dataList = Context.Select<TdWebsiteManagement>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(0)").From(sql);

            var pager = new Pager<TdWebsiteManagement>
            {
                PageSize = pageSize,
                CurrentPage = currentPage,
                TotalRows = dataCount.QuerySingle(),
                Rows = dataList.OrderBy("tb.SysNo asc").Paging(currentPage, pageSize).QueryMany()
            };

            return pager;
        }
        /// <summary>
        /// insert
        /// </summary>
        /// <param name="model">model</param>
        /// <returns>sysNo</returns>
        /// <remarks>2016-07-01 周 创建</remarks>
        public override int InsertWebsiteManagement(TdWebsiteManagement model)
        {
            return Context.Insert("TdWebsiteManagement", model)
                                        .AutoMap(o => o.SysNo)
                                        .ExecuteReturnLastId<int>("SysNo");
        }
        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="WarehouseSysNo"></param>
        /// <returns></returns>
        public override int ExitWebsiteManagement(int WarehouseSysNo)
        {
            var sql = "select count(1) from TdWebsiteManagement where WarehouseSysNo=@WarehouseSysNo";
            return Context.Sql(sql).Parameter("WarehouseSysNo", WarehouseSysNo).QuerySingle<int>();
        }

    }
}
