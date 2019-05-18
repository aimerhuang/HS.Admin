using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess;
using Hyt.DataAccess.CRM;
using Hyt.Model;

namespace Hyt.DataAccess.Oracle.CRM
{
    /// <summary>
    /// 大宗采购数据层实现类
    /// </summary>
    /// <remarks>2013－06-25 杨晗 创建</remarks>
    public class CrBulkPurchaseDaoImpl : ICrBulkPurchaseDao
    {
        /// <summary>
        /// 根据系统编号获取大宗采购模型
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>大宗采购实体</returns>
        /// <remarks>2013－06-25 杨晗 创建</remarks>
        public override CrBulkPurchase GetModel(int sysNo)
        {
            return
                Context.Sql(@"select * from crbulkpurchase where SysNO = @0", sysNo).QuerySingle<CrBulkPurchase>();
        }

        /// <summary>
        /// 大宗采购分页查询
        /// </summary>
        /// <param name="pager">大宗采购查询条件</param>
        /// <returns>大宗采购信息列表</returns>
        /// <remarks>2013－06-25 杨晗 创建</remarks>
        public override Pager<CBCrBulkPurchase> Seach(Pager<CBCrBulkPurchase> pager)
        {
            #region sql条件
            string sql = @"(@contactname is null or cp.contactname like @contactname1) and (@companyname is null or cp.companyname like @companyname1)
                and ((@Status is null or @Status = 0) or cp.Status =@Status) and (@commitdate='1753-01-01' or (cp.commitdate>=@commitdateStart and cp.commitdate<@commitdateEnd))";
            #endregion

            using (var context = Context.UseSharedConnection(true))
            {
                if (pager.PageFilter.CommitDate == DateTime.MinValue)
                {
                    pager.PageFilter.CommitDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                }
                pager.Rows = Context.Select<CBCrBulkPurchase>("cp.*,cr.username as handler")
                                          .From("crbulkpurchase cp left join syuser cr on cp.handlersysno=cr.sysno")
                                          .Where(sql)
                                          .Parameter("contactname", pager.PageFilter.ContactName)
                                          .Parameter("contactname1", "%" + pager.PageFilter.ContactName + "%")
                                          .Parameter("companyname", pager.PageFilter.CompanyName)
                                          .Parameter("companyname1", "%" + pager.PageFilter.CompanyName + "%")
                                          .Parameter("Status", pager.PageFilter.Status)
                                          .Parameter("commitdate", pager.PageFilter.CommitDate)
                                          .Parameter("commitdateStart", pager.PageFilter.CommitDate)
                                          .Parameter("commitdateEnd", pager.PageFilter.CommitDate.AddDays(1))
                                          .Paging(pager.CurrentPage, pager.PageSize)
                                          .OrderBy("handledate desc")
                                          .QueryMany();
                pager.TotalRows = Context.Select<int>("count(1)")
                                        .From("crbulkpurchase cp")
                                        .Where(sql)
                                        .Parameter("contactname", pager.PageFilter.ContactName)
                                          .Parameter("contactname1", "%" + pager.PageFilter.ContactName + "%")
                                          .Parameter("companyname", pager.PageFilter.CompanyName)
                                          .Parameter("companyname1", "%" + pager.PageFilter.CompanyName + "%")
                                          .Parameter("Status", pager.PageFilter.Status)
                                          .Parameter("commitdate", pager.PageFilter.CommitDate)
                                          .Parameter("commitdateStart", pager.PageFilter.CommitDate)
                                          .Parameter("commitdateEnd", pager.PageFilter.CommitDate.AddDays(1))
                                     .QuerySingle();

            }
            return pager;
        }

        /// <summary>
        /// 插入大宗采购信息
        /// </summary>
        /// <param name="model">插入的数据模型</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－06-25 杨晗 创建</remarks>
        public override int Insert(CrBulkPurchase model)
        {
            return Context.Insert<CrBulkPurchase>("crbulkpurchase", model)
                            .AutoMap(x => x.SysNo)
                            .ExecuteReturnLastId<int>("Sysno");
        }

        /// <summary>
        /// 更新大宗采购信息
        /// </summary>
        /// <param name="model">更新的数据模型</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－06-25 杨晗 创建</remarks>
        public override int Update(CrBulkPurchase model)
        {
            return Context.Update<CrBulkPurchase>("crbulkpurchase", model)
                                      .AutoMap(x => x.SysNo)
                                      .Where(x => x.SysNo)
                                      .Execute();
        }

        /// <summary>
        /// 删除大宗采购信息
        /// </summary>
        /// <param name="sysNo">大宗采购系统编号</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks>2013－06-25 杨晗 创建</remarks>
        public override bool Delete(int sysNo)
        {
            int rowsAffected = Context.Delete("crbulkpurchase")
                                      .Where("Sysno", sysNo)
                                      .Execute();
            return rowsAffected > 0;
        }
    }
}
