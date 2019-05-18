using Hyt.DataAccess.Base;
using Hyt.DataAccess.FreightModule;
using Hyt.Model;
using Hyt.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.FreightModule
{
    /// <summary>
    /// 运费模板
    /// </summary>
    /// <remarks>2015-9-9 杨浩 创建</remarks>
    public class FreightModuleDaoImpl : IFreightModuleDao
    {
        public const string freightMouleFiled = "[SysNo],[ModuleCode],[ModuleName],[ProductAddress],[DeliveryTime] ,[IsPost],[ValuationStyle],[Express],[EMS],[SurfaceMail],[Status],[AuditorSysNo],[AuditDate],[CreatedBy] ,[CreatedDate],[LastUpdateBy],[LastUpdateDate]";
        /// <summary>
        /// 获取仓库地址所对应的运费模板
        /// </summary>
        /// <param name="productAddress">仓库地址编号</param>
        /// <returns>运费模板</returns>
        /// <remarks>2015-9-9 杨浩 创建</remarks>
        public override IList<LgFreightModule> GetFreightModuleByProductAddress(int addressSysNo)
        {
            var strSql = string.Format("SELECT {0} FROM  LgFreightModule WHERE (ProductAddress={1} and Status=20) or  ProductAddress=0  ", freightMouleFiled, addressSysNo);
            return Context.Sql(strSql).QueryMany<LgFreightModule>();
        }
        /// <summary>
        /// 获取运费
        /// </summary>
        /// <param name="addressSysNo">收货地址系统编号</param>
        /// <param name="freightModuleSysNo">运费模板系统编号</param>
        /// <param name="productSysNoAndNumber">商品系统编号和购买数量组合（商品系统编号_购买数量,商品系统编号_购买数量...）</param>
        /// <returns></returns>
        /// <remarks>2015-9-9 杨浩 创建</remarks>
        public override IList<FareTotal> GetFareTotal(int addressSysNo, int freightModuleSysNo, string productSysNoAndNumber)
        {
            return Context.StoredProcedure("pro_GetProductListFreight")
                .Parameter("DArea", addressSysNo)
                .Parameter("FreightModuleSysNo", freightModuleSysNo)
                .Parameter("ProductSysNoNumList", productSysNoAndNumber)
                .Parameter("IsAdmin", 1) //后台调用
                .QueryMany<FareTotal>();
        }
    }
}
