using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using Hyt.DataAccess.Basic;
using Hyt.Infrastructure.Memory;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.BLL.Basic
{
    /// <summary>
    ///  支付方式业务BO
    /// <remarks>2013-06-25 朱成果 创建</remarks>
    /// </summary>
    public class PaymentTypeBo : BOBase<PaymentTypeBo>
    {
        /// <summary>
        /// 添加新支付方式
        /// </summary>
        /// <param name="model">支付方式实体</param>
        /// <returns>受影响行</returns>
        /// <remarks>2013-09-06 周唐炬 创建</remarks>
        public int PaymentTypeCreate(BsPaymentType model)
        {
            return IBasicPayDao.Instance.PaymentTypeCreate(model);
        }

        /// <summary>
        /// 修改支付方式
        /// </summary>
        /// <param name="model">支付方式实体</param>
        /// <returns>受影响行</returns>
        /// <remarks>
        /// 2013-09-06 周唐炬 创建
        /// 2014-05-13 朱家宏 增加缓存移出
        /// </remarks>
        public int PaymentTypeUpdate(BsPaymentType model)
        {
            var entity = IBasicPayDao.Instance.GetPaymentType(model.SysNo);
            if (entity != null)
            {
                model.CreatedBy = entity.CreatedBy;
                model.CreatedDate = entity.CreatedDate;
            }
            else
            {
                throw new HytException("该支付方式不存在,请检查!");
            }
            var result = IBasicPayDao.Instance.PaymentTypeUpdate(model);
            if (result > 0)
            {
                MemoryProvider.Default.Remove(string.Format(KeyConstant.PaymentType, model.SysNo));
            }
            return result;
        }

        /// <summary>
        /// 根据编号获取支付方式
        /// </summary>
        /// <param name="sysno">支付方式编号</param>
        /// <returns>
        /// 支付方式实体
        /// </returns>
        /// <remarks>
        /// 2013-06-25 朱成果 创建
        /// </remarks>
        public BsPaymentType GetEntity(int sysno)
        {
            return GetPaymentTypeFromMemory(sysno);
        }

        /// <summary>
        /// 获取所有支付方式
        /// </summary>
        /// <returns>
        /// 返回所有支付方式
        /// </returns>
        /// <remarks>
        /// 2013-09-06 周唐炬 创建
        /// </remarks>
        public IList<CBBsPaymentType> GetAll()
        {
            return IBasicPayDao.Instance.GetAll();
        }

        /// <summary>
        /// 获取有效的支付方式列表
        /// </summary>
        /// <returns>支付方式列表</returns>
        /// <remarks>
        /// 2013-08-20 郑荣华 创建
        /// </remarks>
        public IList<CBBsPaymentType> GetUsedPaymentTypeList()
        {
            var filter = new ParaPaymentTypeFilter { Status = (int)BasicStatus.支付方式状态.启用 };
            return IBasicPayDao.Instance.GetPaymentTypeList(filter);
        }

        /// <summary>
        /// 获取支付方式列表
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns>支付方式列表</returns>
        /// <remarks>
        /// 2013-08-20 郑荣华 创建
        /// </remarks>
        public IList<CBBsPaymentType> GetPaymentTypeList(ParaPaymentTypeFilter filter)
        {
            return IBasicPayDao.Instance.GetPaymentTypeList(filter);
        }

        /// <summary>
        /// 支付名称验证
        /// </summary>
        /// <param name="paymentName">支付名称</param>
        /// <param name="sysNo">支付方式系统编号</param>
        /// <returns>验证结果</returns>
        /// <remarks>2013-09-06 周唐炬 创建</remarks>
        public bool PaymentTypeVerify(string paymentName, int? sysNo)
        {
            var count = IBasicPayDao.Instance.PaymentTypeVerify(paymentName, sysNo);
            return count <= 0;
        }

        /// <summary>
        /// 获取支付类型缓存
        /// </summary>
        /// <param name="sysNo">支付系统编号</param>
        /// <returns>支付类型</returns>
        /// <remarks>2014-05-13 朱家宏 创建</remarks>
        public BsPaymentType GetPaymentTypeFromMemory(int sysNo)
        {
            //缓存时间12小时
            return MemoryProvider.Default.Get(string.Format(KeyConstant.PaymentType, sysNo), 60*12,
                () => IBasicPayDao.Instance.GetPaymentType(sysNo));
        }
    }
}
