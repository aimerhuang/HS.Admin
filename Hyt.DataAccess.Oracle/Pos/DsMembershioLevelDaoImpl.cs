using Hyt.DataAccess.Pos;
using Hyt.Model.Pos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.Pos
{
    public class DsMembershioLevelDaoImpl : IDsMembershioLevelDao
    {
        /// <summary>
        /// 添加会员卡等级
        /// </summary>
        /// <param name="cardMod"></param>
        /// <returns></returns>
        public override int Insert(DsMembershioLevel cardMod)
        {
            return Context.Insert("DsMembershioLevel", cardMod).AutoMap(p => p.SysNo).ExecuteReturnLastId<int>("SysNo");
        }
        /// <summary>
        /// 更新会员卡等级
        /// </summary>
        /// <param name="cardMod"></param>
        public override void Update(DsMembershioLevel cardMod)
        {
            Context.Update("DsMembershioLevel", cardMod).AutoMap(p => p.SysNo).Where(p => p.SysNo).Execute(); ;
        }
        /// <summary>
        /// 获取会员卡等级信息
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public override DsMembershioLevel GetDsMembershioLevelBySysNo(int SysNo)
        {
            string sql = " select * from DsMembershioLevel where SysNo ='" + SysNo + "' ";
            return Context.Sql(sql).QuerySingle<DsMembershioLevel>();
        }

        public override List<DsMembershioLevel> GetDsMembershipLevelList(int dsSysNo)
        {
            string sql = " select * from DsMembershioLevel where ( DsSysNo = '" + dsSysNo + "'  or " + dsSysNo + " = 0  ) ";
            return Context.Sql(sql).QueryMany<DsMembershioLevel>();
        }
    }
}
