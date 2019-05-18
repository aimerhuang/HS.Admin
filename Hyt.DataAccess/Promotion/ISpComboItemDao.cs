using System.Collections.Generic;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Parameter;
namespace Hyt.DataAccess.Promotion
{
    /// <summary>
    /// 促销明细
    /// </summary>
    /// <remarks>2013-08-21  朱成果 创建</remarks>
    public abstract class ISpComboItemDao : DaoBase<ISpComboItemDao>
    {

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2013-08-21  朱成果 创建</remarks>
        public abstract int Insert(SpComboItem entity);
         /// <summary>
        /// 判断同一分销商下同一仓库是否建了相同名称的组合套餐
        /// </summary>
        /// <param name="DealerSysNo"></param>
        /// <param name="WarehouseSysNo"></param>
        /// <param name="Title"></param>
        /// <returns></returns>
        public abstract int IsRepeatSpCombo(int DealerSysNo, int WarehouseSysNo, string Title);
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns></returns>
        /// <remarks>2013-08-21  朱成果 创建</remarks>
        public abstract void Update(SpComboItem entity);

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>数据实体</returns>
        /// <remarks>2013-08-21  朱成果 创建</remarks>
        public abstract SpComboItem GetEntity(int sysNo);

        /// <summary>
        /// 获取组合套餐明细列表
        /// </summary>
        /// <param name="comboSysNo">组合套餐系统编号</param>
        /// <returns>组合套餐明细列表</returns>
        /// <remarks>2013-08-21  朱成果 创建</remarks>
        public abstract List<SpComboItem> GetListByComboSysNo(int comboSysNo);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns></returns>
        /// <remarks>2013-08-21  朱成果 创建</remarks>
        public abstract void Delete(int sysNo);

    }
}
