using System.Collections.Generic;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 借货单打印实体
    /// </summary>
    /// <remarks>
    /// 2013-07-16 郑荣华 创建
    /// </remarks>
    public class PrtLend:WhProductLend
    {
        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WareHouseName { get; set; }

        /// <summary>
        /// 明细列表
        /// </summary>
        public IList<PrtSubLend> List;
    }
    /// <summary>
    /// 入库单打印明细实体
    /// </summary>
    /// <remarks>
    /// 2013-07-16 郑荣华 创建
    /// </remarks>
    public class PrtSubLend : WhProductLendItem
    {
        /// <summary>
        /// 商品ID
        /// </summary>
        public string ErpCode { get; set; }

        /// <summary>
        /// 商品简称
        /// </summary>
        public string ProductShortTitle { get; set; }

        /// <summary>
        /// 商品会员等级价格列表
        /// </summary>
        public IList<CBPdPrice> SubList;
    }

  
}
