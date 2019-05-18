using Hyt.DataAccess.Transport;
using Hyt.Model.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.Transport
{
    public class DsWhRecipienterDaoImpl : IDsWhRecipienterDao
    {
        public override int InsertMod(Model.Transport.DsWhRecipienter mod)
        {
            return Context.Insert("DsWhRecipienter", mod).AutoMap(p => p.SysNo).ExecuteReturnLastId<int>("SysNo");
        }

        public override void UpdateMod(Model.Transport.DsWhRecipienter mod)
        {
            Context.Update("DsWhRecipienter", mod).AutoMap(p => p.SysNo).Where(p => p.SysNo).Execute();
        }

        public override Model.Transport.DsWhRecipienter GetDsWhRecipienter(int SysNo)
        {
            string sql = "select * from DsWhRecipienter where SysNo='" + SysNo + "'";
            return Context.Sql(sql).QuerySingle<Hyt.Model.Transport.DsWhRecipienter>();
        }

        public override void DeleteDsWhRecipienterBySysNo(int SysNo)
        {
            string sql = " delete from DsWhRecipienter where SysNo= '"+SysNo+"' ";
            Context.Sql(sql).Execute();
        }

        public override List<Model.Transport.DsWhRecipienter> GetDsWhRecipienterList(List<string> IdCardList)
        {
            string sql = " select * from DsWhRecipienter where IDCard in ('" + string.Join("','", IdCardList.ToArray()) + "') ";
            return Context.Sql(sql).QueryMany<DsWhRecipienter>();
        }

        public override void GetDsWhRecipienterList(ref Model.Pager<DsWhRecipienter> pageCusList)
        {
            #region sql条件
            string sqlWhere = @" 1=1 ";
            if (!string.IsNullOrEmpty(pageCusList.PageFilter.Name))
            {
                sqlWhere += " and Name = '" + pageCusList.PageFilter.Name.Trim() + "' ";
            }
            #endregion
           

            using (var context = Context.UseSharedConnection(true))
            {
                pageCusList.Rows = context.Select<DsWhRecipienter>("  *  ")
                           .From(@"  DsWhRecipienter ")
                           .Where(sqlWhere)
                           .Paging(pageCusList.CurrentPage, pageCusList.PageSize)
                           .OrderBy("DsWhRecipienter.SysNo desc")
                           .QueryMany();
                pageCusList.TotalRows = context.Select<int>("count(1)")
                           .From(@"  DsWhRecipienter ")
                           .Where(sqlWhere)
                           .QuerySingle();
            }
        }

        public override DsWhRecipienter GetDsWhRecipienterByIDCard(string IDCard)
        {
            string sql = " select * from DsWhRecipienter where IDCard = '" + IDCard + "' ";
            return Context.Sql(sql).QuerySingle<DsWhRecipienter>();
        }
    }
}
