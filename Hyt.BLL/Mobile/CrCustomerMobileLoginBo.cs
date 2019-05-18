using Hyt.DataAccess.Mobile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.Mobile
{
    public class CrCustomerMobileLoginBo : BOBase<CrCustomerMobileLoginBo>
    {

        public  int InsertMod(Model.Mobile.CrCustomerMobileLogin mod)
        {
            return ICrCustomerMobileLoginDao.Instance.InsertMod(mod);
        }

        public  void UpdateMod(Model.Mobile.CrCustomerMobileLogin mod)
        {
            ICrCustomerMobileLoginDao.Instance.UpdateMod(mod);
        }

        public  Model.Mobile.CrCustomerMobileLogin GetModByTokenAndDeviceCode(string token, string deviceCode)
        {
            return ICrCustomerMobileLoginDao.Instance.GetModByTokenAndDeviceCode(token, deviceCode);
        }

        public  Model.Mobile.CrCustomerMobileLogin GetModByCustomerSysNo(int CustomerSysNo)
        {
            return ICrCustomerMobileLoginDao.Instance.GetModByCustomerSysNo(CustomerSysNo);
        }

        public  List<Model.Mobile.CrCustomerMobileLoginHistory> GetModByCustomerHistoryList(int CustomerSysNo)
        {
            return ICrCustomerMobileLoginDao.Instance.GetModByCustomerHistoryList(CustomerSysNo);
        }

        public  int InserHistoryMod(Model.Mobile.CrCustomerMobileLoginHistory mod)
        {
            return ICrCustomerMobileLoginDao.Instance.InserHistoryMod(mod);
        }
    }
}
