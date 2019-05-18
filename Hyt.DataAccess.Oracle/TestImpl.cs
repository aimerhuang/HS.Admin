using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess;

namespace Hyt.DataAccess.Oracle
{
    public class TestImpl : ITest
    {

        public override int Count()
        {
            return Context.Sql("select count(1) from testuser where id>@0", 6).QuerySingle<int>();
        }

        public override string GetName(string s)
        {
            throw new NotImplementedException();
        }


    }
}
