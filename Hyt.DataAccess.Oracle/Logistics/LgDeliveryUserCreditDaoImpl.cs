using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
namespace Hyt.DataAccess.Oracle.Logistics
{
    /// <summary>
    /// 配送员信用额度维护 实现类
    /// </summary>
    /// <remarks>2013-06-09 沈强 创建</remarks>
    public class LgCreditDaoImpl : Hyt.DataAccess.Logistics.ILgDeliveryUserCreditDao
    {
        #region 操作

        /// <summary>
        /// 添加配送员(业务员)信用配额
        /// </summary>
        /// <param name="model">配送员(业务员)信用配额</param>
        /// <returns>添加是否成功</returns>
        /// <remarks>2013-06-19 郑荣华 创建</remarks>
        public override bool Create(LgDeliveryUserCredit model)
        {
            return Context.Insert("LgDeliveryUserCredit", model)
                          .AutoMap()
                          .Execute() > 0;
        }

        /// <summary>
        /// 更新配送员(业务员)信用配额
        /// </summary>
        /// <param name="model">配送员(业务员)信用配额</param>
        /// <returns>更新是否成功</returns>
        /// <remarks>
        /// 2013-06-09 沈强 创建
        /// 2013-07-17 郑荣华 重构
        /// </remarks>
        public override bool Update(LgDeliveryUserCredit model)
        {
            return Context.Update("LgDeliveryUserCredit", model)
                          .AutoMap(x => x.DeliveryUserSysNo, x => x.WarehouseSysNo,x=>x.CreatedDate,x=>x.CreatedBy) //需排除的字段
                          .Where(x => x.DeliveryUserSysNo)
                          .Where(x => x.WarehouseSysNo)
                          .Execute() > 0;

        }

        /// <summary>
        /// 删除配送员信用信息
        /// </summary>
        /// <param name="deliveryUserSysNo">配送员系统编号</param>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks>2013-06-21 郑荣华 创建</remarks>
        public override bool Delete(int deliveryUserSysNo, int warehouseSysNo)
        {
            const string sql = @"delete from LgDeliveryUserCredit where DeliveryUserSysNo=@0 and WarehouseSysNo=@1";

            return Context.Sql(sql, deliveryUserSysNo, warehouseSysNo)
                          .Execute() > 0;

        }

        #endregion

        #region 查询

        /// <summary>
        /// 获取配送员(业务员)信用配额集合
        /// </summary>
        /// <param name="pager">配送员(业务员)信用配额集合分页对象</param>
        /// <returns></returns>
        /// <remarks>2013-06-20 郑荣华 创建</remarks>
        public override void GetLgDeliveryUserCreditList(ref Pager<CBLgDeliveryUserCredit> pager)
        {
            using (var context = Context.UseSharedConnection(true))
            {
                pager.TotalRows = context.Sql("select count(1) from LgDeliveryUserCredit").QuerySingle<int>();

                #region 调试Sql

                /* 
                    select a.*,b.username,b.status,d.warehousename
                                 from lgdeliveryusercredit a left join syuser b on a.deliveryusersysno=b.sysno
                                 left join syuserwarehouse c on a.deliveryusersysno=c.usersysno
                                 left join whwarehouse d on c.warehousesysno=d.sysno
             */

                #endregion

                const string fromSql = @"lgdeliveryusercredit a left join syuser b on a.deliveryusersysno=b.sysno                                       
                                        left join whwarehouse d on a.warehousesysno=d.sysno";

                pager.Rows = context.Select<CBLgDeliveryUserCredit>("a.*,b.username,b.status,d.warehousename")
                                    .From(fromSql)
                                    .OrderBy("b.username")
                                    .Paging(pager.CurrentPage, pager.PageSize)
                                    .QueryMany();
            }
        }

        /// <summary>
        /// 查询配送员所有的信用额度信息
        /// </summary>
        /// <param name="deliveryUserSysNo">配送员系统编号</param>
        /// <returns>配送员信用额度组合实体列表</returns>
        /// <remarks>
        /// 2013-07-17 郑荣华 创建
        /// </remarks>
        public override IList<CBLgDeliveryUserCredit> GetLgDeliveryUserCredit(int deliveryUserSysNo)
        {
            const string sql = @"select a.*,b.username,b.status,d.warehousename
                                 from lgdeliveryusercredit a left join syuser b on a.deliveryusersysno=b.sysno                                
                                 left join whwarehouse d on a.warehousesysno=d.sysno 
                                 where a.deliveryUserSysNo=@0";

            return Context.Sql(sql, deliveryUserSysNo)
                          .QueryMany<CBLgDeliveryUserCredit>();
        }

        /// <summary>
        /// 根据配送员和仓库系统编号获取信用配额信息
        /// </summary>
        /// <param name="deliveryUserSysNo">配送员系统编号</param>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <returns>配送员信用额度组合实体</returns>
        /// <remarks>2013-07-17 郑荣华 创建</remarks>
        public override CBLgDeliveryUserCredit GetLgDeliveryUserCredit(int deliveryUserSysNo, int warehouseSysNo)
        {
            const string sql = @"select a.*,b.username,b.status,d.warehousename
                                 from lgdeliveryusercredit a left join syuser b on a.deliveryusersysno=b.sysno
                                  left join whwarehouse d on a.warehousesysno=d.sysno 
                                 where a.deliveryUserSysNo=@0 and a.warehouseSysNo=@1";

            return Context.Sql(sql, deliveryUserSysNo, warehouseSysNo)
                          .QuerySingle<CBLgDeliveryUserCredit>();
        }

        /// <summary>
        /// 查询配送员(业务员)信用配额集合
        /// </summary>
        /// <param name="pager">配送员(业务员)信用配额集合分页对象</param>
        /// <param name="filter">配送员信用查询条件</param>
        /// <returns></returns>
        /// <remarks>2013-06-21 郑荣华 创建</remarks>
        public override void GetLgDeliveryUserCreditList(ref Pager<CBLgDeliveryUserCredit> pager,
                                                         ParaDeliveryUserCreditFilter filter)
        {
            const string whereSql = @"(@username is null or charindex(b.username,@username)>0)
                                     and (@IsAllowBorrow is null or a.IsAllowBorrow=@IsAllowBorrow )
                                     and (@IsAllowDelivery is null or a.IsAllowDelivery=@IsAllowDelivery)
                                     and (@WarehouseSysNo is null or a.WarehouseSysNo=@WarehouseSysNo)";

            const string sqlcount = @"select count(1) from LgDeliveryUserCredit a 
                                     inner join syuser b on a.DeliveryUserSysNo=b.sysno 
                                     where " + whereSql;

            const string fromSql = @"lgdeliveryusercredit a inner join syuser b on a.deliveryusersysno=b.sysno
                                     left join whwarehouse d on a.warehousesysno=d.sysno";

            using (var context = Context.UseSharedConnection(true))
            {
                pager.TotalRows = context.Sql(sqlcount)
                                         .Parameter("username", filter.UserName)
                                         .Parameter("IsAllowBorrow", filter.IsAllowBorrow)
                                         .Parameter("IsAllowDelivery", filter.IsAllowDelivery)
                                         .Parameter("WarehouseSysNo", filter.WarehouseSysNo)
                                         .QuerySingle<int>();

                pager.Rows = context.Select<CBLgDeliveryUserCredit>("a.*,b.username,b.status,d.warehousename")
                                    .From(fromSql)
                                    .Where(whereSql)
                                    .Parameter("username", filter.UserName)
                                    .Parameter("IsAllowBorrow", filter.IsAllowBorrow)
                                    .Parameter("IsAllowDelivery", filter.IsAllowDelivery)
                                    .Parameter("WarehouseSysNo", filter.WarehouseSysNo)
                                    .OrderBy("a.LastUpdateDate desc,a.warehousesysno,a.remainingdeliverycredit,a.deliveryusersysno")
                                    .Paging(pager.CurrentPage, pager.PageSize)
                                    .QueryMany();
            }
        }

        #endregion

    }
}
