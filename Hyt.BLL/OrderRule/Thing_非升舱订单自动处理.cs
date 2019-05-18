using Hyt.BLL.Log;
using Hyt.BLL.Order;
using Hyt.BLL.Sys;
using Hyt.Model.WorkflowStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace Hyt.BLL.OrderRule
{
    public class Thing_非升舱订单自动处理 : IThing
    {
        private IList<ICommand> _no = new List<ICommand>();
        private IList<ICommand> _yes = new List<ICommand>();

        public Thing_非升舱订单自动处理(string config)
        {
            var model = Newtonsoft.Json.JsonConvert.DeserializeObject<Thing_非升舱订单自动处理Model>(config);
            this._no = OrderEngine.Instance.ParseCommands(model.不自动审核);
            this._yes = OrderEngine.Instance.ParseCommands(model.自动审核);
        }

        /*config 示例
         {不自动审核："订单金额大于50,订单金额大于40 and 商品数量大于10",
            自动审核："XX商城的 AND 订单金额等于XX元的 AND 商品明细为 XXX 的 订单,XX商城 AND 订单金额等于XX元 AND 商品明细XXX，XXX"
         }
         */

        public IList<ICommand> 不自动审核
        {
            get
            {
                return _no;
            }
        }
        public IList<ICommand> 自动审核
        {
            get
            {
                return _yes;
            }
        }

        public void Do(OrderData orderData)
        {
            if (orderData == null || orderData.Order == null||orderData.Order.OrderSource == (int)OrderStatus.销售单来源.分销商升舱) return;//此逻辑不处理升舱订单

            //判断属性不为空
            if (不自动审核 != null)
            {
                foreach (var command in 不自动审核)
                {
                    if (command.Result(orderData) == true)
                    {
                        ThingDo.Instance.AddToJobPool(orderData);
                        return;
                    }
                }
            }
            if (自动审核 != null)
            {
                foreach (var command in 自动审核)
                {
                    if (command.Result(orderData) == true)
                    {
                        ThingDo.Instance.Audit(orderData);
                        return;
                    }
                }
            }
            ThingDo.Instance.AddToJobPool(orderData);
            
        }
    }


    internal class Thing_非升舱订单自动处理Model {
        public string 不自动审核 { get; set; }
        public string 自动审核 { get; set; }
    }

}
