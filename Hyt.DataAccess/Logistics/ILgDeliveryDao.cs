using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Logistics
{
    /// <summary>
    /// 配送单维护 抽象类
    /// </summary>
    /// <remarks>
    /// 2013-06-09 沈强 创建
    /// </remarks>
    public abstract class ILgDeliveryDao : Hyt.DataAccess.Base.DaoBase<ILgDeliveryDao>
    {
        /// <summary>
        /// 根据订单编号获取配送单
        /// </summary>
        /// <param name="sysNo">订单编号</param>
        /// <returns></returns>
        /// <remarks>2016-7-16 杨浩 创建</remarks>
        public abstract List<LgDeliveryItem> GetDeliveryItemByOrderSysNo(int sysNo);
        /// <summary>
        /// 获取配送单明细的物流信息
        /// </summary>
        /// <param name="TransactionSysNo">事务编号</param>
        /// <returns>配送物流内容的</returns>
        /// <remarks>2015-10-08 杨云奕 添加</remarks>
        public abstract IList<LgDeliveryItem> GetLgDeliveryItemListByTransactionSysNo(string TransactionSysNo);
        /// <summary>
        /// 获取配送单集合
        /// </summary>
        /// <param name="pager">分页对象</param>
        /// <param name="currentUser">当前用户系统编号,若传入则查询当前用户用权限的仓库</param>
        /// <param name="hasAllWarehouse">是否拥有所有仓库</param>
        /// <returns></returns>
        /// <remarks>2013-06-09 沈强 创建</remarks>
        public abstract void GetLogisticsDeliveryItems(ref Hyt.Model.Pager<Model.CBLgDelivery> pager, int currentUser, bool hasAllWarehouse);

        /// <summary>
        /// 获取配送单集合
        /// </summary>
        /// <param name="pagerFilter">查询过滤对象</param>
        /// <param name="currentUserSysNo">当前用户系统编号</param>
        /// <param name="hasAllWarehouse">是否拥有所有仓库</param>
        /// <returns>返回配送单列表</returns>
        /// <remarks>2013-10-18 沈强 创建</remarks>
        public abstract Pager<CBLgDelivery> GetLogisticsDeliveryItems(
            Pager<Hyt.Model.Parameter.ParaLogisticsFilter> pagerFilter, int currentUserSysNo, bool hasAllWarehouse);

        /// <summary>
        /// 读取配送单明细
        /// </summary>
        /// <param name="deliverySysNo">配送单sys no.</param>
        /// <returns>配送单明细组合实体列表</returns>
        /// <remarks>
        /// 2013-06-19 沈强 创建
        /// </remarks>
        public abstract IList<CBLgDeliveryItem> GetCBDeliveryItems(int deliverySysNo);

        /// <summary>
        /// 创建配送单
        /// </summary>
        /// <param name="model">配送单主表实体</param>
        /// <returns>返回系统编号</returns>
        /// <remarks>
        /// 2013-6-14 何方 创建
        /// </remarks>
        public abstract int CreateLgDelivery(LgDelivery model);

        /// <summary>
        /// 读取配送单
        /// </summary>
        /// <param name="sysNo">配送单sys no.</param>
        /// <returns>配送单 实体</returns>
        /// <remarks>
        /// 2013/6/17 何方 创建
        /// </remarks>
        public abstract LgDelivery GetDelivery(int sysNo);

        /// <summary>
        /// 获取当天指定的配送单列表
        /// </summary>
        /// <param name="sysNosExcluded">要排除的配送单系统编号集合</param>
        /// <param name="userSysno">用户编号</param>
        /// <returns>当天指定的配送单列表</returns>
        /// <remarks>2013-06-21 黄伟 创建</remarks>
        public abstract Result<List<CBWCFLgDelivery>> GetDeliveryList(List<int> sysNosExcluded, int userSysno);

        /// <summary>
        /// 根据配送单编号获取配送单明细
        /// </summary>
        /// <param name="sysNo">配送单系统编号</param>
        /// <returns>配送单明细列表</returns>
        /// <remarks>2013-12-30 黄伟 创建</remarks>
        public abstract List<LgDeliveryItem> GetItemListByDeliverySysNo(int sysNo);

        /// <summary>
        /// 获取所有配送单集合
        /// </summary>
        /// <param name="userSysNo">用户系统编号</param>
        /// <returns>配送单集合</returns>
        /// <remarks>
        /// 2013-12-30 黄伟 创建
        /// </remarks>   
        public abstract List<CBWCFLgDelivery> GetLgDeliveryListAll(int userSysNo);

        /// <summary>
        /// 更新单据状态
        /// </summary>
        /// <param name="lstStatus">需要更新的单据集合</param>
        /// <param name="user">当前登录用户</param>
        /// <returns>封装的响应实体(状态,状态编码,消息)</returns>
        /// <remarks>2013-06-24 黄伟 创建</remarks>
        /// <remarks>2014-01-14 沈强 修改</remarks>
        public abstract Result UpdateStatus(List<CBWCFStatusUpdate> lstStatus, SyUser user);

        /// <summary>
        /// App签收状态记录数
        /// </summary>
        /// <param name="deliverySysNo">配送单系统编号</param>
        /// <param name="noteSysNo">单据编号(出库单、取货单)</param>
        /// <returns>App签收状态记录数</returns>
        /// <remarks>2014-08-24 周唐炬 创建</remarks>
        public abstract int GetAppSignCount(int deliverySysNo, int noteSysNo);

        /// <summary>
        /// 更新配送单
        /// </summary>
        /// <param name="model">配送单实体</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013/7/14 何方 创建
        /// </remarks>
        public abstract bool Update(LgDelivery model);

        /// <summary>
        /// 读取配送单明细
        /// </summary>
        /// <param name="deliverySysNo">配送单sys no.</param>
        /// <returns>配送单明细实体列表</returns>
        /// <remarks>
        /// 2013-06-17 何方 创建
        /// </remarks>
        public abstract IList<LgDeliveryItem> GetDeliveryItems(int deliverySysNo);

        /// <summary>
        /// 读取配送单明细
        /// </summary>
        /// <param name="syUserSysNo">用户系统编号.</param>
        /// <param name="status">配送单明细状态数组.</param>
        /// <returns>
        /// 配送单明细实体列表
        /// </returns>
        /// <remarks>
        /// 2013-06-17 何方 创建
        /// </remarks>
        public abstract IList<LgDeliveryItem> GetDeliveryItems(int syUserSysNo, int[] status);

        /// <summary>
        /// 读取单个配送单明细
        /// </summary>
        /// <param name="deliveryItemSysNo">配送单明细系统编号.</param>
        /// <returns>配送单明细实体</returns>
        /// <remarks>
        /// 2013-06-17 何方 创建
        /// </remarks>
        public abstract LgDeliveryItem GetDeliveryItem(int deliveryItemSysNo);

        /// <summary>
        /// 读取单个配送单明细
        /// </summary>
        /// <param name="deliverySysNo">配送单系统编号.</param>
        /// <param name="noteSysNo">配送明细单据系统编号.</param>
        /// <param name="noteType">配送明细单据类型.</param>
        /// <returns>
        /// 配送单明细实体
        /// </returns>
        /// <remarks>
        /// 2013-08-20 何方 创建
        /// </remarks>
        public abstract LgDeliveryItem GetDeliveryItem(int deliverySysNo, int noteSysNo, LogisticsStatus.配送单据类型 noteType);

        /// <summary>
        /// 读取单个配送单明细
        /// </summary>
        /// <param name="noteSysNo">配送明细单据系统编号.</param>
        /// <param name="noteType">配送明细单据类型.</param>
        /// <returns>
        /// 配送单明细实体
        /// </returns>
        /// <remarks>
        /// 2016-08-02 杨浩 创建
        /// </remarks>
        public abstract LgDeliveryItem GetDeliveryItem(int noteSysNo, LogisticsStatus.配送单据类型 noteType);

        /// <summary>
        /// 向配送单添加明细
        /// </summary>
        /// <param name="items">配送单明细集合</param>
        /// <returns>
        /// 添加的数量
        /// </returns>
        /// <remarks>
        /// 2013-06-18 何方 创建
        /// </remarks>
        public abstract int AddDeliveryItems(IList<LgDeliveryItem> items);

        /// <summary>
        /// 向配送单添加明细
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-07-06 何方 创建
        /// </remarks>
        public abstract int AddDeliveryItem(LgDeliveryItem item);

        /// <summary>
        /// 更新配送单状态
        /// </summary>
        /// <param name="deliverySysNo">配送单系统编号.</param>
        /// <param name="status">LogisticsStatus.配送单状态</param>
        /// <returns>
        /// 作废结果
        /// </returns>
        /// <remarks>
        /// 2013-06-18 何方 创建
        /// </remarks>
        public abstract bool UpdateStatus(int deliverySysNo, LogisticsStatus.配送单状态 status);

        /// <summary>
        /// 向配送单添加明细
        /// </summary>
        /// <param name="deliveryItemSysNos">配送单明细系统编号集合.</param>
        /// <returns>
        /// 删除结果
        /// </returns>s
        /// <remarks>
        /// 2013-06-18 何方 创建
        /// </remarks>
        public abstract int RemoveDeliveryItems(int[] deliveryItemSysNos);

        /// <summary>
        /// </summary>
        /// <param name="deliveryItemSysNo">配送单明细系统编号..</param>
        /// <param name="status">配送单明细状态.</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013/6/18 何方 创建
        /// </remarks>
        public abstract bool UpdateDeliveryItemStatus(int deliveryItemSysNo, LogisticsStatus.配送单明细状态 status);

        /// <summary>
        /// 更新单个配送单明细状态
        /// </summary>
        /// <param name="deliverySysNo">配送单系统编号.</param>
        /// <param name="noteType">配送单明细单据类型.</param>
        /// <param name="noteSysNo">配送单明细单据系统编号.</param>
        /// <param name="status">配送单明细状态.</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-07-11 何方 创建
        /// </remarks>
        public abstract bool UpdateDeliveryItemStatus(int deliverySysNo, LogisticsStatus.配送单据类型 noteType, int noteSysNo,
                                                      LogisticsStatus.配送单明细状态 status, int operatorSysNo = 0);

        /// <summary>
        /// 更新单个配送单明细
        /// </summary>
        /// <param name="lgDeliveryItem">配送单明细实体</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-06-20 沈强 创建
        /// </remarks>
        public abstract bool UpdateDeliveryItem(LgDeliveryItem lgDeliveryItem);

        /// <summary>
        /// 根据指定仓库系统编号和配送员系统编号获取配送单系统编号
        /// </summary>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <param name="userSysNo">配送员系统编号</param>
        /// <param name="status">配送单状态</param>
        /// <returns>没有返回0；有则返回当前配送单系统编号</returns>
        /// <remarks>
        /// 2013-06-25 沈强 创建
        /// </remarks>
        public abstract int GetDeliveryNoteSysNo(int warehouseSysNo, int userSysNo, Hyt.Model.WorkflowStatus.LogisticsStatus.配送单状态 status);

        /// <summary>
        /// 指定类型的单据是否在其他有效配送单明细中
        /// </summary>
        /// <param name="deliverSysNo">配送单系统编号</param>
        /// <param name="noteType">单据类型</param>
        /// <param name="noteNumber">单据编号</param>
        /// <returns>在有效配送单明细中：true；否则：false</returns>
        /// <remarks>
        /// 2013-06-25 沈强 创建
        /// </remarks>
        public abstract bool IsNoteInDeliveryItem(int deliverSysNo, Hyt.Model.WorkflowStatus.LogisticsStatus.配送单据类型 noteType, int noteNumber);

        /// <summary>
        /// 指定类型的单据是否在其他有效配送单明细中
        /// </summary>
        /// <param name="noteType">单据类型</param>
        /// <param name="noteNumber">单据编号</param>
        /// <returns>在有效配送单明细中：true；否则：false</returns>
        /// <remarks>
        /// 2013-06-25 沈强 创建
        /// </remarks>
        public abstract bool IsNoteInDeliveryItem(Hyt.Model.WorkflowStatus.LogisticsStatus.配送单据类型 noteType, int noteNumber);

        /// <summary>
        /// 更新指定配送单状态与预付、到付金额
        /// </summary>
        /// <param name="deliverSysNo">配送单系统编号</param>
        /// <param name="deliveryStatus">配送单状态</param>
        /// <param name="isForce">是否强制发货（1：是；0：否）</param>
        /// <returns>成功：true；失败：false</returns>
        /// <remarks>
        /// 2013-06-27 沈强 创建
        /// </remarks>
        public abstract bool UpdateDeliveryStatus(int deliverSysNo, Hyt.Model.WorkflowStatus.LogisticsStatus.配送单状态 deliveryStatus, bool isForce);

        /// <summary>
        /// 获取配送单中，指定配送明细状态的数量
        /// </summary>
        /// <param name="deliverySysNo">配送单系统编号</param>
        /// <param name="deliveryItemStatus">配送单明细状态</param>
        /// <returns>返回指定状态数量</returns>
        /// <remarks>
        /// 2013-06-27 沈强 创建
        /// </remarks>
        public abstract int GetDeliveryItemStatusCount(int deliverySysNo, Hyt.Model.WorkflowStatus.LogisticsStatus.配送单明细状态 deliveryItemStatus);

        /// <summary>
        /// 获取配送单中，指定配送明细状态的配送金额
        /// </summary>
        /// <param name="deliverySysNo">配送单系统编号</param>
        /// <param name="deliveryItemStatus">配送单明细状态</param>
        /// <returns>指定状态的配送金额</returns>
        /// <remarks>
        /// 2013-06-27 沈强 创建
        /// </remarks>
        public abstract decimal GetDeliveryAmount(int deliverySysNo, Hyt.Model.WorkflowStatus.LogisticsStatus.配送单明细状态 deliveryItemStatus);

        /// <summary>
        /// 根据指定系统编号删除配送单明细
        /// </summary>
        /// <param name="deliveryItemSysNo">配送单明细系统编号</param>
        /// <returns>成功:true; 失败:false</returns>
        /// <remarks>
        /// 2013-06-27 沈强 创建
        /// </remarks> 
        public abstract bool DeleteDeliveryItem(int deliveryItemSysNo);

        /// <summary>
        /// 检查一组配送单中，配送员是否相同
        /// </summary>
        /// <param name="deliverySysNos">配送单系统编号数组</param>
        /// <returns>相同：true；不同：false</returns>
        /// <remarks>
        /// 2013-06-28 沈强 创建
        /// </remarks>  
        public abstract bool CheckDeliveryUserIsRepeat(int[] deliverySysNos);

        /// <summary>
        /// 根据配送单系统编号数组，获取配送单集合
        /// </summary>
        /// <param name="deliverySysNo">配送单系统编号数组</param>
        /// <returns>配送单集合</returns>
        /// <remarks>
        /// 2013-07-04 沈强 创建
        /// </remarks>   
        public abstract IList<LgDelivery> GetLgDeliveryList(int[] deliverySysNo);

        /// <summary>
        /// 根据配送单系统编号数组，获取配送单明细集合
        /// </summary>
        /// <param name="deliverySysNo">配送单系统编号数组</param>
        /// <returns>配送单明细集合</returns>
        /// <remarks>
        /// 2013-07-04 沈强 创建
        /// </remarks>   
        public abstract IList<LgDeliveryItem> GetLgDeliveryItemList(int[] deliverySysNo);

        /// <summary>
        /// 根据配送单据类型和单据编号，获取配送单为非作废状态的明细集合
        /// </summary>
        /// <param name="noteType">配送单据类型</param>
        /// <param name="noteSysNo">单据编号</param>
        /// <returns>配送单明细集合</returns>
        /// <remarks>
        /// 2013-07-30 沈强 创建
        /// </remarks>  
        public abstract IList<LgDeliveryItem> GetLgDeliveryItemList(
            Hyt.Model.WorkflowStatus.LogisticsStatus.配送单据类型 noteType, int noteSysNo);

        /// <summary>
        /// 根据用户编号来获取配送中的配送单数量
        /// </summary>
        /// <param name="customerSysNo">用户编号</param>
        /// <returns>配送中的配送单数量</returns>
        /// <remarks>2013-08-27 周瑜 创建</remarks>
        public abstract int GetDeliveryingCount(int customerSysNo);

        /// <summary>
        /// 通过事务编号获取配送单列表
        /// </summary>
        /// <param name="transactionSysNo">事务编号</param>
        /// <returns>配送单</returns>
        /// <remarks>2013-09-18 周唐炬 创建</remarks>
        public abstract IList<LgDelivery> GetLgDeliveryList(string transactionSysNo);

        /// <summary>
        /// 通过来源单据类型跟编号获取配送单
        /// </summary>
        /// <param name="noteType">单据类型</param>
        /// <param name="noteNumber">单据编号</param>
        /// <returns>配送单</returns>
        /// <remarks>2013-09-18 周唐炬 创建</remarks>
        public abstract LgDelivery GetDelivery(LogisticsStatus.配送单据类型 noteType, int noteNumber);

        /// <summary>
        /// 通过订单号读取配送单列表
        /// </summary>
        /// <param name="orderSysNo">订单号</param>
        /// <returns>配送单列表</returns>
        /// <remarks>
        /// 2013-12-06 余勇 创建
        /// </remarks>
        public abstract List<CBLgDelivery> GetDeliveryListByOrderSysNo(int orderSysNo);

        /// <summary>
        /// 判断快递单号是否已经被使用
        /// </summary>
        /// <param name="deliverySysNo">快递方式</param>
        /// <param name="express_no">快递单号</param>
        /// <returns>是否存在</returns>
        /// <remarks>2014-04-14 朱成果 创建</remarks>
        public abstract bool IsExistsExpressNo(int deliverySysNo, string express_no);

        /// <summary>
        /// 创建第三方快递配送单
        /// </summary>
        /// <param name="model">rp_第三方快递发货量</param>
        /// <returns>返回系统编号</returns>
        /// <remarks>
        /// 2014-09-23 余勇 创建
        /// </remarks>
        public abstract void CreateExpressLgDelivery(rp_第三方快递发货量 model);


      
       
    }
}

