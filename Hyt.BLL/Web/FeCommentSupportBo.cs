using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Infrastructure.Caching;
using Hyt.DataAccess.Web;

namespace Hyt.BLL.Web
{
    /// <summary>
    /// 商品评论支持
    /// </summary>
    /// <remarks>2013-08-13 邵斌 创建</remarks>
    public class FeCommentSupportBo : BOBase<FeCommentSupportBo>
    {
        /// <summary>
        /// 更新评论支持
        /// </summary>
        /// <param name="isSupport">是否是支持</param>
        /// <param name="feCommentSysNo">评论表系统编号</param>
        /// <param name="customerSysNo">操作人</param>
        /// <returns>返回 true:成功 false:失败</returns>
        /// <remarks>2013-08-13 邵斌 创建</remarks>
        public Result Update(bool isSupport, int feCommentSysNo,int customerSysNo)
        {
            Result result = new Result();
            if (Exist(feCommentSysNo, customerSysNo))
            {
                result.Status = false;
                result.Message = "评价失败！您已评价该评论！";
            }
            result.Status = IFeCommentSupportDao.Instance.Update(isSupport, feCommentSysNo, customerSysNo);
            if (!result.Status)
            {
                result.Message = "您的评价失败，请稍后重试";
            }
            else
            {
                result.Message = "评价成功！";
            }

            return result;
        }

        /// <summary>
        /// 是否已经存在评价打分
        /// </summary>
        /// <param name="feCommentSysNo">评论表系统编号</param>
        /// <param name="customerSysNo">操作人</param>
        /// <returns>返回 true:成功 false:失败</returns>
        /// <remarks>2013-08-13 邵斌 创建</remarks>
        public bool Exist(int feCommentSysNo, int customerSysNo)
        {
            return IFeCommentSupportDao.Instance.Exist(feCommentSysNo, customerSysNo);
        }
    }
}
