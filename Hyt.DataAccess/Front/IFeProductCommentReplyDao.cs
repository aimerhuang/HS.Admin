using System;
using System.Collections.Generic;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Front
{
    /// <summary>
    /// 商品评论回复数据层接口类
    /// </summary>
    /// <remarks>2013-07-10 杨晗 创建</remarks>
    public abstract class IFeProductCommentReplyDao : DaoBase<IFeProductCommentReplyDao>
    {
        /// <summary>
        /// 根据回复系统编号获取实体
        /// </summary>
        /// <param name="sysNo">商品评论回复系统编号</param>
        /// <returns>文章实体</returns>
        /// <remarks>2013-07-10 杨晗 创建</remarks>
        public abstract FeProductCommentReply GetModel(int sysNo);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex">起始页</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="commentSysNo">商品评论系统号</param>
        /// <param name="searchStaus">状态</param>
        /// <param name="count">总条数</param>
        /// <returns>商品评论列表</returns>
        /// <remarks>2013-07-10 杨晗 创建</remarks>
        public abstract IList<CBFeProductCommentReply> Seach(int pageIndex, int pageSize, int? commentSysNo,
                                                 ForeStatus.商品评论回复状态? searchStaus, out int count);

        /// <summary>
        /// 根据商品评论系统号获取旗下所有评论回复
        /// </summary>
        /// <param name="commentSysNo">商品评论系统号</param>
        /// <returns>评论回复列表</returns>
        /// <remarks>2013-08-19 杨晗 创建</remarks>
        public abstract IList<FeProductCommentReply> GetReplyByCommentSysNo(int commentSysNo);

        /// <summary>
        /// 根据商品评论系统号获取旗下所有评论回复
        /// </summary>
        /// <param name="commentSysNo">商品评论系统号</param>
        /// <param name="staus">商品评论回复状态</param>
        /// <returns>评论回复列表</returns>
        /// <remarks>2013-08-19 杨晗 创建</remarks>
        public abstract IList<FeProductCommentReply> GetReplyByCommentSysNo(int commentSysNo, ForeStatus.商品评论回复状态 staus);

        /// <summary>
        /// 插入商品评论回复
        /// </summary>
        /// <param name="model">插入的数据模型</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-07-10 杨晗 创建</remarks>
        public abstract int Insert(FeProductCommentReply model);

        /// <summary>
        /// 更新商品评论回复
        /// </summary>
        /// <param name="model">更新的数据模型</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-07-10 杨晗 创建</remarks>
        public abstract int Update(FeProductCommentReply model);

        /// <summary>
        /// 删除商品评论回复
        /// </summary>
        /// <param name="sysNo">商品评论回复系统号</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks>2013-07-10 杨晗 创建</remarks>
        public abstract bool Delete(int sysNo);
    }
}
