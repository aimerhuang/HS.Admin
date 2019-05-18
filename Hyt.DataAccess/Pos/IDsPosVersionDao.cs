using Hyt.DataAccess.Base;
using Hyt.Model.Pos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Pos
{
    /// <summary>
    /// 收银机版本检查接口
    /// </summary>
    /// <remarks>
    /// 2016-02-24 杨云奕 添加
    /// </remarks>
    public abstract class IDsPosVersionDao : DaoBase<IDsPosVersionDao>
    {
         /// <summary>
         /// 添加版本实体
         /// </summary>
         /// <param name="verSion">版本对象</param>
         /// <returns>返回自动编号</returns>
        public abstract int Insert(DsPosVersion verSion);
        /// <summary>
        /// 修改版本实体
        /// </summary>
        /// <param name="verSion">版本对象</param>
        public abstract void Update(DsPosVersion verSion);

        /// <summary>
        /// 通过经销商编号获取版本信息
        /// </summary>
        /// <param name="sysno">经销商编号</param>
        /// <returns>版本实体</returns>
        public abstract DsPosVersion GetPosVersionByDsSysNo(int sysno);


    }
}
