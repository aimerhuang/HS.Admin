using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.BLL.Sys;
using Hyt.DataAccess.Web;
using Hyt.Infrastructure.Caching;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.WorkflowStatus;

namespace Hyt.BLL.Web
{
    /// <summary>
    /// 商品评论
    /// </summary>
    /// <remarks> 2013-08-09 邵斌 创建</remarks>
    public class FeProductCommentBo : BOBase<FeProductCommentBo>
    {
        /// <summary>
        /// 根据产品SysNo获取最先评论的5位用户
        /// </summary>
        /// <param name="productSysNo">产品SysNo</param>
        /// <returns>键值对(评论Id,客户昵称(或者客户用户名))</returns>
        /// <remarks> 2013-08-09 邵斌 创建</remarks>
        public IDictionary<int, string> GetFirstReviewTop5(int productSysNo)
        {
            return CacheManager.Get(CacheKeys.Items.FirstReviewTop5_, productSysNo.ToString(), delegate
                {
                    return Hyt.DataAccess.Web.IFeProductCommentDao.Instance.GetFirstReviewTop5(productSysNo);
                });
        }

        /// <summary>
        /// 获取指定商品的评论次数详细情况
        /// 评分规则为：0-1分：差评   2-3分：一般    4-5：好评
        /// </summary>
        /// <param name="productSysNo"></param>
        /// <returns>返回总共评论次数，好评次数，一般次数，差评次数</returns>
        /// <remarks>2013-08-09 邵斌 创建</remarks>
        public IDictionary<string, int> GetProductCommentTimesDetialInfo(int productSysNo)
        {
            return CacheManager.Get(CacheKeys.Items.ProductCommentTotalInfo_, productSysNo.ToString(), delegate
            {
                return Hyt.DataAccess.Web.IFeProductCommentDao.Instance.GetProductCommentTimesDetialInfo(productSysNo);
            });
        }

        /// <summary>
        /// 获取商品评价
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <param name="pager">分页参数</param>
        /// <param name="isShare">是否是晒单</param>
        /// <param name="socreRange">操作数据类型</param>
        /// <returns></returns>
        /// <remarks>2013-08-12 邵斌 创建</remarks>
        public void GetProductComment(int productSysNo,ref Pager<CBFeProductComment> pager, bool isShare,int[] socreRange )
        {
            var filter = new ParaFeProductCommentFilter {ProductSysNo = productSysNo};

            //是否是查找晒单数据，如果不是将对查找的分值范围进行限定
            if (!isShare)
            {
                filter.IsShare = -1;
                filter.StartSocre = socreRange[0];
                filter.EndSocre = socreRange[1];
            }
            else
            {
                filter.IsShare = (int)ForeStatus.是否晒单.是;
                filter.StartSocre = 0;                //评分开始区间
                filter.EndSocre = 5;                  //评分结束区间
                
            }

            Hyt.DataAccess.Web.IFeProductCommentDao.Instance.GetProductComment(filter,ref pager);
            
        }

        /// <summary>
        /// 评论回复
        /// </summary>
        /// <param name="feCommentSysNo">评论系统编号</param>
        /// <param name="content">评论回复内容</param>
        /// <param name="customerSysNo">回复人</param>
        /// <returns>返回 true:回复成功 false:回复失败</returns>
        /// <remarks>2013-08-13 邵斌 创建</remarks>
        public bool Replay(int feCommentSysNo, string content, int customerSysNo)
        {
            //关键词过滤
            content = KeyWordsFilter(content);
            var result = IFeProductCommentDao.Instance.Replay(feCommentSysNo, content, customerSysNo);
            if (result.Status)
            {
                //评论回复时添加任务
                SyJobPoolManageBo.Instance.AssignJobByTaskType((int)SystemStatus.任务对象类型.商品评论回复审核, result.StatusCode, 0);
            }

            return result.Status;
        }

        /// <summary>
        /// 评价内容回复关键词过滤 
        /// </summary>
        /// <param name="content">信息内容</param>
        /// <returns>关键字过滤</returns>
        /// <remarks>2013-08-13 邵斌 创建</remarks>
        public string KeyWordsFilter(string content)
        {
            return content;
        }
    }
}
