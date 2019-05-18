using Hyt.DataAccess.Base;
using Hyt.Model.Generated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Logistics
{
    public abstract class ILgFreightValuationModuleDao : DaoBase<ILgFreightValuationModuleDao>
    {
        public abstract List<LgFreightValuationModule> GetFreightModel(int psysNo);
        public abstract List<LgFreightValuationModule> GetFreightModel(int psysNo, int AreaSysNo);
        public abstract LgFreightValuationModule GetFreightModel(int psysNo, int AreaSysNo, decimal decimalValue);

        public abstract int InsertFreightModelData(LgFreightValuationModule mod);
        public abstract void UpdateFreightModelData(LgFreightValuationModule mod);
        public abstract void DeleteFreightModelData(string delIdList);
    }
}
