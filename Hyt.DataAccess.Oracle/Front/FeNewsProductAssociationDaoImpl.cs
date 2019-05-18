using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hyt.DataAccess.Base;
using Hyt.DataAccess.Front;
using Hyt.Model;


namespace Hyt.DataAccess.Oracle.Front
{
    /// <summary>
    /// 新闻商品关联表数据访问 抽象类
    /// </summary>
    /// <remarks>2014-01-14 苟治国 创建</remarks>
    public class FeNewsProductAssociationDaoImpl : IFeNewsProductAssociationDao
    {
        /// <summary>
        /// 查看新闻商品关联表
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>新闻商品表</returns>
        /// <remarks>2014－09-12 陈俊创建</remarks>
        public override Model.FeNewsProductAssociation GetEntity(int sysNo)
        {
            return Context.Sql(@"select * from FeNewsProductAssociation fnp where fnp.SysNO = @0", sysNo).QuerySingle<FeNewsProductAssociation>();
        }

        /// <summary>
        /// 查看新闻商品关联表
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>新闻商品关联表</returns>
        /// <remarks>2013－01-14 苟治国 创建</remarks>
        public override Model.CBFeNewsProductAssociation GetModel(int sysNo)
        {
            return Context.Sql(@"select fnp.*,pd.productname,pd.easname from FeNewsProductAssociation fnp left join pdproduct pd on fnp.productsysno=pd.sysno where fnp.SysNO = @0", sysNo).QuerySingle<CBFeNewsProductAssociation>();
        }

        /// <summary>
        /// 根据条件获取新闻商品关联表
        /// </summary>
        /// <param name="newsSysNo">新闻编号</param>
        /// <returns>新闻商品关联表列表</returns>
        /// <remarks>2013－01-14 苟治国 创建</remarks>
        public override IList<Model.CBFeNewsProductAssociation> Seach(int newsSysNo)
        {
            #region sql条件
            string sql = @"(@NewsSysNo is null or fnp.NewsSysNo=@NewsSysNo)";
            #endregion

            var list = Context.Select<CBFeNewsProductAssociation>("fnp.*,pd.sysno as productsysno,pd.productname,pd.easname")
                                      .From("FeNewsProductAssociation fnp left join pdproduct pd on fnp.productsysno=pd.sysno")
                                      .Where(sql)
                                      .Parameter("NewsSysNo", newsSysNo)
                                      .OrderBy("fnp.DisplayOrder desc").QueryMany();
            return list;
        }

        /// <summary>
        /// 查看在当前类型中是否有相同产品称
        /// </summary>
        /// <param name="newsSysNo">新闻编号</param>
        /// <param name="productSysNo">商品编号</param>
        /// <returns>总数</returns>
        public override int GetCount(int newsSysNo, int productSysNo)
        {
            #region sql条件
            string sqlWhere = @"(@NewsSysNo is null or fn.NewsSysNo =@NewsSysNo) and (@ProductSysNo=0 or fn.ProductSysNo =@ProductSysNo)";
            #endregion

            var single = Context.Select<int>("count(1)")
                              .From("FeNewsProductAssociation fn")
                              .Where(sqlWhere)
                              .Parameter("NewsSysNo", newsSysNo)
                              .Parameter("ProductSysNo", productSysNo)
                              .QuerySingle();
            return single;

        }

        /// <summary>
        /// 插入新闻商品关联表
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－01-14 苟治国 创建</remarks>
        public override int Insert(Model.FeNewsProductAssociation model)
        {
            var result = Context.Insert<FeNewsProductAssociation>("FeNewsProductAssociation", model)
                    .AutoMap(x => x.SysNo)
                    .ExecuteReturnLastId<int>("SysNo");
            return result;
        }

        /// <summary>
        /// 更新新闻商品关联表
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－01-14 苟治国 创建</remarks>
        public override int Update(Model.FeNewsProductAssociation model)
        {
            int rowsAffected = Context.Update<Model.FeNewsProductAssociation>("FeNewsProductAssociation", model)
              .AutoMap(x => x.SysNo)
              .Where(x => x.SysNo)
              .Execute();
            return rowsAffected;
        }

        /// <summary>
        /// 删除新闻商品关联表
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks>2013－01-14 苟治国 创建</remarks>
        public override bool Delete(int sysNo)
        {
            int rowsAffected = Context.Delete("FeNewsProductAssociation")
                          .Where("sysNo", sysNo)
                          .Execute();
            return rowsAffected > 0;
        }
    }
}
