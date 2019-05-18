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
    public class GsSupportAreaDaoImpl : IGsSupportAreaDao
    {
        /// <summary>
        /// 插入团购区域表
        /// </summary>
        /// <param name="entity">团购区域实体</param>
        /// <returns>团购区域实体（带编号）</returns>
        /// <remarks>2013-08-21 余勇  创建</remarks>
        public override GsSupportArea InsertEntity(GsSupportArea entity)
        {
            entity.SysNo = Context.Insert("GsSupportArea", entity)
                                             .AutoMap(o => o.SysNo)
                                            .ExecuteReturnLastId<int>("SysNo");
            return entity;
        }

        /// <summary>
        /// 获取团购区域列表
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>团购商品表</returns>
        /// <remarks>2013-08-22 余勇 创建</remarks>
        public override IList<GsSupportArea> GetItem(int sysNo)
        {
            return Context.Sql("select * from GsSupportArea where GroupShoppingSysNo=@GroupShoppingSysNo")
                 .Parameter("GroupShoppingSysNo", sysNo)
                .QueryMany<GsSupportArea>();

        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>空</returns>
        /// <remarks>2013-08-21  余勇 创建</remarks>
        public override void Delete(int sysNo)
        {
            Context.Sql("Delete from GsSupportArea where GroupShoppingSysNo=@GroupShoppingSysNo")
                 .Parameter("GroupShoppingSysNo", sysNo)
            .Execute();
        }
    }
}
