using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Model.Parameter;
using Hyt.Model.Common;

namespace Hyt.DataAccess.Logistics
{
    /// <summary>
    /// 运费模板 抽象类
    /// </summary>
    /// <remarks>
    /// 015-08-06 王耀发 创建
    /// </remarks>
    public abstract class ILgFreightModuleDao : Hyt.DataAccess.Base.DaoBase<ILgFreightModuleDao>
    {
        /// <summary>
        /// 获取运费模板列表
        /// </summary>
        /// <param name="sysNo">运费模板系统编号</param>
        /// <returns>运费模板列表</returns>
        /// <remarks>2015-08-06 王耀发 创建</remarks>
        public abstract Pager<LgFreightModule> GetLgFreightModuleList(ParaFreightModule filter);

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>数据实体</returns>
        /// <remarks>2015-08-06 王耀发 创建</remarks>
        public abstract LgFreightModule GetEntity(int sysNo);

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="ProductAddress">商品地址编号</param>
        /// <returns>数据实体</returns>
        /// <remarks>2015-08-06 王耀发 创建</remarks>
        public abstract LgFreightModule GetEntityByProductAddress(int ProductAddress);

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2015-08-06 王耀发 创建</remarks>
        public abstract int Insert(LgFreightModule entity);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>修改记录编号</returns>
        /// <remarks>2015-08-21  王耀发 创建</remarks>
        public abstract int Update(LgFreightModule entity);

        /// <summary>
        /// 获取运费模板列表
        /// </summary>
        /// <param name="pager">运费模板查询条件</param>
        /// <returns>运费模板列表</returns>
        /// <remarks>2015-05-12 王耀发 创建</remarks>
        public abstract Pager<LgFreightModule> GetFreightModuleList(Pager<LgFreightModule> pager);

        /// <summary>
        /// 获取仓库地址所对应的运费模板
        /// </summary>
        /// <param name="productAddress">仓库地址编号</param>
        /// <returns>运费模板</returns>
        /// <remarks>2015-9-9 杨浩 创建</remarks>
        public abstract IList<LgFreightModule> GetFreightModuleByProductAddress(int addressSysNo);
        /// <summary>
        /// 获取运费
        /// </summary>
        /// <param name="addressSysNo">收货地址系统编号</param>
        /// <param name="freightModuleSysNo">运费模板系统编号</param>
        /// <param name="productSysNoAndNumber">商品系统编号和购买数量组合（商品系统编号_购买数量,商品系统编号_购买数量...）</param>
        /// <returns></returns>
        /// <remarks>2015-9-9 杨浩 创建</remarks>
        public abstract IList<FareTotal> GetFareTotal(int addressSysNo, int freightModuleSysNo, string productSysNoAndNumber);
        /// <summary>
        /// 获取所有运费模板
        /// </summary>
        /// <returns></returns>
        /// <remarks>2015-9-9 杨浩 创建</remarks>
        public abstract List<LgFreightModule> GetFreightModuleList();
    }
}

