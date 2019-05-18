using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Print;
using Hyt.DataAccess.Weixin;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Oracle.Weixin
{

    /// <summary>
    /// 微信咨询实现类
    /// </summary>
    /// <remarks>
    /// 2013-07-16 郑荣华 创建
    /// </remarks>
    public class MkWeixinQuestionDaoImpl : IMkWeixinQuestionDao
    {
        #region 操作

        /// <summary>
        /// 添加微信咨询信息
        /// </summary>
        /// <param name="model">微信咨询实体类</param>
        /// <returns>返回新增的系统编号</returns>
        /// <remarks>
        /// 2013-11-07 郑荣华 创建
        /// </remarks>
        public override int Create(MkWeixinQuestion model)
        {
            return Context.Insert("MkWeixinQuestion", model)
                          .AutoMap(x => x.SysNo)
                          .ExecuteReturnLastId<int>("SysNo");
        }

        /// <summary>
        /// 修改微信咨询信息
        /// </summary>
        /// <param name="model">微信咨询实体类</param>
        /// <returns>受影响的行数</returns>
        /// <remarks>
        /// 2013-11-07 郑荣华 创建
        /// </remarks>
        public override int Update(MkWeixinQuestion model)
        {
            return Context.Update("MkWeixinQuestion", model)
                          .AutoMap(x => x.SysNo)
                          .Where(x => x.SysNo)
                          .Execute();
        }

        /// <summary>
        /// 微信咨询消息状态更新
        /// </summary>
        /// <param name="weixinId">微信号</param>
        /// <param name="status">微信咨询消息状态</param>
        /// <returns>受影响的行数</returns>
        /// <remarks>
        /// 2013-11-07 郑荣华 创建
        /// </remarks>
        public override int UpdateStatus(string weixinId, MarketingStatus.微信咨询消息状态 status)
        {
            return Context.Sql("update MkWeixinQuestion set status=@0 where WeixinId=@1", (int)status, weixinId)
                .Execute();
        }

        /// <summary>
        /// 删除微信咨询信息
        /// </summary>
        /// <param name="templateSysNo">微信咨询系统编号</param>
        /// <returns>受影响的行数</returns>
        /// <remarks>
        /// 2013-11-07 郑荣华 创建
        /// </remarks>
        public override int Delete(int templateSysNo)
        {
            return Context.Delete("MkWeixinQuestion")
                          .Where("SysNo", templateSysNo)
                          .Execute();
        }

        #endregion

        #region 查询

        /// <summary>
        /// 分页查询微信咨询信息列表
        /// </summary>
        /// <param name="pager">微信咨询列表分页对象</param>
        /// <param name="weixinId">微信号</param>
        /// <returns>微信咨询列表信息</returns>
        /// <remarks>
        /// 2013-11-07 郑荣华 创建
        /// </remarks>
        public override void GetMkWeixinQuestionList(ref Pager<CBMkWeixinQuestion> pager, string weixinId)
        {
            using (var context = Context.UseSharedConnection(true))
            {
                const string sqlSelect = @"t.*,CASE t.ReplyerSysNo WHEN null THEN weixinid ELSE b.account END ShowName";
                const string sqlFrom = @"MkWeixinQuestion t left join syuser b on t.ReplyerSysNo=b.sysno";

                const string sqlWhere = @"(@weixinId is null or t.weixinId= @weixinId)";


                #region sqlcount
                const string sqlCount = @" select count(1) from MkWeixinQuestion t where " + sqlWhere;

                pager.TotalRows = context.Sql(sqlCount)
                                         .Parameter("weixinId", weixinId)
                                         .QuerySingle<int>();
                #endregion

                pager.Rows = context.Select<CBMkWeixinQuestion>(sqlSelect)
                                    .From(sqlFrom)
                                    .Where(sqlWhere)
                                    .Parameter("weixinId", weixinId)
                                    .OrderBy("t.sysno desc")
                                    .Paging(pager.CurrentPage, pager.PageSize)
                                    .QueryMany();
            }
        }

        /// <summary>
        /// 分页查询微信咨询信息（统计）列表
        /// </summary>
        /// <param name="pager">微信咨询列表分页对象</param>
        /// <param name="filter">查询条件</param>
        /// <remarks> 
        /// 2013-11-07 郑荣华 创建
        /// 2013-12-31 陶辉 修改 
        /// </remarks>
        public override void GetMkWeixinQuestionStaticsList(ref Pager<CBMkWeixinQuestion> pager, ParaMkWeixinQuestionFilter filter)
        {
            using (var context = Context.UseSharedConnection(true))
            {
                const string sqlSelect = @"t.*,CASE b.newsnum when null then 0 else b.newsnum END newsnum";

                const string sqlFrom = @"MkWeixinQuestion t left join
                (select t.weixinid,count(1) newsnum from MkWeixinQuestion t where t.status=@status and t.type=@type group by t.weixinid)b 
                on t.weixinid=b.weixinid";

                const string sqlWhere = @"t.type=@type and exists (select 1 from 
                (select t.weixinid,max(t.messagestime) lasttime  from MkWeixinQuestion t where t.type=@type group by t.weixinid) a
                where t.weixinid=a.weixinid and t.messagestime=a.lasttime) 
                --and (@weixinid is null or charindex(t.weixinid,@weixinid)>0) 
                and (@startdate is null or t.messagestime>=@startdate) and (@enddate is null or t.messagestime<=@enddate)";

                #region sqlcount

                const string sqlCount = @"select count(1) from " + sqlFrom + " where " + sqlWhere;

                pager.TotalRows = context.Sql(sqlCount)
                                    .Parameter("status", (int)MarketingStatus.微信咨询消息状态.未读)
                                    .Parameter("type", (int)MarketingStatus.微信咨询类型.咨询)
                    //.Parameter("weixinid", filter.WeixinId)
                    //.Parameter("weixinid", filter.WeixinId)
                                    .Parameter("startdate", filter.StartTime)
                                    .Parameter("enddate", filter.EndTime)
                                    .QuerySingle<int>();
                #endregion

                pager.Rows = context.Select<CBMkWeixinQuestion>(sqlSelect)
                                    .From(sqlFrom)
                                    .Where(sqlWhere)
                                    .Parameter("status", (int)MarketingStatus.微信咨询消息状态.未读)
                                    .Parameter("type", (int)MarketingStatus.微信咨询类型.咨询)
                    //.Parameter("weixinid", filter.WeixinId)
                    //.Parameter("weixinid", filter.WeixinId)
                                    .Parameter("startdate", filter.StartTime)
                                    .Parameter("enddate", filter.EndTime)
                                    .OrderBy("t.MessagesTime desc")
                                    .Paging(pager.CurrentPage, pager.PageSize)
                                    .QueryMany();
            }
        }
        #endregion
    }
}
