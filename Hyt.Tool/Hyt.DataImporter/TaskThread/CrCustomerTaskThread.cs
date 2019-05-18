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
    public class CrCustomerTaskThread :BaseTaskThread
    {
        public CrCustomerTaskThread(TaskBeginHandler OnTaskBegin, TaskGoingHandler OnTaskGoing)
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
            get { return "会员"; }
        }

        protected override int GetTotal()
        {
            return list.Count;
        }

        private IList<CrCustomer> list;

        protected override void Read()
        {

            string sSql = "select distinct " +
                                "sysno," +
                                "CustomerID AS account, " +
                                "pwd AS  password," +
                                "CustomerName as name," +
                                "Nickname AS nickname," +
                                "NULL AS  headimage," +
                                "Email  AS emailaddress, " +
                                "NULL AS  emailstatus," +
                                "CellPhone AS mobilephonenumber," +
                                "CellPhoneStatus  AS  mobilephonestatus," +
                                "Gender as gender," +
                                "CardNo AS idcardno," +
                                "(case when DwellAreaSysNo=-999999 then 0 " + 
                                  "else " +
                                    "DwellAreaSysNo " +
                                  "end ) as areasysno," +
                                "ReceiveAddress AS streetaddress," +
                                "Birthday ," +
                                "Marriage AS maritalstatus," +
                                "Income AS monthlyincome," +
                                "Hobbies as hobbies," +
                                "NULL as registersource," +
                                "RegisterTime as registerdate," +
                                "LastLoginIP," +
                                "LastLoginTime as lastlogindate," +
                                "(case when status =-1 then 0" +
                                        "when status=0 then 1 " +
                                "end ) as status," +
                                "null as levelsysno," +
                                "null as  levelpoint," +
                                "null as experiencepoint," +
                                "null as experiencecoin, " +
                                "null as registersourcesysno," +
                                "null as islevelfixed," +
                                "null as isexperiencepointfixed," +
                                "null as isexperiencecoinfixed " +
                            "from dbo.Customer " +
                            "order by sysno";
                                            

            list = DataProvider.Instance.Sql(sSql).QueryMany<CrCustomer>();
        }

        protected override void Write(int rowIndex)
        {
            DataProvider.OracleInstance.Insert<CrCustomer>("CrCustomer", list[rowIndex]).AutoMap().Execute();
        }
    }
}
