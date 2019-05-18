using Hyt.DataAccess.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.Transport
{
    public class DsWhCustomerMessageBo : BOBase<DsWhCustomerMessageBo>
    {

        public  int InsertMod(Model.Transport.DsWhCustomerMessage mod)
        {
            return IDsWhCustomerMessageDao.Instance.InsertMod(mod);
        }
        public  void UpdateMod(Model.Transport.DsWhCustomerMessage mod)
        {
            IDsWhCustomerMessageDao.Instance.UpdateMod(mod);
        }

        public  List<Model.Transport.DsWhCustomerMessage> GetDsWhCustomerMessageByGMSysNo(int GMSysNo)
        {
            return IDsWhCustomerMessageDao.Instance.GetDsWhCustomerMessageByGMSysNo(GMSysNo);
        }
    }
}
