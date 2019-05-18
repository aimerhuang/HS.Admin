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
    public class CrReceiveAddressTaskThread :BaseTaskThread
    {
        logRecord log = new logRecord();

        public CrReceiveAddressTaskThread(TaskBeginHandler OnTaskBegin, TaskGoingHandler OnTaskGoing)
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
            get { return "接收地址"; }
        }

        protected override int GetTotal()
        {
            return list.Count;
        }

        private IList<CrReceiveAddress> list;

        protected override void Read()
        {
            string sSql = "SELECT  " +
                                "sysno," +
                                "AreaSysNo," +
                                "customersysno," +
                                "name," +
                                "sex AS gender," +
                                "phone AS phonenumber," +
                                "(CASE WHEN LEN(CellPhone)>11 THEN NULL " +
	                                "ELSE CellPhone " +
                                 "END) AS  mobilephonenumber," +
                                "fax AS faxnumber," +
                                "email AS emailaddress," +
                                "address AS streetaddress," +
                                "(CASE WHEN LEN(zip)>6 THEN NULL " +
	                                "ELSE zip " +
                                "END) AS zipcode," +
                                "isdefault," +
                                "NULL AS title " +
                            "FROM Customer_Address " +
	                          "ORDER BY a.SysNo";

            list = DataProvider.Instance.Sql(sSql).QueryMany<CrReceiveAddress>();
        }

        protected override void Write(int rowIndex)
        {
            try
            {
               
                DataProvider.OracleInstance.Insert<CrReceiveAddress>("CrReceiveAddress", list[rowIndex]).AutoMap().Execute();
            }
            catch
            {

                log.CheckLog("导入表【CrReceiveAddress】失败，" + "系统号：" + list[rowIndex].SysNo + "; zipcode:" + list[rowIndex].ZipCode);
            }
        }
    }
}
