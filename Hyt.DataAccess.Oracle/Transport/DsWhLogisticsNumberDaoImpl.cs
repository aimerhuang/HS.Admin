using Hyt.DataAccess.Transport;
using Hyt.Model.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.Transport
{
    public class DsWhLogisticsNumberDaoImpl : IDsWhLogisticsNumberDao
    {
        public override int InsertMod(Model.Transport.DsWhLogisticsNumber mod)
        {
            return Context.Insert("DsWhLogisticsNumber", mod).AutoMap(p => p.SysNo).ExecuteReturnLastId<int>("SysNo");
        }

        public override void UpdataMod(Model.Transport.DsWhLogisticsNumber mod)
        {
            Context.Update("DsWhLogisticsNumber", mod).AutoMap(p => p.SysNo).Where(p => p.SysNo).Execute();
        }

        public override Model.Transport.DsWhLogisticsNumber GetLogisticsNumberByNotUsed(string type)
        {
            string sql = " select * from DsWhLogisticsNumber where LgUsed=0 and LgCode='" + type + "' ";
            return Context.Sql(sql).QuerySingle<DsWhLogisticsNumber>();
        }

        public override List<Model.Transport.DsWhLogisticsNumber> GetAllLogisticsNumberByServiceType(string type)
        {
            string sql = " select * from DsWhLogisticsNumber where LgCode='" + type + "' ";
            return Context.Sql(sql).QueryMany<DsWhLogisticsNumber>();
        }

        public override List<DsWhLogisticsNumber> GetLogisticsNumberByNotUsed(Dictionary<string, int> dicType)
        {
            List<DsWhLogisticsNumber> numberList = new List<DsWhLogisticsNumber>();
            string sql = "select top {0} * from DsWhLogisticsNumber where  LgUsed=0 and LgCode='{1}'";
            foreach (string key in dicType.Keys)
            {
                numberList.AddRange(Context.Sql(sql.Replace("{0}", dicType[key].ToString()).Replace("{1}", key)).QueryMany<DsWhLogisticsNumber>());
            }
            return numberList;
        }

        public override List<CBDsWhLogisticsNumber> GetLogisticsNumberListByCodeList(List<string> listNumber)
        {
            string sql = @" select num = (select COUNT(LgCode) from [DsWhLogisticsNumber] a where a.LgCode=[DsWhLogisticsNumber].LgCode and LgUsed=0 ),LgCode
                            from [DsWhLogisticsNumber] where LgCode in('" + string.Join("','", listNumber.ToArray()) + "')  ";
            return Context.Sql(sql).QueryMany<CBDsWhLogisticsNumber>();
        }
    }
}
