using Hyt.DataAccess.Mobile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.Mobile
{
    public class MBDesignFrameBo : BOBase<MBDesignFrameBo>
    {
        public  int InsreMod(Model.Mobile.MBDesignFrame mod)
        {
           return IMBDesignFrameDao.Instance.InsreMod(mod);
        }

        public  void UpdateMod(Model.Mobile.MBDesignFrame mod)
        {
            IMBDesignFrameDao.Instance.UpdateMod(mod);
        }

        public  void DeleteMod(int SysNo)
        {
            IMBDesignFrameDao.Instance.DeleteMod(SysNo);
        }

        public  List<Model.Mobile.MBDesignFrame> GetPageDataList(int customSysNo, string pageText, string tipCode)
        {
            return IMBDesignFrameDao.Instance.GetPageDataList(customSysNo, pageText, tipCode);
        }

        public  List<Model.Mobile.MBDesignFrame> GetPageDataListByPSysNo(int pSysNo, string pageText, string tipCode)
        {
            return IMBDesignFrameDao.Instance.GetPageDataListByPSysNo(pSysNo, pageText, tipCode);
        }

        public List<Model.Mobile.MBDesignFrame> GetPageDataAllList()
        {
            return IMBDesignFrameDao.Instance.GetPageDataAllList();
        }
    }
}
