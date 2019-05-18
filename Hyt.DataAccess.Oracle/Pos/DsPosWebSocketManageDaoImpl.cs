using Hyt.DataAccess.Pos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.Pos
{
    /// <summary>
    /// 网络服务推送接口
    /// </summary>
    /// <remarks>2016-08-03 杨云奕 添加</remarks>
    public class DsPosWebSocketManageDaoImpl : IDsPosWebSocketManageDao
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public override int InsertMod(Model.Pos.DsPosWebSocketManage mod)
        {
            return Context.Insert("DsPosWebSocketManage", mod).AutoMap(p => p.SysNo).ExecuteReturnLastId<int>();
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public override void UpdateMod(Model.Pos.DsPosWebSocketManage mod)
        {
             Context.Update("DsPosWebSocketManage", mod).AutoMap(p => p.SysNo).Where(p=>p.SysNo).Execute();
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public override void DeleteMod(Model.Pos.DsPosWebSocketManage mod)
        {
            Context.Delete("DsPosWebSocketManage", mod).Where(p => p.SysNo).Execute();
        }
        /// <summary>
        /// 更改状态
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public override void ChangeStatus(Model.Pos.DsPosWebSocketManage mod)
        {
            string sql = " update DsPosWebSocketManage set WS_Status='" + mod.WS_Status + "' where SysNo = '" + mod.SysNo + "'  ";
            Context.Sql(sql).Execute();
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public override List<Model.Pos.DsPosWebSocketManage> List(int? pSysNo)
        {
            string sql = " select * from DsPosWebSocketManage ";
            if(pSysNo!=null&&pSysNo.Value>0)
            {
                sql += " where WS_PosManageSysNo = '" + pSysNo.Value + "' ";
            }
            return Context.Sql(sql).QueryMany<Hyt.Model.Pos.DsPosWebSocketManage>();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public override Model.Pos.DsPosWebSocketManage GetModel(int? SysNo)
        {
            string sql = " select * from DsPosWebSocketManage ";

            sql += " where SysNo = '" + SysNo.Value + "' ";
           
            return Context.Sql(sql).QuerySingle<Hyt.Model.Pos.DsPosWebSocketManage>();
        }

        public override List<Model.Pos.DsPosWebSocketManage> GetSocketManage(int[] PosManageSysNos)
        {
            string sql = " select * from DsPosWebSocketManage ";
            if (PosManageSysNos.Length > 0)
            {
                sql += " where WS_PosManageSysNo in (" + string.Join(",", PosManageSysNos) + ") ";
            }
            return Context.Sql(sql).QueryMany<Hyt.Model.Pos.DsPosWebSocketManage>();
        }
    }
}
