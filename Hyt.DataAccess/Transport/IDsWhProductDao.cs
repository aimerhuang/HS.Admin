using Hyt.DataAccess.Base;
using Hyt.Model.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Transport
{
    /// <summary>
    /// 转运系统商品档案
    /// </summary>
    /// <remarks>
    /// 2016-5-17 杨云奕 添加
    /// </remarks>
    public abstract class IDsWhProductDao : DaoBase<IDsWhProductDao>
    {
        /// <summary>
        /// 添加商品档案
        /// </summary>
        /// <param name="Mod"></param>
        /// <returns></returns>
        public abstract int InsertMod(DsWhProduct Mod);
        /// <summary>
        /// 更新商品档案
        /// </summary>
        /// <param name="Mod"></param>
        public abstract void UpdateMod(DsWhProduct Mod);
        /// <summary>
        /// 删除商品档案数据
        /// </summary>
        /// <param name="SysNo"></param>
        public abstract void DeleteBySysNo(int SysNo);
        /// <summary>
        /// 通过自动编号获取商品档案
        /// </summary>
        /// <param name="SysNo">自动编号</param>
        /// <returns></returns>
        public abstract DsWhProduct GetProductModBySysNo(int SysNo);
        /// <summary>
        /// 通过商品编号获取商品档案数据
        /// </summary>
        /// <param name="GoodsCode"></param>
        /// <returns></returns>
        public abstract DsWhProduct GetProductModByGoodsCode(string GoodsCode);
        /// <summary>
        /// 通过自动编号集合获取商品档案集合
        /// </summary>
        /// <param name="SysNos"></param>
        /// <returns></returns>
        public abstract List<DsWhProduct> GetProductModBySysNos(List<int> SysNos);
        /// <summary>
        /// 通过货品编号获取商品档案集合
        /// </summary>
        /// <param name="GoodsCodes"></param>
        /// <returns></returns>
        public abstract List<DsWhProduct> GetProductModByGoodsCodes(List<string> GoodsCodes);
        /// <summary>
        /// 分页搜索商品数据
        /// </summary>
        /// <param name="pageProList"></param>
        public abstract void DoDsWhProductQuery(ref Model.Pager<CBDsWhProduct> pageProList);

        public abstract List<DsWhProduct> GetProductModByDsSysNo(int DealerSysNo);
        /// <summary>
        /// 获取商品列表
        /// </summary>
        /// <param name="DealerSysNo"></param>
        /// <returns></returns>
        public abstract List<DsWhProduct> GetList(int DealerSysNo);
    }
}
