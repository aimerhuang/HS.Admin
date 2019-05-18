using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hyt.DataImporter.TaskThread;
using Hyt.Model;
using Hyt.ProductImport;

namespace Hyt.DataImporter.TaskThread
{
    public class SoOrderTaskThread:BaseTaskThread
    {
        logRecord log = new logRecord();
                     
        public SoOrderTaskThread(TaskBeginHandler OnTaskBegin, TaskGoingHandler OnTaskGoing)
            : base(OnTaskBegin, OnTaskGoing)
        {
            Read();
        }

        public override int order
        {
            get { return 1; }
        }
        
        public override string name
        {
            get { return "销售单"; }
        }

        protected override int GetTotal()
        {
            return list.Count;
        }

        private IList<SoOrder> list;

        protected override void Read()
        {
            //基础价格(0) 配送员信用价格(70) 会员等级价格(10)
            string sSql = "select  " +
                                    "SysNo,ReceiveAddressSysNo, TransactionSysNo, CustomerSysNo, DefaultWarehouseSysNo," +
                                    "DeliveryTypeSysNo, PayTypeSysNo, OrderAmount, Freight, DiscountAmount," +
                                    "CouponAmount, CashPay, PointPay, InvoiceSysNo, OrderSource, OrderSourceSysNo," +
                                    "SalesType, SalesSysNo, IsHiddenToCustomer, PayStatus, CustomerMessage, InternalRemarks," +
                                    "DeliveryRemarks, DeliveryTime, ContactBeforeDelivery, Remarks, CancelUserType," +
                                    "OnlineStatus, Status, OrderCreatorSysNo, CreateDate, AuditorSysNo, AuditorDate," +
                                    "CancelUserSysNo, CancelDate, LastUpdateBy, LastUpdateDate " +
                          "from soorder order by sysno ";
             
            list = DataProvider.Instance.Sql(sSql).QueryMany<SoOrder>();
        }

        protected override void Write(int rowIndex)
        {
            try
            {   //p => p.ParentCategory
                DataProvider.OracleInstance.Insert<SoOrder>("SoOrder", list[rowIndex]).AutoMap(p => p.OrderItemList, p => p.OrderInvoice, p => p.Customer, p => p.ReceiveAddress).Execute();
            }
            catch (Exception ex)
            {

                log.CheckLog("导入表【SoOrder】失败，" + "系统号：" + list[rowIndex].SysNo.ToString() + "." + ex.Message);
            }
            
        }
    }
}
