using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.UpGrade.GeGeJiaModel
{
    #region 格家订单发货参数

    /// <summary>
    /// 格格家线下发货参数
    /// </summary>
    /// <remarks>2017-08-22 黄杰 创建</remarks>
    public class GeGeJiaDeliverGoods
    {
        /// <summary>
        /// 订单类型，0：渠道订单，1：格格家订单，2：格格团订单，3：格格团全球购订单，4：环球捕手订单，5：燕网订单，6：b2b订单，7：手q，8：云店
        /// </summary>
        public int type { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        public string orderNumber { get; set; }

        /// <summary>
        /// 快递公司名称
        /// </summary>
        public string expressName { get; set; }

        /// <summary>
        /// 快递单号
        /// </summary>
        public string expressNo { get; set; }

    }

    #endregion
}
