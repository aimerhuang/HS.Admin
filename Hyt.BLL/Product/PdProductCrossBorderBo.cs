using Hyt.DataAccess.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.Product
{
    public class PdProductCrossBorderBo : BOBase<PdProductCrossBorderBo>
    {
        public  int InserMod(Model.Generated.PdProductCrossBorder mod)
        {
            return IPdProductCrossBorderDao.Instance.InserMod(mod);
        }

        public  void UpdateMod(Model.Generated.PdProductCrossBorder mod)
        {
            IPdProductCrossBorderDao.Instance.UpdateMod(mod);
        }

        public  List<Model.Generated.PdProductCrossBorder> GetProductListBySysNos(List<int> proSysNos)
        {
            return IPdProductCrossBorderDao.Instance.GetProductListBySysNos(proSysNos);
        }

        public  Model.Generated.PdProductCrossBorder GetProductByProductSysNo(int proSysNo)
        {
            return IPdProductCrossBorderDao.Instance.GetProductByProductSysNo(proSysNo);
        }
    }
}
