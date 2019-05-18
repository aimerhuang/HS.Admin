using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 分销商信息查询参数实体
    /// </summary>
    /// <remarks>
    /// 2013-09-04 郑荣华 创建
    /// </remarks>
    public class ParaDsDealerFilter
    {
        /// <summary>
        /// 分销商名称
        /// </summary>
        public string DealerName { get; set; }

        /// <summary>
        /// 状态:启用(1),禁用(0)
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// 分销商等级系统编号
        /// </summary>
        public int? LevelSysNo { get; set; }

        /// <summary>
        /// 系统用户系统编号
        /// </summary>
        public int? UserSysNo { get; set; }

        /// <summary>
        /// 商品系统编号（因特殊价格和分销商关联，要排除已关联的分销商）
        /// </summary>
        public int? ProductSysNo { get; set; }
        /// <summary>
        /// AppID
        /// </summary>
        public string AppID { get; set; }
        /// <summary>
        /// AppSecret
        /// </summary>
        public string AppSecret { get; set; }        /// <summary>
        /// 微信公众账号
        /// </summary>
        public string WeiXinNum { get; set; }        /// <summary>
        /// 域名
        /// </summary>
        public string DomainName { get; set; }
        /// <summary>
        /// 分销商
        /// </summary>
        public int DealerSysNo { get; set; }
        /// <summary>
        /// 分销商编号集合
        /// </summary>
        public List<int> DealerSysNoList { get; set; }
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

      
    }
}
