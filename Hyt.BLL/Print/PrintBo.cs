using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.BLL.Logistics;
using Hyt.BLL.Order;
using Hyt.DataAccess.Print;
using Hyt.DataAccess.Product;
using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Model.Generated;

namespace Hyt.BLL.Print
{
    /// <summary>
    /// 打印业务类
    /// </summary>
    /// <remarks>
    /// 2013-07-12 郑荣华 创建
    /// </remarks>
    public class PrintBo : BOBase<PrintBo>
    {

        /// <summary>
        /// 获取配货(拣货，出库)单打印实体
        /// </summary>
        /// <param name="outStockSysNo">出库单系统编号</param>
        /// <returns>配货(拣货，出库)单打印实体</returns>
        /// <remarks>
        /// 2013-07-16 郑荣华 创建
        /// </remarks>
        public CBPrtPicking GetPrintPicking(int outStockSysNo)
        {
            return IPrintDao.Instance.GetPrintPicking(outStockSysNo);
        }

        /// <summary>
        /// 获取配送单打印实体
        /// </summary>
        /// <param name="sysNo">配送单号</param>
        /// <returns>配送单打印实体</returns>
        /// <remarks>
        /// 2013-07-16 郑荣华 创建
        /// </remarks>
        public PrtDelivery GetPrintDelivery(int sysNo)
        {
            return IPrintDao.Instance.GetPrintDelivery(sysNo);
        }

        /// <summary>
        /// 获取入库单打印实体
        /// </summary>
        /// <param name="sysNo">入库单号</param>
        /// <returns>入库单打印实体</returns>
        /// <remarks>
        /// 2013-07-16 郑荣华 创建
        /// </remarks>
        public PrtInstock GetPrintInstock(int sysNo)
        {
            return IPrintDao.Instance.GetPrintInstock(sysNo);
        }

        /// <summary>
        /// 获取借货单打印实体
        /// </summary>
        /// <param name="sysNo">借货单号</param>
        /// <returns>借货单打印实体</returns>
        /// <remarks>
        /// 2013-07-16 郑荣华 创建
        /// </remarks>
        public PrtLend GetPrintLend(int sysNo)
        {
            var model = IPrintDao.Instance.GetPrintLend(sysNo);
            if (model != null)
            {
                foreach (var item in model.List)
                {
                    item.SubList = IPdPriceDao.Instance.GetProductLevelPrice(item.ProductSysNo);
                }
            }
            return model;
        }

        /// <summary>
        /// 获取结算单打印实体
        /// </summary>
        /// <param name="sysNo">结算单号</param>
        /// <returns>结算单打印实体</returns>
        /// <remarks>
        /// 2013-07-16 郑荣华 创建
        /// 2013-07-17 黄伟 修改
        /// </remarks>
        public PrtSettlement GetPrintSettlement(int sysNo)
        {
            return IPrintDao.Instance.GetPrintSettlement(sysNo);
        }

        /// <summary>
        /// 获取第三方快递包裹单打印实体
        /// </summary>
        /// <param name="outStockSysNo">出库单系统编号</param>
        /// <returns>快递包裹单打印实体</returns>
        /// <remarks>
        /// 2013-07-12 郑荣华 创建
        /// </remarks>
        public PrtPack GetPrintPack(int outStockSysNo)
        {
            return IPrintDao.Instance.GetPrintPack(outStockSysNo);
        }

        #region 分销

        /// <summary>
        /// 获取分销配货(拣货，出库)单打印实体
        /// </summary>
        /// <param name="outStockSysNo">出库单系统编号</param>
        /// <returns>分销配货(拣货，出库)单打印实体</returns>
        /// <remarks>
        /// 2013-09-13 郑荣华 创建
        /// </remarks>
        public PrtDsPicking GetPrintDsPicking(int outStockSysNo)
        {
            var model = IPrintDao.Instance.GetPrintDsPicking(outStockSysNo);

            if (model != null)
            {
                var mallOrderIds = string.Empty;
                foreach (var cbDsOrder in model.ListDs)
                {
                    mallOrderIds += "," + cbDsOrder.MallOrderId;
                }
                if (!string.IsNullOrEmpty(mallOrderIds))
                {
                    model.MallOrderId = mallOrderIds.Substring(1);
                    model.ShopAccount = model.ListDs[0].ShopAccount;
                    model.ShopName = model.ListDs[0].ShopName;
                    model.IsSelfSupport = model.ListDs[0].IsSelfSupport;
                }
            }
            return model;
        }

        /// <summary>
        /// 获取分销配送结算单打印实体
        /// </summary>
        /// <param name="sysNo">配送单号</param>
        /// <returns>分销配送结算单打印实体</returns>
        /// <remarks>
        /// 2013-09-13 郑荣华 创建
        /// </remarks>
        public PrtDsDelivery GetPrintDsDelivery(int sysNo)
        {      
            return IPrintDao.Instance.GetPrintDsDelivery(sysNo);
        }

        #endregion

        #region 批量打印

         /// <summary>
        /// 批量打打印拣货单实体
        /// </summary>
        /// <param name="outStockSysNos">出库单编号</param>
        /// <returns></returns>
        /// <remarks>
        /// 2014-07-11 朱成果 创建
        /// </remarks>
        public  List<CBPrtPicking> GetPrintPickingList(List<int> outStockSysNos)
        {
            return IPrintDao.Instance.GetPrintPickingList(outStockSysNos);
        }

         /// <summary>
        /// 获取升舱出库单打印信息
        /// </summary>
        /// <param name="outStockSysNos">出库单列表</param>
        /// <returns></returns>
        /// <remarks>
        /// 2014-07-11 朱成果 创建
        /// </remarks>
        public  List<PrtDsPicking> GetPrintDsPickingList(List<int> outStockSysNos)
        {
            return IPrintDao.Instance.GetPrintDsPickingList(outStockSysNos);
        }

          /// <summary>
        /// 获取包裹单数据
        /// </summary>
        /// <param name="outStockSysNos">出库单编号列表</param>
        /// <returns></returns>
        /// <remarks>
        /// 2014-07-14 朱成果 创建
        /// </remarks>
        public  List<CBPrtPack> GetPrintPackList(List<int> outStockSysNos)
        {
            return IPrintDao.Instance.GetPrintPackList(outStockSysNos);
        }
        #endregion

        #region 添加打印商品标签模板
        /// <summary>
        /// 添加打印模板
        /// </summary>
        /// <param name="prtPrint"></param>
        /// <returns></returns>
        public int InsertPrtPrintTempel(PrtPrintTempel prtPrint)
        {
            return IPrintDao.Instance.InsertPrtPrintTempel(prtPrint);
        }
        /// <summary>
        /// 添加打印模板
        /// </summary>
        /// <param name="prtPrint"></param>
        /// <returns></returns>
        public void UpdatePrtPrintTempel(PrtPrintTempel prtPrint)
        {
             IPrintDao.Instance.UpdatePrtPrintTempel(prtPrint);
        }
        /// <summary>
        /// 添加打印模板
        /// </summary>
        /// <param name="prtPrint"></param>
        /// <returns></returns>
        public PrtPrintTempel GetPrtPrintTempel()
        {
            return IPrintDao.Instance.GetPrtPrintTempel();
        }
        #endregion
    }
}
