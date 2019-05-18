using Hyt.DataAccess.Pos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.Pos
{
    public class DsPosWebSocketManageBo : BOBase<DsPosWebSocketManageBo>
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public  int InsertMod(Model.Pos.DsPosWebSocketManage mod)
        {
            return IDsPosWebSocketManageDao.Instance.InsertMod(mod);

        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public  void UpdateMod(Model.Pos.DsPosWebSocketManage mod)
        {
            IDsPosWebSocketManageDao.Instance.UpdateMod(mod);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public  void DeleteMod(Model.Pos.DsPosWebSocketManage mod)
        {
            IDsPosWebSocketManageDao.Instance.DeleteMod(mod);
        }
        /// <summary>
        /// 更改状态
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public  void ChangeStatus(Model.Pos.DsPosWebSocketManage mod)
        {
            IDsPosWebSocketManageDao.Instance.ChangeStatus(mod);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public  List<Model.Pos.DsPosWebSocketManage> List(int? pSysNo)
        {
            return IDsPosWebSocketManageDao.Instance.List(pSysNo);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public  Model.Pos.DsPosWebSocketManage GetModel(int? SysNo)
        {
            return IDsPosWebSocketManageDao.Instance.GetModel(SysNo);
        }

        public List<Model.Pos.DsPosWebSocketManage> GetSocketManage(int[] PosManageSysNos)
        {
            return IDsPosWebSocketManageDao.Instance.GetSocketManage(PosManageSysNos);
        }
    }
}
