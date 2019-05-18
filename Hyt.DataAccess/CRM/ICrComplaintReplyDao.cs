using Hyt.DataAccess.Base;
using Hyt.Model;
using System.Collections.Generic;

namespace Hyt.DataAccess.CRM
{
    /// <summary>
    /// 会员投诉回复 抽象类
    /// </summary>
    /// <remarks>
    /// 2013-07-09 苟治国 创建
    /// </remarks>
    public abstract class ICrComplaintReplyDao : DaoBase<ICrComplaintReplyDao>
    {
        /// <summary>
        /// 查看会员投诉回复
        /// </summary>
        /// <param name="sysNo">会员投诉回复编号</param>
        /// <returns>会员投诉回复</returns>
        /// <remarks>2013－07-09 苟治国 创建</remarks>
        public abstract Model.CrComplaintReply GetModel(int sysNo);

        /// <summary>
        /// 查看会员投诉回复等于会员投诉最后一条记录是否为会员
        /// </summary>
        /// <param name="sysNo">会员投诉编号</param>
        /// <returns>会员投诉回复</returns>
        /// <remarks>2013－07-09 苟治国 创建</remarks>
        public abstract Model.CrComplaintReply GetReplyTop(int sysNo);

        /// <summary>
        /// 根据条件获取会员投诉回复的列表
        /// </summary>
        /// <param name="sysNo">会员投诉系统编号</param>
        /// <returns>会员投诉回复列表</returns>
        /// <remarks>2013－07-09 苟治国 创建</remarks>
        public abstract IList<Model.CBCrComplaintReply> Seach(int sysNo);

        /// <summary>
        /// 插入会员投诉回复
        /// </summary>
        /// <param name="model"></param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－07-09 苟治国 创建</remarks>
        public abstract int Insert(Model.CrComplaintReply model);

        /// <summary>
        /// 更新会员投诉回复
        /// </summary>
        /// <param name="model"></param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－07-09 苟治国 创建</remarks>
        public abstract int Update(Model.CrComplaintReply model);
    }
}
