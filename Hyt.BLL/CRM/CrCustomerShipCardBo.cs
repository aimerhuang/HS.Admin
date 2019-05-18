using Hyt.DataAccess.CRM;
using Hyt.Model;
using Hyt.Model.Generated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.CRM
{
    /// <summary>
    /// 会员卡
    /// </summary>
    /// <remarks>2017-1-16 杨浩 创建</remarks>
    public class CrCustomerShipCardBo : BOBase<CrCustomerShipCardBo>
    {
        /// <summary>
        /// 根据商客户系统编号获取会员卡
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <returns></returns>
        /// <remarks>2017-01-16 杨浩 创建</remarks>
        public string GetCardNumber(int customerSysNo)
        {
            return ICrCustomerShipCardDao.Instance.GetCardNumber(customerSysNo);
        }
        /// <summary>
        /// 根据商会员卡获取客户系统编号
        /// </summary>
        /// <param name="cardNumber">卡号</param>
        /// <remarks>2017-01-16 杨浩 创建</remarks>
        public int GetCustomerSysNo(string cardNumber)
        {
            return ICrCustomerShipCardDao.Instance.GetCustomerSysNo(cardNumber);
        }
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="model">插入的数据模型</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2017-01-16 杨浩 创建</remarks>
        public int Insert(CrCustomerShipCard model)
        {
            int sysNo=ICrCustomerShipCardDao.Instance.Insert(model);
            if (sysNo>0)
            {
                var balanceInfo = BLL.Balance.CrRechargeBo.Instance.GetCrABalanceEntity(model.CustomerSysNo);
                if (balanceInfo == null)
                {
                    var _model = new CrAccountBalance()
                    {
                        CustomerSysNo=model.CustomerSysNo,
                        AvailableBalance=0,  
                        FrozenBalance=0,
                        TolBlance=0,
                        Remark="",
                        State=0,  
                        AddTime=DateTime.Now,
                    };
                    BLL.Balance.CrRechargeBo.Instance.CreateCrAccountBalance(_model);
                }
            }
            return sysNo;
         
        }
    }
}
