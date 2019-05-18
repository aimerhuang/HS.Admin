using Hyt.DataAccess.Pos;
using Hyt.Model.Pos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.Pos
{
    public class DsPosMoneyBoxDaoImpl : IDsPosMoneyBoxDao
    {
        public override int InsertMod(Model.Pos.DsPosMoneyBox box)
        {
            return Context.Insert("DsPosMoneyBox", box).AutoMap(p => p.SysNo).ExecuteReturnLastId<int>("SysNo");
        }

        public override void UpdateMod(Model.Pos.DsPosMoneyBox box)
        {
            Context.Update("DsPosMoneyBox", box).AutoMap(p => p.SysNo).Where(p=>p.SysNo).Execute();
        }

        public override void DeleteMod(int SysNo)
        {
            string sql = " delete from DsPosMoneyBox where SysNo='" + SysNo + "' ";
            Context.Sql(sql).Execute();
        }

        public override Model.Pos.CBDsPosMoneyBox GetEntity(int SysNo)
        {
            string sql = @"select DsPosMoneyBox.*,DsPosManage.pos_posName as PosSYName ,DsDealer.DealerName   from DsPosMoneyBox inner join DsPosManage on DsPosMoneyBox.DsPosSysNo=DsPosManage.SysNo
			                      inner join DsDealer on DsPosMoneyBox.DsSysNo=DsDealer.SysNo where DsPosMoneyBox.SysNo='" + SysNo + "'";
            return Context.Sql(sql).QuerySingle<CBDsPosMoneyBox>();
        }

        public override void GetDsPosMoneyBoxListPagerByDsSysNo(ref Model.Pager<CBDsPosMoneyBox> pager)
        {
            #region sql条件
            string sqlWhere = @"(DsSysNo=@DsSysNo or " + pager.PageFilter.DsSysNo + " = 0  ) ";
            #endregion

            using (var context = Context.UseSharedConnection(true))
            {
                pager.Rows = context.Select<CBDsPosMoneyBox>("DsPosMoneyBox.*,DsPosManage.pos_posName as PosSYName ,DsDealer.DealerName  ")
                           .From(" DsPosMoneyBox inner join DsPosManage on DsPosMoneyBox.DsPosSysNo=DsPosManage.SysNo inner join DsDealer on DsPosMoneyBox.DsSysNo=DsDealer.SysNo ")
                           .Where(sqlWhere)
                           .Parameter("DsSysNo", pager.PageFilter.DsSysNo)
                           .Paging(pager.CurrentPage, pager.PageSize)
                           .OrderBy("DsPosMoneyBox.SysNo desc")
                           .QueryMany();
                pager.TotalRows = context.Select<int>("count(1)")
                           .From(" DsPosMoneyBox inner join DsPosManage on DsPosMoneyBox.DsPosSysNo=DsPosManage.SysNo inner join DsDealer on DsPosMoneyBox.DsSysNo=DsDealer.SysNo  ")
                           .Where(sqlWhere)
                           .Parameter("DsSysNo", pager.PageFilter.DsSysNo)
                           .QuerySingle();
            }
        }
    }
}
