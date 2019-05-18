using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hyt.Model;
using Hyt.DataAccess.Base;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;

namespace Hyt.DataAccess.Web
{
    /// <summary>
    /// 商品评论
    /// </summary>
    /// <remarks>2013-08-09 邵斌 创建 </remarks>
    public abstract class IFeProductCommentDao : DaoBase<IFeProductCommentDao>
    {
        /// <summary>
        /// 根据产品SysNo获取最先评论的5位用户
        /// </summary>
        /// <param name="productSysNo">产品SysNo</param>
        /// <returns>键值对(评论Id,客户昵称(或者客户用户名))</returns>
        /// <remarks>
        /// 2013-08-09 邵斌 创建
        /// </remarks>
        public abstract IDictionary<int, string> GetFirstReviewTop5(int productSysNo);

        /// <summary>
        /// 获取指定商品的评论次数详细情况
        /// </summary>
        /// <param name="productSysNo"></param>
        /// <returns>返回总共评论次数，好评次数，一般次数，差评次数</returns>
        /// <remarks>2013-08-09 邵斌 创建</remarks>
        public abstract IDictionary<string, int> GetProductCommentTimesDetialInfo(int productSysNo);

        /// <summary>
        /// 获取商品评价
        /// </summary>
        /// <param name="filter">商品系统编号</param>
        /// <param name="pager">分页参数</param>
        /// <returns></returns>
        /// <remarks>2013-08-12 邵斌 创建</remarks>
        public abstract void GetProductComment(ParaFeProductCommentFilter filter, ref Pager<CBFeProductComment> pager);

        /// <summary>
        /// 评论回复
        /// </summary>
        /// <param name="feCommentSysNo">评论系统编号</param>
        /// <param name="content">评论回复内容</param>
        /// <param name="customerSysNo">回复人</param>
        /// <returns>返回 true:回复成功 false:回复失败</returns>
        /// <remarks>2013-08-13 邵斌 创建</remarks>
        public abstract Result Replay(int feCommentSysNo, string content, int customerSysNo);
    }
}
