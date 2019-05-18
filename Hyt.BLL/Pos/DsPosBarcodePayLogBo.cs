using Hyt.Model.Pos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.Pos
{
    /// <summary>
    /// 条码收银历史记录
    /// </summary>
    public class DsPosBarcodePayLogBo : BOBase<DsPosBarcodePayLogBo>
    {
        /// <summary>
        /// 添加历史记录
        /// </summary>
        /// <param name="Mod"></param>
        /// <returns></returns>
        public  int InnerDsPosBarcodePayLog(DsPosBarcodePayLog Mod)
        {
            return Hyt.DataAccess.Pos.IDsPosBarcodePayLogDao.Instance.InnerDsPosBarcodePayLog(Mod);
        }
        /// <summary>
        /// 更新历史记录
        /// </summary>
        /// <param name="Mod"></param>
        public  void UpdateDsPosBarcodePayLog(DsPosBarcodePayLog Mod)
        {
            Hyt.DataAccess.Pos.IDsPosBarcodePayLogDao.Instance.UpdateDsPosBarcodePayLog(Mod);
        }
        /// <summary>
        /// 删除历史记录
        /// </summary>
        /// <param name="SysNo"></param>
        public void DeleteDsPosBarcodePayLog(int SysNo)
        {
            Hyt.DataAccess.Pos.IDsPosBarcodePayLogDao.Instance.DeleteDsPosBarcodePayLog(SysNo);
        }
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="pager"></param>
        public void DsPosBarcodePayLogPager(ref Model.Pager<DsPosBarcodePayLog> pager)
        {
            Hyt.DataAccess.Pos.IDsPosBarcodePayLogDao.Instance.DsPosBarcodePayLogPager(ref pager);
        }
    }
}
