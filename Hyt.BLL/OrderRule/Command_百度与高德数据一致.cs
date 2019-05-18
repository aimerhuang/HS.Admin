﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Hyt.BLL.Logistics;
using Hyt.BLL.Order;
using Hyt.BLL.Warehouse;
using Hyt.Model.SystemPredefined;
using Hyt.Model.WorkflowStatus;
using IronPython.Modules;

namespace Hyt.BLL.OrderRule
{
    /// <summary>
    /// 仓库地址匹配地图解析
    /// </summary>
    public class Command_百度与高德数据一致 : ICommand
    {
        private readonly string rulename = "百度与高德数据一致";

        /// <summary>
        /// 返回是否匹配
        /// </summary>
        /// <param name="orderData">订单数据</param>
        /// <returns>true或false</returns>
        /// <remarks>2014-9-10 余勇 创建</remarks>
        public override bool Result(OrderData orderData)
        {
            if (orderData.Order == null || orderData.Order.DeliveryTypeSysNo == 0) return false;
            var orderWarehouse = orderData.Order.DefaultWarehouseSysNo;
            var addressinfo = SoOrderBo.Instance.GetOrderReceiveAddress(orderData.Order.ReceiveAddressSysNo);
            var deliveryType = orderData.Order.DeliveryTypeSysNo;//订单配送方式编号

            if (addressinfo == null)
            {
                return false;
            }
            var address = addressinfo.StreetAddress;//订单街道收货地址
            var baiduMap = LgDeliveryScopeBo.Instance.GetBaiChengInfo(addressinfo.AreaSysNo, address, (int)Hyt.Model.WorkflowStatus.BasicStatus.地图类型.百度地图);//百度配送信息
            var gdMap = LgDeliveryScopeBo.Instance.GetBaiChengInfo(addressinfo.AreaSysNo, address, (int)Hyt.Model.WorkflowStatus.BasicStatus.地图类型.高德地图);//高德配送信息
            if (baiduMap != null && gdMap!=null && baiduMap.IsInScope == gdMap.IsInScope)
            {
                if ((deliveryType == DeliveryType.普通百城当日达) == baiduMap.IsInScope)
                {
                    int wh1 = 0;
                    int wh2 = 0;
                    if (baiduMap.WarehouseNo > 0)
                    {
                        wh1 = baiduMap.WarehouseNo;
                    }
                    else
                    {
                        var warehouse = Hyt.BLL.Warehouse.WhWarehouseBo.Instance.GetWhWareHouse(addressinfo.AreaSysNo, null, baiduMap.IsInScope ? DeliveryType.普通百城当日达 : DeliveryType.第三方快递, WarehouseStatus.仓库状态.启用).FirstOrDefault();
                        if (warehouse != null)
                        {
                            wh1 = warehouse.SysNo;
                        }
                    }
                    if (gdMap.WarehouseNo > 0)
                    {
                        wh2 = gdMap.WarehouseNo;
                    }
                    else
                    {
                        var warehouse = Hyt.BLL.Warehouse.WhWarehouseBo.Instance.GetWhWareHouse(addressinfo.AreaSysNo, null, gdMap.IsInScope ? DeliveryType.普通百城当日达 : DeliveryType.第三方快递, WarehouseStatus.仓库状态.启用).FirstOrDefault();
                        if (warehouse != null)
                        {
                            wh2 = warehouse.SysNo;
                        }
                    }
                    if (wh1 == wh2 && wh1 == orderWarehouse)
                    {
                        return true;
                    }
                }
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
            return this.IsContainCommand(rulename, command) ? new Command_百度与高德数据一致() : null;
        }
    }
}
