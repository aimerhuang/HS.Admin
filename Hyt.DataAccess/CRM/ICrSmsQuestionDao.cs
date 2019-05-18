using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.CRM
{
    /// <summary>
    /// 短信咨询
    /// </summary>
    /// <remarks>2014-02-21 邵斌 创建</remarks>
    public abstract class ICrSmsQuestionDao : DaoBase<ICrSmsQuestionDao>
    {
        /// <summary>
        /// 创建短信咨询
        /// </summary>
        /// <param name="entity">短信实体</param>
        /// <returns>新增记录系统编号</returns>
        /// <remarks>2014-02-21 邵斌 创建</remarks>
        public abstract int Create(CrSmsQuestion entity);

        /// <summary>
        /// 获取咨询短信信息
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>短信实体</returns>
        /// <remarks>2014-02-21 邵斌 创建</remarks>
        public abstract CBCrSmsQuestion Get(int sysNo);

        /// <summary>
        /// 根据手机号码获取短信咨询
        /// </summary>
        /// <param name="mobileNumber">手机号码</param>
        /// <returns>短信实体列表</returns>
        /// <remarks>2014-02-21 邵斌 创建</remarks>
        public abstract IList<CrSmsQuestion> Get(string mobileNumber);

        /// <summary>
        /// 根据单条短信系统编号获取同一手机的短信咨询列表
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>短信实体列表</returns>
        /// <remarks>2014-02-21 邵斌 创建</remarks>
        public abstract IList<CBCrSmsQuestion> GetSmsQuestionListBySysNo(int sysNo);

        /// <summary>
        /// 回复咨询
        /// </summary>
        /// <param name="sysNo">咨询系统编号</param>
        /// <param name="answerBy">回答人</param>
        /// <param name="answerContent">回答内容</param>
        /// <returns>回复结果</returns>
        /// <remarks>2014-02-21 邵斌 创建</remarks>
        public abstract Result Answer(int sysNo, int answerBy, string answerContent);

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="sysNo">咨询系统编号</param>
        /// <param name="status">便跟状态</param>
        /// <returns>结果</returns>
        /// <remarks>2014-02-21 邵斌 创建</remarks>
        public abstract Result UpdateStatus(int sysNo, CustomerStatus.短信咨询状态 status);

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="status">咨询状态</param>
        /// <param name="mobileNumber">咨询人电话</param>
        /// <param name="questionContent">咨询内容</param>
        /// <param name="answer">回复人</param>
        /// <param name="pager">分页对象</param>
        /// <returns>咨询列表</returns>
        /// <remarks>2014-02-21 邵斌 创建</remarks>
        public abstract IList<CBCrSmsQuestion> List(int? status, string mobileNumber, string questionContent, string answer, Pager<CBCrSmsQuestion> pager);
    }
}
