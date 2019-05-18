using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.BLL.Front;
using Hyt.BLL.Order;
using Hyt.DataAccess.Order;
using Hyt.DataAccess.Sys;
using Hyt.Model;
using Hyt.Model.SystemPredefined;
using Hyt.Model.WorkflowStatus;

namespace Hyt.BLL.Sys
{
    /// <summary>
    /// 发布任务
    /// </summary>
    /// <remarks>2013-09-27 吴文强 创建</remarks>
    public class SyJobPoolPublishBo : BOBase<SyJobPoolPublishBo>
    {
        /// <summary>
        /// 订单审核任务
        /// [订单创建成功后调用]
        /// </summary>
        /// <param name="orderSysNo">订单系统编号</param>
        /// <param name="customerName">客户姓名</param>
        /// <param name="mobilePhoneNumber">客户手机号</param>
        /// <param name="executorSysNo">任务执行者编号</param>
        /// <param name="oldTaskType">旧任务类型（如不为空则需删除旧任务）</param>
        /// <param name="priority">优先级</param>
        /// <returns>任务</returns>
        /// <remarks>2013-09-27 吴文强 创建</remarks>
        /// <remarks>2013-11-29 余勇 修改</remarks>
        private SyJobPool OrderAudit(int orderSysNo, string customerName, string mobilePhoneNumber,
                                    int? executorSysNo = null, int? oldTaskType = null, int? priority = null)
        {
            const string url = "/Order/OrderDetail/{0}";
            const string description = "订单号:{0}{1}{2}";
            const SystemStatus.任务对象类型 taskType = SystemStatus.任务对象类型.客服订单审核;
            customerName = !string.IsNullOrEmpty(customerName) ? "," + customerName : "";
            mobilePhoneNumber = !string.IsNullOrEmpty(mobilePhoneNumber) ? "," + mobilePhoneNumber : "";
            var jobDescription = string.Format(description, orderSysNo, customerName, mobilePhoneNumber);
            var jobUrl = string.Format(url, orderSysNo);
            if (oldTaskType != null && oldTaskType.Value > 0)
            {
                //当优先级参数为空时，继承旧任务的优先级
                if (priority == null)
                {
                    var job = ISyJobPoolDao.Instance.GetByTask(orderSysNo, oldTaskType.Value);
                    if (job != null)
                    {
                        priority = job.Priority;
                    }
                }
                SyJobPoolManageBo.Instance.DeleteJobPool(orderSysNo, oldTaskType.Value);
            }

            return CreateJobPool(jobDescription, jobUrl, taskType, orderSysNo, executorSysNo, priority);
        }

        /// <summary>
        /// 订单审核任务
        /// [订单创建成功后调用]
        /// </summary>
        /// <param name="orderSysNo">订单系统编号</param>
        /// <param name="executorSysNo">任务执行者编号</param>
        /// <param name="oldTaskType">旧任务类型（如不为空则需删除旧任务）</param>
        /// <param name="priority">优先级</param>
        /// <returns>任务</returns>
        /// <remarks>2013-09-28 余勇 创建</remarks>
        public SyJobPool OrderAuditBySysNo(int orderSysNo,
                                    int? executorSysNo = null, int? oldTaskType = null, int? priority = null)
        {
            SyJobPool job = null;
            var model = SoOrderBo.Instance.GetEntity(orderSysNo);
            if (model != null)
            {
                var customer = Hyt.DataAccess.CRM.ICrCustomerDao.Instance.GetCrCustomerItem(model.CustomerSysNo);
                if (customer != null)
                {
                    int sysPriority = priority.HasValue ? priority.Value : GetPriorityByOrder(model);
                    job = OrderAudit(orderSysNo, customer.Name, customer.MobilePhoneNumber, executorSysNo, oldTaskType, sysPriority);

                }
            }
            return job;
        }

        /// <summary>
        /// 根据订单信息取得优先级
        /// </summary>
        /// <param name="order">订单实体</param>
        /// <returns>优先级</returns>
        private int GetPriorityByOrder(SoOrder order)
        {
            int priority = SyJobPoolPriorityBo.Instance.GetPriorityByCode(JobPriorityCode.PC001.ToString());

            if (order.DeliveryTypeSysNo == (int)DeliveryType.门店自提)
            {
                priority = SyJobPoolPriorityBo.Instance.GetPriorityByCode(JobPriorityCode.PC050.ToString());
            }
            else if (order.DeliveryTypeSysNo == (int)DeliveryType.第三方快递)
            {
                if (order.ImgFlag == MallTypeFlag.天猫商城旗舰店)
                {
                    priority = SyJobPoolPriorityBo.Instance.GetPriorityByCode(JobPriorityCode.PC025.ToString());
                }
                else
                {
                    priority = SyJobPoolPriorityBo.Instance.GetPriorityByCode(JobPriorityCode.PC020.ToString());
                }
            }
            else if (order.DeliveryTypeSysNo == (int)DeliveryType.百城当日达 ||
                        order.DeliveryTypeSysNo == (int)DeliveryType.定时百城当日达 ||
                        order.DeliveryTypeSysNo == (int)DeliveryType.加急百城当日达 ||
                        order.DeliveryTypeSysNo == (int)DeliveryType.普通百城当日达)
            {
                if (order.ImgFlag == MallTypeFlag.天猫商城旗舰店)
                {
                    priority = SyJobPoolPriorityBo.Instance.GetPriorityByCode(JobPriorityCode.PC035.ToString());
                }
                else
                {
                    priority = SyJobPoolPriorityBo.Instance.GetPriorityByCode(JobPriorityCode.PC030.ToString());
                }
            }

            return priority;
        }

        /// <summary>
        /// 订单待创建出库单
        /// [订单审核成功后调用]
        /// </summary>
        /// <param name="orderSysNo">订单系统编号</param>
        /// <param name="customerName">客户姓名</param>
        /// <param name="mobilePhoneNumber">客户手机号</param>
        /// <param name="executorSysNo">任务执行者编号</param>
        /// <param name="oldTaskType">旧任务类型（如不为空则需删除旧任务）</param>
        /// <param name="priority">优先级</param>
        /// <returns>任务</returns>
        /// <remarks>2013-09-27 吴文强 创建</remarks>
        public SyJobPool OrderWaitStockOut(int orderSysNo, string customerName, string mobilePhoneNumber,
                                    int? executorSysNo = null, int? oldTaskType = null, int? priority = null)
        {
            const string url = "/Order/OrderDetail/{0}";
            const string description = "订单号:{0}{1}{2}";
            const SystemStatus.任务对象类型 taskType = SystemStatus.任务对象类型.客服订单提交出库;
            customerName = !string.IsNullOrEmpty(customerName) ? "," + customerName : "";
            mobilePhoneNumber = !string.IsNullOrEmpty(mobilePhoneNumber) ? "," + mobilePhoneNumber : "";
            var jobDescription = string.Format(description, orderSysNo, customerName, mobilePhoneNumber);
            var jobUrl = string.Format(url, orderSysNo);
            if (oldTaskType != null && oldTaskType.Value > 0)
            {
                //当优先级参数为空时，继承旧任务的优先级
                if (priority == null)
                {
                    var job = ISyJobPoolDao.Instance.GetByTask(orderSysNo, oldTaskType.Value);
                    if (job != null)
                    {
                        priority = job.Priority;
                    }
                }
                SyJobPoolManageBo.Instance.DeleteJobPool(orderSysNo, oldTaskType.Value);
            }
            return CreateJobPool(jobDescription, jobUrl, taskType, orderSysNo, executorSysNo, priority);
        }

        /// <summary>
        /// 商品评论审核
        /// [商品评论审核成功后调用]
        /// </summary>
        /// <param name="commentSysNo">商品评论编号</param>
        /// <param name="customerName">客户姓名</param>
        /// <param name="mobilePhoneNumber">客户手机号</param>
        /// <param name="executorSysNo">任务执行者编号</param>
        /// <param name="oldTaskType">旧任务类型（如不为空则需删除旧任务）</param>
        /// <param name="priority">优先级</param>
        /// <returns>任务</returns>
        /// <remarks>2013-11-29 余勇 创建</remarks>
        public SyJobPool ProductCommentAudit(int commentSysNo, string customerName, string mobilePhoneNumber,
                                    int? executorSysNo = null, int? oldTaskType = null, int? priority = null)
        {
            const string url = "/Front/FeProductCommentAudit/?commentSysNo={0}";
            const string description = "评论号:{0}{1}{2}";
            const SystemStatus.任务对象类型 taskType = SystemStatus.任务对象类型.商品评论审核;
            customerName = !string.IsNullOrEmpty(customerName) ? "," + customerName : "";
            mobilePhoneNumber = !string.IsNullOrEmpty(mobilePhoneNumber) ? "," + mobilePhoneNumber : "";
            var jobDescription = string.Format(description, commentSysNo, customerName, mobilePhoneNumber);
            var jobUrl = string.Format(url, commentSysNo);
            if (oldTaskType != null && oldTaskType.Value > 0)
            {
                //当优先级参数为空时，继承旧任务的优先级
                if (priority == null)
                {
                    var job = ISyJobPoolDao.Instance.GetByTask(commentSysNo, oldTaskType.Value);
                    if (job != null)
                    {
                        priority = job.Priority;
                    }
                }
                SyJobPoolManageBo.Instance.DeleteJobPool(commentSysNo, oldTaskType.Value);
            }
            return CreateJobPool(jobDescription, jobUrl, taskType, commentSysNo, executorSysNo, priority);
        }
        /// <summary>
        /// 商品评论回复审核
        /// [商品评论回复审核成功后调用]
        /// </summary>
        /// <param name="commentSysNo">商品评论编号</param>
        /// <param name="customerName">客户姓名</param>
        /// <param name="mobilePhoneNumber">客户手机号</param>
        /// <param name="executorSysNo">任务执行者编号</param>
        /// <param name="oldTaskType">旧任务类型（如不为空则需删除旧任务）</param>
        /// <param name="priority">优先级</param>
        /// <returns>任务</returns>
        /// <remarks>2013-11-29 余勇 创建</remarks>
        public SyJobPool ProductCommentReplayAudit(int commentSysNo, string customerName, string mobilePhoneNumber,
                                    int? executorSysNo = null, int? oldTaskType = null, int? priority = null)
        {
            const string url = "/Front/FeProductCommentAudit/?commentSysNo={0}";
            const string description = "评论号:{0}{1}{2}";
            const SystemStatus.任务对象类型 taskType = SystemStatus.任务对象类型.商品评论回复审核;
            customerName = !string.IsNullOrEmpty(customerName) ? "," + customerName : "";
            mobilePhoneNumber = !string.IsNullOrEmpty(mobilePhoneNumber) ? "," + mobilePhoneNumber : "";
            var jobDescription = string.Format(description, commentSysNo, customerName, mobilePhoneNumber);
            var model = FeProductCommentReplyBo.Instance.GetModel(Convert.ToInt32(commentSysNo));
            var jobUrl = string.Format(url, model.CommentSysNo);
            if (oldTaskType != null && oldTaskType.Value > 0)
            {
                //当优先级参数为空时，继承旧任务的优先级
                if (priority == null)
                {
                    var job = ISyJobPoolDao.Instance.GetByTask(commentSysNo, oldTaskType.Value);
                    if (job != null)
                    {
                        priority = job.Priority;
                    }
                }
                SyJobPoolManageBo.Instance.DeleteJobPool(commentSysNo, oldTaskType.Value);
            }
            return CreateJobPool(jobDescription, jobUrl, taskType, commentSysNo, executorSysNo, priority);
        }
        /// <summary>
        /// 商品晒单审核
        /// [商品晒单审核成功后调用]
        /// </summary>
        /// <param name="shareSysNo">商品晒单编号</param>
        /// <param name="customerName">客户姓名</param>
        /// <param name="mobilePhoneNumber">客户手机号</param>
        /// <param name="executorSysNo">任务执行者编号</param>
        /// <param name="oldTaskType">旧任务类型（如不为空则需删除旧任务）</param>
        /// <param name="priority">优先级</param>
        /// <returns>任务</returns>
        /// <remarks>2013-11-29 余勇 创建</remarks>
        public SyJobPool ProductShareAudit(int shareSysNo, string customerName, string mobilePhoneNumber,
                                    int? executorSysNo = null, int? oldTaskType = null, int? priority = null)
        {
            const string url = "/Front/FeProductShareAudit/?commentSysNo={0}";
            const string description = "晒单号:{0}{1}{2}";
            const SystemStatus.任务对象类型 taskType = SystemStatus.任务对象类型.商品晒单审核;
            customerName = !string.IsNullOrEmpty(customerName) ? "," + customerName : "";
            mobilePhoneNumber = !string.IsNullOrEmpty(mobilePhoneNumber) ? "," + mobilePhoneNumber : "";
            var jobDescription = string.Format(description, shareSysNo, customerName, mobilePhoneNumber);
            var jobUrl = string.Format(url, shareSysNo);
            if (oldTaskType != null && oldTaskType.Value > 0)
            {
                //当优先级参数为空时，继承旧任务的优先级
                if (priority == null)
                {
                    var job = ISyJobPoolDao.Instance.GetByTask(shareSysNo, oldTaskType.Value);
                    if (job != null)
                    {
                        priority = job.Priority;
                    }
                }
                SyJobPoolManageBo.Instance.DeleteJobPool(shareSysNo, oldTaskType.Value);
            }
            return CreateJobPool(jobDescription, jobUrl, taskType, shareSysNo, executorSysNo, priority);
        }

        /// <summary>
        /// 广告组展示、广告项展示、商品组展示、商品项展示审核、修改
        /// [商品展示审核、修改成功后调用]
        /// </summary>
        /// <param name="shareSysNo">商品展示编号</param>
        /// <param name="groupSysNo">商品组、广告组编号</param>
        /// <param name="userSysNo">每户用户编号</param>
        /// <param name="operatingContent">操作内容</param>
        /// <param name="page">url地址</param>
        /// <param name="executorSysNo">任务执行者编号</param>
        /// <param name="oldTaskType">旧任务类型（如不为空则需删除旧任务）</param>
        /// <param name="priority">优先级</param>
        /// <returns>任务</returns>
        /// <remarks>2013-12-24 苟治国 创建</remarks>
        public SyJobPool FeAudit(int shareSysNo, int groupSysNo, int userSysNo, string page, string operatingContent, int? executorSysNo = null, int? oldTaskType = null, int? priority = null)
        {
            string url = string.Empty;
            string description = string.Empty;

            #region
            switch (page)
            {
                case "AdvertGroup":
                    {
                        url = "/Front/FeAdvertGroup/?commentSysNo={0}";
                        description = "广告组编号:{0}{1}{2}";
                    }
                    break;
                case "AdvertItem":
                    {
                        url = "/Front/FeAdvertItem/?commentSysNo={0}&groupSysNo={1}";
                        description = "广告项编号:{0}{1}{2}";
                    }
                    break;
                case "ProductGroup":
                    {
                        url = "/Front/FeProductGroup/?commentSysNo={0}";
                        description = "商品组编号:{0}{1}{2}";
                    }
                    break;
                case "ProductItem":
                    {
                        url = "/Front/FeProductItem/?commentSysNo={0}&groupSysNo={1}";
                        description = "商品项编号:{0}{1}{2}";
                    }
                    break;
            }
            #endregion

            const SystemStatus.任务对象类型 taskType = SystemStatus.任务对象类型.通知;

            string customerName = string.Empty;
            var model = Hyt.BLL.Sys.SyUserBo.Instance.GetSyUser(userSysNo);
            if (model != null)
            {
                customerName = model.UserName;
            }

            var jobDescription = string.Format(description, shareSysNo, customerName, operatingContent);
            string jobUrl = string.Empty;
            if (groupSysNo > 0)
            {
                jobUrl = string.Format(url, shareSysNo, groupSysNo);
            }
            else
            {
                jobUrl = string.Format(url, shareSysNo);
            }
            if (oldTaskType != null && oldTaskType.Value > 0)
            {
                //当优先级参数为空时，继承旧任务的优先级
                if (priority == null)
                {
                    var job = ISyJobPoolDao.Instance.GetByTask(shareSysNo, oldTaskType.Value);
                    if (job != null)
                    {
                        priority = job.Priority;
                    }
                }
                SyJobPoolManageBo.Instance.DeleteJobPool(shareSysNo, oldTaskType.Value);
            }
            return CreateJobPool(jobDescription, jobUrl, taskType, shareSysNo, executorSysNo, priority);
        }

        /// <summary>
        /// 短信咨询任务发布
        /// </summary>
        /// <param name="smsContent">短信内容</param>
        /// <param name="smsQuestionSysNo">短信咨询系统编号</param>
        /// <param name="answerSysNo">回复人系统编号</param>
        /// <returns>任务</returns>
        /// <remarks>2014-02-21 邵斌 创建</remarks>
        public SyJobPool SmsQuestion(string smsContent, int smsQuestionSysNo, int answerSysNo)
        {
            var job = CreateJobPool(smsContent, string.Format("/CRM/ShowCrSmsQuestionTaskJob?sysNo={0}", smsQuestionSysNo), SystemStatus.任务对象类型.通知, smsQuestionSysNo, answerSysNo);
            job = ISyJobPoolDao.Instance.Get(job.SysNo);
            job.JobUrl = string.Format("/CRM/ShowCrSmsQuestionTaskJob?sysNo={0}&jobPoolSysNo={1}&taskType={2}", smsQuestionSysNo, job.SysNo, (int)SystemStatus.任务对象类型.通知);
            SyJobPoolManageBo.Instance.Update(job);
            return job;
        }

        #region 私有方法

        /// <summary>
        /// 创建任务
        /// </summary>
        /// <param name="jobDescription">任务描述</param>
        /// <param name="jobUrl">任务URL</param>
        /// <param name="taskType">任务对象类型</param>
        /// <param name="taskSysNo">任务对象编号</param>
        /// <param name="executorSysNo">任务执行者编号</param>
        /// <param name="priority">优先级</param>
        /// <returns>任务</returns>
        /// <remarks>2013-09-27 吴文强 创建</remarks>
        private SyJobPool CreateJobPool(string jobDescription, string jobUrl, SystemStatus.任务对象类型 taskType, int taskSysNo, int? executorSysNo = null, int? priority = null)
        {
            //判断订单池是否已存在该记录
            SyJobPool jobPool = ISyJobPoolDao.Instance.GetByTask(taskSysNo, (int)taskType);
            if (jobPool == null)
            {
                int sysPriority = SyJobPoolPriorityBo.Instance.GetPriorityByCode(JobPriorityCode.PC001.ToString());   //优先级编码对应的数据库优先级
                jobPool = new SyJobPool
                    {
                        JobDescription = jobDescription,
                        JobUrl = jobUrl,
                        TaskType = (int)taskType,
                        TaskSysNo = taskSysNo,
                        CreatedDate = DateTime.Now,
                        AssignDate = DateTime.Now,
                        Priority = priority != null ? priority.Value : sysPriority,
                        BeginDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                        EndDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue
                    };
                if (executorSysNo.HasValue && executorSysNo.Value > 0)
                {
                    jobPool.ExecutorSysNo = executorSysNo.Value;
                    jobPool.AssignerSysNo = User.SystemUser;
                    jobPool.Status = (int)SystemStatus.任务池状态.待处理;
                }
                else
                {
                    jobPool.Status = (int)SystemStatus.任务池状态.待分配;
                }

                jobPool.SysNo = ISyJobPoolDao.Instance.Insert(jobPool);
            }
            return jobPool;
        }

        #endregion
    }
}
