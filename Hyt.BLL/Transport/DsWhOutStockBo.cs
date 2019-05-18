using Hyt.DataAccess.Transport;
using Hyt.Model.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.Transport
{
    /// <summary>
    /// 货物出库单操作实体
    /// </summary>
    /// <remarks>
    /// 2016-05-17 杨云奕 添加
    /// </remarks>
    public class DsWhOutStockBo : BOBase<DsWhOutStockBo>
    {
        /// <summary>
        /// 添加出库实体
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public int InsertMod(DsWhOutStock mod)
        {
            return IDsWhOutStockDao.Instance.InsertMod(mod);
        }
        /// <summary>
        /// 修改出库实体
        /// </summary>
        /// <param name="mod"></param>
        public void UpdateMod(DsWhOutStock mod)
        {
            IDsWhOutStockDao.Instance.UpdateMod(mod);
        }
        /// <summary>
        /// 通过自动编号删除实体数据
        /// </summary>
        /// <param name="SysNo"></param>
        public void DeleteBySysNo(int SysNo)
        {
            IDsWhOutStockDao.Instance.DeleteBySysNo(SysNo);
        }
        /// <summary>
        /// 获取出库单
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public DsWhOutStock GetOutStockBySysNo(int SysNo)
        {
            return IDsWhOutStockDao.Instance.GetOutStockBySysNo(SysNo);
        }
        /// <summary>
        /// 获取出库单明细
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public CBDsWhOutStock GetExtendsOutStockBySysNo(int SysNo)
        {
            return IDsWhOutStockDao.Instance.GetExtendsOutStockBySysNo(SysNo);
        }

        /// <summary>
        /// 添加出库单明细记录
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public int InsertModList(DsWhOutStockList mod)
        {
            return IDsWhOutStockDao.Instance.InsertModList(mod);
        }
        /// <summary>
        /// 更新出库单明细记录
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public void UpdateModList(DsWhOutStockList mod)
        {
            IDsWhOutStockDao.Instance.UpdateModList(mod);
        }
        /// <summary>
        /// 删除出库单明细记录
        /// </summary>
        /// <param name="SysNo"></param>
        public  void DeleteModListBySysNo(int SysNo)
        {
            IDsWhOutStockDao.Instance.DeleteModListBySysNo(SysNo);
        }
        /// <summary>
        /// 通过自动编号获取单独的出库单数据
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public DsWhOutStockList GetOutStockListBySysNo(int SysNo)
        {
            return IDsWhOutStockDao.Instance.GetOutStockListBySysNo(SysNo);
        }
        /// <summary>
        /// 通过父ID获取出库单明细
        /// </summary>
        /// <param name="PSysNo"></param>
        /// <returns></returns>
        public List<DsWhOutStockList> GetOutStockListByPSysNo(int PSysNo)
        {
            return IDsWhOutStockDao.Instance.GetOutStockListByPSysNo(PSysNo);
        }
        public void DsWhOutStockPager(ref Model.Pager<Model.Transport.CBDsWhOutStock> pageCusList)
        {
            IDsWhOutStockDao.Instance.DsWhOutStockPager(ref  pageCusList);
        }

        public List<CBDsWhOutStockList> GetDsWhOutStockByBitNumber(string bitNumber)
        {
            return IDsWhOutStockDao.Instance.GetDsWhOutStockByBitNumber( bitNumber);
        }
    }
}
