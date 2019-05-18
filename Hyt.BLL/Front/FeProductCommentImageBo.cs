using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.BLL.Log;
using Hyt.DataAccess.Front;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;

namespace Hyt.BLL.Front
{
    /// <summary>
    /// 晒单图片管理业务逻辑层
    /// </summary>
    /// <remarks>2013-07-12 杨晗 创建</remarks>
    public class FeProductCommentImageBo : BOBase<FeProductCommentImageBo>
    {
        /// <summary>
        /// 根据晒单图片系统编号获取实体
        /// </summary>
        /// <param name="sysNo">晒单图片系统编号</param>
        /// <returns>文章类型实体</returns>
        /// <remarks>2013-07-12 杨晗 创建</remarks>
        public FeProductCommentImage GetModel(int sysNo)
        {
            return IFeProductCommentImageDao.Instance.GetModel(sysNo);
        }

        /// <summary>
        /// 根据晒单系统编号获取所属晒单图片
        /// </summary>
        /// <param name="commentSysNo">晒单系统编号</param>
        /// <returns>晒单图片集合</returns>
        /// <remarks>2013-07-12 杨晗 创建</remarks>
        public IList<FeProductCommentImage> GetFeProductCommentImageByCommentSysNo(int commentSysNo)
        {
            return IFeProductCommentImageDao.Instance.GetFeProductCommentImageByCommentSysNo(commentSysNo);
        }

        /// <summary>
        /// 根据晒单系统编号获取所属晒单图片
        /// </summary>
        /// <param name="commentSysNo">晒单系统编号</param>
        /// <param name="staus">晒单图片状态</param>
        /// <returns>晒单图片集合</returns>
        /// <remarks>2013-07-12 杨晗 创建</remarks>
        public IList<FeProductCommentImage> GetFeProductCommentImageByCommentSysNo(int commentSysNo,ForeStatus.晒单图片状态 staus)
        {
            return IFeProductCommentImageDao.Instance.GetFeProductCommentImageByCommentSysNo(commentSysNo, staus);
        }

        /// <summary>
        /// 插入晒单图片记录
        /// </summary>
        /// <param name="model">插入的数据模型</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-07-12 杨晗 创建</remarks>
        public int Insert(FeProductCommentImage model)
        {
            return IFeProductCommentImageDao.Instance.Insert(model);
        }

        /// <summary>
        /// 更新晒单图片记录
        /// </summary>
        /// <param name="model">更新的数据模型</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-07-12 杨晗 创建</remarks>
        public int Update(FeProductCommentImage model)
        {
            string msg;
            int u = IFeProductCommentImageDao.Instance.Update(model);
            if (model.Status == (int)ForeStatus.晒单图片状态.已审)
            {
                msg = "审核了晒单图片";
            }
            else if (model.Status == (int)ForeStatus.晒单图片状态.作废)
            {
                msg = "作废了晒单图片";
            }
            else
            {
                msg = "取消审核了晒单图片";
            }
            SysLog.Instance.Info(LogStatus.系统日志来源.后台,
                                 msg, LogStatus.系统日志目标类型.评价晒单管理, model.SysNo, null, "",
                                 Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo);
            return u;
        }

        /// <summary>
        /// 删除晒单图片记录
        /// </summary>
        /// <param name="sysNo">晒单图片系统编号</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks>2013-07-12 杨晗 创建</remarks>
        public bool Delete(int sysNo)
        {
            return IFeProductCommentImageDao.Instance.Delete(sysNo);
        }
    }
}
