using Hyt.DataAccess.Pos;
using Hyt.Model.Pos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.Pos
{
    /// <summary>
    /// 条码支付历史记录
    /// </summary>
    public class DsPosBarcodePayLogDaoImpl : IDsPosBarcodePayLogDao
    {
        public override int InnerDsPosBarcodePayLog(Model.Pos.DsPosBarcodePayLog Mod)
        {
            return Context.Insert("DsPosBarcodePayLog", Mod).AutoMap(p => p.SysNo).ExecuteReturnLastId<int>("SysNo");
        }

        public override void UpdateDsPosBarcodePayLog(Model.Pos.DsPosBarcodePayLog Mod)
        {
            Context.Update("DsPosBarcodePayLog", Mod).AutoMap(p => p.SysNo).Where(p => p.SysNo).Execute();
        }

        public override void DeleteDsPosBarcodePayLog(int SysNo)
        {
            string sql = " delete from DsPosBarcodePayLog where SysNo='" + SysNo + "' ";
            Context.Sql(sql).Execute();
        }

        public override void DsPosBarcodePayLogPager(ref Model.Pager<Model.Pos.DsPosBarcodePayLog> pager)
        {
            #region sql条件
            string sqlWhere = @"(DsSysNo=@DsSysNo or " + pager.PageFilter.DsSysNo + " = 0  ) ";
            #endregion

            using (var context = Context.UseSharedConnection(true))
            {
                pager.Rows = context.Select<DsPosBarcodePayLog>("DsPosBarcodePayLog.*")
                           .From(" DsPosBarcodePayLog ")
                           .Where(sqlWhere)
                           .Parameter("DsSysNo", pager.PageFilter.DsSysNo)
                           .Paging(pager.CurrentPage, pager.PageSize)
                           .OrderBy("DsPosBarcodePayLog.SysNo desc")
                           .QueryMany();
                pager.TotalRows = context.Select<int>("count(1)")
                           .From(" DsPosBarcodePayLog ")
                           .Where(sqlWhere)
                           .Parameter("DsSysNo", pager.PageFilter.DsSysNo)
                           .QuerySingle();
            }
        }
    }
}
