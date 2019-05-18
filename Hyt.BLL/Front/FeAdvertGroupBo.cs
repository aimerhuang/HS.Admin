using System;
using System.Collections.Generic;
using Hyt.Model;
using Hyt.DataAccess.Front;
using Hyt.Model.WorkflowStatus;
using Hyt.Infrastructure.Pager;

namespace Hyt.BLL.Front
{
    /// <summary>
    /// 广告模块类型 业务层
    /// </summary>
    /// <remarks>2013-06-17 苟治国 创建</remarks>
    public class FeAdvertGroupBO : BOBase<FeAdvertGroupBO>
    {
        /// <summary>
        /// 获取广告组实体
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-06-17 苟治国 创建</remarks>
        public FeAdvertGroup GetModel(int sysNo)
        {
            return IFeAdvertGroupDao.Instance.GetModel(sysNo);
        }

        /// <summary>
        /// 验证广告组名称
        /// </summary>
        /// <param name="key">广告组名称</param>
        /// <param name="sysNo">广告组编号</param>
        /// <returns></returns>
        /// <remarks>2013-06-17 苟治国 创建</remarks>
        public int FeAdvertGroupChk(string key, int sysNo)
        {
            return IFeAdvertGroupDao.Instance.FeAdvertGroupChk(key, sysNo);
        }

        /// <summary>
        /// 根据条件获取广告组的列表
        /// </summary>
        /// <param name="pageIndex">分页索引</param>
        /// <param name="type">类型</param>
        /// <param name="status">状态</param>
        /// <param name="platformType">平台类型</param>
        /// <param name="key">搜索关键字</param>
        /// <returns>广告组列表</returns>
        /// <remarks>2013-06-17 苟治国 创建</remarks>
        public PagedList<FeAdvertGroup> Seach(int pageIndex, int? type, int? status,int platformType, string key = null)
        {
            var list = new PagedList<FeAdvertGroup>();
            var pager = new Pager<FeAdvertGroup>();

            if (status == null){
                status = -1;
            }
            else{
                status = status ?? -1; 
            }
            pager.CurrentPage = pageIndex;
            pager.PageFilter = new FeAdvertGroup
            {
                Type = -1,
                PlatformType = platformType,
                Status = (int)status,
                Name = key
            };
            pager.PageSize = list.PageSize;
            pager = IFeAdvertGroupDao.Instance.Seach(pager);
            list = new PagedList<FeAdvertGroup>
            {
                Data = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows
            };
            return list;
        }

        /// <summary>
        /// 根据条件获取广告组的总条数
        /// </summary>
        /// <param name="type">广告类型</param>
        /// <param name="platformType">广告组平台类型</param>
        /// <param name="status">广告状态</param>
        /// <param name="key">搜索关键字</param>
        /// <returns>总数</returns>
        /// <remarks>2013－06-17 苟治国 创建</remarks>
        public int GetCount(int? type, int? platformType, int? status, string key = null)
        {
            return IFeAdvertGroupDao.Instance.GetCount(type, platformType, status, key);
        }

        /// <summary>
        /// 新增广告组
        /// </summary>
        /// <param name="model">广告组实体</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-06-17 苟治国 创建</remarks>
        public int Insert(Model.FeAdvertGroup model)
        {
            int result = IFeAdvertGroupDao.Instance.Insert(model);
            if (result > 0)
                BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, string.Format("添加广告组{0}",model.Name), LogStatus.系统日志目标类型.商品组展示, Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo);
            else
                BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台, string.Format("添加广告组{0}失败", model.Name), LogStatus.系统日志目标类型.商品组展示, Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo);
            return result;
        }

        /// <summary>
        /// 更新广告组
        /// </summary>
        /// <param name="model">广告组实体</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-06-17 苟治国 创建</remarks>
        public int Update(Model.FeAdvertGroup model)
        {
            int result = IFeAdvertGroupDao.Instance.Update(model);
            if (result > 0)
            {
                if (Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo != Hyt.Model.SystemPredefined.Constant.UserSysNo)
                {
                    Hyt.BLL.Sys.SyJobPoolPublishBo.Instance.FeAudit(model.SysNo, 0, Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo, "AdvertGroup", "修改广告组", Hyt.Model.SystemPredefined.Constant.UserSysNo, (int)Hyt.Model.WorkflowStatus.SystemStatus.任务对象类型.通知);
                }
                BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, string.Format("更新广告组{0}", model.SysNo), LogStatus.系统日志目标类型.商品组展示, Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo);
            }
            
            else
                BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台, string.Format("更新广告组{0}失败", model.SysNo), LogStatus.系统日志目标类型.商品组展示, Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo);
            return result;
        }

        /// <summary>
        /// 更新广告组状态
        /// </summary>
        /// <param name="collocation">广告组实体</param>
        /// <returns>返回受影响的行</returns>
        /// <remarks>2013-06-17 苟治国 创建</remarks>
        public int UpdateStatus(IList<FeAdvertGroup> collocation)
        {
            int result = 0;
            try
            {
                for (int i = 0; i < collocation.Count; i++)
                {
                    int sysNo = Convert.ToInt32(collocation[i].SysNo);
                    int status = Convert.ToInt32(collocation[i].Status);

                    var model = this.GetModel(sysNo);
                    model.Status = status;
                    result = this.Update(model);
                    if (result > 0)
                    {
                        if (Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo != Hyt.Model.SystemPredefined.Constant.UserSysNo)
                        {
                            Hyt.BLL.Sys.SyJobPoolPublishBo.Instance.FeAudit(model.SysNo, 0, Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo, "AdvertGroup", "修改广告组状态", Hyt.Model.SystemPredefined.Constant.UserSysNo, (int)Hyt.Model.WorkflowStatus.SystemStatus.任务对象类型.通知);
                        }
                        BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, string.Format("修改广告组状态{0}", model.SysNo),LogStatus.系统日志目标类型.商品组展示,Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo,result);
                    }
                    else
                    {
                        BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台, string.Format("修改广告组状态{0}失败", model.SysNo), LogStatus.系统日志目标类型.商品组展示, Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo, result);
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
        /// 添加店铺关联广告组
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ///<remarks>2016-07-28 周 创建</remarks>
        public int InsertDealerFeAdvertItem(DsDealerFeAdvertItem model)
        {
            int result = IFeAdvertGroupDao.Instance.InsertDealerFeAdvertItem(model);
            if (result > 0)
                BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, string.Format("添加店铺关联广告项{0}", model.SysNo), LogStatus.系统日志目标类型.广告组展示, Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo);
            else
                BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台, string.Format("添加店铺关联广告项{0}失败", model.SysNo), LogStatus.系统日志目标类型.广告组展示, Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo);
            return result;
        }
        /// <summary>
        /// 更新店铺关联广告组
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ///<remarks>2016-07-28 周 创建</remarks>
        public int UpdateDealerFeAdvertItem(DsDealerFeAdvertItem model)
        {
            int result = IFeAdvertGroupDao.Instance.UpdateDealerFeAdvertItem(model);
            if (result > 0)
            {
                if (Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo != Hyt.Model.SystemPredefined.Constant.UserSysNo)
                {
                    Hyt.BLL.Sys.SyJobPoolPublishBo.Instance.FeAudit(model.SysNo, 0, Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo, "AdvertGroup", "更新店铺关联广告项", Hyt.Model.SystemPredefined.Constant.UserSysNo, (int)Hyt.Model.WorkflowStatus.SystemStatus.任务对象类型.通知);
                }
                BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, string.Format("更新店铺关联广告项{0}", model.SysNo), LogStatus.系统日志目标类型.广告组展示, Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo);
            }

            else
                BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台, string.Format("更新店铺关联广告项{0}失败", model.SysNo), LogStatus.系统日志目标类型.广告组展示, Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo);
            return result;
        }
        /// <summary>
        /// 获取店铺关联广告组表信息
        /// </summary>
        /// <param name="FeAdvertGroupSysNO"></param>
        /// <returns></returns>
        ///<remarks>2016-07-28 周 创建</remarks>
        public DsDealerFeAdvertItem GetModelDealerFeAdvertItem(int FeAdvertGroupSysNO)
        {
            return IFeAdvertGroupDao.Instance.GetModelDealerFeAdvertItem(FeAdvertGroupSysNO);
        }
        /// <summary>
        /// 删除店铺关联广告项表信息
        /// </summary>
        /// <param name="FeAdvertItemSysNO"></param>
        /// <returns></returns>
        public bool DeleteDealerFeAdvertItem(int FeAdvertItemSysNO)
        {
            return IFeAdvertGroupDao.Instance.DeleteDealerFeAdvertItem(FeAdvertItemSysNO);
        }
        /// <summary>
        /// 是否存在店铺广告项
        /// </summary>
        /// <param name="FeAdvertItemSysNO"></param>
        /// <returns></returns>
        public int IsExistenceDealerFeAdvertItem(int FeAdvertItemSysNO)
        {
            return IFeAdvertGroupDao.Instance.IsExistenceDealerFeAdvertItem(FeAdvertItemSysNO);
        }
    }
}
