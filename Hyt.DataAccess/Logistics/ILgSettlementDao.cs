using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Logistics
{
    /// <summary>
    /// Dao结算单维护
    /// </summary>
    /// <remarks>2013-06-15 黄伟 创建</remarks>
    public abstract class ILgSettlementDao : DaoBase<ILgSettlementDao>
    {
        /// <summary>
        /// 查找/高级查找
        /// </summary>
        /// <returns>结算单主表实体</returns>
        /// <param name="currPageIndex">当前pageIndex</param>
        /// <param name="warehouses">当前登录人员有权限的仓库</param>
        /// <param name="searchParas">高级查询参数集合</param>
        /// <remarks>2013-06-15 黄伟 创建</remarks>
        public abstract Pager<CBLgSettlement> Search(int? currPageIndex, ParaLogisticsLgsettlement searchParas, List<int> warehouses);

        /// <summary>
        /// 获取结算单列表
        /// </summary>
        /// <param name="sysNo">结算单系统编号</param>
        /// <returns>结算单列表,参数为空返回所有</returns>
        /// <remarks>2013-06-15 黄伟 创建</remarks>
        public abstract List<LgSettlement> GetLgSettlement(int? sysNo);

        /// <summary>
        /// 下拉选项-取得仓库列表
        /// </summary>
        /// <returns>仓库列表</returns>
        /// <remarks>2013-06-18 黄伟 创建</remarks>
        public abstract Dictionary<int, string> GetWareHouse();

        /// <summary>
        /// 下拉选项-取得配送人员列表
        /// </summary>
        /// <param name="whSysNo">仓库编号</param>
        /// <returns>配送人员列表</returns>
        /// <remarks>2013-06-18 黄伟 创建</remarks>
        public abstract Dictionary<int, string> GetDeliveryMan(int? whSysNo);

        /// <summary>
        /// 更新结算单状态
        /// </summary>
        /// <param name="lstSettlementSysNo">结算单系统编号集合</param>
        /// <param name="status">审核或作废,枚举结算单状态</param>
        /// <param name="userSysNo">操作人系统编号</param>
        /// <returns>封装的实体(Status,StatusCode,Message)</returns>
        /// <remarks>
        /// 2013/6/28 黄伟 创建
        /// </remarks>
        public abstract Result UpdateStatus(IList<int> lstSettlementSysNo, LogisticsStatus.结算单状态 status, int userSysNo);

        /// <summary>
        /// 新增结算单
        /// </summary>
        /// <param name="lgSettlement">结算单主表实体.</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013/7/12 何方 创建
        /// </remarks>
        public abstract int Create(LgSettlement lgSettlement);

        /// <summary>
        /// 获取支付方式列表
        /// </summary>
        /// <param name="sysNo">支付方式系统编号,null表示返回所有</param>
        /// <returns>支付方式列表</returns>
        /// <remarks>2013-8-8 黄伟 创建</remarks>
        public abstract IList<BsPaymentType> GetBsPaymentTypes(int? sysNo);

        /// <summary>
        /// 获取结算单关联收款单是否存在至少一笔已确认,若确认,不允许作废结算单
        /// 需在订单相关页面操作,取消确认
        /// </summary>
        /// <param name="lstSettlementSysNo">结算单编号集合</param>
        /// <returns>true:已确认;false:待确认</returns>
        /// <remarks>2013-8-8 黄伟 创建</remarks>
        public abstract Result<List<string>> IsFnReciptVoucherConfirmed(List<int> lstSettlementSysNo);

        /// <summary>
        /// 通过订单号查询结算单列表
        /// </summary>
        /// <param name="orderSysNo">订单编号</param>
        /// <returns>结算单列表</returns>
        /// <remarks>2013-12-06 余勇 创建</remarks>
        public abstract List<CBLgSettlement> GetLgSettlementListByOrderSysNo(int orderSysNo);

        /// <summary>
        /// get the appsign info by delivery SysNos  
        /// </summary>
        /// <param name="deliverySysNos">list of delivery sysnos</param>
        /// <returns>list of LgAppSignStatus</returns>
        /// <remarks>2014-1-14 黄伟 创建</remarks>
        public abstract List<LgAppSignStatus> GetAppSignStatus(List<int> deliverySysNos);

        /// <summary>
        /// get the appsign info by delivery SysNos  
        /// </summary>
        /// <param name="deliverySysNo">配送员系统编号</param>
        /// <param name="noteSysNo">出库单</param>
        /// <param name="noteType">单据类型</param>
        /// <returns>list of LgAppSignStatus</returns>
        /// <remarks>2014-1-14 黄伟 创建</remarks>
        /// <remarks>2014-03-24 周唐炬 修改</remarks>
        public abstract LgAppSignStatus GetAppSignStatusforDeliveryItem(int deliverySysNo, int noteSysNo,
                                                                              int noteType);

        /// <summary>
        /// get the partial sign info by AppSignStatus sysnos  
        /// </summary>
        /// <param name="appSignSysNos">list of AppSignStatus sysnos</param>
        /// <returns>list of LgAppSignItem</returns>
        /// <remarks>2014-1-14 黄伟 创建</remarks>
        public abstract List<LgAppSignItem> GetAppSignItem(List<int> appSignSysNos);

        /// <summary>
        /// get the partial sign info by AppSignStatus sysnos  
        /// </summary>
        /// <param name="appSignSysNo">AppSignStatus sysno</param>
        /// <returns>list of LgAppSignItem</returns>
        /// <remarks>2014-1-14 黄伟 创建</remarks>
        public abstract List<LgAppSignItem> GetAppSignItem(int appSignSysNo);

         
    }
}
