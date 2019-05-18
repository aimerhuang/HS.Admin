using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.DataAccess.Client;

namespace Hyt.DataImporter.Task
{
    public abstract class BaseTask
    {
        public abstract void Read();

        public abstract void SetColumnMapping(OracleBulkCopy bcp);
    }
}
