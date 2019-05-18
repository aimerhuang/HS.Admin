using System.Text;
using Hyt.Model;
using System.Collections.Generic;
using Hyt.DataAccess.CRM;
using Hyt.Model.WorkflowStatus;
using Hyt.Infrastructure.Pager;

namespace Hyt.BLL.CRM
{
    /// <summary>
    ///会员投诉回复业务逻辑
    /// </summary>
    /// <remarks>2013－07-09 苟治国 创建</remarks>
    public class CrComplaintReplyBo : BOBase<CrComplaintReplyBo>
    {
        /// <summary>
        /// 查看会员投诉回复
        /// </summary>
        /// <param name="sysNo">会员投诉回复编号</param>
        /// <returns>会员投诉回复</returns>
        /// <remarks>2013－07-09 苟治国 创建</remarks>
        public Model.CrComplaintReply GetModel(int sysNo)
        {
            return ICrComplaintReplyDao.Instance.GetModel(sysNo);
        }

        /// <summary>
        /// 查看会员投诉回复等于会员投诉最后一条记录是否为会员
        /// </summary>
        /// <param name="sysNo">会员投诉编号</param>
        /// <returns>会员投诉回复</returns>
        /// <remarks>2013－07-09 苟治国 创建</remarks>
        public Model.CrComplaintReply GetReplyTop(int sysNo)
        {
            return ICrComplaintReplyDao.Instance.GetReplyTop(sysNo);
        }

        /// <summary>
        /// 根据条件获取会员投诉回复的列表
        /// </summary>
        /// <param name="sysNo">会员投诉系统编号</param>
        /// <returns>会员投诉回复列表</returns>
        /// <remarks>2013－07-09 苟治国 创建</remarks>
        public IList<Model.CBCrComplaintReply> Seach(int sysNo)
        {
            return ICrComplaintReplyDao.Instance.Seach(sysNo);
        }

        /// <summary>
        /// 插入会员投诉回复
        /// </summary>
        /// <param name="model"></param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－07-09 苟治国 创建</remarks>
        public int Insert(Model.CrComplaintReply model)
        {
            int result = ICrComplaintReplyDao.Instance.Insert(model);
            if (result > 0)
                BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "添加一条投诉回复", LogStatus.系统日志目标类型.客户投诉, Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo, result);
            return result;
        }

        /// <summary>
        /// 更新会员投诉回复
        /// </summary>
        /// <param name="model"></param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－07-09 苟治国 创建</remarks>
        public int Update(Model.CrComplaintReply model)
        {
            int result = ICrComplaintReplyDao.Instance.Update(model);
            if (result > 0)
                BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, string.Format("更新员投诉回复{0}",model.SysNo), LogStatus.系统日志目标类型.客户投诉, Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo, result);
            return result;
        }
    }
}
