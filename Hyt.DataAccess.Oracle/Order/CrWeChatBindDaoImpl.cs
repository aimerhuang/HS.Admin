using Hyt.DataAccess.Oracle.Warehouse;
using Hyt.DataAccess.Order;
using Hyt.Model.Generated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.Order
{
    public class CrWeChatBindDaoImpl : ICrWeChatBindDao
    {
        public override int InnerMod(Model.Generated.CrWeChatBind mod)
        {
            return Context.Insert("CrWeChatBind", mod).AutoMap(p => p.SysNo).ExecuteReturnLastId<int>("SysNo");
        }

        public override void UpdateMod(Model.Generated.CrWeChatBind mod)
        {
            Context.Update("CrWeChatBind", mod).AutoMap(p => p.SysNo, x => x.CreateTime).Where(p => p.SysNo).Execute();
        }

        public override void DeleteMod(int SysNo)
        {
            string sql = "delete from CrWeChatBind where SysNo="+SysNo+" ";
            Context.Sql(sql).Execute();
        }

        public override Model.Generated.BCCrWeChatBind GetWeChatBindData(int SysNo)
        {
            string sql = " select CrWeChatBind.*,CrCustomer.Name as AccountName, CrCustomer.Account as AccountCode from CrWeChatBind inner join CrCustomer on CrWeChatBind.AccountSysNo=CrCustomer.SysNo  where CrWeChatBind.SysNo = '" + SysNo + "' ";
            BCCrWeChatBind model = Context.Sql(sql).QuerySingle<BCCrWeChatBind>();
            List<Hyt.Model.WhWarehouse> whhouseList = WhWarehouseDaoImpl.Instance.GetAllWarehouseListBySysNos(model.BindWhSysNos);
            foreach (var mod in whhouseList)
            {
                if(model.BindWarehousName!="")
                {
                    model.BindWarehousName += ",";
                }
                model.BindWarehousName += mod.BackWarehouseName;
            }
            return model;
        }

        public override void GetChatBindDataPager(ref Model.Pager<Model.Generated.BCCrWeChatBind> pager)
        {
            pager.TotalRows = Context.Sql(@"select count(1) from CrWeChatBind a  ")
                                      
                                       .QuerySingle<int>();

            pager.Rows = Context.Select<BCCrWeChatBind>(" CrWeChatBind.*,CrCustomer.Name as AccountName, CrCustomer.Account as AccountCode ")
                                                  .From(@" CrWeChatBind inner join CrCustomer on CrWeChatBind.AccountSysNo=CrCustomer.SysNo ")

                                                  .OrderBy("CrWeChatBind.SysNo")
                                                  .Paging(pager.CurrentPage, pager.PageSize)
                                                  .QueryMany();
        }

        public override List<BCCrWeChatBind> GetWeChatBindListByTrue()
        {
            string sql = " select CrWeChatBind.*, CrCustomer.Openid as OpenId,CrCustomer.Name as AccountName, CrCustomer.Account as AccountCode from CrWeChatBind inner join CrCustomer on CrWeChatBind.AccountSysNo=CrCustomer.SysNo  where CStatus=1 ";
            return Context.Sql(sql).QueryMany<BCCrWeChatBind>();
        }
    }
}
