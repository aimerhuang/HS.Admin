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
    /// 评论支持业务实现类
    /// </summary>
    /// <remarks>2013-08-27 杨晗 创建</remarks>
    public class FeCommentSupportDaoImpl : IFeCommentSupportDao
    {
        /// <summary>
        /// 增加评论支持
        /// </summary>
        /// <param name="model">评论支持模型数据</param>
        /// <returns>成功系统号记录</returns>
        /// <remarks>2013-08-27 杨晗 创建</remarks>
        public override int Insert(FeCommentSupport model)
        {
            return Context.Insert<FeCommentSupport>("FeCommentSupport", model)
                          .AutoMap(x => x.SysNo)
                          .ExecuteReturnLastId<int>("Sysno");
        }

        /// <summary>
        /// 更新评论支持
        /// </summary>
        /// <param name="model">评论支持模型数据</param>
        /// <returns>成功系统号记录</returns>
        /// <remarks>2013-08-27 杨晗 创建</remarks>
        public override int Update(FeCommentSupport model)
        {
            return Context.Update<FeCommentSupport>("FeCommentSupport", model)
                          .AutoMap(x => x.SysNo)
                          .Where(x => x.SysNo)
                          .Execute();
        }

        /// <summary>
        /// 根据评论系统号和用户系统号获取评论支持
        /// </summary>
        /// <param name="productCommentSysNo">评论系统号</param>
        /// <param name="customerSysNo">用户系统号</param>
        /// <returns>文章实体</returns>
        /// <remarks>2013-08-27 杨晗 创建</remarks>
        public override IList<FeCommentSupport> GetFeCommentSupport(int productCommentSysNo, int customerSysNo)
        {
            #region sql条件

            string sql = @"productCommentSysNo=@productCommentSysNo and customerSysNo=@customerSysNo";

            #endregion

            var countBuilder = Context.Select<FeCommentSupport>("fs.*")
                                      .From("FeCommentSupport fs")
                                      .Where(sql)
                                      .Parameter("productCommentSysNo", productCommentSysNo)
                                      .Parameter("customerSysNo", customerSysNo)
                                      .OrderBy("LastUpdateDate desc").QueryMany();
            return countBuilder;
        }

        /// <summary>
        /// 根据评论系统号和用户系统号获取评论支持
        /// </summary>
        /// <param name="productCommentSysNo">评论系统号</param>
        /// <param name="customerSysNo">用户系统号</param>
        /// <returns>文章实体</returns>
        /// <remarks>2013-08-27 杨晗 创建</remarks>
        public override FeCommentSupport GetModel(int productCommentSysNo, int customerSysNo)
        {
            #region sql条件

            string sql = @"productCommentSysNo=@productCommentSysNo and customerSysNo=@customerSysNo";

            #endregion

            var countBuilder = Context.Select<FeCommentSupport>("fs.*")
                                      .From("FeCommentSupport fs")
                                      .Where(sql)
                                      .Parameter("productCommentSysNo", productCommentSysNo)
                                      .Parameter("customerSysNo", customerSysNo)
                                      .QuerySingle();
            return countBuilder;
        }
    }
}
