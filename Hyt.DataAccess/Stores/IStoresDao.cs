using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Stores
{
    /// <summary>
    /// 店铺(经销商)
    /// </summary>
    /// <remarks>2015-12-14 杨浩 创建</remarks>
    public abstract class IStoresDao : DaoBase<IStoresDao>
    {
        /// <summary>
        /// Gets all stores
        /// </summary>
        /// <returns>Stores</returns>
        public abstract IList<Store> GetAllStores();

        /// <summary>
        /// Gets a store 
        /// </summary>
        /// <param name="storeId">Store identifier</param>
        /// <returns>Store</returns>
        public abstract Store GetStoreById(int storeId);
        public abstract SyUser GetDiLiByCreatId(int storeId);

        public abstract int GetStoreIdByWarehouseId(int warehouseSysNo);

        public abstract DsDealerPayType GetStorePayType(int dealerSysNo);
    }
}