using Hyt.DataAccess.Procurement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.Procurement
{
    public class PmManufacturProductBo : BOBase<PmManufacturProductBo>
    {

        public int InsertData(Model.Procurement.PmManufacturProduct mod)
        {
            return IPmManufacturProductDao.Instance.InsertData(mod);
        }

        public void UpdateData(Model.Procurement.PmManufacturProduct mod)
        {
            IPmManufacturProductDao.Instance.UpdateData(mod);
        }

        public void DeleteData(int ProSysNo)
        {
            IPmManufacturProductDao.Instance.DeleteData(ProSysNo);
        }

        public void DeleteData(int ProSysNo, int PmSysNo)
        {
            IPmManufacturProductDao.Instance.DeleteData(ProSysNo, PmSysNo);
        }

        public Model.Procurement.CBPmManufacturProduct GetManufacturProduct(int ProSysNo, int PmSysNo)
        {
            return IPmManufacturProductDao.Instance.GetManufacturProduct(ProSysNo, PmSysNo);
        }

        public List<Model.Procurement.CBPmManufacturProduct> GetManufacturProductByList(int ProSysNo)
        {
            return IPmManufacturProductDao.Instance.GetManufacturProductByList(ProSysNo);
        }
    }
}
