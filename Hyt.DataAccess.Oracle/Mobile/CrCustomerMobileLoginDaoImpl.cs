using Hyt.DataAccess.Mobile;
using Hyt.Model.Mobile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.Mobile
{
    public class CrCustomerMobileLoginDaoImpl : ICrCustomerMobileLoginDao
    {
        public override int InsertMod(Model.Mobile.CrCustomerMobileLogin mod)
        {
            return Context.Insert("CrCustomerMobileLogin", mod).AutoMap(p => p.SysNo).ExecuteReturnLastId<int>("SysNo");
        }

        public override void UpdateMod(Model.Mobile.CrCustomerMobileLogin mod)
        {
            Context.Update("CrCustomerMobileLogin", mod).AutoMap(p => p.SysNo).Where(p=>p.SysNo).Execute();
        }

        public override Model.Mobile.CrCustomerMobileLogin GetModByTokenAndDeviceCode(string token, string deviceCode)
        {
            string sql = " select * from CrCustomerMobileLogin where CustomerToken='" + token + "'  ";//and CustomerLoginDeviceCode = '"+deviceCode+"'
            return Context.Sql(sql).QuerySingle<CrCustomerMobileLogin>();
        }

        public override Model.Mobile.CrCustomerMobileLogin GetModByCustomerSysNo(int CustomerSysNo)
        {
            string sql = " select * from CrCustomerMobileLogin where CustomerSysNo='" + CustomerSysNo + "' ";
            return Context.Sql(sql).QuerySingle<CrCustomerMobileLogin>();
        }

        public override List<Model.Mobile.CrCustomerMobileLoginHistory> GetModByCustomerHistoryList(int CustomerSysNo)
        {
            string sql = " select * from CrCustomerMobileLogin inner join CrCustomerMobileLoginHistory on CrCustomerMobileLogin.SysNo=CrCustomerMobileLoginHistory.CustomerMobileSysNo ";
            sql += " where CrCustomerMobileLogin.CustomerSysNo = '" + CustomerSysNo + "' ";
            return Context.Sql(sql).QueryMany<CrCustomerMobileLoginHistory>();
        }

        public override int InserHistoryMod(CrCustomerMobileLoginHistory mod)
        {
            return Context.Insert("CrCustomerMobileLoginHistory", mod).AutoMap(p => p.SysNo).ExecuteReturnLastId<int>("SysNo");
        }
    }
}
