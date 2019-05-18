using Hyt.DataAccess.Base;
using Hyt.Model.Pos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Pos
{
    public abstract class IDsPosMoneyBoxDao : DaoBase<IDsPosMoneyBoxDao>
    {
        public abstract int InsertMod(DsPosMoneyBox box);
        public abstract void UpdateMod(DsPosMoneyBox box);
        public abstract void DeleteMod(int SysNo);
        public abstract CBDsPosMoneyBox GetEntity(int SysNo);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pager"></param>
        public abstract void GetDsPosMoneyBoxListPagerByDsSysNo(ref Model.Pager<CBDsPosMoneyBox> pager);
    }
}
