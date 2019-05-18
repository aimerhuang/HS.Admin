using Hyt.DataAccess.Base;
using Hyt.Model.Procurement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Procurement
{
    public abstract class IPmDeliveryDao : DaoBase<IPmDeliveryDao>
    {
        public abstract int InnerGoodsDelivery(PmGoodsDelivery delivery);
        public abstract void UpdateGoodsDelivery(PmGoodsDelivery delivery);

        public abstract PmGoodsDelivery GetGoodsDelivery(int SysNo);


        public abstract int InnerGoodsDeliveryItem(PmGoodsDeliveryItem deliveryItem);
        public abstract void UpdateGoodsDeliveryItem(PmGoodsDeliveryItem deliveryItem);

        public abstract List<PmGoodsDeliveryItem> GetGoodsDeliveryItems(int PSysNo);
        public abstract CBPmGoodsDelivery GetCBPmGoodsDeliveryByPSysNo(int pSysNo);

        public abstract List<CBPmGoodsDeliveryItem> GetCBPmGoodsDeliveryItemByPSysNo(int pSysNo);

        public abstract List<CBPmGoodsDeliveryItem> GetCBPmGoodsDeliveryItemBySysNo(int SysNo);

        public abstract Model.Procurement.CBPmGoodsDelivery GetCBPmGoodsDeliveryBySysNo(int SysNo);

        public abstract List<PmGoodsDelivery> GetDeliveryListByPSysNo(string pSysNoList);

        public abstract List<CBPmGoodsDeliveryItem> GetPmGoodsDeliveryItemByOrderPSysNo(int pSysNo);

        public abstract List<PmGoodsDeliveryItem> GetPmGoodsDeliveryItemByPSysNo(int SysNo);

        public abstract void GetPmGoodsDeliveryPager(ref Model.Pager<CBPmGoodsDelivery> pager);
    }
}
