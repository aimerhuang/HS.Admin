using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Model.Parameter;

namespace Hyt.DataAccess.SellBusiness
{
    /// <summary>
    /// 分销商等级信息
    /// </summary>
    /// <param name="filter">分销商等级信息</param>
    /// <returns>返回分销商等级信息</returns>
    /// <remarks>2015-09-15 王耀发 创建</remarks>
    public abstract class ICrSellBusinessGradeDao : Hyt.DataAccess.Base.DaoBase<ICrSellBusinessGradeDao>
    {
        /// <summary>
        /// 获取分销商列表
        /// </summary>
        /// <param name="sysNo">分销商系统编号</param>
        /// <returns>分销商列表</returns>
        /// <remarks>2015-08-30 王耀发 创建</remarks>
        public abstract Pager<CrSellBusinessGrade> GetCrSellBusinessGradeList(ParaSellBusinessGradeFilter filter);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="WarehouseSysNo"></param>
        /// <param name="PdProductSysNo"></param>
        /// <returns></returns>
        /// <remarkss>2015-08-06 王耀发 创建</remarks>
        public abstract CrSellBusinessGrade GetEntity(int SysNo);

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2015-08-06 王耀发 创建</remarks>
        public abstract int Insert(CrSellBusinessGrade entity);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>修改记录编号</returns>
        /// <remarks>2015-08-21  王耀发 创建</remarks>
        public abstract int Update(CrSellBusinessGrade entity);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>删除记录</returns>
        /// <remarks>2015-08-30 王耀发 创建</remarks>
        public abstract int Delete(int sysNo);
    }
}

