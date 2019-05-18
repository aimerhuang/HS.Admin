using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.DataAccess.Base;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Weixin
{
 
    /// <summary>
    /// 微信咨询抽象类
    /// </summary>
    /// <remarks>
    /// 2013-07-16 郑荣华 创建
    /// </remarks>
    public abstract class IMkWeixinQuestionDao : DaoBase<IMkWeixinQuestionDao>
    {

        #region 操作

        /// <summary>
        /// 添加微信咨询信息
        /// </summary>
        /// <param name="model">微信咨询实体类</param>
        /// <returns>返回新增的系统编号</returns>
        /// <remarks>
        /// 2013-11-07 郑荣华 创建
        /// </remarks>
        public abstract int Create(MkWeixinQuestion model);

        /// <summary>
        /// 修改微信咨询信息
        /// </summary>
        /// <param name="model">微信咨询实体类</param>
        /// <returns>受影响的行数</returns>
        /// <remarks>
        /// 2013-11-07 郑荣华 创建
        /// </remarks>
        public abstract int Update(MkWeixinQuestion model);

        /// <summary>
        /// 微信咨询消息状态更新
        /// </summary>
        /// <param name="weixinId">微信号</param>
        /// <param name="status">微信咨询消息状态</param>
        /// <returns>受影响的行数</returns>
        /// <remarks>
        /// 2013-11-07 郑荣华 创建
        /// </remarks>
        public abstract int UpdateStatus(string weixinId, MarketingStatus.微信咨询消息状态 status);

        /// <summary>
        /// 删除微信咨询信息
        /// </summary>
        /// <param name="templateSysNo">微信咨询系统编号</param>
        /// <returns>受影响的行数</returns>
        /// <remarks>
        /// 2013-11-07 郑荣华 创建
        /// </remarks>
        public abstract int Delete(int templateSysNo);

        #endregion

        #region 查询

        /// <summary>
        /// 根据微信号获取微信咨询信息
        /// </summary>
        /// <param name="pager">微信咨询列表分页对象</param>
        /// <param name="weixinId">微信号</param>
        /// <returns>微信咨询列表信息</returns>
        /// <remarks>
        /// 2013-11-07 郑荣华 创建
        /// </remarks>
        public abstract void GetMkWeixinQuestionList(ref Pager<CBMkWeixinQuestion> pager, string weixinId);

        /// <summary>
        /// 分页查询微信咨询信息（统计）列表
        /// </summary>
        /// <param name="pager">微信咨询列表分页对象</param>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        /// <remarks> 
        /// 2013-11-07 郑荣华 创建
        /// </remarks>
        public abstract void GetMkWeixinQuestionStaticsList(ref Pager<CBMkWeixinQuestion> pager, ParaMkWeixinQuestionFilter filter);

        #endregion
    }
}
