using Hyt.DataAccess.Base;
using Hyt.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;

namespace Hyt.DataAccess.Weixin
{
    /// <summary>
    /// 微信自定义菜单
    /// </summary>
    /// <remarks>2016-1-9 杨浩 创建</remarks>
    public abstract class IMkCustomizeMenuDao : DaoBase<IMkCustomizeMenuDao>
    {
        /// <summary>
        /// 获取全部分销商菜单
        /// </summary>
        /// <returns></returns>
        /// <remarks>2016-1-9 杨浩 创建</remarks>
        public abstract IList<MkCustomizeMenu> GetAllMkCustomizeMenuList();
        /// <summary>
        /// 根据分销商获取分销商菜单
        /// </summary>
        /// <param name="dealerSysNo">分销商编号</param>
        /// <returns></returns>
        /// <remarks>2016-1-9 杨浩 创建</remarks>
        public abstract IList<MkCustomizeMenu> GetAllMkCustomizeMenuList(int dealerSysNo);
        /// <summary>
        /// 删除分销商的菜单
        /// </summary>
        /// <param name="dealerSysNo">分销商编号</param>
        /// <returns></returns>
        public abstract int DeleteMkCustomizeMenuByDealerSysNo(int dealerSysNo);
        /// <summary>
        /// 删除分销商的菜单
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns></returns>
        public abstract int DeleteMkCustomizeMenu(int sysNo);
        /// <summary>
        /// 获取分销商微信菜单
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public abstract MkCustomizeMenu GetEntity(int SysNo);
        /// <summary>
        /// 添加分销商微信菜单
        /// </summary>
        /// <param name="customizeMenu">微信菜单实体</param>
        /// <returns></returns>
        public abstract int AddMkCustomizeMenu(MkCustomizeMenu customizeMenu);
        /// <summary>
        /// 更新分销商微信公众号菜单
        /// </summary>
        /// <param name="customizeMenu">微信菜单实体</param>
        /// <returns></returns>
        public abstract int UpdateMkCustomizeMenu(MkCustomizeMenu customizeMenu);
        /// <summary>
        /// 获取子菜单总数
        /// </summary>
        /// <param name="pid">父级编号</param>
        /// <returns></returns>
        public abstract int GetMkCustomizeMenuChildCount(int pid);
        /// <summary>
        /// 获取分销商对应菜单总数
        /// </summary>
        /// <param name="pid">父级编号</param>
        /// <param name="dealerSysNo">经销商编号</param>
        /// <returns></returns>
        /// <remarks>2016-1-11 王耀发 创建</remarks>
        public abstract int GetMkCustomizeMenuCountInDealerParent(int pid, int dealerSysNo);

        /// <summary>
        /// 获取子菜单
        /// </summary>
        /// <param name="pid">父级编号</param>
        /// <returns></returns>
        public abstract IList<MkCustomizeMenu> GetMkCustomizeMenuChilds(int pid);

        /// <summary>
        /// 获取对应微信菜单
        /// 2016-1-11 王耀发 创建
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public abstract Pager<CBMkCustomizeMenu> GetMkCustomizeMenuList(ParaMkCustomizeMenuFilter filter);

        /// <summary>
        /// 获取对应微信菜单(子级)
        /// 2016-1-11 王耀发 创建
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public abstract Pager<CBMkCustomizeMenu> GetMkCustomizeSubMenuList(ParaMkCustomizeMenuFilter filter);
        /// <summary>
        /// 同步信营全球购经销商的菜单，只能同步两级菜单
        /// 王耀发 2016-2-3 创建
        /// </summary>
        /// <param name="DealerSysNo">被同步的经销商系统编号</param>
        /// <returns></returns>
        public abstract int ProCreateMkCustomizeMenu(int DealerSysNo);
    }
}
