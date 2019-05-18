using Hyt.DataAccess.Base;
using Hyt.Model.Generated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Order
{
    public abstract class ISoChangeOrderPriceDao : DaoBase<ISoChangeOrderPriceDao>
    {

        public abstract int Insert(SoChangeOrderPrice mod);
        public abstract void Update(SoChangeOrderPrice mod);
        public abstract void Delete(int SysNo);
        public abstract SoChangeOrderPrice GetDataByOrderSysNo(int OrderSysNo);
    }
}
