using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Promotion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Promotion
{
    /// <summary>
    /// 秒杀功能项
    /// </summary>
    public abstract class ISpSpikeDao:DaoBase<ISpSpikeDao>
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public abstract int InsertMod(SpSpike mod);
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="mod"></param>
        public abstract void UpdateMod(SpSpike mod);
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="SpSpikePager"></param>
        public abstract void GetSpSpikeListPager(ref Pager<SpSpike> SpSpikePager);
        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="SysNo"></param>
        /// <param name="Status"></param>
        public abstract void UpdateSpSpikeStatus(int SysNo, int Status);
        /// <summary>
        /// 删除秒杀
        /// </summary>
        /// <param name="SysNo"></param>
        public abstract void DeleteSpSpikeBySysNo(int SysNo);

        public abstract SpSpike GetSpSpikeBySysNo(int SysNo);


        public abstract int InsertSpSpikeItem(SpSpikeItem item);
        public abstract int UpdateSpSpikeItem(SpSpikeItem item);
        public abstract void DeleteSpSpikeItem(int SysNo);
        public abstract void GetSpSpikeItemListPager(ref Pager<SpSpikeItem> SpSpikePager);
        public abstract List<SpSpikeItem> GetSpSpikeItemList(int pSysNo);
    }
}
