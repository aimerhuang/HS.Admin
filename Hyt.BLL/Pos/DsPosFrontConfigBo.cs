using Hyt.DataAccess.Pos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.Pos
{
    public class DsPosFrontConfigBo : BOBase<DsPosFrontConfigBo>
    {
        public  List<Model.Pos.DsPosFrontConfig> GetFrontConfigList()
        {
            return IDsPosFrontConfigDao.Instance.GetFrontConfigList();
        }
    }
}
