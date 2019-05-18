using System;
using System.Collections.Generic;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.DataAccess.Front;
using Hyt.Model.Parameter;
using Hyt.Model.WorkflowStatus;

namespace Hyt.BLL.Front
{
    /// <summary>
    /// 广告项 业务层
    /// </summary>
    /// <remarks>2013-06-17 苟治国 创建</remarks>
    public class FeAdvertItemBo : BOBase<FeAdvertItemBo>
    {
        /// <summary>
        /// 获取广告项实体
        /// </summary>
        /// <param name="sysNo">广告项编号</param>
        /// <returns>广告项实体</returns>
        /// <remarks>2013-06-17 苟治国 创建</remarks>
        public FeAdvertItem GetModel(int sysNo)
        {
            return IFeAdvertItemDao.Instance.GetModel(sysNo);
        }

        /// <summary>
        /// 根据条件获取广告项的列表
        /// </summary>
        /// <param name="pageIndex">索引</param>
        /// <param name="groupSysNo">广告组编号</param>
        /// <param name="status">状态</param>
        /// <param name="beginDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="linkTitle">连接名称</param>
        /// <returns>广告项列表</returns>
        /// <remarks>2013-10-11 苟治国 创建</remarks>
        public PagedList<Model.CBFeAdvertItem> Seach(ParaAdvertItemFilter filter)
        {
            var list = new PagedList<CBFeAdvertItem>();

            var pager = new Pager<CBFeAdvertItem>();
            pager.CurrentPage = (int)filter.id;
            pager.PageFilter = new CBFeAdvertItem
            {
                GroupSysNo = filter.groupSysNo,
                Status = (int)filter.status,
                LinkTitle = filter.linkTitle,
                Name = filter.linkTitle,
                DealerSysNo = filter.DealerSysNo,
                IsBindDealer = filter.IsBindDealer,
                IsBindAllDealer = filter.IsBindAllDealer,
                DealerCreatedBy = filter.DealerCreatedBy,
                SelectedDealerSysNo = filter.SelectedDealerSysNo
            };

            var para = new ParaFeAdvertItem()
                {
                    StartTime = filter.beginDate,
                    EndTime = filter.endDate,
                };

            pager.PageSize = list.PageSize;
            pager = IFeAdvertItemDao.Instance.Seach(pager, para);
            list = new PagedList<CBFeAdvertItem>
            {
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows
            };
            return list;
        }

        /// <summary>
        /// 根据广告组获取所有广告项分类
        /// </summary>
        /// <param name="groupSysNo">广告组编号</param>
        /// <returns>广告项列表</returns>
        /// <remarks>2013－06-17 苟治国 创建</remarks>
        public IList<FeAdvertItem> GetListByGroup(int groupSysNo)
        {
            return IFeAdvertItemDao.Instance.GetListByGroup(groupSysNo);
        }

        /// <summary>
        /// 新增广告项
        /// </summary>
        /// <param name="model">广告项实体</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-06-17 苟治国 创建</remarks>
        public int Insert(FeAdvertItem model)
        {
            model.LastUpdateDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            int result = IFeAdvertItemDao.Instance.Insert(model);
            if (result > 0)
            {
                RemoveCache(model.GroupSysNo);
                BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, string.Format("添加广告项{0}", model.Name), LogStatus.系统日志目标类型.商品项展示, Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo);
            }
            else
            {
                BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台, string.Format("添加广告项{0}失败", model.Name), LogStatus.系统日志目标类型.商品项展示, Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo);
            }
            return result;
        }

        /// <summary>
        /// 更新广告项
        /// </summary>
        /// <param name="model">广告项实体</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-06-17 苟治国 创建</remarks>
        public int Update(FeAdvertItem model)
        {
            int result = IFeAdvertItemDao.Instance.Update(model);
            if (result > 0)
            {
                RemoveCache(model.GroupSysNo);
                if (Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo != Hyt.Model.SystemPredefined.Constant.UserSysNo)
                {
                    Hyt.BLL.Sys.SyJobPoolPublishBo.Instance.FeAudit(model.SysNo, model.GroupSysNo, Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo, "AdvertItem", "修改广告项", Hyt.Model.SystemPredefined.Constant.UserSysNo, (int)Hyt.Model.WorkflowStatus.SystemStatus.任务对象类型.通知);
                }

                BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, string.Format("修改广告项{0}", model.SysNo), LogStatus.系统日志目标类型.商品项展示, Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo);
            }
            else
            {
                BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台, string.Format("修改广告项{0}失败", model.SysNo), LogStatus.系统日志目标类型.商品项展示, Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo);
            }
            return result;
        }

        /// <summary>
        /// 更新广告项状态
        /// </summary>
        /// <param name="collocation">广告项实体</param>
        /// <returns>返回受影响的行</returns>
        /// <remarks>2013-06-17 苟治国 创建</remarks>
        public int UpdateStatus(IList<FeAdvertItem> collocation)
        {
            int result = 0;
            try
            {
                for (int i = 0; i < collocation.Count; i++)
                {
                    int sysNo = Convert.ToInt32(collocation[i].SysNo);
                    int status = Convert.ToInt32(collocation[i].Status);

                    var model = this.GetModel(sysNo);
                    if (model.Status != (int)Hyt.Model.WorkflowStatus.ForeStatus.广告项状态.作废)
                    {
                        model.Status = status;
                        result = this.Update(model);
                        if (result > 0)
                        {
                            RemoveCache(model.GroupSysNo);
                            if (Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo != Hyt.Model.SystemPredefined.Constant.UserSysNo)
                            {
                                Hyt.BLL.Sys.SyJobPoolPublishBo.Instance.FeAudit(model.SysNo, model.GroupSysNo, Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo, "AdvertItem", "修改广告项状态", Hyt.Model.SystemPredefined.Constant.UserSysNo, (int)Hyt.Model.WorkflowStatus.SystemStatus.任务对象类型.通知);
                            }
                            BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, string.Format("修改广告项{0}", model.SysNo), LogStatus.系统日志目标类型.商品项展示, Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo);
                        }
                        else
                        {
                            BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台, string.Format("修改广告项{0}失败", model.SysNo), LogStatus.系统日志目标类型.商品项展示, Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo);
                        }
                    }

                }

            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        /// <summary>
        /// 批量移除前台广告缓存
        /// </summary>
        /// <param name="groupSysNo">广告组编号</param>
        /// <remarks>2013-12-03 苟治国 创建</remarks>
        public void RemoveCache(int groupSysNo)
        {
            var model = BLL.Front.FeAdvertGroupBO.Instance.GetModel(groupSysNo);
            if (model != null)
            {
                Hyt.Infrastructure.Caching.CacheManager.RemoveCache(Hyt.Infrastructure.Caching.CacheKeys.Items.WebAdvertItem_, model.Code);
            }
        }
        /// <summary>
        /// 同步总部已审核的广告
        /// </summary>
        /// <param name="GroupSysNo"></param>
        /// <param name="DealerSysNo"></param>
        /// <param name="CreatedBy"></param>
        /// <returns></returns>
        /// <remarks>2016-1-13 王耀发 创建</remarks>
        public int ProCreateFeAdvertItem(int GroupSysNo, int DealerSysNo, int CreatedBy)
        {
            return IFeAdvertItemDao.Instance.ProCreateFeAdvertItem(GroupSysNo, DealerSysNo, CreatedBy);
        }
    }
}
