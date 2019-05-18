using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    /// <summary>
    /// 广告项池查询实体类
    /// </summary>
    /// <remarks>
    /// 2013/6/18 苟治国 创建
    /// </remarks>
    public partial class CBFeAdvertItem
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
        /// 广告名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 广告内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 广告图片Url
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// 广告链接
        /// </summary>
        public string LinkUrl { get; set; }

        /// <summary>
        /// 广告链接提示信息
        /// </summary>
        public string LinkTitle { get; set; }

        /// <summary>
        /// 广告打开方式
        /// </summary>
        public int OpenType { get; set; }

        /// <summary>
        /// 广告开始时间
        /// </summary>
        public DateTime BeginDate { get; set; }

        /// <summary>
        /// 广告结束时间
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 显示顺序
        /// </summary>
        public int DisplayOrder { get; set; }

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
        /// 广告组类型
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 分销商系统编号
        /// </summary>
        public int DealerSysNo { get; set; }
        /// <summary>
        /// 分销商
        /// </summary>
        public List<int> intDealerSysNoList { get; set; }
        /// <summary>
        /// 分销商
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
    }
}
