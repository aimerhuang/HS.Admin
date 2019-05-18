using System;
using System.Collections.Generic;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Front
{
    /// <summary>
    /// 商品评论接口类
    /// </summary>
    /// <remarks>
    /// 2014/1/9 何方 创建
    /// </remarks>
    public abstract class IFeProductCommentDao : DaoBase<IFeProductCommentDao>
    {
        /// <summary>
        /// 查看商品评论
        /// </summary>
        /// <param name="sysNo">商品评论编号</param>
        /// <returns>商品评论</returns>
        /// <remarks>2013－06-27 苟治国 创建</remarks>
        public abstract CBFeProductComment GetModel(int sysNo);

        /// <summary>
        /// 查看商品评论/晒单
        /// </summary>
        /// <param name="orderSysNo">商品评论编号</param>
        /// <param name="productSysNo">商品评论编号</param>
        /// <param name="customerSysNo">商品评论编号</param>
        /// <returns>商品评论/晒单</returns>
        /// <remarks>2013-08-16 杨晗 创建</remarks>
        public abstract IList<CBFeProductComment> GetProductCommentList(int orderSysNo, int productSysNo, int customerSysNo);

        /// <summary>
        /// 获取已审核的前*条晒单信息
        /// </summary>
        /// <param name="count">条数</param>
        /// <returns>晒单列表</returns>
        /// <remarks>2013-08-21 杨晗 创建</remarks>
        public abstract IList<FeProductComment> GetShareList(int count);

        /// <summary>
        /// 查看商品评论
        /// </summary>
        /// <param name="sysNo">商品评论编号</param>
        /// <returns>商品评论</returns>
        /// <remarks>2013－06-27 苟治国 创建</remarks>
        /// <remarks>2013-07-10 杨晗 修改</remarks>
        public abstract FeProductComment GetProductComment(int sysNo);

        /// <summary>
        /// 根据条件获取商品评论的列表
        /// </summary>
        /// <param name="pageIndex">分页索引</param>
        /// <param name="pageSize">分页码</param>
        /// <param name="isShare">是否晒单</param>
        /// <param name="isComment">是否评论</param>
        /// <param name="status">状态</param>
        /// <param name="isBest">是否精华</param>
        /// <param name="isTop">是否置顶</param>
        /// <param name="beginDate">评论开始时间</param>
        /// <param name="endDate">评论结束时间</param>
        /// <param name="count">评论总条数</param>
        /// <param name="customerName">会员名称</param>
        /// <param name="erpCode">产品编号</param>
        /// <param name="productSysNo">产品系统号</param>
        /// <returns>商品评论列表</returns>
        /// <remarks>2013－06-27 苟治国 创建</remarks>
        /// <remarks>2013-07-10 杨晗 修改</remarks>
        public abstract IList<CBFeProductComment> Seach(int pageIndex, int pageSize, int? isShare,int? isComment,int? status,
                                                        int? isBest, int? isTop,
                                                        DateTime? beginDate, DateTime? endDate, out int count,
                                                        string customerName = null, string erpCode = null, string productSysNo = null);

        /// <summary>
        /// 插入商品评论
        /// </summary>
        /// <param name="model">插入的数据模型</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-07-10 杨晗 创建</remarks>
        public abstract int Insert(FeProductComment model);

        /// <summary>
        /// 更新商品评论
        /// </summary>
        /// <param name="model">更新的数据模型</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-07-10 杨晗 创建</remarks>
        public abstract int Update(FeProductComment model);

        /// <summary>
        /// 删除商品评论
        /// </summary>
        /// <param name="sysNo">商品评论主键</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks>2013-07-10 杨晗 创建</remarks>
        public abstract bool Delete(int sysNo);

        /// <summary>
        /// 获取商品总评分
        /// </summary>
        /// <param name="productSysNo">商品系统号</param>
        /// <returns>总评分</returns>
        /// <remarks>2013-08-16 杨晗 创建</remarks>
        public abstract int GetScore(int productSysNo);

        /// <summary>
        /// 获取商品总评论条数
        /// </summary>
        /// <param name="productSysNo">商品系统号</param>
        /// <returns>总评论条数</returns>
        /// <remarks>2013-08-16 杨晗 创建</remarks>
        public abstract int GetReviewCount(int productSysNo);

        /// <summary>
        /// 获取商品总晒单数
        /// </summary>
        /// <param name="productSysNo">商品系统号</param>
        /// <returns>总晒单数</returns>
        /// <remarks>2013-12-06 杨浩 创建</remarks>
        public abstract int GetShowCount(int productSysNo);

        /// <summary>
        /// 晒单或评价商品列表 （订单下商品的晒单评价状态）
        /// </summary>
        /// <param name="isComment">是否为评论</param>
        /// <param name="customerSysNo">会员系统编号</param>
        /// <param name="pager">分页实体</param>
        /// <remarks>
        /// 2013-08-29 郑荣华 创建
        /// </remarks> 
        public abstract void GetProductShowOrComment(int? isComment, int customerSysNo,ref Pager<CBSoOrderItem> pager);
    }
}
