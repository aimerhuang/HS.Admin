using Hyt.DataAccess.Transport;
using Hyt.Model.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.Transport
{
    /// <summary>
    /// 收货人实名认证档案
    /// </summary>
    /// <remarks>
    /// 2016-05-17 杨云奕 添加
    /// </remarks>
    public class DsWhRecipienterBo : BOBase<DsWhRecipienterBo>
    {
        /// <summary>
        /// 添加收件人档案
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public int InsertMod(DsWhRecipienter mod)
        {
            return IDsWhRecipienterDao.Instance.InsertMod(mod);
        }
        /// <summary>
        /// 更新收件人档案
        /// </summary>
        /// <param name="mod"></param>
        public void UpdateMod(DsWhRecipienter mod)
        {
            IDsWhRecipienterDao.Instance.UpdateMod(mod);
        }
        /// <summary>
        /// 获取收件人实名档案
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public DsWhRecipienter GetDsWhRecipienter(int SysNo)
        {
            return IDsWhRecipienterDao.Instance.GetDsWhRecipienter(SysNo);
        }
        /// <summary>
        /// 删除收件人实名档案
        /// </summary>
        /// <param name="SysNo"></param>
        public void DeleteDsWhRecipienterBySysNo(int SysNo)
        {
            IDsWhRecipienterDao.Instance.DeleteDsWhRecipienterBySysNo(SysNo);
        }

        public List<DsWhRecipienter> GetDsWhRecipienterList(List<string> IdCardList)
        {
            return IDsWhRecipienterDao.Instance.GetDsWhRecipienterList(IdCardList);
        }

        public void GetDsWhRecipienterPager(ref Model.Pager<DsWhRecipienter> pageCusList)
        {
            IDsWhRecipienterDao.Instance.GetDsWhRecipienterList(ref pageCusList);
        }

        public DsWhRecipienter GetDsWhRecipienterByIDCard(string IDCard)
        {
            return IDsWhRecipienterDao.Instance.GetDsWhRecipienterByIDCard(IDCard);
        }
    }
}
