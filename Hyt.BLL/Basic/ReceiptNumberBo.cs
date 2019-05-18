using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.Basic
{
    /// <summary>
    /// 单据编号
    /// </summary>
    /// <remarks>2016-1-1 杨浩 创建</remarks>
    public class ReceiptNumberBo : BOBase<ReceiptNumberBo>
    {
        /// <summary>
        /// 获取编号
        /// </summary>
        /// <param name="type">编号类型</param>
        /// <param name="code">编号前缀</param>
        /// <returns></returns>
        /// <remarks>2016-1-1 杨浩 创建</remarks>
        public string GetNumber(int type, string code)
        {
            return Hyt.DataAccess.Basic.IReceiptNumberDao.Instance.GetNumber(type, code);
        }
        /// <summary>
        /// 获取得海关报文序列号
        /// </summary>
        /// <returns></returns>
        /// <remarks>2016-1-1 杨浩 创建</remarks>
        public string GetCustomsOrderNo()
        {
            return GetNumber(0, "");
        }
        /// <summary>
        /// 获取门店订单编号
        /// </summary>
        /// <returns></returns>
        /// <remarks>2016-5-25 杨浩 创建</remarks>
        public string GetStoresOrderNo()
        {
            return GetNumber(2, "ST");
        }
        /// <summary>
        /// 获取销售订单编号
        /// </summary>
        /// <returns></returns>
        /// <remarks>2016-5-25 杨浩 创建</remarks>
        public string GetOrderNo()
        {
            return GetNumber(1, "So");
        }
        /// <summary>
        /// 获取采购单编号
        /// </summary>
        /// <returns></returns>
        /// <remarks>2016-6-17 杨浩 创建</remarks>
        public string GetPurchaseNo()
        {
            return GetNumber(5, "PO");
        }

        /// <summary>
        /// 获取库存盘点单编号
        /// </summary>
        /// <returns></returns>
        /// <remarks>2016-06-21 陈海裕 创建</remarks>
        public string GetCheckOrderNo()
        {
            return GetNumber(6, "PD" + DateTime.Now.ToString("yyyyMMdd"));
        }

        /// <summary>
        /// 获取采购退换出库编号
        /// </summary>
        /// <returns></returns>
        /// <remarks>2016-06-23 杨浩 创建</remarks>
        public string GetPurchaseOutNo()
        {
            return GetNumber(7, "CKDB" + DateTime.Now.ToString("yyyyMMdd"));
        }

        /// <summary>
        /// 获取库存调拨单编号
        /// </summary>
        /// <returns></returns>
        /// <remarks>2016-06-29 陈海裕 创建</remarks>
        public string GetAtAllocationNo()
        {
            return GetNumber(8, "DB" + DateTime.Now.ToString("yyyyMMdd"));
        }

        /// <summary>
        /// 获取库存调拨出库单编号
        /// </summary>
        /// <returns></returns>
        /// <remarks>2016-06-30 陈海裕 创建</remarks>
        public string GetAllocationOutNo()
        {
            return GetNumber(9, "DBCK" + DateTime.Now.ToString("yyyyMMdd"));
        }
    }
}
