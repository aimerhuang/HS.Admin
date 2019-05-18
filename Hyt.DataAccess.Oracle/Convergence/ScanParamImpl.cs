using Hyt.DataAccess.Convergence;
using Hyt.Model.Convergence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.Convergence
{
    public class ScanParamImpl : ScanParamDao
    {
        public override ScanParam GetScanParam(int OrderNo) {
            return Context.Sql(@"select s.ProductName P5_ProductName,so.CashPay  P3_Amount ,b.PaymentName Q1_FrpCode from 
            SoOrderItem s,SoOrder so,BsPaymentType b where so.SysNo=s.OrderSysNo and b.SysNo=so.PayTypeSysNo and so.SysNo=@0  ", OrderNo).QuerySingle<ScanParam>();
        }
    }
}
