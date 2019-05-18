using Hyt.DataAccess.Pos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.Pos
{
    /// <summary>
    /// 订单数据推送服务器操作类
    /// </summary>
    /// <remarks>2016-10-08 杨云奕 添加</remarks>
    public class SyWSOrderToClientJobBo : BOBase<SyWSOrderToClientJobBo>
    {
        public  int InnerMod(Model.Pos.SyWSOrderToClientJob mod)
        {
             return ISyWSOrderToClientJobDao.Instance.InnerMod(mod);
        }

        public  void UpdateMod(Model.Pos.SyWSOrderToClientJob mod)
        {
            ISyWSOrderToClientJobDao.Instance.UpdateMod(mod);
        }

        public  List<Model.SoOrder> GetNotPosthOrderDataToClientList(int dayValue)
        {
            return ISyWSOrderToClientJobDao.Instance.GetNotPosthOrderDataToClientList(dayValue);
        }

        public  List<Model.Pos.SyWSOrderToClientJob> GetPoshDataToClientList(int? DsSysNo)
        {
            return ISyWSOrderToClientJobDao.Instance.GetPoshDataToClientList(DsSysNo);
        }
    }
}
