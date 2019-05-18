using System;
using Hyt.DataAccess.Base;
using Hyt.Model;
using System.Collections.Generic;

namespace Hyt.DataAccess.CRM
{
    /// <summary>
    /// 大宗采购接口抽象类
    /// </summary>
    /// <remarks>2013－06-25 杨晗 创建</remarks>
    public abstract class ICrBulkPurchaseDao : DaoBase<ICrBulkPurchaseDao>
    {
        /// <summary>
        /// 根据系统编号获取大宗采购模型
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>大宗采购实体</returns>
        /// <remarks>2013－06-25 杨晗 创建</remarks>
        public abstract CrBulkPurchase GetModel(int sysNo);

        /// <summary>
        /// 大宗采购分页查询
        /// </summary>
        /// <param name="pager">大宗采购查询条件</param>
        /// <returns>大宗采购信息列表</returns>
        /// <remarks>2013－06-25 杨晗 创建</remarks>
        public abstract Pager<CBCrBulkPurchase> Seach(Pager<CBCrBulkPurchase> pager);
      
        /// <summary>
        /// 插入大宗采购信息
        /// </summary>
        /// <param name="model">插入的数据模型</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－06-25 杨晗 创建</remarks>
        public abstract int Insert(CrBulkPurchase model);

        /// <summary>
        /// 更新大宗采购信息
        /// </summary>
        /// <param name="model">更新的数据模型</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－06-25 杨晗 创建</remarks>
        public abstract int Update(CrBulkPurchase model);

        /// <summary>
        /// 删除大宗采购信息
        /// </summary>
        /// <param name="sysNo">大宗采购系统编号</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks>2013－06-25 杨晗 创建</remarks>
        public abstract bool Delete(int sysNo);
    }
}
