using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hyt.Model;
using Hyt.DataAccess.Base;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;

namespace Hyt.DataAccess.Web
{
    /// <summary>
    /// 会员中中
    /// </summary>
    /// <remarks>2013-08-08 苟治国 创建</remarks>
    public abstract class ICrCustomerQuestionDao : DaoBase<ICrCustomerQuestionDao>
    {
        /// <summary>
        /// 获取指定会员咨询信息
        /// </summary>
        /// <param name="pager">咨询查询条件</param>
        /// <returns>咨询订单列表</returns>
        /// <remarks>2013-08-08 苟治国 创建</remarks>
        public abstract Pager<CBCrCustomerQuestion> GetQuestion(Pager<CBCrCustomerQuestion> pager);

        /// <summary>
        /// 获取指定会员咨询信息
        /// </summary>
        /// <param name="pager">咨询查询条件</param>
        /// <returns>咨询订单列表</returns>
        /// <remarks>2013-08-08 苟治国 创建</remarks>
        public abstract Pager<CBCrProductDetailCustomerQuestion> GetQuestion(Pager<CBCrProductDetailCustomerQuestion> pager);

        /// <summary>
        /// 获取商品的咨询次数信息
        /// </summary>
        /// <param name="productSysNo"></param>
        /// <returns>返回各种统计次</returns>
        /// <remarks>2013-08-09 邵斌 创建</remarks>
        public abstract IDictionary<string, int> GetCustomerQuestionsCountInfo(int productSysNo);

        /// <summary>
        /// 获取商品的咨询次数
        /// </summary>
        /// <param name="productSysNo"></param>
        /// <returns>咨询次数</returns>
        /// <remarks>2013-12-23 杨浩 创建</remarks>
        public abstract int GetCustomerQuestionsCount(int productSysNo);

        /// <summary>
        /// 新增商品咨询
        /// </summary>
        /// <param name="model">商品咨询明细</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-11-19 苟治国 创建</remarks>
        public abstract int Insert(Model.CrCustomerQuestion model);
    }
}
