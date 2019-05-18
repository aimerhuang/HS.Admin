using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.UpGrade.HaiDaiModel
{
    /// <summary>
    /// 海带订单详情接口结果集
    /// </summary>
    /// <remarks>2017-6-13 罗勤尧 创建</remarks>
    public class HaiDaiReuliOrderItem
    {
        public int result { set; get; }
        public string data { set; get; }

        public DataDetail OrderInfo { get; set; }
    }
    public class DataDetail
    {
        /// <summary>
        /// 是否异常
        /// </summary>
        public int abnormal { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public int abnormalSource { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string  adminRemark{set;get;}

        /// <summary>
        /// 
        /// </summary>
        public int afterSale { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string afterSaleName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string allocationTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string allocationTimeStr { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string city { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string createTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string createTimeStr { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string dlyCode { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public decimal goodsCost { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string idNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string kuaidiData { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string kuaidiName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string kuaidiNo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int orderId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string paySuccessTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string paySuccessTimeStr { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string province { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string region { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int returnOrderId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string returnStatus { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string shipAddr { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int shipCityId { get; set; }



        /// <summary>
        /// 
        /// </summary>
        public string shipMobile { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal shipMoney { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string shipName { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string shipNo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int shipProvinceId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int shipRegionId { get; set; }




        /// <summary>
        /// 
        /// </summary>
        public string shipTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string shipTimeStr { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int signingTime { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string signingTimeStr { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string sn { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int status { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string statusName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal  tax { get; set; }

        /// <summary>
        /// 明细
        /// </summary>
        public List<orderItemList> orderItemList { get; set; }
    }
}
