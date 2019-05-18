using Hyt.DataAccess.Procurement;
using Hyt.Model;
using Hyt.Model.Procurement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.Procurement
{
    public class PmWareGoodsBo : BOBase<PmWareGoodsBo>
    {
        public void InnerOrUpdatePmWareGoods(PmWareGoods tempMod)
        {
            ///获取仓库库存数量
            PmWareGoods mod = IPmWareGoodsDao.Instance.GetPmWareGoodsByProSysNo(tempMod.ProSysNo);
            if (mod == null)
            {
                tempMod.SysNo = IPmWareGoodsDao.Instance.InsertPmWareGoodsDao(tempMod);
            }
            else
            {
                IPmWareGoodsDao.Instance.SetPmWareGoodsUpdateDataByProSysNo(tempMod.ProSysNo, tempMod.WareNum, tempMod.StayInWare, tempMod.Freeze);
                tempMod.SysNo = mod.SysNo;
            }
            ///待回库数据
            if (tempMod.StayInWare > 0)
            {
                InsertPmWareGoodsHistoryData(tempMod.SysNo, "厂家生产配送中", tempMod.StayInWare);
            }
            else  if (tempMod.StayInWare < 0)
            {
                InsertPmWareGoodsHistoryData(tempMod.SysNo, "厂家生产已配送回库", tempMod.StayInWare);
            }
            ///库存数量变化
            if (tempMod.WareNum > 0)
            {
                InsertPmWareGoodsHistoryData(tempMod.SysNo, "商品入库", tempMod.WareNum);
            }
            else if (tempMod.WareNum < 0)
            {
                InsertPmWareGoodsHistoryData(tempMod.SysNo, "商品出库", tempMod.WareNum);
            }
            ///冻结商品数量变化
            if (tempMod.Freeze > 0)
            {
                InsertPmWareGoodsHistoryData(tempMod.SysNo, "配送出库冻结", tempMod.Freeze);
            }
            else if (tempMod.Freeze < 0)
            {
                InsertPmWareGoodsHistoryData(tempMod.SysNo, "配送完成消冻", tempMod.Freeze);
            }
        }
        /// <summary>
        /// 历史记录
        /// </summary>
        /// <param name="pSysNo"></param>
        /// <param name="txt"></param>
        /// <param name="num"></param>
        public void InsertPmWareGoodsHistoryData(int pSysNo, string txt,int num)
        {
            PmWareGoodsHistory history = new PmWareGoodsHistory();
            history.PSysNo = pSysNo;
            history.CreateDate = DateTime.Now;
            history.TextInfo = txt;
            history.GHValue = num;
            IPmWareGoodsDao.Instance.InnerPmWareGoodsHistory(history);
        }
        /// <summary>
        /// 通过分页获取数据
        /// </summary>
        /// <param name="pager"></param>
        public void GetPmWareGoodsPager(ref Pager<CBPmWareGoods> pager)
        {
            IPmWareGoodsDao.Instance.GetPmWareGoodsListByPager(ref pager);
        }

        /// <summary>
        /// 获取库存数据集合
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        public List<CBPmWareGoods> GetPmWareGoodsListBySysNos(string idList)
        {
            return IPmWareGoodsDao.Instance.GetPmWareGoodsListBySysNos(idList);
        }
    }
}
