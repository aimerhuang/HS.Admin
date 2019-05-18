using Hyt.DataAccess.Procurement;
using Hyt.Model.Procurement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.Procurement
{
    public class PmDeliveryDaoImpl : IPmDeliveryDao
    {
        public override int InnerGoodsDelivery(Model.Procurement.PmGoodsDelivery delivery)
        {
            return Context.Insert<Model.Procurement.PmGoodsDelivery>("PmGoodsDelivery", delivery).AutoMap(p => p.SysNo).ExecuteReturnLastId<int>("SysNo");
        }

        public override void UpdateGoodsDelivery(Model.Procurement.PmGoodsDelivery delivery)
        {
            Context.Update<Model.Procurement.PmGoodsDelivery>("PmGoodsDelivery", delivery).AutoMap(p => p.SysNo).Where(p => p.SysNo).Execute();
        }

        public override Model.Procurement.PmGoodsDelivery GetGoodsDelivery(int SysNo)
        {
            string sql = "select * from  PmGoodsDelivery where SysNo = '" + SysNo + "' ";
            return Context.Sql(sql).QuerySingle<Model.Procurement.PmGoodsDelivery>();
        }

        public override int InnerGoodsDeliveryItem(Model.Procurement.PmGoodsDeliveryItem deliveryItem)
        {
            return Context.Insert<Model.Procurement.PmGoodsDeliveryItem>("PmGoodsDeliveryItem", deliveryItem).AutoMap(p => p.SysNo).ExecuteReturnLastId<int>("SysNo");
        }

        public override void UpdateGoodsDeliveryItem(Model.Procurement.PmGoodsDeliveryItem deliveryItem)
        {
            Context.Update<Model.Procurement.PmGoodsDeliveryItem>("PmGoodsDeliveryItem", deliveryItem).AutoMap(p => p.SysNo).Where(p=>p.SysNo).Execute();
        }

        public override List<Model.Procurement.PmGoodsDeliveryItem> GetGoodsDeliveryItems(int PSysNo)
        {
            string sql = "select * from  PmGoodsDeliveryItem where gdi_PSysNo = '" + PSysNo + "' ";
            return Context.Sql(sql).QueryMany<Model.Procurement.PmGoodsDeliveryItem>();
        }


        public override Model.Procurement.CBPmGoodsDelivery GetCBPmGoodsDeliveryByPSysNo(int pSysNo)
        {
            CBPmGoodsDelivery mod = new CBPmGoodsDelivery();
            mod.ListItems = GetCBPmGoodsDeliveryItemByPSysNo(pSysNo);
            Dictionary<int, CBPmGoodsDeliveryItem> dic_Items = new Dictionary<int, CBPmGoodsDeliveryItem>();
            foreach (var item in mod.ListItems)
            {
                if (dic_Items.ContainsKey(item.gdi_GoodSysNo))
                {
                    dic_Items[item.gdi_GoodSysNo].gdi_SendQuity += item.gdi_SendQuity;
                }
                else
                {
                    dic_Items.Add(item.gdi_GoodSysNo, item);
                }
            }
            mod.ListItems.Clear();
            foreach (int key in dic_Items.Keys)
            {
                if (dic_Items[key].Poi_ProQuity > dic_Items[key].gdi_SendQuity)
                {
                    mod.ListItems.Add(dic_Items[key]);
                }
            }
            return mod;
        }
        public override List<CBPmGoodsDeliveryItem> GetCBPmGoodsDeliveryItemByPSysNo(int pSysNo)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(@" select PmPointsOrderItem.SysNo as gdi_PointItemSysNo, PmProcurementOrderItem.Poi_ProSysNo as gdi_GoodSysNo, PmProcurementOrderItem.Poi_ProName as Cb_ProName,
                            PdProduct.SalesMeasurementUnit as Cb_Unit ,'' as Cb_Spec,PmProcurementOrderItem.Poi_ProQuity,PmGoodsDeliveryItem.gdi_SendQuity");
            sb.AppendLine(@" from PmGoodsDeliveryItem right join PmPointsOrderItem on PmPointsOrderItem.SysNo=PmGoodsDeliveryItem.gdi_PointItemSysNo ");
            sb.AppendLine(@" right join PmProcurementOrderItem on PmPointsOrderItem.Poi_ProcurementItemSysNo=PmProcurementOrderItem.SysNo  ");
            sb.AppendLine(@" inner join PdProduct on PmProcurementOrderItem.Poi_ProSysNo=PdProduct.SysNo ");
            sb.AppendLine(@" where PmPointsOrderItem.Poi_PSysNo = '" + pSysNo + "' ");
            return Context.Sql(sb.ToString()).QueryMany<CBPmGoodsDeliveryItem>();
        }

        public override Model.Procurement.CBPmGoodsDelivery GetCBPmGoodsDeliveryBySysNo(int SysNo)
        {
            string sql = " select PmGoodsDelivery.*,SyUser.UserName as CurrentName from PmGoodsDelivery left join SyUser on PmGoodsDelivery.gd_DeliveryUserSys=SyUser.SysNo where PmGoodsDelivery.SysNo='" + SysNo + "'";
            CBPmGoodsDelivery mod = new CBPmGoodsDelivery();
            mod = Context.Sql(sql).QuerySingle<CBPmGoodsDelivery>();
            mod.ListItems = GetCBPmGoodsDeliveryItemBySysNo(SysNo);
            return mod;
        }
        public override List<CBPmGoodsDeliveryItem> GetCBPmGoodsDeliveryItemBySysNo(int SysNo)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" select PmPointsOrderItem.SysNo as gdi_PointItemSysNo,  PmProcurementOrderItem.Poi_ProSysNo as gdi_GoodSysNo,PmProcurementOrderItem.Poi_ProName as Cb_ProName,PdProduct.SalesMeasurementUnit as Cb_Unit ,'' as Cb_Spec,PmProcurementOrderItem.Poi_ProQuity,PmGoodsDeliveryItem.gdi_SendQuity");
            sb.AppendLine(" from PmGoodsDeliveryItem right join PmPointsOrderItem on PmPointsOrderItem.SysNo=PmGoodsDeliveryItem.gdi_PointItemSysNo ");
            sb.AppendLine(" right join PmProcurementOrderItem on PmPointsOrderItem.Poi_ProcurementItemSysNo=PmProcurementOrderItem.SysNo and PmProcurementOrderItem.Poi_ProSysNo=PmGoodsDeliveryItem.gdi_GoodSysNo ");
            sb.AppendLine(" inner join PdProduct on PmProcurementOrderItem.Poi_ProSysNo=PdProduct.SysNo ");
            sb.AppendLine(" where PmGoodsDeliveryItem.gdi_PSysNo='" + SysNo + "' ");
            return Context.Sql(sb.ToString()).QueryMany<CBPmGoodsDeliveryItem>();
        }

        public override List<PmGoodsDelivery> GetDeliveryListByPSysNo(string pSysNoList)
        {
            if (!string.IsNullOrEmpty(pSysNoList))
            {
                string sql = " select *  from PmGoodsDelivery where gd_PSysNo in (" + pSysNoList + ") ";
                return Context.Sql(sql).QueryMany<PmGoodsDelivery>();
            }
            else
            {
                return new List<PmGoodsDelivery>();
            }
           
        }

        public override List<CBPmGoodsDeliveryItem> GetPmGoodsDeliveryItemByOrderPSysNo(int pSysNo)
        {
            return GetCBPmGoodsDeliveryItemByPSysNo(pSysNo);
        }

        public override List<PmGoodsDeliveryItem> GetPmGoodsDeliveryItemByPSysNo(int SysNo)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" select * from  PmGoodsDeliveryItem where gdi_PSysNo = '" + SysNo + "' ");
            return Context.Sql(sb.ToString()).QueryMany<PmGoodsDeliveryItem>();
        }

        public override void GetPmGoodsDeliveryPager(ref Model.Pager<CBPmGoodsDelivery> pager)
        {
            #region sql条件
            string sqlWhere = @"";
           if(pager.PageFilter.gd_Type==2)
           {
               sqlWhere = " gd_PSysNo='0' ";
           }
            #endregion

            using (var context = Context.UseSharedConnection(true))
            {
                pager.Rows = context.Select<CBPmGoodsDelivery>(" PmGoodsDelivery.*,SyUser.UserName as CurrentName ")
                           .From(" PmGoodsDelivery left join SyUser on PmGoodsDelivery.gd_DeliveryUserSys=SyUser.SysNo ")
                           .Where(sqlWhere)
                           .Paging(pager.CurrentPage, pager.PageSize)
                           .OrderBy("SysNo desc")
                           .QueryMany();
                pager.TotalRows = context.Select<int>("count(1)")
                           .From(" PmGoodsDelivery left join SyUser on PmGoodsDelivery.gd_DeliveryUserSys=SyUser.SysNo ")
                           .Where(sqlWhere)
                           .QuerySingle();
            }
        }
    }
}
