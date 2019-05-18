using Hyt.DataAccess.Base;
using Hyt.Model.Mobile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Mobile
{
    public abstract class ICrCustomerMobileLoginDao : DaoBase<ICrCustomerMobileLoginDao>
    {
        public abstract int InsertMod(CrCustomerMobileLogin mod);
        public abstract void UpdateMod(CrCustomerMobileLogin mod);
        public abstract CrCustomerMobileLogin GetModByTokenAndDeviceCode(string token, string deviceCode);
        public abstract CrCustomerMobileLogin GetModByCustomerSysNo(int CustomerSysNo);
        public abstract List<CrCustomerMobileLoginHistory> GetModByCustomerHistoryList(int CustomerSysNo);

        public abstract int InserHistoryMod(CrCustomerMobileLoginHistory mod);
    }
}
