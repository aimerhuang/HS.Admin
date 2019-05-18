using Hyt.DataAccess.Base;
using Hyt.Model.Pos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Pos
{
    public abstract class IDsPosWebSocketHistoryDao : DaoBase<IDsPosWebSocketHistoryDao>
    {
        public abstract int Inser(DsPosWebSocketHistory mod);
        public abstract void Update(DsPosWebSocketHistory mod);
        public abstract DsPosWebSocketHistory GetModel(int SysNo);
        public abstract List<DsPosWebSocketHistory> GetList(int DsSysNo);
    }
}
