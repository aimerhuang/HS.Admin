using Hyt.DataAccess.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.Transport
{
    public class DsWhHistoryBo : BOBase<DsWhHistoryBo>
    {

        public  int insertMod(Model.Transport.DsWhHistory history)
        {
            return IDsWhHistoryDao.Instance.insertMod(history);
        }

        public  List<Model.Transport.CBDsWhHistory> GetHistoryListByCourierNumber(string Code)
        {
            return IDsWhHistoryDao.Instance.GetHistoryListByCourierNumber(Code);
        }

        public Model.Transport.CBDsWhHistory GetEntityBySysNo(int SysNo)
        {
            return IDsWhHistoryDao.Instance.GetEntityBySysNo(SysNo);
        }

        public void UpdateMod(Hyt.Model.Transport.DsWhHistory history)
        {
            IDsWhHistoryDao.Instance.UpdateMod(history);
        }
    }
}
