using System;
using System.ComponentModel;
using Hyt.Model;
using Hyt.DataAccess.Base;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;

namespace Hyt.DataAccess.Express
{
    /// <summary>
    ///物流公司账号表 数据访问接口
    ///</summary>
    /// <remarks> 2015-10-10 朱成果 创建</remarks>
    public abstract class ILgDeliveryCompanyAccountDao : DaoBase<ILgDeliveryCompanyAccountDao>
    {
        #region 自动生成代码
        /// <summary>
        /// 插入(物流公司账号表)
        ///</summary>
        /// <param name="entity">物流公司账号表</param>
        /// <returns>新增记录编号</returns>
        /// <remarks> 2015-10-10 朱成果 创建</remarks>
        public abstract int Insert(LgDeliveryCompanyAccount entity);

        /// <summary>
        /// 更新(物流公司账号表)
        /// </summary>
        /// <param name="entity">物流公司账号表</param>
        /// <returns>影响的行</returns>
        /// <remarks> 2015-10-10 朱成果 创建</remarks>
        public abstract int Update(LgDeliveryCompanyAccount entity);

        /// <summary>
        /// 获取(物流公司账号表)
        ///</summary>
        /// <param name="sysno">系统编号</param>
        /// <returns>物流公司账号表</returns>
        /// <remarks> 2015-10-10 朱成果 创建</remarks>
        public abstract LgDeliveryCompanyAccount GetEntity(int sysno);

        /// <summary>
        /// 删除(物流公司账号表)
        ///</summary>
        /// <param name="sysno">系统编号</param>
        /// <returns>影响的行</returns>
        /// <remarks> 2015-10-10 朱成果 创建</remarks>
        public abstract int Delete(int sysno);
        #endregion

        #region 自定义
        /// <summary>
        /// 分页列表
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        /// <remarks>2015-10-10 王江 创建</remarks>
        public abstract Pager<CBLgDeliveryCompanyAccount> GetElectronicsSurfaceList(ParaElectronicsSurfaceFilter filter);

        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <remarks>2015-10-10 王江 创建</remarks>
        public abstract bool IsExistName(LgDeliveryCompanyAccount entity);

        /// <summary>
        /// 判断是否存在，不包含自身
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <remarks>2015-10-10 王江 创建</remarks>
        public abstract bool IsExistNameWithOutSelf(LgDeliveryCompanyAccount entity);


        /// <summary>
        /// 根据配送方式编号与仓库编号返回实体
        /// </summary>
        /// <param name="warehouseSysNo"></param>
        /// <param name="deliveryTypeSysNo"></param>
        /// <returns></returns>
        public abstract LgDeliveryCompanyAccount GetEntityByWarehouseNoAndDeliveryTypeNo(int warehouseSysNo,int deliveryTypeSysNo);

        #endregion
    }

}
