using Hyt.DataAccess.Warehouse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.Warehouse
{
    public class WhWarehouseChangeLogDaoImpl : IWhWarehouseChangeLogDao
    {
        public override int CreateMod(Model.Generated.WhWarehouseChangeLog log)
        {
            if (!CheckWhWarehouseChangeLogTable("WhWarehouseChangeLog"))
            {
                CreateWhWarehouseChangeLogTable("WhWarehouseChangeLog");
            }
            return Context.Insert("WhWarehouseChangeLog", log).AutoMap(p => p.SysNo).ExecuteReturnLastId<int>("SysNo");
        }

        public override void UpdateMod(Model.Generated.WhWarehouseChangeLog log)
        {
            Context.Update("WhWarehouseChangeLog", log).AutoMap(p => p.SysNo).Where(p=>p.SysNo).Execute();
        }

        public override void DeleteMod(int SysNo)
        {
            string sql = "delete from WhWarehouseChangeLog where SysNo=" + SysNo;
            Context.Sql(sql).Execute();
        }

        public override List<Model.Generated.WhWarehouseChangeLog> WarehouseLogList(int wareSysNo, int proSysNo)
        {
            string sql = "select * from WhWarehouseChangeLog where WareSysNo='" + wareSysNo + "' and ProSysNo ='" + proSysNo + "' order by ChageDate asc ";

            return Context.Sql(sql).QueryMany<Hyt.Model.Generated.WhWarehouseChangeLog>();
        }

        

        public override bool CheckWhWarehouseChangeLogTable(string table)
        {
             string sql = " select count(*) from sysobjects where id = object_id('" + table + "') ";
             int num = Context.Sql(sql).ExecuteReturnLastId<int>();
            if(num==0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public override void CreateWhWarehouseChangeLogTable(string table)
        {
            string sql = @"CREATE TABLE [WhWarehouseChangeLog](
	                        [SysNo] [int] IDENTITY(1,1) NOT NULL,
	                        [WareSysNo] [int] NULL,
	                        [ProSysNo] [int] NULL,
	                        [ChangeQuantity] [int] NULL,
	                        [Quantity] [int] NULL,
	                        [BusinessTypes] [varchar](100) NULL,
	                        [LogData] [text] NULL,
	                        [ChageDate] [datetime] NULL,
	                        [CreateDate] [datetime] NULL
                        )";
            Context.Sql(sql).Execute();
        }

        public override List<Model.Generated.WhWarehouseChangeLog> WarehouseLogList(int wareSysNo)
        {
            string sql = "select * from WhWarehouseChangeLog where WareSysNo='" + wareSysNo + "' order by ChageDate asc ";

            return Context.Sql(sql).QueryMany<Hyt.Model.Generated.WhWarehouseChangeLog>();
        }

        public override List<Model.Generated.WhWarehouseChangeLog> WarehouseLogList(string OrderNo)
        {
            string sql = "select * from WhWarehouseChangeLog where LogData like '%" + OrderNo + "%' order by ChageDate asc ";

            return Context.Sql(sql).QueryMany<Hyt.Model.Generated.WhWarehouseChangeLog>();
        }

        public override void UpdateWhInventoryWarehouseLog()
        {
            string sql = @"SELECT 
                           WhWarehouseChangeLog.SysNo
                          ,[WareSysNo]
                          ,[ProSysNo]
                          ,[ChangeQuantity]
                          ,[Quantity]
                          ,[BusinessTypes]
                          ,[LogData]
                          ,[WhInventory].EndDate as ChageDate
                          ,[CreateDate]
                      FROM [WhWarehouseChangeLog] inner join [WhInventory] on convert(varchar(500),[WhWarehouseChangeLog].LogData) = '盘点单号：'+[WhInventory].TransactionSysNo+''
	                    and [WhInventory].WarehouseSysNo=[WhWarehouseChangeLog].WareSysNo
                      where [WhInventory].EndDate>[ChageDate]";
            List<Hyt.Model.Generated.WhWarehouseChangeLog> list = Context.Sql(sql).QueryMany<Hyt.Model.Generated.WhWarehouseChangeLog>();
            foreach(var mod in list)
            {
                UpdateMod(mod);
            }
        }
    }
}
