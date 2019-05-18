using Hyt.DataAccess.Procurement;
using Hyt.Model.Procurement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.Procurement
{
    /// <summary>
    /// 采购库存操作
    /// </summary>
    public class PmWareGoodsDaoImpl : IPmWareGoodsDao
    {
        /// <summary>
        /// 插入采购库存数据
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public override int InsertPmWareGoodsDao(Model.Procurement.PmWareGoods mod)
        {
            return Context.Insert("PmWareGoods", mod).AutoMap(p => p.SysNo).ExecuteReturnLastId<int>("SysNo");
        }
        /// <summary>
        /// 修改采购库存数据
        /// </summary>
        /// <param name="mod"></param>
        public override void UpdatePmWareGoodsDao(Model.Procurement.PmWareGoods mod)
        {
            Context.Update("PmWareGoods", mod).AutoMap(p => p.SysNo).Where(p => p.SysNo).Execute();
        }

        /// <summary>
        /// 获取采购分页库存
        /// </summary>
        /// <param name="pager"></param>
        public override void GetPmWareGoodsListByPager(ref Model.Pager<Model.Procurement.CBPmWareGoods> pager)
        {
            #region sql条件
            string sqlWhere = @"";
            if(pager.PageFilter.Type== 0 )
            {
                sqlWhere += "( WareNum <= 0   ) ";
            }
            else if (pager.PageFilter.Type > 0)
            {
                sqlWhere += "( WareNum > 0   ) ";
            }
            #endregion

            using (var context = Context.UseSharedConnection(true))
            {
                pager.Rows = context.Select<CBPmWareGoods>(" PmWareGoods.*, PdProduct.ProductName as ProName,'' as Spec ")
                           .From(" PmWareGoods  inner join  PdProduct on  PmWareGoods.ProSysNo=PdProduct.SysNo ")
                           .Where(sqlWhere)
                           .Paging(pager.CurrentPage, pager.PageSize)
                           .OrderBy("SysNo desc")
                           .QueryMany();
                pager.TotalRows = context.Select<int>("count(1)")
                           .From(" PmWareGoods  inner join  PdProduct on  PmWareGoods.ProSysNo=PdProduct.SysNo ")
                           .Where(sqlWhere)
                           .QuerySingle();
            }
        }

        /// <summary>
        /// 获取商品数据
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        public override PmWareGoods GetPmWareGoodsByProSysNo(int sysNo)
        {
            string sql = " select * from PmWareGoods where ProSysNo = '" + sysNo + "' ";
            return Context.Sql(sql).QuerySingle<PmWareGoods>();
        }
        /// <summary>
        /// 更新获取数据
        /// </summary>
        /// <param name="sysNo"></param>
        /// <param name="wareNum"></param>
        /// <param name="stayInWare"></param>
        /// <param name="freeze"></param>
        public override void SetPmWareGoodsUpdateDataByProSysNo(int sysNo, int wareNum, int stayInWare, int freeze)
        {
            string sql = "update PmWareGoods set StayInWare=StayInWare+(" + stayInWare + ") ,WareNum=WareNum+(" + wareNum + "),Freeze=Freeze+(" + freeze + ") where ProSysNo='" + sysNo + "'";
            Context.Sql(sql).Execute();
        }
        /// <summary>
        /// 添加采购库存商品记录
        /// </summary>
        /// <param name="history"></param>
        public override int InnerPmWareGoodsHistory(PmWareGoodsHistory history)
        {
            return Context.Insert("PmWareGoodsHistory", history).AutoMap(p => p.SysNo).ExecuteReturnLastId<int>("SysNo");
        }
        /// <summary>
        /// 获取采购入库历史记录
        /// </summary>
        /// <param name="ProSysNo"></param>
        /// <returns></returns>
        public override List<PmWareGoodsHistory> GetPmWareGoodsHistoryList(int ProSysNo)
        {
            string sql = " select * from PmWareGoodsHistory where PSysNo = '" + ProSysNo + "' ";
            return Context.Sql(sql).QueryMany<PmWareGoodsHistory>();
        }

        public override List<CBPmWareGoods> GetPmWareGoodsListBySysNos(string idList)
        {
            string sql = " select PmWareGoods.*, PdProduct.ProductName as ProName ,PdProduct.SalesMeasurementUnit as Unit ,'' as Spec from ";
            sql += " PmWareGoods  inner join  PdProduct on  PmWareGoods.ProSysNo=PdProduct.SysNo ";
            sql += " where PdProduct.SysNo in(" + idList + ") ";
            return Context.Sql(sql).QueryMany<CBPmWareGoods>();
        }
    }
}
