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
    /// 商品回复数据层实现类
    /// </summary>
    /// <remarks>2013-07-10 杨晗 创建</remarks>
    public class FeProductCommentReplyDaoImpl : IFeProductCommentReplyDao
    {
        /// <summary>
        /// 根据回复系统编号获取实体
        /// </summary>
        /// <param name="sysNo">商品评论回复系统编号</param>
        /// <returns>文章实体</returns>
        /// <remarks>2013-07-10 杨晗 创建</remarks>
        public override FeProductCommentReply GetModel(int sysNo)
        {
            return Context.Sql(@"select * from FeProductCommentReply where SysNO = @0", sysNo)
                          .QuerySingle<FeProductCommentReply>();
        }

        /// <summary>
        /// 根据商品评论系统号获取旗下所有评论回复
        /// </summary>
        /// <param name="commentSysNo">商品评论系统号</param>
        /// <returns>评论回复列表</returns>
        /// <remarks>2013-08-19 杨晗 创建</remarks>
        public override IList<FeProductCommentReply> GetReplyByCommentSysNo(int commentSysNo)
        {
            return Context.Sql(@"select * from FeProductCommentReply where COMMENTSYSNO = @0", commentSysNo)
                          .QueryMany<FeProductCommentReply>();
        }

        /// <summary>
        /// 根据商品评论系统号获取旗下所有评论回复
        /// </summary>
        /// <param name="commentSysNo">商品评论系统号</param>
        /// <param name="staus">商品评论回复状态</param>
        /// <returns>评论回复列表</returns>
        /// <remarks>2013-08-19 杨晗 创建</remarks>
        public override IList<FeProductCommentReply> GetReplyByCommentSysNo(int commentSysNo, ForeStatus.商品评论回复状态 staus)
        {
            return Context.Sql(@"select * from FeProductCommentReply where COMMENTSYSNO = @0 and Status=@1", commentSysNo, (int)staus)
                          .QueryMany<FeProductCommentReply>();
        }

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
        public override IList<CBFeProductCommentReply> Seach(int pageIndex, int pageSize, int? commentSysNo,
                                                             ForeStatus.商品评论回复状态? searchStaus, out int count)
        {
            #region sql条件

            var sql =
                @"((@CommentSysNo is null or @CommentSysNo=0) or fr.CommentSysNo = @CommentSysNo)
                       and ((@Status is null or @Status=0) or fr.Status =@Status)";

            #endregion


            IList<CBFeProductCommentReply> countBuilder;
            using (var _context = Context.UseSharedConnection(true))
            {
                count = _context.Select<int>("count(1)")
                                .From("FeProductCommentReply fr LEFT OUTER JOIN crcustomer cc ON fr.customersysno=cc.sysno")
                                .Where(sql)
                                .Parameter("CommentSysNo", commentSysNo)
                                .Parameter("Status", searchStaus)
                                .QuerySingle();

                countBuilder = _context.Select<CBFeProductCommentReply>("fr.*,cc.name")
                                       .From("FeProductCommentReply fr LEFT OUTER JOIN crcustomer cc ON fr.customersysno=cc.sysno")
                                       .Where(sql)
                                       .Parameter("CommentSysNo", commentSysNo)
                                       .Parameter("Status", searchStaus)
                                       .Paging(pageIndex, pageSize).OrderBy("fr.sysno desc").QueryMany();
            }
            return countBuilder;
        }

        /// <summary>
        /// 插入商品评论回复
        /// </summary>
        /// <param name="model">插入的数据模型</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-07-10 杨晗 创建</remarks>
        public override int Insert(FeProductCommentReply model)
        {
            return Context.Insert<FeProductCommentReply>("FeProductCommentReply", model)
                          .AutoMap(x => x.SysNo)
                          .ExecuteReturnLastId<int>("Sysno");
        }

        /// <summary>
        /// 更新商品评论回复
        /// </summary>
        /// <param name="model">更新的数据模型</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-07-10 杨晗 创建</remarks>
        public override int Update(FeProductCommentReply model)
        {
            return Context.Update<FeProductCommentReply>("FeProductCommentReply", model)
                          .AutoMap(x => x.SysNo)
                          .Where(x => x.SysNo)
                          .Execute();
        }

        /// <summary>
        /// 删除商品评论回复
        /// </summary>
        /// <param name="sysNo">商品评论回复系统号</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks>2013-07-10 杨晗 创建</remarks>
        public override bool Delete(int sysNo)
        {
            var count = Context.Delete("FeProductCommentReply")
                               .Where("Sysno", sysNo)
                               .Execute();
            return count > 0;
        }
    }
}
