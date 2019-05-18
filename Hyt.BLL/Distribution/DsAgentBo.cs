using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using Hyt.DataAccess.MallSeller;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.DataAccess.Distribution;
using Hyt.BLL.Sys;
using Hyt.Infrastructure.Memory;

namespace Hyt.BLL.Distribution
{
    /// <summary>
    /// 代理商业务层
    /// </summary>
    /// <remarks> 2016-04-13 刘伟豪 创建 </remarks>
    public class DsAgentBo : BOBase<DsAgentBo>
    {
        /// <summary>
        /// 分页查询代理商
        /// </summary>
        /// <param name="pager">分页</param>
        /// <param name="filter">查询条件</param>
        /// <remarks> 2016-04-13 刘伟豪 创建 </remarks>
        public void Seach(ref Pager<CBDsAgent> pager, ParaDsAgentFilter filter)
        {
            IDsAgentDao.Instance.Search(ref pager, filter);
        }

        /// <summary>
        /// 分页查询代理商资金列表
        /// </summary>
        /// <param name="pager">分页</param>
        /// <param name="filter">查询条件</param>
        /// <remarks> 2016-04-18 刘伟豪 创建 </remarks>
        public void SeachAgentPrePayment(ref Pager<CBDsAgent> pager, ParaDsAgentFilter filter)
        {
            IDsAgentDao.Instance.SeachAgentPrePayment(ref pager, filter);
        }

        /// <summary>
        /// 根据名称查询代理商列表，用于判断名称唯一性，新增时用
        /// </summary>
        /// <param name="agentName">代理商名称</param>
        /// <remarks> 2016-04-14 刘伟豪 创建 </remarks>
        public IList<CBDsAgent> List(string agentName)
        {
            return IDsAgentDao.Instance.List(agentName);
        }

        /// <summary>
        /// 根据名称查询代理商列表，用于判断名称唯一性，修改时用
        /// </summary>
        /// <param name="agentName">代理商名称</param>
        /// <param name="sysNo">要排除的代理商系统编号</param>
        /// <remarks> 2016-04-15 刘伟豪 创建 </remarks>
        public IList<CBDsAgent> List(string agentName, int sysNo)
        {
            return IDsAgentDao.Instance.List(agentName, sysNo);
        }

        /// <summary>
        /// 用于新建检查用户系统编号不重复，查询代理商信息，新增时用
        /// </summary>
        /// <param name="userSysNo">用户系统编号</param>
        /// <remarks> 2016-04-14 刘伟豪 创建 </remarks>
        public IList<CBDsAgent> List(int userSysNo)
        {
            return IDsAgentDao.Instance.List(userSysNo);
        }

        /// <summary>
        /// 用于更新检查用户系统编号不重复，查询代理商信息，修改时用
        /// </summary>
        /// <param name="userSysNo">用户系统编号</param>
        /// <param name="sysNo">要排除的代理商系统编号</param>
        /// <remarks> 2016-04-14 刘伟豪 创建 </remarks>
        public IList<CBDsAgent> List(int userSysNo, int sysNo)
        {
            return IDsAgentDao.Instance.List(userSysNo, sysNo);
        }

        /// <summary>
        /// 新建代理商
        /// </summary>
        /// <param name="model">代理商实体</param>
        /// <remarks> 2016-04-14 刘伟豪 创建 </remarks>
        public int Create(DsAgent model)
        {
            var sysNo = IDsAgentDao.Instance.Create(model);
            if (sysNo <= 0) return sysNo;//未成功直接返回
            Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "创建代理商", LogStatus.系统日志目标类型.代理商, sysNo);
            var modelPre = new DsAgentPrePayment //金额自动初始化为0
            {
                CreatedBy = model.CreatedBy,
                CreatedDate = model.CreatedDate,
                AgentSysNo = sysNo,
                LastUpdateBy = model.LastUpdateBy,
                LastUpdateDate = model.LastUpdateDate
            };
            DsAgentPrePaymentBo.Instance.Create(modelPre);
            return sysNo;//成功则创建代理商预存款后 返回新加的系统编号
        }

        /// <summary>
        /// 获取代理商实体
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <remarks> 2016-04-14 刘伟豪 创建 </remarks>
        public CBDsAgent GetDsAgent(int sysNo)
        {
            return IDsAgentDao.Instance.GetDsAgent(sysNo);
        }

        /// <summary>
        /// 根据管理员系统编号，获取代理商实体
        /// </summary>
        /// <param name="userSysNo">管理员系统编号</param>
        /// <remarks> 2016-04-15 刘伟豪 创建 </remarks>
        public CBDsAgent GetDsAgentByUserSysNo(int userSysNo)
        {
            return IDsAgentDao.Instance.GetDsAgentByUserSysNo(userSysNo);
        }

        /// <summary>
        /// 修改代理商
        /// </summary>
        /// <param name="model">代理商实体</param>
        /// <remarks> 2016-04-15 刘伟豪 创建 </remarks>
        public bool Update(DsAgent model)
        {
            bool r = IDsAgentDao.Instance.Update(model);
            if (r)
            {
                SyUserBo.Instance.UpdateSyUserStatus(model.UserSysNo, model.Status);
                UpdateDealersLevel(model.UserSysNo, model.LevelSysNo);
                Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "修改代理商", LogStatus.系统日志目标类型.代理商, model.SysNo);
            }
            return r;
        }

        /// <summary>
        /// 修改代理商时，同时统一更新下级分销商等级
        /// </summary>
        /// <param name="agentSysNo">代理商系统编号</param>
        /// <param name="levelSysNo">分销商等级编号</param>
        /// <remarks> 2016-04-15 刘伟豪 创建 </remarks>
        public bool UpdateDealersLevel(int agentSysNo, int levelSysNo)
        {
            return IDsAgentDao.Instance.UpdateDealersLevel(agentSysNo, levelSysNo);
        }

        /// <summary>
        /// 代理商状态更新，同时修改关联账号状态,状态枚举相同
        /// </summary>
        /// <param name="sysNo">代理商系统编号</param>
        /// <param name="status">代理商状态</param>
        /// <param name="lastUpdateBy">最后更新人</param>
        /// <remarks> 2016-04-15 刘伟豪 创建 </remarks>
        public bool UpdateStatus(int sysNo, DistributionStatus.代理商状态 status, int lastUpdateBy)
        {
            bool r = IDsAgentDao.Instance.UpdateStatus(sysNo, status, lastUpdateBy);
            if (r)
            {
                var userSysNo = GetDsAgent(sysNo).UserSysNo;
                var userStatus = status == DistributionStatus.代理商状态.启用 ? 1 : 0;
                SyUserBo.Instance.UpdateSyUserStatus(userSysNo, userStatus);
                Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "修改代理商状态：" + status.ToString(), LogStatus.系统日志目标类型.代理商, sysNo);
            }

            return r;
        }

        /// <summary>
        /// 代理商提现
        /// </summary>
        /// <param name="sysNo">代理商系统编号</param>
        /// <param name="amount">金额</param>
        /// <param name="syUser">操作者</param>
        /// <param name="remarks">备注</param>
        /// <returns>系统编号</returns>
        /// <remarks> 2016-04-18 刘伟豪 创建 </remarks>
        public int Withdraw(int sysNo, decimal amount, SyUser syUser, string remarks)
        {
            int ItemSysNo = 0;
            if (!CheckAgentStatus(sysNo)) throw new HytException("非法操作，代理商禁用时不能提现!");
            var model = DsAgentPrePaymentBo.Instance.GetDsAgentPrePayment(sysNo);
            if (model == null)
            {
                throw new HytException("未找到代理商充值记录!");
            }
            else
            {
                if (model.AvailableAmount >= amount)
                {
                    IDsAgentPrePaymentDao.Instance.SubtractAvailableAmount(sysNo, amount, syUser.SysNo);
                    model.AvailableAmount -= amount;
                    var itemModel = new DsAgentPrePaymentItem()
                    {
                        AgentPrePaymentSysNo = model.SysNo,
                        Source = (int)DistributionStatus.预存款明细来源.提现,
                        SourceSysNo = model.SysNo,
                        Increased = decimal.Zero,
                        Decreased = amount,
                        Surplus = model.AvailableAmount,
                        Status = (int)DistributionStatus.预存款明细状态.冻结,
                        Remarks = "代理商提现",
                    };
                    itemModel.CreatedBy = itemModel.LastUpdateBy = syUser.SysNo;
                    itemModel.CreatedDate = itemModel.LastUpdateDate = DateTime.Now;
                    ItemSysNo = IDsAgentPrePaymentItemDao.Instance.Create(itemModel);
                }
                else
                {
                    throw new HytException("提取金额超过预存款可用余额!");
                }
            }
            return ItemSysNo;
        }

        private bool CheckAgentStatus(int sysNo)
        {
            var model = IDsAgentDao.Instance.GetDsAgent(sysNo);
            return model != null && model.Status == (int)DistributionStatus.代理商状态.启用;
        }
    }
}