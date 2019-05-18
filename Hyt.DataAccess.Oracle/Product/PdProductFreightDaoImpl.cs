using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Product;
using Hyt.Model;
using Hyt.Model.LogisApp;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Util;
using Hyt.Model.Parameter;

namespace Hyt.DataAccess.Oracle.Product
{
    /// <summary>
    /// 取运费模板数据访问类
    /// </summary>
    /// <remarks>
    /// 2015-08-06 王耀发 创建
    /// </remarks>
    public class PdProductFreightDaoImpl : IPdProductFreightDao
    {

        #region 数据记录增，删，改，查
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2015-08-3  王耀发 创建</remarks>
        public override int Insert(PdProductFreight entity)
        {
            entity.SysNo = Context.Insert("PdProductFreight", entity)
                                        .AutoMap(o => o.SysNo)
                                        .ExecuteReturnLastId<int>("SysNo");
            return entity.SysNo;
        }


        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="FreightModuleSysNo">运费模板编号</param>
        /// <returns></returns>
        /// <remarks>2015-08-21  王耀发 创建</remarks>
        public override void DeleteByPdProductSysNo(int PdProductSysNo)
        {
            Context.Sql("Delete from PdProductFreight where PdProductSysNo=@PdProductSysNo")
                 .Parameter("PdProductSysNo", PdProductSysNo)
            .Execute();
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="PdProductSysNo">商品编号</param>
        /// <returns>数据实体</returns>
        /// <remarks>2015-08-06 王耀发 创建</remarks>
        public override PdProductFreight GetEntityByPdProductSysNo(int PdProductSysNo)
        {

            return Context.Sql("select a.* from PdProductFreight a where a.PdProductSysNo=@PdProductSysNo")
                   .Parameter("PdProductSysNo", PdProductSysNo)
              .QuerySingle<PdProductFreight>();
        }
        #endregion
    }
}
