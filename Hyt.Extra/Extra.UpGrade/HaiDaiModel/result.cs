using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.UpGrade.HaiDaiModel
{
    /// <summary>
    /// 海带订单接口结果集
    /// </summary>
    /// <remarks>2017-6-13 罗勤尧 创建</remarks>
    public class resultL
    {
        public List<resultList> result { set; get; }

        public int totalPageCount { set; get; }

        public int totalCount { set; get; }

        public int pageSize { set;get;}

        public int currentPageNo { set; get; }
    }
    /// <summary>
    /// 海带订单接口结果集
    /// </summary>
    /// <remarks>2017-6-13 罗勤尧 创建</remarks>
    public class resultList
    {
        /// <summary>
        /// 
        /// </summary>
        public int abnormalStatus { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string abnormalStatusName { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public int has_ship_info { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public decimal goodsCost { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public string kuaidiCode { set; get; }


        /// <summary>
        /// 
        /// </summary>
        public string kuaidiNo { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public int orderId { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public string paySuccessTime { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public string paySuccessTimeLong { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public string shipMobile { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public string shipMoney { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public string shipName { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public string sn { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public string status { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public decimal tax { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public string validTime { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public string validTimeLong { set; get; }

        /// <summary>
        /// 订单明细
        /// </summary>
        public List<orderItemList> orderItemList { set; get; }

    }
}
