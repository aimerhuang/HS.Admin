using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Extra.SMS;
using Hyt.DataAccess.CRM;
using Hyt.DataAccess.Sys;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;
using Hyt.BLL.Authentication;

namespace Hyt.BLL.CRM
{
    /// <summary>
    /// 短信咨询
    /// </summary>
    /// <remarks>2014-02-21 邵斌 创建</remarks>
    public class CrSmsQuestionBo : BOBase<CrSmsQuestionBo>
    {
        /// <summary>
        /// 创建短信咨询
        /// </summary>
        /// <param name="entity">短信实体</param>
        /// <returns>新增记录系统编号</returns>
        /// <remarks>2014-02-21 邵斌 创建</remarks>
        public int Create(CrSmsQuestion entity)
        {
            return ICrSmsQuestionDao.Instance.Create(entity);
        }

        /// <summary>
        /// 获取咨询短信信息
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>短信实体</returns>
        /// <remarks>2014-02-21 邵斌 创建</remarks>
        public CBCrSmsQuestion Get(int sysNo)
        {
            return ICrSmsQuestionDao.Instance.Get(sysNo);
        }

        /// <summary>
        /// 根据手机号码获取短信咨询
        /// </summary>
        /// <param name="mobileNumber">手机号码</param>
        /// <returns>短信实体列表</returns>
        /// <remarks>2014-02-21 邵斌 创建</remarks>
        public IList<CrSmsQuestion> Get(string mobileNumber)
        {
            return ICrSmsQuestionDao.Instance.Get(mobileNumber);
        }

        /// <summary>
        /// 根据单条短信系统编号获取同一手机的短信咨询列表
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>短信实体列表</returns>
        /// <remarks>2014-02-21 邵斌 创建</remarks>
        public IList<CBCrSmsQuestion> GetSmsQuestionListBySysNo(int sysNo)
        {
            return ICrSmsQuestionDao.Instance.GetSmsQuestionListBySysNo(sysNo);
        }

        /// <summary>
        /// 回复咨询
        /// </summary>
        /// <param name="sysNo">咨询系统编号</param>
        /// <param name="answerBy">回答人</param>
        /// <param name="answerContent">回答内容</param>
        /// <returns>回复结果</returns>
        /// <remarks>2014-02-21 邵斌 创建</remarks>
        public Result Answer(int sysNo, int answerBy, string answerContent)
        {
            Result result = new Result();
            var smsContentObj = ICrSmsQuestionDao.Instance;
            var smsModel = smsContentObj.Get(sysNo);

            result.Status = true;
            result.Status = result.Status && (smsModel != null) && (smsModel.AnswerSysNo == 0) && (smsModel.Status == (int)CustomerStatus.短信咨询状态.待回复);

            if (smsModel.AnswerSysNo > 0)
            {
                result.Message = "咨询状态改变：咨询已经被其他同事回复";
            }

            if (smsModel.Status != (int)CustomerStatus.短信咨询状态.待回复)
            {
                result.Message = "咨询状态为" + ((CustomerStatus.短信咨询状态)smsModel.Status).ToString() + "不能进行回复";
            }

            if (result.Status)
            {
                 result =  smsContentObj.Answer(sysNo, answerBy, answerContent);
            }

            if (result.Status)
            {
                var smsSender = Extra.SMS.SmsProviderFactory.CreateProvider();
                if (smsSender.Send(smsModel.MobilePhoneNumber.Trim(), answerContent + "【品胜当日达】", null).Status == SmsResultStatus.Failue)
                {
                    smsContentObj.UpdateStatus(sysNo, CustomerStatus.短信咨询状态.回复失败);

                    result.Status = false;
                    result.Message = "回复短信无法发送";
                }
            }

            if (result.Status)
            {
                //写日志
                BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, string.Format("回复短信咨询{0}", sysNo),
                                             LogStatus.系统日志目标类型.短信咨询,
                                             sysNo, AdminAuthenticationBo.Instance.Current.Base.SysNo);
            }
            else
            {
                //写错误日志
                BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台, string.Format("回复短信咨询{0}失败", sysNo),
                                             LogStatus.系统日志目标类型.短信咨询,
                                             sysNo, AdminAuthenticationBo.Instance.Current.Base.SysNo);
            }
                
            return result;
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="sysNo">咨询系统编号</param>
        /// <param name="status">便跟状态</param>
        /// <returns>结果</returns>
        /// <remarks>2014-02-21 邵斌 创建</remarks>
        public Result UpdateStatus(int sysNo, CustomerStatus.短信咨询状态 status)
        {
            return ICrSmsQuestionDao.Instance.UpdateStatus(sysNo, status);
        }

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
        public IList<CBCrSmsQuestion> List(int? status, string mobileNumber, string answer, string questionContent,
                                         PagedList<CBCrSmsQuestion> pager)
        {

            Pager<CBCrSmsQuestion> transPager = new Pager<CBCrSmsQuestion>();
            transPager.CurrentPage = pager.CurrentPageIndex;
            transPager.PageSize = pager.PageSize;
            ICrSmsQuestionDao.Instance.List(status, mobileNumber, answer, questionContent, transPager);
            pager.TData = transPager.Rows;
            pager.TotalItemCount = transPager.TotalRows;
            return pager.TData;
        }

        /// <summary>
        /// 读取系统配置的用户短信回复执行人配置列表
        /// </summary>
        /// <returns>返回用户列表</returns>
        /// <remarks>2014-03-05 邵斌 创建 </remarks>
        public IList<SyUser> LoadExecutorList()
        {
            var config = Sys.SyConfigBo.Instance.GetModel("SmsQuestionExecutor", SystemStatus.系统配置类型.常规配置);
            if (config == null)
            {
               config = new SyConfig();
                config.CreatedBy = BLL.Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo;
                config.CategoryId = (int) SystemStatus.系统配置类型.常规配置;
                config.CreatedDate = DateTime.Now;
                config.Description = "客户短信咨询任务回复人列表";
                config.Key = "SmsQuestionExecutor";
                config.LastUpdateBy = config.CreatedBy;
                config.LastUpdateDate = config.CreatedDate;
                Sys.SyConfigBo.Instance.Create(config);
            }

            List<int> intSysNoList = new List<int>();
            IList<SyUser> result = new List<SyUser>();
            if (!string.IsNullOrWhiteSpace(config.Value))
            {
                var userSysNoList = config.Value.Split(',').ToList();
                foreach (var sysNo in userSysNoList)
                {
                    intSysNoList.Add(int.Parse(sysNo));
                }
                result = ISyUserDao.Instance.GetSyUser(intSysNoList);
            }
            return result;
        } 


        /// <summary>
        /// 保存系统配置的用户短信回复执行人配置列表
        /// </summary>
        /// <param name="value">参数值</param>
        /// <returns>返回: true 成功 false 失败</returns>
        /// <remarks>2014-03-05 邵斌 创建</remarks>
        public bool SaveExecutorSetting(string value)
        {
            int result = 0;
            var config = Sys.SyConfigBo.Instance.GetModel("SmsQuestionExecutor", SystemStatus.系统配置类型.常规配置);
            if (config == null)
            {
                config = new SyConfig();
                config.CreatedBy = AdminAuthenticationBo.Instance.Current.Base.SysNo;
                config.CategoryId = (int) SystemStatus.系统配置类型.常规配置;
                config.CreatedDate = DateTime.Now;
                config.Description = "客户短信咨询任务回复人列表";
                config.Key = "SmsQuestionExecutor";
                config.LastUpdateBy = config.CreatedBy;
                config.LastUpdateDate = config.CreatedDate;
                config.Value = value;
                result = Sys.SyConfigBo.Instance.Create(config);
            }
            else
            {
                config.Value = value;
                config.LastUpdateBy = AdminAuthenticationBo.Instance.Current.Base.SysNo;
                config.LastUpdateDate = DateTime.Now;
                result = Sys.SyConfigBo.Instance.Update(config);
            }

            return result > 0;
        }
    }
}
