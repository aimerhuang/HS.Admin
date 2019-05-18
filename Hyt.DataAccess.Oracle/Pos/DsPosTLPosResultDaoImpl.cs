using Hyt.DataAccess.Pos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.Pos
{
    public class DsPosTLPosResultDaoImpl : IDsPosTLPosResultDao
    {
        /// <summary>
        /// 添加通联回调数据
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public override int InsertMod(Model.Pos.DsPosTLPosResult mod)
        {
           return  Context.Insert("DsPosTLPosResult", mod).AutoMap(p => p.SysNo).ExecuteReturnLastId<int>("SysNo");
        }
        /// <summary>
        /// 更新通联回调数据
        /// </summary>
        /// <param name="mod"></param>
        public override void UpdateMod(Model.Pos.DsPosTLPosResult mod)
        {
            Context.Update("DsPosTLPosResult", mod).AutoMap(p => p.SysNo).Where(p=>p.SysNo).Execute();
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="PosKey"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        public override Model.Pos.DsPosTLPosResult GetDsPosTLPosResultData(string bizseq, string termid, string Status)
        {
            string sql = " select * from DsPosTLPosResult where bizseq='" + bizseq + "' and termid ='" + termid + "'  and stauts ='" + Status + "'  ";
            return Context.Sql(sql).QuerySingle<Hyt.Model.Pos.DsPosTLPosResult>();

        }

        public override Model.Pos.DsPosTLPosResult GetDsPosTLPosBizseq(string bizseq,string PosKey)
        {
            string sql = " select * from DsPosTLPosResult where bizseq='" + bizseq + "' and termid ='" + PosKey + "'  ";
            return Context.Sql(sql).QuerySingle<Hyt.Model.Pos.DsPosTLPosResult>();
        }

        public override Model.Pos.DsPosTLPosResult GetDsPosTLPosBizseq(string PosKey)
        {
            string sql = " select * from DsPosTLPosResult where and termid ='" + PosKey + "' and stauts ='-1'  order by SysNo DESC  ";
            return Context.Sql(sql).QuerySingle<Hyt.Model.Pos.DsPosTLPosResult>();
        }
    }
}
