using Hyt.DataAccess.Base;
using Hyt.Model;
using System.Collections.Generic;

namespace Hyt.DataAccess.CRM
{
    /// <summary>
    /// 会员投诉抽象类
    /// </summary>
    /// <remarks>
    /// 2013-07-09 苟治国 创建
    /// </remarks>
    public abstract class ICrComplaintDao : DaoBase<ICrComplaintDao>
    {
        /// <summary>
        /// 查看会员投诉
        /// </summary>
        /// <param name="sysNo">会员投诉编号</param>
        /// <returns>会员投诉</returns>
        /// <remarks>2013－07-09 苟治国 创建</remarks>
        public abstract Model.CrComplaint GetModelSingle(int sysNo);

        /// <summary>
        /// 查看会员投诉
        /// </summary>
        /// <param name="sysNo">会员投诉编号</param>
        /// <returns>会员投诉</returns>
        /// <remarks>2013－07-09 苟治国 创建</remarks>
        public abstract Model.CBCrComplaint GetModel(int sysNo);

        /// <summary>
        /// 根据条件获取会员投诉的列表
        /// </summary>
        /// <param name="pager">会员投诉查询条件</param>
        /// <returns>会员投诉列表</returns>
        /// <remarks>2013－07-09 苟治国 创建</remarks>
        public abstract Pager<Model.CBCrComplaint> Seach(Pager<CBCrComplaint> pager);

        /// <summary>
        /// 插入会员投诉
        /// </summary>
        /// <param name="model"></param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－07-09 苟治国 创建</remarks>
        public abstract int Insert(Model.CrComplaint model);

        /// <summary>
        /// 更新会员投诉
        /// </summary>
        /// <param name="model"></param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－07-09 苟治国 创建</remarks>
        public abstract int Update(Model.CrComplaint model);

        /// <summary>
        /// 更新广告组状态
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="strWhere">条件</param>
        /// <returns>返回受影响的行</returns>
        /// <remarks>2013－07-09 苟治国 创建</remarks>
        public abstract int UpdateStatus(string status, string strWhere);

        /// <summary>
        /// 删除会员投诉
        /// </summary>
        /// <param name="sysNo">会员投诉编号</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks>2013－07-09 苟治国 创建</remarks>
        public abstract bool Delete(int sysNo);
    }
}
