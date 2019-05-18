using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;
namespace Hyt.DataAccess.Order
{
    /// <summary>
    /// 订单收货地址
    /// </summary>
    /// <remarks>2013-06-13 朱成果 创建</remarks>
    public abstract class ISoReceiveAddressDao:DaoBase<ISoReceiveAddressDao>
    {
        /// <summary>
        /// 获取订单的收货地址信息
        /// </summary>
        /// <param name="SysNo">编号</param>
        /// <returns></returns>
        /// <remarks>2013-06-13 朱成果 创建</remarks>
        public abstract SoReceiveAddress GetOrderReceiveAddress(int sysNo);

        /// <summary>
        /// 保存订单收货地址
        /// </summary>
        /// <param name="entity">订单收货地址实体</param>
        /// <returns>订单收货地址实体（带编号)</returns>
        /// <remarks>2013-06-27 黄志勇 创建</remarks>
        public abstract SoReceiveAddress InsertEntity(SoReceiveAddress entity);

        /// <summary>
        /// 更新收货地址信息
        /// </summary>
        /// <param name="entity">订单收货地址实体</param>
        /// <returns></returns>
        /// <remarks>2013-06-27 朱成果 创建</remarks>
        public abstract void UpdateEntity(SoReceiveAddress entity);

        /// <summary>
        /// 删除收货地址信息
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-07-13 王耀发 创建</remarks>
        public abstract int DeleteEntity(int sysNo);

        /// <summary>
        /// 判断当前收货信息是否已经存在
        /// </summary>
        /// <param name="name">收货人</param>
        /// <param name="mobliePhoneNumber">收货人手机号</param>
        /// <param name="areaSysNo">收货区域信息</param>
        /// <param name="streetAddress">街道地址</param>
        /// <param name="sIDCardNo">身份证</param>
        /// <returns></returns>
        /// <remarks>2016-5-9 杨浩 创建</remarks>
        public abstract int ExistReceiveAddress(string name, string mobliePhoneNumber, string areaSysNo, string streetAddress, string sIDCardNo);

        /// <summary>
        /// 获取收货地址列表
        /// </summary>
        /// <param name="SysNos"></param>
        /// <returns></returns>
        public abstract List<CBSoReceiveAddress> GetOrderReceiveAddressByList(string SysNos);
        
    }
}
