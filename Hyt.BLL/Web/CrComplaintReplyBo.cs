using System.Text;
using Hyt.Model;
using System.Collections.Generic;
using Hyt.DataAccess.Web;

namespace Hyt.BLL.Web
{
    /// <summary>
    /// 投诉业务逻辑层
    /// </summary>
    /// <remarks>2013－08-05 苟治国 创建</remarks>
    public class CrComplaintReplyBo : BOBase<CrComplaintReplyBo>
    {
        /// <summary>
        /// 插入会员投诉回复
        /// </summary>
        /// <param name="model"></param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－11-19 苟治国 创建</remarks>
        public int Insert(Model.CrComplaintReply model)
        {
            return ICrComplaintReplyDao.Instance.Insert(model);
        }

        /// <summary>
        /// 更新会员投诉回复
        /// </summary>
        /// <param name="model"></param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－11-19 苟治国 创建</remarks>
        public int Update(Model.CrComplaintReply model)
        {
            return ICrComplaintReplyDao.Instance.Update(model);
        }
    }
}
