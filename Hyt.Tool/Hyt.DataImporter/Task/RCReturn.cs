using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.DataAccess.Client;
using Hyt.Model;
using Hyt.ProductImport;

namespace Hyt.DataImporter.Task
{
    public class RCReturn : BaseTask
    {
        public override void Read()
        {

           
            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.AppSettings["sqlconnectionstring"].ToString()))
            {
                SqlCommand command = new SqlCommand("ImportData.dbo.proc_RcReturn",myConn);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter myAdapter = new SqlDataAdapter(command);

                myAdapter.Fill(Common.RDS, "RCReturn");

            }
        }

        /*
        sysno, transactionsysno, rmaid, ordersysno, customersysno, source, 
        handledepartment, warehousesysno, pickupaddresssysno, receiveaddresssysno, rmatype, 
        pickuptypesysno, pickuptime, invoicesysno, ispickupinvoice, shiptypesysno, refundtype, 
        orginamount, orginpoint, refundpoint, redeemamount, internalremark, rmaremark, status, 
        createby, createdate, cancelby, canceldate, auditorby, auditordate, refundby, refunddate, 
        lastupdateby, lastupdatedate, deductedinvoiceamount, refundtotalamount, refundbank, 
        refundaccountname, refundaccount, refundproductamount*/
        public override void SetColumnMapping(OracleBulkCopy bcp)
        {
            bcp.ColumnMappings.Add("sysno", "sysno");
            bcp.ColumnMappings.Add("transactionsysno", "transactionsysno");
            bcp.ColumnMappings.Add("rmaid", "rmaid");
            bcp.ColumnMappings.Add("ordersysno", "ordersysno");
            bcp.ColumnMappings.Add("customersysno", "customersysno");
            bcp.ColumnMappings.Add("source", "source");

            bcp.ColumnMappings.Add("handledepartment", "handledepartment");
            bcp.ColumnMappings.Add("warehousesysno", "warehousesysno");
            bcp.ColumnMappings.Add("pickupaddresssysno", "pickupaddresssysno");
            bcp.ColumnMappings.Add("receiveaddresssysno", "receiveaddresssysno");
            bcp.ColumnMappings.Add("rmatype", "rmatype");

            bcp.ColumnMappings.Add("pickuptypesysno", "pickuptypesysno");
            bcp.ColumnMappings.Add("pickuptime", "pickuptime");
            bcp.ColumnMappings.Add("invoicesysno", "invoicesysno");
            bcp.ColumnMappings.Add("ispickupinvoice", "ispickupinvoice");
            bcp.ColumnMappings.Add("shiptypesysno", "shiptypesysno");
            bcp.ColumnMappings.Add("refundtype", "refundtype");

            bcp.ColumnMappings.Add("orginamount", "orginamount");
            bcp.ColumnMappings.Add("orginpoint", "orginpoint");
            bcp.ColumnMappings.Add("refundpoint", "refundpoint");
            bcp.ColumnMappings.Add("redeemamount", "redeemamount");
            bcp.ColumnMappings.Add("internalremark", "internalremark");
            bcp.ColumnMappings.Add("rmaremark", "rmaremark");
            bcp.ColumnMappings.Add("status", "status");

            bcp.ColumnMappings.Add("createby", "createby");
            bcp.ColumnMappings.Add("createdate", "createdate");
            bcp.ColumnMappings.Add("cancelby", "cancelby");
            bcp.ColumnMappings.Add("canceldate", "canceldate");
            bcp.ColumnMappings.Add("auditorby", "auditorby");
            bcp.ColumnMappings.Add("auditordate", "auditordate");
            bcp.ColumnMappings.Add("refundby", "refundby");
            bcp.ColumnMappings.Add("refunddate", "refunddate");

            bcp.ColumnMappings.Add("lastupdateby", "lastupdateby");
            bcp.ColumnMappings.Add("lastupdatedate", "lastupdatedate");
            bcp.ColumnMappings.Add("deductedinvoiceamount", "deductedinvoiceamount");
            bcp.ColumnMappings.Add("refundtotalamount", "refundtotalamount");
            bcp.ColumnMappings.Add("refundbank", "refundbank");

            bcp.ColumnMappings.Add("refundaccountname", "refundaccountname");
            bcp.ColumnMappings.Add("refundaccount", "refundaccount");
            bcp.ColumnMappings.Add("refundproductamount", "refundproductamount");

        }
    }
}
