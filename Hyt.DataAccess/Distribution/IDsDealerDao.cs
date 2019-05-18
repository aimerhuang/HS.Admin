using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Model.Generated;

namespace Hyt.DataAccess.Distribution
{
    /// <summary>
    /// 分销商信息维护接口层
    /// </summary>
    /// <remarks>
    /// 2013-09-04 郑荣华 创建
    /// </remarks>
    public abstract class IDsDealerDao : DaoBase<IDsDealerDao>
    {
        #region 操作

        /// <summary>
        /// 创建分销商
        /// </summary>
        /// <param name="model">分销商实体</param>
        /// <returns>新加的系统编号</returns>
        /// <remarks>
        /// 2013-09-04 郑荣华 创建
        /// </remarks>
        public abstract int Create(DsDealer model);

        /// <summary>
        /// 修改分销商
        /// </summary>
        /// <param name="model">分销商实体</param>
        /// <returns>受影响的行数</returns>
        /// <remarks>
        /// 2013-09-04 郑荣华 创建
        /// </remarks>
        public abstract int Update(DsDealer model);

        /// <summary>
        /// 分销商状态更新
        /// </summary>
        /// <param name="sysNo">分销商系统编号</param>
        /// <param name="status">分销商状态</param>
        /// <param name="lastUpdateBy">最后更新人</param>
        /// <returns>受影响的行数</returns>
        /// <remarks>
        /// 2013-09-04 郑荣华 创建
        /// </remarks>
        public abstract int UpdateStatus(int sysNo, DistributionStatus.分销商状态 status, int lastUpdateBy);

        #endregion

        #region 查询
        /// <summary>
        /// 根据企业编号获取加盟商信息
        /// </summary>
        /// <param name="enterpriseID">企业编号</param>
        /// <returns>经销商信息</returns>
        /// <remarks>2015-01-29 杨浩 创建</remarks>
        public abstract DsDealer GetDealerByEnterpriseID(int enterpriseID);
        /// <summary>
        /// 通过过滤条件获取分销商列表
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <returns>分销商列表</returns>
        ///<remarks>2013-09-03 周唐炬 创建</remarks>
        public abstract Pager<CBDsDealer> GetDealerList(ParaDealerFilter filter);

        /// <summary>
        /// 根据系统编号获取分销商信息
        /// </summary>
        /// <param name="sysNo">分销商系统编号</param>
        /// <returns>分销商信息</returns>
        /// <remarks>
        /// 2013-09-04 郑荣华 创建
        /// </remarks>
        public abstract CBDsDealer GetDsDealer(int sysNo);

        /// <summary>
        /// 根据订单编号获取分销商信息
        /// </summary>
        /// <param name="sysNo">订单系统编号</param>
        /// <returns>分销商信息</returns>
        /// <remarks>
        /// 2013-05-19 罗勤尧 创建
        /// </remarks>
        public abstract DsDealerLiJiaEdit GetCBDsDealerByOrderId(int sysNo);
        /// <summary>
        /// 根据订单编号获取分销商信息
        /// </summary>
        /// <param name="sysNo">订单系统编号</param>
        /// <returns>分销商信息</returns>
        /// <remarks>
        /// 2013-05-19 罗勤尧 创建
        /// </remarks>
        public abstract DsDealer GetDsDealerByOrderSysNo(int sysNo);
        /// <summary>
        /// 根据系统编号获取分销商信息
        /// </summary>
        /// <param name="sysNo">分销商系统编号</param>
        /// <returns>分销商信息</returns>
        /// <remarks>
        /// 2013-09-04 郑荣华 创建
        /// </remarks>
        public abstract DsDealerLiJiaEdit GetDsDealerLiJia(int sysNo);
        /// <summary>
        /// 根据系统编号获取分销商信息
        /// </summary>
        /// <param name="sysNo">分销商系统编号</param>
        /// <returns>分销商信息</returns>
        /// <remarks>
        /// 2013-09-04 郑荣华 创建
        /// </remarks>
        public abstract CBDsDealer GetCBDsDealer(int sysNo);

        /// <summary>
        /// 分页查询分销商信息列表
        /// </summary>
        /// <param name="pager">分销商信息列表分页对象</param>
        /// <param name="filter">查询条件</param>
        /// <remarks> 
        /// 2013-09-04 郑荣华 创建
        /// </remarks>
        public abstract void GetDsDealerList(ref Pager<CBDsDealer> pager, ParaDsDealerFilter filter);

        /// <summary>
        /// 查询分销商信息
        /// </summary>
        /// <param name="filter">查询参数实体</param>
        /// <returns>分销商信息列表</returns>
        /// <remarks> 
        /// 2013-09-04 郑荣华 创建 
        /// </remarks>   
        public abstract IList<CBDsDealer> GetDsDealerList(ParaDsDealerFilter filter);
        /// <summary>
        /// 判断是否有分销商重复
        /// </summary>
        /// <param name="AppID"></param>
        /// <param name="sysNo"></param>
        /// <remarks> 
        /// 2015-09-05 王耀发 创建 
        /// </remarks>   
        public abstract IList<CBDsDealer> GetDsDealerListByDealerName(string DealerName, int sysNo);
        /// <summary>
        /// 用于更新检查用户系统编号不重复，查询分销商信息
        /// </summary>
        /// <param name="userSysNo">用户系统编号</param>
        /// <param name="sysNo">要排除的分销商系统编号</param>
        /// <returns>分销商信息列表</returns>
        /// <remarks> 
        /// 2013-09-05 郑荣华 创建 
        /// </remarks>   
        public abstract IList<CBDsDealer> GetDsDealerList(int userSysNo, int sysNo);
        /// <summary>
        /// 判断是否有AppID重复
        /// </summary>
        /// <param name="AppID"></param>
        /// <param name="sysNo"></param>
        /// <remarks> 
        /// 2015-09-05 王耀发 创建 
        /// </remarks>   
        public abstract IList<CBDsDealer> GetDsDealerListByAppID(string AppID, int sysNo);
        /// <summary>
        /// 判断是否有AppSecret重复
        /// </summary>
        /// <param name="AppSecret"></param>
        /// <param name="sysNo"></param>
        /// <remarks> 
        /// 2015-09-05 王耀发 创建 
        /// </remarks>   
        public abstract IList<CBDsDealer> GetDsDealerListByAppSecret(string AppSecret, int sysNo);
        /// <summary>
        /// 判断是否有WeiXinNum重复
        /// </summary>
        /// <param name="WeiXinNum"></param>
        /// <param name="sysNo"></param>
        /// <remarks> 
        /// 2015-09-05 王耀发 创建 
        public abstract IList<CBDsDealer> GetDsDealerListByWeiXinNum(string WeiXinNum, int sysNo);
        /// <summary>
        /// 判断是否有DomainName重复
        /// </summary>
        /// <param name="DomainName"></param>
        /// <param name="sysNo"></param>
        /// <remarks> 
        /// 2015-09-05 王耀发 创建 
        public abstract IList<CBDsDealer> GetDsDealerListByDomainName(string DomainName, int sysNo);
        /// <summary>
        /// 查询所有分销商信息
        /// </summary>
        /// <returns>分销商信息列表</returns>
        /// <remarks> 
        /// 2013-09-04 郑荣华 创建 
        /// </remarks>      
        public abstract IList<CBDsDealer> GetDsDealerList();

        /// <summary>
        /// 根据系统用户编号获取分销商信息
        /// </summary>
        /// <param name="userSysNo">系统用户编号</param>
        /// <returns>分销商信息</returns>
        /// <remarks>
        /// 2013-09-09 余勇 创建
        /// </remarks>
        public abstract CBDsDealer GetDsDealerByUserNo(int userSysNo);

        /// <summary>
        /// 根据分销商用户编号获取分销商信息
        /// </summary>
        /// <param name="dsUserSysNo">分销商用户编号</param>
        /// <returns>分销商信息</returns>
        /// <remarks>
        /// 2014-06-09 余勇 创建
        /// </remarks>
        public abstract DsDealer GetDsDealerByDsUser(int dsUserSysNo);

        /// <summary>
        /// 根据仓库编号获取分销商信息
        /// </summary>
        /// <param name="warehousSysNo">仓库编号</param>
        /// <returns>分销商信息</returns>
        /// <remarks>
        /// 2014-06-09 余勇 创建
        /// </remarks>
        public abstract DsDealer GetDsDealerByWarehousSysNo(int warehousSysNo);

        /// <summary>
        /// 根据名称获取分销商
        /// </summary>
        /// <param name="DealerName"></param>
        /// <returns></returns>
        /// <remarks>
        /// 2016-06-09 王耀发 创建
        /// </remarks> 
        public abstract DsDealer GetDsDealerByName(string DealerName);

        /// <summary>
        /// 获取所有分销商信息
        /// </summary>
        /// <returns>分销商数据列表</returns>
        /// 2015-09-19 王耀发 创建
        public abstract IList<DsDealer> GetAllDealerList();

        /// <summary>
        /// 获取用户有可管理的所有分销商
        /// </summary>
        /// <param name="userSysNo">用户系统编号.</param>
        /// <returns>分销商集合</returns>
        /// <remarks>
        /// 2015-09-19 王耀发 创建
        public abstract IList<DsDealer> GetUserDealerList(int userSysNo);

        /// <summary>
        /// 查询所有分销商信息
        /// </summary>
        /// <returns>分销商信息列表</returns>
        /// <remarks> 
        /// 2015-12-31 王耀发 创建 
        /// </remarks>      
        public abstract IList<DsDealer> GetDsDealersList();

        /// <summary>
        /// 获得当前用户有权限看到的分销商
        /// 2016-1-4 王耀发 创建
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public abstract IList<DsDealer> GetDealersListByCurUser(ParaDsDealerFilter filter);

        /// <summary>
        /// 获得创建用户对应的分销商
        /// 2016-1-29 王耀发 创建
        /// </summary>
        /// <param name="DealerCreatedBy"></param>
        /// <param name="Type">当前登录账号类型 F：为分销商</param>
        /// <param name="TypeSysNo"></param>
        /// <returns></returns>
        public abstract IList<DsDealer> GetDealersListByCreatedBy(int DealerCreatedBy, string Type, int TypeSysNo);
        #endregion

        /// <summary>
        /// 获得当前经销商树形图
        /// </summary>
        /// <param name="DealerSysNo"></param>
        /// <returns></returns>
        /// 2016-1-20 王耀发 创建
        public abstract IList<CBDsDealer> GetDealerTreeList(string Type, int TypeSysNo);

        /// <summary>
        /// 获得代理商列表
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="TypeSysNo"></param>
        /// <returns></returns>
        /// 2016-1-29 王耀发 创建
        public abstract IList<CBDsDealer> GetDaiLiShangList(string Type, int TypeSysNo);
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="State">状态</param>
        /// <param name="pager">分页对象</param>
        /// <returns>2016-03-30 周海鹏 创建</returns>
        public abstract IList<DsApplyStore> List(int State, Pager<DsApplyStore> pager);
         /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="sysNoItems">ID组</param>
        /// <returns></returns>
        public abstract int Update(string sysNoItems);

        /// <summary>
        /// 更新微信AppID、AppSecret
        /// </summary>
        /// <param name="SysNo">系统编号</param>
        /// <param name="AppID">AppID</param>
        /// <param name="AppSecret">AppSecret</param>
        /// <returns>受影响的行数</returns>
        /// <remarks>
        /// 2016-05-10 王耀发 创建
        /// </remarks>
        public abstract int UpdateAppIDandSecret(int SysNo, string AppID, string AppSecret);
        /// <summary>
        /// 更新微信利嘉会员id
        /// </summary>
        /// <param name="LiJiaSysNo">利嘉会员系统编号</param>
        /// <returns>受影响的行数</returns>
        /// <remarks>
        /// 2017-05-19 罗勤尧 创建
        public abstract int UpdateLiJiaSysNo(int LiJiaSysNo, int SysNo);

        /// <summary>
        /// 根据分销商获取店铺
        /// </summary>
        /// <param name="DealerSysNo"></param>
        /// <returns></returns>
        public abstract IList<WhWarehouse> WhWarehouseList(int DealerSysNo);
    }
}
