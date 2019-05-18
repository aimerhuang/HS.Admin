using System;
using System.Collections.Generic;
using Hyt.BLL.Log;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.DataAccess.Front;
using Hyt.Model.WorkflowStatus;

namespace Hyt.BLL.Front
{
    /// <summary>
    /// 商品评论回复 业务层
    /// </summary>
    /// <remarks>2013-06-10 杨晗 创建</remarks>
    public class FeProductCommentReplyBo : BOBase<FeProductCommentReplyBo>
    {
        /// <summary>
        /// 根据回复系统编号获取实体
        /// </summary>
        /// <param name="sysNo">商品评论回复系统编号</param>
        /// <returns>文章实体</returns>
        /// <remarks>2013-07-10 杨晗 创建</remarks>
        public FeProductCommentReply GetModel(int sysNo)
        {
            return IFeProductCommentReplyDao.Instance.GetModel(sysNo);
        }

        /// <summary>
        /// 根据商品评论系统号获取旗下所有评论回复
        /// </summary>
        /// <param name="commentSysNo">商品评论系统号</param>
        /// <returns>评论回复列表</returns>
        /// <remarks>2013-08-19 杨晗 创建</remarks>
        public IList<FeProductCommentReply> GetReplyByCommentSysNo(int commentSysNo)
        {
            return IFeProductCommentReplyDao.Instance.GetReplyByCommentSysNo(commentSysNo);
        }

        /// <summary>
        /// 根据商品评论系统号获取旗下所有评论回复
        /// </summary>
        /// <param name="commentSysNo">商品评论系统号</param>
        /// <param name="staus">商品评论回复状态</param>
        /// <returns>评论回复列表</returns>
        /// <remarks>2013-08-19 杨晗 创建</remarks>
        public IList<FeProductCommentReply> GetReplyByCommentSysNo(int commentSysNo, ForeStatus.商品评论回复状态 staus)
        {
            return IFeProductCommentReplyDao.Instance.GetReplyByCommentSysNo(commentSysNo, staus);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex">起始页</param>
        /// <param name="commentSysNo">商品评论系统号</param>
        /// <param name="searchStaus">状态</param>
        /// <returns>商品评论列表</returns>
        /// <remarks>2013-07-10 杨晗 创建</remarks>
        public PagedList<CBFeProductCommentReply> Seach(int? pageIndex, int? commentSysNo, ForeStatus.商品评论回复状态? searchStaus)
        {
            pageIndex = pageIndex ?? 1;
            var model = new PagedList<CBFeProductCommentReply>();
            int count;
            var list = IFeProductCommentReplyDao.Instance.Seach((int) pageIndex, model.PageSize, commentSysNo,
                                                                searchStaus, out count);
            model.TData = list;
            model.TotalItemCount = count;
            model.CurrentPageIndex = (int) pageIndex;
            return model;
        }

        /// <summary>
        /// 插入商品评论回复
        /// </summary>
        /// <param name="model">插入的数据模型</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-07-10 杨晗 创建</remarks>
        public int Insert(FeProductCommentReply model)
        {
            return IFeProductCommentReplyDao.Instance.Insert(model);
        }

        /// <summary>
        /// 更新商品评论回复
        /// </summary>
        /// <param name="model">更新的数据模型</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-07-10 杨晗 创建</remarks>
        public int Update(FeProductCommentReply model)
        {
            string msg;
            int u= IFeProductCommentReplyDao.Instance.Update(model);
            if (model.Status == (int)ForeStatus.商品评论回复状态.已审)
            {
                msg = "审核了评论/晒单回复";
            }
            else if (model.Status == (int)ForeStatus.商品评论回复状态.作废)
            {
                msg = "作废了评论/晒单回复";
            }
            else
            {
                msg = "取消什么了评论/晒单回复";
            }
            SysLog.Instance.Info(LogStatus.系统日志来源.后台,
                                 msg, LogStatus.系统日志目标类型.评价晒单管理, model.SysNo, null, "",
                                 Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo);
            return u;
        }

        /// <summary>
        /// 删除商品评论回复
        /// </summary>
        /// <param name="sysNo">商品评论回复系统号</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks>2013-07-10 杨晗 创建</remarks>
        public bool Delete(int sysNo)
        {
            return IFeProductCommentReplyDao.Instance.Delete(sysNo);
        }
    }
}
