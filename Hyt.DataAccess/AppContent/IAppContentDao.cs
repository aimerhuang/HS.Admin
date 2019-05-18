using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.B2CApp;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.AppContent
{
    /// <summary>
    /// APP内容管理
    /// </summary>
    /// <remarks>2013-9-4 黄伟 创建</remarks>
    public abstract class IAppContentDao : DaoBase<IAppContentDao>
    {
        #region 商品浏览历史

        /// <summary>
        /// 删除用户产品浏览历史
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <returns>受影响行</returns>
        /// <remarks>2013-09-05 周唐炬 创建</remarks>
        public abstract int DeleteHistory(int customerSysNo);

        /// <summary>
        /// 客户商品浏览历史记录查询
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="customerLevel">客户等级</param>
        /// <returns>客户商品浏览历史记录查询列表</returns>
        /// <remarks>2013-09-05 周唐炬 创建</remarks>
        public abstract IList<SimplProduct> GetProBroHistory(int customerSysNo, int customerLevel);

        /// <summary>
        /// 商品浏览历史记录查询
        /// </summary>
        /// <param name="para">CBCrBrowseHistory</param>
        /// <param name="currPageIndex">当前页索引</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <returns>Dictionary-CBCrBrowseHistory</returns>
        /// <remarks>2013-9-4 黄伟 创建</remarks>
        public abstract Dictionary<int, IList<CBCrBrowseHistory>> QueryProBroHistory(CBCrBrowseHistory para, int currPageIndex=1, int pageSize=10);

        /// <summary>
        /// 删除浏览历史记录
        /// </summary>
        /// <param name="lstDelSysnos">要删除的历史记录编号集合</param>
        /// <returns></returns>
        /// <remarks>2013-9-4 黄伟 创建</remarks>
        public abstract void DeleteBrowseHistory(List<int> lstDelSysnos);

        #endregion

        #region app版本管理

        /// <summary>
        /// app版本管理
        /// </summary>
        /// <param name="para">CBApVersion</param>
        /// <param name="currPageIndex">当前页索引</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <returns>Dictionary-CBApVersion</returns>
        /// <remarks>2013-9-9 黄伟 创建</remarks>
        public abstract Dictionary<int, IList<CBApVersion>> QueryAppVersion(CBApVersion para, int currPageIndex = 1, int pageSize = 10);

        /// <summary>
        /// 删除版本
        /// </summary>
        /// <param name="lstDelSysNos">要删除的版本编号集合</param>
        /// <returns></returns>
        /// <remarks>2013-9-10 黄伟 创建</remarks>
        public abstract void DeleteVersion(List<int> lstDelSysNos);

        /// <summary>
        /// 新增版本
        /// </summary>
        /// <param name="model">CBApVersion</param>
        /// <param name="operatorSysNo">操作人员编号</param> 
        /// <returns>Result instance</returns>
        /// <remarks>2013-9-10 黄伟 创建</remarks>
        public abstract int CreateVersion(CBApVersion model, int operatorSysNo);

        /// <summary>
        /// 更新版本
        /// </summary>
        /// <param name="operatorSysNo">操作人员编号</param>
        /// <param name="model">CBApVersion</param>
        /// <returns></returns>
        /// <remarks>2013-9-10 黄伟 创建</remarks>
        public abstract int UpdateVersion(CBApVersion model, int operatorSysNo);

        /// <summary>
        /// 根据App代码分组获取最新版本
        /// </summary>
        /// <returns>最新版本列表</returns>
        /// <remarks>
        /// 2013-10-24 郑荣华 创建
        /// </remarks>
        public abstract IList<CBApVersion> GetLastAppVersion();

        /// <summary>
        /// 获取版本
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>版本</returns>
        /// <remarks>
        /// 2013-11-27 郑荣华 创建
        /// </remarks>
        public abstract CBApVersion GetAppVersion(int sysNo);

        #endregion

        #region App推送服务

        /// <summary>
        /// 创建推送消息对象
        /// </summary>
        /// <param name="model">消息对象</param>
        /// <returns>返回 true:成功 false:失败</returns>
        /// <remarks>2014-01-14 邵斌 创建</remarks>
        public abstract bool CreateApPushService(CBApPushService model);

        /// <summary>
        /// 获取单个推送服务对象
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>返回推送对象模型</returns>
        /// <remarks>2014-01-14 邵斌 创建</remarks>
        public abstract CBApPushService GetApPushService(int sysNo);

        /// <summary>
        /// 更新推送消息对象
        /// </summary>
        /// <param name="model">消息对象</param>
        /// <returns>返回 true:更新成功 false:更新失败</returns>
        /// <remarks>2014-01-14 邵斌 创建</remarks>
        public abstract bool UpdateApPushService(CBApPushService model);

        /// <summary>
        /// 更新推送消息状态
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <param name="status">更新状态</param>
        /// <param name="updateBy">更新人</param>
        /// <returns>返回 true：成功 false：失败</returns>
        /// <remarks>2014-01-14 邵斌 创建</remarks>
        public abstract bool UpdateApPushServiceStatus(int sysNo, AppStatus.App推送服务状态 status,int updateBy);

        /// <summary>
        /// 读取推送消息列表
        /// </summary>
        /// <param name="para">分页分页参数，并返回结果到对象</param>
        /// <returns></returns>
        /// <remarks>2014-01-14 邵斌 创建</remarks>
        public abstract void GetApPushService(ref Pager<CBApPushService> para);

        #endregion
    }
}
