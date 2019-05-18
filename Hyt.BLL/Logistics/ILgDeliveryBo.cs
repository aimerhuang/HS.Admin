using System.Collections.Generic;
using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.BLL.Logistics
{
    /// <summary>
    /// </summary>
    /// <remarks>
    /// 2013/7/14 何方 创建
    /// </remarks>
    public interface ILgDeliveryBo
    {
        #region 配送单

        /// <summary>
        /// 创建配送单
        /// </summary>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <param name="deliverUserSysNo">配送员系统编号.</param>
        /// <param name="delivertType">配送方式.</param>
        /// <param name="operateUserSysNo">操作人系统编号.</param>
        /// <param name="items">配送单明细 列表</param>
        /// <param name="isForce">是否配送额度不足时强制放行</param>
        /// <param name="userIp">访问者ip</param>
        /// <returns>
        /// 创建配送单系统编号
        /// </returns>
        /// <remarks>
        /// 2013-06-14 何方 创建
        /// </remarks>
        int CreateLgDelivery(int warehouseSysNo, int deliverUserSysNo, CBLgDeliveryType delivertType, int operateUserSysNo, List<LgDeliveryItem> items, bool isForce, string userIp);

        /// <summary>
        /// 读取配送单
        /// </summary>
        /// <param name="sysNo">配送单sys no.</param>
        /// <returns>配送单 实体</returns>
        /// <remarks>
        /// 2013/6/17 何方 创建
        /// </remarks>
        LgDelivery GetDelivery(int sysNo);

        /// <summary>
        /// 读取多个配送单
        /// </summary>
        /// <param name="sysNos">配送单系统编号集合.</param>
        /// <returns>配送单集合</returns>
        /// <remarks>
        /// 2013/6/17 何方 创建
        /// </remarks>
       IList<LgDelivery> GetDeliveryList(int[] sysNos);

        /// <summary>
        /// 获取配送单集合
        /// </summary>
        /// <param name="pager">分页对象</param>
        /// <returns></returns>
       /// <remarks>2013-06-19 何方 创建</remarks>
        void GetLgDelivery(ref Pager<CBLgDelivery> pager,SysAuthorization currentUser=null);

        /// <summary>
        /// 作废配送单
        /// </summary>
        /// <param name="deliverySysNo">配送单系统编号.</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-06-19 何方 创建
        /// </remarks>
        Result CancelDelivery(int deliverySysNo, int operatorSysNo);

        #endregion
        #region 配送单明细
        /// <summary>
        /// 读取配送单明细
        /// </summary>
        /// <param name="deliverySysNo">配送单sys no.</param>
        /// <returns>配送单列表列表</returns>
        /// <remarks>
        /// 2013/6/17 何方 创建
        /// </remarks>
        IList<LgDeliveryItem> GetDeliveryItemList(int deliverySysNo);

        /// <summary>
        /// 读取配送单明细
        /// </summary>
        /// <param name="deliverySysNos">配送单系统编号集合.</param>
        /// <returns>配送单明细列表</returns>
        /// <remarks>
        /// 2013/6/17 何方 创建
        /// </remarks>
        IList<LgDeliveryItem> GetDeliveryItemList(int[] deliverySysNos);

        /// <summary>
        /// 读取配送单明细
        /// </summary>
        /// <param name="deliverySysNo">配送单sys no.</param>
        /// <returns>配送单明细组合实体列表</returns>
        /// <remarks>
        /// 2013-06-19 沈强 创建
        /// </remarks>
        IList<CBLgDeliveryItem> GetCbDeliveryItems(int deliverySysNo);

        /// <summary>
        /// 更新单个配送单明细
        /// </summary>
        /// <param name="lgDeliveryItem">配送单明细实体</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-06-20 沈强 创建
        /// </remarks>
        bool UpdateDeliveryItem(LgDeliveryItem lgDeliveryItem);

        /// <summary>
        /// 更新指定系统编号的配送单明细信息（仅更新支付类型、支付单号、配送单明细状态）
        /// </summary>
        /// <param name="sysNo">配送单明细系统编号</param>
        /// <param name="paymentType">支付类型</param>
        /// <param name="payNo">支付单号</param>
        /// <param name="status">配送单明细状态</param>
        /// <returns>更新结果</returns>
        /// <remarks>2013-06-26 沈强 创建</remarks>
        Result UpdateDeliveryItem(int sysNo, int paymentType, string payNo, int status);

        /// <summary>
        /// 读取单个配送单明细
        /// </summary>
        /// <param name="deliveryItemSysNo">配送单明细系统编号.</param>
        /// <returns>配送单明细实体</returns>
        /// <remarks>
        /// 2013-06-20 沈强 创建
        /// </remarks>
        LgDeliveryItem GetDeliveryItem(int deliveryItemSysNo);

        #endregion

        /// <summary>
        /// 获取当天指定的配送单列表
        /// </summary>
        /// <param name="sysNosExcluded">要排除的配送单系统编号集合</param>
        /// <param name="userSysno">用户编号</param>
        /// <returns>当天指定的配送单列表</returns>
        /// <remarks>2013-06-21 黄伟 创建</remarks>
        Result<List<CBWCFLgDelivery>> GetDeliveryList(List<int> sysNosExcluded,int userSysno);

        /// <summary>
        /// 根据指定仓库系统编号和配送员系统编号获取配送单系统编号
        /// </summary>
        /// <param name="warehouseSysno">仓库系统编号</param>
        /// <param name="userSysno">配送员系统编号</param>
        /// <param name="status">配送单状态</param>
        /// <returns>没有返回0；有则返回当前配送单系统编号</returns>
        /// <remarks>
        /// 2013-06-25 沈强 创建
        /// </remarks>
        int GetDeliveryNoteSysNo(int warehouseSysno, int userSysno, Hyt.Model.WorkflowStatus.LogisticsStatus.配送单状态 status);

        /// <summary>
        /// 指定类型的单据是否在其他有效配送单明细中
        /// </summary>
        /// <param name="deliverSysno">配送单系统编号</param>
        /// <param name="noteType">单据类型</param>
        /// <param name="noteNumber">单据编号</param>
        /// <returns>在有效配送单明细中：true；否则：false</returns>
        /// <remarks>
        /// 2013-06-25 沈强 创建
        /// </remarks>
        bool IsNoteInDeliveryItem(int deliverSysno, LogisticsStatus.配送单据类型 noteType, int noteNumber);
 
        /// <summary>
        /// 根据指定系统编号删除配送单明细
        /// </summary>
        /// <param name="deliveryItemSysNo">The delivery item sys no.</param>
        /// <returns>
        /// 删除结果
        /// </returns>
        /// <remarks>
        /// 2013-06-27 沈强 创建
        /// </remarks>
        Result DeleteDeliveryItem(int deliveryItemSysNo);

        /// <summary>
        /// 更新配送单状态
        /// </summary>
        /// <param name="deliverySysNo">配送单系统编号.</param>
        /// <param name="status">LogisticsStatus.配送单状态</param>
        /// <returns>
        /// 作废结果
        /// </returns>
        /// <remarks>
        /// 2013-07-14 何方 创建
        /// </remarks>
        bool UpdateStatus(int deliverySysNo, LogisticsStatus.配送单状态 status);

        /// <summary>
        /// 通过事务编号获取配送单列表
        /// </summary>
        /// <param name="transactionSysNo">事务编号</param>
        /// <returns>配送单</returns>
        /// <remarks>2013-09-18 周唐炬 创建</remarks>
        IList<LgDelivery> GetLgDeliveryList(string transactionSysNo);

        /// <summary>
        /// 通过来源单据类型跟编号获取配送单
        /// </summary>
        /// <param name="noteType">单据类型</param>
        /// <param name="noteNumber">单据编号</param>
        /// <remarks>2013-09-18 周唐炬 创建</remarks>
        /// <returns>配送单列表</returns>
        LgDelivery GetDelivery(LogisticsStatus.配送单据类型 noteType, int noteNumber);

        /// <summary>
        /// 判断快递单号是否已经被使用
        /// </summary>
        /// <param name="deliverySysNo">快递方式</param>
        /// <param name="express_no">快递单号</param>
        /// <returns>是否存在</returns>
        /// <remarks>2014-04-14 朱成果 创建</remarks>
        bool IsExistsExpressNo(int deliverySysNo, string express_no);

        /// <summary>
        /// 检查是否存在未入库的入库单（入库单来源为当前入库单)
        /// </summary>
        /// <param name="whStockOutNO">出库单编号</param>
        /// <returns></returns>
       /// <remarks>2014-04-14 朱成果 创建</remarks>
        Result CheckInStock(int whStockOutNO);

    }
}