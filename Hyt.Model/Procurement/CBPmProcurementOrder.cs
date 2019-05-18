using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Procurement
{
    public class CBPmProcurementOrder : PmProcurementOrder
    {
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateName { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public string UpdateName { get; set; }

        public List<CBPmProcurementOrderItem> orderItemList = new List<CBPmProcurementOrderItem>();
        public List<CBPmProcurementWebPrice> webPriceList = new List<CBPmProcurementWebPrice>();
        public List<CBPmPointsOrder> pointOrderList = new List<CBPmPointsOrder>();
    }
}
