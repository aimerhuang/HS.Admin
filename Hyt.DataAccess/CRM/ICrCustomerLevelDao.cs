using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Transfer;

namespace Hyt.DataAccess.CRM
{
    /// <summary>
    /// 会员等级表数据访问接口
    /// </summary>
    /// <remarks>2013-06-27 邵斌 创建</remarks>
    public abstract class ICrCustomerLevelDao : DaoBase<ICrCustomerLevelDao>
    {
        /// <summary>
        /// 读取全部会员等级列表
        /// </summary>
        /// <returns>分会会员等级列表（全部）</returns>
        /// <remarks>2013-06-27 邵斌 创建</remarks>
        public abstract IList<CrCustomerLevel> List();

        /// <summary>
        /// 获取单个最高会员等级信息
        /// </summary>
        /// <returns>返回会员等级对象</returns>
        /// <remarks>2013-12-18 苟治国 创建</remarks>
        public abstract CrCustomerLevel GetCustomerUpperLevel();

        /// <summary>
        /// 获取单个会员等级信息
        /// </summary>
        /// <param name="sysNo">等级编号</param>
        /// <returns>返回会员等级对象</returns>
        /// <remarks>2013-06-27 邵斌 创建</remarks>
        public abstract CrCustomerLevel GetCustomerLevel(int sysNo);
        /// <summary>
        /// 获取单个会员等级信息
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        public abstract CBCrCustomerLevelImgUrl GetCustomerLevelImgUrl(int sysNo);
        /// <summary>
        /// 根据会员等级ID查询会员等级图标表
        /// </summary>
        /// <param name="CrCustomerLevelSysNo"></param>
        /// <returns></returns>
        public abstract CBCrCustomerLevelImgUrl GetCrLevelImgUrl(int CrCustomerLevelSysNo);
        /// <summary>
        /// 根据条件获取会员等级的列表
        /// </summary>
        /// <param name="CanPayForProduct">惠源币是否可用于支付货款</param>
        /// <param name="CanPayForService">惠源币是否可用于支付服务</param>
        /// <returns>会员等级列表</returns>
        /// <remarks>2013－07-10 苟治国 创建</remarks>
        public abstract IList<Model.CrCustomerLevel> Seach(int? CanPayForProduct, int? CanPayForService);

        /// <summary>
        /// 会员等级区间比较
        /// </summary>
        /// <param name="sysNo">会员等级编号</param>
        /// <returns>会员等级列表</returns>
        /// <remarks>2013－07-11 苟治国 创建</remarks>
        public abstract IList<Model.CrCustomerLevel> compare(int sysNo);

        /// <summary>
        /// 插入会员等级
        /// </summary>
        /// <param name="model"></param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－07-10 苟治国 创建</remarks>
        public abstract int Insert(Model.CrCustomerLevel model);

        /// <summary>
        /// 更新会员等级
        /// </summary>
        /// <param name="model"></param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－07-10 苟治国 创建</remarks>
        public abstract int Update(Model.CrCustomerLevel model);

         /// <summary>
        /// 插入会员等级图标表数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public abstract int InsertCrCustomerLevelImgUrl(CrCustomerLevelImgUrl model);
        /// <summary>
        /// 更新会员等级图标表数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public abstract int UpdateCrCustomerLevelImgUrl(CrCustomerLevelImgUrl model);
          /// <summary>
        /// 更新等级图标
        /// </summary>
        /// <param name="CrCustomerLevelSysNo"></param>
        /// <param name="CrCustomerLevelImgUrl"></param>
        /// <returns></returns>
        public abstract int UpdateCrCustomerLevelImgUrl(int CrCustomerLevelSysNo, string CrCustomerLevelImgUrl);
    }
}
