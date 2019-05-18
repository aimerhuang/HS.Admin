using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Sys;
using Hyt.Model;

namespace Hyt.BLL.Sys
{
    /// <summary>
    /// 我的菜单Bo
    /// </summary>
    /// <remarks>2014-01-08 周唐炬 添加注释</remarks>
    public class SyMyMenuBo : BOBase<SyMyMenuBo>
    {
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="syMyMenu">数据实体</param>
        /// <returns></returns>
        /// <remarks>2013-10-09  周瑜 创建</remarks>
        public void Insert(SyMyMenu syMyMenu)
        {
            ISyMyMenuDao.Instance.Insert(syMyMenu);
        }

        /// <summary>
        /// 获取我的菜单
        /// </summary>
        /// <param name="userSysNo">用户系统编号</param>
        /// <param name="menuSysNo">菜单系统编号</param>
        /// <returns>我的菜单</returns>
        /// <remarks>2013-10-09  周瑜 创建</remarks>
        public SyMyMenu GetMoudle(int userSysNo, int menuSysNo)
        {
            return ISyMyMenuDao.Instance.GetModel(userSysNo, menuSysNo);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="userSysNo">用户系统编号</param>
        /// <param name="menuSysNo">菜单系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-10-09  周瑜 创建</remarks>
        public void Delete(int userSysNo, int menuSysNo)
        {
            ISyMyMenuDao.Instance.Delete(userSysNo, menuSysNo);
        }

        /// <summary>
        /// 获取我的菜单
        /// </summary>
        /// <param name="userSysNo">系统用户号</param>
        /// <returns>菜单列表</returns>
        /// <remarks> 2013-10-09 周瑜 创建</remarks>
        public IList<SyMenu> GetList(int userSysNo)
        {
            return ISyMyMenuDao.Instance.GetList(userSysNo);
        }
    }
}
