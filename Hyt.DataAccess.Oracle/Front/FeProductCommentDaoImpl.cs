using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess;
using Hyt.DataAccess.Front;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Model;

namespace Hyt.DataAccess.Oracle.Front
{
    /// <summary>
    /// 商品评论据访问 抽象类
    /// </summary>
    /// <remarks>2013-07-10 苟治国 创建</remarks>
    public class FeProductCommentDaoImpl : IFeProductCommentDao
    {
        /// <summary>
        /// 查看商品评论
        /// </summary>
        /// <param name="sysNo">商品评论编号</param>
        /// <returns>商品评论</returns>
        /// <remarks>2013－06-27 苟治国 创建</remarks>
        /// <remarks>2013-07-10 杨晗 修改</remarks>
        public override CBFeProductComment GetModel(int sysNo)
        {
            var sql = @"select ft.*,pt.productname,pt.erpcode,cc.name,cc.mobilephonenumber,cc.account 
from feproductcomment ft left outer join CrCustomer cc on ft.customersysno=cc.sysno left outer join pdproduct pt on ft.productsysno=pt.sysno
where ft.sysno=@sysNo";
            return Context.Sql(sql)
                          .Parameter("sysNo", sysNo)
                          .QuerySingle<CBFeProductComment>();
        }

        /// <summary>
        /// 查看商品评论/晒单
        /// </summary>
        /// <param name="orderSysNo">商品评论编号</param>
        /// <param name="productSysNo">商品评论编号</param>
        /// <param name="customerSysNo">商品评论编号</param>
        /// <returns>商品评论/晒单</returns>
        /// <remarks>2013-08-16 杨晗 创建</remarks>
        public override IList<CBFeProductComment> GetProductCommentList(int orderSysNo, int productSysNo,
                                                                        int customerSysNo)
        {
            var sql = @"select ft.*,pt.productname,pt.erpcode,cc.name,cc.mobilephonenumber,cc.account 
from feproductcomment ft left outer join CrCustomer cc on ft.customersysno=cc.sysno left outer join pdproduct pt on ft.productsysno=pt.sysno
where ft.orderSysNo=@orderSysNo and ft.productSysNo=@productSysNo and ft.customerSysNo=@customerSysNo ";

            return Context.Sql(sql)
                          .Parameter("orderSysNo", orderSysNo)
                          .Parameter("productSysNo", productSysNo)
                          .Parameter("customerSysNo", customerSysNo)
                          .QueryMany<CBFeProductComment>();
        }

        /// <summary>
        /// 获取已审核的前*条晒单信息
        /// </summary>
        /// <param name="count">条数</param>
        /// <returns>晒单列表</returns>
        /// <remarks>2013-08-21 杨晗 创建</remarks>
        /// <remarks>2013-12-24 黄波 修改了SQL语句,原SQL不能实现所要求的结果</remarks>
        /// <remarks>2013-01-23 邵斌 修改了SQL语句,原SQL会筛选出晒单主表通过审核但晒单图片确没有一个审核通过的记录</remarks>
        public override IList<FeProductComment> GetShareList(int count)
        {
            /* 原SQL
            const string sql =
                @"select t.* from feproductcomment t where t.isshare =:isshare and t.sharestatus=:sharestatus and rownum between 1 and :count order by t.sharetime desc";
             */
            //修改后的SQL

            //黄波版本SQL
            //const string sql = @"select * from (select t.* from feproductcomment t where t.isshare =:isshare and t.sharestatus=:sharestatus order by t.sharetime desc) where rownum between 1 and :count";


            #region 测试SQL

            /*
             select *
              from (select t.*
                      from feproductcomment t
                      inner join (select commentsysno from FeProductCommentImage where status=20 group by commentsysno) fci on t.sysno = fci.commentsysno  
                     where t.isshare = 1
                       and t.sharestatus = 20
                     order by t.sharetime desc)
             where rownum between 1 and 17
             * */

            #endregion

            const string sql = @" select *
              from (select t.*
                      from feproductcomment t
                      inner join (select commentsysno from FeProductCommentImage where status=@imgStatus group by commentsysno) fci on t.sysno = fci.commentsysno  
                     where t.isshare = @isshare
                       and t.sharestatus = @sharestatus
                     order by t.sharetime desc)
             where rownum between 1 and @count";
            return Context.Sql(sql)
                          .Parameter("imgStatus", (int)ForeStatus.晒单图片状态.已审)
                          .Parameter("isshare", (int)ForeStatus.是否晒单.是)
                          .Parameter("sharestatus", (int)ForeStatus.商品晒单状态.已审)
                          .Parameter("count", count)
                          .QueryMany<FeProductComment>();
        }

        /// <summary>
        /// 查看商品评论
        /// </summary>
        /// <param name="sysNo">商品评论编号</param>
        /// <returns>商品评论</returns>
        /// <remarks>2013－06-27 苟治国 创建</remarks>
        /// <remarks>2013-07-10 杨晗 修改</remarks>
        public override FeProductComment GetProductComment(int sysNo)
        {
            return Context.Sql("select * from FeProductComment where sysNo=@sysNo")
                          .Parameter("sysNo", sysNo)
                          .QuerySingle<FeProductComment>();
        }

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
        public override IList<CBFeProductComment> Seach(int pageIndex, int pageSize, int? isShare, int? isComment,
                                                        int? status,
                                                        int? isBest, int? isTop,
                                                        DateTime? beginDate, DateTime? endDate, out int count,
                                                        string customerName = null, string erpCode = null, string productSysNo = null)
        {
            #region sql条件

            string sqlWhere =
                @"((@IsShare is null or @IsShare=0) or fp.IsShare =@IsShare)
                 and ((@isComment is null or @isComment=0) or fp.isComment =@isComment)
                 and ((@isBest is null or @isBest=-1) or fp.isBest =@isBest)
                 and ((@isTop is null or @isTop=-1) or fp.isTop =@isTop) ";

                 if(customerName!=""&&customerName!=null)
                  sqlWhere +="  and cc.Name like @Name1 ";
                 if (erpCode != "" && erpCode!=null)
                    sqlWhere +="  and pt.ERPCODE =@erpCode ";
                 if(productSysNo!=""&& productSysNo!=null)
                     sqlWhere += " and pt.SysNo =@productSysNo ";
                 //--and ((@beginDate is null or fp.commentTime>= @beginDate) and (@endDate is null or fp.commentTime<=@endDate))
                
            isShare = isShare ?? 0;
            isComment = isComment ?? 0;
            if ((int)isShare == (int)ForeStatus.是否晒单.是)
            {
                sqlWhere += " and ((@Status is null or @Status=-1) or fp.ShareStatus =@Status)";
            }
            else if ((int)isComment == (int)ForeStatus.是否评论.是)
            {
                sqlWhere += " and ((@Status is null or @Status=-1) or fp.CommentStatus =@Status)";
            }

            #endregion

            using (var context = Context.UseSharedConnection(true))
            {
                if (beginDate == null || beginDate == DateTime.MinValue)
                    beginDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                else
                    sqlWhere += " and fp.commentTime>= @beginDate ";
                
                if (endDate == null || endDate == DateTime.MinValue)
                    endDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                else
                    sqlWhere += " and fp.commentTime<=@endDate ";
               
                count = context.Select<int>("count(1)")
                           .From("FeProductComment fp LEFT OUTER JOIN pdproduct pt ON fp.productsysno=pt.sysno LEFT OUTER JOIN CrCustomer cc ON fp.customersysno=cc.sysno")
                           .Where(sqlWhere)
                           .Parameter("IsShare", isShare)
                           .Parameter("isComment", isComment)
                           .Parameter("isBest", isBest)
                           .Parameter("isTop", isTop)
                           .Parameter("Name", customerName)
                           .Parameter("Name1", "%" + customerName + "%")
                           .Parameter("erpCode", erpCode)
                           .Parameter("ProductSysNo", productSysNo)
                           .Parameter("beginDate", beginDate)
                           .Parameter("endDate", endDate)
                           .Parameter("Status", status)
                           .QuerySingle();

                var list =
                    context.Select<CBFeProductComment>(
                        "fp.*,pt.productname,pt.erpcode,cc.name,cc.mobilephonenumber,cc.account")
                           .From("FeProductComment fp LEFT OUTER JOIN pdproduct pt ON fp.productsysno=pt.sysno LEFT OUTER JOIN CrCustomer cc ON fp.customersysno=cc.sysno")
                           .Where(sqlWhere)
                           .Parameter("IsShare", isShare)
                           .Parameter("isComment", isComment)
                           .Parameter("isBest", isBest)
                           .Parameter("isTop", isTop)
                           .Parameter("Name", customerName)
                           .Parameter("Name1", "%" + customerName + "%")
                           .Parameter("erpCode", erpCode)
                           .Parameter("ProductSysNo", productSysNo)
                           .Parameter("beginDate", beginDate)
                           .Parameter("endDate", endDate)
                           .Parameter("Status", status)
                           .Paging(pageIndex, pageSize).OrderBy("fp.SysNo desc").QueryMany();
                return list;
            }
        }

        /// <summary>
        /// 插入商品评论
        /// </summary>
        /// <param name="model">插入的数据模型</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－06-17 杨晗 创建</remarks>
        public override int Insert(FeProductComment model)
        {
            var id = Context.Insert<FeProductComment>("FeProductComment", model)
                            .AutoMap(x => x.SysNo)
                            .ExecuteReturnLastId<int>("Sysno");
            return id;
        }

        /// <summary>
        /// 更新商品评论
        /// </summary>
        /// <param name="model">更新的数据模型</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－06-17 杨晗 创建</remarks>
        public override int Update(FeProductComment model)
        {
            var rowsAffected = Context.Update<FeProductComment>("FeProductComment", model)
                                      .AutoMap(x => x.SysNo)
                                      .Where(x => x.SysNo)
                                      .Execute();
            return rowsAffected;
        }

        /// <summary>
        /// 删除商品评论
        /// </summary>
        /// <param name="sysNo">商品评论主键</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks>2013－06-17 杨晗 创建</remarks>
        public override bool Delete(int sysNo)
        {
            var rowsAffected = Context.Delete("FeProductComment")
                                      .Where("Sysno", sysNo)
                                      .Execute();
            return rowsAffected > 0;
        }

        /// <summary>
        /// 获取商品总评分
        /// </summary>
        /// <param name="productSysNo">商品系统号</param>
        /// <returns>总评分</returns>
        /// <remarks>2013-08-16 杨晗 创建</remarks>
        public override int GetScore(int productSysNo)
        {
            const string sql =
                @"select sum(Score) as scores from FeProductComment where productSysNo=@productSysNo
  and IsComment=@IsComment and CommentStatus=@CommentStatus
";
            return Context.Sql(sql)
                          .Parameter("productSysNo", productSysNo)
                          .Parameter("IsComment", (int)ForeStatus.是否评论.是)
                          .Parameter("CommentStatus", (int)ForeStatus.商品评论状态.已审)
                          .QuerySingle<int>();
        }

        /// <summary>
        /// 获取商品总评论条数
        /// </summary>
        /// <param name="productSysNo">商品系统号</param>
        /// <returns>总评论条数</returns>
        /// <remarks>2013-08-16 杨晗 创建</remarks>
        public override int GetReviewCount(int productSysNo)
        {
            const string sql =
                @"select count(1) from FeProductComment where productSysNo=@productSysNo
  and IsComment=@IsComment and CommentStatus=@CommentStatus
";
            return Context.Sql(sql)
                          .Parameter("productSysNo", productSysNo)
                          .Parameter("IsComment", (int)ForeStatus.是否评论.是)
                          .Parameter("CommentStatus", (int)ForeStatus.商品评论状态.已审)
                          .QuerySingle<int>();
        }

        /// <summary>
        /// 获取商品总晒单数
        /// </summary>
        /// <param name="productSysNo">商品系统号</param>
        /// <returns>总晒单数</returns>
        /// <remarks>2013-12-06 杨浩 创建</remarks>
        public override int GetShowCount(int productSysNo)
        {
            const string sql =
                @"select count(1) from FeProductComment where productSysNo=@productSysNo
  and IsShare=@IsShare and ShareStatus=@ShareStatus
";
            return Context.Sql(sql)
                          .Parameter("productSysNo", productSysNo)
                          .Parameter("IsShare", (int)ForeStatus.是否晒单.是)
                          .Parameter("ShareStatus", (int)ForeStatus.商品晒单状态.已审)
                          .QuerySingle<int>();
        }

        /// <summary>
        /// 晒单或评价商品列表 （订单下商品的晒单评价状态）
        /// </summary>
        /// <param name="isComment">是否为评论</param>
        /// <param name="customerSysNo">会员系统编号</param>
        /// <param name="pager">分页实体</param>
        /// <remarks>
        /// 2013-08-29 郑荣华 创建
        /// 2013-12-23 杨浩 修改sql 
        /// </remarks> 
        public override void GetProductShowOrComment(int? isComment, int customerSysNo, ref Pager<CBSoOrderItem> pager)
        {
            int pageStart = (pager.CurrentPage - 1) * pager.PageSize + 1;
            int pageEnd = pager.CurrentPage * pager.PageSize;
            using (var context = Context.UseSharedConnection(true))
            {
                #region 条件

                const string sql = @"select * from
                    (
                        select row_number() over (order by t.sysno) FLUENTDATA_ROWNUMBER,t.*        
                            from  soorderitem t                             
                            inner join soorder m on t.ordersysno=m.sysno                               
                            where  m.customersysno=@CustomerSysNo and m.status=@status
                    )
                    where fluentdata_RowNumber between @pageStart and @pageEnd
                    order by fluentdata_RowNumber";

                #endregion

                #region 执行

                const string sqlCount = @"select count(1) from soorderitem t                                         
                                        inner join soorder m on t.ordersysno=m.sysno where m.customersysno=@CustomerSysNo and m.status=@status";

                pager.TotalRows = context.Sql(sqlCount)
                                       .Parameter("CustomerSysNo", customerSysNo)
                                       .Parameter("status", (int)OrderStatus.销售单状态.已完成)
                                       .QuerySingle<int>();


                pager.Rows = context.Sql(sql)
                                    .Parameter("CustomerSysNo", customerSysNo)
                                    .Parameter("status", (int)OrderStatus.销售单状态.已完成)
                                    .Parameter("pageStart", pageStart)
                                    .Parameter("pageEnd", pageEnd)
                                    .QueryMany<CBSoOrderItem>();

                #endregion
            }
        }
    }
}
