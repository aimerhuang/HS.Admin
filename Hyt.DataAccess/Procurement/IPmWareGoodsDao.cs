using Hyt.DataAccess.Base;
using Hyt.Model.Procurement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Procurement
{
    /// <summary>
    /// 采购仓库商品库存类型
    /// </summary>
    public abstract class IPmWareGoodsDao :  DaoBase<IPmWareGoodsDao>
    {
        /// <summary>
        /// 新增采购商品
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public abstract int InsertPmWareGoodsDao(PmWareGoods mod);
        /// <summary>
        /// 更新采购商品
        /// </summary>
        /// <param name="mod"></param>
        public abstract void UpdatePmWareGoodsDao(PmWareGoods mod);
        /// <summary>
        /// 采购商品分页
        /// </summary>
        /// <param name="pager"></param>
        public abstract void GetPmWareGoodsListByPager(ref Model.Pager<CBPmWareGoods> pager);
        /// <summary>
        /// 通过商品编号获取采购商品库存信息
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        public abstract PmWareGoods GetPmWareGoodsByProSysNo(int sysNo);
        /// <summary>
        /// 设置采购库存商品数量情况
        /// </summary>
        /// <param name="sysNo"></param>
        /// <param name="wareNum"></param>
        /// <param name="stayInWare"></param>
        /// <param name="freeze"></param>
        public abstract void SetPmWareGoodsUpdateDataByProSysNo(int sysNo, int wareNum, int stayInWare, int freeze);

        /// <summary>
        /// 添加采购库存商品记录
        /// </summary>
        /// <param name="history"></param>
        public abstract int InnerPmWareGoodsHistory(PmWareGoodsHistory history);
        /// <summary>
        /// 获取采购入库历史记录
        /// </summary>
        /// <param name="ProSysNo"></param>
        /// <returns></returns>
        public abstract List<PmWareGoodsHistory> GetPmWareGoodsHistoryList(int ProSysNo);

        public abstract List<CBPmWareGoods> GetPmWareGoodsListBySysNos(string idList);
    }
}
