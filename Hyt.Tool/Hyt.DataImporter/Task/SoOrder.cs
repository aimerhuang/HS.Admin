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
    public class SoOrder :BaseTask
    {
        public override void Read()
        {

            //基础价格(0) 配送员信用价格(70) 会员等级价格(10)
            string sSql = "select  * from ImportData.dbo.soorder order by sysno ";

            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.AppSettings["sqlconnectionstring"].ToString()))
            {

                SqlDataAdapter myAdapter = new SqlDataAdapter(sSql, myConn);

                myAdapter.Fill(Common.RDS, "SoOrder");

            }
        }

        /*sysno, receiveaddresssysno, transactionsysno, customersysno, defaultwarehousesysno, 
        deliverytypesysno, paytypesysno, orderamount, freightamount, orderdiscountamount,
        couponamount, cashpay, coinpay, invoicesysno, ordersource, 
        ishiddentocustomer, paystatus, customermessage, internalremarks, deliveryremarks, 
        deliverytime, contactbeforedelivery, remarks, createdate,auditorsysno,
        auditordate, cancelusersysno, canceldate, status, lastupdateby, 
        lastupdatedate, ordersourcesysno,ordercreatorsysno, salestype, salessysno,
         stamp, onlinestatus, cancelusertype, productamount, productdiscountamount, 
        freightdiscountamount, productchangeamount, freightchangeamount*/
        public override void SetColumnMapping(OracleBulkCopy bcp)
        {
            bcp.ColumnMappings.Add("sysno", "sysno");
            bcp.ColumnMappings.Add("receiveaddresssysno", "receiveaddresssysno");
            bcp.ColumnMappings.Add("transactionsysno", "transactionsysno");
            bcp.ColumnMappings.Add("customersysno", "customersysno");
            bcp.ColumnMappings.Add("defaultwarehousesysno", "defaultwarehousesysno");

            bcp.ColumnMappings.Add("deliverytypesysno", "deliverytypesysno");
            bcp.ColumnMappings.Add("paytypesysno", "paytypesysno");
            bcp.ColumnMappings.Add("orderamount", "orderamount");
            bcp.ColumnMappings.Add("freightamount", "freightamount");
            bcp.ColumnMappings.Add("orderdiscountamount", "orderdiscountamount");

            bcp.ColumnMappings.Add("couponamount", "couponamount");
            bcp.ColumnMappings.Add("cashpay", "cashpay");
            bcp.ColumnMappings.Add("coinpay", "coinpay");
            bcp.ColumnMappings.Add("invoicesysno", "invoicesysno");
            bcp.ColumnMappings.Add("ordersource", "ordersource");

            bcp.ColumnMappings.Add("ishiddentocustomer", "ishiddentocustomer");
            bcp.ColumnMappings.Add("paystatus", "paystatus");
            bcp.ColumnMappings.Add("customermessage", "customermessage");
            bcp.ColumnMappings.Add("internalremarks", "internalremarks");
            bcp.ColumnMappings.Add("deliveryremarks", "deliveryremarks");

            bcp.ColumnMappings.Add("deliverytime", "deliverytime");
            bcp.ColumnMappings.Add("contactbeforedelivery", "contactbeforedelivery");
            bcp.ColumnMappings.Add("remarks", "remarks");
            bcp.ColumnMappings.Add("createdate", "createdate");
            bcp.ColumnMappings.Add("auditorsysno", "auditorsysno");

            bcp.ColumnMappings.Add("auditordate", "auditordate");
            bcp.ColumnMappings.Add("cancelusersysno", "cancelusersysno");
            bcp.ColumnMappings.Add("canceldate", "canceldate");
            bcp.ColumnMappings.Add("status", "status");
            bcp.ColumnMappings.Add("lastupdateby", "lastupdateby");

            bcp.ColumnMappings.Add("lastupdatedate", "lastupdatedate");
            bcp.ColumnMappings.Add("ordersourcesysno", "ordersourcesysno");
            bcp.ColumnMappings.Add("ordercreatorsysno", "ordercreatorsysno");
            bcp.ColumnMappings.Add("salestype", "salestype");
            bcp.ColumnMappings.Add("salessysno", "salessysno");

            bcp.ColumnMappings.Add("stamp", "stamp");
            bcp.ColumnMappings.Add("onlinestatus", "onlinestatus");
            bcp.ColumnMappings.Add("cancelusertype", "cancelusertype");
            bcp.ColumnMappings.Add("productamount", "productamount");
            bcp.ColumnMappings.Add("productdiscountamount", "productdiscountamount");

            bcp.ColumnMappings.Add("freightdiscountamount", "freightdiscountamount");
            bcp.ColumnMappings.Add("productchangeamount", "productchangeamount");
            bcp.ColumnMappings.Add("freightchangeamount", "freightchangeamount");

        }
    }
}
