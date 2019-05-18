using Hyt.DataAccess.Stores;
using Hyt.Model;
using Hyt.Model.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.Stores
{
    /// <summary>
    /// 店铺(经销商)
    /// </summary>
    /// <remarks>2015-12-14 杨浩 创建</remarks>
    public class StoresDaoImpl : IStoresDao
    {
        private const string STORE = "[SysNo],[LevelSysNo],[DealerName],[EnterpriseID],[AreaSysNo],[StreetAddress],[Contact],[PhoneNumber],[MobilePhoneNumber],[EmailAddress],[Type],[ErpCode],[ErpName],[Status],[CreatedBy],[CreatedDate],[LastUpdateBy],[LastUpdateDate],[UserSysNo],[ImageUrl],[AppID],[AppSecret],[WeiXinNum],[DomainName],[Token] ";
        /// <summary>
        /// 获取全部店铺
        /// </summary>
        /// <returns>Stores</returns>
        public override IList<Store> GetAllStores()
        {
            return Context.Sql(string.Format("select * from DsDealer order by SysNo asc")).QueryMany<Store>();
        }

        /// <summary>
        /// 获取指定店铺
        /// </summary>
        /// <param name="storeId">Store identifier</param>
        /// <returns>Store</returns>
        public override Store GetStoreById(int storeId)
        {
            return Context.Sql(string.Format("select * from DsDealer where SysNo=@0", STORE), storeId).QuerySingle<Store>();
        }
        /// <summary>
        /// 获取指定店铺代理商
        /// </summary>
        /// <param name="storeId">Store identifier</param>
        /// <returns>Store</returns>
        public override SyUser GetDiLiByCreatId(int storeId)
        {
            return Context.Sql(string.Format("select s.* from SyUser s left join DsDealer ds on ds.CreatedBy=s.SysNo where ds.SysNo=@0", STORE), storeId).QuerySingle<SyUser>();
        }
        public override int GetStoreIdByWarehouseId(int warehouseSysNo)
        {
            return Context.Sql("select DealerSysNo from DsDealerWharehouse where WarehouseSysNo=@0", warehouseSysNo).QuerySingle<int>();
        }

        public override Model.DsDealerPayType GetStorePayType(int dealerSysNo)
        {
            return Context.Sql("select * from DsDealerPayType where DealerSysNo=@0", dealerSysNo).QuerySingle<Model.DsDealerPayType>();
        }
    }
}