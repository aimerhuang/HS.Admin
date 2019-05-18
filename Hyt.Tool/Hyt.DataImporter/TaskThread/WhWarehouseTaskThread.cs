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
    public class WhWarehouseTaskThread :BaseTaskThread
    {
        public WhWarehouseTaskThread(TaskBeginHandler OnTaskBegin, TaskGoingHandler OnTaskGoing)
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
            get { return "仓库"; }
        }

        private IList<WhWarehouse> list;

        protected override void Read()
        {
            string sSql = "SELECT   distinct  " +
                                    "sysno," +
                                    "stockid AS erpcode ," +
                                    "(case WHEN stockid='028-01' THEN 'RMA028-01'" +
			                            "ELSE  NULL " + 
		                                "END )AS erprmacode," +
		                            "NULL AS erprmacode ," +
                                    "StockName AS warehousename ," +
                                    "DistrictSysNo AS areasysno," +
                                    "Address AS streetaddress ,"+
                                    "contact ," +
                                    "phone ," +
                                    "(case when  status=0 then 1  " +
                                           "when status=-1 then 0 " +
                                       "end ) as status," + 
                                    "StockType AS warehousetype ," +
                                    "Xpoint AS latitude ," +
                                    "ypoint AS longitude ," +
                                    "img AS imgurl ," +
                                    "2 AS createdby ," +
                                    "GETDATE() AS createddate ," +
                                    "2 AS lastupdateby , " +
                                    "GETDATE() AS lastupdatedate " +
                            "FROM    dbo.Stock " +
						    "WHERE stockid !='RMA028-01'";
            list = DataProvider.Instance.Sql(sSql).QueryMany<WhWarehouse>();
        }

        protected override int GetTotal()
        {
            return list.Count;
        }

        protected override void Write(int rowIndex)
        {
            DataProvider.OracleInstance.Insert<WhWarehouse>("WhWarehouse", list[rowIndex]).AutoMap().Execute();
        }
    }
}
