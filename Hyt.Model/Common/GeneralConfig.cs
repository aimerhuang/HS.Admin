using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Common
{
    /// <summary>
    /// 网站全局配置
    /// </summary>
    /// <remarks>2015-10-6 杨浩 创建</remarks>
    public class GeneralConfig : ConfigBase
    {
        /// <summary>
        /// 网站标题
        /// </summary>
        public string WebTitle{get; set;}
        /// <summary>
        /// 当前网站域名
        /// </summary>
        public string Domain{get; set;}
        /// <summary>
        /// 网站ICP信息
        /// </summary>
        public string Icp{get; set;}
        /// <summary>
        /// 是否关闭网站0否1是
        /// </summary>
        public int Closed{get; set;}
        /// <summary>
        /// 关闭网站提示信息
        /// </summary>
        public string ClosedReason{get; set;}
        /// <summary>
        /// Seo标题
        /// </summary>
        public string SeoTitle{get; set;}
        /// <summary>
        /// Seo关键字
        /// </summary>
        public string SeokeyWords{get; set;}
        /// <summary>
        /// Seo描述
        /// </summary>
        public string Seodescription { get; set; }
        /// <summary>
        /// 是否pc端0：否 1：是
        /// </summary>
        public int IsPc { get; set; }
        /// <summary>
        /// 禁用微信接口的店铺Id列表(,店铺Id...店铺Id,)
        /// </summary>
        public string DisableWeiXinApiStoreIds { get; set; }
        /// <summary>
        /// 启用密码进入的店铺Id列表(,店铺Id...店铺Id,)
        /// </summary>
        public string EnablePassStoreIds { get; set; }
        /// <summary>
        /// 是否关闭分销功能：0：开启，1：关闭 (即：默认为开启分销功能)
        /// </summary>
        public int IsSellBusinessClose { get; set; }
        /// <summary>
        /// 减库存标识：1-支付后减库存，0-出库后减库存，
        /// </summary>
        /// <remarks>2016-3-14 刘伟豪 创建</remarks>
        public int ReducedInventory { get; set; }
        /// <summary>
        /// 回滚锁定库存标识：1-订单到期未支付，0-订单作废，
        /// </summary>
        /// <remarks>2017-9-9 罗勤尧 创建</remarks>
        public int LockInventoryRollback { get; set; }
        /// <summary>
        /// 是否启用经销商后台客户（0：不启用 1：启用）
        /// </summary>
        /// <remarks>2016-5-18 杨浩 添加</remarks>
        public int IsDealerCustomer { get; set; }
        /// <summary>
        /// 返利到期天数
        /// </summary>
        /// <remarks>2016-5-31 杨浩 添加</remarks>
        public int OrderRebatesRecordDay { get; set; }
        /// <summary>
        /// 自动确认订单收货到期天数
        /// </summary>
        /// <remarks>2016-5-31 杨浩 添加</remarks>
        public int OrderConfirmationReceiptDay { get; set; }
        /// <summary>
        /// 是否启用经销商城(1:不启用 0:启用)
        /// </summary>
        /// <remarks>2016-7-13 杨云奕 添加</remarks>
        public int IsDealerMall { get; set; }
        /// <summary>
        /// 独立商城 1：是 0否
        /// </summary>
        public int SelfMall { get; set; }
        /// <summary>
        /// 是否启用发送微信模板消息  1是 0否
        /// </summary>
        public int IsSendWeChatTempMessage { get; set; }

        public int pointToCoinRate = 50;
        /// <summary>
        /// 积分抵扣比例
        /// </summary>
        public int PointToCoinRate;
        /// <summary>
        /// 赚取积分不将运费参与计算
        /// </summary>
        public int IsEarnPointWithoutFreight { get; set; }
        /// <summary>
        /// 启用数据库搜索（1:是 0：否）
        /// </summary>
        public int IsSqlSearch { get; set; }
        /// <summary>
        /// WebConfig路径多个以逗号分隔（用于删除站点本地缓存）
        /// </summary>
        public string WebConfigPaths { get; set; }
    }
}