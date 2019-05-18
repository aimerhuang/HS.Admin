using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.CRM;
using Hyt.Model;
using Hyt.Model.Transfer;

namespace Hyt.DataAccess.Oracle.CRM
{
    /// <summary>
    /// 顾客关注商品数据访问类
    /// </summary>
    /// <remarks>
    /// 2013-08-26 郑荣华 创建
    /// </remarks>
    public class CrFavoritesDaoImpl : ICrFavoritesDao
    {
        /// <summary>
        /// 添加商品关注
        /// </summary>
        /// <param name="model">商品关注实体</param>        
        /// <returns>已添加则返回-1，未添加则返回新增的系统编号</returns>
        /// <remarks>
        /// 2013-08-26 郑荣华 创建
        /// </remarks>
        public override int Create(CrFavorites model)
        {
            var crf = Context.Sql("select * from CrFavorites where CustomerSysNo= @0 and ProductSysNo= @1", model.CustomerSysNo, model.ProductSysNo)
                             .QuerySingle<CrFavorites>();
            if (crf != null) return -1;
            //如果需要 加上商品静态数据统计 
            return Context.Insert("CrFavorites", model)
                          .AutoMap(x => x.SysNo)
                          .ExecuteReturnLastId<int>("SysNo");
        }

        /// <summary>
        /// 删除商品关注
        /// </summary>
        /// <param name="crSysNo">顾客系统编号</param>
        /// <param name="pdSysNo">商品系统编号</param>
        /// <returns>受影响行数</returns>
        /// <remarks>
        /// 2013-10-31 郑荣华 创建
        /// </remarks>
        public override int Delete(int crSysNo, int pdSysNo)
        {
            const string sql = @"delete from CrFavorites where CustomerSysNo=@0 and ProductSysNo=@1";

            return Context.Sql(sql, crSysNo, pdSysNo)
                          .Execute();
            //return Context.Delete("CrFavorites")
            //              .Where("CustomerSysNo", crSysNo)
            //              .Where("ProductSysNo", pdSysNo)
            //              .Execute();
        }

        /// <summary>
        /// 删除商品关注
        /// </summary>
        /// <param name="sysNo">商品关注系统编号</param>
        /// <returns>受影响行数</returns>
        /// <remarks>
        /// 2013-08-26 郑荣华 创建
        /// </remarks>
        public override int Delete(int sysNo)
        {
            return Context.Delete("CrFavorites")
                          .Where("SysNo", sysNo)
                          .Execute();
        }

        /// <summary>
        /// 获取顾客关注的商品
        /// </summary>
        /// <param name="crSysNo">顾客系统编号</param>
        /// <param name="pdSysNo">商品编号</param>
        /// <param name="pager">分页对象</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-08-26 郑荣华 创建
        /// </remarks>
        public override void GetCrFavoritesList(int? crSysNo, int? pdSysNo, ref Pager<CBCrFavorites> pager)
        {
            using (var context = Context.UseSharedConnection(true))
            {
                const string sqlWhere = @"
                (:CustomerSysNo is null or t.CustomerSysNo= @CustomerSysNo)
                and (:ProductSysNo is null or t.ProductSysNo= @ProductSysNo)                                      
               ";
                #region sqlcount

                const string sqlCount = @" select count(1) from CrFavorites t inner join PdProduct a on t.productsysno=a.sysno where " + sqlWhere;

                pager.TotalRows = context.Sql(sqlCount)
                                       .Parameter("CustomerSysNo", crSysNo)
                                       .Parameter("ProductSysNo", pdSysNo)
                                       .QuerySingle<int>();
                #endregion

                pager.Rows = context.Select<CBCrFavorites>("t.*,a.productname,a.productimage")
                                    .From("CrFavorites t inner join PdProduct a on t.productsysno=a.sysno")
                                    .Where(sqlWhere)
                                    .Parameter("CustomerSysNo", crSysNo)
                                    .Parameter("ProductSysNo", pdSysNo)
                                    .OrderBy("t.CustomerSysNo,t.ProductSysNo")
                                    .Paging(pager.CurrentPage, pager.PageSize)
                                    .QueryMany();
            }
        }

        /// <summary>
        /// 获取关注商品
        /// </summary>
        /// <param name="customerSysNo">会员系统编号</param>
        /// <param name="productSysNo">商品系统编号</param>
        /// <returns>关注实体</returns>
        /// <remarks> 2013-09-12 邵斌 创建 </remarks>
        public override CrFavorites GetFavorites(int customerSysNo, int productSysNo)
        {
            return Context.Select<CrFavorites>("*")
                       .From("CrFavorites")
                       .Where("customersysno=@customerSysNo and productsysno=@productSysNo")
                       .Parameter("customerSysNo", customerSysNo)
                       .Parameter("productSysNo", productSysNo)
                       .QuerySingle();
        }

        /// <summary>
        /// 获取收藏
        /// </summary>
        /// <param name="sysNo">收藏的系统编号</param>
        /// <returns>收藏实体</returns>
        /// <remarks> 2013-09-12 杨晗 创建 </remarks>
        public override CrFavorites GetFavorites(int sysNo)
        {
            return Context.Select<CrFavorites>("*")
                       .From("CrFavorites")
                       .Where("sysNo=:0")
                       .Parameter("0", sysNo)
                       .QuerySingle();
        }

        /// <summary>
        /// 是否已关注
        /// </summary>
        /// <param name="customerSysNo">顾客系统编号</param>
        /// <param name="productSysNo">商品编号</param>
        /// <returns>true or false</returns>
        /// <remarks> 2013-10-28 杨浩 创建 </remarks>     
        public override bool IsAttention(int customerSysNo, int productSysNo)
        {
            string sql = "select count(1) from CrFavorites where customersysno=@customerSysNo and productsysno=@productSysNo";

            return Context.Sql(sql)
                          .Parameter("customerSysNo", customerSysNo)
                          .Parameter("productSysNo", productSysNo)
                          .QuerySingle<int>() > 0;
        }

        /// <summary>
        /// 获取已关注数量
        /// </summary>
        /// <param name="customerSysNo">顾客系统编号</param>
        /// <returns>关注数量</returns>
        /// <remarks> 2013-10-28 杨浩 创建 </remarks>        
        public override int GetAttentionCount(int customerSysNo)
        {
            string sql = "select count(1) from CrFavorites where customersysno=@customerSysNo";

            return Context.Sql(sql)
                          .Parameter("customerSysNo", customerSysNo)
                          .QuerySingle<int>();
        }
    }
}
