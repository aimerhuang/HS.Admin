using Hyt.DataAccess.Pos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.Pos
{
    /// <summary>
    /// 收银机版本检查接口
    /// </summary>
    /// <remarks>
    /// 2016-02-24 杨云奕 添加
    /// </remarks>
    public class DsPosVersionBo : BOBase<DsPosVersionBo>
    {
        /// <summary>
        /// 添加版本实体
        /// </summary>
        /// <param name="verSion">版本对象</param>
        /// <returns>返回自动编号</returns>
        public int Insert(Model.Pos.DsPosVersion verSion)
        {
            return IDsPosVersionDao.Instance.Insert(verSion);
        }
        /// <summary>
        /// 修改版本实体
        /// </summary>
        /// <param name="verSion">版本对象</param>
        public void Update(Model.Pos.DsPosVersion verSion)
        {
            IDsPosVersionDao.Instance.Update(verSion);
        }
        /// <summary>
        /// 通过经销商编号获取版本信息
        /// </summary>
        /// <param name="sysno">经销商编号</param>
        /// <returns>版本实体</returns>
        public Model.Pos.DsPosVersion GetPosVersionByDsSysNo(int sysno)
        {
            return IDsPosVersionDao.Instance.GetPosVersionByDsSysNo(sysno);
        }
    }
}
