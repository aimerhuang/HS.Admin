using Hyt.DataAccess.Promotion;
using Hyt.Model;
using Hyt.Model.Promotion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.Promotion
{
    public class SpSpikeBo : BOBase<SpSpikeBo>
    {
        public  int InsertMod(Model.Promotion.SpSpike mod)
        {
            return ISpSpikeDao.Instance.InsertMod(mod);
        }

        public  void UpdateMod(Model.Promotion.SpSpike mod)
        {
            ISpSpikeDao.Instance.UpdateMod(mod);
        }

        public  void GetSpSpikeListPager(ref Model.Pager<Model.Promotion.SpSpike> SpSpikePager)
        {
            ISpSpikeDao.Instance.GetSpSpikeListPager(ref SpSpikePager);
        }

        public  void UpdateSpSpikeStatus(int SysNo, int Status)
        {
            ISpSpikeDao.Instance.UpdateSpSpikeStatus(SysNo, Status);
        }

        public  void DeleteSpSpikeBySysNo(int SysNo)
        {
            ISpSpikeDao.Instance.DeleteSpSpikeBySysNo(SysNo);
        }

        public Model.Promotion.SpSpike GetSpSpikeBySysNo(int SysNo)
        {
            return ISpSpikeDao.Instance.GetSpSpikeBySysNo(SysNo);
        }


        public int InsertSpSpikeItem(SpSpikeItem item) {
            return ISpSpikeDao.Instance.InsertSpSpikeItem(item);
        }
        public int UpdateSpSpikeItem(SpSpikeItem item) {
            return ISpSpikeDao.Instance.UpdateSpSpikeItem(item);
        }
        public void DeleteSpSpikeItem(int SysNo) {
            ISpSpikeDao.Instance.DeleteSpSpikeItem(SysNo);
        }
        public void GetSpSpikeItemListPager(ref Pager<SpSpikeItem> SpSpikePager) { }
        public List<SpSpikeItem> GetSpSpikeItemList(int pSysNo) {
            return ISpSpikeDao.Instance.GetSpSpikeItemList(pSysNo);
        }
    }
}
