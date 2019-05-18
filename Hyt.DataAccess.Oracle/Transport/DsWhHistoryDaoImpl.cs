using Hyt.DataAccess.Transport;
using Hyt.Model.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.Transport
{
    public class DsWhHistoryDaoImpl : IDsWhHistoryDao
    {
        public override int insertMod(Model.Transport.DsWhHistory history)
        {
            return Context.Insert("DsWhHistory", history).AutoMap(p=>p.SysNo).ExecuteReturnLastId<int>("SysNo");
        }

        public override List<Model.Transport.CBDsWhHistory> GetHistoryListByCourierNumber(string Code)
        {
            string Sql = " select DsWhHistory.*,DsWhGoodsManagement.CreateTime as GMCreateTime from  DsWhHistory inner join  DsWhGoodsManagement on DsWhGoodsManagement.CourierNumber=DsWhHistory.OrderCode  ";
            Sql += " where CourierNumber in ('" + string.Join("','", Code.Split(',')) + "') or AssNumber in ('" + string.Join("','", Code.Split(',')) + "')";
            return Context.Sql(Sql).QueryMany<CBDsWhHistory>();
        }
        /// <summary>
        /// 获取记录历史信息
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public override CBDsWhHistory GetEntityBySysNo(int SysNo)
        {
            string sql = " select * from DsWhHistory where SysNo='"+SysNo+"' ";
            return Context.Sql(sql).QuerySingle<CBDsWhHistory>();
        }
        /// <summary>
        /// 更新历史记录信息
        /// </summary>
        /// <param name="entity"></param>
        public override void UpdateMod(DsWhHistory entity)
        {
            Context.Update("DsWhHistory", entity).AutoMap(p=>p.SysNo).Where(p=>p.SysNo).Execute();
        }
    }
}
