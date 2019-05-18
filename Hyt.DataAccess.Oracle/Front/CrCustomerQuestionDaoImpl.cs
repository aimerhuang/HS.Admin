using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess;
using Hyt.DataAccess.Front;
using Hyt.Model.Parameter;
using Hyt.Model.WorkflowStatus;
using Hyt.Model;

namespace Hyt.DataAccess.Oracle.Front
{
    /// <summary>
    /// 会员咨询
    /// </summary>
    /// <remarks>2013-06-25 苟治国 添加</remarks>
    public class CrCustomerQuestionDaoImpl : ICrCustomerQuestionDao
    {
        /// <summary>
        /// 查看会员咨询
        /// </summary>
        /// <param name="sysNo">会员咨询编号</param>
        /// <returns>会员咨询</returns>
        /// <remarks>2013－06-25 苟治国 创建</remarks>
        public override Model.CrCustomerQuestion GetModel(int sysNo)
        {
            return Context.Sql(@"select* from crcustomerquestion where sysno=@0", sysNo).QuerySingle<Model.CrCustomerQuestion>();
        }

        /// <summary>
        /// 集合查看商品咨询
        /// </summary>
        /// <param name="sysNo">会员咨询编号</param>
        /// <returns>会员咨询</returns>
        /// <remarks>2013－06-25 苟治国 创建</remarks>
        public override Model.CBCrCustomerQuestion GetGroupModel(int sysNo)
        {
            #region strSql

            const string strSql = @"select cq.*,pp.productname,cc.sysno as customerno,cc.MobilePhoneNumber,su.UserName from crcustomerquestion cq 
                              inner join pdproduct pp on cq.productsysno=pp.sysno 
                              inner join crcustomer cc on cq.customersysno=cc.sysno
                              left join syuser su on cq.answersysno=su.sysno where cq.sysno=@0";

            #endregion

            return Context.Sql(strSql, sysNo).QuerySingle<Model.CBCrCustomerQuestion>();
        }

        /// <summary>
        /// 查询会员咨询列表
        /// </summary>
        /// <param name="pager">会员咨询列表分页对象</param>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        /// <remarks> 
        /// 2013-08-21 郑荣华 创建
        /// </remarks>
        public override void GetCrCustomerQuestionList(ref Pager<CBCrCustomerQuestion> pager, ParaCrCustomerQuestionFilter filter)
        {
            using (var context = Context.UseSharedConnection(true))
            {
                const string sqlWhere = @"(@QuestionType is null or cq.QuestionType =@QuestionType)
                                         and (@Status is null or cq.Status =@Status)
                                         and (@customersysno is null or cq.customersysno =@customersysno)
                                         and (@ProductSysNo is null or cq.ProductSysNo =@ProductSysNo)
                                         and (@ProductName is null or charindex(pp.ProductName,@ProductName)>=0)
                                         and (@beginDate is null or questionDate>= @beginDate) 
                                         and (@endDate is null or questionDate<=@endDate)                                      
                                        ";

                #region sqlcount

                const string sqlFrom = @"crcustomerquestion cq inner join pdproduct pp on cq.productsysno=pp.sysno 
                                        inner join crcustomer cc on cq.customersysno=cc.sysno 
                                        left join syuser su on cq.answersysno=su.sysno";

                const string sqlCount = @"select count(1) from crcustomerquestion cq inner join pdproduct pp on cq.productsysno=pp.sysno 
                                        inner join crcustomer cc on cq.customersysno=cc.sysno where " + sqlWhere;

                pager.TotalRows = context.Sql(sqlCount)
                                       .Parameter("QuestionType", filter.QuestionType)
                                       .Parameter("Status", filter.Status)
                                       .Parameter("customersysno", filter.CustomerSysNo)
                                       .Parameter("ProductSysNo", filter.ProductSysNo)
                                       .Parameter("ProductName", filter.ProductName)
                                       .Parameter("beginDate", filter.BeginDate)
                                       .Parameter("endDate", filter.EndDate)
                                       .QuerySingle<int>();
                #endregion

                pager.Rows = context.Select<CBCrCustomerQuestion>("cq.*,pp.productname,cc.sysno as customerno,cc.MobilePhoneNumber,cc.nickname,su.UserName")
                                    .From(sqlFrom)
                                    .Where(sqlWhere)
                                    .Parameter("QuestionType", filter.QuestionType)
                                    .Parameter("Status", filter.Status)
                                    .Parameter("customersysno", filter.CustomerSysNo)
                                    .Parameter("ProductSysNo", filter.ProductSysNo)
                                    .Parameter("ProductName", filter.ProductName)
                                    .Parameter("beginDate", filter.BeginDate)
                                    .Parameter("endDate", filter.EndDate)
                                    .OrderBy("cq.QuestionDate")
                                    .Paging(pager.CurrentPage, pager.PageSize)
                                    .QueryMany();
            }
        }

        /// <summary>
        /// 根据条件获取会员咨询的列表
        /// </summary>
        /// <param name="pageIndex">分页索引</param>
        /// <param name="pageSize">分页码</param>
        /// <param name="type">会员咨询类型</param>
        /// <param name="staus">会员咨询状态</param>
        /// <param name="customersysno">会员编号</param>
        /// <param name="productSysNo">商品编号</param>
        /// <param name="productName">商品名称</param>
        /// <param name="beginDate">查询开始时间</param>
        /// <param name="endDate">查询结束时间</param>
        /// <returns>会员咨询列表</returns>
        /// <remarks>2013－06-25 苟治国 创建</remarks>
        public override IList<Model.CBCrCustomerQuestion> Seach(int pageIndex, int pageSize, int? type, int? staus, int? customersysno, string productSysNo = null, string productName = null, string beginDate = null, string endDate = null)
        {
            #region 原语句
            //select cq.*,pp.productname,cc.sysno as customerno from crcustomerquestion cq inner join pdproduct pp on cq.productsysno=pp.sysno inner join crcustomer cc on cq.customersysno=cc.sysno
            #endregion

            #region sql条件
            string sqlWhere = @"(@QuestionType=-1 or cq.QuestionType =@QuestionType)
                             and (@Status=-1 or cq.Status =@Status)
                             and (@customersysno=-1 or cq.customersysno =@customersysno)
                             and (@ProductSysNo is null or cq.ProductSysNo like @ProductSysNo1)
                             and (@ProductName is null or pp.ProductName like @ProductName1)
                             ";


            #endregion
            if (!string.IsNullOrEmpty(beginDate))
            {
                sqlWhere += "and (questionDate>= @beginDate) ";
            }
            else
            {
                beginDate = System.Data.SqlTypes.SqlDateTime.MinValue.ToString();
            }
            if (!string.IsNullOrEmpty(endDate))
            {
                sqlWhere += "and (questionDate<=@endDate)";
            }
            else
            {
                endDate = System.Data.SqlTypes.SqlDateTime.MaxValue.ToString();
            }
            var list = Context.Select<CBCrCustomerQuestion>("cq.*,pp.productname,cc.sysno as customerno,cc.MobilePhoneNumber,su.UserName")
                              .From("crcustomerquestion cq inner join pdproduct pp on cq.productsysno=pp.sysno inner join crcustomer cc on cq.customersysno=cc.sysno left join syuser su on cq.answersysno=su.sysno")
                              .Where(sqlWhere)
                              .Parameter("QuestionType", type)
                              .Parameter("Status", staus)
                              .Parameter("customersysno", customersysno)
                              .Parameter("ProductSysNo", productSysNo)
                              .Parameter("ProductSysNo1", "%" + productSysNo + "%")
                              .Parameter("ProductName", productName)
                              .Parameter("ProductName1", "%" + productName + "%")
                              .Parameter("beginDate", Convert.ToDateTime(beginDate))
                              .Parameter("endDate", Convert.ToDateTime(endDate))
                              .Paging(pageIndex, pageSize).OrderBy("cq.QuestionDate desc").QueryMany();
            return list;
        }

        /// <summary>
        /// 根据条件获取会员咨询的总条数
        /// </summary>
        /// <param name="type">会员咨询类型</param>
        /// <param name="staus">会员咨询状态</param>
        /// <param name="customersysno">会员编号</param>
        /// <param name="productSysNo">商品编号</param>
        /// <param name="productName">商品名称</param>
        /// <param name="beginDate">查询开始时间</param>
        /// <param name="endDate">查询结束时间</param>
        /// <returns>总数</returns>
        /// <remarks>2013-06-25 苟治国 创建</remarks>
        public override int GetCount(int? type, int? staus, int? customersysno, string productSysNo = null, string productName = null, string beginDate = null, string endDate = null)
        {
            #region sql条件
            string sqlWhere = @"(@QuestionType=-1 or cq.QuestionType =@QuestionType)
                             and (@Status=-1 or cq.Status =@Status)
                             and (@customersysno=-1 or cq.customersysno =@customersysno)
                             and (@ProductSysNo is null or cq.ProductSysNo like @ProductSysNo1)
                             and (@ProductName is null or pp.ProductName like @ProductName1)
                             and ((@beginDate is null or questionDate>= @beginDate) and (@endDate is null or questionDate<=@endDate))";
            #endregion

            if (beginDate == null || Convert.ToDateTime(beginDate) == DateTime.MinValue)
            {
                beginDate = System.Data.SqlTypes.SqlDateTime.MinValue.ToString();
            }
            if (endDate == null || Convert.ToDateTime(endDate) == DateTime.MinValue)
            {
                endDate = System.Data.SqlTypes.SqlDateTime.MinValue.ToString();
            }

            var list = Context.Select<int>("count(1)")
                              .From("crcustomerquestion cq inner join pdproduct pp on cq.productsysno=pp.sysno inner join crcustomer cc on cq.customersysno=cc.sysno left join syuser su on cq.answersysno=su.sysno")
                              .Where(sqlWhere)
                              .Parameter("QuestionType", type)
                              .Parameter("Status", staus)
                              .Parameter("customersysno", customersysno)
                              .Parameter("ProductSysNo", productSysNo)
                              .Parameter("ProductSysNo1", "%" + productSysNo + "%")
                              .Parameter("ProductName", productName)
                              .Parameter("ProductName1", "%" + productName + "%")
                              .Parameter("beginDate", Convert.ToDateTime(beginDate))
                              .Parameter("endDate", Convert.ToDateTime(endDate))
                              .QuerySingle();
            return list;
        }

        /// <summary>
        /// 新增商品咨询
        /// </summary>
        /// <param name="model">商品咨询明细</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-06-25 苟治国 创建</remarks>
        public override int Insert(Model.CrCustomerQuestion model)
        {
            var result = Context.Insert<CrCustomerQuestion>("CrCustomerQuestion", model)
                                .AutoMap(x => x.SysNo)
                                .ExecuteReturnLastId<int>("SysNo");
            return result;
        }

        /// <summary>
        /// 更新商品咨询
        /// </summary>
        /// <param name="model">商品咨询明细</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-06-25 苟治国 创建</remarks>
        public override int Update(Model.CrCustomerQuestion model)
        {
            int rowsAffected = Context.Update<Model.CrCustomerQuestion>("CrCustomerQuestion", model)
                                      .AutoMap(x => x.SysNo)
                                      .Where(x => x.SysNo)
                                      .Execute();
            return rowsAffected;
        }

        /// <summary>
        /// 删除商品咨询
        /// </summary>
        /// <param name="sysNo">商品咨询编号</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks>2013-06-25 苟治国 创建</remarks>
        public override bool Delete(int sysNo)
        {
            int rowsAffected = Context.Delete("CrCustomerQuestion")
                                      .Where("sysNo", sysNo)
                                      .Execute();
            return rowsAffected > 0;
        }
        /// <summary>
        /// 获得对应状态的商品咨询数目
        /// </summary>
        /// <param name="Status">状态</param>
        /// <returns>数目</returns>
        /// <remarks>2016-03-19 王耀发 创建</remarks>
        public override int GetCusQuestionCounts(int Status)
        {
            string sql = "crcustomerquestion cq inner join pdproduct pp on cq.productsysno=pp.sysno inner join crcustomer cc on cq.customersysno=cc.sysno left join syuser su on cq.answersysno=su.sysno where cq.Status = @Status";
            var dataCount = Context.Select<int>("count(1)").From(sql)
                .Parameter("Status", Status);
            int TotalRows = dataCount.QuerySingle();
            return TotalRows;
        }
        /// <summary>
        /// 分页获取申请列表
        /// </summary>
        /// <param name="pager"></param>
        /// <param name="filter"></param>
        public override void GetDsDealerApplyWebList(ref Pager<DsDealerApplyWeb> pager, ParaDsDealerApplyWebFilter filter)
        {
            using (var context = Context.UseSharedConnection(true))
            {
                const string sqlSelect = @" * ";

                const string sqlFrom = @" DsDealerApplyWeb ";
                string sqlWhere = " DealerSysNo=" + filter.DealerSysno;
                if (filter.Status != 0)
                {
                    sqlWhere += @" and Status=" + filter.Status;
                }

                #region sqlcount

                string sqlCount = @" select count(1) from DsDealerApplyWeb where " + sqlWhere;

                pager.TotalRows = context.Sql(sqlCount)
                                         .QuerySingle<int>();
                #endregion

                pager.Rows = context.Select<DsDealerApplyWeb>(sqlSelect)
                                    .From(sqlFrom)
                                    .Where(sqlWhere)
                                    .OrderBy(" CreatedDate desc ")
                                    .Paging(pager.CurrentPage, pager.PageSize)
                                    .QueryMany();
            }
        }
        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="SysNo"></param>
        /// <param name="Status"></param>
        public override void UpdateDsDealerApplyWeStatus(int SysNo, int Status)
        {
            Context.Update("DsDealerApplyWeb")
                .Column("Status", Status)
                .Where("SysNo", SysNo).Execute();
        }
    }
}
