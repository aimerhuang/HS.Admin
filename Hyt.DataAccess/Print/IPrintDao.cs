using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.DataAccess.Base;
using Hyt.Model.Generated;

namespace Hyt.DataAccess.Print
{
    /// <summary>
    /// 打印模块公共抽象类
    /// </summary>
    /// <remarks>
    /// 2013-07-12 郑荣华 创建
    /// </remarks>
    public abstract class IPrintDao : DaoBase<IPrintDao>
    {
        /// <summary>
        /// 获取配货(拣货，出库)单打印实体
        /// </summary>
        /// <param name="outStockSysNo">出库单系统编号</param>
        /// <returns>配货(拣货，出库)单打印实体</returns>
        /// <remarks>
        /// 2013-07-16 郑荣华 创建
        /// </remarks>
        public abstract CBPrtPicking GetPrintPicking(int outStockSysNo);

        /// <summary>
        /// 获取配送单打印实体
        /// </summary>
        /// <param name="sysNo">配送单号</param>
        /// <returns>配送单打印实体</returns>
        /// <remarks>
        /// 2013-07-16 郑荣华 创建
        /// </remarks>
        public abstract PrtDelivery GetPrintDelivery(int sysNo);

        /// <summary>
        /// 获取入库单打印实体
        /// </summary>
        /// <param name="sysNo">入库单号</param>
        /// <returns>入库单打印实体</returns>
        /// <remarks>
        /// 2013-07-16 郑荣华 创建
        /// </remarks>
        public abstract PrtInstock GetPrintInstock(int sysNo);

        /// <summary>
        /// 获取借货单打印实体
        /// </summary>
        /// <param name="sysNo">借货单号</param>
        /// <returns>借货单打印实体</returns>
        /// <remarks>
        /// 2013-07-16 郑荣华 创建
        /// </remarks>
        public abstract PrtLend GetPrintLend(int sysNo);

        /// <summary>
        /// 获取结算单打印实体
        /// </summary>
        /// <param name="sysNo">结算单号</param>
        /// <returns>结算单打印实体</returns>
        /// <remarks>
        /// 2013-07-16 郑荣华 创建
        /// </remarks>
        public abstract PrtSettlement GetPrintSettlement(int sysNo);

        /// <summary>
        /// 获取第三方快递包裹单打印实体
        /// </summary>
        /// <param name="outStockSysNo">出库单系统编号</param>
        /// <returns>快递包裹单打印实体</returns>
        /// <remarks>
        /// 2013-07-12 郑荣华 创建
        /// </remarks>
        public abstract PrtPack GetPrintPack(int outStockSysNo);

        /// <summary>
        /// 获取分销配货(拣货，出库)单打印实体
        /// </summary>
        /// <param name="outStockSysNo">出库单系统编号</param>
        /// <returns>分销配货(拣货，出库)单打印实体</returns>
        /// <remarks>
        /// 2013-09-13 郑荣华 创建
        /// </remarks>
        public abstract PrtDsPicking GetPrintDsPicking(int outStockSysNo);

        /// <summary>
        /// 获取分销配送结算单打印实体
        /// </summary>
        /// <param name="sysNo">配送单号</param>
        /// <returns>分销配送结算单打印实体</returns>
        /// <remarks>
        /// 2013-09-13 郑荣华 创建
        /// </remarks>
        public abstract PrtDsDelivery GetPrintDsDelivery(int sysNo);

        
        /// <summary>
        /// 批量打印拣货单实体
        /// </summary>
        /// <param name="outStockSysNos">出库单编号</param>
        /// <returns></returns>
        /// <remarks>
        /// 2014-07-11 朱成果 创建
        /// </remarks>
        public abstract List<CBPrtPicking> GetPrintPickingList(List<int> outStockSysNos);

        /// <summary>
        /// 获取升舱出库单打印信息
        /// </summary>
        /// <param name="outStockSysNos">出库单列表</param>
        /// <returns></returns>
        /// <remarks>
        /// 2014-07-11 朱成果 创建
        /// </remarks>
        public abstract List<PrtDsPicking> GetPrintDsPickingList(List<int> outStockSysNos);

          /// <summary>
        /// 获取包裹单数据
        /// </summary>
        /// <param name="outStockSysNos">出库单编号列表</param>
        /// <returns></returns>
        /// <remarks>
        /// 2014-07-14 朱成果 创建
        /// </remarks>
        public abstract List<CBPrtPack> GetPrintPackList(List<int> outStockSysNos);

        /// <summary>
        /// 添加打印模板
        /// </summary>
        /// <param name="prtPrint"></param>
        /// <returns></returns>
        public abstract int InsertPrtPrintTempel(PrtPrintTempel prtPrint);
        /// <summary>
        /// 添加打印模板
        /// </summary>
        /// <param name="prtPrint"></param>
        /// <returns></returns>
        public abstract void UpdatePrtPrintTempel(PrtPrintTempel prtPrint);
        /// <summary>
        /// 添加打印模板
        /// </summary>
        /// <param name="prtPrint"></param>
        /// <returns></returns>
        public abstract PrtPrintTempel GetPrtPrintTempel();

    }
}
