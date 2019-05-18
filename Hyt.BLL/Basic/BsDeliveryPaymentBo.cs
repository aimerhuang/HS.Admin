using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Basic;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.BLL.Basic
{
    /// <summary>
    /// 配送方式支付方式关联业务类
    /// </summary>
    /// <remarks>
    /// 2013-08-01 郑荣华 创建
    /// </remarks>
    public class BsDeliveryPaymentBo : BOBase<BsDeliveryPaymentBo>
    {
        #region 操作

        /// <summary>
        /// 创建配送方式支付方式关联信息
        /// </summary>
        /// <param name="model">配送方式支付方式关联信息实体</param>
        /// <returns>创建的配送方式支付方式关联信息sysNo</returns>
        /// <remarks> 
        /// 2013-08-02 郑荣华 创建 
        /// </remarks>
        public int Create(BsDeliveryPayment model)
        {
            var r = IBsDeliveryPaymentDao.Instance.Create(model);
            Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "创建配送方式支付方式关联", LogStatus.系统日志目标类型.配送方式支付方式关联, r);
            return r;
        }

        /// <summary>
        /// 删除配送方式支付方式关联信息
        /// </summary>
        /// <param name="sysNo">要删除的配送方式支付方式关联信息系统编号</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks> 
        /// 2013-08-02 郑荣华 创建
        /// </remarks>
        public bool Delete(int sysNo)
        {
            var r = IBsDeliveryPaymentDao.Instance.Delete(sysNo) > 0;
            if (r)
                Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "删除配送方式支付方式关联", LogStatus.系统日志目标类型.配送方式支付方式关联, sysNo);
            return r;
        }

        /// <summary>
        /// 批量删除配送方式支付方式关联信息
        /// </summary>
        /// <param name="sysNo">用,分隔的系统编号</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks> 
        /// 2013-08-02 郑荣华 创建
        /// </remarks>
        public bool DeleteBatch(string sysNo)
        {
            var batch = Array.ConvertAll(sysNo.Split(','), int.Parse).ToList();
            foreach (var item in batch)
            {
                IBsDeliveryPaymentDao.Instance.Delete(item);
                Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "删除配送方式支付方式关联", LogStatus.系统日志目标类型.配送方式支付方式关联, item);
            }

            return true;
        }

        /// <summary>
        /// 删除配送方式支付方式关联信息
        /// </summary>
        /// <param name="paymentSysNo">支付方式系统编号</param>
        /// <param name="deliverySysNo">配送方式系统编号</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks> 
        /// 2013-08-02 郑荣华 创建
        /// </remarks>
        public bool Delete(int paymentSysNo, int deliverySysNo)
        {
            var r = IBsDeliveryPaymentDao.Instance.Delete(paymentSysNo, deliverySysNo) > 0;
            if (r)
                Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "删除配送方式支付方式关联", LogStatus.系统日志目标类型.配送方式支付方式关联, deliverySysNo);
            return r;
        }
        #endregion

        #region 查询

        /// <summary>
        /// 查询配送方式支付方式关联信息
        /// </summary>
        /// <param name="pager">配送方式支付方式关联列表分页对象</param>
        /// <param name="filter">配送方式支付方式关联查询条件</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-08-02 郑荣华 创建
        /// </remarks>
        public void GetListByPayment(ref Pager<CBBsDeliveryPayment> pager, ParaBsDeliveryPaymentFilter filter)
        {
            IBsDeliveryPaymentDao.Instance.GetBsDeliveryPaymentList(ref pager, filter);
        }

        /// <summary>
        /// 根据支付方式查询配送方式支付方式关联列表信息
        /// </summary>
        /// <param name="paymentSysNo">支付方式系统编号</param>
        /// <returns>配送方式支付方式关联列表</returns>
        /// <remarks>
        /// 2013-08-02 郑荣华 创建
        /// </remarks>
        public IList<CBBsDeliveryPayment> GetListByPayment(int paymentSysNo)
        {
            return IBsDeliveryPaymentDao.Instance.GetListByPayment(paymentSysNo);
        }

        /// <summary>
        /// 根据配送方式查询配送方式支付方式关联列表信息
        /// </summary>
        /// <param name="deliverySysNo">配送方式系统编号</param>
        /// <returns>配送方式支付方式关联列表</returns>
        /// <remarks>
        /// 2013-08-02 郑荣华 创建
        /// </remarks>
        public IList<CBBsDeliveryPayment> GetListByDelivery(int deliverySysNo)
        {
            return IBsDeliveryPaymentDao.Instance.GetListByDelivery(deliverySysNo);
        }

        /// <summary>
        /// 配送和支付方式匹配条数
        /// </summary>
        /// <param name="paymentSysNo">支付方式编号</param>
        /// <param name="deliverySysNo">配送方式系统编号</param>
        /// <returns>配送和支付方式匹配条数</returns>
        /// <remarks>2013-09-12 黄志勇 创建</remarks>
        public int GetBsDeliveryPaymentCount(int paymentSysNo, int deliverySysNo)
        {
            return IBsDeliveryPaymentDao.Instance.GetBsDeliveryPaymentCount(paymentSysNo, deliverySysNo);
        }
        #endregion

    }
}
