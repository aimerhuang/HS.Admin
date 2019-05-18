using Hyt.DataAccess.Transport;
using Hyt.Model.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.Transport
{
    /// <summary>
    /// 转运系统客户实体
    /// </summary>
    /// <remarks>
    /// 2016-05-17 杨云奕 添加
    /// </remarks>
    public class DsWhCustomerBo : BOBase<DsWhCustomerBo>
    {
        /// <summary>
        /// 添加客户实体
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public int InsertMod(DsWhCustomer mod)
        {
            return IDsWhCustomerDao.Instance.InsertMod(mod);
        }
        /// <summary>
        /// 修改客户实体
        /// </summary>
        /// <param name="mod"></param>
        public void UpdateMod(DsWhCustomer mod)
        {
            IDsWhCustomerDao.Instance.UpdateMod(mod);
        }
        /// <summary>
        /// 获取客户列表
        /// </summary>
        /// <returns></returns>
        public  List<DsWhCustomer> GetCustomerList()
        {
            return IDsWhCustomerDao.Instance.GetCustomerList();
        }
        /// <summary>
        /// 通过编号获取客户实体
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public DsWhCustomer GetCustomerBySysNo(int SysNo)
        {
            return IDsWhCustomerDao.Instance.GetCustomerBySysNo(SysNo);
        }
        /// <summary>
        /// 通过编号删除客户
        /// </summary>
        /// <param name="SysNo"></param>
        public void DeleteCustomerBySysNo(int SysNo)
        {
            IDsWhCustomerDao.Instance.DeleteCustomerBySysNo(SysNo);
        }
        /// <summary>
        /// 通过系统账号获取客户信息
        /// </summary>
        /// <param name="Account"></param>
        /// <returns></returns>
        public DsWhCustomer GetCustomerByAssAccount(string Account)
        {
            return IDsWhCustomerDao.Instance.GetCustomerByAssAccount(Account);
        }

        /// <summary>
        /// 装运系统客户筛选
        /// </summary>
        /// <param name="pageCusList"></param>
        public void DoDsWhCustomerQuery(ref Model.Pager<CBDsWhCustomer> pageCusList)
        {
            IDsWhCustomerDao.Instance.DoDsWhCustomerQuery(ref pageCusList);
        }

        public DsWhCustomer GetCustomerByCusCode(string CusCode)
        {
            return IDsWhCustomerDao.Instance.GetCustomerByCusCode(CusCode);
        }
    }
}
