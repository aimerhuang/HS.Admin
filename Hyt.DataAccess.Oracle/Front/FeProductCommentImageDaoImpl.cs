using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess;
using Hyt.DataAccess.Front;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Oracle.Front
{
    /// <summary>
    /// 晒单图片数据层实现类
    /// </summary>
    /// <remarks>2013-07-12 杨晗 创建</remarks>
    public class FeProductCommentImageDaoImpl : IFeProductCommentImageDao
    {
        /// <summary>
        /// 根据晒单图片系统编号获取实体
        /// </summary>
        /// <param name="sysNo">晒单图片系统编号</param>
        /// <returns>文章类型实体</returns>
        /// <remarks>2013-07-12 杨晗 创建</remarks>
        public override FeProductCommentImage GetModel(int sysNo)
        {
            return Context.Sql(@"select * from FeProductCommentImage where SysNO = @0", sysNo)
                          .QuerySingle<FeProductCommentImage>();
        }

        /// <summary>
        /// 根据晒单系统编号获取所属晒单图片
        /// </summary>
        /// <param name="commentSysNo">晒单系统编号</param>
        /// <returns>晒单图片集合</returns>
        /// <remarks>2013-07-12 杨晗 创建</remarks>
        public override IList<FeProductCommentImage> GetFeProductCommentImageByCommentSysNo(int commentSysNo)
        {
            #region sql条件

            string sql = @"(commentSysNo=@commentSysNo)";

            #endregion

            var countBuilder = Context.Select<FeProductCommentImage>("ft.*")
                                      .From("FeProductCommentImage ft")
                                      .Where(sql)
                                      .Parameter("commentSysNo", commentSysNo)
                                      .QueryMany();
            return countBuilder;
        }

        /// <summary>
        /// 根据晒单系统编号获取所属晒单图片
        /// </summary>
        /// <param name="commentSysNo">晒单系统编号</param>
        /// <param name="staus">晒单图片状态</param>
        /// <returns>晒单图片集合</returns>
        /// <remarks>2013-07-12 杨晗 创建</remarks>
        public override IList<FeProductCommentImage> GetFeProductCommentImageByCommentSysNo(int commentSysNo,
                                                                                            ForeStatus.晒单图片状态
                                                                                                staus)
        {
            #region sql条件

            string sql = @"commentSysNo=@commentSysNo and Status=@Status";

            #endregion

            var countBuilder = Context.Select<FeProductCommentImage>("ft.*")
                                      .From("FeProductCommentImage ft")
                                      .Where(sql)
                                      .Parameter("commentSysNo", commentSysNo)
                                      .Parameter("Status", staus)
                                      .QueryMany();
            return countBuilder;
        }

        /// <summary>
        /// 插入晒单图片记录
        /// </summary>
        /// <param name="model">插入的数据模型</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-07-12 杨晗 创建</remarks>
        public override int Insert(FeProductCommentImage model)
        {
            return Context.Insert<FeProductCommentImage>("FeProductCommentImage", model)
                          .AutoMap(x => x.SysNo)
                          .ExecuteReturnLastId<int>("Sysno");
        }

        /// <summary>
        /// 更新晒单图片记录
        /// </summary>
        /// <param name="model">更新的数据模型</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-07-12 杨晗 创建</remarks>
        public override int Update(FeProductCommentImage model)
        {
            return Context.Update<FeProductCommentImage>("FeProductCommentImage", model)
                          .AutoMap(x => x.SysNo)
                          .Where(x => x.SysNo)
                          .Execute();
        }

        /// <summary>
        /// 删除晒单图片记录
        /// </summary>
        /// <param name="sysNo">晒单图片系统编号</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks>2013-07-12 杨晗 创建</remarks>
        public override bool Delete(int sysNo)
        {
            var rowsAffected = Context.Delete("FeProductCommentImage")
                                      .Where("Sysno", sysNo)
                                      .Execute();
            return rowsAffected > 0;
        }
    }
}
