using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    /// <summary>
    /// 网品项查询实体类
    /// </summary>
    /// <remarks>
    /// 2013/6/24 苟治国 创建
    /// </remarks>
    [Serializable]
    public class CBFeProductItem
    {

        /// <summary>
        /// 系统编号
        /// </summary>
        public int SysNo { get; set; }

        /// <summary>
        /// 系统编号
        /// </summary>
        public int GroupSysNo { get; set; }

        /// <summary>
        /// 商品编号
        /// </summary>
        public int ProductSysNo { get; set; }

        /// <summary>
        /// 展示开始时间
        /// </summary>
        public DateTime BeginDate { get; set; }

        /// <summary>
        /// 展示结束时间
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 显示顺序
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// 显示标志：正常（10）、新品（20）、热销（30）、推荐
        /// </summary>
        public int DispalySymbol { get; set; }

        /// <summary>
        /// 状态：待审（10）、已审（20）、作废（－10）
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public int CreatedBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// 最后更新人
        /// </summary>
        public int LastUpdateBy { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime LastUpdateDate { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 后台显示名称
        /// </summary>
        public string EasName { get; set; }

        /// <summary>
        /// 商品图片
        /// </summary>
        public string ProductImage { get; set; }

        /// <summary>
        /// 商品编号
        /// </summary>
        public string ErpCode { get; set; }
        /// <summary>
        /// 分销商系统编号
        /// </summary>
        public int DealerSysNo { get; set; }
        /// <summary>
        /// 分销商
        /// </summary>
        public List<int> intDealerSysNoList { get; set; }
        /// <summary>
        ///分销商名称
        /// </summary>
        public string DealerName { get; set; }
        /// <summary>
        /// 是否绑定经销商
        /// </summary>
        public bool IsBindDealer { get; set; }
        /// <summary>
        /// 是否绑定所有经销商
        /// </summary>
        public bool IsBindAllDealer { get; set; }
        /// <summary>
        /// 经销商创建人
        /// </summary>
        public int DealerCreatedBy { get; set; }
        /// <summary>
        /// 搜索条件选中的分销商
        /// </summary>
        public int SelectedDealerSysNo { get; set; }
        public decimal BasicPrice { get; set; }
        public string UserPriceList { get; set; }
        public string LevelValueList { get; set; }
        public string DisCount { get; set; }
        public string OrginImagePath { get; set; }
        public string ProTips { get; set; }
        public string SaleNumber { get; set; }
    }
}
