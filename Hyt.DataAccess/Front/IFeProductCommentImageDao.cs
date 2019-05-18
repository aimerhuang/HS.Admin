using System.Collections.Generic;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Front
{
    /// <summary>
    /// 晒单图片数据层接口类
    /// </summary>
    /// <remarks>2013-07-12 杨晗 创建</remarks>
    public abstract class IFeProductCommentImageDao : DaoBase<IFeProductCommentImageDao>
    {
        /// <summary>
        /// 根据晒单图片系统编号获取实体
        /// </summary>
        /// <param name="sysNo">晒单图片系统编号</param>
        /// <returns>文章类型实体</returns>
        /// <remarks>2013-07-12 杨晗 创建</remarks>
        public abstract FeProductCommentImage GetModel(int sysNo);

        /// <summary>
        /// 根据晒单系统编号获取所属晒单图片
        /// </summary>
        /// <param name="commentSysNo">晒单系统编号</param>
        /// <returns>晒单图片集合</returns>
        /// <remarks>2013-07-12 杨晗 创建</remarks>
        public abstract IList<FeProductCommentImage> GetFeProductCommentImageByCommentSysNo(int commentSysNo);

        /// <summary>
        /// 根据晒单系统编号获取所属晒单图片
        /// </summary>
        /// <param name="commentSysNo">晒单系统编号</param>
        /// <param name="staus">晒单图片状态</param>
        /// <returns>晒单图片集合</returns>
        /// <remarks>2013-07-12 杨晗 创建</remarks>
        public abstract IList<FeProductCommentImage> GetFeProductCommentImageByCommentSysNo(int commentSysNo,
                                                                                            ForeStatus.晒单图片状态 staus);

        /// <summary>
        /// 插入晒单图片记录
        /// </summary>
        /// <param name="model">插入的数据模型</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-07-12 杨晗 创建</remarks>
        public abstract int Insert(FeProductCommentImage model);

        /// <summary>
        /// 更新晒单图片记录
        /// </summary>
        /// <param name="model">更新的数据模型</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-07-12 杨晗 创建</remarks>
        public abstract int Update(FeProductCommentImage model);

        /// <summary>
        /// 删除晒单图片记录
        /// </summary>
        /// <param name="sysNo">晒单图片系统编号</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks>2013-07-12 杨晗 创建</remarks>
        public abstract bool Delete(int sysNo);
    }
}
