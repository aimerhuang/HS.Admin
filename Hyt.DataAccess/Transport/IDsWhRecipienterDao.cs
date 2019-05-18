using Hyt.DataAccess.Base;
using Hyt.Model.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Transport
{
    /// <summary>
    /// 收货人实名认证档案
    /// </summary>
    /// <remarks>
    /// 2016-05-17 杨云奕 添加
    /// </remarks>
    public abstract class IDsWhRecipienterDao : DaoBase<IDsWhRecipienterDao>
    {
        /// <summary>
        /// 添加收件人档案
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public abstract int InsertMod(DsWhRecipienter mod);
        /// <summary>
        /// 更新收件人档案
        /// </summary>
        /// <param name="mod"></param>
        public abstract void UpdateMod(DsWhRecipienter mod);
        /// <summary>
        /// 获取收件人实名档案
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public abstract DsWhRecipienter GetDsWhRecipienter(int SysNo);
        /// <summary>
        /// 删除收件人实名档案
        /// </summary>
        /// <param name="SysNo"></param>
        public abstract void DeleteDsWhRecipienterBySysNo(int SysNo);
        /// <summary>
        /// 收货人档案
        /// </summary>
        /// <param name="IdCardList"></param>
        /// <returns></returns>
        public abstract List<DsWhRecipienter> GetDsWhRecipienterList(List<string> IdCardList);

        public abstract void GetDsWhRecipienterList(ref Model.Pager<DsWhRecipienter> pageCusList);

        public abstract DsWhRecipienter GetDsWhRecipienterByIDCard(string IDCard);
    }
}
