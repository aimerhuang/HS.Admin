using Hyt.BLL.Logistics;
using Hyt.Model.SystemPredefined;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.OrderRule
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>2015/11/4 15:44:30 朱成果 创建</remarks>
    public class Command_高德千城当日达:ICommand
    {
        private readonly string rulename = "高德千城当日达";

        /// <summary>
        /// 返回是否匹配
        /// </summary>
        /// <param name="orderData">订单数据</param>
        /// <returns>true或false</returns>
        /// <remarks>2015-11-4 朱成果 创建</remarks>
        public override bool Result(OrderData orderData)
        {
            if (orderData.Order == null ) return false;
            var addressinfo = Hyt.BLL.Order.SoOrderBo.Instance.GetOrderReceiveAddress(orderData.Order.ReceiveAddressSysNo);
            if (addressinfo == null)
            {
                return false;
            }
            var address = addressinfo.StreetAddress;//订单街道收货地址
            var gdMap = LgDeliveryScopeBo.Instance.GetBaiChengInfo(addressinfo.AreaSysNo, address, (int)Hyt.Model.WorkflowStatus.BasicStatus.地图类型.高德地图);//高德配送信息
            if (gdMap!=null&&gdMap.IsInScope)
            {
                int whgd = -1;//高德默认仓库
                if (gdMap.WarehouseNo > 0)
                {
                    whgd = gdMap.WarehouseNo;
                }
                else
                {
                    var warehouse = Hyt.BLL.Warehouse.WhWarehouseBo.Instance.GetWhWareHouse(addressinfo.AreaSysNo, null, DeliveryType.普通百城当日达,Hyt.Model.WorkflowStatus.WarehouseStatus.仓库状态.启用).FirstOrDefault();
                    if (warehouse != null)
                    {
                        whgd = warehouse.SysNo;
                    }
                }
                orderData.Order.DefaultWarehouseSysNo=whgd;
                orderData.Order.DeliveryTypeSysNo=DeliveryType.普通百城当日达;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 解析命令
        /// </summary>
        /// <param name="command">命令</param>
        /// <returns>命令对象</returns>
        /// <remarks>2014-9-10 余勇 创建</remarks>
        public override ICommand Parse(string command)
        {
            return this.IsContainCommand(rulename, command) ? new Command_高德千城当日达() : null;
        }
    }
}
