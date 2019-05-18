using Hyt.DataAccess.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.Transport
{
    public class DsWhCustomerConfigBo : BOBase<DsWhCustomerConfigBo>
    {
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public  int InsertMod(Model.Transport.DsWhCustomerConfig mod)
        {
            return IDsWhCustomerConfigDao.Instance.InsertMod(mod);
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="mod"></param>
        public  void UpdateMod(Model.Transport.DsWhCustomerConfig mod)
        {
            IDsWhCustomerConfigDao.Instance.UpdateMod(mod);
        }
        /// <summary>
        /// 获取单条数据信息
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public  Model.Transport.DsWhCustomerConfig GetDsWhCustomerConfig(int SysNo)
        {
            return IDsWhCustomerConfigDao.Instance.GetDsWhCustomerConfig(SysNo);
        }
        /// <summary>
        /// 获取所有状态信息
        /// </summary>
        /// <returns></returns>
        public  List<Model.Transport.DsWhCustomerConfig> GetDsWhCustomerList()
        {
            return IDsWhCustomerConfigDao.Instance.GetDsWhCustomerList();
        }
    }
}
