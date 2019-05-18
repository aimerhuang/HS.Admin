using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.BLL.Log;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.DataAccess.Logistics;
using Hyt.Model.WorkflowStatus;

namespace Hyt.BLL.Logistics
{

    /// <summary>
    /// 配送员信用额度业务类
    /// </summary>
    /// <remarks>
    /// 2013-06-20 郑荣华 创建
    /// </remarks>
    public class DeliveryUserCreditBo : BOBase<DeliveryUserCreditBo>
    {
        #region 操作
        /// <summary>
        /// 添加配送员(业务员)信用配额
        /// </summary>
        /// <param name="model">配送员(业务员)信用配额</param>
        /// <returns>添加是否成功SysNo</returns>
        /// <remarks>2013-06-19 郑荣华 创建</remarks>
        public bool Create(LgDeliveryUserCredit model)
        {
            //Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "创建配送员信用额度", LogStatus.系统日志目标类型.配送员信用额度, model.DeliveryUserSysNo);
            return ILgDeliveryUserCreditDao.Instance.Create(model);
        }

        /// <summary>
        /// 更新配送员(业务员)信用配额
        /// </summary>
        /// <param name="model">配送员(业务员)信用配额</param>
        /// <returns>更新是否成功</returns>
        /// <remarks>2013-06-09 沈强 创建</remarks>
        public bool Update(LgDeliveryUserCredit model)
        {
            //Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "修改配送员信用额度", LogStatus.系统日志目标类型.配送员信用额度, model.DeliveryUserSysNo);
            return ILgDeliveryUserCreditDao.Instance.Update(model); ;
        }

        /// <summary>
        /// 增加或减少配送员可用信用额度
        /// </summary>
        /// <param name="warehouseSysNo">The warehouse sys no.</param>
        /// <param name="deliveryUserSysNo">The delivery user sys no.</param>
        /// <param name="remainingDeliveryCredit">增加或减少的可用配送信用额度,减少使用负数</param>
        /// <param name="remainingBorrowingCredit">增加或减少的可用借货信用额度,减少使用负数</param>
        /// <param name="reason">操作原因</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-12-16 何方 创建
        /// </remarks>
        public LgDeliveryUserCredit UpdateRemaining(int warehouseSysNo, int deliveryUserSysNo, decimal remainingDeliveryCredit, decimal remainingBorrowingCredit, string reason)
        {
            var model = ILgDeliveryUserCreditDao.Instance.GetLgDeliveryUserCredit(deliveryUserSysNo, warehouseSysNo);
            if (model == null)
            {
                throw new HytException("仓库:{0}配送员:{1}尚未配置信用额度", warehouseSysNo, deliveryUserSysNo);
            }

            var current = Authentication.AdminAuthenticationBo.Instance.Current;
            model.LastUpdateBy = current != null ? current.Base.SysNo : deliveryUserSysNo;

            model.LastUpdateDate = DateTime.Now;

            if (remainingDeliveryCredit != 0)
            {
                model.RemainingDeliveryCredit += remainingDeliveryCredit;
                var logMsg = string.Format("修改配送员可用配送额度:{0},{1}", remainingDeliveryCredit, reason);
                SysLog.Instance.Info(LogStatus.系统日志来源.后台, logMsg, LogStatus.系统日志目标类型.配送员信用额度, model.DeliveryUserSysNo, model.LastUpdateBy);
            }
            if (remainingBorrowingCredit != 0)
            {
                model.RemainingBorrowingCredit += remainingBorrowingCredit;
                var logMsg = string.Format("修改配送员可用借货额度:{0},{1}", remainingBorrowingCredit, reason);
                SysLog.Instance.Info(LogStatus.系统日志来源.后台, logMsg, LogStatus.系统日志目标类型.配送员信用额度, model.DeliveryUserSysNo, model.LastUpdateBy);
            }

            ILgDeliveryUserCreditDao.Instance.Update(model);
            return model;
        }

        /// <summary>
        /// 删除配送员信用信息
        /// </summary>
        /// <param name="deliveryUserSysNo">配送员系统编号</param>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks>2013-06-21 郑荣华 创建</remarks>
        public bool Delete(int deliveryUserSysNo, int warehouseSysNo)
        {
            var r = ILgDeliveryUserCreditDao.Instance.Delete(deliveryUserSysNo, warehouseSysNo);
            if (r)
                Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "删除配送员信用额度（配送+仓库）", LogStatus.系统日志目标类型.配送员信用额度, deliveryUserSysNo);
            return r;
        }
        #endregion

        #region 查询
        /// <summary>
        /// 获取配送员信用配额列表
        /// </summary>
        /// <param name="pager">配送员信用配额列表分页对象</param>
        /// <remarks>2013-06-20 郑荣华 创建</remarks>
        public void GetLgDeliveryUserCreditList(ref Pager<CBLgDeliveryUserCredit> pager)
        {
            ILgDeliveryUserCreditDao.Instance.GetLgDeliveryUserCreditList(ref pager);
        }

        /// <summary>
        /// 查询配送员(业务员)信用配额集合
        /// </summary>
        /// <param name="pager">配送员信用配额列表分页对象</param>
        /// <param name="filter">配送员信用查询条件</param>
        /// <remarks>2013-06-21 郑荣华 创建</remarks>
        public void GetLgDeliveryUserCreditList(ref Pager<CBLgDeliveryUserCredit> pager, ParaDeliveryUserCreditFilter filter)
        {
            ILgDeliveryUserCreditDao.Instance.GetLgDeliveryUserCreditList(ref pager, filter);
        }

        /// <summary>
        /// 根据配送员系统编号获取信用配额信息
        /// </summary>
        /// <param name="deliveryUserSysNo">配送员系统编号</param>
        /// <returns>配送员信用额度组合实体列表</returns>
        /// <remarks>
        /// 2013-06-09 沈强 创建
        /// 2013-07-19 郑荣华 修改
        /// </remarks>
        public IList<CBLgDeliveryUserCredit> GetLgDeliveryUserCredit(int deliveryUserSysNo)
        {
            return ILgDeliveryUserCreditDao.Instance.GetLgDeliveryUserCredit(deliveryUserSysNo);
        }

        /// <summary>
        /// 根据配送员和仓库系统编号获取信用配额信息
        /// </summary>
        /// <param name="deliveryUserSysNo">配送员系统编号</param>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <returns>配送员信用额度组合实体</returns>
        /// <remarks>2013-07-17 郑荣华 创建</remarks>
        public CBLgDeliveryUserCredit GetLgDeliveryUserCredit(int deliveryUserSysNo, int warehouseSysNo)
        {
            return ILgDeliveryUserCreditDao.Instance.GetLgDeliveryUserCredit(deliveryUserSysNo, warehouseSysNo);
        }
        #endregion

        #region 判断
        /// <summary>
        /// 判断是否已存在配送员信用信息
        /// </summary>
        /// <param name="deliveryUserSysNo">配送员系统编号</param>
        /// <returns>存在返回true,不存在返回false</returns>
        /// <remarks>2013-06-20 郑荣华 创建</remarks>
        public bool IsExistDeliveryUser(int deliveryUserSysNo)
        {
            return ILgDeliveryUserCreditDao.Instance.GetLgDeliveryUserCredit(deliveryUserSysNo) != null;
        }
        #endregion
    }
}
