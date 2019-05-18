using Hyt.DataAccess.Stores;
using Hyt.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.Stores
{
    /// <summary>
    /// 经销商产品特殊价格表
    /// </summary>
    /// <remarks>2015-12-7 杨浩 创建</remarks>
    public class DsSpecialPriceDaoImpl : IDsSpecialPriceDao 
    {
        /// <summary>
        /// 经销商产品特殊价表字段
        /// </summary>
        private const string DSSPECIALPRICE = "[SysNo],[DealerSysNo],[ProductSysNo],[Price],[Status],[ShopPrice],[CreatedBy],[CreatedDate],[LastUpdateBy],[LastUpdateDate]";
        /// <summary>
        /// 获得全部经销商产品价格列表
        /// </summary>
        /// <returns></returns>
        public override IList<DsSpecialPrice> GetAllSpecialPrices()
        {
            return Context.Sql(string.Format("select {0} from DsSpecialPrice",DSSPECIALPRICE)).QueryMany<DsSpecialPrice>();
        }
        /// <summary>
        /// 获得指定经销商的产品价格
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        public override IList<DsSpecialPrice> GetSpecialPricesBySysNo(int sysNo)
        {
            return Context.Sql(string.Format("SELECT {0} FROM DsSpecialPrice WHERE DealerSysNo={1}", DSSPECIALPRICE,sysNo)).QueryMany<DsSpecialPrice>();
        }

        /// <summary>
        /// 获得指定经销商的产品价格
        /// </summary>
        /// <param name="sysNo">经销商编号</param>
        /// <param name="productSysNo">产品编号</param>
        /// <returns></returns>
        /// <remarks>2016-1-3 杨浩 创建</remarks>
        public override DsSpecialPrice GetSpecialPricesBySysNo(int sysNo, int productSysNo)
        {
            return Context
                .Sql(string.Format("select top 1 [Price] from DsSpecialPrice where ProductSysNo={0} and DealerSysNo={1} and Status={2}", productSysNo, sysNo,(int)Hyt.Model.WorkflowStatus.DistributionStatus.经销商特殊价格状态.启用))
                .QuerySingle<DsSpecialPrice>();
        }
    }
}
