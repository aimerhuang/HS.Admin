using Hyt.DataAccess.Base;
using Hyt.Model.Pos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Pos
{
    /// <summary>
    /// 网络服务推送接口
    /// </summary>
    /// <remarks>2016-08-03 杨云奕 添加</remarks>
    public abstract class IDsPosWebSocketManageDao : DaoBase<IDsPosWebSocketManageDao>
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public abstract int InsertMod(DsPosWebSocketManage mod);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public abstract void UpdateMod(DsPosWebSocketManage mod);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public abstract void DeleteMod(DsPosWebSocketManage mod);
        /// <summary>
        /// 更改状态
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public abstract void ChangeStatus(DsPosWebSocketManage mod);
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public abstract List<DsPosWebSocketManage> List(int? pSysNo );
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public abstract DsPosWebSocketManage GetModel(int? SysNo);
        /// <summary>
        /// 获取推送服务定义
        /// </summary>
        /// <param name="PosManageSysNos">自动编号</param>
        /// <returns></returns>
        public abstract List<DsPosWebSocketManage> GetSocketManage(int[] PosManageSysNos);
    }
}
