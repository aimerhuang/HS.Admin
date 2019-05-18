using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Pos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Pos
{
    public abstract class IDsPosManageDao : DaoBase<IDsPosManageDao>
    {
        public abstract int Insert(DsPosManage mod);
        public abstract void Update(DsPosManage mod);
        public abstract void Delete(int sysNo);
        public abstract DsPosManage GetEntity(int sysNo);
        public abstract List<Model.Pos.CBDsPosManage> GetEntityListByDsSysNo(int dsSysNo);

        public abstract List<DsPosManage> GetEntityListByPosKey(string Key);

        public abstract CBPdProductDetail GetDealerProductByBarCode(string code, int dealerSysNo);

        public abstract List<DsPosOrderItem> GetPosOrderItemBySerialNumber(string numberNo);

        /// <summary>
        /// 获取商品档案数据
        /// </summary>
        /// <param name="detailList"></param>
        /// <returns></returns>
        public abstract List<CBPdProductDetail> GetPosProductDetailList(List<CBPdProductDetail> detailList, int warehouseSysNo, string dsProLink = "left");
        /// <summary>
        /// 获取实体通过机器终端码
        /// </summary>
        /// <param name="Termid"></param>
        /// <returns></returns>
        public abstract DsPosManage GetEntityByTermidNum(string Termid);
    }
}
