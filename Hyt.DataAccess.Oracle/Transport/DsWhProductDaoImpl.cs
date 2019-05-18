using Hyt.DataAccess.Transport;
using Hyt.Model.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.Transport
{
    /// <summary>
    /// 转运系统商品档案
    /// </summary>
    /// <remarks>
    /// 2016-5-17 杨云奕 添加
    /// </remarks>
    public class DsWhProductDaoImpl : IDsWhProductDao
    {
        /// <summary>
        /// 添加商品档案
        /// </summary>
        /// <param name="Mod"></param>
        /// <returns></returns>
        public override int InsertMod(Model.Transport.DsWhProduct Mod)
        {
            return Context.Insert("DsWhProduct", Mod).AutoMap(p => p.SysNo).ExecuteReturnLastId<int>("SysNo");
        }
        /// <summary>
        /// 更新商品档案
        /// </summary>
        /// <param name="Mod"></param>
        public override void UpdateMod(Model.Transport.DsWhProduct Mod)
        {
             Context.Update("DsWhProduct", Mod).AutoMap(p => p.SysNo).Where(p=>p.SysNo).Execute();
        }
        /// <summary>
        /// 删除商品档案数据
        /// </summary>
        /// <param name="SysNo"></param>
        public override void DeleteBySysNo(int SysNo)
        {
            string sql = " delete from  DsWhProduct where SysNo = '" + SysNo + "' ";
            Context.Sql(sql).Execute();
        }
        /// <summary>
        /// 通过自动编号获取商品档案
        /// </summary>
        /// <param name="SysNo">自动编号</param>
        /// <returns></returns>
        public override Model.Transport.DsWhProduct GetProductModBySysNo(int SysNo)
        {
            string sql = " select * from DsWhProduct where SysNo = '" + SysNo + "' ";
            return Context.Sql(sql).QuerySingle<DsWhProduct>();
        }
        /// <summary>
        /// 通过商品编号获取商品档案数据
        /// </summary>
        /// <param name="GoodsCode"></param>
        /// <returns></returns>
        public override Model.Transport.DsWhProduct GetProductModByGoodsCode(string GoodsCode)
        {
            string sql = " select * from DsWhProduct where ProCode = '" + GoodsCode + "' ";
            return Context.Sql(sql).QuerySingle<DsWhProduct>();
        }
        /// <summary>
        /// 通过自动编号集合获取商品档案集合
        /// </summary>
        /// <param name="SysNos"></param>
        /// <returns></returns>
        public override List<Model.Transport.DsWhProduct> GetProductModBySysNos(List<int> SysNos)
        {
            string sql = " select * from DsWhProduct where SysNo in (" + string.Join(",", SysNos) + ") ";
            return Context.Sql(sql).QueryMany<DsWhProduct>();
        }
        /// <summary>
        /// 通过货品编号获取商品档案集合
        /// </summary>
        /// <param name="GoodsCodes"></param>
        /// <returns></returns>
        public override List<Model.Transport.DsWhProduct> GetProductModByGoodsCodes(List<string> GoodsCodes)
        {
            string sql = " select * from DsWhProduct where ProCode in ('" + string.Join("','", GoodsCodes) + "') ";
            return Context.Sql(sql).QueryMany<DsWhProduct>();
        }
        /// <summary>
        /// 分页搜索商品数据
        /// </summary>
        /// <param name="pageProList"></param>
        public override void DoDsWhProductQuery(ref Model.Pager<CBDsWhProduct> pageProList)
        {
            #region sql条件
            string sqlWhere = @"   ";
            if(pageProList.PageFilter.IsAllDealer)
            {
                sqlWhere = " 1=1 ";
            }
            else if(pageProList.PageFilter.IsDealer)
            {
                sqlWhere = " DsSysNo = '" + pageProList.PageFilter.DsSysNo + "' ";
            }
            else if(pageProList.PageFilter.IsCustomer)
            {
                sqlWhere = " CustomerCode = '" + pageProList.PageFilter.CustomerCode + "' ";
            }
            #endregion

            

            using (var context = Context.UseSharedConnection(true))
            {
                pageProList.Rows = context.Select<CBDsWhProduct>(" DsWhProduct.*  ")
                           .From(@"  DsWhProduct ")
                           .Where(sqlWhere)
                           .Paging(pageProList.CurrentPage, pageProList.PageSize)
                           .OrderBy(" DsWhProduct.SysNo desc ")
                           .QueryMany();
                pageProList.TotalRows = context.Select<int>("count(1)")
                           .From(@"  DsWhProduct ")
                           .Where(sqlWhere)
                           .QuerySingle();
            }
        }

        public override List<DsWhProduct> GetProductModByDsSysNo(int DealerSysNo)
        {
            string sql = " select * from DsWhProduct where DsSysNo='" + DealerSysNo + "' ";
            return Context.Sql(sql).QueryMany<DsWhProduct>();
        }

        /// <summary>
        /// 获取商品列表
        /// </summary>
        /// <param name="DealerSysNo"></param>
        /// <returns></returns>
        public override List<DsWhProduct> GetList(int DealerSysNo)
        {
            string sql = " select * from  DsWhProduct where DsSysNo = '" + DealerSysNo + "' ";
            return Context.Sql(sql).QueryMany<DsWhProduct>();
        }
    }
}
