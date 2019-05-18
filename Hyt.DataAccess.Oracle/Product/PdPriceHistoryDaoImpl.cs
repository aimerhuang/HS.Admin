using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Product;
using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Util;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Oracle.Product
{
    /// <summary>
    /// 商品历史调价接口
    /// </summary>
    /// <remarks>2013-06-26 邵斌 创建</remarks>
    public class PdPriceHistoryDaoImpl : IPdPriceHistoryDao
    {
        /// <summary>
        /// 保存调价申请
        /// </summary>
        /// <param name="priceHistory">调价申请对象</param>
        /// <returns>是否保存成功</returns>
        /// <remarks>2013-06-27 邵斌 创建</remarks>
        public override bool SavePdPriceHistory(params PdPriceHistory[] priceHistory)
        {
            bool result = true;    //返回结果

            //创建公用数据库连接
            using (var currentContext = Context.UseSharedConnection(true))
            {
                //遍历要添加的调价审请，并一个一个的插入到数据库
                foreach (PdPriceHistory p in priceHistory)
                {
                    if (p.AuditDate == DateTime.MinValue)
                    {
                        p.AuditDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                    }
                    
                    //string sql = string.Format("delete from PdPriceHistory where PriceSysNo={0} and Status=10 ", p.PriceSysNo);
                    //int rowsAffected = currentContext.Sql(sql).Execute();

                    //如果添加失败将返回false
                    result = (result && (currentContext.Insert<PdPriceHistory>("PdPriceHistory", p)
                                                .AutoMap(c => c.SysNo)
                                                .ExecuteReturnLastId<int>("SysNo") > 0));
                    //如果更新失败将回归数据库，并返回。
                    if (!result)
                    {
                        return false;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 获取价格等级修改记录列表(分页方法)
        /// </summary>
        /// <param name="pageIndex">起始页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="status">审批状态</param>
        /// <param name="erpCode">商品编码</param>
        /// <param name="count">抛出总数</param>
        /// <param name="productName">商品名称</param>
        /// <returns>价格等级修改记录列表</returns>
        /// <remarks>2013-07-17 杨晗 创建</remarks>
        /// <remarks>2013-07-22 黄波 添加排序</remarks>
        /// <remarks>2016-03-14 陈海裕 重构</remarks>
        public override IList<CBPdPriceHistory> GetPriceHistorieList(int pageIndex, int pageSize, int status, int? erpCode, out int count, string productName = null)
        {
            var returnValue = new List<CBPdPriceHistory>();
            int pageStart = (pageIndex - 1) * pageSize + 1;
            int pageEnd = pageIndex * pageSize;

            #region sql

            StringBuilder sqlText = new StringBuilder();
            // 数据插入临时表#TEMP_PdPriceHistory
            sqlText.AppendLine("SELECT	PPH.* INTO #TEMP_PdPriceHistory");
            sqlText.AppendLine("FROM	PdPriceHistory PPH");
            sqlText.AppendLine("		LEFT OUTER JOIN PdPrice PRI ON PPH.pricesysno=PRI.sysno");
            sqlText.AppendLine("		LEFT OUTER JOIN PdProduct PRO ON PRI.productsysno=PRO.sysno");
            sqlText.AppendLine("WHERE	(@productName IS NULL OR PRO.EasName LIKE @productName1 OR PRO.ErpCode LIKE @productName1) AND (@status = 0 OR PPH.status=@status)");
            sqlText.AppendLine("ORDER BY PPH.SysNo DESC,PRO.SysNo DESC,relationcode");
            // 删除重复记录
            sqlText.AppendLine("DELETE FROM #TEMP_PdPriceHistory");
            sqlText.AppendLine("WHERE	RelationCode IN (SELECT RelationCode FROM #TEMP_PdPriceHistory GROUP BY RelationCode HAVING COUNT(RelationCode)>1)");
            sqlText.AppendLine("		AND #TEMP_PdPriceHistory.SysNo NOT IN (SELECT MIN(SysNo) FROM #TEMP_PdPriceHistory GROUP BY RelationCode HAVING COUNT(RelationCode)>1)");
            // 数据插入临时表#TEMP_RelationCode
            sqlText.AppendLine("SELECT	T_PPH.SysNo,T_PPH.RelationCode INTO #TEMP_RelationCode FROM #TEMP_pdpricehistory T_PPH");
            // 分页排序
            sqlText.AppendLine("SELECT	d.erpcode,d.easname as productname,a.*,c.pricesource,c.sourcesysno,");
            sqlText.AppendLine("		(SELECT username FROM SyUser WHERE sysno=a.applysysno) applyname,");
            sqlText.AppendLine("		(SELECT username FROM SyUser WHERE sysno=a.auditor) auditorname");
            sqlText.AppendLine("FROM	pdpricehistory a");
            sqlText.AppendLine("		LEFT OUTER JOIN pdprice c ON a.pricesysno=c.sysno");
            sqlText.AppendLine("		LEFT OUTER JOIN pdproduct d ON c.productsysno=d.sysno");
            sqlText.AppendLine("WHERE	a.relationcode IN");
            sqlText.AppendLine("		(");
            sqlText.AppendLine("		SELECT TOP(" + pageSize + ") RelationCode");
            sqlText.AppendLine("		FROM	#TEMP_RelationCode");
            sqlText.AppendLine("		WHERE	SysNo NOT IN (SELECT TOP (" + pageSize * (pageIndex - 1) + ") SysNo FROM #TEMP_RelationCode)");
            sqlText.AppendLine("		)");
            sqlText.AppendLine("ORDER BY a.auditDate DESC,a.applydate DESC,a.relationcode,c.pricesource,c.sourcesysno");
            // 删除临时表
            sqlText.AppendLine("DROP TABLE #TEMP_PdPriceHistory");
            sqlText.AppendLine("DROP TABLE #TEMP_RelationCode");

            string sqlCount =
                @"select count(relationcode) from (select pd.relationcode from pdpricehistory pd  LEFT OUTER JOIN  pdprice pc on  pd.pricesysno=pc.sysno
                LEFT OUTER JOIN  pdproduct p  on pc.productsysno=p.sysno where  
                (@productName is null or p.easname like @productName1 or p.erpcode like @productName1)
                and (@status = 0 or pd.status = @status)
                group by pd.relationcode) as dd";
            #endregion

            using (var context = Context.UseSharedConnection(true))
            {
                count = context.Sql(sqlCount)
                    .Parameter("productName", productName)
                               .Parameter("productName1", "%" + productName + "%")
                               .Parameter("status", status)
                               .QuerySingle<int>();
                returnValue = context.Sql(sqlText.ToString())
                    .Parameter("productName", productName)
                           .Parameter("productName1", "%" + productName + "%")
                           .Parameter("status", status)
                           .Parameter("pageStart", pageStart)
                           .Parameter("pageEnd", pageEnd)
                           .QueryMany<CBPdPriceHistory>();
            }
            return returnValue;
        }

        /// <summary>
        /// 根据关系码获取价格等级修改记录列表
        /// </summary>
        /// <param name="relationCode">等级价格记录关系码</param>
        /// <returns>价格等级修改记录列表</returns>
        /// <remarks>2013-07-18 杨晗 创建</remarks>
        public override IList<CBPdPriceHistory> GetPriceHistorieListByRelationCode(string relationCode)
        {
            #region sql

            const string sql = @"select d.sysno as ProductSysNo,d.erpcode,d.productname,a.*,c.pricesource,c.sourcesysno,
                        (select username from SyUser where sysno=a.applysysno) applyname,
                        (select username from SyUser where sysno=a.auditor) auditorname
                        from pdpricehistory a 
                        left join pdprice c  on a.pricesysno=c.sysno 
                        left join  pdproduct d on c.productsysno=d.sysno
                         where  
                         a.relationcode= @relationCode order by c.pricesource,c.sourcesysno
                        ";

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
        /// <remarks>2013－06-17 杨晗 创建</remarks>
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
