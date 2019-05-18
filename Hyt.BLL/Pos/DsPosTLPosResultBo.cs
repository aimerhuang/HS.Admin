using Hyt.DataAccess.Pos;
using Hyt.Model.Pos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.Pos
{
    public class DsPosTLPosResultBo : BOBase<DsPosTLPosResultBo>
    {
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public int InsertMod(DsPosTLPosResult mod)
        {
            return IDsPosTLPosResultDao.Instance.InsertMod(mod);
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="mod"></param>
        public  void UpdateMod(DsPosTLPosResult mod)
        {
            IDsPosTLPosResultDao.Instance.UpdateMod(mod);
        }
        /// <summary>
        /// 查询获取数据
        /// </summary>
        /// <param name="PosKey"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        public DsPosTLPosResult GetDsPosTLPosResultData(string Bizseq, string termid, string Status)
        {
            return IDsPosTLPosResultDao.Instance.GetDsPosTLPosResultData(Bizseq, termid, Status);
        }

        public DsPosTLPosResult GetDsPosTLPosBizseq(string bizseq,string PosKey)
        {
            return IDsPosTLPosResultDao.Instance.GetDsPosTLPosBizseq(bizseq, PosKey);
        }

        public DsPosTLPosResult GetDsPosTLPosBizseq(string PosKey)
        {
            return IDsPosTLPosResultDao.Instance.GetDsPosTLPosBizseq( PosKey);
        }
    }
}
