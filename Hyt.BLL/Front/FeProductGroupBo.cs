using System;
using System.Collections.Generic;
using Hyt.Model;
using Hyt.DataAccess.Front;
using Hyt.Model.WorkflowStatus;
using Hyt.Infrastructure.Pager;

namespace Hyt.BLL.Front
{
    /// <summary>
    /// 商品组 业务层
    /// </summary>
    /// <remarks>2013-07-20 苟治国 创建</remarks>
    public class FeProductGroupBo : BOBase<FeProductGroupBo>
    {
        /// <summary>
        /// 查看商品组
        /// </summary>
        /// <param name="sysNo">商品组主关键</param>
        /// <returns>商品组</returns>
        /// <remarks>2013-06-20 苟治国 创建</remarks>
        public FeProductGroup GetModel(int sysNo)
        {
            return IFeProductGroupDao.Instance.GetModel(sysNo);
        }

        /// <summary>
        /// 根据商品组编号查询商品组
        /// </summary>
        /// <param name="code">商品组编号</param>
        /// <param name="platform">商品组平台类型</param>
        /// <returns>商品组</returns>
        /// <remarks>2013－08-21 周瑜 创建</remarks>
        public IList<FeProductGroup> GetModelByGroupcode(string code, ForeStatus.商品组平台类型 platform)
        {
            return IFeProductGroupDao.Instance.GetModelByGroupcode(code,platform);
        }

        /// <summary>
        /// 验证商品组名称
        /// </summary>
        /// <param name="key">广告组名称</param>
        /// <param name="sysNo">广告组编号</param>
        /// <returns>返回关键字条数</returns>
        /// <remarks>2013-06-21 苟治国 创建</remarks>
        public int FeProductGroupChk(string key, int sysNo)
        {
            return IFeProductGroupDao.Instance.FeProductGroupChk(key, sysNo);
        }

        /// <summary>
        /// 根据条件获取产品组的列表
        /// </summary>
        /// <param name="pageIndex">索引</param>
        /// <param name="platformType">平台类型</param>
        /// <param name="status">状态</param>
        /// <param name="key">关键字</param>
        /// <returns>产品组列表</returns>
        /// <remarks>2013－06-21 苟治国 创建</remarks>
        public PagedList<FeProductGroup> Seach(int pageIndex, int? platformType, int? status, string key = null)
        {
            var list = new PagedList<FeProductGroup>();
            var pager = new Pager<FeProductGroup>();

            if (status == null)
            {
                status = -1;
            }
            else
            {
                status = status ?? -1;
            }

            pager.CurrentPage = pageIndex;
            pager.PageFilter = new FeProductGroup
            {
                PlatformType = (int)platformType,
                Status = (int)status,
                Name = key
            };
            pager.PageSize = list.PageSize;
            pager = IFeProductGroupDao.Instance.Seach(pager);
            list = new PagedList<FeProductGroup>
            {
                Data = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows
            };
            return list;
        }

        /// <summary>
        /// 根据条件获取产品组的列表
        /// </summary>
        /// <param name="pageIndex">索引</param>
        /// <param name="platformType">平台类型</param>
        /// <param name="status">状态</param>
        /// <param name="key">关键字</param>
        /// <returns>产品组列表</returns>
        /// <remarks>2013－06-21 苟治国 创建</remarks>
        public PagedList<FeProductGroup> SeachProductGroup(int pageIndex, int? platformType, int? status, string key = null)
        {
            var list = new PagedList<FeProductGroup>();
            list.PageSize = 1000;
            var pager = new Pager<FeProductGroup>();

            if (status == null)
            {
                status = -1;
            }
            else
            {
                status = status ?? -1;
            }

            pager.CurrentPage = pageIndex;
            pager.PageFilter = new FeProductGroup
            {
                PlatformType = (int)platformType,
                Status = (int)status,
                Name = key
            };
            pager.PageSize = list.PageSize;
            pager = IFeProductGroupDao.Instance.Seach(pager);
            list = new PagedList<FeProductGroup>
            {
                Data = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows
            };
            return list;
        }

        /// <summary>
        /// 新增商品组
        /// </summary>
        /// <param name="model">插入的数据模型</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-06-21 苟治国 创建</remarks>
        public int Insert(FeProductGroup model)
        {
            int result = IFeProductGroupDao.Instance.Insert(model);
            if (result > 0)
                BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, string.Format("添加商品组{0}",model.Name), LogStatus.系统日志目标类型.商品组展示, Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo);
            else
                BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台, string.Format("添加商品组{0}失败", model.Name), LogStatus.系统日志目标类型.商品组展示, Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo);
            return result;
        }

        /// <summary>
        /// 更新商品组
        /// </summary>
        /// <param name="model">更新的数据模型</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-06-21 苟治国 创建</remarks>
        public int Update(FeProductGroup model)
        {
            int result = IFeProductGroupDao.Instance.Update(model);
            if (result > 0)
            {
                if (Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo !=Hyt.Model.SystemPredefined.Constant.UserSysNo)
                {
                    Hyt.BLL.Sys.SyJobPoolPublishBo.Instance.FeAudit(model.SysNo, 0, Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo, "ProductGroup", "修改商品组", Hyt.Model.SystemPredefined.Constant.UserSysNo, (int)Hyt.Model.WorkflowStatus.SystemStatus.任务对象类型.通知);
                }
                
                BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, string.Format("修改商品组{0}", model.SysNo), LogStatus.系统日志目标类型.商品组展示, Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo);
            }
            
            else
            {
                BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台, string.Format("修改商品组{0}失败", model.SysNo), LogStatus.系统日志目标类型.商品组展示, Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo);
            }
            return result;
        }

        /// <summary>
        /// 更新商品组状态
        /// </summary>
        /// <param name="collocation">商品组实体</param>
        /// <returns>返回受影响的行</returns>
        /// <remarks>2013-06-21 苟治国 创建</remarks>
        public int UpdateStatus(IList<FeProductGroup> collocation)
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
                            Hyt.BLL.Sys.SyJobPoolPublishBo.Instance.FeAudit(model.SysNo, 0, Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo, "ProductGroup", "修改商品组状态", Hyt.Model.SystemPredefined.Constant.UserSysNo, (int)Hyt.Model.WorkflowStatus.SystemStatus.任务对象类型.通知);
                        }
                        
                        BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, string.Format("修改商品组状态{0}", model.SysNo), LogStatus.系统日志目标类型.商品组展示, Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo);
                    }
                    else
                        BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台, string.Format("修改商品组状态{0}失败", model.SysNo), LogStatus.系统日志目标类型.商品组展示, Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo);
                }

            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        public List<CBFeProductItem> GetModelByGroupSysNo(int groupSysNo)
        {
            return IFeProductGroupDao.Instance.GetModelByGroupSysNo(groupSysNo);
        }

        public List<CBFeProductItem> GetProductInfoList(int levelSysNo, int groupSysNo)
        {
            return IFeProductGroupDao.Instance.GetProductInfoList(levelSysNo, groupSysNo); 
        }
    }
}
