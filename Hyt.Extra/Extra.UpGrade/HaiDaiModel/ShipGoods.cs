using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.UpGrade.HaiDaiModel
{
   public  class ShipGoods
    {
        /// <summary>
        /// 结果代码
        /// </summary>
        public int result { set; get; }
       
        /// <summary>
        /// 消息
        /// </summary>
        public string message { set; get; }
       
        /// <summary>
        /// 结果
        /// </summary>
        public Ship data { set; get; }
    }
    public  class Ship
    {
        /// <summary>
        /// 
        /// </summary>
        public int orderId { set; get; }
    }
}
