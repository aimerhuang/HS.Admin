using System;
using System.Collections.Generic;
using Hyt.DataAccess.Distribution;
using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Oracle.Distribution
{
    /// <summary>
    /// 分销商产品等级价格
    /// </summary>
    /// <remarks>
    /// 2013-09-13 周瑜 创建
    /// </remarks>
    public class DsLevelPriceImpl : IDsLevelPriceDao
    {
        /// <summary>
        /// 获取分销商等级
        /// </summary>
        /// <param></param>
        /// <returns>分销商等级</returns>
        /// <remarks>2013-09-13 周瑜 创建</remarks>
        public override IList<DsDealerLevel> GetDsDealerLevel()
        {
            var list = Context.Sql("select SysNo, levelname from dsdealerlevel ").QueryMany<DsDealerLevel>();
            return list;
        }

        /// <summary>
        /// 获取价格等级修改记录列表(分页方法)
        /// </summary>
        /// <param name="pageIndex">起始页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="status">审批状态</param>
        /// <param name="sysNo">商品编号</param>
        /// <param name="erpCode">商品编号</param>
        /// <param name="count">抛出总数</param>
        /// <returns>价格等级修改记录列表</returns>
        /// <remarks>2013-09-16 周瑜 创建</remarks>
        public override IList<CBPdPriceHistory> GetPriceHistorieList(int pageIndex, int pageSize, int? status, int? sysNo, string erpCode, out int count)
        {

            #region sql

            const string sql = @"(SELECT A.APPLYDATE, A.RELATIONCODE, C.SYSNO as ProductSysNo, A.STATUS
                                  FROM PdPriceHistory A
                                 INNER JOIN PdPrice B
                                    ON A.Pricesysno = B.Sysno
                                 INNER JOIN PdProduct C
                                    ON B.Productsysno = C.Sysno                                   
                                 WHERE B.PRICESOURCE = @0
                                  AND (@1 IS NULL OR C.Sysno=@1)
                                  AND (@2 IS NULL OR charindex(C.ERPCODE, @2) > 0)
                                  AND (@3 IS NULL OR A.STATUS=@3)
                                 GROUP BY A.APPLYDATE, A.RELATIONCODE, C.SYSNO, A.STATUS)tb";

            #endregion

            var paras = new object[]
                {
                  ProductStatus.产品价格来源.分销商等级价.GetHashCode(),
                  sysNo,
                  erpCode,
                  status
                };

            var returnValue = Context.Select<CBPdPriceHistory>(@"tb.*").From(sql).Parameters(paras).OrderBy(@"tb.APPLYDATE desc").Paging(pageIndex, pageSize).QueryMany();
            count = Context.Select<int>(@"count(1)").From(sql).Parameters(paras).QuerySingle();
            return returnValue;
        }

        /// <summary>
        /// 根据关系码获取价格等级修改记录列表
        /// </summary>
        /// <param name="relationCode">等级价格记录关系码</param>
        /// <returns>价格等级修改记录列表</returns>
        /// <remarks>2013-09-16 周瑜 创建</remarks>
        public override IList<CBPdPriceHistory> GetPriceHistorieListByRelationCode(string relationCode)
        {
            #region sql

            const string sql = @"select d.erpcode,d.productname,a.*,c.pricesource,c.sourcesysno,
(select username from SyUser where sysno=a.applysysno) applyname,
(select username from SyUser where sysno=a.auditor) auditorname
from pdpricehistory a left outer join pdprice c on a.pricesysno=c.sysno left outer join pdproduct d on c.productsysno=d.sysno
 where a.relationcode= @relationCode order by c.pricesource,c.sourcesysno";

            #endregion

            return
                Context.Sql(sql)
                       .Parameter("relationCode", relationCode)
                       .QueryMany<CBPdPriceHistory>();
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="relationCode">关系码</param>
        /// <param name="opinion">意见</param>
        /// <param name="status">状态</param>
        /// <param name="auditor">审批人</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-09-16 周瑜 创建</remarks>
        public override int Update(string relationCode, string opinion, int status, int auditor)
        {

            int rowsAffected =
                Context.Sql(
                    "update PdPriceHistory set opinion=@opinion , status=@status , auditor=@auditor , auditDate=@auditDate where relationCode=@relationCode")
                       .Parameter("opinion", opinion)
                       .Parameter("status", status)
                       .Parameter("auditor", auditor)
                       .Parameter("auditDate", DateTime.Now)
                       .Parameter("relationCode", relationCode)
                       .Execute();
            return rowsAffected;
        }
    }
}
