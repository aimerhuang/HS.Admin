using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hyt.Model;
using Hyt.DataAccess.Base;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;

namespace Hyt.DataAccess.Web
{
    /// <summary>
    /// 客户数据访问 抽象类
    /// </summary>
    /// <remarks>2013-08-12 苟治国 创建</remarks>
    public abstract class ICrCustomerDao : DaoBase<ICrCustomerDao>
    {
        ///<summary>
        ///根据手机获取用户
        /// </summary>
        /// <param name="mobile">手机.</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-08-12 苟治国 创建
        /// </remarks>
        public abstract CrCustomer GetCustomerByCellphone(string mobile);

        ///<summary>
        ///根据呢称获取用户
        /// </summary>
        /// <param name="nickName">呢称.</param>
        /// <param name="sysNo">客户编号.</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-08-16 苟治国 创建
        /// </remarks>
        public abstract CrCustomer GetCustomerByName(string nickName,int sysNo);

        /// <summary>
        /// 获取用户收货地址列表
        /// </summary>
        /// <param name="customerSysno">用户编号</param>       
        /// <returns>收货地址列表</returns>
        /// <remarks>
        /// 2013-08-12 唐永勤 创建
        /// </remarks>
        public abstract IList<CBCrReceiveAddress> GetCustomerReceiveAddress(int customerSysno);

        /// <summary>
        /// 获取用户收货地址
        /// </summary>
        /// <param name="receiveSysno">收货地址编号</param>       
        /// <returns>收货地址</returns>
        /// <remarks>
        /// 2013-08-12 唐永勤 创建
        /// </remarks>
        public abstract CBCrReceiveAddress GetCustomerReceiveAddressBySysno(int receiveSysno);

        /// <summary>
        /// 更新客户
        /// </summary>
        /// <param name="model">客户资料</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-11-11 苟治国 创建</remarks>
        public abstract int Update(Model.CrCustomer model);

        /// <summary>
        /// 更新登录时间、Ip
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <param name="lastLoginIp">客户ip</param>
        /// <returns>空</returns>
        /// <remarks>2013-12-20 苟治国 创建</remarks>
        public abstract void UpdateDateTimeAndIp(int sysNo, string lastLoginIp);

        /// <summary>
        /// 更新会员密码
        /// </summary>
        /// <param name="sysno">编号</param>
        /// <param name="passWord">密码</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－09-24 苟治国 创建</remarks>
        public abstract int UpdatePassWord(int sysno, string passWord);
        /// <summary>
        /// 更新分销商等级
        /// </summary>
        /// <param name="sysno">编号</param>
        /// <param name="gradeId">分销等级</param>
        /// <returns></returns>
        /// <remarks>2016-12-10 杨浩 创建</remarks>
        public abstract int UpdateSellBusinessGradeId(int sysno, int gradeId);
    }
}
