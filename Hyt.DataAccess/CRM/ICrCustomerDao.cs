using System;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Transfer;
using System.Collections.Generic;
using Hyt.Model.SellBusiness;

namespace Hyt.DataAccess.CRM
{
    /// <summary>
    /// 用户相关接口
    /// </summary>
    /// <remarks>
    /// 更新会员信息的方法需要排除经验积分 等级积分 惠源币,禁止直接更新
    /// 接口中 UpdateExperienceCoin UpdateExperiencePoint UpdateLevelPoint 禁止在BO中实现
    /// </remarks>
    public abstract class ICrCustomerDao : DaoBase<ICrCustomerDao>
    {
        /// <summary>
        /// 更新客户系统上下级关系
        /// </summary>
        /// <param name="sysNo">客户系统编号</param>
        /// <param name="customerSysNos">上下级关系码</param>
        /// <returns></returns>
        /// <remarks>2016-5-25 杨浩 创建</remarks>
        public abstract int UpdateCustomerSysNos(int sysNo,string customerSysNos);
        /// <summary>
        /// 更新客户可提返点
        /// </summary>
        /// <param name="brokerage">可提返点,可为负数</param>
        /// <param name="sysNo">客户系统编号</param>
        /// <returns></returns>
        /// <remarks>2016-5-13 杨浩 创建</remarks>
        public abstract int UpdateCustomerBrokerage(decimal brokerage,int sysNo);
        /// <summary>
        /// 获取会员详细信息
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>会员信息</returns>
        /// <remarks>2013－06-09 黄志勇 创建</remarks>
        /// <remarks>2013-07-11 黄波 修改</remarks>
        public abstract CrCustomer GetCrCustomerItem(int sysNo);

        /// <summary>
        /// 根据会员等级ID获取等级信息
        /// </summary>
        /// <param name="sysNo">会员等级ID</param>
        /// <returns>等级信息</returns>
        /// <remarks>2013－07-01 黄志勇 创建</remarks>
        public abstract Model.CrCustomerLevel GetCustomerLevel(int sysNo);

        /// <summary>
        /// 根据会员姓名搜索会员列表(模糊查询)
        /// </summary>
        /// <param name="name">会员姓名</param>
        /// <param name="rownum">最大条数</param>
        /// <returns>会员列表</returns>
        /// <remarks>2013－07-02 黄志勇 创建</remarks>
        public abstract IList<CrCustomer> SearchCustomerByName(string name, int rownum);

        /// <summary>
        /// 获取会员详细信息
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>会员信息</returns>
        /// <remarks>2013-07-15 苟治国 创建</remarks>
        public abstract CBCrCustomer GetModel(int sysNo);

        /// <summary>
        /// 更新会员
        /// </summary>
        /// <param name="model"></param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－07-15 苟治国 创建</remarks>
        public abstract int Update(Model.CrCustomer model);

        /// <summary>
        /// 根据条件获取会员列表
        /// </summary>
        /// <param name="pager">会员查询条件</param>
        /// <returns>会员列表</returns>
        /// <remarks>2013－07-15 苟治国 创建</remarks>
        public abstract Pager<CBCrCustomer> Seach(Pager<CBCrCustomer> pager);

        /// <summary>
        /// 根据手机搜索会员列表
        /// </summary>
        /// <param name="mobile">手机</param>
        /// <returns>会员列表</returns>
        /// <remarks>2013－06-09 黄志勇 创建</remarks>
        public abstract IList<CrCustomer> SearchCustomerByMobile(string mobile);

        /// <summary>
        /// 根据帐号搜索会员列表（模糊查询）
        /// </summary>
        /// <param name="account">帐号</param>
        /// <param name="rownum">返回条数</param>
        /// <returns>会员列表</returns>
        /// <remarks>2013－7-25 黄志勇 修改</remarks>
        public abstract IList<CrCustomer> SearchCustomerByAccount(string account, int rownum);

        /// <summary>
        /// 获取中级会员、高级会员
        /// </summary>
        /// <returns>会员列表</returns>
        /// <remarks>2013－11-8 苟治国 创建</remarks>
        public abstract IList<CrCustomer> SearchCustomerList();

        /// <summary>
        /// 根据经销商筛选会员
        /// </summary>
        /// <param name="word">关键字</param>
        /// <param name="dealer">经销商id</param>
        /// <returns></returns>
        public abstract IList<CrCustomer> SearchCustomer(string word, int dealer);

        /// <summary>
        /// 创建会员
        /// </summary>
        /// <param name="customer">会员</param>
        /// <returns>会员id</returns>
        /// <remarks>2013－06-09 黄志勇 创建</remarks>
        public abstract int CreateCustomer(CrCustomer customer);

        /// <summary>
        /// 创建收货地址
        /// </summary>
        /// <param name="address">收货地址</param>
        /// <returns>收货地址</returns>
        /// <remarks>2013－07-11 黄志勇 创建</remarks>
        public abstract CrReceiveAddress CreateCustomerReceiveAddress(CrReceiveAddress address);

        /// <summary>
        /// 查询用户的收货地址
        /// </summary>
        /// <param name="customerSysNo">会员ID</param>
        /// <returns>收货地址列表</returns>
        /// <remarks>2013－06-09 黄志勇 创建</remarks>
        public abstract IList<CrReceiveAddress> LoadCustomerAddress(int customerSysNo);

        /// <summary>
        /// 查询用户的默认收货地址
        /// </summary>
        /// <param name="customerSysNo">会员ID</param>
        /// <returns>收货地址列表</returns>
        /// <remarks>2014-03-18 周唐炬 创建</remarks>
        public abstract CrReceiveAddress LoadCustomerDefaultAddress(int customerSysNo);

        /// <summary>
        /// 根据收货地址ID获取收货地址
        /// </summary>
        /// <param name="sysNo">地址</param>
        /// <returns>收货地址</returns>
        /// <remarks>2013－06-09 黄志勇 创建</remarks>
        public abstract CrReceiveAddress GetCustomerAddressBySysNo(int sysNo);

        /// <summary>
        /// 根据会员id查询默认收货地址
        /// </summary>
        /// <param name="customerSysNo">会员id</param>
        /// <returns>默认收货地址</returns>
        /// <remarks>2013-07-1 黄志勇  创建</remarks>
        public abstract CrReceiveAddress SearchReceiveAddressByCustomerSysNo(int customerSysNo);

        /// <summary>
        /// 根据用户账号获取前台用户信息
        /// </summary>
        /// <param name="account">用户账号</param>
        /// <returns>返回前台用户</returns>
        /// <remarks>2013-07-09 杨浩 创建</remarks>
        public abstract CBCrCustomer GetCrCustomer(string account);
        /// <summary>
        /// 判断分销商是否包含该会员
        /// </summary>
        /// <param name="SysNo"></param>
        /// <param name="DealerSysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-1-20 王耀发 创建</remarks>
        public abstract CrCustomer GetCustomerBySysNoDearler(int SysNo, int DealerSysNo);

        /// <summary>
        /// 更新会员头像
        /// </summary>
        /// <param name="sysno">会员系统编号</param>
        /// <param name="imagePath">头像图片地址</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－08-28 周瑜 创建</remarks>
        [Obsolete("作废")]
        public abstract int UpdateHeadImage(int sysno, string imagePath);

        /// <summary>
        /// 获取会员头像
        /// </summary>
        /// <param name="sysno">会员系统编号</param>
        /// <returns>头像的地址</returns>
        /// <remarks>2013－08-29 周瑜 创建</remarks>
        public abstract string GetHeadImage(int sysno);
        /// <summary>
        /// 查询所有的用户
        /// </summary>     
        /// <returns>用户信息列表</returns>
        /// <remarks> 
        /// 2015-09-19 王耀发 创建
        public abstract IList<CrCustomer> GetCrCustomerList();

        /// <summary>
        /// 执行分销商返利
        /// </summary>
        /// <param name="recommendSysNo">直接推荐人系统编号</param>
        /// <param name="complySysNo">消费客户系统编号</param>
        /// <param name="indirect2Id">间2推荐人系统编号</param>
        /// <param name="indirectId">间接推荐人系统编号</param>
        /// <param name="orderSysNo">订单系统编号</param>
        /// <param name="action">0:注册 1:关注 2:购物</param>
        /// <param name="goodsAmount">商品总金额</param>
        /// <returns></returns>
        /// <remarks>2015－09-11 杨浩 创建 2015-10-28 王耀发引用</remarks>
        public abstract CrSellBusinessRebatesResult ExecuteSellBusinessRebates(int recommendSysNo, int complySysNo, int indirect2Id, int indirectId, int orderSysNo, string action, decimal goodsAmount);
        /// <summary>
        /// 更新会员对应 Brokerage，可提佣金 BrokerageFreeze，冻结佣金
        /// </summary>
        /// <param name="SysNo"></param>
        /// <param name="Value"></param>
        /// <remarks>2016-1-8 王耀发引用</remarks>
        public abstract void UpdateCustomerValue(int SysNo, decimal Value);

        /// <summary>
        /// 更新会员对应 BrokerageFreeze，冻结佣金
        /// </summary>
        /// <param name="SysNo"></param>
        /// <param name="Value"></param>
        /// <remarks>2016-1-8 王耀发引用</remarks>
        public abstract void UpdateCustomerValueConfirm(int SysNo, decimal Value);

        /// <summary>
        /// 获取分销商对应的会员
        /// </summary>
        /// <param name="DealerSysNo">分销商系统编号</param>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>
        /// <remarks>2016-1-20 王耀发 创建</remarks>
        /// <remarks>2016-4-29 刘伟豪 添加关键字搜索</remarks>
        public abstract IList<CrCustomer> GetCrCustomerListByDealerSyNo(int DealerSysNo, string keyword = "");

        public abstract List<CrCustomer> GetCrCustomerListByDealerSyNoQuery(int DealerSysNo);

        public abstract List<CBCrCustomer> SeachCanBeParentList(int id, string keyword = "");

        public abstract bool CustomerToParent(int cSysNo, int pSysNo);

        public abstract bool UpdateInviteAndIndirectNum(int customerSysNo);

        public abstract void UpdateIsSellBusiness(int SysNo, string IsSellBusiness);

        public abstract bool UpdateSellBusinessGrade(int id, int gid);

        /// <summary>
        /// 获取所有会员信息
        /// </summary>
        /// <returns>会员信息集合</returns>
        /// <remarks>2015-09-10 王耀发 创建</remarks>
        public abstract IList<CrCustomer> GetAllCustomer();

        /// <summary>
        /// 新增会员信息
        /// </summary>
        /// <param name="models">商品信息列表</param>
        /// <returns>空</returns>
        /// <remarks>2015-09-10 王耀发 创建</remarks>
        public abstract void CreateCrCustomer(List<CrCustomer> models);

        /// <summary>
        /// 更新会员信息
        /// </summary>
        /// <param name="models">会员信息列表</param>
        /// <returns>空</returns>
        /// <remarks>2015-09-10 王耀发 创建</remarks>
        public abstract void UpdateExcelCrCustomer(List<CrCustomer> models);
        /// <summary>
        /// 更新用户冻结佣金
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="amount">金额</param>
        /// <returns></returns>
        /// <remarks>2017-1-10 杨浩 创建</remarks>
        public abstract bool UpdateCustomerBrokerageFreeze(int customerSysNo, decimal amount);

  
    }
}
