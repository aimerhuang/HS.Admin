using Hyt.DataAccess.Logistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.Logistics
{
    /// <summary>
    /// 运费模板保价操作类型
    /// </summary>
    /// <remarks>2015-11-27 杨云奕 添加</remarks>
    public class LgFreightValuationModuleDaoImpl : ILgFreightValuationModuleDao
    {
        /// <summary>
        /// 获取区域里的运费膜拜内容
        /// </summary>
        /// <param name="psysNo">模版id</param>
        /// <returns></returns>
        public override List<Model.Generated.LgFreightValuationModule> GetFreightModel(int psysNo)
        {
            string sql = " select * from LgFreightValuationModule where lgfvm_pid='" + psysNo + "' ";
            return Context.Sql(sql).QueryMany<Model.Generated.LgFreightValuationModule>();
        }
        /// <summary>
        /// 获取区域里的运费膜拜内容
        /// </summary>
        /// <param name="psysNo">模版id</param>
        /// <param name="AreaSysNo">区域值</param>
        /// <returns></returns>
        public override List<Model.Generated.LgFreightValuationModule> GetFreightModel(int psysNo, int AreaSysNo)
        {
            string sql = " select * from LgFreightValuationModule where lgfvm_pid='" + psysNo + "' and lgfvm_AreaSysNo like '%," + AreaSysNo.ToString().Substring(0, 2).PadRight(6, '0') + ",%' ";
            return Context.Sql(sql).QueryMany<Model.Generated.LgFreightValuationModule>();
        }

        /// <summary>
        /// 通过 区域，模版id，报价金额获取实体
        /// </summary>
        /// <param name="psysNo">模版id</param>
        /// <param name="AreaSysNo">区域值</param>
        /// <param name="decimalValue">报价金额</param>
        /// <returns></returns>
        public override Model.Generated.LgFreightValuationModule GetFreightModel(int psysNo, int AreaSysNo, decimal decimalValue)
        {
            string sql = " select * from LgFreightValuationModule where lgfvm_pid='" + psysNo + "' and lgfvm_AreaSysNo like '%," + AreaSysNo.ToString().Substring(0, 2).PadRight(6, '0') + ",%'  ";
            sql += " and  lgfvm_MinValua<=" + decimalValue + " and lgfvm_MaxValua>=" + decimalValue + " ";
            return Context.Sql(sql).QuerySingle<Model.Generated.LgFreightValuationModule>();
        }
        /// <summary>
        /// 新增膜拜信息
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public override int InsertFreightModelData(Model.Generated.LgFreightValuationModule mod)
        {
            return Context.Insert<Model.Generated.LgFreightValuationModule>("LgFreightValuationModule", mod).AutoMap(p => p.SysNo).ExecuteReturnLastId<int>("SysNo");
        }
        /// <summary>
        /// 修改报价设置膜拜信息
        /// </summary>
        /// <param name="mod"></param>
        public override void UpdateFreightModelData(Model.Generated.LgFreightValuationModule mod)
        {
            Context.Update<Model.Generated.LgFreightValuationModule>("LgFreightValuationModule", mod).AutoMap(p => p.SysNo).Where(p=>p.SysNo).Execute();
        }

        /// <summary>
        /// 删除报价定义
        /// </summary>
        /// <param name="delIdList"></param>
        public override void DeleteFreightModelData(string delIdList)
        {
            string sql = " delete from  LgFreightValuationModule where sysNo in (" + delIdList + ") ";
            Context.Sql(sql).Execute();
        }
    }
}
