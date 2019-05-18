using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Print;
using Hyt.Model;
using Hyt.Model.Transfer;

namespace Hyt.BLL.Print
{
    /// <summary>
    /// 配送方式打印模板设置业务类
    /// </summary>
    /// <remarks>
    /// 2013-07-16 郑荣华 创建
    /// </remarks>
    public class DeliveryPrintTemplateBo : BOBase<DeliveryPrintTemplateBo>
    {
        #region 操作

        /// <summary>
        /// 添加打印模板信息
        /// </summary>
        /// <param name="model">打印模板实体类</param>
        /// <returns>true:添加成功;false:添加失败</returns>
        /// <remarks>
        /// 2013-07-12 郑荣华 创建
        /// </remarks>
        public int CreateDeliveryPrintTemplate(LgDeliveryPrintTemplate model)
        {
            return IDeliveryPrintTemplateDao.Instance.CreateDeliveryPrintTemplate(model);
        }

        /// <summary>
        /// 修改打印模板信息
        /// </summary>
        /// <param name="model">打印模板实体类</param>
        /// <returns>true:修改成功;false:修改失败</returns>
        /// <remarks>
        /// 2013-07-12 郑荣华 创建
        /// </remarks>
        public bool UpdateDeliveryPrintTemplate(LgDeliveryPrintTemplate model)
        {
            return IDeliveryPrintTemplateDao.Instance.UpdateDeliveryPrintTemplate(model) > 0; ;
        }

        /// <summary>
        /// 删除打印模板信息
        /// </summary>
        /// <param name="templateSysNo">打印模板系统编号</param>
        /// <returns>true:删除成功;false:删除失败</returns>
        /// <remarks>
        /// 2013-07-12 郑荣华 创建
        /// </remarks>
        public bool DeleteDeliveryPrintTemplate(int templateSysNo)
        {
            return IDeliveryPrintTemplateDao.Instance.DeleteDeliveryPrintTemplate(templateSysNo) > 0; ;
        }

        #endregion

        #region 查询

        /// <summary>
        /// 根据配送方式系统编号获取打印模板信息
        /// </summary>
        /// <param name="deliveryTypeSysNo">配送方式系统编号</param>
        /// <returns>打印模板列表信息</returns>
        /// <remarks>
        /// 2013-07-12 郑荣华 创建
        /// </remarks>
        public IList<CBLgDeliveryPrintTemplate> GetDeliveryPrintTemplateList(int deliveryTypeSysNo)
        {
            return IDeliveryPrintTemplateDao.Instance.GetDeliveryPrintTemplateList(deliveryTypeSysNo);
        }

        /// <summary>
        /// 根据系统编号获取打印模板信息
        /// </summary>
        /// <param name="templateSysNo">打印模板系统编号</param>
        /// <returns>打印模板信息</returns>
        /// <remarks>
        /// 2013-07-12 郑荣华 创建
        /// </remarks>
        public CBLgDeliveryPrintTemplate GetDeliveryPrintTemplate(int templateSysNo)
        {
            return IDeliveryPrintTemplateDao.Instance.GetDeliveryPrintTemplate(templateSysNo);
        }

        /// <summary>
        /// 根据模板名称获取打印模板信息
        /// </summary>
        /// <param name="templateName">打印模板名称</param>
        /// <param name="sysNo">要排除的系统编号</param>
        /// <returns>打印模板列表信息</returns>
        /// <remarks>
        /// 2013-11-30 郑荣华 创建 用于检查模板名称重复,新建时设置成sysNo=0
        /// </remarks>
        public IList<CBLgDeliveryPrintTemplate> GetDeliveryPrintTemplateList(string templateName, int sysNo)
        {
            return IDeliveryPrintTemplateDao.Instance.GetDeliveryPrintTemplateList(templateName, sysNo);
        }

        #endregion

    }
}
