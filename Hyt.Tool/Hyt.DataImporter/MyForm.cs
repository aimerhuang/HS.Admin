using System;
using System.Configuration;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Hyt.DataImporter.Task;
using Oracle.DataAccess.Client;
using System.Data.SqlClient;

namespace Hyt.DataImporter
{
    public partial class MyForm : Form
    {
        private int count3 = 0;
        private int count2_1 = 0;
        private int count2_2 = 0;
        private int count2_3 = 0; 
        private int count2_4 = 0;

        public MyForm()
        {
            InitializeComponent();
            backgroundWorker1.WorkerSupportsCancellation = true;
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker2.WorkerSupportsCancellation = true;
            backgroundWorker2.WorkerReportsProgress = true;

            backgroundWorker3.WorkerSupportsCancellation = true;
            backgroundWorker3.WorkerReportsProgress = true;
            backgroundWorker4.WorkerSupportsCancellation = true;
            backgroundWorker4.WorkerReportsProgress = true;
            backgroundWorker5.WorkerSupportsCancellation = true;
            backgroundWorker5.WorkerReportsProgress = true;
        }
            
        private void MyForm_Load(object sender, EventArgs e)
        {
            //using (SqlConnection con2 = new SqlConnection("Data Source=112.74.65.202;Initial Catalog=xingying;Persist Security Info=True;User ID=demo;Password=demo"))
            //{
            //    using (SqlConnection con = new SqlConnection("Data Source=112.74.65.202;Initial Catalog=xingying2;Persist Security Info=True;User ID=demo;Password=demo"))
            //    {
            //        con.Open();
            //        SqlCommand command = new SqlCommand();
            //        command.Connection = con;
            //        command.CommandType = CommandType.Text;
            //        command.CommandText = "select [Status],sysNo from SoOrder";

            //        con2.Open();
            //        using (System.Data.SqlClient.SqlDataAdapter adp = new System.Data.SqlClient.SqlDataAdapter(command))
            //        {
            //            System.Data.DataTable dt = new System.Data.DataTable();
            //            adp.Fill(dt);
            //            foreach (DataRow row in dt.Rows)
            //            {
            //                command.Connection = con2;
            //                command.CommandType = CommandType.Text;
            //                command.CommandText = "update [SoOrder]  set [Status]=" + row["Status"].ToString() + " where SysNo=" + row["sysNo"].ToString();
            //                command.ExecuteNonQuery();
            //            }
            //        }
            //    }
            //}
        }

        #region 三期
        private void btnhyt3_Click(object sender, System.EventArgs e)
        {
            Common.RDS.Clear();
            if (this.backgroundWorker1.IsBusy)
            {
                return;
            }
            btnhyt3.Enabled = false;
            this.txthyt3.Clear();
            this.txthyt3.Text += "***********************3期基础数据*******************" + Environment.NewLine;
            List<BaseTask> listObject = new List<BaseTask>();
            AddBaseTaskObject3ToList(listObject);
            this.backgroundWorker1.RunWorkerAsync(listObject);
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bw = (BackgroundWorker)sender;
            List<BaseTask> listBaseTask = (List<BaseTask>)(e.Argument);
            int i = 0;

            foreach (BaseTask task in listBaseTask)
            {
           
                task.Read();
            }

            if (!bw.CancellationPending)
            {

                foreach (DataTable dtData in Common.RDS.Tables)
                {
                    Common.BulkToOracle(dtData);
                    i += 1;
                    backgroundWorker1.ReportProgress(i * 100 / Common.RDS.Tables.Count, dtData);
                }

            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            DataTable table;
            progressBarHYT3.Value = e.ProgressPercentage;
            table = (DataTable)e.UserState;

            txthyt3.Text += table.TableName + "导入完成，记录数：" + table.Rows.Count.ToString() + Environment.NewLine;
            count3 += 1;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // First, handle the case where an exception was thrown.
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }
            else if (e.Cancelled)
            {
                // Next, handle the case where the user canceled 
                // the operation.
                // Note that due to a race condition in 
                // the DoWork event handler, the Cancelled
                // flag may not have been set, even though
                // CancelAsync was called.

                txthyt3.Text += "Canceled";
            }
            else
            {
                // Finally, handle the case where the operation 
                // succeeded.
                txthyt3.Text += "三期初始数据导入完毕，共计导入表（个）：" + count3 + Environment.NewLine;
                btnhyt3.Enabled = true;
            }

            // Enable the Start button.
            //btnStart.Enabled = true;

            // Disable the Cancel button.
            //btnCancel.Enabled = false;
        }
        #endregion

        #region 二期（1）
        private void btnhyt2_1_Click(object sender, EventArgs e)
        {
            Common.RDS.Clear();
            if (this.backgroundWorker2.IsBusy)
            {
                return;
            }
            btnhyt2_1.Enabled = false;
            this.txthyt3.Text +="***********************2期基础数据*******************" + Environment.NewLine;
            List<BaseTask> listObject = new List<BaseTask>();

            AddBaseTaskObject2ToList1(listObject);

            this.backgroundWorker2.RunWorkerAsync(listObject);
        }
      
        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bw = (BackgroundWorker)sender;
            List<BaseTask> listBaseTask = (List<BaseTask>)(e.Argument);
            int i = 0;

            foreach (BaseTask task in listBaseTask)
            {
                   
                task.Read();
            }

            if (!bw.CancellationPending)
            {

                foreach (DataTable dtData in Common.RDS.Tables)
                {
                    Common.BulkToOracle(dtData);
                    i += 1;
                    backgroundWorker2.ReportProgress(i * 100 / Common.RDS.Tables.Count, dtData);
                }

            }
        }

        private void backgroundWorker2_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            DataTable table;
            progressBarHYT2_1.Value = e.ProgressPercentage;
            table = (DataTable)e.UserState;

            txthyt3.Text += table.TableName + "导入完成，记录数：" + table.Rows.Count.ToString() + Environment.NewLine;
            count2_1 += 1;
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // First, handle the case where an exception was thrown.
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }
            else if (e.Cancelled)
            {
                // Next, handle the case where the user canceled 
                // the operation.
                // Note that due to a race condition in 
                // the DoWork event handler, the Cancelled
                // flag may not have been set, even though
                // CancelAsync was called.

                txthyt3.Text += "Canceled";

            }
            else
            {
                // Finally, handle the case where the operation 
                // succeeded.
                txthyt3.Text += "2期1段初始数据导入完毕，共计导入表（个）：" + count2_1 + Environment.NewLine;
                btnhyt2_1.Enabled = true;
            }

        }
        #endregion

        #region 二期（2）
        private void btnhyt2_2_Click(object sender, EventArgs e)
        {
            Common.RDS.Clear();
            if (this.backgroundWorker3.IsBusy)
            {
                return;
            }
            btnhyt2_2.Enabled = false;
            this.txthyt3.Text += "***********************2期2段*******************" + Environment.NewLine;
            
            List<BaseTask> listObject = new List<BaseTask>();

            AddBaseTaskObject2ToList2(listObject);

            
            this.backgroundWorker3.RunWorkerAsync(listObject);
        }

        private void backgroundWorker3_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bw = (BackgroundWorker)sender;
            List<BaseTask> listBaseTask = (List<BaseTask>)(e.Argument);
            int i = 0;

            foreach (BaseTask task in listBaseTask)
            {

                task.Read();
            }

            if (!bw.CancellationPending)
            {

                foreach (DataTable dtData in Common.RDS.Tables)
                {
                    Common.BulkToOracle(dtData);
                    i += 1;
                    backgroundWorker3.ReportProgress(i * 100 / Common.RDS.Tables.Count, dtData);
                }

            }
        }

        private void backgroundWorker3_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            DataTable table;
            progressBarHYT2_2.Value = e.ProgressPercentage;
            table = (DataTable)e.UserState;

            txthyt3.Text += table.TableName + "导入完成，记录数：" + table.Rows.Count.ToString() + Environment.NewLine;
            count2_2 += 1;
        }

        private void backgroundWorker3_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // First, handle the case where an exception was thrown.
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }
            else if (e.Cancelled)
            {
                // Next, handle the case where the user canceled 
                // the operation.
                // Note that due to a race condition in 
                // the DoWork event handler, the Cancelled
                // flag may not have been set, even though
                // CancelAsync was called.

                txthyt3.Text += "Canceled";

            }
            else
            {
                // Finally, handle the case where the operation 
                // succeeded.
                txthyt3.Text += "2期2段初始数据导入完毕，共计导入表（个）：" + count2_2 + Environment.NewLine;
                btnhyt2_2.Enabled = true;
            }
        }

        #endregion

        #region 二期(3)
        private void btnhyt2_3_Click(object sender, EventArgs e)
        {
            Common.RDS.Clear();
            if (this.backgroundWorker4.IsBusy)
            {
                return;
            }
            btnhyt2_3.Enabled = false;
            this.txthyt3.Text += "***********************2期3段*******************" + Environment.NewLine;
            List<BaseTask> listObject = new List<BaseTask>();

            AddBaseTaskObject2ToList3(listObject);

            this.backgroundWorker4.RunWorkerAsync(listObject);
        }

        private void backgroundWorker4_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bw = (BackgroundWorker)sender;
            List<BaseTask> listBaseTask = (List<BaseTask>)(e.Argument);
            int i = 0;

            foreach (BaseTask task in listBaseTask)
            {

                task.Read();
            }

            if (!bw.CancellationPending)
            {

                foreach (DataTable dtData in Common.RDS.Tables)
                {
                    Common.BulkToOracle(dtData);
                    i += 1;
                    backgroundWorker4.ReportProgress(i * 100 / Common.RDS.Tables.Count, dtData);
                }

            }
        }

        private void backgroundWorker4_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            DataTable table;
            progressBarHYT2_3.Value = e.ProgressPercentage;
            table = (DataTable)e.UserState;

            txthyt3.Text += table.TableName + "导入完成，记录数：" + table.Rows.Count.ToString() + Environment.NewLine;
            count2_3 += 1;
        }

        private void backgroundWorker4_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // First, handle the case where an exception was thrown.
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }
            else if (e.Cancelled)
            {
                // Next, handle the case where the user canceled 
                // the operation.
                // Note that due to a race condition in 
                // the DoWork event handler, the Cancelled
                // flag may not have been set, even though
                // CancelAsync was called.

                txthyt3.Text += "Canceled";

            }
            else
            {
                // Finally, handle the case where the operation 
                // succeeded.
                txthyt3.Text += "2期3段初始数据导入完毕，共计导入表（个）：" + count2_3 + Environment.NewLine;
                btnhyt2_3.Enabled = true;
            }
        }

        #endregion

        #region 二期(4)
        private void btnhyt2_4_Click(object sender, EventArgs e)
        {
            Common.RDS.Clear();
            if (this.backgroundWorker5.IsBusy)
            {
                return;
            }
            btnhyt2_4.Enabled = false;
            this.txthyt3.Text += "***********************2期4段*******************" + Environment.NewLine;
            List<BaseTask> listObject = new List<BaseTask>();

            AddBaseTaskObject2ToList4(listObject);

            this.backgroundWorker5.RunWorkerAsync(listObject);
        }
             
       
        private void backgroundWorker5_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bw = (BackgroundWorker)sender;
            List<BaseTask> listBaseTask = (List<BaseTask>)(e.Argument);
            int i = 0;

            foreach (BaseTask task in listBaseTask)
            {

                task.Read();
            }

            if (!bw.CancellationPending)
            {

                foreach (DataTable dtData in Common.RDS.Tables)
                {
                    Common.BulkToOracle(dtData);
                    i += 1;
                    backgroundWorker5.ReportProgress(i * 100 / Common.RDS.Tables.Count, dtData);
                }

            }
        }

        private void backgroundWorker5_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            DataTable table;
            progressBarHYT2_4.Value = e.ProgressPercentage;
            table = (DataTable)e.UserState;

            txthyt3.Text += table.TableName + "导入完成，记录数：" + table.Rows.Count.ToString() + Environment.NewLine;
            count2_4 += 1;
        }

        private void backgroundWorker5_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // First, handle the case where an exception was thrown.
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }
            else if (e.Cancelled)
            {
                // Next, handle the case where the user canceled 
                // the operation.
                // Note that due to a race condition in 
                // the DoWork event handler, the Cancelled
                // flag may not have been set, even though
                // CancelAsync was called.

                txthyt3.Text += "Canceled";

            }
            else
            {
                // Finally, handle the case where the operation 
                // succeeded.
                txthyt3.Text += "2期4段初始数据导入完毕，共计导入表（个）：" + count2_4;
                btnhyt2_4.Enabled = true;
            }
        }
        #endregion

        /// <summary>
        /// 添加三期基础任务对象
        /// </summary>
        /// <param name="listObject"></param>
        private void AddBaseTaskObject3ToList(List<BaseTask> listObject)
        {

            BaseTask bsDP = new BsDeliveryPayment();
            BaseTask bsPT = new BsPaymentType();
            BaseTask bsCode = new BsCode();
            BaseTask bsOrganization = new BsOrganization();
            BaseTask bsOrganizationWarehouse = new BsOrganizationWarehouse();

            BaseTask dsMallType = new DsMallType();
            BaseTask dsDealerLevel = new DsDealerLevel();

            BaseTask feSearchKeyword = new FeSearchKeyword();
            BaseTask feadvertgroup = new FeAdvertGroup();
            BaseTask feadvertitem = new FeAdvertItem();
            BaseTask feproductgroup = new FeProductGroup();
            BaseTask feproductitem = new FeProductItem();

            BaseTask fnIT = new FnInvoiceType();

            BaseTask lgDT = new LgDeliveryType();
            BaseTask lgPT = new LgPickupType();
            BaseTask lgDeliveryPrintTemplate = new LgDeliveryPrintTemplate();

            BaseTask pdProductStatistics = new PdProductStatistics();

            BaseTask SyUG = new SyUserGroup();
            BaseTask syGU = new SyGroupUser();
            BaseTask syMenu = new SyMenu();
            BaseTask syMenuPrivilege = new SyMenuPrivilege();
            BaseTask syPrivilege = new SyPrivilege();
            BaseTask syRole = new SyRole();
            BaseTask syRoleMenu = new SyRoleMenu();
            BaseTask syRolePrivilege = new SyRolePrivilege();
            BaseTask syPermission = new SyPermission();
            BaseTask syMyMenu = new SyMyMenu();
            BaseTask syTaskConfig = new SyTaskConfig();

            BaseTask spcombo = new SpCombo();
            BaseTask spComboItem = new SpComboItem();
            BaseTask spcopon = new spcoupon();
            BaseTask sqpromotion = new SPpromotion();
            BaseTask spromotiongift = new SPpromotiongift();
            BaseTask sppromotionoverlay = new SPpromotionoverlay();
            BaseTask sppromotionrule = new SPpromotionrule();
            BaseTask sppromotionrulecondition = new SPpromotionrulecondition();
            BaseTask sppromotionrulekeyvalue = new SPpromotionrulekeyvalue();
            
            BaseTask whwdy = new WHwarehouseDeliveryType();
            BaseTask whWPT = new WhWarehousePickupType();

            listObject.Add(bsDP);
            listObject.Add(bsPT);
            listObject.Add(bsCode);
            listObject.Add(bsOrganization);
            listObject.Add(bsOrganizationWarehouse);

            listObject.Add(dsMallType);
            listObject.Add(dsDealerLevel);

            listObject.Add(feSearchKeyword);
            listObject.Add(feadvertgroup);
            listObject.Add(feadvertitem);
            listObject.Add(feproductgroup);
            listObject.Add(feproductitem);

            listObject.Add(fnIT);

            listObject.Add(lgDT);
            listObject.Add(lgPT);
            listObject.Add(lgDeliveryPrintTemplate);

            listObject.Add(pdProductStatistics);

            listObject.Add(spcombo);
            listObject.Add(spComboItem);
            listObject.Add(spcopon);
            listObject.Add(sqpromotion);
            listObject.Add(spromotiongift);
            //listObject.Add(sppromotionoverlay);
            listObject.Add(sppromotionrule);
            listObject.Add(sppromotionrulecondition);
            listObject.Add(sppromotionrulekeyvalue);

            listObject.Add(syGU);
            listObject.Add(SyUG);
            listObject.Add(syMenu);
            listObject.Add(syPermission);

            listObject.Add(syMenuPrivilege);
            listObject.Add(syPrivilege);
            listObject.Add(syRole);
            listObject.Add(syRoleMenu);
            listObject.Add(syRolePrivilege);
            listObject.Add(syMyMenu);
            listObject.Add(syTaskConfig);

            listObject.Add(whwdy);
            listObject.Add(whWPT);

        }

        /// <summary>
        /// 二期基础数据
        /// </summary>
        /// <param name="listObject"></param>
        private void AddBaseTaskObject2ToList1(List<BaseTask> listObject)
        {
            BaseTask bsAT = new BsArea();

            BaseTask crCustomer = new CrCustomer();
            BaseTask crCL = new CrCustomerLevel();
            BaseTask crCQ = new CrCustomerQuestion();
            BaseTask crRA = new CrReceiveAddress();

            BaseTask feArticle = new FeArticle();
            BaseTask feAC = new FeArticleCategory();
            BaseTask feCS = new FeCommentSupport();
            BaseTask fepc = new FeProductComment();
            BaseTask fePCI = new FeProductCommentImage();
            BaseTask fePCR = new FeProductCommentReply();

            BaseTask lgDS = new LgDeliveryScope();
            BaseTask lgDUC = new LgDeliveryUserCredit();

            BaseTask pdAttribute = new PdAttribute();
            BaseTask pdAG = new PdAttributeGroup();
            BaseTask pdAGA = new PdAttributeGroupAssociation();
            BaseTask pdAO = new PdAttributeOption();
            BaseTask pdBrand = new PdBrand();
            BaseTask pdCGA = new PdCatAttributeGroupAso();
            BaseTask pdCategory = new PdCategory();
            BaseTask pdCA = new PdCategoryAssociation();
            BaseTask pdPrice = new PdPrice();
            BaseTask pdProduct = new PdProduct();
            BaseTask pdPA = new PdProductAssociation();
            BaseTask pdProductAttribute = new PdProductAttribute();
            //BaseTask pdPI = new PdProductImage();
            BaseTask pdTemplate = new PdTemplate();

            BaseTask soRA = new SoReceiveAddress();

            BaseTask syUser = new SyUser();
            BaseTask syUW = new SyUserWarehouse();

            BaseTask whwarehouse = new WhWarehouse();
            BaseTask whwa = new WHwarehousearea();

            listObject.Add(bsAT);

            listObject.Add(crCustomer);
            listObject.Add(crCL);
            listObject.Add(crCQ);
            listObject.Add(crRA);

            listObject.Add(feArticle);
            listObject.Add(feAC);
            listObject.Add(feCS);
            listObject.Add(fepc);
            listObject.Add(fePCI);
            listObject.Add(fePCR);

            listObject.Add(lgDS);
            listObject.Add(lgDUC);

            listObject.Add(pdAttribute);
            listObject.Add(pdAG);
            listObject.Add(pdAGA);
            listObject.Add(pdAO);
            listObject.Add(pdBrand);
            listObject.Add(pdCGA);
            listObject.Add(pdCategory);
            listObject.Add(pdCA);
            listObject.Add(pdPrice);
            listObject.Add(pdProduct);
            listObject.Add(pdPA);
            listObject.Add(pdProductAttribute);
            listObject.Add(pdTemplate);
            //listObject.Add(pdPI);

            listObject.Add(soRA);

            listObject.Add(syUW);
            listObject.Add(syUser);

            listObject.Add(whwarehouse);
            listObject.Add(whwa);
        }

        /// <summary>
        /// 二期业务数据1段 财务
        /// </summary>
        /// <param name="listObject"></param>
        private void AddBaseTaskObject2ToList2(List<BaseTask> listObject)
        {

            BaseTask fninvoice = new FnInvoice();

            BaseTask fnpv = new FnReceiptVoucher();
            BaseTask fnReceiptVoucherItem = new FnReceiptVoucherItem();
            BaseTask fnop = new FnOnlinePayment();
            BaseTask fnPaymentVoucher = new FnPaymentVoucher();
            BaseTask fnPaymentVoucherItem = new FnPaymentVoucherItem();

          

            listObject.Add(fninvoice);
            listObject.Add(fnpv);
            listObject.Add(fnReceiptVoucherItem);
            listObject.Add(fnop);
            

            listObject.Add(fnPaymentVoucher);
            listObject.Add(fnPaymentVoucherItem);
        }

        /// <summary>
        /// 二期业务数据2段 订单数据
        /// </summary>
        /// <param name="listObject"></param>
        private void AddBaseTaskObject2ToList3(List<BaseTask> listObject)
        {
         
            BaseTask soOrder = new SoOrder();
            BaseTask soOrderItem = new SoOrderItem();
                      
            listObject.Add(soOrder);
            listObject.Add(soOrderItem);
     
        }

        /// <summary>
        /// 二期业务数据三段 仓库物流
        /// </summary>
        /// <param name="listObject"></param>
        private void AddBaseTaskObject2ToList4(List<BaseTask> listObject)
        {
            //BaseTask bsAT = new BsArea();

            //BaseTask crCustomer = new CrCustomer();
            //BaseTask crCL = new CrCustomerLevel();
            //BaseTask crCQ = new CrCustomerQuestion();
            //BaseTask crRA = new CrReceiveAddress();

            //BaseTask feArticle = new FeArticle();
            //BaseTask feAC = new FeArticleCategory();
            //BaseTask feCS = new FeCommentSupport();
            //BaseTask fepc = new FeProductComment();
            //BaseTask fePCI = new FeProductCommentImage();
            //BaseTask fePCR = new FeProductCommentReply();

            //BaseTask fninvoice = new FnInvoice();

            //BaseTask fnpv = new FnReceiptVoucher();
            //BaseTask fnReceiptVoucherItem = new FnReceiptVoucherItem();
            //BaseTask fnop = new FnOnlinePayment();
            //BaseTask fnPaymentVoucher = new FnPaymentVoucher();
            //BaseTask fnPaymentVoucherItem = new FnPaymentVoucherItem();

            //BaseTask lgDelivery = new LgDelivery();
            //BaseTask lgDI = new LgDeliveryItem();
            //BaseTask lgDS = new LgDeliveryScope();

            //BaseTask lgDUC = new LgDeliveryUserCredit();

            //BaseTask lgsettlement = new LgSettlement();
            //BaseTask lgSI = new LgSettlementItem();

            //BaseTask pdAttribute = new PdAttribute();
            //BaseTask pdAG = new PdAttributeGroup();
            //BaseTask pdAGA = new PdAttributeGroupAssociation();
            //BaseTask pdAO = new PdAttributeOption();
            //BaseTask pdBrand = new PdBrand();
            //BaseTask pdCGA = new PdCatAttributeGroupAso();
            //BaseTask pdCategory = new PdCategory();
            //BaseTask pdCA = new PdCategoryAssociation();
            //BaseTask pdPrice = new PdPrice();
            //BaseTask pdProduct = new PdProduct();
            //BaseTask pdPA = new PdProductAssociation();
            //BaseTask pdProductAttribute = new PdProductAttribute();
            //BaseTask pdPI = new PdProductImage();
            //BaseTask pdTemplate = new PdTemplate();

            //BaseTask soOrder = new SoOrder();
            //BaseTask soOrderItem = new SoOrderItem();
            //BaseTask soRA = new SoReceiveAddress();

            BaseTask lgDelivery = new LgDelivery();
            BaseTask lgDI = new LgDeliveryItem();

            BaseTask lgsettlement = new LgSettlement();
            BaseTask lgSI = new LgSettlementItem();

            BaseTask whStockOut = new WhStockOut();
            BaseTask whStockOutItem = new WhstockOutItem();
          
            BaseTask whstockin = new WhStockIn();
            BaseTask whstockinItem = new WHStockinItem();
            BaseTask rcReturn = new RCReturn();
            BaseTask rcReturnItem = new RcReturnItem();

            //listObject.Add(bsAT);

            ////listObject.Add(bsOrganization);
            //listObject.Add(crCustomer);
            //listObject.Add(crCL);
            //listObject.Add(crCQ);
            //listObject.Add(crRA);

            //listObject.Add(feArticle);
            //listObject.Add(feAC);
            //listObject.Add(feCS);
            //listObject.Add(fePCI);
            //listObject.Add(fePCR);
            //listObject.Add(fepc);

            //listObject.Add(fnpv);
            //listObject.Add(fnReceiptVoucherItem);
            //listObject.Add(fnop);
            //listObject.Add(fninvoice);

            //listObject.Add(fnPaymentVoucher);
            //listObject.Add(fnPaymentVoucherItem);

            //listObject.Add(lgDelivery);
            //listObject.Add(lgDI);
            //listObject.Add(lgDS);
            //listObject.Add(lgsettlement);
            //listObject.Add(lgSI);

            //listObject.Add(pdAttribute);
            //listObject.Add(pdAG);
            //listObject.Add(pdAGA);
            //listObject.Add(pdAO);
            //listObject.Add(pdBrand);
            //listObject.Add(pdCGA);
            //listObject.Add(pdCategory);
            //listObject.Add(pdCA);
            //listObject.Add(pdPrice);
            //listObject.Add(pdProduct);
            //listObject.Add(pdPA);
            //listObject.Add(pdProductAttribute);
            //listObject.Add(pdTemplate);
            //listObject.Add(pdPI);

            //listObject.Add(soOrder);
            //listObject.Add(soOrderItem);
            //listObject.Add(soRA);         

         

            listObject.Add(whStockOut);
            listObject.Add(whStockOutItem);
            listObject.Add(whstockin);
            listObject.Add(whstockinItem);

            listObject.Add(rcReturn);
            listObject.Add(rcReturnItem);

            listObject.Add(lgDelivery);
            listObject.Add(lgDI);

            listObject.Add(lgsettlement);
            listObject.Add(lgSI);

        }

        private void btnSetCategoryCode_Click(object sender, EventArgs e)
        {
         

     

            //using (OracleConnection conn1 = new OracleConnection(ConfigurationManager.AppSettings["OracleConnectionString"].ToString()))
            //{
            //    //OracleTransaction myTransaction = conn_bulkcopy.BeginTransaction();
            //    try
            //    {
            //        conn1.Open();
            //        OracleCommand command = new OracleCommand();
            //        command.Connection = conn1;
            //        command.CommandType = CommandType.StoredProcedure;
            //        command.CommandText = "proc_setadminpassword;";
            //        command.ExecuteNonQuery();
            //        MessageBox.Show("商品分类编码设置成功。");

            //    }
            //    catch (Exception ex)
            //    {
            //        throw ex;

            //    }

            //}
        }
        
        private void btnSetAreaMapping_Click(object sender, EventArgs e)
        {
            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.AppSettings["sqlconnectionstring"].ToString()))
            {
                myConn.Open();
                SqlCommand command = new SqlCommand("ImportData.dbo.proc_生成区域映射表", myConn);
                command.CommandType = CommandType.StoredProcedure;
                command.ExecuteNonQuery();
                MessageBox.Show("成功生成区域映射表【Area_1]","系统提示",MessageBoxButtons.OK);
            }
        }

        private void txthyt3_TextChanged(object sender, EventArgs e)
        {

        }

    }
}
