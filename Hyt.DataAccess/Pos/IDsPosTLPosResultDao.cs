using Hyt.DataAccess.Base;
using Hyt.Model.Pos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Pos
{
    public abstract class IDsPosTLPosResultDao : DaoBase<IDsPosTLPosResultDao>
    {
        public abstract int InsertMod(DsPosTLPosResult mod);
        public abstract void UpdateMod(DsPosTLPosResult mod);
        public abstract DsPosTLPosResult GetDsPosTLPosResultData(string bizseq, string termid, string Status);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bizseq"></param>
        /// <returns></returns>
        public abstract DsPosTLPosResult GetDsPosTLPosBizseq(string bizseq, string PosKey);

        public abstract DsPosTLPosResult GetDsPosTLPosBizseq(string PosKey);
    }
}
