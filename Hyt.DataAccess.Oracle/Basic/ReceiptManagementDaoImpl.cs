using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Basic;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Oracle.Basic
{
    public class ReceiptManagementDaoImpl : IReceiptManagementDao
    {
        /// <summary>
        /// query 收款账目
        /// </summary>
        /// <param name="para">ParaBasicReceiptManagement</param>
        /// <param name="id">page index</param>
        /// <param name="pageSize">page size</param>
        /// <returns>list of FnReceiptTitleAssociation</returns>
        /// <remarks>2013-10-9 hw created</remarks>
        public override Dictionary<int, List<FnReceiptTitleAssociation>> QueryReceipt(ParaBasicReceiptManagement para,
                                                                                      int id = 1, int pageSize = 10)
        {
            string sqlSelect = @"a.*",
                   sqlFrom = @"FnReceiptTitleAssociation a",
                   sqlCondition =
                                 @"(
                                    @CodeOrName is null or a.EasReceiptCode=@CodeOrName or a.EasReceiptName=@CodeOrName)
                                    and (@IsDef is null or a.IsDefault=@IsDef)
                                    and (@PayMentType is null or a.PAYMENTTYPESYSNO=@PayMentType)
                                    ";

            using (var context = Context.UseSharedConnection(true))
            {
                var lstResult = context.Select<FnReceiptTitleAssociation>(sqlSelect)
                                       .From(sqlFrom)
                                       .AndWhere(sqlCondition)
                                       .Parameter("CodeOrName", para.CodeOrName)
                                       //.Parameter("CodeOrName", para.CodeOrName)
                                       //.Parameter("CodeOrName", para.CodeOrName)
                                       .Parameter("IsDef", para.IsDef == -1 ? null : para.IsDef)
                                       //.Parameter("IsDef", para.IsDef == -1 ? null : para.IsDef)
                                       .Parameter("PayMentType", para.PayMentType == -1 ? null : para.PayMentType)
                                       //.Parameter("PayMentType", para.PayMentType == -1 ? null : para.PayMentType)
                                       .Paging(id, pageSize) //index从1开始
                                       .OrderBy("a.sysno desc")
                                       .QueryMany();
                var count = context.Select<int>(@"count(*)")
                                   .From(sqlFrom)
                                   .AndWhere(sqlCondition)
                                   .Parameter("CodeOrName", para.CodeOrName)
                                   //.Parameter("CodeOrName", para.CodeOrName)
                                   //.Parameter("CodeOrName", para.CodeOrName)
                                   .Parameter("IsDef", para.IsDef == -1 ? null : para.IsDef)
                                   //.Parameter("IsDef", para.IsDef == -1 ? null : para.IsDef)
                                   .Parameter("PayMentType", para.PayMentType == -1 ? null : para.PayMentType)
                                   //.Parameter("PayMentType", para.PayMentType == -1 ? null : para.PayMentType)
                                   .QuerySingle();
                return new Dictionary<int, List<FnReceiptTitleAssociation>> {{count, lstResult}};
            }
        }

        /// <summary>
        /// 根据父级系统编码获取父级系统编号
        /// </summary>
        /// <param name="code">父级系统编码</param>
        /// <returns>父级系统编号</returns>
        /// <remarks>2013-10-10 黄伟 创建</remarks>
        public override int GetSysNoByCode(string code)
        {
            return
                Context.Sql("select sysno from FnReceiptTitleAssociation where EasReceiptCode=@code")
                       .Parameter("code", code)
                       .QuerySingle<int>();
        }

        /// <summary>
        /// 获取收款科目
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>系统编号</returns>
        /// <remarks>2013-11-13 黄伟 创建</remarks>
        public override FnReceiptTitleAssociation GetReceipt(int sysNo)
        {
            return
                Context.Sql("select * from FnReceiptTitleAssociation where sysNo=@sysNo")
                       .Parameter("sysNo", sysNo)
                       .QuerySingle<FnReceiptTitleAssociation>();
        }

        /// <summary>
        /// update parentNo for the datas
        /// </summary>
        /// <param name="sysNo">SysNo of the data</param>
        /// <returns>void</returns>
        /// <remarks>2013-10-10 黄伟 创建</remarks>
        public override void UpdateParentNo(List<string> sysNo)
        {
//            Context.Sql(@"update FnReceiptTitleAssociation t
//set t.parentsysno=substr(t.code, 1, regexp_charindex(t.code, '[.]')-1)
//where sysno in(:sysno)"
//                ).Parameter("sysno", sysNo).Execute();
            Context.Sql(@"update FnReceiptTitleAssociation t
set t.parentsysno=substr(t.code, 1, regexp_charindex(t.code, '[.]')-1)
where code in(@sysno)"
                ).Parameter("sysno", sysNo).Execute();
        }

        /// <summary>
        /// query all FnReceiptTitleAssociation
        /// </summary>
        /// <returns>list of FnReceiptTitleAssociation</returns>
        /// <remarks>2013-10-10 黄伟 创建</remarks>
        public override List<FnReceiptTitleAssociation> QueryAll()
        {
            return Context.Sql("select * from FnReceiptTitleAssociation").QueryMany<FnReceiptTitleAssociation>();
        }

        /// <summary>
        /// query FnReceiptTitleAssociation by warehouse and paytype
        /// </summary>
        /// <param name="whSysNo">仓库系统编号</param>
        /// <param name="payTypeSysNo">付款方式系统编号</param>
        /// <returns>list of FnReceiptTitleAssociation</returns>
        /// <remarks>2013-10-10 黄伟 创建</remarks>
        public override List<FnReceiptTitleAssociation> QueryEasByWhAndPayType(int whSysNo,int payTypeSysNo)
        {
            return Context.Sql("select * from FnReceiptTitleAssociation where warehousesysno=@warehousesysno and paymenttypesysno=@paymenttypesysno").Parameter("warehousesysno", whSysNo).Parameter("paymenttypesysno", payTypeSysNo).QueryMany<FnReceiptTitleAssociation>();
        }

        /// <summary>
        /// 删除财务账目
        /// </summary>
        /// <param name="lstDelSysnos">要删除的财务账目编号集合</param>
        /// <returns></returns>
        /// <remarks>2013-10-9 黄伟 创建</remarks>
        public override void DeleteReceipt(List<int> lstDelSysnos)
        {
            Context.Sql("delete FnReceiptTitleAssociation where sysno in(@lstDelSysnos)")
                   .Parameter("lstDelSysnos", lstDelSysnos)
                   .Execute();
        }

        /// <summary>
        /// 新增财务账目
        /// </summary>
        /// <param name="models">list of FnReceiptTitleAssociation</param>
        /// <param name="operatorSysNo">操作人员编号</param>
        /// <returns>Result instance</returns>
        /// <remarks>2013-10-9 黄伟 创建</remarks>
        public override void CreateReceipt(List<FnReceiptTitleAssociation> models, int operatorSysNo)
        {
            models.ForEach(model =>
                {
                    model.CreatedBy = operatorSysNo;
                    model.CreatedDate = DateTime.Now;
                    model.LastUpdateBy = operatorSysNo;
                    model.LastUpdateDate = DateTime.Now;
                    //model.ParentCode = model.Code.Split('.')[0] == model.Code
                    //                       ? "0"
                    //                       : model.Code.Split('.')[0];
                    //model.IsDefault = FinanceStatus.收款科目状态.启用.GetHashCode();//status default
                    //model.PaymentTypeSysNo=//titletype PaymentTypeSysNo
                    Context.Insert("FnReceiptTitleAssociation", model).AutoMap(m => m.SysNo).Execute();
                });
        }

        /// <summary>
        /// 更新财务账目
        /// </summary>
        /// <param name="models">list of FnReceiptTitleAssociation</param>
        /// <param name="operatorSysNo">操作人员编号</param>
        /// <returns></returns>
        /// <remarks>2013-10-9 黄伟 创建</remarks>
        public override void UpdateReceipt(List<FnReceiptTitleAssociation> models, int operatorSysNo)
        {
            models.ForEach(model =>
                {
                    model.LastUpdateBy = operatorSysNo;
                    model.LastUpdateDate = DateTime.Now;
                    //model.ParentCode = model.Code.Split('.')[0] == model.Code
                    //                       ? "0"
                    //                       : model.Code.Split('.')[0];
                    if(model.WarehouseSysNo!=0)
                        Context.Update("FnReceiptTitleAssociation")
                            //.AutoMap(m => m.SysNo)
                               .Column("EasReceiptCode", model.EasReceiptCode)
                               .Column("EasReceiptName", model.EasReceiptName)
                               .Column("WarehouseSysNo", model.WarehouseSysNo)
                            //.Column("ParentCode", model.ParentCode)
                               .Column("IsDefault", model.IsDefault)
                               .Column("PaymentTypeSysNo", model.PaymentTypeSysNo)
                               .Column("LastUpdateBy", model.LastUpdateBy)
                               .Column("LastUpdateDate", model.LastUpdateDate)
                               .Where("SysNo", model.SysNo).Execute();
                    else
                    {
                        Context.Update("FnReceiptTitleAssociation")
                            //.AutoMap(m => m.SysNo)
                       .Column("EasReceiptCode", model.EasReceiptCode)
                       .Column("EasReceiptName", model.EasReceiptName)
                       //.Column("WarehouseSysNo", model.WarehouseSysNo)
                                            //.Column("ParentCode", model.ParentCode)
                       .Column("IsDefault", model.IsDefault)
                       .Column("PaymentTypeSysNo", model.PaymentTypeSysNo)
                       .Column("LastUpdateBy", model.LastUpdateBy)
                       .Column("LastUpdateDate", model.LastUpdateDate)
                       .Where("SysNo", model.SysNo).Execute();
                    }
                });
        }

        /// <summary>
        /// 设置科目启用禁用
        /// </summary>
        /// <param name="id">receipt id</param>
        /// <param name="status">启用或禁用</param>
        /// <param name="operatorSysNo">操作人员编号</param>
        /// <returns></returns>
        /// <remarks>2013-10-9 黄伟 创建</remarks>
        public override void SetReceiptStatus(int id, int status, int operatorSysNo)
        {
            Context.Update("FnReceiptTitleAssociation")
                   .Column("Status", status)
                   .Column("LastUpdateBy", operatorSysNo)
                   .Column("LastUpdateDate", DateTime.Now)
                   .Where("SysNo", id)
                   .Execute();
        }
    }
}