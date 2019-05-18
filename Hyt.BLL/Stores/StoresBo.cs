using Hyt.Infrastructure.Caching;
using Hyt.Model;
using Hyt.Model.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.Stores
{
    /// <summary>
    /// 店铺(经销商)
    /// </summary>
    /// <remarks>2015-12-14 杨浩 创建</remarks>
    public class StoresBo : BOBase<StoresBo>
    {
         private static AttachmentConfig attachmentConfig = Hyt.BLL.Config.Config.Instance.GetAttachmentConfig();
        /// <summary>
        /// 获取全部店铺
        /// </summary>
        /// <returns>Stores</returns>
         public IList<Store> GetAllStores()
         {
             var items = (List<Store>)CacheManager.Get(CacheKeys.Items.StoreAll, "", () =>
             {
                 var stores = Hyt.DataAccess.Stores.IStoresDao.Instance.GetAllStores();

                 //序列化扩展对象
                 List<Store> temps = new List<Store>();
                 foreach (var item in stores)
                 {
                     try
                     {
                         item.ExtensionsObj = Hyt.Util.Serialization.JsonUtil.ToObject<StoreExtensions>(item.Extensions);
                     }
                     catch {item.ExtensionsObj = new StoreExtensions();}
                     temps.Add(item);
                 }

                 return temps;
             });

             return items;
         }

        /// <summary>
        /// 获取指定店铺
        /// </summary>
        /// <param name="storeId">Store identifier</param>
        /// <returns>Store</returns>
        public Store GetStoreById(int storeId)
        {
            var item = (Store)CacheManager.Get(CacheKeys.Items.StoresInfo_, storeId.ToString(), () =>
            {
                var store = Hyt.DataAccess.Stores.IStoresDao.Instance.GetStoreById(storeId);
                //序列化扩展对象
                try
                {
                    store.ExtensionsObj = Hyt.Util.Serialization.JsonUtil.ToObject<StoreExtensions>(store.Extensions);
                }
                catch{store.ExtensionsObj = new StoreExtensions();}

                return store;
          
            });

            return item;
        }

        /// <summary>
        /// 获取店铺logo
        /// </summary>
        /// <returns></returns>
        /// <remarks>2015-12-22 杨浩 创建</remarks>
        public string GetStoreLogoImagePath(string pathFormat)
        {
            if (pathFormat != "")
                return attachmentConfig.FileServer + pathFormat;
            else
                return "";
        }


        /// <summary>
        /// 根据仓库编号，获取对应经销商实体
        /// </summary>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        public Store GetStoreByWarehouseId(int warehouseSysNo)
        {
            int storeId = Hyt.DataAccess.Stores.IStoresDao.Instance.GetStoreIdByWarehouseId(warehouseSysNo);
            var item = (Store)CacheManager.Get(CacheKeys.Items.StoresInfo_, storeId.ToString(), () =>
            {
                var store = Hyt.DataAccess.Stores.IStoresDao.Instance.GetStoreById(storeId);
                //序列化扩展对象
                try
                {
                    store.ExtensionsObj = Hyt.Util.Serialization.JsonUtil.ToObject<StoreExtensions>(store.Extensions);
                }
                catch { store.ExtensionsObj = new StoreExtensions(); }

                return store;

            });

            return item;
        }

        /// <summary>
        /// 获取经销商绑定支付方式信息
        /// </summary>
        /// <param name="dealerSysNo">经销商系统编号</param>
        public DsDealerPayType GetStorePayType(int dealerSysNo)
        {
            return DataAccess.Stores.IStoresDao.Instance.GetStorePayType(dealerSysNo);
        }
    }
}