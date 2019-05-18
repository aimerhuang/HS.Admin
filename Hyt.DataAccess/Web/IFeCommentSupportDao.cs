using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Web
{
    /// <summary>
    /// 前台商品咨询
    /// </summary>
    /// <remarks>2013-08-13 邵斌 创建</remarks>
    public abstract class IFeCommentSupportDao : DaoBase<IFeCommentSupportDao>
    {
        /// <summary>
        /// 添加评论支持
        /// </summary>
        /// <param name="model">评论支持数据模型</param>
        /// <param name="context">数据库操作上下文(用于共用数据库连接)</param>
        /// <returns>返回 true:成功 false:失败</returns>
        /// <remarks>2013-08-13 邵斌 创建</remarks>
        public abstract bool Add(FeCommentSupport model, IDbContext context = null);

        /// <summary>
        /// 更新评论支持
        /// </summary>
        /// <param name="isSupport">是否是支持</param>
        /// <param name="feCommentSysNo">评论表系统编号</param>
        /// <param name="customerSysNo">操作人</param>
        /// <param name="context">数据库操作上下文(用于共用数据库连接)</param>
        /// <returns>返回 true:成功 false:失败</returns>
        /// <remarks>2013-08-13 邵斌 创建</remarks>
        public abstract bool Update(bool isSupport,int feCommentSysNo, int customerSysNo, IDbContext context = null);

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="feCommentSysNo">评论系统编号</param>
        /// <param name="customerSysNo">会员系统编号</param>
        /// <param name="context">数据库操作上下文(用于共用数据库连接)</param>
        /// <returns>返回评论支持对象</returns>
        /// <remarks>2013-08-13 邵斌 创建</remarks>
        public abstract FeCommentSupport GetMode(int feCommentSysNo, int customerSysNo,IDbContext context = null);

        /// <summary>
        /// 判断用户是否已经评价过
        /// </summary>
        /// <param name="feCommentSysNo">评价系统编号</param>
        /// <param name="customerSysNo">会员系统编号</param>
        /// <param name="context">数据库操作上下文(用于共用数据库连接)</param>
        /// <returns>返回 true：已经存在 false：不存在</returns>
        /// <remarks>2013-08-13 邵斌 创建</remarks>
        public abstract bool Exist(int feCommentSysNo, int customerSysNo, IDbContext context = null);
    }
}
