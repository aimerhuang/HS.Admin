using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.QJT;
using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Model.Parameter;

namespace Hyt.DataAccess.Oracle.QJT
{
    /// <summary>
    /// 千机团串码设置
    /// </summary>
    /// <remarks>2016-02-17 谭显锋 创建</remarks>  
    public class QJTProductImeiDaoImpl : IQJTProductImeiDao
    {
        /// <summary>
        /// 添加千机团串码设置
        /// </summary>
        /// <param name="model">实体信息</param>
        /// <returns>返回新建记录的sysno</returns>       
        /// <remarks>2016-02-17 谭显锋 创建</remarks>
        public override int Create(QJTProductImei model)
        {
            int sysno = 0;
            sysno = Context.Insert<QJTProductImei>("QJTProductImei", model)
                        .AutoMap(x => x.SysNo)
                        .ExecuteReturnLastId<int>("Sysno");
            return sysno;
        }

        /// <summary>
        /// 根据商品编号判断商品是否属于千机团需要添加串码的商品
        /// </summary>
        /// <param name="productSysno">商品编号</param>
        /// <returns>是串码商品返回true,否则返回false</returns>
        /// <remarks>2016-02-18 谭显锋 创建</remarks>
        public override bool IsImeiProduct(int productSysno)
        {
            return Context.Sql(@"select t.productsysno as pid
                                        from 
                                        PdCategoryAssociation  t 
                                        inner join QJTProductImei q 
                                        on t.categorysysno=q.productcategorysysno and q.isusecategory=1 and  t.ismaster=1
                                        where  t.productsysno=:productsysno
                                        union 
                                  select productsysno as pid 
                                         from 
                                         qjtproductimei where productsysno =@productsysno
                                        ")
                                .Parameter("productsysno", productSysno)
                                .Parameter("productsysno", productSysno)
                                .QuerySingle<int>() > 0;
        }


        /// <summary>
        /// 获取设置列表
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns></returns>
        /// <remarks>2016-02-18 朱成果 创建</remarks>
        public override Pager<CBQJTProductImei> GetList(ParProductImeiFilter filter)
        {
            Pager<CBQJTProductImei> pager = new Pager<CBQJTProductImei>();
            pager.CurrentPage = filter.Id;
            pager.PageSize = filter.PageSize;
            string from = @"QJTProductImei qt
                                        left outer join PdCategory  pd
                                        on qt.productcategorysysno=pd.sysno
                                        left outer join PdProduct pdt
                                        on pdt.sysno=qt.productsysno
                                        left outer join syuser su1
                                        on su1.sysno=qt.createdby
                                        left outer join syuser su2
                                        on su2.sysno=qt.lastupdateby";
            string where="1=1";
            System.Collections.ArrayList arrlst=new System.Collections.ArrayList();
            if(filter.IsUseCategory.HasValue)
            {
                where+=" and qt.IsUseCategory=@IsUseCategory";
                arrlst.Add(filter.IsUseCategory.Value);
            }
            pager.TotalRows= Context.Select<int>("count(0)")
                                    .From(from)
                                    .Where(where)
                                    .Parameters(arrlst)
                                    .QuerySingle();

            pager.Rows = Context.Select<CBQJTProductImei>(@"qt.*,
                                                            pd.categoryname as ProductCategoryName,
                                                            pdt.easname as ProductName,
                                                            su1.username as CreateUserName,
                                                            su2.username as UpdateUserName")
                               .From(from)
                               .Where(where)
                                .Parameters(arrlst)
                                .Paging(filter.Id, filter.PageSize)
                                .OrderBy("qt.sysno desc")
                                .QueryMany();
            return pager;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sysno">编号</param>
        /// <returns></returns>
        /// <remarks>2016-02-18 朱成果 创建</remarks>
        public override int Delete(int sysno)
        {
          return   Context.Sql("Delete from QJTProductImei where sysno=@sysno")
                   .Parameter("sysno",sysno)
                   .Execute();
        }
    }
}
