using Hyt.DataAccess.Base;
using Hyt.Model.Mobile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Mobile
{
    public abstract class IMBDesignFrameDao : DaoBase<IMBDesignFrameDao>
    {
        public abstract int InsreMod(MBDesignFrame mod);
        public abstract void UpdateMod(MBDesignFrame mod);
        public abstract void DeleteMod(int SysNo);
        public abstract List<MBDesignFrame> GetPageDataList(int customSysNo, string pageText, string tipCode);
        public abstract List<MBDesignFrame> GetPageDataListByPSysNo(int pSysNo, string pageText, string tipCode);

        public abstract List<MBDesignFrame> GetPageDataAllList();
    }
}
