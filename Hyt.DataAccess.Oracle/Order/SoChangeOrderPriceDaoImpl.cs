using Hyt.DataAccess.Order;
using Hyt.Model.Generated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.Order
{
    public class SoChangeOrderPriceDaoImpl : ISoChangeOrderPriceDao
    {
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public override int Insert(Model.Generated.SoChangeOrderPrice mod)
        {
            return Context.Insert("SoChangeOrderPrice", mod).AutoMap(p => p.SysNo).ExecuteReturnLastId<int>("SysNo");
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="mod"></param>
        public override void Update(Model.Generated.SoChangeOrderPrice mod)
        {
            Context.Update("SoChangeOrderPrice", mod).AutoMap(p => p.SysNo).Execute();
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="SysNo"></param>
        public override void Delete(int SysNo)
        {
            string sql = " delete from SoChangeOrderPrice where SysNo='" + SysNo + "' ";
            Context.Sql(sql).Execute();
        }
        /// <summary>
        /// 获取调价单数据
        /// </summary>
        /// <param name="OrderSysNo"></param>
        /// <returns></returns>
        public override Model.Generated.SoChangeOrderPrice GetDataByOrderSysNo(int OrderSysNo)
        {
            string sql = " select * from  SoChangeOrderPrice where OrderSysNo = '" + OrderSysNo + "'  ";
            sql += " order by CraeteDate desc ";
            return Context.Sql(sql).QuerySingle<SoChangeOrderPrice>();
        }
    }
}
