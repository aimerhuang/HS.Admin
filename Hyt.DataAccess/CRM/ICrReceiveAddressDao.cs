using Hyt.DataAccess.Base;
using Hyt.Model;
using System.Collections.Generic;
using Hyt.Model.Transfer;

namespace Hyt.DataAccess.CRM
{
    /// <summary>
    /// 会员收货地址
    /// </summary>
    /// <remarks>2013－07-02 余勇 创建</remarks>
    public abstract class ICrReceiveAddressDao : DaoBase<ICrReceiveAddressDao>
    {
        /// <summary>
        /// 修改会员收货地址
        /// </summary>
        /// <param name="address">收货地址实体</param>
        /// <returns>影响行数</returns>
        /// <remarks>2013－07-02 余勇 创建</remarks>
        public abstract int UpdateReceiveAddress(Model.CrReceiveAddress address);

        /// <summary>
        /// 添加会员收货地址
        /// </summary>
        /// <param name="address">收货地址实体</param>
        /// <returns>收货地址系统编号</returns>
        /// <remarks>2013－07-02 余勇 创建</remarks>
        public abstract int InsertReceiveAddress(Model.CrReceiveAddress address);

        /// <summary>
        /// 根据系统号获取收货地址模型
        /// </summary>
        /// <param name="sysNo">收货地址系统号</param>
        /// <returns>收货地址模型数据</returns>
        /// <remarks>2013-08-05 杨晗 创建</remarks>
        public abstract CrReceiveAddress GetCrReceiveAddress(int sysNo);

        /// <summary>
        /// 根据用户系统号获取收货地址
        /// </summary>
        /// <param name="customerSysNo">用户系统号</param>
        /// <param name="num">获取头几条数</param>
        /// <returns>用户收货地址列表</returns>
        /// <remarks>2013-08-05 杨晗 创建</remarks>
        public abstract IList<CBCrReceiveAddress> GetCrReceiveAddressByCustomerSysNo(int customerSysNo, int num);

        /// <summary>
        /// 删除用户收货地址
        /// </summary>
        /// <param name="sysNo">收货地址系统号</param>
        /// <returns>是否成功</returns>
        /// <remarks>2013-08-05 杨晗 创建</remarks>
        public abstract bool Delete(int sysNo);

        /// <summary>
        /// 设置某用户收货地址全部为非默认
        /// </summary>
        /// <param name="customerSysNo">用户系统号</param>
        /// <returns>成功或失败信息</returns>
        /// <remarks>2013-08-05 杨晗 创建</remarks>
        public abstract bool SetUsedAddress(int customerSysNo);

        /// <summary>
        /// 将某个地址设为该用户的默认收货地址
        /// </summary>
        /// <param name="sysNo">收货地址系统号</param>
        /// <param name="customerSysNo">用户系统号</param>
        /// <returns>成功或失败信息</returns>
        /// <remarks>2013-08-26 周瑜 创建</remarks>
        public abstract bool SetDefaultAddress(int sysNo, int customerSysNo);
    }
}
