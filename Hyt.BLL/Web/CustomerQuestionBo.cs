using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Infrastructure.Caching;
using Hyt.Model;
using Hyt.DataAccess.Web;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Infrastructure.Pager;

namespace Hyt.BLL.Web
{
    /// <summary>
    /// web咨询业务逻辑层
    /// </summary>
    /// <remarks>2013－08-08 苟治国 创建</remarks>
    public class CustomerQuestionBo : BOBase<CustomerQuestionBo>
    {
        /// <summary>
        /// 获取指定会员咨询信息
        /// </summary>
        /// <param name="pageIndex">分页索引</param>
        /// <param name="customersysno">会员编号</param>
        /// <returns></returns>
        /// <remarks>2013-08-08 苟治国 创建</remarks>
        public PagedList<CBCrCustomerQuestion> GetQuestion(int pageIndex, int customersysno)
        {
            var list = new PagedList<CBCrCustomerQuestion>();
            var pager = new Pager<CBCrCustomerQuestion>();

            pager.CurrentPage = pageIndex;
            pager.PageFilter = new CBCrCustomerQuestion
            {
                CustomerSysNo = (int)customersysno
            };
            pager.PageSize = list.PageSize;
            pager = ICrCustomerQuestionDao.Instance.GetQuestion(pager);
            list = new PagedList<CBCrCustomerQuestion>
            {
                Data = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows,
                IsLoading = false,
                Style = PagedList.StyleEnum.WebSmall
            
            };
            return list;
        }

        /// <summary>
        /// 读取商品问题列表
        /// </summary>
        /// <param name="pager">分页对象</param>
        /// <returns>商品问题列表</returns>
        /// <remarks>2013-08-13 邵斌 创建</remarks>
        public IList<CBCrProductDetailCustomerQuestion> GetQuestion(Pager<CBCrProductDetailCustomerQuestion> pager)
        {

            string key = string.Format("{0}_{1}", pager.PageFilter.ProductSysNo, pager.PageFilter.QuestionType+1);
            return CacheManager.Get(CacheKeys.Items.ProductQuestions_, key, delegate{
                   return ICrCustomerQuestionDao.Instance.GetQuestion(pager).Rows;
            });
        }

        /// <summary>
        /// 获取商品的咨询次数信息
        /// </summary>
        /// <param name="productSysNo"></param>
        /// <returns>返回各种统计次</returns>
        /// <remarks>2013-08-09 邵斌 创建</remarks>
        public IDictionary<string, int> GetCustomerQuestionsCountInfo(int productSysNo)
        {
            return CacheManager.Get(CacheKeys.Items.ProductQuestionsTotalInfo_, productSysNo.ToString(), delegate
                {
                    return ICrCustomerQuestionDao.Instance.GetCustomerQuestionsCountInfo(productSysNo);
                });
        }

        /// <summary>
        /// 获取商品的咨询次数
        /// </summary>
        /// <param name="productSysNo"></param>
        /// <returns>咨询次数</returns>
        /// <remarks>2013-12-23 杨浩 创建</remarks>
        public int GetCustomerQuestionsCount(int productSysNo)
        {
            //return ICrCustomerQuestionDao.Instance.GetCustomerQuestionsCount(productSysNo);
            return CacheManager.Get(CacheKeys.Items.ProductQuestionsCount_, productSysNo.ToString(),
                                    () => ICrCustomerQuestionDao.Instance.GetCustomerQuestionsCount(productSysNo));
        }

        /// <summary>
        /// 新增商品咨询
        /// </summary>
        /// <param name="model">商品咨询明细</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-11-19 苟治国 创建</remarks>
        public int Insert(Model.CrCustomerQuestion model)
        {
            return ICrCustomerQuestionDao.Instance.Insert(model);
        }
    }
}
