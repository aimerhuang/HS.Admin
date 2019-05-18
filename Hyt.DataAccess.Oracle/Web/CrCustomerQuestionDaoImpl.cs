using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Util;
using Hyt.Model;
using Hyt.DataAccess.Web;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Model.Parameter;

namespace Hyt.DataAccess.Oracle.Web
{
    /// <summary>
    /// 会员咨询
    /// </summary>
    /// <remarks>2013-08-08 苟治国 创建</remarks>
    public class CrCustomerQuestionDaoImpl : ICrCustomerQuestionDao
    {
        /// <summary>
        /// 获取指定会员咨询信息
        /// </summary>
        /// <param name="pager">咨询查询条件</param>
        /// <returns>咨询订单列表</returns>
        /// <remarks>2013-08-08 苟治国 创建</remarks>
        public override Pager<CBCrCustomerQuestion> GetQuestion(Pager<CBCrCustomerQuestion> pager)
        {
            #region 测试SQL
            //select cq.*,pp.productname,pp.productimage,cc.sysno as customerno,cc.MobilePhoneNumber,su.UserName from crcustomerquestion cq inner join pdproduct pp on cq.productsysno=pp.sysno inner join crcustomer cc on cq.customersysno=cc.sysno left join syuser su on cq.answersysno=su.sysno where cq.customersysno=2
            #endregion

            #region sql条件
            string sqlWhere = @"(@customersysno=-1 or cq.customersysno =@customersysno)";// and (:beginTime is null or createDate>=:beginTime) and (:endTime is null or createDate<=:endTime)
            #endregion

            using (var _context = Context.UseSharedConnection(true))
            {
                pager.Rows = _context.Select<CBCrCustomerQuestion>("cq.*,pp.productname,pp.productimage,cc.sysno as customerno,cc.MobilePhoneNumber,su.UserName")
                                    .From("crcustomerquestion cq inner join pdproduct pp on cq.productsysno=pp.sysno inner join crcustomer cc on cq.customersysno=cc.sysno left join syuser su on cq.answersysno=su.sysno")
                                    .Where(sqlWhere)
                                    .Parameter("customersysno", pager.PageFilter.CustomerSysNo)
                                    .Paging(pager.CurrentPage, pager.PageSize).OrderBy("cq.QuestionDate desc").QueryMany();

                pager.TotalRows = _context.Select<int>("count(1)")
                                    .From("crcustomerquestion cq inner join pdproduct pp on cq.productsysno=pp.sysno inner join crcustomer cc on cq.customersysno=cc.sysno left join syuser su on cq.answersysno=su.sysno")
                                    .Where(sqlWhere)
                                    .Parameter("customersysno", pager.PageFilter.CustomerSysNo)
                                    .QuerySingle();
            }
            return pager;
        }

        /// <summary>
        /// 获取指定会员咨询信息
        /// </summary>
        /// <param name="pager">咨询查询条件</param>
        /// <returns>咨询订单列表</returns>
        /// <remarks>
        /// 2013-08-13 邵  斌 筛选商品和问题类型
        /// </remarks>
        public override Pager<CBCrProductDetailCustomerQuestion> GetQuestion(Pager<CBCrProductDetailCustomerQuestion> pager)
        {
            #region 测试SQL
            //select cq.*,pp.productname,pp.productimage,cc.sysno as customerno,cc.MobilePhoneNumber,su.UserName,cc.levelsysno,crl.levelname from crcustomerquestion cq inner join pdproduct pp on cq.productsysno=pp.sysno inner join crcustomer cc on cq.customersysno=cc.sysno inner join crcustomerlevel crl on cc.levelsysno = crl.sysno left join syuser su on cq.answersysno=su.sysno where cq.customersysno=2
            #endregion

            #region sql条件
            string sqlWhere = @"(@productsysno=0 or cq.productsysno=@productsysno) and (@customersysno=-1 or cq.customersysno =@customersysno) and (@questionType =-1 or QuestionType=@questionType) and cq.status=@status";// and (:beginTime is null or createDate>=:beginTime) and (:endTime is null or createDate<=:endTime)
            #endregion

            using (var _context = Context.UseSharedConnection(true))
            {
                pager.Rows = _context.Select<CBCrProductDetailCustomerQuestion>("cq.*,cc.MobilePhoneNumber,su.UserName,cc.levelsysno,crl.levelname,cc.HeadImage,cc.Account as CustomerAccount,cc.nickname as CustomerNickName")
                                    .From(@"
                                                crcustomerquestion cq 
                                                inner join crcustomer cc on cq.customersysno=cc.sysno 
                                                inner join CrCustomerLevel crl on cc.levelsysno = crl.sysno
                                                left join syuser su on cq.answersysno=su.sysno"
                                    )
                                    .Where(sqlWhere)
                                    .Parameter("productsysno", pager.PageFilter.ProductSysNo)
                                    .Parameter("customersysno", pager.PageFilter.CustomerSysNo)
                                    .Parameter("questionType", pager.PageFilter.QuestionType)
                                    .Parameter("status", (int)CustomerStatus.会员咨询状态.已回复)
                                    .Paging(pager.CurrentPage, pager.PageSize).OrderBy("cq.QuestionDate desc").QueryMany();

                pager.TotalRows = _context.Select<int>("count(1)")
                                    .From("crcustomerquestion cq inner join crcustomer cc on cq.customersysno=cc.sysno inner join CrCustomerLevel crl on cc.levelsysno = crl.sysno left join syuser su on cq.answersysno=su.sysno")
                                    .Where(sqlWhere)
                                    .Parameter("productsysno", pager.PageFilter.ProductSysNo)
                                    .Parameter("customersysno", pager.PageFilter.CustomerSysNo)
                                    .Parameter("questionType", pager.PageFilter.QuestionType)
                                    .Parameter("status", (int)CustomerStatus.会员咨询状态.已回复)
                                    .QuerySingle();
            }

            return pager;
        }

        /// <summary>
        /// 获取商品的咨询次数信息
        /// </summary>
        /// <param name="productSysNo"></param>
        /// <returns>返回各种统计次</returns>
        /// <remarks>2013-08-09 邵斌 创建</remarks>
        public override IDictionary<string, int> GetCustomerQuestionsCountInfo(int productSysNo)
        {
            #region 测试SQL
            /*
            select 'All' as name,count(sysno) as countNum from CrCustomerQuestion where productsysno=1 and Status=20
            union all
            select 'Product' as name,count(sysno) as countNum from CrCustomerQuestion where productsysno=1 and QuestionType=10 and Status=20
            union all
            select 'Pay' as name,count(sysno) as countNum from CrCustomerQuestion where productsysno=1 and QuestionType=20 and Status=20
            union all
            select 'Express' as name,count(sysno) as countNum from CrCustomerQuestion where productsysno=1 and QuestionType=30 and Status=20
            union all
            select 'Other' as name,count(sysno) as countNum from CrCustomerQuestion where productsysno=1 and QuestionType=40 and Status=20
             */
            #endregion

            IDictionary<string, int> result = new Dictionary<string, int>();

            IList<dynamic> list = Context.Sql(@"
                                    select 'All' as name,count(sysno) as countNum from CrCustomerQuestion where productsysno=@0 and Status=@status
                                    union all
                                    select 'Product' as name,count(sysno) as countNum from CrCustomerQuestion where productsysno=@1 and QuestionType=@type and Status=@status
                                    union all
                                    select 'Pay' as name,count(sysno) as countNum from CrCustomerQuestion where productsysno=@2 and QuestionType=@type and Status=@status
                                    union all
                                    select 'Express' as name,count(sysno) as countNum from CrCustomerQuestion where productsysno=@3 and QuestionType=@type and Status=@status
                                    union all
                                    select 'Other' as name,count(sysno) as countNum from CrCustomerQuestion where productsysno=@4 and QuestionType=@type and Status=@status
            ", new object[]
                {
                  productSysNo, 
                  (int)CustomerStatus.会员咨询状态.已回复,
                  productSysNo, 
                  (int)CustomerStatus.会员咨询类型.商品,
                  (int)CustomerStatus.会员咨询状态.已回复,
                  productSysNo, 
                  (int)CustomerStatus.会员咨询类型.支付,
                  (int)CustomerStatus.会员咨询状态.已回复,
                  productSysNo, 
                  (int)CustomerStatus.会员咨询类型.配送,
                  (int)CustomerStatus.会员咨询状态.已回复,
                  productSysNo,
                  (int)CustomerStatus.会员咨询类型.其他,
                  (int)CustomerStatus.会员咨询状态.已回复
                }).QueryMany<dynamic>();

            //构造返回结果
            foreach (var o in list)
            {
                result.Add(string.Format("{0}", o.NAME), int.Parse(string.Format("{0}", o.COUNTNUM)));
            }

            return result;
        }

        /// <summary>
        /// 获取商品的咨询次数
        /// </summary>
        /// <param name="productSysNo"></param>
        /// <returns>咨询次数</returns>
        /// <remarks>2013-12-23 杨浩 创建</remarks>
        public override int GetCustomerQuestionsCount(int productSysNo)
        {
            var count = Context.Sql(@"select count(1) from CrCustomerQuestion where productsysno=@0 and Status=@status ")
                               .Parameter("productsysno", productSysNo)
                               .Parameter("Status", (int)CustomerStatus.会员咨询状态.已回复)
                               .QuerySingle<int>();
            return count;
        }

        /// <summary>
        /// 新增商品咨询
        /// </summary>
        /// <param name="model">商品咨询明细</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-11-19 苟治国 创建</remarks>
        public override int Insert(Model.CrCustomerQuestion model)
        {
            var result = Context.Insert<CrCustomerQuestion>("CrCustomerQuestion", model)
                                .AutoMap(x => x.SysNo)
                                .ExecuteReturnLastId<int>("SysNo");
            return result;
        }
    }
}
