using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.DataAccess.Front;
using Hyt.Infrastructure.Pager;
using Hyt.Model.WorkflowStatus;

namespace Hyt.BLL.Front
{
    /// <summary>
    /// 友情链接业务逻辑层
    /// </summary>
    /// <remarks>2013－12 - 09 苟治国 创建</remarks>
    public class MkBlogrollBo : BOBase<MkBlogrollBo>
    {
        /// <summary>
        /// 查看友情链接
        /// </summary>
        /// <param name="sysNo">友情链接编号</param>
        /// <returns>友情链接</returns>
        /// <remarks>2013－12-09 苟治国 创建</remarks>
        public Model.MkBlogroll GetModel(int sysNo)
        {
            return IMkBlogrollDao.Instance.GetModel(sysNo);
        }

        /// <summary>
        /// 判断友情连接标题是否重复
        /// </summary>
        /// <param name="key">友情连接标题</param>
        /// <param name="sysNo">编号</param>
        /// <returns>重复为true,否则为false</returns>
        /// <remarks>2013－12-09 苟治国 创建</remarks>
        public bool Verify(string key, int sysNo)
        {
            return IMkBlogrollDao.Instance.Verify(key,sysNo);
        }

        /// <summary>
        /// 根据条件获取友情链接列表
        /// </summary>
        /// <param name="id">索引</param>
        /// <param name="status">状态:待审(10),已审(20),作废(-10)</param>
        /// <param name="webSiteName">网站名称</param>
        /// <returns>友情链接列表</returns>
        /// <remarks>2013－12-09 苟治国 创建</remarks>
        public PagedList<Model.MkBlogroll> Seach(int id, int? status, string webSiteName = null)
        {
            if (status == null){
                status = -1;
            }else{
                status = status ?? -1;
            }

            var pager = new Pager<MkBlogroll>()
            {
                CurrentPage = id,
                PageFilter = new MkBlogroll()
                {
                    Status = (int)status,
                    WebSiteName = webSiteName
                },
            };

            var list = new PagedList<MkBlogroll>();
            pager.PageSize = list.PageSize;
            pager = IMkBlogrollDao.Instance.Seach(pager);

            list = new PagedList<MkBlogroll>
            {
                TData = pager.Rows,
                Data = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows,
                Style = PagedList.StyleEnum.Default
            };
            return list;
        }

        /// <summary>
        /// 插入友情链接
        /// </summary>
        /// <param name="model">友情链接明细</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－12-09 苟治国 创建</remarks>
        public int Insert(Model.MkBlogroll model)
        {
            int result = IMkBlogrollDao.Instance.Insert(model);
            if (result > 0)
            {
                BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, string.Format("添加友情链接{0}", model.WebSiteName), LogStatus.系统日志目标类型.用户, Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo);
            }
            else
            {
                BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台, string.Format("添加友情链接{0}失败", model.WebSiteName), LogStatus.系统日志目标类型.用户, Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo);
            }
            return result;
        }

        /// <summary>
        /// 更新友情链接
        /// </summary>
        /// <param name="model">更新友情链接</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－12-09 苟治国 创建</remarks>
        public int Update(Model.MkBlogroll model)
        {
            int result = IMkBlogrollDao.Instance.Update(model);
            if (result > 0)
            {
                BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, string.Format("更新添加友情链接{0}", model.WebSiteName), LogStatus.系统日志目标类型.用户, Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo);
            }
            else
            {
                BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台, string.Format("更新友情链接{0}失败", model.WebSiteName), LogStatus.系统日志目标类型.用户, Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo);
            }
            return result;
        }

        /// <summary>
        /// 更新友情链接状态
        /// </summary>
        /// <param name="collocation">友情链接实体</param>
        /// <param name="auditorSysNo">编号</param>
        /// <returns>返回受影响的行</returns>
        /// <remarks>2013－12-09 苟治国 创建</remarks>
        public int UpdateStatus(IList<Model.MkBlogroll> collocation, int auditorSysNo)
        {
            int result = 0;
            try
            {
                for (int i = 0; i < collocation.Count; i++)
                {
                    int sysNo = Convert.ToInt32(collocation[i].SysNo);
                    int status = Convert.ToInt32(collocation[i].Status);

                    var model = this.GetModel(sysNo);
                    if (model.Status != (int)Hyt.Model.WorkflowStatus.MarketingStatus.友情链接管理状态.作废)
                    {
                        model.Status = status;
                        model.AuditorSysNo = auditorSysNo;
                        model.AuditDate = DateTime.Now;
                        model.LastUpdateBy = auditorSysNo;
                        model.LastUpdateDate = DateTime.Now;
                        result = this.Update(model);
                    }
                    BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, string.Format("更新友情链接{0}状态失败", model.WebSiteName), LogStatus.系统日志目标类型.用户, Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo);
                }

            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }
    }
}
