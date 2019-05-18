using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.CRM;
using Hyt.Model;
using Hyt.Model.Parameter;

namespace Hyt.DataAccess.Oracle.CRM
{
    /// <summary>
    /// 商品浏览记录业务类
    /// </summary>
    /// <remarks>
    /// 2013-09-10 郑荣华 创建
    /// </remarks>
    public class CrBrowseHistoryDaoImpl : ICrBrowseHistoryDao
    {
        /// <summary>
        /// 新加商品浏览记录
        /// </summary>
        /// <param name="model">商品浏览记录实体</param>
        /// <returns>新加的系统编号</returns>
        /// <remarks>
        /// 2013-09-10 郑荣华 创建
        /// </remarks>
        public override int Create(CrBrowseHistory model)
        {
            return Context.Insert("CrBrowseHistory", model)
                          .AutoMap(x => x.SysNo)
                          .ExecuteReturnLastId<int>("SysNo");
        }

        /// <summary>
        /// 更新商品浏览记录
        /// </summary>
        /// <param name="model">商品浏览记录实体</param>
        /// <returns>受影响的行数</returns>
        /// <remarks>
        /// 2013-09-10 郑荣华 创建
        /// </remarks>
        public override int Update(CrBrowseHistory model)
        {
            return Context.Update("CrBrowseHistory", model)
                          .AutoMap(x => x.SysNo)
                          .Where(x => x.SysNo)
                          .Execute();
        }

        /// <summary>
        /// 获取商品浏览记录
        /// </summary>
        /// <param name="filter">商品浏览历史查询条件实体</param>
        /// <returns>商品浏览记录</returns>
        /// <remarks>
        /// 2013-09-10 郑荣华 创建
        /// </remarks>
        public override CrBrowseHistory GetCrBrowseHistory(ParaCrBrowseHistoryFilter filter)
        {
            const string sqlWhere = @"
                (@BrowseType is null or t.BrowseType= @BrowseType) 
                and (@CategorySysNo is null or t.CategorySysNo= @CategorySysNo)
                and (@CustomerSysNo is null or t.CustomerSysNo= @CustomerSysNo)
                and (@IsPageDown is null or t.IsPageDown=@IsPageDown)
                and (@KeyWord is null or t.KeyWord=@KeyWord)
                and (@ProductSysNo is null or t.ProductSysNo=@ProductSysNo)
               ";
            const string sql = "select t.* from CrBrowseHistory t where " + sqlWhere;
            return Context.Sql(sql)
                          .Parameter("BrowseType", filter.BrowseType)
                          .Parameter("CategorySysNo", filter.CategorySysNo)
                          .Parameter("CustomerSysNo", filter.CustomerSysNo)
                          .Parameter("IsPageDown", filter.IsPageDown)
                          .Parameter("KeyWord", filter.KeyWord)
                          .Parameter("ProductSysNo", filter.ProductSysNo)
                          .QuerySingle<CrBrowseHistory>();

        }
    }
}
