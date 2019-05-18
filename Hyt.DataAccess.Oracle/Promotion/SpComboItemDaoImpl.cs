using Hyt.Model;
using System.Collections.Generic;
using Hyt.DataAccess.Base;
using Hyt.DataAccess.Promotion;
namespace Hyt.DataAccess.Oracle.Promotion
{
    /// <summary>
    /// 组合套餐明细
    /// </summary>
    /// <remarks>2013-08-21  朱成果 创建</remarks>
    public class SpComboItemDaoImpl : ISpComboItemDao
    {

        #region 数据记录增，删，改，查
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2013-08-21  朱成果 创建</remarks>
        public override int Insert(SpComboItem entity)
        {
            entity.SysNo = Context.Insert("SpComboItem", entity)
                                        .AutoMap(o => o.SysNo)
                                        .ExecuteReturnLastId<int>("SysNo");
            return entity.SysNo;
        }
        /// <summary>
        /// 判断同一分销商下同一仓库是否建了相同名称的组合套餐
        /// </summary>
        /// <param name="DealerSysNo"></param>
        /// <param name="WarehouseSysNo"></param>
        /// <param name="Title"></param>
        /// <returns></returns>
        public override int IsRepeatSpCombo(int DealerSysNo, int WarehouseSysNo, string Title)
        {
            string sql = "SELECT count(1) from SpCombo where DealerSysNo=" + DealerSysNo + " And  WarehouseSysNo=" + WarehouseSysNo + " and Title='" + Title + "'";
            return Context.Sql(sql).QuerySingle<int>();
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns></returns>
        /// <remarks>2013-08-21  朱成果 创建</remarks>
        public override void Update(SpComboItem entity)
        {

            Context.Update("SpComboItem", entity)
                   .AutoMap(o => o.SysNo)
                   .Where("SysNo", entity.SysNo)
                   .Execute();
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>数据实体</returns>
        /// <remarks>2013-08-21  朱成果 创建</remarks>
        public override SpComboItem GetEntity(int sysNo)
        {

            return Context.Sql("select * from SpComboItem where SysNo=@SysNo")
                   .Parameter("SysNo", sysNo)
              .QuerySingle<SpComboItem>();
        }

        /// <summary>
        /// 获取组合套餐明细列表
        /// </summary>
        /// <param name="comboSysNo">组合套餐系统编号</param>
        /// <returns>组合套餐明细列表</returns>
        /// <remarks>2013-08-21  朱成果 创建</remarks>
        public override List<SpComboItem> GetListByComboSysNo(int comboSysNo)
        {
            return Context.Sql("select * from SpComboItem where ComboSysNo=@ComboSysNo")
                   .Parameter("ComboSysNo", comboSysNo)
                  .QueryMany<SpComboItem>();
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns></returns>
        /// <remarks>2013-08-21  朱成果 创建</remarks>
        public override void Delete(int sysNo)
        {
            Context.Sql("Delete from SpComboItem where ComboSysNo=@SysNo")
                 .Parameter("SysNo", sysNo)
            .Execute();
        }
        #endregion

    }
}
