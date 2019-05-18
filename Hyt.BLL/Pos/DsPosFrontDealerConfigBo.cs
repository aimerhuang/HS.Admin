using Hyt.DataAccess.Pos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.Pos
{
    public class DsPosFrontDealerConfigBo : BOBase<DsPosFrontDealerConfigBo>
    {

        public  int InsertMod(Model.Pos.DsPosFrontDealerConfig mod)
        {
            return IDsPosFrontDealerConfigDao.Instance.InsertMod(mod);
        }

        public  void UpdateMod(Model.Pos.DsPosFrontDealerConfig mod)
        {
             IDsPosFrontDealerConfigDao.Instance.UpdateMod(mod);
        }

        public  void DeleteMod(int DsSysNo)
        {
            IDsPosFrontDealerConfigDao.Instance.DeleteMod(DsSysNo);
        }

        public  List<Model.Pos.DsPosFrontDealerConfig> GetModelList(int DsSysNo)
        {
            return IDsPosFrontDealerConfigDao.Instance.GetModelList(DsSysNo); 
        }
    }
}
