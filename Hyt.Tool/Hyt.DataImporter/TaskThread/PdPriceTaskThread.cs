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
    public class PdPriceTaskThread :BaseTaskThread
    {
        public PdPriceTaskThread(TaskBeginHandler OnTaskBegin, TaskGoingHandler OnTaskGoing)
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
            get { return "商品价格"; }
        }

        protected override int GetTotal()
        {
            return list.Count;
        }

        private IList<PdPrice> list;

        protected override void Read()
        {
            //基础价格(0) 配送员信用价格(70) 会员等级价格(10)
            string sSql = "SELECT ProductSysNo,BasicPrice AS price,	0 AS PriceSource,NULL AS sourcesysno,1 AS status FROM Product_Price " +
                            "UNION  " +
                          "SELECT ProductSysNo,DistributionCreditPrice AS price,70 AS PriceSource,NULL AS sourcesysno,1 AS status FROM Product_Price " +  
                           "union " +	 
                         "SELECT  ProductSysNo,CustomerRankPrice AS price,10 AS pricesource, CustomerRankSysNo AS sourcesysno,1 AS status FROM Product_CustomerRank_Price " +
                         "ORDER BY productsysno ";

            list = DataProvider.Instance.Sql(sSql).QueryMany<PdPrice>();
        }

        protected override void Write(int rowIndex)
        {
            DataProvider.OracleInstance.Insert<PdPrice>("PdPrice", list[rowIndex]).AutoMap(x => x.SysNo).Execute();
        }
    }
}
