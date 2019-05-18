using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Model.Parameter;

namespace Hyt.DataAccess.Logistics
{
    /// <summary>
    /// 运费模板 抽象类
    /// </summary>
    /// <remarks>
    /// 015-08-06 王耀发 创建
    /// </remarks>
    public abstract class IFreightModuleDetailsDao : Hyt.DataAccess.Base.DaoBase<IFreightModuleDetailsDao>
    {
        /// <summary>
        /// 更新运费模板详情
        /// </summary>
        /// <param name="entity">运费模板详情实体</param>
        /// <returns></returns>
        /// <remarks>2015-11-22 杨浩 创建</remarks>
        public abstract int UpdateFreightModuleDetails(LgFreightModuleDetails entity);
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2015-08-06 王耀发 创建</remarks>
        public abstract int Insert(LgFreightModuleDetails entity);

        /// <summary>
        /// 获取运费明细
        /// </summary>
        /// <param name="FreightModuleSysNo">运费模板编号</param>
        /// <param name="IsPost">是否包邮</param>
        /// <param name="ValuationStyle">计价方式</param>
        /// <returns>运费明细列表</returns>
        /// <remarks>2015-08-10  王耀发 修改</remarks>
        public abstract List<LgFreightModuleDetails> GetFreightModuleDetailsBy(int FreightModuleSysNo, int IsPost, int ValuationStyle, int DeliveryStyle);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns></returns>
        /// <remarks>2015-08-06 王耀发 创建</remarks>
        public abstract void Delete(int sysNo);
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns></returns>
        /// <remarks>2015-11-22 杨浩 创建</remarks>
        public abstract void DeleteBySysNos(string sysNos);
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="FreightModuleSysNo">运费模板编号</param>
        /// <returns></returns>
        //// <remarks>2015-08-06 王耀发 创建</remarks>
        public abstract void DeleteByFreightModuleSysNo(int FreightModuleSysNo);
    }
}

