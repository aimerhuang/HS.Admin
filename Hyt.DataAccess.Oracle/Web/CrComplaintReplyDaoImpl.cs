using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess;
using Hyt.DataAccess.Web;
using Hyt.Model.WorkflowStatus;
using Hyt.Model;

namespace Hyt.DataAccess.Oracle.Web
{
    /// <summary>
    /// 投诉回复数据访问  
    /// </summary>
    /// <remarks>2013-11-19 苟治国 创建</remarks>
    public class CrComplaintReplyDaoImpl : ICrComplaintReplyDao
    {
        /// <summary>
        /// 插入会员投诉回复
        /// </summary>
        /// <param name="model"></param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－11-19 苟治国 创建</remarks>
        public override int Insert(Model.CrComplaintReply model)
        {
            var result = Context.Insert<CrComplaintReply>("CrComplaintReply", model)
                    .AutoMap(x => x.SysNo)
                    .ExecuteReturnLastId<int>("SysNo");
            return result;
        }

        /// <summary>
        /// 更新会员投诉回复
        /// </summary>
        /// <param name="model"></param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－11-19 苟治国 创建</remarks>
        public override int Update(Model.CrComplaintReply model)
        {
            int rowsAffected = Context.Update<Model.CrComplaintReply>("CrComplaintReply", model)
                          .AutoMap(x => x.SysNo)
                          .Where(x => x.SysNo)
                          .Execute();
            return rowsAffected;
        }
    }
}
