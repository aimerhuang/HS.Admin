using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hyt.DataImporter.TaskThread;
using Hyt.Model;
using Hyt.ProductImport;
using Oracle.DataAccess.Client;

namespace Hyt.DataImporter.TaskThread
{
    public class BsAreaTaskThread :BaseTaskThread
    {

        public BsAreaTaskThread(TaskBeginHandler OnTaskBegin, TaskGoingHandler OnTaskGoing)
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
            get { return "地区"; }
        }

        protected override int GetTotal()
        {
            return list.Count;
        }

        private IList<BsArea> list;

        protected override void Read()
        {
            string sSql = "SELECT DISTINCT " +
                                "sysno," +
                                "0 AS parentsysno," +
                                "ProvinceName AS areaname," +
                                "NULL AS areacode," +
                                "NULL AS nameacronym," +
                                "OrderNumber AS displayorder," +
                                "1 AS arealevel," +
                                "(CASE WHEN Status=0 THEN 1 " +
	                            "WHEN Status=-1 THEN 0 " +
	                            "END)  AS status, " +
                                "NULL AS createdby, " +
                                "NULL AS createddate, " +
                                "NULL AS lastupdateby," +
                                "NULL as lastupdatedate " +
                          "FROM dbo.Area " +
                          "WHERE ProvinceSysNo=-999999 " +
                    "UNION " +
                        "SELECT sysno," +
                                "ProvinceSysNo AS parentsysno," +
                                "CityName AS areaname," +
                                "NULL AS areacode," +
                                "NULL AS nameacronym," +
                                "OrderNumber AS displayorder," +
                                "2 AS arealevel," +
                                "(CASE WHEN Status=0 THEN 1 " +
	                            "WHEN Status=-1 THEN 0 " +
	                                "END)  AS status," +
                                "NULL AS createdby," +
                                "NULL AS createddate," +
                                "NULL AS lastupdateby," +
                                "NULL as lastupdatedate " +
                        "FROM dbo.Area " +
                        "WHERE ProvinceSysNo<>-999999 AND (CitySysNo=-999999 OR CitySysNo IS NULL) " +
                   "UNION " +
                        "SELECT sysno, " +
                                "CitySysNo AS parentsysno," +
                                "DistrictName AS areaname," +
                                "NULL AS areacode," +
                                "NULL AS nameacronym," +
                                "OrderNumber AS displayorder," +
                                "3 AS arealevel," +
                                "(CASE WHEN Status=0 THEN 1 " +
	                             "WHEN Status=-1 THEN 0 " +
	                                "END)  AS status," +
                                "NULL AS createdby," +
                                "NULL AS createddate," +
                                "NULL AS lastupdateby," +
                                "NULL as lastupdatedate " +
                        "FROM dbo.Area " +
                        "WHERE ProvinceSysNo<>-999999 AND CitySysNo<>-999999  AND DistrictName IS not null " +
                        "ORDER BY parentsysno,sysno ";

           
            list = DataProvider.Instance.Sql(sSql).QueryMany<BsArea>();
        }

        
        protected override void Write(int rowIndex)
        {
            DataProvider.OracleInstance.Insert<BsArea>("BsArea", list[rowIndex]).AutoMap().Execute();
        }

        
        
    }
}
