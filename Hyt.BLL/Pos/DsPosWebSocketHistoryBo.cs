using Hyt.DataAccess.Pos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.Pos
{
    public class DsPosWebSocketHistoryBo : BOBase<DsPosWebSocketHistoryBo>
    {

        public  int Inser(Model.Pos.DsPosWebSocketHistory mod)
        {
            return IDsPosWebSocketHistoryDao.Instance.Inser(mod);
        }

        public  void Update(Model.Pos.DsPosWebSocketHistory mod)
        {
             IDsPosWebSocketHistoryDao.Instance.Update(mod);
        }

        public  Model.Pos.DsPosWebSocketHistory GetModel(int SysNo)
        {
            return IDsPosWebSocketHistoryDao.Instance.GetModel(SysNo);
        }

        public  List<Model.Pos.DsPosWebSocketHistory> GetList(int DsSysNo)
        {
            return IDsPosWebSocketHistoryDao.Instance.GetList(DsSysNo);
        }
    }
}
