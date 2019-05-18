using Hyt.DataAccess.Base;
using Hyt.Model;
using System.Collections.Generic;
namespace Hyt.DataAccess.Web
{
    /// <summary>
    /// 投诉回复数据访问 抽象类 
    /// </summary>
    /// <remarks>2013-11-19 苟治国 创建</remarks>
    public abstract class ICrComplaintReplyDao : DaoBase<ICrComplaintReplyDao>
    {
        /// <summary>
        /// 插入会员投诉回复
        /// </summary>
        /// <param name="model"></param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－11-19 苟治国 创建</remarks>
        public abstract int Insert(Model.CrComplaintReply model);

        /// <summary>
        /// 更新会员投诉回复
        /// </summary>
        /// <param name="model"></param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－11-19 苟治国 创建</remarks>
        public abstract int Update(Model.CrComplaintReply model);
    }
}
