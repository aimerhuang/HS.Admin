using Hyt.DataAccess.Pos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.Pos
{
    /// <summary>
    /// 收银机版本检查接口
    /// </summary>
    /// <remarks>
    /// 2016-02-24 杨云奕 添加
    /// </remarks>
    public class DsPosVersionDaoImpl : IDsPosVersionDao
    {
        /// <summary>
        /// 添加版本实体
        /// </summary>
        /// <param name="verSion">版本对象</param>
        /// <returns>返回自动编号</returns>
        public override int Insert(Model.Pos.DsPosVersion verSion)
        {
            return Context.Insert("DsPosVersion", verSion).AutoMap(p => p.SysNo).ExecuteReturnLastId<int>("SysNo");
        }
        /// <summary>
        /// 修改版本实体
        /// </summary>
        /// <param name="verSion">版本对象</param>
        public override void Update(Model.Pos.DsPosVersion verSion)
        {
            Context.Update("DsPosVersion", verSion).AutoMap(p => p.SysNo).Execute();
        }
        /// <summary>
        /// 通过经销商编号获取版本信息
        /// </summary>
        /// <param name="sysno">经销商编号</param>
        /// <returns>版本实体</returns>
        public override Model.Pos.DsPosVersion GetPosVersionByDsSysNo(int sysno)
        {
            string sql = " select * from DsPosVersion  ";
            return Context.Sql(sql).QuerySingle<Hyt.Model.Pos.DsPosVersion>();
        }
    }
}
