using Hyt.DataAccess.Logistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.Logistics
{
    public class LgFreightValuationModuleBo : BOBase<LgFreightValuationModuleBo>
    {
        /// <summary>
        /// 获取区域里的运费膜拜内容
        /// </summary>
        /// <param name="psysNo">模版id</param>
        /// <returns></returns>
        public  List<Model.Generated.LgFreightValuationModule> GetFreightModel(int psysNo)
        {
            return ILgFreightValuationModuleDao.Instance.GetFreightModel(psysNo);
        }
        /// <summary>
        /// 获取区域里的运费膜拜内容
        /// </summary>
        /// <param name="psysNo">模版id</param>
        /// <param name="AreaSysNo">区域值</param>
        /// <returns></returns>
        public  List<Model.Generated.LgFreightValuationModule> GetFreightModel(int psysNo, int AreaSysNo)
        {
            return ILgFreightValuationModuleDao.Instance.GetFreightModel(psysNo, AreaSysNo);
        }

        /// <summary>
        /// 通过 区域，模版id，报价金额获取实体
        /// </summary>
        /// <param name="psysNo">模版id</param>
        /// <param name="AreaSysNo">区域值</param>
        /// <param name="decimalValue">报价金额</param>
        /// <returns></returns>
        public  Model.Generated.LgFreightValuationModule GetFreightModel(int psysNo, int AreaSysNo, decimal decimalValue)
        {
            return ILgFreightValuationModuleDao.Instance.GetFreightModel(psysNo, AreaSysNo, decimalValue);
        }

        /// <summary>
        /// 添加和修改
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public int InnerOrUpdateFreightModel(Hyt.Model.Generated.LgFreightValuationModule mod)
        { 
            int sysNo=-1;
            if (mod.SysNo > 0)
            {
                ILgFreightValuationModuleDao.Instance.UpdateFreightModelData(mod);
            }
            else
            {
               sysNo=  ILgFreightValuationModuleDao.Instance.InsertFreightModelData(mod);
            }
            return sysNo;
        }

        /// <summary>
        /// 删除运费模版
        /// </summary>
        /// <param name="sysNoIdList"></param>
        public void DeleteFreightModel(string sysNoIdList)
        {
            ILgFreightValuationModuleDao.Instance.DeleteFreightModelData(sysNoIdList);
        }

        
    }
}
