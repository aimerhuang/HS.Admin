using Hyt.DataAccess.Promotion;
using Hyt.Model.Promotion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.Promotion
{
    public class SpSpikeDaoImpl : ISpSpikeDao
    {
        public override int InsertMod(Model.Promotion.SpSpike mod)
        {
            return Context.Insert("SpSpike", mod).AutoMap(p => p.SysNo).ExecuteReturnLastId<int>("SysNo");
        }

        public override void UpdateMod(Model.Promotion.SpSpike mod)
        {
            Context.Update("SpSpike", mod).AutoMap(p => p.SysNo, p => p.CreatedDate, p => p.CreatedBy).Where(p => p.SysNo).Execute();
        }

        public override void GetSpSpikeListPager(ref Model.Pager<Model.Promotion.SpSpike> SpSpikePager)
        {
            #region sql条件
            string sqlWhere = @"   ";
           
            #endregion
            using (var context = Context.UseSharedConnection(true))
            {
                SpSpikePager.Rows = context.Select<SpSpike>(" * ")
                           .From(@"  SpSpike  ")
                           .Paging(SpSpikePager.CurrentPage, SpSpikePager.PageSize)
                           .OrderBy(" SpSpike.CreatedDate ASC ")
                           .QueryMany();
                SpSpikePager.TotalRows = context.Select<int>("count(1)")
                           .From(@" SpSpike   ")
                           .QuerySingle();
            }
        }

        public override void UpdateSpSpikeStatus(int SysNo, int Status)
        {
            string sql = " update SpSpike set Status = " + Status + " where SysNo=" + SysNo + "  ";
            Context.Sql(sql).Execute();
        }

        public override void DeleteSpSpikeBySysNo(int SysNo)
        {
            string sql = " delete from SpSpike   where SysNo=" + SysNo + "  ";
            Context.Sql(sql).Execute();
        }

        public override SpSpike GetSpSpikeBySysNo(int SysNo)
        {
            string sql = " select * from  SpSpike where SysNo = '"+SysNo+"' ";
            return Context.Sql(sql).QuerySingle<SpSpike>();
        }

        public override int InsertSpSpikeItem(SpSpikeItem item)
        {
            return Context.Insert("SpSpikeItem", item).AutoMap(p => p.SysNo).ExecuteReturnLastId<int>("SysNo");
        }

        public override int UpdateSpSpikeItem(SpSpikeItem item)
        {
            return Context.Update("SpSpikeItem", item).AutoMap(p => p.SysNo).Where(p=>p.SysNo).Execute();
        }

        public override void DeleteSpSpikeItem(int SysNo)
        {
            string sql = " delete from SpSpikeItem where SysNo = '" + SysNo + "'  ";
            Context.Sql(sql).Execute();
        }

        public override void GetSpSpikeItemListPager(ref Model.Pager<SpSpikeItem> SpSpikePager)
        {
            throw new NotImplementedException();
        }

        public override List<SpSpikeItem> GetSpSpikeItemList(int pSysNo)
        {
            string sql = " select *  from SpSpikeItem where SpikeSysNo = '" + pSysNo + "'  ";
            return Context.Sql(sql).QueryMany<SpSpikeItem>();
        }
    }
}
