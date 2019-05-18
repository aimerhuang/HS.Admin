using Hyt.DataAccess.Weixin;
using Hyt.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.MallSeller;
using Hyt.Infrastructure.Pager;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.DataAccess.Distribution;
using Hyt.BLL.Sys;
using Hyt.Infrastructure.Memory;

namespace Hyt.BLL.Weixin
{
    /// <summary>
    /// 微信自定义菜单
    /// </summary>
    /// <remarks>2016-1-9 杨浩 创建</remarks>
    public class MkCustomizeMenuBo : BOBase<MkCustomizeMenuBo>
    {
        /// <summary>
        /// 获取全部分销商菜单
        /// </summary>
        /// <returns></returns>
        /// <remarks>2016-1-9 杨浩 创建</remarks>
        public IList<MkCustomizeMenu> GetAllMkCustomizeMenuList()
        {
            return IMkCustomizeMenuDao.Instance.GetAllMkCustomizeMenuList();
        }
        /// <summary>
        /// 根据分销商获取分销商菜单
        /// </summary>
        /// <param name="dealerSysNo">分销商编号</param>
        /// <returns></returns>
        /// <remarks>2016-1-9 杨浩 创建</remarks>
        public  IList<MkCustomizeMenu> GetAllMkCustomizeMenuList(int dealerSysNo)
        {
            return IMkCustomizeMenuDao.Instance.GetAllMkCustomizeMenuList(dealerSysNo);
        }
        /// <summary>
        /// 删除分销商的菜单
        /// </summary>
        /// <param name="dealerSysNo">分销商编号</param>
        /// <returns></returns>
        public  int DeleteMkCustomizeMenuByDealerSysNo(int dealerSysNo)
        {
            return IMkCustomizeMenuDao.Instance.DeleteMkCustomizeMenuByDealerSysNo(dealerSysNo);
        }
        /// <summary>
        /// 删除分销商的菜单
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns></returns>
        public  int DeleteMkCustomizeMenu(int sysNo)
        {
            return IMkCustomizeMenuDao.Instance.DeleteMkCustomizeMenu(sysNo);
        }
        /// <summary>
        /// 获取分销商微信菜单
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public MkCustomizeMenu GetEntity(int SysNo)
        {
            return IMkCustomizeMenuDao.Instance.GetEntity(SysNo);
        }
        /// <summary>
        /// 添加分销商微信菜单
        /// </summary>
        /// <param name="customizeMenu">微信菜单实体</param>
        /// <returns></returns>
        public int AddMkCustomizeMenu(MkCustomizeMenu customizeMenu)
        {
            return IMkCustomizeMenuDao.Instance.AddMkCustomizeMenu(customizeMenu);

        }
        /// <summary>
        /// 更新分销商微信公众号菜单
        /// </summary>
        /// <param name="customizeMenu">微信菜单实体</param>
        /// <returns></returns>
        public int UpdateMkCustomizeMenu(MkCustomizeMenu customizeMenu)
        {          
            return IMkCustomizeMenuDao.Instance.UpdateMkCustomizeMenu(customizeMenu);
        }
        /// <summary>
        /// 获取子菜单总数
        /// </summary>
        /// <param name="pid">父级编号</param>
        /// <returns></returns>
        public int GetMkCustomizeMenuChildCount(int pid)
        {
            return IMkCustomizeMenuDao.Instance.GetMkCustomizeMenuChildCount(pid);
        }
        /// <summary>
        /// 获取分销商对应菜单总数
        /// </summary>
        /// <param name="pid">父级编号</param>
        /// <param name="dealerSysNo">经销商编号</param>
        /// <returns></returns>
        /// <remarks>2016-1-11 王耀发 创建</remarks>
        public int GetMkCustomizeMenuCountInDealerParent(int pid, int dealerSysNo)
        {
            return IMkCustomizeMenuDao.Instance.GetMkCustomizeMenuCountInDealerParent(pid, dealerSysNo);
        }

        /// <summary>
        /// 获取子菜单
        /// </summary>
        /// <param name="pid">父级编号</param>
        /// <returns></returns>
        public IList<MkCustomizeMenu> GetMkCustomizeMenuChilds(int pid)
        {
            return IMkCustomizeMenuDao.Instance.GetMkCustomizeMenuChilds(pid);
        }
        /// <summary>
        /// 获取对应微信菜单
        /// 2016-1-11 王耀发 创建
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public PagedList<CBMkCustomizeMenu> GetMkCustomizeMenuList(ParaMkCustomizeMenuFilter filter)
        {
            if (filter != null)
            {
                var model = new PagedList<CBMkCustomizeMenu>();
                filter.PageSize = model.PageSize;
                var pager = IMkCustomizeMenuDao.Instance.GetMkCustomizeMenuList(filter);
                if (null != pager)
                {
                    model.TData = pager.Rows;
                    model.TotalItemCount = pager.TotalRows;
                    model.CurrentPageIndex = filter.CurrentPage;
                }
                return model;
            }
            return null;
        }
        /// <summary>
        /// 获取对应微信菜单(子级)
        /// 2016-1-11 王耀发 创建
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public PagedList<CBMkCustomizeMenu> GetMkCustomizeSubMenuList(ParaMkCustomizeMenuFilter filter)
        {
            if (filter != null)
            {
                var model = new PagedList<CBMkCustomizeMenu>();
                filter.PageSize = model.PageSize;
                var pager = IMkCustomizeMenuDao.Instance.GetMkCustomizeSubMenuList(filter);
                if (null != pager)
                {
                    model.TData = pager.Rows;
                    model.TotalItemCount = pager.TotalRows;
                    model.CurrentPageIndex = filter.CurrentPage;
                }
                return model;
            }
            return null;
        }
        /// <summary>
        /// 保存微信菜单
        /// </summary>
        /// <param model>模型</param>
        /// <remarks>2016-1-11 王耀发 创建</remarks>    
        public Result SaveMkCustomizeMenu(MkCustomizeMenu model, SyUser user)
        {
            Result r = new Result()
            {
                Status = false
            };
            MkCustomizeMenu entity = IMkCustomizeMenuDao.Instance.GetEntity(model.SysNo);
            if (entity != null)
            {
                model.SysNo = entity.SysNo;
                int rowsAffected = IMkCustomizeMenuDao.Instance.UpdateMkCustomizeMenu(model);
                if (rowsAffected > 0)
                {
                    r.Status = true;
                    r.Message = "保存成功";
                }
                else
                {
                    r.Status = false;
                    r.Message = "保存失败";
                }
            }
            else
            {
                int rowsAffected = IMkCustomizeMenuDao.Instance.AddMkCustomizeMenu(model);
                if (rowsAffected > 0)
                {
                    r.Status = true;
                    r.Message = "保存成功";
                }
                else
                {
                    r.Status = false;
                    r.Message = "保存失败";
                }
            }
            return r;
        }
        /// <summary>
        /// 同步信营全球购经销商的菜单，只能同步两级菜单
        /// 王耀发 2016-2-3 创建
        /// </summary>
        /// <param name="DealerSysNo">被同步的经销商系统编号</param>
        /// <returns></returns>
        public int ProCreateMkCustomizeMenu(int DealerSysNo)
        {
            return IMkCustomizeMenuDao.Instance.ProCreateMkCustomizeMenu(DealerSysNo);
        }
    }
}
