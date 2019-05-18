using Hyt.BLL.Basic;
using Hyt.Model.WorkflowStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.OrderRule
{
    /// <summary>
    /// 预付订单必须完成支付
    /// </summary>
    /// <remarks>2015-1-21 16:34:31 朱成果 创建</remarks>
    public class Command_预付订单必须完成支付 : ICommand 
    {
        private readonly string rulename = "预付订单必须完成支付";
        /// <summary>
        /// 到付订单必须完成支付
        /// </summary>
        /// <param name="orderData">订单数据</param>
        /// <returns>true或false</returns>
        /// <remarks>2015-1-21 朱成果 创建</remarks>
        public override bool Result(OrderData orderData)
        {
            bool ismath=false;
            if (orderData.Order != null)
            {
                ismath=true;
                var payType = PaymentTypeBo.Instance.GetPaymentTypeFromMemory(orderData.Order.PayTypeSysNo).PaymentType;
                if(payType == (int)BasicStatus.支付方式类型.预付&&orderData.Order.PayStatus != (int)OrderStatus.销售单支付状态.已支付)
                {
                    ismath = false;
                }
            }
            return ismath;
        }

        /// <summary>
        /// 解析命令
        /// </summary>
        /// <param name="command">命令</param>
        /// <returns>命令对象</returns>
        /// <remarks>2014-9-10 余勇 创建</remarks>
        public override ICommand Parse(string command)
        {
            return this.IsContainCommand(rulename, command) ? new Command_预付订单必须完成支付() : null;
        }
    }
}
