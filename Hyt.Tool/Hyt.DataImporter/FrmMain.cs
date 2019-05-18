using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Hyt.DataImporter.TaskThread;

namespace Hyt.DataImporter
{
    public partial class FrmMain : Form
    {
      
        private bool locker=false;
        
        private TaskBeginHandler taskBeginHandler ;
        private TaskGoingHandler taskGoingHandler;

        public FrmMain()
        {
            InitializeComponent();

            taskBeginHandler = new TaskBeginHandler(OnTaskBegin);
            taskGoingHandler = new TaskGoingHandler(OnTaskGoing);
            
            InitTaskLit();
        }

        public List<BaseTaskThread> list=new List<BaseTaskThread>();
        public void InitTaskLit()
        {
            //list.Add(new SyUserTaskThread(taskBeginHandler, taskGoingHandler));
            //list.Add(new SyPermissionTaskThread(taskBeginHandler, taskGoingHandler));
            //list.Add(new SyUserGroupTaskThread(taskBeginHandler, taskGoingHandler));
            //list.Add(new SyGroupUserTaskThread(taskBeginHandler, taskGoingHandler));
            //list.Add(new SyMenuTaskThread(taskBeginHandler, taskGoingHandler));
            
            //list.Add(new BsAreaTaskThread(taskBeginHandler, taskGoingHandler));
            //list.Add(new BsDeliveryPaymentTaskThread(taskBeginHandler, taskGoingHandler));
            //list.Add(new BsPaymentTypeTaskThread(taskBeginHandler, taskGoingHandler));
            //list.Add(new BsDeliveryPaymentTaskThread(taskBeginHandler, taskGoingHandler));

            //list.Add(new CrCustomerQuestionTaskThread(taskBeginHandler, taskGoingHandler));
            //list.Add(new CrCustomerTaskThread(taskBeginHandler, taskGoingHandler));
            //list.Add(new CrReceiveAddressTaskThread(taskBeginHandler, taskGoingHandler));
            //list.Add(new CrCustomerLevelTaskThread(taskBeginHandler, taskGoingHandler));
            

            //list.Add(new FeArticleCategoryTaskThread(taskBeginHandler, taskGoingHandler));
            //list.Add(new FeArticleTaskThread(taskBeginHandler, taskGoingHandler));
            //list.Add(new FeCommentSupportTaskThread(taskBeginHandler, taskGoingHandler));
            //list.Add(new FeProductCommentImageTaskThread(taskBeginHandler, taskGoingHandler));
            //list.Add(new FeProductCommentReplyTaskThread(taskBeginHandler, taskGoingHandler));
            //list.Add(new FnInvoiceTypeTaskThread(taskBeginHandler, taskGoingHandler));

            /*  */
            //list.Add(new LgDeliveryItemTaskThread(taskBeginHandler, taskGoingHandler));
            //list.Add(new LgDeliveryScopeTaskThread(taskBeginHandler, taskGoingHandler));
            //list.Add(new LgDeliveryTaskThread(taskBeginHandler, taskGoingHandler));
            //list.Add(new LgDeliveryTypeTaskThread(taskBeginHandler, taskGoingHandler));
            //list.Add(new LgSettlementTaskThread(taskBeginHandler, taskGoingHandler));
            //list.Add(new LgSettlementItemTaskThread(taskBeginHandler, taskGoingHandler));
            //list.Add(new LgPickupTypeTaskThread(taskBeginHandler, taskGoingHandler));

            //list.Add(new PdAttributeGroupAssociationTaskThread(taskBeginHandler, taskGoingHandler));
            //list.Add(new PdAttributeGroupTaskThread(taskBeginHandler, taskGoingHandler));
            //list.Add(new PdAttributeOptionTaskThread(taskBeginHandler, taskGoingHandler));
            //list.Add(new PdAttributeTaskThread(taskBeginHandler, taskGoingHandler));
            //list.Add(new PdBrandTaskThread(taskBeginHandler, taskGoingHandler));
            //list.Add(new PdCatAttributeGroupAsoTaskThread(taskBeginHandler, taskGoingHandler));
            //list.Add(new PdCategoryAssociationTaskThread(taskBeginHandler, taskGoingHandler));
            list.Add(new PdCategoryTaskThread(taskBeginHandler, taskGoingHandler));
            //list.Add(new PdPriceTaskThread(taskBeginHandler, taskGoingHandler));
            //list.Add(new PdProductAssociationTaskThread(taskBeginHandler, taskGoingHandler));
            //list.Add(new PdProductAttributeTaskThread(taskBeginHandler, taskGoingHandler));
            //list.Add(new PdProductTaskThread(taskBeginHandler, taskGoingHandler));

            //list.Add(new SoOrderItemTaskThread(taskBeginHandler, taskGoingHandler));
            //list.Add(new SoOrderTaskThread(taskBeginHandler, taskGoingHandler));
                       
            //list.Add(new WhstockOutItemTaskThread(taskBeginHandler, taskGoingHandler));
            //list.Add(new WhStockOutTaskThread(taskBeginHandler, taskGoingHandler));
            //list.Add(new WhWarehouseTaskThread(taskBeginHandler, taskGoingHandler));
            //list.Add(new SyUserWarehouseTaskThread(taskBeginHandler, taskGoingHandler));
            
            
            list.OrderBy(model => model.order);
        }

        public void OnTaskBegin(string name,int total)
        {
            if (TxtMessage.InvokeRequired)
            {
                Invoke(new TaskBeginHandler(OnTaskBegin), new object[] { name, total });
            }
            else
            {
                TxtMessage.Text += "导入[" + name + "]\r\n";
            }

            locker = total == 0 ? false : true;
        }

        public void OnTaskGoing(string name, int total,int progress)
        {
            if (TxtMessage.InvokeRequired)
            {
                Invoke(new TaskGoingHandler(OnTaskGoing), new object[] { name, total, progress});
            }
            else
            {
                TxtMessage.Text += "导入进度：" + progress + "/" + total + "\r\n";
            }

            locker = progress == total ? false : true;
        }

        private void BtnBeginImport_Click(object sender, EventArgs e)
        {
            foreach (BaseTaskThread thread in list)
            {
                thread.Run();
                
                while (locker)
                {
                    Thread.Sleep(100);
                }
            }
        }
    }
}
