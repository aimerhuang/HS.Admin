using System.Collections.Generic;
using Hyt.DataAccess.Base;
using Hyt.Model;

namespace Hyt.DataAccess.Front
{
    /// <summary>
    /// 评论支持业务接口类
    /// </summary>
    /// <remarks>2013-08-27 杨晗 创建</remarks>
    public abstract class IFeCommentSupportDao : DaoBase<IFeCommentSupportDao>
    {

        /// <summary>
        /// 增加评论支持
        /// </summary>
        /// <param name="model">评论支持模型数据</param>
        /// <returns>成功系统号记录</returns>
        /// <remarks>2013-08-27 杨晗 创建</remarks>
        public abstract int Insert(FeCommentSupport model);

        /// <summary>
        /// 更新评论支持
        /// </summary>
        /// <param name="model">评论支持模型数据</param>
        /// <returns>成功系统号记录</returns>
        /// <remarks>2013-08-27 杨晗 创建</remarks>
        public abstract int Update(FeCommentSupport model);

        /// <summary>
        /// 根据评论系统号和用户系统号获取评论支持
        /// </summary>
        /// <param name="productCommentSysNo">评论系统号</param>
        /// <param name="customerSysNo">用户系统号</param>
        /// <returns>文章实体</returns>
        /// <remarks>2013-08-27 杨晗 创建</remarks>
        public abstract IList<FeCommentSupport> GetFeCommentSupport(int productCommentSysNo, int customerSysNo);

        /// <summary>
        /// 根据评论系统号和用户系统号获取评论支持
        /// </summary>
        /// <param name="productCommentSysNo">评论系统号</param>
        /// <param name="customerSysNo">用户系统号</param>
        /// <returns>文章实体</returns>
        /// <remarks>2013-08-27 杨晗 创建</remarks>
        public abstract FeCommentSupport GetModel(int productCommentSysNo, int customerSysNo);
    }
}
