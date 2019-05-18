using Hyt.DataAccess.FreightModule;
using Hyt.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.FreightModule
{
    /// <summary>
    /// 运费模板项价格
    /// </summary>
    /// <remarks>2015-11-21 杨浩 创建</remarks>
    public class FreightModuleItemPricesImpl : IFreightModuleItemPricesDao 
    {
        /// <summary>
        /// 添加运费模板项价格
        /// </summary>
        /// <param name="lgFreightModuleItemPrices">运费模板项价格实体类</param>
        /// <returns></returns>
        /// <remarks>2015-11-21 杨浩 创建</remarks>
        public override int AddFreightModuleItemPrices(LgFreightModuleItemPrices lgFreightModuleItemPrices)
        {
           return  Context.Insert<LgFreightModuleItemPrices>("LgFreightModuleItemPrices", lgFreightModuleItemPrices)
                .AutoMap(x=>x.SysNo)
                .ExecuteReturnLastId<int>("Sysno");
                
        }
        /// <summary>
        /// 更新运费模板项价格
        /// </summary>
        /// <param name="lgFreightModuleItemPrices">运费模板项价格实体类</param>
        /// <returns></returns>
        /// <remarks>2015-11-21 杨浩 创建</remarks>
        public override int UpdateFreightModuleItemPrices(LgFreightModuleItemPrices lgFreightModuleItemPrices)
        {
            return Context.Update<LgFreightModuleItemPrices>("LgFreightModuleItemPrices", lgFreightModuleItemPrices)
              .AutoMap(x => x.SysNo)
              .Where("sysno", lgFreightModuleItemPrices.SysNo)
              .Execute();
        }
        /// <summary>
        /// 删除运费模板项价格
        /// </summary>
        /// <param name="sysNos">运费模板项价格编号</param>
        /// <returns></returns>
        /// <remarks>2015-11-21 杨浩 创建</remarks>
        public override int DeleteFreightModuleItemPricesBySysNos(string sysNos)
        {
            string sql = @"delete LgFreightModuleItemPrices where SysNo in (" + sysNos + ")";
            return Context.Sql(sql).Execute();
        }

        /// <summary>
        /// 获取运费模板项价格
        /// </summary>
        /// <param name="freightModuleDetailsSysNo">运费模板详情编号</param>
        /// <param name="sysNo">运费模板项价格编号</param>
        /// <returns></returns>
        /// <remarks>2015-11-21 杨浩 创建</remarks>
        public override IList<LgFreightModuleItemPrices> GetFreightModuleItemPricesList(int freightModuleDetailsSysNo, int sysNo)
        {
            string sqlWhere = " 1=1 ";
            if(freightModuleDetailsSysNo>0)
               sqlWhere+=" and freightModuleDetailsSysNo=" + freightModuleDetailsSysNo;
            if (sysNo > 0)
                sqlWhere += " and sysNo=" + sysNo;

            return Context.Select<LgFreightModuleItemPrices>("*")
                .From("LgFreightModuleItemPrices")
                .Where(sqlWhere)
                .QueryMany();  
                 
        }
    }
}
