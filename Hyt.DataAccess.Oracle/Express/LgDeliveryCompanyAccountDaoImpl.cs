using System;
using System.Collections;
using System.ComponentModel;
using Hyt.Model;
using Hyt.DataAccess.Express;
using Hyt.Model.Transfer;

namespace Hyt.DataAccess.Oracle.Express
{
    /// <summary>
    ///物流公司账号表 数据访问实现
    ///</summary>
    /// <remarks> 2015-10-10 朱成果 创建</remarks>
    public class LgDeliveryCompanyAccountDaoImpl : ILgDeliveryCompanyAccountDao
    {
        #region 自动生成代码
        /// <summary>
        /// 插入(物流公司账号表)
        ///</summary>
        /// <param name="entity">物流公司账号表</param>
        /// <returns>新增记录编号</returns>
        /// <remarks> 2015-10-10 朱成果 创建</remarks>
        public override int Insert(LgDeliveryCompanyAccount entity)
        {
            entity.SysNo = Context.Insert("LGDELIVERYCOMPANYACCOUNT", entity)
                                .AutoMap(o => o.SysNo)
                                .ExecuteReturnLastId<int>("SysNo");
            return entity.SysNo;
        }

        /// <summary>
        /// 更新(物流公司账号表)
        ///</summary>
        /// <param name="entity">物流公司账号表</param>
        /// <returns>影响的行</returns>
        /// <remarks> 2015-10-10 朱成果 创建</remarks>
        public override int Update(LgDeliveryCompanyAccount entity)
        {
            return Context.Update("LGDELIVERYCOMPANYACCOUNT", entity)
                   .AutoMap(o => o.SysNo)
                   .Where("SysNo", entity.SysNo)
                   .Execute();
        }

        /// <summary>
        /// 获取(物流公司账号表)
        ///</summary>
        /// <param name="sysno">系统编号</param>
        /// <returns>物流公司账号表</returns>
        /// <remarks> 2015-10-10 朱成果 创建</remarks>
        public override LgDeliveryCompanyAccount GetEntity(int sysno)
        {
            return Context.Sql("select * from LGDELIVERYCOMPANYACCOUNT where SysNo=@sysno")
                           .Parameter("sysno", sysno)
                           .QuerySingle<LgDeliveryCompanyAccount>();

        }

        /// <summary>
        /// 删除(物流公司账号表)
        ///</summary>
        /// <param name="sysno">系统编号</param>
        /// <returns>影响的行</returns>
        /// <remarks> 2015-10-10 朱成果 创建</remarks>
        public override int Delete(int sysno)
        {
            return Context.Sql("delete from LGDELIVERYCOMPANYACCOUNT where SysNo=@sysno")
                           .Parameter("sysno", sysno)
                           .Execute();
        }
        #endregion

        #region 自定义

        #region 界面分页列表
        /// <summary>
        /// 界面分页列表
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>分页列表</returns>
        /// <remarks>2015-10-9 王江 创建</remarks>
        public override Pager<CBLgDeliveryCompanyAccount> GetElectronicsSurfaceList(Model.Parameter.ParaElectronicsSurfaceFilter filter)
        {
            string sqlSelect = @"m.sysno,m.accountname,n.deliverytypename,m.accountid,m.accountsecretkey,s.username,m.createdate,d.username username1,m.lastupdatedate";

            string sqlFrom = @"LGDELIVERYCOMPANYACCOUNT m left join LgDeliveryType n on m.deliverytypesysno=n.sysno left join SyUser s on m.createby=s.sysno left join SyUser d on m.lastupdateby=d.sysno";

            var parameters = new ArrayList();
            System.Text.StringBuilder strWhere = new System.Text.StringBuilder("1=1");

            if (!string.IsNullOrWhiteSpace(filter.AccountName)) //经销商城类型
            {
                strWhere.AppendFormat(" and m.AccountName=@AccountName", filter.AccountName);
                parameters.Add(filter.AccountName);
            }
            if (filter.DeliveryTypeSysNo.HasValue && filter.DeliveryTypeSysNo.Value > 0) //配送方式
            {
                strWhere.AppendFormat(" and m.DeliveryTypeSysNo=@DeliveryTypeSysNo", filter.DeliveryTypeSysNo.Value);
                parameters.Add(filter.DeliveryTypeSysNo.Value);
            }
            var pager = new Pager<CBLgDeliveryCompanyAccount>
            {
                CurrentPage = filter.Id,
                PageSize = filter.PageSize
            };

            using (var context = Context.UseSharedConnection(true))
            {
                var results = context.Select<CBLgDeliveryCompanyAccount>(sqlSelect)
                                       .From(sqlFrom)
                                       .AndWhere(strWhere.ToString())
                                       .Parameters(parameters)
                                       .Paging(filter.Id, filter.PageSize) //index从1开始
                                       .OrderBy("m.SysNo asc")
                                       .QueryMany();
                var count = context.Select<int>(@"count(*)")
                                   .From(sqlFrom)
                                   .AndWhere(strWhere.ToString())
                                   .Parameters(parameters)
                                   .QuerySingle();
                pager.Rows = results;
                pager.TotalRows = count;
            }
            return pager;
        }
        #endregion

        #region 检测名称是否存在

        /// <summary>
        /// 检测名称是否存在
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>返回true则存在</returns>
        /// <remarks>2015-10-10 王江 创建</remarks>
        public override bool IsExistName(LgDeliveryCompanyAccount entity)
        {
            return Context.Sql("select count(1) from LGDELIVERYCOMPANYACCOUNT m where m.accountname=@accountname")
                           .Parameter("accountname", entity.AccountName).QuerySingle<int>()>0;
        }

        /// <summary>
        /// 检测名称是否存在(不包含此记录本身)
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>返回true则存在</returns>
        /// <remarks>2015-10-10 王江 创建</remarks>
        public override bool IsExistNameWithOutSelf(LgDeliveryCompanyAccount entity)
        {
            return Context.Sql("select count(1) from LGDELIVERYCOMPANYACCOUNT m where m.accountname=@accountname and m.sysno!=@paraSysNo")
                           .Parameter("accountname", entity.AccountName).Parameter("paraSysNo", entity.SysNo).QuerySingle<int>() > 0;
        }

        #endregion

        #region 根据配送方式编号与仓库编号返回实体
        /// <summary>
        /// 根据配送方式编号与仓库编号返回实体
        /// </summary>
        /// <param name="warehouseSysNo">仓库编号</param>
        /// <param name="deliveryTypeSysNo">配送方式编号</param>
        /// <returns></returns>
        public override LgDeliveryCompanyAccount GetEntityByWarehouseNoAndDeliveryTypeNo(int warehouseSysNo, int deliveryTypeSysNo)
        {
            return Context.Sql("select n.* from LgStoreLogisticsCompany m left join LGDELIVERYCOMPANYACCOUNT n on m.accountsysno=n.sysno where n.deliverytypesysno=@deliverytypesysno and  m.warehousesysno=@warehousesysno")
                           .Parameter("deliverytypesysno", deliveryTypeSysNo)
                           .Parameter("warehousesysno", warehouseSysNo)
                           .QuerySingle<LgDeliveryCompanyAccount>();

        }


        #endregion

        #endregion
    }

}
