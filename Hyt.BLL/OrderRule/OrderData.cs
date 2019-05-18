using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.BLL.Distribution;
using Hyt.BLL.Web;
using Hyt.Model;
using Hyt.Model.Transfer;
using SoOrderBo = Hyt.BLL.Order.SoOrderBo;

namespace Hyt.BLL.OrderRule
{
    /// <summary>
    /// 所有订单业务规则验证需要的数据都从此对象获取
    /// </summary>
    public class OrderData
    {
        private string _warehouseName;

        private string _shopName;

        private CBBsAreaDetail _receivearea;

        private CBBsAreaDetail _warehouseArea ;

        private SoOrder _order;

        public SoOrder Order
        {
            get { return _order; }
            set
            {
                _warehouseName = string.Empty;
                _shopName = string.Empty;
                _receivearea = null;
                _warehouseArea = null;
                _order = value;
            } 
        }

        /// <summary>
        /// 关联任务消息 2015-01-21 朱成果
        /// </summary>
        public SyJobMessage JobMessage
        {
            get;
            set;
        }

        /// <summary>
        /// 手动审单客服编号
        /// </summary>
        public int? AssignTo
        {
            get;
            set;
        }

        public IList<SoOrderItem> OrderItems { get; set; }

        public string WarehouseName
        {
            get
            {
                if (string.IsNullOrEmpty(_warehouseName))
                {
                    int warehouseSysNo = Order.DefaultWarehouseSysNo;
                    if (warehouseSysNo > 0)
                    {
                        _warehouseName = Hyt.BLL.Warehouse.WhWarehouseBo.Instance.GetWarehouseName(warehouseSysNo);
                    }
                }
                return _warehouseName;
            }
        }

        /// <summary>
        /// 订单默认仓库 可能为null
        /// </summary>
        public WhWarehouse Warehouse
        {
            get {
                if (Order.DefaultWarehouseSysNo > 0)
                {
                    return Hyt.BLL.Warehouse.WhWarehouseBo.Instance.GetWarehouseEntity(Order.DefaultWarehouseSysNo);
                }
                else {
                    return null;
                }
            }
        }



        public string ShopName
        {
            get
            {
                if (string.IsNullOrEmpty(_shopName))
                {
                    var dealerMall = DsDealerMallBo.Instance.GetEntity(Order.OrderSourceSysNo);//商城信息
                    if (dealerMall != null)
                    {
                        _shopName = dealerMall.ShopName;
                    }
                }
                return _shopName;
            }
        }

        public CBBsAreaDetail ReceiveArea
        {
            get
            {
                if (_receivearea == null)
                {
                    var receiveAddress = SoOrderBo.Instance.GetOrderReceiveAddress(Order.ReceiveAddressSysNo);
                    _receivearea = BsAreaBo.Instance.GetAreaDetail(receiveAddress.AreaSysNo);
                }
                return _receivearea;
            }
        }

         public CBBsAreaDetail WarehouseArea
        {
            get
            {
                if (_warehouseArea == null)
                {
                    var warehouseAddress = Hyt.BLL.Warehouse.WhWarehouseBo.Instance.GetWarehouse(Order.DefaultWarehouseSysNo);
                    _warehouseArea = Hyt.BLL.Web.BsAreaBo.Instance.GetAreaDetail(warehouseAddress.AreaSysNo);
                }
                return _warehouseArea;
            }
        }
    }
}
