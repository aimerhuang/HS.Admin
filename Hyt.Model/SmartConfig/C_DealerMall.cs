using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.SmartConfig
{
    /// <summary>
    /// 分销商商城配置
    /// </summary>
    /// <remarks>
    /// 约定命名规则为 前缀“C_ ”+ “业务表名”
    /// 这种方式不太符合规范但是好用，配合VS智能提示写代码方便
    /// 2014-08-20 杨文兵 创建
    /// </remarks>
    public class C_DealerMall
    {

        public C_DealerMall()
        {
            //在构造函数中设置配置项的默认值
            EnabledAutoConfirmReceipt = true;
            EnabledAddGift = false;
        }
        /// <summary>
        /// 升舱时自动确认收款单
        /// </summary>
        public bool EnabledAutoConfirmReceipt { get; set; }
        /// <summary>
        /// 添加赠品
        /// </summary>
        public bool EnabledAddGift { get; set; }
    }
}
