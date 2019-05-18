using System;
using System.Collections.Generic;
using Hyt.DataAccess.Base;
using Hyt.DataAccess.Union;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;

namespace Hyt.DataAccess.Oracle.Union
{
    /// <summary>
    ///     微信DAOImpl
    /// </summary>
    /// <remarks>黄伟 2013-10-23 创建</remarks>
    public class WeChatDaoImpl : IWeChatDao
    {
        #region 微信自动回复配置

        /// <summary>
        ///     create
        /// </summary>
        /// <param name="model">MkWeixinConfig</param>
        /// <returns>SysNo of the created</returns>
        /// <remarks>黄伟 2013-10-23 创建</remarks>
        public override int CreateConfiguration(MkWeixinConfig model)
        {
            if (model.LastUpdateDate == DateTime.MinValue)
            {
                model.LastUpdateDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            return Context.Insert("MkWeixinConfig", model).AutoMap(p => p.SysNo).ExecuteReturnLastId<int>("SysNo");
        }

        /// <summary>
        ///     update
        /// </summary>
        /// <param name="model">MkWeixinConfig</param>
        /// <returns>返回受影响的行数(更新)</returns>
        /// <remarks>黄伟 2013-11-15 创建</remarks>
        public override int UpdateConfiguration(MkWeixinConfig model)
        {
            return Context.Update("MkWeixinConfig", model).AutoMap(p => p.SysNo).Where(p => p.SysNo).Execute();
        }

        /// <summary>
        /// 获取微信自动回复配置
        /// </summary>
        /// <returns>MkWeixinConfig</returns>
        /// <remarks>2013-11-14 陶辉 创建</remarks>
        public override MkWeixinConfig GetMkWeixinConfig()
        {
            return
                Context.Sql("select * from MkWeixinConfig")
                       .QuerySingle<MkWeixinConfig>();
        }

        /// <summary>
        /// 获取对应分销商微信自动回复配置
        /// </summary>
        /// <returns>MkWeixinConfig</returns>
        /// <remarks>2013-11-14 陶辉 创建</remarks>
        public override MkWeixinConfig GetMkWeixinConfig(int AgentSysNo, int DealerSysNo)
        {
            return
                Context.Sql("select * from MkWeixinConfig where AgentSysNo = @AgentSysNo and DealerSysNo = @DealerSysNo")
                       .Parameter("AgentSysNo", AgentSysNo)
                       .Parameter("DealerSysNo", DealerSysNo)
                       .QuerySingle<MkWeixinConfig>();
        }

        /// <summary>
        /// 分页查询分销商信息列表
        /// </summary>
        /// <param name="pager">分销商信息列表分页对象</param>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        /// <remarks> 
        /// 2016-04-28 王耀发 创建
        /// </remarks>
        public override void GetMkWeixinConfigList(ref Pager<CBMkWeixinConfig> pager, ParaMkWeixinConfigFilter filter)
        {
            using (var context = Context.UseSharedConnection(true))
            {
                const string sqlSelect = @"t.*,dea.DealerName";

                const string sqlFrom = @"MkWeixinConfig t left join DsDealer dea on t.DealerSysNo = dea.SysNo";
                string sqlWhere = "1=1";

                //判断是否绑定所有分销商
                if (!filter.IsBindAllDealer)
                {
                    //判断是否绑定分销商
                    if (filter.IsBindDealer)
                    {
                        sqlWhere += " and dea.SysNo = " + filter.DealerSysNo;
                    }
                    else
                    {
                        sqlWhere += " and dea.CreatedBy = " + filter.DealerCreatedBy;
                    }
                }
                if (filter.SelectedAgentSysNo != -1)
                {
                    if (filter.SelectedDealerSysNo != -1)
                    {
                        sqlWhere += " and dea.SysNo = " + filter.SelectedDealerSysNo;
                    }
                    else
                    {
                        sqlWhere += " and dea.CreatedBy = " + filter.SelectedAgentSysNo;
                    }
                }
                sqlWhere += @" and (@Token is null or charindex(t.Token,@Token)>0)                           
                                          ";

                #region sqlcount

                string sqlCount = @" select count(1) from MkWeixinConfig t left join DsDealer dea on t.DealerSysNo = dea.SysNo where " + sqlWhere;

                pager.TotalRows = context.Sql(sqlCount)
                                         .Parameter("Token", filter.Token)
                                         .QuerySingle<int>();
                #endregion

                pager.Rows = context.Select<CBMkWeixinConfig>(sqlSelect)
                                    .From(sqlFrom)
                                    .Where(sqlWhere)
                                    .Parameter("Token", filter.Token)
                                    .OrderBy("t.sysno desc")
                                    .Paging(pager.CurrentPage, pager.PageSize)
                                    .QueryMany();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        /// <remarkss>2015-08-06 王耀发 创建</remarks>
        public override MkWeixinConfig GetEntity(int SysNo)
        {

            return Context.Sql("select * from MkWeixinConfig  where SysNo=@SysNo")
                   .Parameter("SysNo", SysNo)
              .QuerySingle<MkWeixinConfig>();
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2015-08-21  王耀发 创建</remarks>
        public override int Insert(MkWeixinConfig entity)
        {
            entity.SysNo = Context.Insert("MkWeixinConfig", entity)
                                        .AutoMap(o => o.SysNo)
                                        .ExecuteReturnLastId<int>("SysNo");
            return entity.SysNo;
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>修改记录编号</returns>
        /// <remarks>2015-08-21  王耀发 创建</remarks>
        public override int Update(MkWeixinConfig entity)
        {

            return Context.Update("MkWeixinConfig", entity)
                   .AutoMap(o => o.SysNo)
                   .Where("SysNo", entity.SysNo)
                   .Execute();
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>删除记录</returns>
        /// <remarks>2015-08-30 王耀发 创建</remarks>
        public override int Delete(int sysNo)
        {
            return Context.Delete("MkWeixinConfig")
                               .Where("SysNo", sysNo)
                               .Execute();
        }

        #endregion

        #region 微信关键字管理

        /// <summary>
        ///     query 微信关键字列表
        /// </summary>
        /// <param name="para">MkWeixinKeywords</param>
        /// <param name="id">page index</param>
        /// <param name="pageSize">page size</param>
        /// <returns>list of MkWeixinKeywords</returns>
        /// <remarks>2013-10-25 hw created</remarks>
        public override Dictionary<int, List<CBMkWeixinKeywords>> QueryKeyWords(ParaMkWeixinKeywords para, int id = 1,
                                                                              int pageSize = 10)
        {
            string sqlWhere = " and 1=1";

            //判断是否绑定所有分销商
            if (!para.IsBindAllDealer)
            {
                //判断是否绑定分销商
                if (para.IsBindDealer)
                {
                    sqlWhere += " and dea.SysNo = " + para.DealerSysNo;
                }
                else
                {
                    sqlWhere += " and dea.CreatedBy = " + para.DealerCreatedBy;
                }
            }
            if (para.SelectedAgentSysNo != -1)
            {
                if (para.SelectedDealerSysNo != -1)
                {
                    sqlWhere += " and dea.SysNo = " + para.SelectedDealerSysNo;
                }
                else
                {
                    sqlWhere += " and dea.CreatedBy = " + para.SelectedAgentSysNo;
                }
            }

            string sqlSelect = @"a.*,dea.DealerName",
                   sqlFrom = @"MkWeixinKeywords a left join DsDealer dea on a.DealerSysNo = dea.SysNo",

                //关键词进行查找
                //sqlCondition = @"(:keyWords is null or a.keyWords=:keyWords)";
                //sqlCondition = @"(:keyWords is null)"; 
                   sqlCondition = string.Format(@"(@keyWords is null 
                                                or keywords like '{0}' -- single  
                                                or keywords like '{0};%' --starts with
                                                or keywords like '%;{0}'--ends with 
                                                or keywords like '%;{0};'--ends with
                                                or keywords like '%;{0};%'--in the middle 
                                                )", para.Keywords);
            sqlCondition += sqlWhere;

            using (IDbContext context = Context.UseSharedConnection(true))
            {
                List<CBMkWeixinKeywords> lstResult = context.Select<CBMkWeixinKeywords>(sqlSelect)
                                                          .From(sqlFrom)
                                                          .AndWhere(sqlCondition)
                                                          .Parameter("keyWords", para.Keywords)
                                                          .Paging(id, pageSize) //index从1开始
                                                          .OrderBy("a.sysno desc")
                                                          .QueryMany();
                int count = context.Select<int>(@"count(*)")
                                   .From(sqlFrom)
                                   .AndWhere(sqlCondition)
                                   .Parameter("keyWords", para.Keywords)
                                   .QuerySingle();
                return new Dictionary<int, List<CBMkWeixinKeywords>> { { count, lstResult } };
            }
        }

        /// <summary>
        ///     根据系统编号获取MkWeixinKeywords
        /// </summary>
        /// <param name="sysNo">sysno of MkWeixinKeywords</param>
        /// <returns>MkWeixinKeywords</returns>
        /// <remarks>2013-10-24 黄伟 创建</remarks>
        public override MkWeixinKeywords GetMkWeixinKeywordsBySysNo(int sysNo)
        {
            return
                Context.Sql("select * from MkWeixinKeywords where sysno=@sysNo")
                       .Parameter("sysNo", sysNo)
                       .QuerySingle<MkWeixinKeywords>();
        }

        /// <summary>
        ///     根据关键词系统编号获取MkWeixinKeywordsReply
        /// </summary>
        /// <param name="sysNo">sysno of MkWeixinKeywords</param>
        /// <returns>list of MkWeixinKeywordsReply</returns>
        /// <remarks>2013-10-24 黄伟 创建</remarks>
        public override List<MkWeixinKeywordsReply> GetContentByKeyWords(int sysNo)
        {
            return
                Context.Sql("select * from MkWeixinKeywordsReply where WeixinKeywordsSysNo=@WeixinKeywordsSysNo")
                       .Parameter("WeixinKeywordsSysNo", sysNo)
                       .QueryMany<MkWeixinKeywordsReply>();
        }

        /// <summary>
        ///     根据关键词获取回复内容
        /// </summary>
        /// <param name="keyWords">关键词</param>
        /// <returns>MkWeixinKeywords</returns>
        /// <remarks>2013-10-24 黄伟 创建</remarks>
        public override List<MkWeixinKeywordsReply> GetMkWeixinReplyByKeyWords(string keyWords)
        {
            string sql = string.Format(@"select * from MkWeixinKeywordsReply r 
                                    where r.weixinkeywordssysno in
                                    (
                                        select sysno from MkWeixinKeywords 
                                        where keywords like '{0}' -- single  
                                        or keywords like '{0};%' --starts with
                                        or keywords like '%;{0}'--ends with 
                                        or keywords like '%;{0};'--ends with
                                        or keywords like '%;{0};%'--in the middle  
                                    )", keyWords);
            return Context.Sql(sql).QueryMany<MkWeixinKeywordsReply>();
        }

        /// <summary>
        ///     新增微信关键字
        /// </summary>
        /// <param name="model">MkWeixinKeywords</param>
        /// <param name="operatorSysNo">操作人员编号</param>
        /// <returns>Result instance</returns>
        /// <remarks>2013-10-24 黄伟 创建</remarks>
        public override int CreateKeyWords(MkWeixinKeywords model, int operatorSysNo)
        {
            model.CreatedBy = operatorSysNo;
            model.CreatedDate = DateTime.Now;
            model.LastUpdateBy = operatorSysNo;
            model.LastUpdateDate = DateTime.Now;
            model.Status = MarketingStatus.微信关键词状态.启用.GetHashCode();
            //model.ReplyCount++;

            return Context.Insert("MkWeixinKeywords", model).AutoMap(m => m.SysNo).ExecuteReturnLastId<int>("SysNo");
        }

        /// <summary>
        ///     更新微信关键字
        /// </summary>
        /// <param name="models">list of MkWeixinKeywords</param>
        /// <param name="operatorSysNo">操作人员编号</param>
        /// <returns></returns>
        /// <remarks>2013-10-24 黄伟 创建</remarks>
        public override void UpdateKeyWords(List<MkWeixinKeywords> models, int operatorSysNo)
        {
            models.ForEach(model =>
                {
                    model.LastUpdateBy = operatorSysNo;
                    model.LastUpdateDate = DateTime.Now;

                    Context.Update("MkWeixinKeywords")
                           .Column("Keywords", model.Keywords)
                           .Column("Status", model.Status)
                           .Column("LastUpdateBy", model.LastUpdateBy)
                           .Column("LastUpdateDate", model.LastUpdateDate)
                           .Where("SysNo", model.SysNo).Execute();
                });
        }

        /// <summary>
        ///     删除微信关键字
        /// </summary>
        /// <param name="lstDelSysNos">要删除的微信关键字编号集合</param>
        /// <returns></returns>
        /// <remarks>2013-10-24 黄伟 创建</remarks>
        public override void DeleteKeyWords(List<int> lstDelSysNos)
        {
            using (IDbContext ctx = Context.UseSharedConnection(true))
            {
                //del related content
                ctx.Sql("delete MkWeixinKeywordsReply where WeixinKeywordsSysNo in(@lstDelSysnos)")
                   .Parameter("lstDelSysnos", lstDelSysNos)
                   .Execute();

                ctx.Sql("delete MkWeixinKeywords where sysno in(@lstDelSysnos)")
                   .Parameter("lstDelSysnos", lstDelSysNos)
                   .Execute();
            }
        }

        /// <summary>
        ///     设置微信关键字状态启用/禁用
        /// </summary>
        /// <param name="sysNo">MkWeixinKeywords sysno</param>
        /// <param name="status">启用或禁用</param>
        /// <param name="operatorSysNo">操作人员编号</param>
        /// <returns>void</returns>
        /// <remarks>2013-10-24 黄伟 创建</remarks>
        public override void SetKeyWordsStatus(int sysNo, int status, int operatorSysNo)
        {
            Context.Update("MkWeixinKeywords")
                   .Column("Status", status)
                   .Column("LastUpdateBy", operatorSysNo)
                   .Column("LastUpdateDate", DateTime.Now)
                   .Where("SysNo", sysNo)
                   .Execute();
        }

        #endregion

        #region 微信关键字对应内容管理

        /// <summary>
        ///     query 微信关键字所对应任务列表
        /// </summary>
        /// <param name="para">MkWeixinKeywordsReply</param>
        /// <param name="id">page index</param>
        /// <param name="pageSize">page size</param>
        /// <returns>list of MkWeixinKeywordsReply</returns>
        /// <remarks>2013-10-25 hw created</remarks>
        public override Dictionary<int, List<MkWeixinKeywordsReply>> QueryKeyWordsContent(MkWeixinKeywordsReply para,
                                                                                          int id = 1, int pageSize = 10)
        {
            string sqlSelect = @"a.*",
                   sqlFrom = @"MkWeixinKeywordsReply a",
                //关键词进行查找
                   sqlCondition = @"(@WeixinKeywordsSysNo is null or a.WeixinKeywordsSysNo=@WeixinKeywordsSysNo)";

            using (IDbContext context = Context.UseSharedConnection(true))
            {
                List<MkWeixinKeywordsReply> lstResult = context.Select<MkWeixinKeywordsReply>(sqlSelect)
                                                               .From(sqlFrom)
                                                               .AndWhere(sqlCondition)
                                                               .Parameter("WeixinKeywordsSysNo",
                                                                          para.WeixinKeywordsSysNo)
                                                               .Paging(id, pageSize) //index从1开始
                                                               .OrderBy("a.sysno desc")
                                                               .QueryMany();
                int count = context.Select<int>(@"count(*)")
                                   .From(sqlFrom)
                                   .AndWhere(sqlCondition)
                                   .Parameter("WeixinKeywordsSysNo", para.WeixinKeywordsSysNo)
                                   .QuerySingle();
                return new Dictionary<int, List<MkWeixinKeywordsReply>> { { count, lstResult } };
            }
        }

        /// <summary>
        ///     query 微信关键字所对应任务列表
        /// </summary>
        /// <param name="para">MkWeixinKeywordsReply</param>
        /// <returns>list of MkWeixinKeywordsReply</returns>
        /// <remarks>2013-10-25 hw created</remarks>
        public override List<MkWeixinKeywordsReply> QueryKeyWordsContentAll(MkWeixinKeywordsReply para)
        {
            return Context.Sql("select * from MkWeixinKeywordsReply").QueryMany<MkWeixinKeywordsReply>();
        }

        /// <summary>
        ///     根据系统编号获取MkWeixinKeywordsReply
        /// </summary>
        /// <param name="sysNo">sysno of MkWeixinKeywordsReply</param>
        /// <returns>MkWeixinKeywordsReply</returns>
        /// <remarks>2013-10-24 黄伟 创建</remarks>
        public override MkWeixinKeywordsReply GetMkWeixinKeywordsReplyBySysNo(int sysNo)
        {
            return
                Context.Sql("select * from MkWeixinKeywordsReply where sysno=@sysNo")
                       .Parameter("sysNo", sysNo)
                       .QuerySingle<MkWeixinKeywordsReply>();
        }

        /// <summary>
        ///     新增微信关键字
        /// </summary>
        /// <param name="model">MkWeixinKeywords</param>
        /// <param name="operatorSysNo">操作人员编号</param>
        /// <returns>Result instance</returns>
        /// <remarks>2013-10-24 黄伟 创建</remarks>
        public override int CreateKeyWordsContent(MkWeixinKeywordsReply model, int operatorSysNo)
        {
            model.CreatedBy = operatorSysNo;
            model.CreatedDate = DateTime.Now;
            model.LastUpdateBy = operatorSysNo;
            model.LastUpdateDate = DateTime.Now;
            model.Status = MarketingStatus.微信关键词回复状态.启用.GetHashCode();

            Context.Sql("update MkWeixinKeywords set ReplyCount=ReplyCount+1 where sysno=@sysno")
                   .Parameter("sysno", model.WeixinKeywordsSysNo).Execute();
            return Context.Insert("MkWeixinKeywordsReply", model).AutoMap(m => m.SysNo).Execute();
        }

        /// <summary>
        ///     更新微信关键字对应内容
        /// </summary>
        /// <param name="models">list of MkWeixinKeywordsReply</param>
        /// <param name="operatorSysNo">操作人员编号</param>
        /// <returns></returns>
        /// <remarks>2013-10-24 黄伟 创建</remarks>
        public override void UpdateKeyWordsContent(List<MkWeixinKeywordsReply> models, int operatorSysNo)
        {
            //just update the required columns
            models.ForEach(model =>
                {
                    model.LastUpdateBy = operatorSysNo;
                    model.LastUpdateDate = DateTime.Now;

                    Context.Update("MkWeixinKeywordsReply")
                           .Column("Title", model.Title)
                        //.Column("ReplyType", model.ReplyType)
                           .Column("Content", model.Content)
                           .Column("Image", model.Image)
                           .Column("Hyperlink", model.Hyperlink)
                        //.Column("Status", model.Status)
                           .Column("LastUpdateBy", model.LastUpdateBy)
                           .Column("LastUpdateDate", model.LastUpdateDate)
                           .Where("SysNo", model.SysNo).Execute();
                });
        }

        /// <summary>
        ///     删除微信关键字对应内容
        /// </summary>
        /// <param name="lstDelSysNos">要删除的微信关键字对应内容编号集合</param>
        /// <returns></returns>
        /// <remarks>2013-10-24 黄伟 创建</remarks>
        public override void DeleteKeyWordsContent(List<int> lstDelSysNos)
        {
            int keyWordsSysNo = GetMkWeixinKeywordsReplyBySysNo(lstDelSysNos[0]).WeixinKeywordsSysNo;

            using (IDbContext ctx = Context.UseSharedConnection(true))
            {
                ctx.Sql("delete MkWeixinKeywordsReply where sysno in(@lstDelSysnos)")
                   .Parameter("lstDelSysnos", lstDelSysNos)
                   .Execute();
                //update 关键词对应回复内容条数
                ctx.Sql("update MkWeixinKeywords set ReplyCount=ReplyCount-@count where sysno =@sysno")
                   .Parameter("count", lstDelSysNos.Count)
                   .Parameter("sysno", keyWordsSysNo)
                   .Execute();
            }
        }

        /// <summary>
        ///     设置微信关键字回复内容条目状态启用/禁用
        /// </summary>
        /// <param name="sysNo">MkWeixinKeywords sysno</param>
        /// <param name="status">启用或禁用</param>
        /// <param name="operatorSysNo">操作人员编号</param>
        /// <returns></returns>
        /// <remarks>2013-10-24 黄伟 创建</remarks>
        public override void SetKeyWordsContentStatus(int sysNo, int status, int operatorSysNo)
        {
            Context.Update("MkWeixinKeywordsReply")
                   .Column("Status", status)
                   .Column("LastUpdateBy", operatorSysNo)
                   .Column("LastUpdateDate", DateTime.Now)
                   .Where("SysNo", sysNo)
                   .Execute();
        }

        /// <summary>
        ///     检查关键词是否存在
        /// </summary>
        /// <param name="keyWords">关键词</param>
        /// <returns>true:exist;false:not  exist</returns>
        /// <remarks>2013-10-24 黄伟 创建</remarks>
        public override bool IsKeyWordsExist(string keyWords)
        {
            return
                Context.Sql("select count(*) from MkWeixinKeywords where keyWords=@keyWords")
                       .Parameter("keyWords", keyWords)
                       .QuerySingle<int>() > 0;
        }

        #endregion
    }
}