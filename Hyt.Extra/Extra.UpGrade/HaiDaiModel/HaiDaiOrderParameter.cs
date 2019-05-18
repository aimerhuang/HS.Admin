using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.UpGrade.HaiDaiModel
{
   
    /// <summary>
    /// 海带订单查询参数
    /// </summary>
    /// <remarks>2017-6-12 罗勤尧 创建</remarks>
    public class HaiDaiOrderParameter
    {
        #region 传入参数
        /// <summary>
        /// 会员id
        /// </summary>
        public int memberid { get; set; }
        /// <summary>
        /// 会员id
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// 订单起始时间
        /// </summary>
        public string payStartTime { get; set; }

        /// <summary>
        /// 订单截止时间
        /// </summary>
        public string payEndTime { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string shipMobile { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string shipName { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string goodsName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string psn { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string sn { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int tradeType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int depotid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string sort { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsDesc { get; set; }

        /// <summary>
        /// 页码
        /// </summary>
        public int  PageNum { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int type { get; set; }

        #endregion
    }
}
