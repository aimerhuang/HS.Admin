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
    public class WhStockOut : BaseTask
    {
        public override void Read()
        {
            
            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.AppSettings["sqlconnectionstring"].ToString()))
            {
                SqlCommand command = new SqlCommand("ImportData.dbo.proc_whstockout", myConn);
                command.CommandType=CommandType.StoredProcedure;
                SqlDataAdapter myAdapter = new SqlDataAdapter(command);
              
                myAdapter.Fill(Common.RDS, "WhStockOut");

            }
        }

        /*sysno, transactionsysno, warehousesysno, ordersysno, receiveaddresssysno, iscod, stockoutamount, receivable, 
            status, signtime, isprintedpackagecover, isprintedpickupcover, customermessage,remarks, createdby, createddate, 
        lastupdateby, lastupdatedate, deliveryremarks, deliverytime, contactbeforedelivery, pickupdate, deliverytypesysno, 
        stockoutby, stockoutdate, stamp, invoicesysno*/
        public override void SetColumnMapping(OracleBulkCopy bcp)
        {
            bcp.ColumnMappings.Add("sysno", "sysno");
            bcp.ColumnMappings.Add("transactionsysno", "transactionsysno");
            bcp.ColumnMappings.Add("warehousesysno", "warehousesysno");
            bcp.ColumnMappings.Add("ordersysno", "ordersysno");
            bcp.ColumnMappings.Add("receiveaddresssysno", "receiveaddresssysno");
            bcp.ColumnMappings.Add("iscod", "iscod");
            bcp.ColumnMappings.Add("stockoutamount", "stockoutamount");
            bcp.ColumnMappings.Add("receivable", "receivable");
            bcp.ColumnMappings.Add("status", "status");
            bcp.ColumnMappings.Add("signtime", "signtime");
            bcp.ColumnMappings.Add("isprintedpackagecover", "isprintedpackagecover");
            bcp.ColumnMappings.Add("isprintedpickupcover", "isprintedpickupcover");
            bcp.ColumnMappings.Add("customermessage", "customermessage");
            bcp.ColumnMappings.Add("remarks", "remarks");
            bcp.ColumnMappings.Add("createdby", "createdby");
            bcp.ColumnMappings.Add("createddate", "createddate");
            bcp.ColumnMappings.Add("lastupdateby", "lastupdateby");
            bcp.ColumnMappings.Add("lastupdatedate", "lastupdatedate");
            bcp.ColumnMappings.Add("deliveryremarks", "deliveryremarks");
            bcp.ColumnMappings.Add("deliverytime", "deliverytime");
            bcp.ColumnMappings.Add("contactbeforedelivery", "contactbeforedelivery");
            bcp.ColumnMappings.Add("pickupdate", "pickupdate");
            bcp.ColumnMappings.Add("deliverytypesysno", "deliverytypesysno");
            bcp.ColumnMappings.Add("stockoutby", "stockoutby");
            bcp.ColumnMappings.Add("stockoutdate", "stockoutdate");
            bcp.ColumnMappings.Add("stamp", "stamp");
            bcp.ColumnMappings.Add("invoicesysno", "invoicesysno");
        }
    }
}
