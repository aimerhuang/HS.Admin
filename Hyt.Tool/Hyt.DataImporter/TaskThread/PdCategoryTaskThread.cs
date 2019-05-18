using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hyt.Model;
using Hyt.ProductImport;

namespace Hyt.DataImporter.TaskThread
{
    public class PdCategoryTaskThread : BaseTaskThread
    {
        logRecord log = new logRecord();
        public PdCategoryTaskThread(TaskBeginHandler OnTaskBegin, TaskGoingHandler OnTaskGoing)
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
            get { return "商品分类"; }
        }

        private IList<PdCategory> list;

        protected override void Read()
        {
            //string sSql = "SELECT  sysno AS sysno," +
            //                         "0 AS ParentSysNo," +
            //                         "C1Name as CategoryName," +
            //                         "OrderList as DisplayOrder," +
            //                         "SEOTitle as SeoTitle," +
            //                         "SEOKeyword as SeoKeyword," +
            //                         "SEODescription as SeoDescription," +
            //                         "null as templatesysno,"+
            //                         "null as remarks,"+
            //                         "null as createdby,"+
            //                         "null as createddate," +
            //                         "null as lastupdateby,"+
            //                         "null as  lastupdatedate,"+
            //                         "1 as status " + 
            //                 "from Category1 " +
            //                "UNION " +
            //                "SELECT  c3.SysNo AS sysno," +
            //                         "c2.c1sysno AS ParentSysNo," +
            //                         "c3name AS CategoryName," +
            //                         "C3.OrderList AS DisplayOrder," +
            //                         "C3.SEOTitle as SeoTitle," +
            //                         "C3.SEOKeyword as SeoKeyword," +
            //                         "C3.SEODescription as SeoDescription," +
            //                          "null as templatesysno," +
            //                         "null as remarks," +
            //                         "null as createdby," +
            //                         "null as createddate," +
            //                         "null as lastupdateby," +
            //                         "null as  lastupdatedate," +
            //                         "1 as status " + 
            //                "FROM dbo.Category3 AS c3 LEFT JOIN dbo.Category2 AS c2 ON c3.C2SysNo=c2.SysNo " +
            //                 "ORDER BY ParentSysNo,sysno";
            string sSql = "select *from PdCategory order by sysno";
            list = DataProvider.Instance.Sql(sSql).QueryMany<PdCategory>();
        }

        protected override int GetTotal()
        {
            return list.Count;
        }

        protected override void Write(int rowIndex)
        {
            try
            {
                DataProvider.OracleInstance.Insert<PdCategory>("PdCategory", list[rowIndex]).AutoMap(p => p.ParentCategory).Execute();
            }
            catch(Exception ex)
            {

                log.CheckLog("导入表【PdCategory】失败，" + "系统号：" + list[rowIndex].SysNo.ToString());
            }
            
        }
    }
}
