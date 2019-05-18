using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using Hyt.BLL.Log;
using Hyt.BLL.Sys;
using Hyt.DataAccess.AppContent;
using Hyt.DataAccess.CRM;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.B2CApp;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.BLL.Authentication;

namespace Hyt.BLL.AppContent
{
    /// <summary>
    /// AppContentBoa-app内容管理bo
    /// </summary>
    /// <remarks>2013-09-04 黄伟 创建</remarks>
    public class AppContentBo : BOBase<AppContentBo>
    {
        /// <summary>
        /// 删除用户产品浏览历史
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <returns>受影响行</returns>
        /// <remarks>2013-09-05 周唐炬 创建</remarks>
        public int DeleteHistory(int customerSysNo)
        {
            return IAppContentDao.Instance.DeleteHistory(customerSysNo);
        }

        /// <summary>
        /// 客户商品浏览历史记录查询
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <returns>客户商品浏览历史记录查询列表</returns>
        /// <remarks>2013-09-05 周唐炬 创建</remarks>
        public IList<SimplProduct> GetProBroHistory(int customerSysNo)
        {
            var customer = ICrCustomerDao.Instance.GetCrCustomerItem(customerSysNo);
            var customerLevel = Model.SystemPredefined.CustomerLevel.初级;; //会员默认等级
            if (null != customer)
            {
                customerLevel = customer.LevelSysNo; //加入会员等级
            }
            var list = IAppContentDao.Instance.GetProBroHistory(customerSysNo, customerLevel);
            foreach (var item in list)
            {
                item.Thumbnail = Web.ProductImageBo.Instance.GetProductImagePath(Web.ProductThumbnailType.Image180, item.SysNo);
            }
            return list;
        }

        /// <summary>
        /// 商品浏览历史记录查询
        /// </summary>
        /// <param name="para">CBCrBrowseHistory</param>
        /// <param name="currPageIndex">当前页索引</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <returns>Pager-CBCrBrowseHistory分页对象</returns>
        /// <remarks>2013-9-4 黄伟 创建</remarks>
        public Dictionary<int, IList<CBCrBrowseHistory>> QueryProBroHistory(CBCrBrowseHistory para, int currPageIndex = 1, int pageSize = 10)
        {
            return IAppContentDao.Instance.QueryProBroHistory(para, currPageIndex, pageSize);
        }

        /// <summary>
        /// 删除浏览历史记录
        /// </summary>
        /// <param name="lstDelSysnos">要删除的历史记录编号集合</param>
        /// <returns></returns>
        /// <remarks>2013-9-4 黄伟 创建</remarks>
        public void DeleteBrowseHistory(List<int> lstDelSysnos)
        {
            IAppContentDao.Instance.DeleteBrowseHistory(lstDelSysnos);
        }

        /// <summary>
        /// app版本管理
        /// </summary>
        /// <param name="para">CBApVersion</param>
        /// <param name="currPageIndex">当前页索引</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <returns>Pager-CBCrBrowseHistory分页对象</returns>
        /// <remarks>2013-9-4 黄伟 创建</remarks>
        public Dictionary<int, IList<CBApVersion>> QueryAppVersion(CBApVersion para, int currPageIndex = 1, int pageSize = 10)
        {
            return IAppContentDao.Instance.QueryAppVersion(para, currPageIndex, pageSize);
        }

        /// <summary>
        /// 删除版本
        /// </summary>
        /// <param name="lstDelSysnos">要删除的版本编号集合</param>
        /// <param name="userIp">访问者ip</param>
        /// <param name="operatorSysno">操作人员编号</param>
        /// <returns>Result instance</returns>
        /// <remarks>2013-9-10 黄伟 创建</remarks>
        public Result DeleteVersion(List<int> lstDelSysnos, string userIp, int operatorSysno)
        {
            try
            {
               
                    IAppContentDao.Instance.DeleteVersion(lstDelSysnos);
                    SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "删除App版本",
                                             LogStatus.系统日志目标类型.App版本, 0, null, userIp, operatorSysno);
                
            }
            catch (Exception ex)
            {

                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "删除App版本",
                                         LogStatus.系统日志目标类型.App版本, 0, ex, userIp, operatorSysno);
                return new Result { Status = false, Message = string.Format("删除失败:{0}", ex.Message) };
            }

            return new Result { Status = true, Message = "操作成功!" };
        }

        /// <summary>
        /// 新增版本
        /// </summary>
        /// <param name="model">CBApVersion</param>
        /// <param name="userIp">访问者ip</param>
        /// <param name="operatorSysno">操作人员编号</param>
        /// <returns>Result instance</returns>
        /// <remarks>2013-9-10 黄伟 创建</remarks>
        public Result CreateVersion(CBApVersion model, string userIp, int operatorSysno)
        {
            try
            {
                
                    var sysNo = IAppContentDao.Instance.CreateVersion(model, operatorSysno);
                    SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "创建App版本",
                                             LogStatus.系统日志目标类型.App版本, sysNo, null, userIp, operatorSysno);
                
            }
            catch (Exception ex)
            {

                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "创建App版本",
                                         LogStatus.系统日志目标类型.App版本, 0, ex, userIp, operatorSysno);
                return new Result { Status = false, Message = string.Format("保存失败:{0}", ex.Message) };
            }

            return new Result { Status = true, Message = "保存成功!" };
        }

        /// <summary>
        /// 更新版本
        /// </summary>
        /// <param name="model">CBApVersion</param>
        /// <param name="userIp">访问者ip</param>
        /// <param name="operatorSysno">操作人员编号</param>
        /// <returns></returns>
        /// <remarks>2013-9-10 黄伟 创建</remarks>
        public Result UpdateVersion(CBApVersion model, string userIp, int operatorSysno)
        {
            try
            {
               
                    var sysNo = IAppContentDao.Instance.UpdateVersion(model, operatorSysno);
                    SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "更新App版本",
                                             LogStatus.系统日志目标类型.App版本, sysNo, null, userIp, operatorSysno);
                
            }
            catch (Exception ex)
            {
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "更新App版本",
                                         LogStatus.系统日志目标类型.App版本, 0, ex, userIp, operatorSysno);
                return new Result { Status = false, Message = string.Format("更新失败:{0}", ex.Message) };
            }

            return new Result { Status = true, Message = "更新成功!" };
        }

        /// <summary>
        /// 根据App代码分组获取最新版本
        /// </summary>
        /// <returns>最新版本列表</returns>
        /// <remarks>
        /// 2013-10-24 郑荣华 创建
        /// </remarks>
        public IList<CBApVersion> GetLastAppVersion()
        {
            return IAppContentDao.Instance.GetLastAppVersion();
        }

        /// <summary>
        /// 获取版本
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>版本</returns>
        /// <remarks>
        /// 2013-11-27 郑荣华 创建
        /// </remarks>
        public CBApVersion GetAppVersion(int sysNo)
        {
            return IAppContentDao.Instance.GetAppVersion(sysNo); 
        }

        #region APP推送服务

        /// <summary>
        /// 创建推送消息对象
        /// </summary>
        /// <param name="model">消息对象</param>
        /// <returns>返回 true:成功 false:失败</returns>
        /// <remarks>2014-01-14 邵斌 创建</remarks>
        public bool CreateApPushService(CBApPushService model)
        {
            var result =  IAppContentDao.Instance.CreateApPushService(model);
            if (result)
            {
                //写日志
                BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, string.Format("新建推送消息{0}", model.SysNo),
                                             LogStatus.系统日志目标类型.推送信息,
                                             model.SysNo, (Authentication.AdminAuthenticationBo.Instance.Current == null ? 12 : Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo));
            }
            else
            {
                //写错误日志
                BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台, "新建推送消息失败",
                                             LogStatus.系统日志目标类型.推送信息,
                                             model.SysNo, (Authentication.AdminAuthenticationBo.Instance.Current == null ? 12 : Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo));
            }

            return result;
        }

        /// <summary>
        /// 获取单个推送服务对象
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>返回推送对象模型</returns>
        /// <remarks>2014-01-14 邵斌 创建</remarks>
        public CBApPushService GetApPushService(int sysNo)
        {
            return IAppContentDao.Instance.GetApPushService(sysNo);
        }

        /// <summary>
        /// 更新推送消息对象
        /// </summary>
        /// <param name="model">消息对象</param>
        /// <returns>返回 true:更新成功 false:更新失败</returns>
        /// <remarks>2014-01-14 邵斌 创建</remarks>
        public bool UpdateApPushService(CBApPushService model)
        {
            var result = IAppContentDao.Instance.UpdateApPushService(model);

            if (result)
            {
                //写日志
                BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, string.Format("修改推送消息{0}", model.SysNo),
                                             LogStatus.系统日志目标类型.推送信息,
                                             model.SysNo, AdminAuthenticationBo.Instance.Current.Base.SysNo);
            }
            else
            {
                //写错误日志
                BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台, string.Format("修改推送消息{0}失败", model.SysNo),
                                             LogStatus.系统日志目标类型.推送信息,
                                             model.SysNo, AdminAuthenticationBo.Instance.Current.Base.SysNo);
            }

            return result;
        }

        /// <summary>
        /// 更新推送消息状态
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <param name="status">更新状态</param>
        /// <param name="updateBy">更新人</param>
        /// <returns>返回 true：成功 false：失败</returns>
        /// <remarks>2014-01-14 邵斌 创建</remarks>
        public bool UpdateApPushServiceStatus(int sysNo, AppStatus.App推送服务状态 status, int updateBy)
        {
            var result = IAppContentDao.Instance.UpdateApPushServiceStatus(sysNo, status, updateBy);

            if (result)
            {
                //写日志
                BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, string.Format("推送信息状态变更为{0}", status.ToString()),
                                             LogStatus.系统日志目标类型.推送信息,
                                             sysNo, AdminAuthenticationBo.Instance.Current.Base.SysNo);
            }
            else
            {
                //写错误日志
                BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台, string.Format("推送信息状态变更为{0}失败", status.ToString()),
                                             LogStatus.系统日志目标类型.推送信息,
                                             sysNo, AdminAuthenticationBo.Instance.Current.Base.SysNo);
            }

            return result;
        }

        /// <summary>
        /// 读取推送消息列表
        /// </summary>
        /// <param name="para">分页分页参数，并返回结果到对象</param>
        /// <returns></returns>
        /// <remarks>2014-01-14 邵斌 创建</remarks>
        public void GetApPushService(ref PagedList<CBApPushService>  pageList,CBApPushService fileter){

            Pager<CBApPushService> para = new Pager<CBApPushService>();
            para.CurrentPage = pageList.CurrentPageIndex;
            para.PageSize = pageList.PageSize;
            para.PageFilter = fileter;

            IAppContentDao.Instance.GetApPushService(ref para);

            pageList.TData = para.Rows;
            pageList.TotalItemCount = para.TotalRows;
        }

        #endregion
    }
}
