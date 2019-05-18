using Hyt.DataAccess.Purchase;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.Purchase
{
    /// <summary>
    /// 采购单
    /// </summary>
    /// <remarks>2016-6-15 杨浩 创建</remarks>
    public class PrPurchaseDaoImpl:IPrPurchaseDao
    {
        /// <summary>
        /// 添加采购单
        /// </summary>
        /// <param name="purchase">采购单实体类对象</param>
        /// <returns></returns>
        public override int AddPurchase(PrPurchase purchase)
        {
            purchase.SysNo = Context.Insert("PrPurchase", purchase)
                                       .AutoMap(o => o.SysNo,o=>o.PurchaseDetails)
                                       .ExecuteReturnLastId<int>("SysNo");
            return purchase.SysNo;
        }
        /// <summary>
        /// 更新采购单
        /// </summary>
        /// <param name="purchase">采购单实体类对象</param>
        /// <returns></returns>
        public override int UpdatePurchase(PrPurchase purchase)
        {
           int rows= Context.Update("PrPurchase", purchase)
                  .AutoMap(o => o.SysNo, o => o.CreatedBy, o => o.CreatedDate, o => o.PaymentStatus, o => o.Status, o => o.WarehousingStatus,o => o.PurchaseDetails)
                  .Where("SysNo", purchase.SysNo)
                  .Execute();
           return rows;
        }
        /// <summary>
        /// 获取采购单详情
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns></returns>
        public override PrPurchase GetPrPurchaseInfo(int sysNo)
        {
            return Context.Sql("select * from PrPurchase where SysNo=@SysNo")
                  .Parameter("SysNo", sysNo)
             .QuerySingle<PrPurchase>();
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns></returns>
        /// <remarks>2014-01-08  杨浩 创建</remarks>
        public override void Delete(int sysNo)
        {
            Context.Sql("Delete from PrPurchase where SysNo=@SysNo")
                 .Parameter("SysNo", sysNo)
            .Execute();
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sysNos"></param>
        /// <remarks>2016-6-18 杨浩 创建</remarks>
        public override void Delete(string sysNos)
        {
            Context.Sql("Delete from PrPurchase where SysNo in("+sysNos+")")              
            .Execute();
        }
        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="sysNo">采购单系统编号</param>
        /// <param name="statusType">状态类型（0：状态，1：付款状态，2：入库状态）</param>
        /// <param name="status"></param>
        /// <returns></returns>
        public override bool UpdateStatus(int sysNo, int statusType, int status)
        {
           string setStr = " set ";
           if (statusType == 0)          
               setStr += "Status="+status;
           else if(statusType==1)
               setStr += "PaymentStatus=" + status;
           else if (statusType == 2)
               setStr += "WarehousingStatus=" + status;

           int rows = Context.Sql("Update PrPurchase "+ setStr+" where SysNo=@SysNo")
               .Parameter("SysNo", sysNo)
            .Execute();
           return rows > 0;
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
        public override bool UpdateStatus(int sysNo, int paymentStatus, int warehousingStatus, int status)
        {
            string setStr = "";
            if (status > 0||status==-10)
                setStr += ",Status=" + status;
            else if (paymentStatus > 0 || paymentStatus==-10)
                setStr += ",PaymentStatus=" + paymentStatus;
            else if (warehousingStatus > 0 || warehousingStatus==-10)
                setStr += ",WarehousingStatus=" + warehousingStatus;

            int rows = Context.Sql("Update PrPurchase  set " + setStr.TrimStart(',')+ " where SysNo=@SysNo")
                .Parameter("SysNo", sysNo)
             .Execute();
            return rows > 0;
        }
        /// <summary>
        /// 查询采购单
        /// </summary>
        /// <param name="para">查询参数</param>
        /// <returns></returns>
        /// <remarks>2016-6-21 杨浩 增加查询条件</remarks>
        public override Pager<CBPrPurchase> QueryPrPurchase(ParaPrPurchaseFilter para)
        {
            var paras = new List<object>();
        
            string whereStr = " where 1=1 ";          
            if (para.WarehouseSysNo > 0)
            {              
                whereStr += " and ph.WarehouseSysNo=@"+paras.Count;
                paras.Add(para.WarehouseSysNo);
            }
            if (!string.IsNullOrEmpty(para.PurchaseCode)&&para.PurchaseCode!="")
            {
                whereStr += " and (ph.PurchaseCode=@" + paras.Count + " or phd.ProductName like '%" + para.PurchaseCode + "%' )";
                paras.Add(para.PurchaseCode);
            }
            if (para.Status !=0)
            {
                whereStr += " and ph.Status=@" + paras.Count;
                paras.Add(para.Status);
            }

            if (para.CreatedDate.HasValue)
            {
                whereStr += " and ph.CreatedDate=@" + paras.Count;
                paras.Add(para.CreatedDate);
            }

            string sql = @"
              (
              select distinct ph.*,wh.BackWarehouseName,wh.WarehouseName,pmf.FName from PrPurchase as ph 
                inner join PrPurchaseDetails as phd on ph.SysNo=phd.PurchaseSysNo
                left join WhWarehouse as wh on wh.SysNo=ph.WarehouseSysNo
                left join PmManufacturer as pmf on pmf.SysNo=ph.ManufacturerSysNo " + whereStr+") tb";

         
            
            var dataList = Context.Select<CBPrPurchase>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(0)").From(sql);
            dataList.Parameters(paras.ToArray());
            dataCount.Parameters(paras.ToArray());
            var pager = new Pager<CBPrPurchase>
            {
                PageSize = para.PageSize,
                CurrentPage = para.Id,
                TotalRows = dataCount.QuerySingle(),
                Rows = dataList.OrderBy("tb.sysNo desc").Paging(para.Id, para.PageSize).QueryMany()
            };
            return pager;
        }
        /// <summary>
        /// 更新采购单已入库数
        /// </summary>
        /// <param name="sysNo">采购单系统编号</param>
        /// <param name="enterQuantity">已入库数</param>
        /// <returns></returns>
        /// <remarks>2016-6-21 杨浩 创建</remarks>
        public override bool UpdateEnterQuantity(int sysNo, int enterQuantity)
        {
            int rows = Context.Sql("Update PrPurchase  set enterQuantity=@enterQuantity where SysNo=@SysNo")
               .Parameter("SysNo", sysNo)
               .Parameter("enterQuantity",enterQuantity)
               .Execute();
            return rows > 0;
        }

        public override List<CBPrPurchaseDetails> QueryPrPurchaseByOrderDetail(ParaPrPurchaseFilter para)
        {
           
            string whereStr = " where 1=1 ";
            if (para.WarehouseSysNo > 0)
            {
                whereStr += " and ph.WarehouseSysNo=" + para.WarehouseSysNo;
                
            }
            if (!string.IsNullOrEmpty(para.PurchaseCode) && para.PurchaseCode != "")
            {
                whereStr += " and (ph.PurchaseCode='" + para.PurchaseCode + "' or phd.ProductName like '%" + para.PurchaseCode + "%' )";
                
            }
            if (para.Status != 0)
            {
                whereStr += " and ph.Status=" + para.Status;
                
            }

            if (para.CreatedDate.HasValue)
            {
                whereStr += " and ph.CreatedDate='" + para.CreatedDate + "'";
                
            }

            string sql = @"
              (
              select  ph.*,wh.BackWarehouseName,wh.WarehouseName,pmf.FName ,
                   phd.ProductName as ProductName, phd.ErpCode as ErpCode,
                    phd.Quantity as ProQuantity,phd.EnterQuantity as ProEnterQuantity,
                    phd.Money as ProMoney , phd.TotalMoney as ProTotalMoney, 
                    phd.Remarks as ProductRemarks ,  a.CategoryName as OneCategory ,b.CategoryName as SecondCategory,c.CategoryName as thridCategory,PdPrice.Price
                from PrPurchase as ph 
                inner join PrPurchaseDetails as phd on ph.SysNo=phd.PurchaseSysNo
                left join WhWarehouse as wh on wh.SysNo=ph.WarehouseSysNo
                left join PmManufacturer as pmf on pmf.SysNo=ph.ManufacturerSysNo 
                inner join PdProduct on PdProduct.SysNo=phd.ProductSysNo
                left join PdCategoryAssociation on PdCategoryAssociation.ProductSysNo=PdProduct.SysNo and 
                PdCategoryAssociation.IsMaster=1
                left join PdCategory a on a.SysNo=PdCategoryAssociation.CategorySysNo
                left join PdCategory b on b.SysNo=a.ParentSysNo
                left join PdCategory c on c.SysNo=b.ParentSysNo
                left join PdPrice on PdPrice.ProductSysNo=PdProduct.SysNo and PdPrice.PriceSource=10 and PdPrice.SourceSysNo=1
                " + whereStr + ") tb";
            sql = "select tb.* from " + sql + " order by tb.SysNo desc ";
            return Context.Sql(sql).QueryMany<CBPrPurchaseDetails>();
           
        }

        /// <summary>
        /// 获取采购单明细
        /// </summary>
        /// <param name="purchaseSysno">采购单系统编号</param>
        /// <returns></returns>
        /// <remarks>2018-01-04 杨浩 创建</remarks>
        public override IList<PrPurchaseDetails> GetPurchaseDetailsByPurchaseSysNo(int purchaseSysno)
        {
            return Context.Sql(string.Format(@"select pds.[SysNo],pds.[PurchaseSysNo],pds.[ProductSysNo],pds.[ProductName],pds.[Quantity],pds.[EnterQuantity],pds.[Money],pds.[TotalMoney],pds.[Remarks] 
                                    ,pd.ErpCode  from PrPurchaseDetails as pds  inner join PdProduct as pd on pd.sysno=pds.[ProductSysNo]
                                    where purchaseSysNo={0}", purchaseSysno))
                   .QueryMany<PrPurchaseDetails>();
        }
    }
}
