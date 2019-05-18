using System;
using System.Collections.Generic;
using System.ComponentModel;
using Hyt.Model;
using Hyt.DataAccess.Express;
namespace Hyt.DataAccess.Oracle.Express
{
    /// <summary>
    ///仓库物流公司账号关联表 数据访问实现
    ///</summary>
    /// <remarks> 2015-10-10 朱成果 创建</remarks>
    public class LgStoreLogisticsCompanyDaoImpl : ILgStoreLogisticsCompanyDao
    {
        #region 自动生成代码
        /// <summary>
        /// 插入(仓库物流公司账号关联表)
        ///</summary>
        /// <param name="entity">仓库物流公司账号关联表</param>
        /// <returns>新增记录编号</returns>
        /// <remarks> 2015-10-10 朱成果 创建</remarks>
        public override int Insert(LgStoreLogisticsCompany entity)
        {
            entity.SysNo = Context.Insert("LGSTORELOGISTICSCOMPANY", entity)
                                .AutoMap(o => o.SysNo)
                                .ExecuteReturnLastId<int>("SysNo");
            return entity.SysNo;
        }

        /// <summary>
        /// 更新(仓库物流公司账号关联表)
        ///</summary>
        /// <param name="entity">仓库物流公司账号关联表</param>
        /// <returns>影响的行</returns>
        /// <remarks> 2015-10-10 朱成果 创建</remarks>
        public override int Update(LgStoreLogisticsCompany entity)
        {
            return Context.Update("LGSTORELOGISTICSCOMPANY", entity)
                   .AutoMap(o => o.SysNo)
                   .Where("SysNo", entity.SysNo)
                   .Execute();
        }

        /// <summary>
        /// 获取(仓库物流公司账号关联表)
        ///</summary>
        /// <param name="sysno">系统编号</param>
        /// <returns>仓库物流公司账号关联表</returns>
        /// <remarks> 2015-10-10 朱成果 创建</remarks>
        public override LgStoreLogisticsCompany GetEntity(int sysno)
        {
            return Context.Sql("select * from LGSTORELOGISTICSCOMPANY where SysNo=@sysno")
                           .Parameter("sysno", sysno)
                           .QuerySingle<LgStoreLogisticsCompany>();

        }

        /// <summary>
        /// 删除(仓库物流公司账号关联表)
        ///</summary>
        /// <param name="sysno">系统编号</param>
        /// <returns>影响的行</returns>
        /// <remarks> 2015-10-10 朱成果 创建</remarks>
        public override int Delete(int sysno)
        {
            return Context.Sql("delete from LGSTORELOGISTICSCOMPANY where SysNo=@sysno")
                           .Parameter("sysno", sysno)
                           .Execute();
        }
        #endregion

        #region 自定义

        /// <summary>
        /// 返回仓库物流公司账号关联实体对象（根据仓库编号  物流账号编号）
        /// </summary>
        /// <param name="entity">仓库物流公司账号关联 实体</param>
        /// <returns></returns>
        /// <remarks>2015-10-12 王江 创建</remarks>
        public override LgStoreLogisticsCompany IsExistRecord(LgStoreLogisticsCompany entity)
        {
            return Context.Sql("select * from LgStoreLogisticsCompany m where m.warehousesysno=:WarehouseSysNo and m.accountsysno=@AccountSysNo")
               .Parameter("WarehouseSysNo", entity.WarehouseSysNo)
               .Parameter("AccountSysNo", entity.AccountSysNo)
               .QuerySingle<LgStoreLogisticsCompany>();
        }

        /// <summary>
        /// 返回已关联仓库编号列表（根据物流账号编号 AccountSysNo）
        /// </summary>
        /// <param name="entity">仓库物流公司账号关联 实体</param>
        /// <returns></returns>
        /// <remarks>2015-10-12 王江 创建</remarks>
        public override IList<LgStoreLogisticsCompany> GetRelateWarehouseListByAccountSysNo(int accountSysNo)
        {
            return Context.Sql("select * from LgStoreLogisticsCompany m where m.accountsysno=@AccountSysNo")
               .Parameter("AccountSysNo", accountSysNo)
               .QueryMany<LgStoreLogisticsCompany>();
        }
        
        #endregion
    }
}
