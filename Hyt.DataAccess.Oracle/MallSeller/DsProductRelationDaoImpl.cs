using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.MallSeller;
using Hyt.Model;
using Hyt.Model.Transfer;
using System.Collections;

namespace Hyt.DataAccess.Oracle.MallSeller
{
    public class DsProductRelationDaoImpl : IDsProductRelationDao
    {
        /// <summary>
        /// 查询升舱商品关系维护关联
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <remarks>查询升舱商品关系维护关联分页数据</remarks>
        /// <remarks>2014-10-10 谭显锋 创建</remarks>
        public override Model.Pager<Model.Transfer.CBDsProductRelation> Query(Model.Parameter.ParaDsProductRelationFilter filter)
        {
            //:product is null or charindex(lower(b.EasName),lower(:product)) > 0 or charindex(lower(b.ErpCode),lower(:product)) > 0
            string sql =
               @"(select da.sysno,ds.shopname,da.mallproductid,da.mallproductattr,p.erpcode,p.easname from DsProductAssociation da
                   inner join pdproduct p on da.hytproductsysno=p.sysno
                   inner join dsdealermall ds on ds.sysno=da.dealermallsysno {0}) tb";
            var paras = new ArrayList();
            var where = "where 1=1 ";
            if (!string.IsNullOrWhiteSpace(filter.EasName))
            {
                where += " and (p.easname is null or charindex(lower(p.easname),lower(@p0p0))>0) or (p.erpcode is null or charindex(p.erpcode,@p0p0)>0)";
                paras.Add(filter.EasName);
            }
            sql = string.Format(sql, where);
            var dataList = Context.Select<CBDsProductRelation>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(0)").From(sql);
            dataList.Parameters(paras);
            dataCount.Parameters(paras);

            var pager = new Pager<CBDsProductRelation>
            {
                PageSize = filter.PageSize,
                CurrentPage = filter.Id,
                TotalRows = dataCount.QuerySingle(),
                Rows = dataList.OrderBy("tb.sysNo desc").Paging(filter.Id, filter.PageSize).QueryMany()
            };

            return pager;
        }

        /// <summary>
        /// 根据sysNo删除DsProductAssociation表中的数据
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>受影响行数</returns>
        /// <remarks>2014-10-10 谭显锋 创建</remarks>
        public override int Delete(int sysNo)
        {
            return Context.Delete("DsProductAssociation")
                    .Where("SysNo", sysNo)
                    .Execute();
        }
    }
}
