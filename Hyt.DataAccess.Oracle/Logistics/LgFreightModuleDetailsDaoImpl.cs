using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.DataAccess.Logistics;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Util;
namespace Hyt.DataAccess.Oracle.Logistics
{
    /// <summary>
    /// 取运费模板数据访问类
    /// </summary>
    /// <remarks>
    /// 2015-08-06 王耀发 创建
    /// </remarks>
    public class LgFreightModuleDetailsDaoImpl : IFreightModuleDetailsDao
    {

        #region 数据记录增，删，改，查
        /// <summary>
        /// 更新运费模板详情
        /// </summary>
        /// <param name="entity">运费模板详情实体</param>
        /// <returns></returns>
        /// <remarks>2015-11-22 杨浩 创建</remarks>
        public override int UpdateFreightModuleDetails(LgFreightModuleDetails entity)
        {
            return Context.Update<LgFreightModuleDetails>("LgFreightModuleDetails", entity)
             .AutoMap(x => x.SysNo, x => x.CreatedDate, x => x.CreatedBy)
             .Where("sysno", entity.SysNo)
             .Execute();
        }
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2013-08-21  朱成果 创建</remarks>
        /// <remarks>2015-08-3  王耀发 修改</remarks>
        public override int Insert(LgFreightModuleDetails entity)
        {
            entity.SysNo = Context.Insert("LgFreightModuleDetails", entity)
                                        .AutoMap(o => o.SysNo)
                                        .ExecuteReturnLastId<int>("SysNo");
            return entity.SysNo;
        }

        /// <summary>
        /// 获取运费明细
        /// </summary>
        /// <param name="FreightModuleSysNo">运费模板编号</param>
        /// <param name="IsPost">是否包邮</param>
        /// <param name="ValuationStyle">计价方式</param>
        /// <returns>运费明细列表</returns>
        /// <remarks>2015-08-10  王耀发 修改</remarks>
        public override List<LgFreightModuleDetails> GetFreightModuleDetailsBy(int FreightModuleSysNo, int IsPost, int ValuationStyle, int DeliveryStyle)
        {
            return Context.Sql(@"
                                select a.*
                                from  LgFreightModuleDetails a
                                where a.FreightModuleSysNo=@FreightModuleSysNo and a.IsPost=@IsPost and a.ValuationStyle=@ValuationStyle and a.DeliveryStyle=@DeliveryStyle")
                .Parameter("FreightModuleSysNo", FreightModuleSysNo)
                .Parameter("IsPost", IsPost)
                .Parameter("ValuationStyle", ValuationStyle)
                .Parameter("DeliveryStyle", DeliveryStyle)
                .QueryMany<LgFreightModuleDetails>();
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns></returns>
        /// <remarks>2015-08-21  王耀发 创建</remarks>
        public override void Delete(int sysNo)
        {
            Context.Sql("Delete from LgFreightModuleDetails where SysNo=@SysNo")
                 .Parameter("SysNo", sysNo)
            .Execute();
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns></returns>
        /// <remarks>2015-11-22 杨浩 创建</remarks>
        public override void DeleteBySysNos(string sysNos)
        {
            Context.Sql("Delete from LgFreightModuleDetails where SysNo in(" + sysNos + ")")
           .Execute();
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="FreightModuleSysNo">运费模板编号</param>
        /// <returns></returns>
        /// <remarks>2015-08-21  王耀发 创建</remarks>
        public override void DeleteByFreightModuleSysNo(int FreightModuleSysNo)
        {
            Context.Sql("Delete from LgFreightModuleDetails where FreightModuleSysNo=@FreightModuleSysNo")
                 .Parameter("FreightModuleSysNo", FreightModuleSysNo)
            .Execute();
        }
        #endregion
    }
}
