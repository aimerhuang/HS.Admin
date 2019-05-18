using System;
using System.Collections.Generic;
using System.ComponentModel;
using Hyt.Model;
using Hyt.DataAccess.Base;
namespace Hyt.DataAccess.Express
{
    /// <summary>
    ///仓库物流公司账号关联表 数据访问接口
    ///</summary>
    /// <remarks> 2015-10-10 朱成果 创建</remarks>
    public abstract class ILgStoreLogisticsCompanyDao : DaoBase<ILgStoreLogisticsCompanyDao>
    {
        #region 自动生成代码
        /// <summary>
        /// 插入(仓库物流公司账号关联表)
        ///</summary>
        /// <param name="entity">仓库物流公司账号关联表</param>
        /// <returns>新增记录编号</returns>
        /// <remarks> 2015-10-10 朱成果 创建</remarks>
        public abstract int Insert(LgStoreLogisticsCompany entity);

        /// <summary>
        /// 更新(仓库物流公司账号关联表)
        /// </summary>
        /// <param name="entity">仓库物流公司账号关联表</param>
        /// <returns>影响的行</returns>
        /// <remarks> 2015-10-10 朱成果 创建</remarks>
        public abstract int Update(LgStoreLogisticsCompany entity);

        /// <summary>
        /// 获取(仓库物流公司账号关联表)
        ///</summary>
        /// <param name="sysno">系统编号</param>
        /// <returns>仓库物流公司账号关联表</returns>
        /// <remarks> 2015-10-10 朱成果 创建</remarks>
        public abstract LgStoreLogisticsCompany GetEntity(int sysno);

        /// <summary>
        /// 删除(仓库物流公司账号关联表)
        ///</summary>
        /// <param name="sysno">系统编号</param>
        /// <returns>影响的行</returns>
        /// <remarks> 2015-10-10 朱成果 创建</remarks>
        public abstract int Delete(int sysno);
        #endregion

        #region 自定义

        /// <summary>
        /// 返回仓库物流公司账号关联实体对象（根据仓库编号  物流账号编号）
        /// </summary>
        /// <param name="entity">仓库物流公司账号关联 实体</param>
        /// <returns>仓库物流公司账号关联实体</returns>
        /// <remarks>2015-10-12 王江 创建</remarks>
        public abstract LgStoreLogisticsCompany IsExistRecord(LgStoreLogisticsCompany entity);

        /// <summary>
        /// 返回已关联仓库编号列表（根据物流账号编号 AccountSysNo）
        /// </summary>
        /// <param name="entity">仓库物流公司账号关联 实体</param>
        /// <returns>仓库物流公司账号关联结果集</returns>
        /// <remarks>2015-10-12 王江 创建</remarks>
        public abstract IList<LgStoreLogisticsCompany> GetRelateWarehouseListByAccountSysNo(int accountSysNo);
        
        #endregion
    }

}
