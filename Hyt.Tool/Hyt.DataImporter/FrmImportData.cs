using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Hyt.DataImporter.Task;

namespace Hyt.DataImporter
{
    public partial class FrmImportData : Form
    {
        private int count = 0;

        public FrmImportData()
        {
            InitializeComponent();
            backgroundWorker2.WorkerSupportsCancellation = true;
            backgroundWorker2.WorkerReportsProgress = true;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (this.backgroundWorker2.IsBusy)
            {
                return;
            }
            
            this.TxtResult.Clear();
            List<BaseTask> listObject = new List<BaseTask>();
            AddBaseTaskObjectToList(listObject);
            this.backgroundWorker2.RunWorkerAsync(listObject);
            btnStart.Enabled = false;
            btnCancel.Enabled = true;
        }
                
        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bw = (BackgroundWorker)sender;
            List<BaseTask> listBaseTask = (List<BaseTask>)(e.Argument);
            int i = 0;

            foreach (BaseTask task in listBaseTask)
            {
                if (task is PdCategoryAssociation)  
                task.Read();

              // task.Read();
            }
             
            if (!bw.CancellationPending)
            {

                foreach (DataTable dtData in Common.RDS.Tables)
                {
                    Common.BulkToOracle(dtData);
                    i += 1;
                    backgroundWorker2.ReportProgress(i * 100 / Common.RDS.Tables.Count, dtData);
                }
               
                //backgroundWorker2.ReportProgress(i * 100 / Common.RDS.Tables.Count, i);
            }
        }

       private void backgroundWorker2_ProgressChanged(object sender, ProgressChangedEventArgs e)
       {
           DataTable table;
           progressBar1.Value = e.ProgressPercentage;
           table = (DataTable)e.UserState;
           TxtResult.Text += table.TableName + "导入完成，记录数：" + table.Rows.Count.ToString() + Environment.NewLine;
           count += 1;
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

               TxtResult.Text += "Canceled";
           }
           else
           {
               // Finally, handle the case where the operation 
               // succeeded.
               TxtResult.Text += "全部导入完毕，共计导入表（个）：" + count;
           }

           
           // Enable the Start button.
           btnStart.Enabled = true;

           // Disable the Cancel button.
           btnCancel.Enabled = false;

       }

       private void AddBaseTaskObjectToList(List<BaseTask> listObject)
       {
           BaseTask bsAT = new BsArea();
           BaseTask bsDP = new BsDeliveryPayment();
           BaseTask bsPT = new BsPaymentType();
           BaseTask bsCode = new BsCode();
           BaseTask bsOrganization = new BsOrganization();
           BaseTask bsOrganizationWarehouse = new BsOrganizationWarehouse();

           BaseTask crCustomer = new CrCustomer();
           BaseTask crCL = new CrCustomerLevel();
           BaseTask crCQ = new CrCustomerQuestion();
           BaseTask crRA = new CrReceiveAddress();

           BaseTask dsMallType = new DsMallType();
           BaseTask dsDealerLevel = new DsDealerLevel();
           BaseTask feArticle = new FeArticle();
           BaseTask feAC = new FeArticleCategory();
           BaseTask feCS = new FeCommentSupport();
           BaseTask fepc = new FeProductComment();
           BaseTask fePCI = new FeProductCommentImage();
           BaseTask fePCR = new FeProductCommentReply();
           BaseTask feSearchKeyword = new FeSearchKeyword();
           BaseTask feadvertgroup = new FeAdvertGroup();
           BaseTask feadvertitem = new FeAdvertItem();
           BaseTask feproductgroup = new FeProductGroup();
           BaseTask feproductitem = new FeProductItem();

           BaseTask fnIT = new FnInvoiceType();
           BaseTask fninvoice = new FnInvoice();
           
           BaseTask fnpv= new  FnReceiptVoucher();
           BaseTask fnReceiptVoucherItem = new FnReceiptVoucherItem();
           BaseTask fnop = new FnOnlinePayment();
           BaseTask fnPaymentVoucher = new FnPaymentVoucher();
           BaseTask fnPaymentVoucherItem = new FnPaymentVoucherItem();

           BaseTask lgDelivery = new LgDelivery();
           BaseTask lgDI = new LgDeliveryItem();
           BaseTask lgDS = new LgDeliveryScope();
           BaseTask lgDT = new LgDeliveryType();
           BaseTask lgDUC = new LgDeliveryUserCredit();
           BaseTask lgPT = new LgPickupType();
           BaseTask lgsettlement = new LgSettlement();
           BaseTask lgSI = new LgSettlementItem();
           BaseTask lgDeliveryPrintTemplate = new LgDeliveryPrintTemplate();
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
           BaseTask pdPI = new PdProductImage();
           BaseTask pdTemplate = new PdTemplate();
           BaseTask pdProductStatistics = new PdProductStatistics();

           BaseTask soOrder = new SoOrder();
           BaseTask soOrderItem = new SoOrderItem();
           BaseTask soRA = new SoReceiveAddress();
           BaseTask SyUG = new SyUserGroup();
           BaseTask syGU = new SyGroupUser();
           BaseTask syMenu = new SyMenu();
           BaseTask syMenuPrivilege = new SyMenuPrivilege();
           BaseTask syPrivilege = new SyPrivilege();
           BaseTask syRole = new SyRole();
           BaseTask syRoleMenu = new SyRoleMenu();
           BaseTask syRolePrivilege = new SyRolePrivilege();
           BaseTask syPermission = new SyPermission();
           BaseTask syUser = new SyUser();
           BaseTask syUW = new SyUserWarehouse();
           BaseTask syMyMenu = new SyMyMenu();

           BaseTask spcombo = new SpCombo();
           BaseTask spComboItem = new SpComboItem();
           BaseTask spcopon = new spcoupon();
           BaseTask sqpromotion = new SPpromotion();
           BaseTask spromotiongift = new SPpromotiongift();
           BaseTask sppromotionoverlay = new SPpromotionoverlay();
           BaseTask sppromotionrule = new SPpromotionrule();
           BaseTask sppromotionrulecondition = new SPpromotionrulecondition();
           BaseTask sppromotionrulekeyvalue = new SPpromotionrulekeyvalue();
           BaseTask syTaskConfig = new SyTaskConfig();

           BaseTask whStockOut = new WhStockOut();
           BaseTask whStockOutItem = new WhstockOutItem();
           BaseTask whwarehouse = new WhWarehouse();
           BaseTask whwa = new WHwarehousearea();
           BaseTask whwdy = new WHwarehouseDeliveryType();
           BaseTask whWPT = new WhWarehousePickupType(); 
           BaseTask whstockin = new WhStockIn();
           BaseTask whstockinItem = new WHStockinItem();
           BaseTask rcReturn = new RCReturn();
           BaseTask rcReturnItem = new RcReturnItem();

           listObject.Add(bsAT);
           listObject.Add(bsDP);
           listObject.Add(bsPT);
           listObject.Add(bsCode);
           listObject.Add(bsOrganization);
           listObject.Add(bsOrganizationWarehouse);

           listObject.Add(crCustomer);
           listObject.Add(crCL);
           listObject.Add(crCQ);
           listObject.Add(crRA);
           listObject.Add(dsMallType);
           listObject.Add(dsDealerLevel);
           listObject.Add(feArticle);
           listObject.Add(feAC);
           listObject.Add(feCS);
           listObject.Add(fePCI);
           listObject.Add(fePCR);
           listObject.Add(fepc);
           listObject.Add(feSearchKeyword);
           listObject.Add(feadvertgroup);
           listObject.Add(feadvertitem);
           listObject.Add(feproductgroup);
           listObject.Add(feproductitem);
          
           listObject.Add(fnIT);
           listObject.Add(fnpv);
           listObject.Add(fnReceiptVoucherItem);
           listObject.Add(fnop);
           listObject.Add(fninvoice);
          
           listObject.Add(fnPaymentVoucher);
           listObject.Add(fnPaymentVoucherItem);

           listObject.Add(lgDelivery);
           listObject.Add(lgDI);
           listObject.Add(lgDS);
           listObject.Add(lgDT);
           listObject.Add(lgPT);
           listObject.Add(lgsettlement);
           listObject.Add(lgSI);
           listObject.Add(lgDeliveryPrintTemplate);

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
           listObject.Add(pdProductStatistics);

           listObject.Add(soOrder);
           listObject.Add(soOrderItem);

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
           listObject.Add(syUW);
           listObject.Add(SyUG);
           listObject.Add(syMenu);
           listObject.Add(syUser);
           listObject.Add(syPermission);

           listObject.Add(syMenuPrivilege);
           listObject.Add(syPrivilege);
           listObject.Add(syRole);
           listObject.Add(syRoleMenu);
           listObject.Add(syRolePrivilege);
           listObject.Add(syMyMenu);
           listObject.Add(syTaskConfig);

           listObject.Add(whStockOut);
           listObject.Add(whStockOutItem);
           listObject.Add(whwarehouse);
           listObject.Add(whwa);
           listObject.Add(whwdy);
           listObject.Add(whWPT);
           listObject.Add(whstockin);
           listObject.Add(whstockinItem);
           listObject.Add(rcReturn);
           listObject.Add(rcReturnItem);
           listObject.Add(lgDUC);
           listObject.Add(pdPI);
           listObject.Add(soRA);
       }

       private void btnQuit_Click(object sender, EventArgs e)
       {
           Application.Exit();
       }

       private void FrmImportData_Load(object sender, EventArgs e)
       {

       }

   }
}

