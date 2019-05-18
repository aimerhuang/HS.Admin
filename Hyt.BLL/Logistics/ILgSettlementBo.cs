using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Hyt.Model;
using Hyt.DataAccess.Logistics;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.BLL.Logistics
{

    /// <summary>
    /// 结算单业务逻辑
    /// </summary>
    /// <remarks>2013-06-25 何方 创建</remarks>
    public interface ILgSettlementBo
    {
        ///  <summary>
        ///  查找/高级查找
        ///  </summary>
        /// <param name="currPageIndex">当前pageIndex</param>
        /// <param name="searchParas">高级查询参数集合</param>
        /// <param name="warehouses">当前登录人员有权限的仓库</param>
        /// <returns>结算单主表实体</returns>
        /// <remarks>2013-06-15 黄伟 创建</remarks>
        Pager<CBLgSettlement> Search(int? currPageIndex, ParaLogisticsLgsettlement searchParas, List<int> warehouses);

        /// <summary>
        /// 下拉选项-取得仓库列表
        /// </summary>
        /// <param></param>
        /// <returns>仓库列表</returns>
        /// <remarks>2013-06-18 黄伟 创建</remarks>
        Dictionary<int, string> GetWareHouse();

        /// <summary>
        /// 下拉选项-取得配送人员列表
        /// </summary>
        /// <param name="whSysNo">仓库编号</param>
        /// <returns>配送人员列表</returns>
        /// <remarks>2013-06-18 黄伟 创建</remarks>
        Dictionary<int, string> GetDeliveryMan(int? whSysNo);

        /// <summary>
        /// 更新结算单状态
        /// </summary>
        /// <param name="lstSettlementSysNo">结算单系统编号集合逗号分隔</param>
        /// <param name="settlementStatus">审核或作废,枚举结算单状态</param>
        /// <param name="opreatorSysNo">操作人员系统编号</param>
        /// <returns>封装的实体(Status,StatusCode,Message)</returns>
        /// <remarks>2013-06-28 黄伟 创建</remarks>
        Result UpdateStatus(IList<int> lstSettlementSysNo, LogisticsStatus.结算单状态 settlementStatus, int opreatorSysNo);

        /// <summary>
        /// 审核结算单
        /// </summary>
        /// <param name="cbCreateSettlement">生成结算单model</param>
        /// <param name="userIp">访问者ip</param>
        /// <returns>封装的响应实体</returns>
        /// <remarks>
        /// 2013/7/2 何方 创建
        /// 2013-07-15 黄伟 修改
        /// </remarks>
        Result<int> CreateSettlement(CBCreateSettlement cbCreateSettlement,string userIp,bool withTrans=true);

        /// <summary>
        /// 获取结算单明细
        /// </summary>
        /// <param name="settlementSysNo">结算单主表系统编号</param>
        /// <returns>结算单明细列表</returns>
        /// <remarks>2013-06-28 黄伟 创建</remarks>
        LgSettlement GetLgSettlementWithItems(int settlementSysNo);

    }
}
