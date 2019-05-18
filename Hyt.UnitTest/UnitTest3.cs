using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Hyt.DataAccess.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hyt.UnitTest
{
    [TestClass]
    public class UnitTest3
    {
        protected IDbContext Context
        {
            //用Transations.Transaction.Current判断下是否有当前事务，有的话返回当前事务的链接
            get
            {
                IDbContext context = new DbContext().ConnectionString("Data Source=202;User Id=hytuser;Password=hytuser;", new OracleProvider());
                return context;
            }
        }

        [TestMethod]
        public void TestMethod1()
        {
            using (var ts = new TransactionScope())
            {
                Context.Sql("update DsPrePayment set TotalPrestoreAmount = TotalPrestoreAmount + 10 where sysno=1").Execute();
                Thread.Sleep(500);
                ts.Complete();
            }
        }

        [TestMethod]
        public void TestMethod2()
        {
            using (var ts = new TransactionScope())
            {
                Context.Sql("update DsPrePayment set TotalPrestoreAmount = TotalPrestoreAmount - 10 where sysno=1").Execute();
                Thread.Sleep(500);
                ts.Complete();
            }
        }

        [TestMethod]
        public void TestMethod3()
        {
            List<Task> tasks = new List<Task>();

            for (int i = 0; i < 50; i++)
            {
                var t1 = Task.Factory.StartNew(() => { TestMethod1(); });
                tasks.Add(t1);
            }
            for (int i = 0; i < 50; i++)
            {
                var t2 = Task.Factory.StartNew(() => { TestMethod2(); });
                tasks.Add(t2);
            }
            //var t2 = Task.Factory.StartNew(() => { TestMethod2(); });
            Task.WaitAll(tasks.ToArray());
        }
    }
}
