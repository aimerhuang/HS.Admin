using Hyt.DataAccess.Pos;
using Hyt.Model.Transfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.Pos
{
    public class DsPosManageDaoImpl : IDsPosManageDao
    {
        /// <summary>
        /// 检查数据表是否存在相应的字段，如果存在则不增加，不存在则增加
        /// </summary>
        /// <param name="keyList"></param>
        /// <returns></returns>
        /// <remarks>2016-11-14 杨云奕 添加</remarks>
        public void CheckKeyExcel(List<DBKey> keyList)
        {
            string checkWhere = "";
            foreach (DBKey key in keyList)
            {
                if (!string.IsNullOrEmpty(checkWhere))
                {
                    checkWhere += " or ";
                }
                checkWhere += "  name='" + key.KeyName + "' ";
            }
            string sql = "select   *   from   syscolumns   where   id=object_id('DsSpecialPrice')   and (  " + checkWhere + " )";
            List<SelectKeyData> selectList = Context.Sql(sql).QueryMany<SelectKeyData>();

            foreach (DBKey key in keyList)
            {
                SelectKeyData tempKeyData = selectList.Find(p => p.name == key.KeyName);
                if (tempKeyData == null)
                {
                    sql = " alter table DsSpecialPrice add " + key.KeyName + " " + key.Type + " null ";
                    Context.Sql(sql).Execute();
                }

            }


        }
        /// <summary>
        /// 添加pos机管理
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        /// <remarks>
        /// 2016-03-03 杨云奕 修改  数据库商品档案表添加 DsSysNo 字段，引起关联表自动冲突报错
        /// </remarks>
        public override int Insert(Model.Pos.DsPosManage mod)
        {
            return Context.Insert<Model.Pos.DsPosManage>("DsPosManage", mod).AutoMap(p => p.SysNo).ExecuteReturnLastId<int>("SysNo");
        }
        /// <summary>
        /// 更新pos机管理数据
        /// </summary>
        /// <param name="mod"></param>
        public override void Update(Model.Pos.DsPosManage mod)
        {
            Context.Update<Model.Pos.DsPosManage>("DsPosManage", mod).AutoMap(p => p.SysNo).Where(p => p.SysNo).Execute();
        }
        /// <summary>
        /// 删除pos机数据
        /// </summary>
        /// <param name="sysNo"></param>
        public override void Delete(int sysNo)
        {
            string sql = "delete from DsPosManage where sysNo=" + sysNo;
            Context.Sql(sql).Execute();
        }

        public override Model.Pos.DsPosManage GetEntity(int sysNo)
        {
            string sql = "select * from DsPosManage where sysNo=" + sysNo;
            return Context.Sql(sql).QuerySingle<Hyt.Model.Pos.DsPosManage>();
        }

        public override List<Model.Pos.CBDsPosManage> GetEntityListByDsSysNo(int dsSysNo)
        {
            string sql = "select DsPosManage.*,DsDealer.DealerName from DsPosManage inner join DsDealer on DsPosManage.pos_DsSysNo=DsDealer.SysNo where pos_DsSysNo='" + dsSysNo + "'  or " + dsSysNo + " = 0 ";
            return Context.Sql(sql).QueryMany<Model.Pos.CBDsPosManage>();
        }

        public override List<Model.Pos.DsPosManage> GetEntityListByPosKey(string Key)
        {
            string sql = "select * from DsPosManage where pos_KeyData='" + Key + "' ";
            return Context.Sql(sql).QueryMany<Model.Pos.DsPosManage>();
        }

        public override Model.CBPdProductDetail GetDealerProductByBarCode(string code, int WarehousSysNo)
        {
            List<DBKey> keyList = new List<DBKey>();
            keyList.Add(new DBKey() { KeyName = "WholesalePrice", Type = "decimal(18, 2)" });
            CheckKeyExcel(keyList);

            //string sql = "select PdProduct.*, price.Price as BasicPrice, price1.Price as SalesPrice,DsSpecialPrice.Price as spSalesPrice,DsSpecialPrice.DealerSysNo from DsSpecialPrice inner join PdProduct on PdProduct.SysNo=DsSpecialPrice.ProductSysNo ";
            //sql += " left join  (select * from PdPrice where PriceSource =0 and (0 =0 or sourcesysno=0)) price  on price.productsysno = PdProduct.sysno ";
            //sql += " left join  (select * from PdPrice where PriceSource =10 and (0 =0 or sourcesysno=0)) price1  on price1.productsysno = PdProduct.sysno ";
            //sql += " where DsSpecialPrice.DealerSysNo=" + dealerSysNo + " and DsSpecialPrice.Status=1 and PdProduct.Barcode='" + code + "'";

            string sql = @"select (case isnull(PdProductStock.Barcode,'') when '' then PdProduct.Barcode else PdProductStock.Barcode end ) as Barcode, PdProduct.ProductName, PdProduct.SysNo, PdProduct.CreatedDate, 
                            (case isnull(PdProductStock.LastUpdateDate,'') when '' then PdProduct.LastUpdateDate else PdProductStock.LastUpdateDate end ) as LastUpdateDate,  PdProduct.Stamp, price.Price as BasicPrice, 
                            PdProduct.Status,  (case isnull(price2.ShopPrice,0) when 0 then  case isnull(price1.Price,0) when 0 then price.Price else price1.Price end  else price2.ShopPrice end  ) as SalesPrice,0 as spSalesPrice,
                            PdProduct.SysNo as ProSysNo,PdProductStock.StockQuantity as WareNum  ";
            sql += " from  PdProduct  left join  (select * from PdPrice where PriceSource =0 ) price  on price.productsysno = PdProduct.sysno   ";
            sql += "   left join  (select * from PdPrice where PriceSource =20  and sourceSysNo=0 ) price1  on price1.productsysno = PdProduct.sysno   ";
            sql += "   left join  (select DsSpecialPrice.* from DsSpecialPrice inner join DsDealerWharehouse on DsDealerWharehouse.DealerSysNo=DsSpecialPrice.DealerSysNo where DsDealerWharehouse.WarehouseSysNo =" + WarehousSysNo + "   ) price2  on price2.productsysno = PdProduct.sysno   ";
            sql += "   left join PdProductStock on PdProductStock.WarehouseSysNo=" + WarehousSysNo + "  ";
            sql += "   and PdProductStock.PdProductSysNo=PdProduct.SysNo   ";

            sql += " where PdProduct.Barcode='" + code + "' ";

            return Context.Sql(sql).QuerySingle<Model.CBPdProductDetail>();
        }

        public override List<Model.Pos.DsPosOrderItem> GetPosOrderItemBySerialNumber(string numberNo)
        {
            string sql = "select DsPosOrderItem.* from DsPosOrder inner join DsPosOrderItem on DsPosOrder.sysNo=DsPosOrderItem.pSysNo where SerialNumber='" + numberNo + "' ";
            return Context.Sql(sql).QueryMany<Model.Pos.DsPosOrderItem>();
        }

        /// <summary>
        /// 获取商品档案数据
        /// </summary>
        /// <param name="detailList"></param>
        /// <returns></returns>
        public override List<Model.CBPdProductDetail> GetPosProductDetailList(List<Model.CBPdProductDetail> detailList, int WarehousSysNo, string dsProLink = "left")
        {
            List<DBKey> keyList = new List<DBKey>();
            keyList.Add(new DBKey() { KeyName = "WholesalePrice", Type = "decimal(18, 2)" });
            CheckKeyExcel(keyList);


            List<Model.CBPdProductDetail> AllProductDetailList = new List<Model.CBPdProductDetail>();
            //string sql = "select (case isnull(PdProductStock.Barcode,'') when '' then PdProduct.Barcode else PdProductStock.Barcode end ) as Barcode, PdProduct.ProductName, PdProduct.SysNo, PdProduct.CreatedDate, (case isnull(PdProductStock.LastUpdateDate,'') when '' then PdProduct.LastUpdateDate else PdProductStock.LastUpdateDate end ) as LastUpdateDate,  PdProduct.Stamp, price.Price as BasicPrice, price1.Price as SalesPrice,DsSpecialPrice.Price as spSalesPrice,DsSpecialPrice.DealerSysNo,PdProduct.SysNo as ProSysNo,PdProductStock.StockQuantity as WareNum ";
            //sql += " from DsSpecialPrice right join PdProduct on PdProduct.SysNo=DsSpecialPrice.ProductSysNo ";
            //sql += " left join  (select * from PdPrice where PriceSource =0 ) price  on price.productsysno = PdProduct.sysno ";
            //sql += " left join  (select * from PdPrice where PriceSource =10  and sourceSysNo=1 ) price1  on price1.productsysno = PdProduct.sysno ";
            //sql += " left join DsDealerWharehouse on DsDealerWharehouse.DealerSysNo=DsSpecialPrice.DealerSysNo  ";
            //sql += " left join PdProductStock on PdProductStock.WarehouseSysNo=DsDealerWharehouse.WarehouseSysNo and PdProductStock.PdProductSysNo=DsSpecialPrice.ProductSysNo  ";
            //sql += " where (DsSpecialPrice.DealerSysNo=" + dealerSysNo + " or DsSpecialPrice.DealerSysNo is null )";
            string sql = @"select (case isnull(PdProductStock.Barcode,'') when '' then PdProduct.Barcode else PdProductStock.Barcode end ) as Barcode, PdProduct.ProductName, PdProduct.SysNo, PdProduct.CreatedDate, 
                            (case isnull(PdProductStock.LastUpdateDate,'') when '' then PdProduct.LastUpdateDate else PdProductStock.LastUpdateDate end ) as LastUpdateDate,  PdProduct.Stamp, price.Price as BasicPrice, 
                            PdProduct.Status,  (case isnull(price2.ShopPrice,0) when 0 then  case isnull(price1.Price,0) when 0 then price.Price else price1.Price end  else price2.ShopPrice end  ) as SalesPrice,0 as spSalesPrice,
                            PdProduct.SysNo as ProSysNo,PdProductStock.StockQuantity as WareNum , (case isnull(price2.WholesalePrice,0) when 0 then  case isnull(PdProduct.TradePrice,0) when 0 then price.Price else PdProduct.TradePrice end  else price2.WholesalePrice end  ) as spWholesalePrice ";
            sql += " from  PdProduct  left join  (select * from PdPrice where PriceSource =0 ) price  on price.productsysno = PdProduct.sysno   ";
            sql += "   left join  (select * from PdPrice where PriceSource =20  and sourceSysNo=0 ) price1  on price1.productsysno = PdProduct.sysno   ";
            sql += "  left join  (select DsSpecialPrice.* from DsSpecialPrice inner join DsDealerWharehouse on DsDealerWharehouse.DealerSysNo=DsSpecialPrice.DealerSysNo where DsDealerWharehouse.WarehouseSysNo =" + WarehousSysNo + "   ) price2  on price2.productsysno = PdProduct.sysno   ";
            sql += "   " + dsProLink + "   join PdProductStock on PdProductStock.WarehouseSysNo=" + WarehousSysNo + "  ";
            sql += "    and PdProductStock.PdProductSysNo=PdProduct.SysNo   ";
            string where1 = "";
            string where2 = "";
            foreach (Hyt.Model.CBPdProductDetail mod in detailList)
            {
                if (!string.IsNullOrEmpty(where1))
                {
                    where1 += " and ";
                    where2 += " or ";
                }
                where1 += "( PdProduct.SysNo != '" + mod.ProSysNo + "' )";
                where2 += "( PdProduct.SysNo = '" + mod.ProSysNo + "' and ( PdProduct.LastUpdateDate>'" + mod.LastUpdateDate.AddHours(-1) + "' or PdProductStock.LastUpdateDate>'" + mod.LastUpdateDate.AddHours(-1) + "' ) )";
            }
            //System.IO.File.WriteAllText("E:\\WEB\\admin.yoyo2o.com\\test.txt", sql + (string.IsNullOrEmpty(where1) ? "" : " where  (" + where1 + ")  "));
            ///添加商品档案数据
            List<Model.CBPdProductDetail> AddProductDetailList = Context.Sql(sql + (string.IsNullOrEmpty(where1) ? "" : " where  (" + where1 + ")  ")).QueryMany<Hyt.Model.CBPdProductDetail>();
            foreach (Hyt.Model.CBPdProductDetail mod in AddProductDetailList)
            {
                mod.SysNo = 0;
                AllProductDetailList.Add(mod);
            }
            // AllProductDetailList.AddRange(AddProductDetailList);
            if (!string.IsNullOrEmpty(where2))
            {
                //System.IO.File.WriteAllText("E:\\WEB\\admin.yoyo2o.com\\test2.txt", sql + (string.IsNullOrEmpty(where2) ? "" : " where  (" + where2 + ")  "));
                ///获取数据更新
                List<Model.CBPdProductDetail> UpdateProductDetailList = Context.Sql(sql + (string.IsNullOrEmpty(where2) ? "" : " where  (" + where2 + ")  ")).QueryMany<Hyt.Model.CBPdProductDetail>();
                foreach (Hyt.Model.CBPdProductDetail mod in UpdateProductDetailList)
                {
                    Hyt.Model.CBPdProductDetail tempDetail=detailList.Find(p => p.ProSysNo == mod.SysNo);
                    if (tempDetail != null)
                    {
                        mod.SysNo = tempDetail.SysNo;
                    }
                    else
                    {
                        mod.SysNo = 0;
                    }
                    AllProductDetailList.Add(mod);
                }
            }
            return AllProductDetailList;
        }
        /// <summary>
        /// 通过机器终端码获取收银机信息
        /// </summary>
        /// <param name="Termid"></param>
        /// <returns></returns>
        public override Model.Pos.DsPosManage GetEntityByTermidNum(string Termid)
        {
            string sql = " select * from DsPosManage where Pos_TLTermid = '" + Termid + "' ";
            return Context.Sql(sql).QuerySingle<Hyt.Model.Pos.DsPosManage>();
        }
    }
}
