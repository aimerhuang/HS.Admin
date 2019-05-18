using Hyt.DataAccess.Pos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.Pos
{
    public class DsDealerPayConfigBo : BOBase<DsDealerPayConfigBo>
    {
        public  int InnerMod(Model.Pos.DsDealerPayConfig mod)
        {
            throw new NotImplementedException();
        }

        public  void UpdateMod(Model.Pos.DsDealerPayConfig mod)
        {
            throw new NotImplementedException();
        }

        public  Model.Pos.DsDealerPayConfig GetMod(int DsSysNo, string type)
        {
            return IDsDealerPayConfigDao.Instance.GetMod(DsSysNo, type);
        }
    }
}
