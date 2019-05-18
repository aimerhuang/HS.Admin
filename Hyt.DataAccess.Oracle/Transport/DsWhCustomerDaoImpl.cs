using Hyt.DataAccess.Transport;
using Hyt.Model.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.Transport
{
    /// <summary>
    /// 转运系统客户实体
    /// </summary>
    /// <remarks>
    /// 2016-05-17 杨云奕 添加
    /// </remarks>
    public class DsWhCustomerDaoImpl : IDsWhCustomerDao
    {
        /// <summary>
        /// 添加客户实体
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public override int InsertMod(Model.Transport.DsWhCustomer mod)
        {
            return Context.Insert<DsWhCustomer>("DsWhCustomer", mod).AutoMap(p => p.SysNo).ExecuteReturnLastId<int>("SysNo");
        }
        /// <summary>
        /// 修改客户实体
        /// </summary>
        /// <param name="mod"></param>
        public override void UpdateMod(Model.Transport.DsWhCustomer mod)
        {
            Context.Update<DsWhCustomer>("DsWhCustomer", mod).AutoMap(p => p.SysNo).Where(p => p.SysNo).Execute();
        }
        /// <summary>
        /// 获取客户列表
        /// </summary>
        /// <returns></returns>
        public override List<Model.Transport.DsWhCustomer> GetCustomerList()
        {
            string sql = " select *from DsWhCustomer ";
            return Context.Sql(sql).QueryMany<DsWhCustomer>();
        }
        /// <summary>
        /// 通过编号获取客户实体
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public override Model.Transport.DsWhCustomer GetCustomerBySysNo(int SysNo)
        {
            string sql = " select * from  DsWhCustomer where SysNo = '" + SysNo + "' ";
            return Context.Sql(sql).QuerySingle<DsWhCustomer>();
        }
        /// <summary>
        /// 通过编号删除客户
        /// </summary>
        /// <param name="SysNo"></param>
        public override void DeleteCustomerBySysNo(int SysNo)
        {
            string sql = " delete from DsWhCustomer where SysNo = '" + SysNo + "' ";
            Context.Sql(sql).Execute();
        }
        /// <summary>
        /// 通过系统账号获取客户信息
        /// </summary>
        /// <param name="Account"></param>
        /// <returns></returns>
        public override DsWhCustomer GetCustomerByAssAccount(string Account)
        {
            string sql = " select * from  DsWhCustomer where AssAccount = '" + Account + "' ";
            return Context.Sql(sql).QuerySingle<DsWhCustomer>();
        }

        public override void DoDsWhCustomerQuery(ref Model.Pager<CBDsWhCustomer> pageCusList)
        {
            #region sql条件
            string sqlWhere = @"  ";
            #endregion
            if (pageCusList.PageFilter.IsAllDealer)
            {
                sqlWhere = " 1=1 ";
            }
            else if (pageCusList.PageFilter.IsDealer)
            {
                sqlWhere = " DsWhCustomer.DsSysNo = '" + pageCusList.PageFilter.DsSysNo + "' ";
            }
            else if (pageCusList.PageFilter.IsCustomer)
            {
                sqlWhere = " DsWhCustomer.SysNo = '" + pageCusList.PageFilter.SysNo + "' ";
            }

            using (var context = Context.UseSharedConnection(true))
            {
                pageCusList.Rows = context.Select<CBDsWhCustomer>("  DsWhCustomer.*, DsDealer.DealerName as DsName , DsDealer.ErpCode as DsCode , (a.AreaName+' '+b.AreaName+' '+c.AreaName) as CountryName  ")
                           .From(@"  DsWhCustomer inner join DsDealer on DsWhCustomer.DsSysNo=DsDealer.SysNo
                                     inner join BsArea c on DsWhCustomer.CusCountryCode=c.SysNo
                                     inner join BsArea b on c.ParentSysNo=b.SysNo
                                     inner join BsArea a on b.ParentSysNo=a.SysNo ")
                           .Where(sqlWhere)
                           .Paging(pageCusList.CurrentPage, pageCusList.PageSize)
                           .OrderBy("DsWhCustomer.SysNo desc")
                           .QueryMany();
                pageCusList.TotalRows = context.Select<int>("count(1)")
                           .From(@"  DsWhCustomer inner join DsDealer on DsWhCustomer.DsSysNo=DsDealer.SysNo
                                     inner join BsArea c on DsWhCustomer.CusCountryCode=c.SysNo
                                     inner join BsArea b on c.ParentSysNo=b.SysNo
                                     inner join BsArea a on b.ParentSysNo=a.SysNo ")
                           .Where(sqlWhere)
                           .QuerySingle();
            }
        }

        public override DsWhCustomer GetCustomerByCusCode(string CusCode)
        {
            string sql = "select * from DsWhCustomer where CusCode = '" + CusCode + "' ";
            return Context.Sql(sql).QuerySingle<DsWhCustomer>();
        }
    }
}
