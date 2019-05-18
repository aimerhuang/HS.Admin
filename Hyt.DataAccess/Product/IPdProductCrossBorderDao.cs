using Hyt.DataAccess.Base;
using Hyt.Model.Generated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Product
{
    public abstract class IPdProductCrossBorderDao : DaoBase<IPdProductCrossBorderDao>
    {
        public abstract int InserMod(PdProductCrossBorder mod);
        public abstract void UpdateMod(PdProductCrossBorder mod);
        public abstract List<PdProductCrossBorder> GetProductListBySysNos(List<int> proSysNos);
        public abstract PdProductCrossBorder GetProductByProductSysNo(int proSysNo);
    }
}
