using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.BaseInfo;
using Hyt.DataAccess.CRM;
using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Util.Validator;
using Hyt.Util.Validator.Rule;

namespace Hyt.BLL.CRM
{
    /// <summary>
    /// 用户收货地址业务类
    /// </summary>
    /// <remarks>2013-08-05 杨晗 创建</remarks>
    public class CrReceiveAddressBo : BOBase<CrReceiveAddressBo>
    {
        /// <summary>
        /// 验证收货地址是否合法
        /// </summary>
        /// <param name="model">收货地址实体</param>
        /// <param name="message">消息</param>
        /// <returns></returns>
        /// <remarks>2013-12-2 黄波 创建</remarks>
        public bool ValidReceiveAddress(Model.CrReceiveAddress model,out string message)
        {
            message = "";
            if (!VHelper.ValidatorRule(new Rule_NotAllowNull(model.Name, "必须添加收货人姓名!")).IsPass)
            {
                message = "必须添加收货人姓名!";
                return false;
            }
            if (!VHelper.ValidatorRule(new Rule_NotAllowNull(model.StreetAddress, "必须添加收货地址!")).IsPass)
            {
                message = "必须添加收货地址!";
                return false;
            }
            if (!VHelper.ValidatorRule(new Rule_Mobile(model.MobilePhoneNumber)).IsPass && !VHelper.ValidatorRule(new Rule_Telephone(model.PhoneNumber)).IsPass)
            {
                message = "手机或固定号码必须填写一项!";
                return false;
            }
            if (Hyt.BLL.Basic.BasicAreaBo.Instance.GetArea(model.AreaSysNo) == null)
            {
                message = "请选择正确的所在地区!";
                return false;
            }
            return true;
        }

        /// <summary>
        /// 根据系统号获取收货地址模型
        /// </summary>
        /// <param name="sysNo">收货地址系统号</param>
        /// <returns>收货地址模型数据</returns>
        /// <remarks>2013-08-05 杨晗 创建</remarks>
        public CrReceiveAddress GetCrReceiveAddress(int sysNo)
        {
            return ICrReceiveAddressDao.Instance.GetCrReceiveAddress(sysNo);
        }

        /// <summary>
        /// 根据用户系统号获取收货地址
        /// </summary>
        /// <param name="customerSysNo">用户系统号</param>
        /// <param name="num">获取头几条数</param>
        /// <returns>用户收货地址列表</returns>
        /// <remarks>2013-08-05 杨晗 创建</remarks>
        public IList<CBCrReceiveAddress> GetCrReceiveAddressByCustomerSysNo(int customerSysNo, int num)
        {
            var list = ICrReceiveAddressDao.Instance.GetCrReceiveAddressByCustomerSysNo(customerSysNo, num);

            return list;
        }

        /// <summary>
        /// 修改会员收货地址
        /// </summary>
        /// <param name="address">收货地址实体</param>
        /// <returns>影响行数</returns>
        /// <remarks>2013-08-05 杨晗 创建</remarks>
        public int UpdateReceiveAddress(CrReceiveAddress address)
        {
            return ICrReceiveAddressDao.Instance.UpdateReceiveAddress(address);
        }

        /// <summary>
        /// 添加会员收货地址
        /// </summary>
        /// <param name="address">收货地址实体</param>
        /// <returns>收货地址系统编号</returns>
        /// <remarks>2013-08-05 杨晗 创建</remarks>
        public int InsertReceiveAddress(CrReceiveAddress address)
        {
            return ICrReceiveAddressDao.Instance.InsertReceiveAddress(address);
        }

        /// <summary>
        /// 删除用户收货地址
        /// </summary>
        /// <param name="sysNo">收货地址系统号</param>
        /// <returns>是否成功</returns>
        /// <remarks>2013-08-05 杨晗 创建</remarks>
        public bool Delete(int sysNo)
        {
            return ICrReceiveAddressDao.Instance.Delete(sysNo);
        }

        /// <summary>
        /// 设置某用户收货地址全部为非默认
        /// </summary>
        /// <param name="customerSysNo">用户系统号</param>
        /// <returns>成功或失败信息</returns>
        /// <remarks>2013-08-05 杨晗 创建</remarks>
        public bool SetUsedAddress(int customerSysNo)
        {
            return ICrReceiveAddressDao.Instance.SetUsedAddress(customerSysNo);
        }

        /// <summary>
        /// 将某个地址设为该用户的默认收货地址
        /// </summary>
        /// <param name="sysNo">收货地址系统号</param>
        /// <param name="customerSysNo">用户系统号</param>
        /// <returns>成功或失败信息</returns>
        /// <remarks>2013-08-26 周瑜 创建</remarks>
        public bool SetDefaultAddress(int sysNo, int customerSysNo)
        {
            return ICrReceiveAddressDao.Instance.SetDefaultAddress(sysNo, customerSysNo);
        }

        /// <summary>
        /// 获取个收货地址记录的完整地址
        /// </summary>
        /// <param name="addressSysNo"></param>
        /// <returns>完整地址</returns>
        /// <remarks>2013-08-05 杨晗 创建</remarks>
        public string GetCompleteAddress(int addressSysNo)
        {
            var address = ICrReceiveAddressDao.Instance.GetCrReceiveAddress(addressSysNo);
              //优化 var area = IBsAreaDao.Instance.GetArea(address.AreaSysNo);
             //优化 var city = IBsAreaDao.Instance.GetArea(area.ParentSysNo);
            //优化 var province = IBsAreaDao.Instance.GetArea(city.ParentSysNo);
            var area = Hyt.BLL.Basic.BasicAreaBo.Instance.GetArea(address.AreaSysNo);
            var city = Hyt.BLL.Basic.BasicAreaBo.Instance.GetArea(area.ParentSysNo);
            var province = Hyt.BLL.Basic.BasicAreaBo.Instance.GetArea(city.ParentSysNo);
            return string.Format("{0} {1} {2} {3}", province.AreaName, city.AreaName, area.AreaName, address.StreetAddress);
        }
    }
}
