using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Promotion;
using Hyt.Model;
using Hyt.Model.Parameter;

namespace Hyt.DataAccess.Oracle.Promotion
{
    /// <summary>
    /// 团购
    /// </summary>
    /// <remarks>2013-08-20 朱家宏 创建</remarks>
    public class GsGroupShoppingItemDaoImpl : IGsGroupShoppingItemDao
    {
        /// <summary>
        /// 插入团购表
        /// </summary>
        /// <param name="entity">团购表实体</param>
        /// <returns>团购表实体（带编号）</returns>
        /// <remarks>2013-08-21 余勇  创建</remarks>
        public override GsGroupShoppingItem InsertEntity(GsGroupShoppingItem entity)
        {
            entity.SysNo = Context.Insert("GsGroupShoppingItem", entity)
                                             .AutoMap(o => o.SysNo)
                                            .ExecuteReturnLastId<int>("SysNo");
            return entity;
        }

        /// <summary>
        /// 获取团购明细
        /// </summary>
        /// <returns>团购明细</returns>
        /// <remarks>2013-09-01 吴文强 创建</remarks>
        public override GsGroupShoppingItem GetEntity(int sysNo)
        {
            return Context.Sql("select * from GsGroupShoppingItem where SysNo=@SysNo")
                 .Parameter("SysNo", sysNo)
                .QuerySingle<GsGroupShoppingItem>();
        }

        /// <summary>
        /// 获取团购商品表
        /// </summary>
        /// <returns>团购商品列表</returns>
        /// <remarks>2013-08-22 余勇 创建</remarks>
        public override IList<GsGroupShoppingItem> GetItem(int sysNo)
        {
            return Context.Sql("select * from GsGroupShoppingItem where GroupShoppingSysNo=@GroupShoppingSysNo")
                 .Parameter("GroupShoppingSysNo", sysNo)
                .QueryMany<GsGroupShoppingItem>();

        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>空</returns>
        /// <remarks>2013-08-21  余勇 创建</remarks>
        public override void Delete(int sysNo)
        {
            Context.Sql("Delete from GsGroupShoppingItem where GroupShoppingSysNo=@GroupShoppingSysNo")
                 .Parameter("GroupShoppingSysNo", sysNo)
            .Execute();
        }

    }
}
