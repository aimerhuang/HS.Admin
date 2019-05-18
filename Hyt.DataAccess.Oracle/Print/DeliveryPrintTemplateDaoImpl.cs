using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Print;
using Hyt.Model;
using Hyt.Model.Transfer;

namespace Hyt.DataAccess.Oracle.Print
{
    /// <summary>
    /// 配送方式打印模板数据访问类
    /// </summary>
    /// <remarks>
    /// 2013-07-16 郑荣华 创建
    /// </remarks>
    public class DeliveryPrintTemplateDaoImpl : IDeliveryPrintTemplateDao
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
        public override int CreateDeliveryPrintTemplate(LgDeliveryPrintTemplate model)
        {
            return Context.Insert<LgDeliveryPrintTemplate>("LgDeliveryPrintTemplate", model)
                          .AutoMap(x => x.SysNo)
                          .ExecuteReturnLastId<int>("SysNo");
        }

        /// <summary>
        /// 修改打印模板信息
        /// </summary>
        /// <param name="model">打印模板实体类</param>
        /// <returns>true:修改成功;false:修改失败</returns>
        /// <remarks>
        /// 2013-07-12 郑荣华 创建
        /// </remarks>
        public override int UpdateDeliveryPrintTemplate(LgDeliveryPrintTemplate model)
        {
            return Context.Update<LgDeliveryPrintTemplate>("LgDeliveryPrintTemplate", model)
                          .AutoMap(x => x.SysNo, x => x.CreatedBy, x => x.CreatedDate)
                          .Where(x => x.SysNo)
                          .Execute();
        }

        /// <summary>
        /// 删除打印模板信息
        /// </summary>
        /// <param name="templateSysNo">打印模板系统编号</param>
        /// <returns>true:删除成功;false:删除失败</returns>
        /// <remarks>
        /// 2013-07-12 郑荣华 创建
        /// </remarks>
        public override int DeleteDeliveryPrintTemplate(int templateSysNo)
        {
            return Context.Delete("LgDeliveryPrintTemplate")
                          .Where("SysNo", templateSysNo)
                          .Execute();
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
        public override IList<CBLgDeliveryPrintTemplate> GetDeliveryPrintTemplateList(int deliveryTypeSysNo)
        {
            const string sql = @"select t.*,b.DeliveryTypeName from LgDeliveryPrintTemplate t 
                                 left join LgDeliveryType b on t.DeliveryTypeSysNo=b.sysno 
                                 where t.DeliveryTypeSysNo=@0";

            return Context.Sql(sql, deliveryTypeSysNo)
                          .QueryMany<CBLgDeliveryPrintTemplate>();
        }

        /// <summary>
        /// 根据系统编号获取打印模板信息
        /// </summary>
        /// <param name="templateSysNo">打印模板系统编号</param>
        /// <returns>打印模板列表信息</returns>
        /// <remarks>
        /// 2013-07-12 郑荣华 创建
        /// </remarks>
        public override CBLgDeliveryPrintTemplate GetDeliveryPrintTemplate(int templateSysNo)
        {
            const string sql = @"select t.*,b.DeliveryTypeName from LgDeliveryPrintTemplate t 
                                 left join LgDeliveryType b on t.DeliveryTypeSysNo=b.sysno 
                                 where t.sysno=@0";

            return Context.Sql(sql, templateSysNo)
                          .QuerySingle<CBLgDeliveryPrintTemplate>();
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
        public override IList<CBLgDeliveryPrintTemplate> GetDeliveryPrintTemplateList(string templateName, int sysNo)
        {
            const string sql = @"select t.* from LgDeliveryPrintTemplate t where t.sysno<>@0 and t.name=@1";
            return Context.Sql(sql, sysNo, templateName)
                          .QueryMany<CBLgDeliveryPrintTemplate>();
        }
        #endregion
    }
}