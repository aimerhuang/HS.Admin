using Hyt.BLL.Log;
using Hyt.BLL.Promotion;
using Hyt.BLL.Warehouse;
using Hyt.BLL.Web;
using Hyt.DataAccess.Promotion;
using Hyt.DataAccess.Purchase;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Util;
using NPOI.SS.Formula.Functions;
using PanGu.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Hyt.BLL.Purchase
{
    /// <summary>
    /// 采购单
    /// </summary>
    /// <remarks>2016-6-15 杨浩 创建</remarks>
    public class PrPurchaseBo : BOBase<PrPurchaseBo>
    {
        /// <summary>
        /// 采购单作废后，更采购单信息
        /// </summary>
        /// <param name="inModel">入库单</param>
        /// <param name="user">操作用户</param>
        /// <returns></returns>
        /// <remarks>2016-06-20 杨浩 创建</remarks>
        public void PrInStockCancelCallBack(WhStockIn inModel, SyUser user)
        {
            if (inModel == null)
            {
                throw new Exception("入库单信息不存在");
            }
            if (inModel.SourceType != (int)WarehouseStatus.入库单据类型.采购单)
            {
                return;
            }
            var PR= IPrPurchaseDao.Instance.GetPrPurchaseInfo(inModel.SourceSysNO);//采购单
            if (PR.Status != (int)PurchaseStatus.采购单状态.待入库)
            {
                throw new Exception("采购单已经完成了入库，不能作废");
            }
       
            PR.Status = (int)PurchaseStatus.采购单状态.待审核;
            IPrPurchaseDao.Instance.UpdateStatus(PR.SysNo,0,PR.Status);                   
        }
        /// <summary>
        /// 审核采购单
        /// </summary>
        /// <param name="sysNo">采购单编号</param>
        /// <remarks>2016-6-18 杨浩 创建</remarks>
        public bool AuditPurchase(int sysNo)
        {
            var purchase = IPrPurchaseDao.Instance.GetPrPurchaseInfo(sysNo);
            if (purchase == null)
                return false;
            if (purchase.Status != (int)PurchaseStatus.采购单状态.待审核)
                return false;

            if (UpdateStatus(sysNo, 0, (int)PurchaseStatus.采购单状态.待入库))
            {
                var paymentVoucherMod = new FnPaymentVoucher();
                paymentVoucherMod.CreatedBy = BLL.Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo;
                paymentVoucherMod.CreatedDate = DateTime.Now;
                paymentVoucherMod.CustomerSysNo = 0;
                paymentVoucherMod.LastUpdateBy = paymentVoucherMod.CreatedBy;
                paymentVoucherMod.LastUpdateDate = DateTime.Now;
                paymentVoucherMod.PayableAmount = purchase.TotalMoney;
                paymentVoucherMod.PayDate = new DateTime(1753, 1, 1);
                paymentVoucherMod.PayerSysNo = 0;
                paymentVoucherMod.RefundAccount = "";
                paymentVoucherMod.RefundAccountName = "";
                paymentVoucherMod.RefundBank = "";
                paymentVoucherMod.Source = (int)Hyt.Model.WorkflowStatus.FinanceStatus.付款来源类型.采购单;
                paymentVoucherMod.SourceSysNo = sysNo;
                paymentVoucherMod.Status = (int)Hyt.Model.WorkflowStatus.FinanceStatus.付款单状态.待付款;
                paymentVoucherMod.Remarks = "";
                BLL.Finance.FinanceBo.Instance.CreatePaymentVoucher(paymentVoucherMod);

                //创建入库单
                WhStockIn inEntity = new WhStockIn
                {
                    CreatedBy = paymentVoucherMod.CreatedBy,
                    CreatedDate = DateTime.Now,
                    DeliveryType = 20,
                    IsPrinted = (int)WarehouseStatus.是否已经打印拣货单.否,
                    SourceSysNO = sysNo,
                    SourceType = (int)WarehouseStatus.入库单据类型.采购单,
                    Status = (int)WarehouseStatus.入库单状态.待入库,
                    TransactionSysNo = purchase.PurchaseCode,
                    WarehouseSysNo = purchase.WarehouseSysNo,
                    Stamp = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                    LastUpdateBy = paymentVoucherMod.CreatedBy,
                    LastUpdateDate = DateTime.Now
                };

                inEntity.ItemList = new List<WhStockInItem>();
                var purchaseDetails = BLL.Purchase.PrPurchaseDetailsBo.Instance.GetPurchaseDetailsList(sysNo);
                //入库明细
                foreach (var item in purchaseDetails)
                {
                    inEntity.ItemList.Add(new WhStockInItem
                    {
                        CreatedBy = paymentVoucherMod.CreatedBy,
                        CreatedDate = DateTime.Now,
                        ProductName = item.ProductName,
                        ProductSysNo = item.ProductSysNo,
                        StockInQuantity = item.Quantity,
                        RealStockInQuantity = 0,
                        LastUpdateBy = paymentVoucherMod.CreatedBy,
                        LastUpdateDate = DateTime.Now,
                        SourceItemSysNo = item.SysNo //记录入库单明细来源单号（采购单明细编号)
                    });

                }

                var inSysNo = InStockBo.Instance.CreateStockIn(inEntity); //保存入库单数据      
            }
            return true;
        }
        /// <summary>
        /// 添加采购单
        /// </summary>
        /// <param name="purchase">采购单实体类对象</param>
        /// <returns></returns>
        public void AddPurchase(PrPurchase purchase)
        {
            var sysNo = 0;
            purchase.Quantity = purchase.PurchaseDetails.Sum(x => x.Quantity);
            purchase.TotalMoney = purchase.PurchaseDetails.Sum(x => x.Quantity * x.Money);
            if (purchase.SysNo > 0)
            {
                IPrPurchaseDao.Instance.UpdatePurchase(purchase);
                sysNo = purchase.SysNo;
            }            
            else
                sysNo=IPrPurchaseDao.Instance.AddPurchase(purchase);

            if (sysNo > 0)
            {
                foreach (var item in purchase.PurchaseDetails)
                {
                    item.PurchaseSysNo = sysNo;
                    item.TotalMoney = item.Quantity * item.Money;
                    item.EnterQuantity = 0;
                    if(item.SysNo<=0)
                        IPrPurchaseDetailsDao.Instance.AddPurchaseDetails(item);
                    else
                        IPrPurchaseDetailsDao.Instance.UpdatePurchaseDetails(item);
                }
            }
          
        }
        /// <summary>
        /// 更新采购单
        /// </summary>
        /// <param name="purchase">采购单实体类对象</param>
        /// <returns></returns>
        public int UpdatePurchase(PrPurchase purchase)
        {
            return IPrPurchaseDao.Instance.UpdatePurchase(purchase);    
        }
        /// <summary>
        /// 获取采购单详情
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns></returns>
        /// <remarks>2016-6-18 杨浩 创建</remarks>
        public PrPurchase GetPrPurchaseInfo(int sysNo)
        {
            var purchase=IPrPurchaseDao.Instance.GetPrPurchaseInfo(sysNo);
            purchase.PurchaseDetails = IPrPurchaseDetailsDao.Instance.GetPurchaseDetailsList(sysNo);
            return purchase;
        }
        /// <summary>
        /// 获取采购单名称
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>仓库名称</returns>
        /// <remarks>2016-6-18 王耀发 创建</remarks>
        public string GetPurchaseCode(int sysNo)
        {
            var purchase = IPrPurchaseDao.Instance.GetPrPurchaseInfo(sysNo);
            return purchase == null ? "未知采购单" : purchase.PurchaseCode;
        }
        /// <summary>
        /// 删除采购单
        /// </summary>
        /// <param name="sysNos">采购单系统编号(多个已‘,'分隔)</param>
        /// <remarks>2016-6-20 杨浩 创建</remarks>
        public void DeletePrPurchase(string sysNos)
        {
            IPrPurchaseDetailsDao.Instance.DeleteByPurchaseSysNos(sysNos);
            IPrPurchaseDao.Instance.Delete(sysNos); 
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns></returns>
        /// <remarks>2014-01-08  杨浩 创建</remarks>
        public void Delete(int sysNo)
        {
           IPrPurchaseDao.Instance.Delete(sysNo);  
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sysNos"></param>
        /// <remarks>2016-6-18 杨浩 创建</remarks>
        public  void Delete(string sysNos)
        {
            IPrPurchaseDao.Instance.Delete(sysNos);  
        }
        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="sysNo">采购单系统编号</param>
        /// <param name="statusType">状态类型（0：状态，1：付款状态，2：入库状态）</param>
        /// <param name="status"></param>
        /// <returns></returns>
        public  bool UpdateStatus(int sysNo, int statusType, int status)
        {
           return IPrPurchaseDao.Instance.UpdateStatus(sysNo,statusType,status);  
        }
        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="sysNo">采购单系统编号</param>
        /// <param name="paymentStatus">支付状态</param>
        /// <param name="warehousingStatus">入库状态</param>
        /// <param name="status">采购单状态</param>
        /// <returns></returns>
        /// <remarks>2016-6-21 杨浩 创建</remarks>
        public  bool UpdateStatus(int sysNo, int paymentStatus, int warehousingStatus, int status)
        {
            return IPrPurchaseDao.Instance.UpdateStatus(sysNo,paymentStatus,warehousingStatus,status);  
        }
        /// <summary>
        /// 查询采购单
        /// </summary>
        /// <param name="para">查询参数</param>
        /// <returns></returns>
        public PagedList<CBPrPurchase> QueryPrPurchase(ParaPrPurchaseFilter para,int pageSize=10)
        {
            PagedList<CBPrPurchase> model = null;
            if (para != null)
            {
                model = new PagedList<CBPrPurchase>();
                model.PageSize = pageSize;
                var pager = IPrPurchaseDao.Instance.QueryPrPurchase(para);
                if (null != pager)
                {
                    model.TData = pager.Rows;
                    model.TotalItemCount = pager.TotalRows;
                    model.CurrentPageIndex = para.Id;
                }
            }
            return model;  
        }
        /// <summary>
        /// 更新采购单已入库数
        /// </summary>
        /// <param name="sysNo">采购单系统编号</param>
        /// <param name="enterQuantity">已入库数</param>
        /// <returns></returns>
        /// <remarks>2016-6-21 杨浩 创建</remarks>
        public  bool UpdateEnterQuantity(int sysNo, int enterQuantity)
        {
            return IPrPurchaseDao.Instance.UpdateEnterQuantity(sysNo, enterQuantity);  
        }

        public PrPurchase GetPrRePurchaseInfo(int sysNo)
        {
            var purchase = IPrPurchaseDao.Instance.GetPrPurchaseInfo(sysNo);
            purchase.PurchaseDetails = IPrPurchaseDetailsDao.Instance.GetRePurchaseDetailsList(sysNo);
            return purchase;
        }


        public void ExportPurchaseData(IList<CBPrPurchaseDetails> list, string userIp, int operatorSysno)
        {
            try
            {
                // 查询商品
                List<CBPrPurchaseDetailsOutput> outputData = new List<CBPrPurchaseDetailsOutput>();
                foreach(var mod in list)
                {
                    CBPrPurchaseDetailsOutput tempdata = outputData.Find(p => p.采购单代码 == mod.PurchaseCode);
                    if(tempdata==null)
                    {
                        CBPrPurchaseDetailsOutput output = new CBPrPurchaseDetailsOutput()
                        {
                            采购单代码 = mod.PurchaseCode,
                            备注 = mod.Remarks,
                            采购数量 = mod.Quantity.ToString(),
                            采购总额 = mod.TotalMoney.ToString(),
                            仓库 = mod.BackWarehouseName,
                            创建时间 = mod.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss"),
                            付款状态 = ((PurchaseStatus.采购单付款状态)mod.PaymentStatus).ToString(),
                            供应商 = mod.FName,
                            商品备注 = mod.ProductRemarks,
                            商品编码 = mod.ErpCode,
                            商品单价 = mod.ProMoney,
                            商品名称 = mod.ProductName,
                            商品数量 = mod.ProQuantity,
                            商品已入库数量 = mod.ProEnterQuantity,
                            已入库数量 = mod.EnterQuantity.ToString(),
                            状态 = ((PurchaseStatus.采购单状态)mod.Status).ToString(),
                            商品总金额 = mod.ProTotalMoney

                        };
                        outputData.Add(output);
                    }
                    else
                    {
                        CBPrPurchaseDetailsOutput output = new CBPrPurchaseDetailsOutput()
                        {
                           
                            商品备注 = mod.ProductRemarks,
                            商品编码 = mod.ErpCode,
                            一级分类=mod.OneCategory,
                            二级分类 = mod.SecondCategory,
                            会员价=mod.Price,
                            商品单价 = mod.ProMoney,
                            商品名称 = mod.ProductName,
                            商品数量 = mod.ProQuantity,
                            商品已入库数量 = mod.ProEnterQuantity,
                            商品总金额 = mod.ProTotalMoney

                        };
                        outputData.Add(output);
                    }
                }
                var fileName = string.Format("商品({0})", DateTime.Now.ToString("yyyyMMddHHmmss"));

                /*
                 * 仓库名称
                 * 商品编码
                 * 后台显示名称
                 * 条形码
                 * 海关备案号
                 * 采购价格
                 * 库存数量
                 */

                //导出Excel，并设置表头列名
                Util.ExcelUtil.Export<CBPrPurchaseDetailsOutput>(outputData,
                    new List<string> { "采购单代码", "供应商", "仓库", "采购数量", "已入库数量", "采购总额",
                                       "创建时间",  "备注", "付款状态", "状态", "商品编码",
                                       "商品名称","一级分类","二级分类","会员价", "商品数量", "已入库数量", "商品单价", "商品总金额", "商品备注"},
                    fileName);

                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "商品导出excel",
                                         LogStatus.系统日志目标类型.ExcelExporting, 0, null, userIp, operatorSysno);
            }
            catch (Exception ex)
            {

                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "商品导出excel",
                                         LogStatus.系统日志目标类型.ExcelExporting, 0, ex, userIp, operatorSysno);

            }
        }

        public List<CBPrPurchaseDetails> QueryPrPurchaseByOrderDetail(ParaPrPurchaseFilter para)
        {
            return IPrPurchaseDao.Instance.QueryPrPurchaseByOrderDetail(para);
        }


        #region 采购单商品导入Excel 2017-07-03 吴琨 创建
        public Resuldt ImportExcel(System.IO.Stream stream, int SysNo)
        {
            DataTable dt = null;
            var cols = DicColsMapping.Select(p => p.Value).ToArray();
            try
            {
                dt = ExcelUtil.ImportExcel(stream,cols);
            }
            catch (Exception ex)
            {
                return new Resuldt
                {
                    Message = string.Format("数据导入错误,请选择正确的excel文件"),
                    Status = false
                };
            }
            if (dt == null)
            {
                //not all the cols mapped
                return new Resuldt
                {
                    Message = string.Format("请选择正确的excel文件!"),
                    Status = false
                };
            }

            if (dt.Rows.Count == 0)
            {
                return new Resuldt
                {
                    Message = "导入的数据为空!",
                    Status = false
                };
            }

            Resuldt run = new Resuldt();
            List<CBSimplePdProduct> listModel = new List<CBSimplePdProduct>();
            int fail = 0;//失败记录数
            int success = 0; //成功记录数
            string failstr=""; //失败条数记录
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                CBSimplePdProduct model = PdProductBo.Instance.GetProductErpCode(dt.Rows[i]["产品编码"].ToString(), dt.Rows[i]["条形码"].ToString());
                if (model == null)
                {
                    fail++;
                    failstr+=(i+2)+"、";
                    dt.Rows.Remove(dt.Rows[i]);
                }
                else
                {
                    success++;
                    listModel.Add(model);
                }
            }
            if (success > 0 && dt.Rows.Count > 0) run.Data = dt;
            if (success > 0 && listModel != null) run.listModel = listModel;
            run.Message = "导入成功" + success + "件商品,失败" + fail + "件商品;";
            if (fail > 0) run.Message += "失败原因为:产品编码有误,不存在此件商品。失败条数为第" + failstr.Trim('、') + "条。";
            run.Status = true;
            return  run;
        }

        /// <summary>
        /// import cols mapping between DB and excel
        /// </summary>
        /// <remarks>2017-07-03 吴琨 创建</remarks>
        private static readonly Dictionary<string, string> DicColsMapping = new Dictionary<string, string>
        {
                {"ErpCode", "产品编码"},
                {"Barcode", "条形码"},
                {"ProductName", "产品名称"},
                {"Quantity", "采购数量"},
                {"Money", "采购单价"},
                {"Remarks", "备注"}
        };

        #endregion


        /// <summary>
        /// 获取采购单明细
        /// </summary>
        /// <param name="purchaseSysno">采购单系统编号</param>
        /// <returns></returns>
        /// <remarks>2018-01-04 杨浩 创建</remarks>
        public IList<PrPurchaseDetails> GetPurchaseDetailsByPurchaseSysNo(int purchaseSysno)
        {
            return IPrPurchaseDao.Instance.GetPurchaseDetailsByPurchaseSysNo(purchaseSysno);
        }
    }
}
