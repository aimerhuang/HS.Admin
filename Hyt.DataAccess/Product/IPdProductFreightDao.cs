using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Model.Parameter;

namespace Hyt.DataAccess.Product
{
    /// <summary>
    /// 运费模板 抽象类
    /// </summary>
    /// <remarks>
    /// 015-08-06 王耀发 创建
    /// </remarks>
    public abstract class IPdProductFreightDao : Hyt.DataAccess.Base.DaoBase<IPdProductFreightDao>
    {

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2015-08-06 王耀发 创建</remarks>
        public abstract int Insert(PdProductFreight entity);


        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="PdProductSysNo">产品编号</param>
        /// <returns></returns>
        //// <remarks>2015-08-06 王耀发 创建</remarks>
        public abstract void DeleteByPdProductSysNo(int PdProductSysNo);

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="PdProductSysNo">商品编号</param>
        /// <returns>数据实体</returns>
        /// <remarks>2015-08-06 王耀发 创建</remarks>
        public abstract PdProductFreight GetEntityByPdProductSysNo(int PdProductSysNo);

    }
}

