using System;
using System.Collections.Generic;
using Hyt.DataAccess.MallSeller;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using System.Linq;

namespace Hyt.BLL.MallSeller
{
    public class DsReturnBo : BOBase<DsReturnBo>
    {

        /// <summary>
        /// 退货订单分页
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>分页</returns>
        /// <remarks>2013-09-10 余勇 创建</remarks>
        public Pager<CBDsReturn> GetPagerList(ParaDsReturnFilter filter)
        {
            return IDsReturnDao.Instance.Query(filter);
        }

        /// <summary>
        /// 根据hyt退换货单号获取实体
        /// </summary>
        /// <param name="value">hyt退换货单号</param>
        /// <returns></returns>
        /// <remarks>2013-09-10 朱家宏 创建</remarks>
        public DsReturn SelectByRmaSysNo(int value)
        {
            return IDsReturnDao.Instance.SelectByRmaSysNo(value);
        }

        /// <summary>
        /// 获取分销商退换货明细
        /// </summary>
        /// <param name="dsReturnSysNo">分销商退换货单编号</param>
        /// <returns></returns>
        /// <remarks>2013-09-10 朱家宏 创建</remarks>
        public IList<DsReturnItem> SelectItems(int dsReturnSysNo)
        {
            return IDsReturnDao.Instance.SelectItems(dsReturnSysNo);
        }

        /// <summary>
        /// 获取分销商退换货单
        /// </summary>
        /// <param name="shopAccount">账户</param>
        /// <param name="mallTypeSysNo">类型</param>
        /// <param name="top">取前几条</param>
        /// <param name="isFinish">退款完成</param>
        /// <returns></returns>
        /// <remarks>2013-09-10 黄志勇 创建</remarks>
        public List<CBDsReturn> GetReturn(string shopAccount, int mallTypeSysNo, int top, bool? isFinish)
        {
            return IDsReturnDao.Instance.GetReturn(shopAccount, mallTypeSysNo, top, isFinish);
        }

        /// <summary>
        /// 创建分销商退换货单及明细
        /// </summary>
        /// <param name="dsReturn">主表实体</param>
        /// <param name="returnItems">副表实体</param>
        /// <returns>主表编号</returns>
        /// <remarks>2013-09-12 朱家宏 创建</remarks>
        public int Create(DsReturn dsReturn, List<DsReturnItem> returnItems)
        {
            if (dsReturn == null || returnItems == null || !returnItems.Any())
            {
                throw new ArgumentNullException();
            }

            var mainSysNo = IDsReturnDao.Instance.Insert(dsReturn);
            if (mainSysNo > 0)
            {
                foreach (var item in returnItems)
                {
                    item.DsReturnSysNo = mainSysNo;
                    IDsReturnDao.Instance.InsertItem(item);
                }
            }

            return mainSysNo;
        }

        #region 商城退换货审核后更新分销工具退款
        /// <summary>
        /// 商城退换货审核后更新分销工具退款信息
        /// </summary>
        /// <param name="hytRMAID">退换货编号</param>
        /// <param name="hytOrder">商城订单号</param>
        /// <param name="mallReturnAmount">金额</param>
        /// <remarks>2013-09-25 朱成果 创建</remarks>
        public void HytRMAAuditCallBack(int hytRMAID, int hytOrder, decimal mallReturnAmount)
        {
            var dsRc = Hyt.DataAccess.MallSeller.IDsReturnDao.Instance.SelectByRmaSysNo(hytRMAID);
            if (dsRc != null)
            {
                dsRc.MallReturnAmount = mallReturnAmount;
                Hyt.DataAccess.MallSeller.IDsReturnDao.Instance.Update(dsRc);
            }
        }
        #endregion
    }
}
