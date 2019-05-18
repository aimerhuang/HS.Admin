using Hyt.DataAccess.Pos;
using Hyt.Model.Pos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.Pos
{
    public class DsPosManageBo : BOBase<DsPosManageBo>
    {
        public int Insert(DsPosManage mod)
        {
            return IDsPosManageDao.Instance.Insert(mod);
        }
        public void Update(DsPosManage mod) {
            IDsPosManageDao.Instance.Update(mod);
        }
        public void Delete(int sysNo) {
            IDsPosManageDao.Instance.Delete(sysNo);
        }
        public DsPosManage GetEntity(int sysNo) {
            return IDsPosManageDao.Instance.GetEntity(sysNo);
        }
        public List<Model.Pos.CBDsPosManage> GetEntityListByDsSysNo(int dsSysNo)
        {
            return IDsPosManageDao.Instance.GetEntityListByDsSysNo(dsSysNo);
        }

        public List<Model.Pos.DsPosManage> GetEntityListByPosKey(string Key)
        {
            List<Model.Pos.DsPosManage> modList = IDsPosManageDao.Instance.GetEntityListByPosKey(Key);
            //foreach (var mod in modList)
            //{
            //    mod.pos_posName = mod.pos_posName.Trim();
            //}
            return modList;
        }

        public Model.CBPdProductDetail GetDealerProductByBarCode(string code, int dealerSysNo)
        {
            return IDsPosManageDao.Instance.GetDealerProductByBarCode(code, dealerSysNo);
        }

        public List<DsPosOrderItem> GetPosOrderItemBySerialNumber(string numberNo)
        {
            return IDsPosManageDao.Instance.GetPosOrderItemBySerialNumber(numberNo);
        }

        /// <summary>
        /// 获取商品档案
        /// </summary>
        /// <param name="detailList"></param>
        public List<Model.CBPdProductDetail> GetPosProductDetailList(List<Model.CBPdProductDetail> detailList, int warehouseSysNo, string dsProLink = "left")
        {
            return IDsPosManageDao.Instance.GetPosProductDetailList(detailList, warehouseSysNo, dsProLink);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public DsPosManage GetEntityByTermidNum(string Termid)
        {
            return IDsPosManageDao.Instance.GetEntityByTermidNum(Termid);
        }
    }
}
