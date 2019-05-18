using Hyt.DataAccess.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.Transport
{
    /// <summary>
    /// 总航运控制
    /// </summary>
    /// <remarks>
    /// 2016-5-18 杨云奕 添加
    /// </remarks>
    public class DsWhTotalWaybillBo : BOBase<DsWhTotalWaybillBo>
    {
        /// <summary>
        /// 添加总运单
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public  int InsertMod(Model.Transport.DsWhTotalWaybill mod)
        {
            return IDsWhTotalWaybillDao.Instance.InsertMod(mod);
        }
        /// <summary>
        /// 修改总运单数据
        /// </summary>
        /// <param name="mod"></param>
        public  void UpdateMod(Model.Transport.DsWhTotalWaybill mod)
        {
            IDsWhTotalWaybillDao.Instance.UpdateMod(mod);
        }
        /// <summary>
        /// 删除总运单数据
        /// </summary>
        /// <param name="SysNo"></param>
        public  void DeleteBySysNo(int SysNo)
        {
            IDsWhTotalWaybillDao.Instance.DeleteBySysNo(SysNo);
        }
        /// <summary>
        /// 通过编号获取总运单数据
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public  Model.Transport.DsWhTotalWaybill GetDsWhTotalWaybillBySysNo(int SysNo)
        {
            return IDsWhTotalWaybillDao.Instance.GetDsWhTotalWaybillBySysNo(SysNo);
        }
        /// <summary>
        /// 通过编号获取总运单数据
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public  Model.Transport.CBDsWhTotalWaybill GetCBDsWhTotalWaybillBySysNo(int SysNo)
        {
            return IDsWhTotalWaybillDao.Instance.GetCBDsWhTotalWaybillBySysNo(SysNo);
        }
        /// <summary>
        /// 添加总运单包裹数据
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public  int InsertModList(Model.Transport.DsWhTotalWaybillList mod)
        {
            return IDsWhTotalWaybillDao.Instance.InsertModList(mod);
        }
        /// <summary>
        /// 修改总运单包裹数据
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public  int UpdateModList(Model.Transport.DsWhTotalWaybillList mod)
        {
            return IDsWhTotalWaybillDao.Instance.UpdateModList(mod);
        }
        /// <summary>
        /// 通过父id删除总运单包裹明细
        /// </summary>
        /// <param name="PSysNo"></param>
        public  void DeleteModListByPSysNo(int PSysNo)
        {
            IDsWhTotalWaybillDao.Instance.DeleteModListByPSysNo(PSysNo);
        }
        /// <summary>
        /// 通过父id获取总运单中包裹数据
        /// </summary>
        /// <param name="PSysNo"></param>
        /// <returns></returns>
        public  List<Model.Transport.DsWhTotalWaybillList> GetDsWhTotalWaybillListByPSysNo(int PSysNo)
        {
            return IDsWhTotalWaybillDao.Instance.GetDsWhTotalWaybillListByPSysNo(PSysNo);
        }

        public void DeleteModListBySysNo(int SysNo)
        {
             IDsWhTotalWaybillDao.Instance.DeleteModListBySysNo(SysNo);
        }

        public void DsWhTotalWaybillPager(ref Model.Pager<Model.Transport.CBDsWhTotalWaybill> pageCusList)
        {
            IDsWhTotalWaybillDao.Instance.DsWhTotalWaybillPager(ref pageCusList);
        }



        public List<Model.Transport.DsWhTotalWaybill> GetTotalWayBillByCourierNumber(string CourierNumber)
        {
            return IDsWhTotalWaybillDao.Instance.GetTotalWayBillByCourierNumber(CourierNumber);
        }
    }
}
