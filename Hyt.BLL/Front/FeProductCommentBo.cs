using System;
using System.Collections.Generic;
using System.Linq;
using Hyt.DataAccess.Order;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.DataAccess.Front;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.BLL.Front
{
    /// <summary>
    /// 商品评论 业务层
    /// </summary>
    /// <remarks>2013-06-20 苟治国 创建</remarks>
    public class FeProductCommentBo : BOBase<FeProductCommentBo>
    {
        /// <summary>
        /// 查看商品评论
        /// </summary>
        /// <param name="sysNo">商品评论编号</param>
        /// <returns>商品评论</returns>
        /// <remarks>2013－06-27 苟治国 创建</remarks>
        public CBFeProductComment GetModel(int sysNo)
        {
            return IFeProductCommentDao.Instance.GetModel(sysNo);
        }

        /// <summary>
        /// 查看商品评论/晒单
        /// </summary>
        /// <param name="orderSysNo">商品评论编号</param>
        /// <param name="productSysNo">商品评论编号</param>
        /// <param name="customerSysNo">商品评论编号</param>
        /// <returns>商品评论/晒单</returns>
        /// <remarks>2013-08-16 杨晗 创建</remarks>
        public IList<CBFeProductComment> GetProductCommentList(int orderSysNo, int productSysNo, int customerSysNo)
        {
            return IFeProductCommentDao.Instance.GetProductCommentList(orderSysNo, productSysNo, customerSysNo);
        }

        
        /// <summary>
        /// 获取已审核的前*条晒单信息
        /// </summary>
        /// <param name="count">条数</param>
        /// <returns>晒单列表</returns>
        /// <remarks>2013-08-21 杨晗 创建</remarks>
        public IList<FeProductComment> GetShareList(int count)
        {
            return IFeProductCommentDao.Instance.GetShareList(count);
        }

        /// <summary>
        /// 获取当前用户某商品的评价
        /// </summary>
        /// <param name="orderSysNo">商品评论编号</param>
        /// <param name="productSysNo">商品评论编号</param>
        /// <param name="customerSysNo">商品评论编号</param>
        /// <returns>当前用户某商品的评价</returns>
        /// <remarks>2013-08-19 杨晗 创建</remarks>
        public CBFeProductComment GetComment(int orderSysNo, int productSysNo, int customerSysNo)
        {
            var list = IFeProductCommentDao.Instance.GetProductCommentList(orderSysNo, productSysNo, customerSysNo);
            if (list == null || list.Count == 0)
            {
                return null;
            }
            var comment = list.FirstOrDefault(c => c.IsComment==(int)ForeStatus.是否评论.是);
            return comment;
        }

        /// <summary>
        /// 获取当前用户某商品的晒单
        /// </summary>
        /// <param name="orderSysNo">商品评论编号</param>
        /// <param name="productSysNo">商品评论编号</param>
        /// <param name="customerSysNo">商品评论编号</param>
        /// <returns>当前用户某商品的晒单</returns>
        /// <remarks>2013-08-19 杨晗 创建</remarks>
        public CBFeProductComment GetShare(int orderSysNo, int productSysNo, int customerSysNo)
        {
            var list = IFeProductCommentDao.Instance.GetProductCommentList(orderSysNo, productSysNo, customerSysNo);
            if (list == null || list.Count == 0)
            {
                return null;
            }
            var share = list.FirstOrDefault(c => c.IsShare == (int)ForeStatus.是否晒单.是);
            return share;
        }

        /// <summary>
        /// 查看商品评论
        /// </summary>
        /// <param name="sysNo">商品评论编号</param>
        /// <returns>商品评论</returns>
        /// <remarks>2013-07-10 杨晗 修改</remarks>
        public FeProductComment GetProductComment(int sysNo)
        {
            return IFeProductCommentDao.Instance.GetProductComment(sysNo);
        }

        /// <summary>
        /// 根据条件获取商品评论的列表
        /// </summary>
        /// <param name="pageIndex">分页索引</param>
        /// <param name="isShare">是否晒单</param>
        /// <param name="isComment">是否评论</param>
        /// <param name="status">状态</param>
        /// <param name="isBest">是否精华</param>
        /// <param name="isTop">是否置顶</param>
        /// <param name="beginDate">评论开始时间</param>
        /// <param name="endDate">评论结束时间</param>
        /// <param name="customerName">会员名称</param>
        /// <param name="erpCode">产品编号</param>
        /// <param name="productSysNo">产品系统号</param>
        /// <returns>商品评论列表</returns>
        /// <remarks>2013－06-27 苟治国 创建</remarks>
        /// <remarks>2013-07-10 杨晗 修改</remarks>
        public PagedList<CBFeProductComment> Seach(int? pageIndex, int? isShare, int? isComment, int? status,
                                                   int? isBest, int? isTop,
                                                   DateTime? beginDate, DateTime? endDate,
                                                   string customerName = null, string erpCode = null, string productSysNo = null, int executorSysNo = 0)
        {
            pageIndex = pageIndex ?? 1;
            var model = new PagedList<CBFeProductComment>();
            int count;
            var list = IFeProductCommentDao.Instance.Seach((int) pageIndex, model.PageSize, isShare, isComment, status,
                                                           isBest, isTop,
                                                           beginDate, endDate, out count, customerName, erpCode, productSysNo);
            model.TData = list;
            model.TotalItemCount = count;
            model.CurrentPageIndex = (int) pageIndex;
            return model;

        }

        /// <summary>
        /// 插入商品评论
        /// </summary>
        /// <param name="model">插入的数据模型</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-07-10 杨晗 创建</remarks>
        public int Insert(FeProductComment model)
        {
            return IFeProductCommentDao.Instance.Insert(model);
        }

        /// <summary>
        /// 更新商品评论
        /// </summary>
        /// <param name="model">更新的数据模型</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-07-10 杨晗 创建</remarks>
        public int Update(FeProductComment model)
        {
            return IFeProductCommentDao.Instance.Update(model);
        }

        /// <summary>
        /// 删除商品评论
        /// </summary>
        /// <param name="sysNo">商品评论主键</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks>2013-07-10 杨晗 创建</remarks>
        public bool Delete(int sysNo)
        {
            return IFeProductCommentDao.Instance.Delete(sysNo);
        }

        /// <summary>
        /// 获取晒单评价中当前客户已完成的销售单明细列表
        /// </summary>
        /// <param name="pageIndex">分页索引</param>
        /// <param name="customerSysNo">当前用户系统号</param>
        /// <returns>销售单明细列表</returns>
        /// <remarks>2013-08-15 杨晗 创建</remarks>
        public PagedList<CBFeCommentList> GetFeCommentList(int? pageIndex, int customerSysNo)
        {
            pageIndex = pageIndex ?? 1;
            var model = new PagedList<CBFeCommentList>();
            int count;
            var list = ISoOrderItemDao.Instance.GetFeCommentList((int) pageIndex, model.PageSize, customerSysNo,
                                                                 out count);
            model.TData = list;
            model.TotalItemCount = count;
            model.CurrentPageIndex = (int) pageIndex;
            return model;

        }

        /// <summary>
        /// 获取晒单评价中当前客户已完成并且没晒单的销售单明细列表
        /// </summary>
        /// <param name="customerSysNo">用户系统号</param>
        /// <returns>销售单明细列表</returns>
        /// <remarks>2013-08-15 杨晗 创建</remarks>
        public IList<CBFeCommentList> GetFeCommentList(int customerSysNo)
        {
            return ISoOrderItemDao.Instance.GetFeCommentList(customerSysNo);
        }

        /// <summary>
        /// 获取商品总评分
        /// </summary>
        /// <param name="productSysNo">商品系统号</param>
        /// <returns>总评分</returns>
        /// <remarks>2013-08-16 杨晗 创建</remarks>
        public int GetScore(int productSysNo)
        {
            return IFeProductCommentDao.Instance.GetScore(productSysNo);
        }

        /// <summary>
        /// 获取商品总评论条数
        /// </summary>
        /// <param name="productSysNo">商品系统号</param>
        /// <returns>总评论条数</returns>
        /// <remarks>2013-08-16 杨晗 创建</remarks>
        public int GetReviewCount(int productSysNo)
        {
            return IFeProductCommentDao.Instance.GetReviewCount(productSysNo);
        }

        /// <summary>
        /// 获取商品总晒单数
        /// </summary>
        /// <param name="productSysNo">商品系统号</param>
        /// <returns>总晒单数</returns>
        /// <remarks>2013-12-06 杨浩 创建</remarks>
        public int GetShowCount(int productSysNo)
        {
            return IFeProductCommentDao.Instance.GetShowCount(productSysNo);
        }
        
        /// <summary>
        /// 晒单或评价商品列表 （订单下商品的晒单评价状态）
        /// </summary>
        /// <param name="isComment">是否为评论</param>
        /// <param name="customerSysNo">会员系统编号</param>
        /// <param name="pager">分页实体</param>
        /// <remarks>
        /// 2013-08-29 郑荣华 创建
        /// </remarks> 
        public void GetProductShowOrComment(int? isComment, int customerSysNo, ref Pager<CBSoOrderItem> pager)
        {
            IFeProductCommentDao.Instance.GetProductShowOrComment(isComment, customerSysNo, ref pager);
        }
    }
}
