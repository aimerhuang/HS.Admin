using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Web;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Oracle.Web
{
    /// <summary>
    /// 商品评论咨询
    /// </summary>
    /// <remarks>2013-08-09 邵斌 创建 </remarks>
    public class FeProductCommentDaoImpl : IFeProductCommentDao
    {
        /// <summary>
        /// 根据产品SysNo获取最先评论的5位用户
        /// </summary>
        /// <param name="productSysNo">产品SysNo</param>
        /// <returns>键值对(评论Id,客户昵称(或者客户用户名))</returns>
        /// <remarks>
        /// 2013-08-09 邵斌 创建
        /// </remarks>
        public override IDictionary<int, string> GetFirstReviewTop5(int productSysNo)
        {

            #region 测试SQL

            /*
             SELECT
                rm.SysNo
                , (CASE                                                 --取昵称时选用优先顺序为 昵称 - 账号 - 电话号码
                    WHEN length(c.nickname)=0 or c.nickname IS NULL     --如果昵称是否为空
                      THEN 
                        case                                            
                          when length(c.account)=0 or c.account is null --如果昵称为空，就判断用户账户是否为空
                          then
                            to_char(c.MobilePhoneNumber)                --如果账户也为空就使用注册电话号码
                          else
                            to_char(c.Account)                          --如果账户不为空就用账号
                        end            
                      ELSE to_char(c.nickname)                          --如果昵称不为空就用昵称 
                   END) AS Nickname
            FROM FeProductComment rm                                       --评价主表
              inner JOIN CrCustomer c                                      --客户表
                ON rm.CustomerSysNo = c.SysNo
            WHERE 
                rm.CommentStatus = 20
                AND rm.ProductSysNo = 1
                AND rownum<6
            ORDER BY rm.CommentTime
             */

            #endregion

            IDictionary<int, string> result = new Dictionary<int, string>();

            IList<dynamic> list = Context.Sql(@"
                                                SELECT
                                                    rm.SysNo
                                                    , (CASE                                                 --取昵称时选用优先顺序为 昵称 - 账号 - 电话号码
                                                        WHEN length(c.nickname)=0 or c.nickname IS NULL     --如果昵称是否为空
                                                          THEN 
                                                            case                                            
                                                              when length(c.account)=0 or c.account is null --如果昵称为空，就判断用户账户是否为空
                                                              then
                                                                c.MobilePhoneNumber                --如果账户也为空就使用注册电话号码
                                                              else
                                                                c.Account                          --如果账户不为空就用账号
                                                            end            
                                                          ELSE c.nickname                         --如果昵称不为空就用昵称 
                                                       END) AS Nickname
                                                FROM FeProductComment rm                                       --评价主表
                                                  inner JOIN CrCustomer c                                      --客户表
                                                    ON rm.CustomerSysNo = c.SysNo
                                                WHERE 
                                                    rm.CommentStatus = @0
                                                    AND rm.ProductSysNo = @1
                                                    AND rownum<6
                                                ORDER BY rm.CommentTime
                                        ", (int)ForeStatus.商品评论状态.已审, productSysNo).QueryMany<dynamic>();

            //构造返回结果
            foreach (var o in list)
            {
                result.Add((int)o.SYSNO, string.Format("{0}", o.NICKNAME));
            }

            return result;
        }

        /// <summary>
        /// 获取指定商品的评论次数详细情况
        /// 评分规则为：0-1分：差评   2-3分：一般    4-5：好评
        /// </summary>
        /// <param name="productSysNo"></param>
        /// <returns>返回总共评论次数，好评次数，一般次数，差评次数</returns>
        /// <remarks>2013-08-09 邵斌 创建</remarks>
        public override IDictionary<string, int> GetProductCommentTimesDetialInfo(int productSysNo)
        {
            #region 测试SQL

            /*
            select 'all' as name,count(sysno) as countNum from FeProductComment where productsysno=1                                        --全部评论次数
            union all
            select 'poor' as name, count(sysno) as countNum from FeProductComment where productsysno=1 and score between  0 and 1           --差评
            union all
            select 'ordinary' as name, count(sysno) as countNum from FeProductComment where productsysno=1 and score between  2 and 3       --一般
            union all
            select 'satisfied' as name, count(sysno) as countNum from FeProductComment where productsysno=1 and score between  4 and 5      --好评
            union all
            select 'shared' as name, count(sysno) as countNum from FeProductComment where productsysno=1 and IsShare=1                      --有晒单
             */

            #endregion

            IDictionary<string, int> result = new Dictionary<string, int>();

            IList<dynamic> list = Context.Sql(@"
                                    select 'all' as name,count(sysno) as countNum from FeProductComment where productsysno=@0 and (commentstatus=@1 or (sharestatus=@sharestatus and IsShare=1))
                                    union all
                                    select 'poor' as name, count(sysno) as countNum from FeProductComment where productsysno=@2 and commentstatus=@3 and score > -1 and score< 2
                                    union all
                                    select 'ordinary' as name, count(sysno) as countNum from FeProductComment where productsysno=@4 and commentstatus=@5 and score > 1 and score< 4
                                    union all
                                    select 'satisfied' as name, count(sysno) as countNum from FeProductComment where productsysno=@6 and commentstatus=@7 and score > 3 and  score< 6
                                    union all
                                    select 'shared' as name, count(sysno) as countNum from FeProductComment where productsysno=@8 and sharestatus=@9 and IsShare=1
            ", new object[]
                {
                    productSysNo, 
                    (int)ForeStatus.商品评论状态.已审,
                    (int) ForeStatus.商品晒单状态.已审,
                    productSysNo, 
                    (int)ForeStatus.商品评论状态.已审,
                    productSysNo, 
                    (int)ForeStatus.商品评论状态.已审,
                    productSysNo, 
                    (int)ForeStatus.商品评论状态.已审,
                    productSysNo,
                    (int)ForeStatus.商品晒单状态.已审
                }).QueryMany<dynamic>();

            //构造返回结果
            foreach (var o in list)
            {
                result.Add(string.Format("{0}", o.NAME), int.Parse(string.Format("{0}", o.COUNTNUM)));
            }

            return result;
        }

        /// <summary>
        /// 获取商品评价
        /// </summary>
        /// <param name="filter">商品系统编号</param>
        /// <param name="pager">分页参数</param>
        /// <returns></returns>
        /// <remarks>2013-08-12 邵斌 创建</remarks>
        public override void GetProductComment(ParaFeProductCommentFilter filter, ref Pager<CBFeProductComment> pager)
        {
            #region 测试SQL  获取商品评论

            /*
            select
                fc.*,cr.account  as Account,cr.nickname as Name,cr.mobilephonenumber as MobilePhoneNumber,crl.sysno as levelSysno,crl.levelname as levelname
            from 
                FeProductComment fc
                left join CrCustomer cr on cr.sysno = fc.customersysno
                inner join Crcustomerlevel crl on cr.levelsysno = crl.sysno
            where 
                fc.productsysno=1 and fc.commentstatus = 20
             */

            #endregion

            using (var context = Context.UseSharedConnection(true))
            {
                IList<CBFeProductComment> list = context.Select<CBFeProductComment>("fc.*,cr.account  as Account,cr.nickname as Name,cr.mobilephonenumber as MobilePhoneNumber,cr.headimage as HeadImage,crl.sysno as levelSysno,crl.levelname as levelname")
                    .From("FeProductComment fc left join CrCustomer cr on cr.sysno = fc.customersysno inner join Crcustomerlevel crl on cr.levelsysno = crl.sysno")
                    .Where("fc.productsysno=@0 and (fc.commentstatus = @1 or (@IsShare>-1 and sharestatus = @sharestatus)) and (@2 = -1 or fc.IsShare = @3) and (fc.score > (@4-1) and fc.score <(@5+1))")
                    .OrderBy("istop desc, fc.CommentTime desc")
                    .Parameter("0", filter.ProductSysNo)
                    .Parameter("1", (int)ForeStatus.商品评论状态.已审)
                    .Parameter("IsShare", filter.IsShare)
                    .Parameter("sharestatus", (int)ForeStatus.商品晒单状态.已审)
                    .Parameter("2", filter.IsShare)
                    .Parameter("3", filter.IsShare)
                    .Parameter("4", filter.StartSocre)
                    .Parameter("5", filter.EndSocre)
                    .Paging(pager.CurrentPage, pager.PageSize)
                    .QueryMany();

                foreach (var cbFeProductComment in list)
                {
                    #region 测试SQL 获取评论回复
                    /*
                    select 
                            fcr.*,cr.account  as Account,cr.nickname as Name,cr.mobilephonenumber as MobilePhoneNumber,cr.headimage as HeadImage, 
                    from
                            FeProductCommentReply fcr
                            left join CrCustomer cr on cr.sysno = fcr.customersysno
                    where
                            fcr.commentsysno = 1 and fcr.Status=20
                    */
                    #endregion

                    //评论回复
                    cbFeProductComment.Reply = context.Sql(@"
                            select 
                                  fcr.*,cr.account  as Account,cr.nickname as Name,cr.mobilephonenumber as MobilePhoneNumber,cr.headimage as HeadImage 
                            from
                                  FeProductCommentReply fcr
                                  left join CrCustomer cr on cr.sysno = fcr.customersysno
                            where
                                  fcr.commentsysno = @0 and fcr.Status = @1
                        ", cbFeProductComment.SysNo, (int)ForeStatus.商品评论回复状态.已审).QueryMany<CBFeProductCommentReply>();

                    #region 测试SQL 获取评论晒单图片

                    /*
                    select
                        fci.* 
                    from 
                        FeProductCommentImage fci
                    where
                        fci.commentsysno = 1  and and fci.Status=  20
                     */

                    #endregion

                    //商品晒单
                    cbFeProductComment.ShowMyProductImage = context.Sql(@"
                            select
                                fci.* 
                            from 
                                FeProductCommentImage fci
                            where
                                fci.commentsysno = @0 and fci.Status = @1
                        ", cbFeProductComment.SysNo, (int)ForeStatus.晒单图片状态.已审).QueryMany<FeProductCommentImage>();

                }

                pager.Rows = list;

                //统计记录数
                pager.TotalRows =
                    context.Sql(@"select 
                                        count(fc.sysno) as countRow 
                                  from 
                                        FeProductComment fc  
                                        left join CrCustomer cr on cr.sysno = fc.customersysno 
                                        inner join Crcustomerlevel crl on cr.levelsysno = crl.sysno
                                  where
                                        fc.productsysno=@0 and (fc.commentstatus = @1 or (@IsShare>-1 and sharestatus = @sharestatus)) and fc.IsShare>@2 and (@3<0 or fc.score between @4 and @5)
                                  order by fc.CommentTime desc")
                           .Parameter("0", filter.ProductSysNo)
                           .Parameter("1", (int)ForeStatus.商品评论状态.已审)
                           .Parameter("IsShare", filter.IsShare)
                           .Parameter("sharestatus", (int)ForeStatus.商品晒单状态.已审)
                           .Parameter("2", filter.IsShare - 1)
                           .Parameter("3", filter.IsShare)
                           .Parameter("4", filter.StartSocre)
                           .Parameter("5", filter.EndSocre)
                           .QuerySingle<int>();
            }
        }

        /// <summary>
        /// 评论回复
        /// </summary>
        /// <param name="feCommentSysNo">评论系统编号</param>
        /// <param name="content">评论回复内容</param>
        /// <param name="customerSysNo">回复人</param>
        /// <returns>返回 true:回复成功 false:回复失败</returns>
        /// <remarks>2013-08-13 邵斌 创建</remarks>
        public override Result Replay(int feCommentSysNo, string content, int customerSysNo)
        {
            //初始化数据
            FeProductCommentReply replay = new FeProductCommentReply()
                {
                    CommentSysNo = feCommentSysNo,
                    ReplyContent = content,
                    CustomerSysNo = customerSysNo,
                    ReplyDate = DateTime.Now,
                    Status = (int)ForeStatus.商品评论回复状态.待审
                };

            Result result = new Result();
            using (var context = Context.UseSharedConnection(true))
            {
                //更新商品评论表回复次数
                context.Sql("update FeProductComment set ReplyCount= ReplyCount+1 where sysno=@0", feCommentSysNo);

                //插入数据
                result.StatusCode = context.Insert<FeProductCommentReply>("FeProductCommentReply", replay)
                       .AutoMap(r => r.SysNo)
                       .ExecuteReturnLastId<int>("SysNo");

                result.Status = result.StatusCode > 0;
            }
            return result;

        }
    }
}
