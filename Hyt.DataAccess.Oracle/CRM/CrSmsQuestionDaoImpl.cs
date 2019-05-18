using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.CRM;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Oracle.CRM
{
    /// <summary>
    /// 短信咨询
    /// </summary>
    /// <remarks>2014-02-21 邵斌 创建</remarks>
    public class CrSmsQuestionDaoImpl : ICrSmsQuestionDao
    {
        /// <summary>
        /// 创建短信咨询
        /// </summary>
        /// <param name="entity">短信实体</param>
        /// <returns>新增记录系统编号</returns>
        /// <remarks>2014-02-21 邵斌 创建</remarks>
        public override int Create(CrSmsQuestion entity)
        {
            return Context.Insert("CrSmsQuestion", entity).AutoMap(o => o.SysNo).ExecuteReturnLastId<int>("SysNo");
        }

        /// <summary>
        /// 获取咨询短信信息
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>短信实体</returns>
        /// <remarks>2014-02-21 邵斌 创建</remarks>
        public override CBCrSmsQuestion Get(int sysNo)
        {
            return Context.Select<CBCrSmsQuestion>("csq.*,cc.account as Customer").From("CrSmsQuestion csq left join CrCustomer cc on csq.customersysno=cc.sysno").Where("csq.sysno=@sysno").Parameter("sysno", sysNo).QuerySingle();
        }

        /// <summary>
        /// 根据手机号码获取短信咨询
        /// </summary>
        /// <param name="mobileNumber">手机号码</param>
        /// <returns>短信实体列表</returns>
        /// <remarks>2014-02-21 邵斌 创建</remarks>
        public override IList<CrSmsQuestion> Get(string mobileNumber)
        {
            return
                Context.Select<CrSmsQuestion>("*")
                       .From("CrSmsQuestion")
                       .Where("MobilePhoneNumber=@MobilePhoneNumber")
                       .Parameter("MobilePhoneNumber", mobileNumber)
                       .QueryMany();
        }

        /// <summary>
        /// 根据单条短信系统编号获取同一手机的短信咨询列表
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>短信实体列表</returns>
        /// <remarks>2014-02-21 邵斌 创建</remarks>
        public override IList<CBCrSmsQuestion> GetSmsQuestionListBySysNo(int sysNo)
        {
            /*
             * 测试SQL 通过一个问题的系统编号找出这个人（同一电话号码）其他的问题
                select * from
                CrSmsQuestion csq 
                inner join (select mobilephonenumber from CrSmsQuestion where sysno=3) t on csq.mobilephonenumber = t.mobilephonenumber

             * */

            return Context.Sql(@"select csq.*,cc.account as Customer,su.account as answername from
                CrSmsQuestion csq 
                inner join (select mobilephonenumber from CrSmsQuestion where sysno=@sysno) t on csq.mobilephonenumber = t.mobilephonenumber
                left join CrCustomer cc on csq.customersysno=cc.sysno                
                left join syuser su on csq.AnswerSysNo = su.sysno 
                where csq.sysno <= @sysno
                order by csq.sysno asc")
             .Parameter("sysno", sysNo)
             .QueryMany<CBCrSmsQuestion>();
        }

        /// <summary>
        /// 回复咨询
        /// </summary>
        /// <param name="sysNo">咨询系统编号</param>
        /// <param name="answerBy">回答人</param>
        /// <param name="answerContent">回答内容</param>
        /// <returns>回复结果</returns>
        /// <remarks>2014-02-21 邵斌 创建</remarks>
        public override Result Answer(int sysNo, int answerBy, string answerContent)
        {
            using (var content = Context.UseSharedConnection(true))
            {
                Result result = new Result();

                var model = content.Select<CrSmsQuestion>("*").From("CrSmsQuestion").Where("sysno = @sysno").Parameter("sysno", sysNo).QuerySingle();
                model.AnswerDate = DateTime.Now;
                model.AnswerSysNo = answerBy;
                model.Answer = answerContent;
                model.Status = (int)CustomerStatus.短信咨询状态.已回复;


                result.Status = (content.Update<CrSmsQuestion>("CrSmsQuestion", model)
                    .AutoMap(o => o.SysNo)
                    .Where("sysno", sysNo)
                    .Execute() > 0);

                return result;
            }
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="sysNo">咨询系统编号</param>
        /// <param name="status">短信咨询状态</param>
        /// <returns>结果</returns>
        /// <remarks>2014-02-21 邵斌 创建</remarks>
        public override Result UpdateStatus(int sysNo, CustomerStatus.短信咨询状态 status)
        {
            Result result = new Result();

            result.Status = (Context.Sql("update CrSmsQuestion set Status=@0 where sysno=@1", (int)status, sysNo).Execute() > 0);

            return result;
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="status">咨询状态</param>
        /// <param name="mobileNumber">咨询人电话</param>
        /// <param name="questionContent">咨询内容</param>
        /// <param name="answer">回复人</param>
        /// <param name="pager">分页对象</param>
        /// <returns>咨询列表</returns>
        /// <remarks>2014-02-21 邵斌 创建</remarks>
        public override IList<CBCrSmsQuestion> List(int? status, string mobileNumber, string answer, string questionContent, Pager<CBCrSmsQuestion> pager)
        {
            /*
             * 测试SQL
             select * from
            CrSmsQuestion csq
            where
            (:status=0 or csq.status=:status) 
            and (:MobilePhoneNumber is null or charindex(csq.MobilePhoneNumber,:MobilePhoneNumber) > 0 )
            and (:Question is null or charindex(csq.Question,:Question) > 0 )
            and (:Answersysno =0 or csq.Answersysno=:Answersysno)
             */

            status = status ?? 0;

            using (var content = Context.UseSharedConnection(true))
            {
                int answerSysNo = 0;
                if (!string.IsNullOrWhiteSpace(answer))
                {
                    answerSysNo = content.Sql("select sysno from syuser where Account=@Account", answer.Trim()).QuerySingle<int>();
                }

                pager.Rows =
                    content.Select<CBCrSmsQuestion>("csq.*,cc.account as Customer")
                           .From("CrSmsQuestion csq left join CrCustomer cc on csq.customersysno=cc.sysno")
                           .Where(@"(@status=0 or csq.status=@status) 
                                and ((@MobilePhoneNumber is null or charindex(csq.MobilePhoneNumber,@MobilePhoneNumber) > 0 ) or (@Question is null or charindex(csq.Question,@Question) > 0 ))            
                                and (@Answersysno =0 or csq.Answersysno=@Answersysno)")
                           .Parameter("status", status.Value)
                           .Parameter("MobilePhoneNumber", mobileNumber)
                           .Parameter("Question", questionContent)
                           .Parameter("Answersysno", answerSysNo)
                           .OrderBy("csq.sysno desc")
                           .Paging(pager.CurrentPage, pager.PageSize)
                           .QueryMany();

                pager.TotalRows =
                    content.Sql(@"select count(csq.sysno) from CrSmsQuestion csq left join CrCustomer cc on csq.customersysno=cc.sysno where 
                            (@status=0 or csq.status=@status) 
                            and ((@MobilePhoneNumber is null or charindex(csq.MobilePhoneNumber,@MobilePhoneNumber) > 0 ) or (@Question is null or charindex(csq.Question,@Question) > 0 ))   
                            and (@Answersysno =0 or csq.Answersysno=@Answersysno)")
                           .Parameter("status", status.Value)
                           .Parameter("MobilePhoneNumber", mobileNumber)
                           .Parameter("Question", questionContent)
                           .Parameter("Answersysno", answerSysNo)
                           .QuerySingle<int>();
            }

            return pager.Rows;
        }
    }
}
