using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Transactions;
using Hyt.BLL.Log;
using Hyt.BLL.Warehouse;
using Hyt.DataAccess.AppContent;
using Hyt.DataAccess.Basic;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.WorkflowStatus;
using Hyt.Util;
using System.IO;

namespace Hyt.BLL.Basic
{
    /// <summary>
    /// 收款科目管理BO
    /// </summary>
    /// <remarks>2013-10-9 hw created</remarks>
    public class ReceiptManagementBo : BOBase<ReceiptManagementBo>
    {
        /// <summary>
        /// import cols mapping between DB and excel
        /// </summary>
        private static readonly Dictionary<string, string> DicColsMapping = new Dictionary<string, string>
            {
                {"Code", "收款科目编码"},
                {"Name", "收款科目名称"},
                {"WhCode", "仓库编码"}
            };

        /// <summary>
        /// query 收款账目
        /// </summary>
        /// <param name="para">ParaBasicReceiptManagement</param>
        /// <param name="id">page index</param>
        /// <param name="pageSize">page size</param>
        /// <returns>list of FnReceiptTitleAssociation</returns>
        /// <remarks>2013-10-9 hw created</remarks>
        public Dictionary<int, List<FnReceiptTitleAssociation>> QueryReceipt(ParaBasicReceiptManagement para, int id = 1,
                                                                             int pageSize = 10)
        {
            return IReceiptManagementDao.Instance.QueryReceipt(para, id, pageSize);
        }

        /// <summary>
        /// query all from FnReceiptTitleAssociation
        /// </summary>
        /// <param></param>
        /// <returns>list of FnReceiptTitleAssociation</returns>
        /// <remarks>2013-10-10 黄伟 创建</remarks>
        public List<FnReceiptTitleAssociation> QueryAll()
        {
            return IReceiptManagementDao.Instance.QueryAll();
        }

        /// <summary>
        /// 导入excel
        /// </summary>
        /// <param name="stream">导入的excel stream format</param>
        /// <param name="userIp">操作人员ip</param>
        /// <param name="operatorSysno">操作人员编号</param>
        /// <param name="payType">支付类型</param>
        /// <returns>封装的泛型result对象</returns>
        /// <remarks>2013-10-10 黄伟 创建</remarks>
        public Result<List<FnReceiptTitleAssociation>> ImportExcel(Stream stream, string userIp, int operatorSysno,
                                                                   int payType = 1)
        {
            //var dt = ExcelUtil.ImportExcel(@"d:\会计科目20131009093409.xls", "编码", "名称", "现金科目", "禁用");
            DataTable dt = null;
            var cols = DicColsMapping.Select(p => p.Value).ToArray();
            try
            {
                dt = ExcelUtil.ImportExcel(stream, cols);
            }
            catch (Exception ex)
            {
                //exception happened,some not caughted
                return new Result<List<FnReceiptTitleAssociation>>
                    {
                        Data = null,
                        Message = string.Format("数据导入错误:{0}", ex.Message),
                        Status = false
                    };
            }
            if (dt == null)
            {
                //not all the cols mapped
                return new Result<List<FnReceiptTitleAssociation>>
                    {
                        Data = null,
                        Message = string.Format("请选择正确的excel!"),
                        Status = false
                    };
            }
            var lstImported = new List<FnReceiptTitleAssociation>();
            var lstExisted = QueryAll();

            //dt to list
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                if (cols.Any(p => (dt.Rows[i][p] == null || string.IsNullOrEmpty(dt.Rows[i][p].ToString()))))
                {
                    //data integrity check falied,just pick up the qualified data
                    continue;
                }

                //同一仓库,支付,第一个设置成默认 
                var isDef = 0;
                var warehouse =
    WhWarehouseBo.Instance.GetWhWareHouseByErpCode(dt.Rows[i][DicColsMapping["WhCode"]].ToString());
                if(warehouse!=null)
                {
                    if (
                        !lstExisted.Any(
                            p =>
                            p.WarehouseSysNo == warehouse.SysNo && p.PaymentTypeSysNo == payType))
                    {
                        isDef = 1;
                    }

                }
                else
                {
                    continue;
                }

                //if (warehouse == null)
                //{
                //    continue;
                //}
                //var pNo=GetSysNoByCode(dt.Rows[i]["编码"].ToString().Split('.')[0]);
                lstImported.Add(new FnReceiptTitleAssociation
                    {
                        EasReceiptCode = dt.Rows[i][DicColsMapping["Code"]].ToString(),
                        EasReceiptName = dt.Rows[i][DicColsMapping["Name"]].ToString(),
                        WarehouseSysNo = warehouse == null ? 0 : warehouse.SysNo, //,
                        PaymentTypeSysNo = payType, //confirmed with team
                        //同一仓库,支付,第一个设置成默认
                        IsDefault = isDef
                        //ParentCode = dt.Rows[i]["编码"].ToString().Split('.')[0]
                    });
            }
            if (!lstImported.Any())
            {
                return new Result<List<FnReceiptTitleAssociation>>
                    {
                        Data = null,
                        Message = string.Format("Excel中无有效数据!"),
                        Status = false
                    };
            }

            //var lstExisted = QueryAll();
            var lstToInsert = new List<FnReceiptTitleAssociation>();
            var lstToUpdate = new List<FnReceiptTitleAssociation>();

            lstImported.ForEach(i =>
                {
                    //according to the col 'Code'
                    //update for the existed data 
                    if (
                        lstExisted.Any(
                            e =>
                            e.EasReceiptCode == i.EasReceiptCode && e.PaymentTypeSysNo == i.PaymentTypeSysNo &&
                            e.WarehouseSysNo == i.WarehouseSysNo))
                    {
                        lstToUpdate.Add(i);
                    }
                    else //insert
                    {
                        lstToInsert.Add(i);
                    }
                });

            //do the db change
            try
            {
                
                    IReceiptManagementDao.Instance.CreateReceipt(lstToInsert, operatorSysno);
                    IReceiptManagementDao.Instance.UpdateReceipt(lstToUpdate, operatorSysno);
                    //UpdateParentNo(lstToInsert.Select(p => p.Code).ToList());
                   
                //using (var tran = new TransactionScope())
                //{
                //    //update parentsysno for the new inserted data
                //    UpdateParentNo(lstToInsert.Select(p => p.Code).ToList());
                //    SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "更新收款账目parentSysno_batch",
                //                             LogStatus.系统日志目标类型.收款账目管理, 0, null, userIp, operatorSysno);
                //    tran.Complete();
                //}
            }
            catch (Exception ex)
            {
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "导入收款账目",
                                         LogStatus.系统日志目标类型.收款账目管理, 0, ex, userIp, operatorSysno);
                return new Result<List<FnReceiptTitleAssociation>>
                    {
                        Data = null,
                        Message = string.Format("数据更新错误!"),
                        Status = false
                    };
            }

            SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "创建收款账目batch",
                                     LogStatus.系统日志目标类型.收款账目管理, 0, null, userIp, operatorSysno);
            SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "更新收款账目batch",
                                     LogStatus.系统日志目标类型.收款账目管理, 0, null, userIp, operatorSysno);

            return new Result<List<FnReceiptTitleAssociation>>
                {
                    Data = null,
                    Message = string.Format("导入成功!"),
                    Status = true
                };
        }

        /// <summary>
        /// 根据系统编码获取系统编号
        /// </summary>
        /// <param name="code">系统编码</param>
        /// <returns>父级系统编号</returns>
        /// <remarks>2013-10-10 黄伟 创建</remarks>
        public int GetSysNoByCode(string code)
        {
            return IReceiptManagementDao.Instance.GetSysNoByCode(code);
        }

        /// <summary>
        /// update parentNo for the datas
        /// </summary>
        /// <param name="sysNo">SysNo of the data</param>
        /// <returns></returns>
        /// <remarks>2013-10-10 黄伟 创建</remarks>
        public void UpdateParentNo(List<string> sysNo)
        {
            IReceiptManagementDao.Instance.UpdateParentNo(sysNo);
        }

        /// <summary>
        /// 新增版本
        /// </summary>
        /// <param name="models">list of FnReceiptTitleAssociation</param>
        /// <param name="userIp">访问者ip</param>
        /// <param name="operatorSysno">操作人员编号</param>
        /// <returns>Result instance</returns>
        /// <remarks>2013-9-10 黄伟 创建</remarks>
        public Result CreateReceipt(List<FnReceiptTitleAssociation> models, string userIp, int operatorSysno)
        {
            if (CheckExist(models))
            {
                return new Result {Status = false, Message = "已存在此科目!"};
            }

            try
            {
                //同一仓库,支付,第一个设置成默认                
                var lstExist = QueryAll();
                models.ForEach(m =>
                    {
                        if (
                            !lstExist.Any(
                                p =>
                                p.WarehouseSysNo == m.WarehouseSysNo && p.PaymentTypeSysNo == m.PaymentTypeSysNo))
                        {
                            m.IsDefault = 1;
                        }
                    });

                
                    IReceiptManagementDao.Instance.CreateReceipt(models, operatorSysno);
                    //UpdateParentNo(models.Select(p => p.Code).ToList());

                
            }
            catch (Exception ex)
            {
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "创建收款账目batch",
                                         LogStatus.系统日志目标类型.收款账目管理, 0, ex, userIp, operatorSysno);
                return new Result {Status = false, Message = string.Format("保存失败:{0}", ex.Message)};
            }

            SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "创建收款账目batch",
                                     LogStatus.系统日志目标类型.收款账目管理, 0, null, userIp, operatorSysno);
            return new Result {Status = true, Message = "保存成功!"};
        }

        /// <summary>
        /// 检查是否有存在的收款科目
        /// </summary>
        /// <param name="models">收款科目管理结合</param>
        /// <returns>true:存在;false:不存在</returns>
        /// <remarks>2013-9-10 黄伟 创建</remarks>
        private bool CheckExist(List<FnReceiptTitleAssociation> models)
        {
            var lstExisted = QueryAll();
            var exist = false;
            models.ForEach(m =>
                {
                    if (
                        lstExisted.Any(
                            p =>
                            p.EasReceiptCode == m.EasReceiptCode && p.WarehouseSysNo == m.WarehouseSysNo &&
                            p.PaymentTypeSysNo == m.PaymentTypeSysNo))
                        exist = true;
                });
            return exist;
        }

        /// <summary>
        /// 更新收款科目
        /// </summary>
        /// <param name="models">list of FnReceiptTitleAssociation</param>
        /// <param name="userIp">访问者ip</param>
        /// <param name="operatorSysno">操作人员编号</param>
        /// <returns></returns>
        /// <remarks>2013-9-10 黄伟 创建</remarks>
        public Result UpdateReceipt(List<FnReceiptTitleAssociation> models, string userIp, int operatorSysno)
        {
            try
            {
                //check for concurrency--load data from db for comparing
                var lst = (from m in models
                           let entity = IReceiptManagementDao.Instance.GetReceipt(m.SysNo)
                           where entity == null || entity.LastUpdateDate.Equals(m.LastUpdateDate)
                           select m);

                if (lst.Any())
                {
                    return new Result {Message = "保存失败!数据已更改,请重新获取后再试!", Status = false};
                }
                //end 

                
                    IReceiptManagementDao.Instance.UpdateReceipt(models, operatorSysno);

            }
            catch (Exception ex)
            {
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "更新收款账目batch",
                                         LogStatus.系统日志目标类型.收款账目管理, 0, ex, userIp, operatorSysno);
                return new Result {Status = false, Message = string.Format("更新失败:{0}", ex.Message)};
            }

            SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "更新收款账目batch",
                                     LogStatus.系统日志目标类型.收款账目管理, 0, null, userIp, operatorSysno);
            return new Result {Status = true, Message = "更新成功!"};
        }

        /// <summary>
        /// 删除版本
        /// </summary>
        /// <param name="lstDelSysnos">要删除的版本编号集合</param>
        /// <param name="userIp">访问者ip</param>
        /// <param name="operatorSysno">操作人员编号</param>
        /// <returns>Result instance</returns>
        /// <remarks>2013-9-10 黄伟 创建</remarks>
        public Result DeleteReceipt(List<int> lstDelSysnos, string userIp, int operatorSysno)
        {
            try
            {
                
                    IReceiptManagementDao.Instance.DeleteReceipt(lstDelSysnos);
               
            }
            catch (Exception ex)
            {
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "删除收款账目",
                                         LogStatus.系统日志目标类型.收款账目管理, 0, ex, userIp, operatorSysno);
                return new Result {Status = false, Message = string.Format("删除失败:{0}", ex.Message)};
            }
            SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "删除收款账目",
                                     LogStatus.系统日志目标类型.收款账目管理, 0, null, userIp, operatorSysno);
            return new Result {Status = true, Message = "操作成功!"};
        }

        /// <summary>
        /// 设置科目启用禁用
        /// </summary>
        /// <param name="id">receipt id</param>
        /// <param name="status">启用或禁用</param>
        /// <param name="userIp">操作人员ip</param>
        /// <param name="operatorSysno">操作人员编号</param>
        /// <returns></returns>
        /// <remarks>2013-10-9 黄伟 创建</remarks>
        public Result SetReceiptStatus(int id, int status, string userIp, int operatorSysno)
        {
            try
            {
                IReceiptManagementDao.Instance.SetReceiptStatus(id, status, operatorSysno);
            }
            catch (Exception ex)
            {
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "更新收款账目",
                                         LogStatus.系统日志目标类型.收款账目管理, 0, ex, userIp, operatorSysno);
                return new Result {Message = "更新失败!", Status = false};
            }

            SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "更新收款账目",
                                     LogStatus.系统日志目标类型.收款账目管理, 0, null, userIp, operatorSysno);

            return new Result {Message = "更新成功!", Status = true};
        }

        /// <summary>
        /// query FnReceiptTitleAssociation by warehouse and paytype
        /// </summary>
        /// <param name="whSysNo">仓库系统编号</param>
        /// <param name="payTypeSysNo">付款方式系统编号</param>
        /// <returns>list of FnReceiptTitleAssociation</returns>
        /// <remarks>2013-10-10 黄伟 创建</remarks>
        public List<FnReceiptTitleAssociation> QueryEasByWhAndPayType(int whSysNo, int payTypeSysNo)
        {
            return IReceiptManagementDao.Instance.QueryEasByWhAndPayType(whSysNo, payTypeSysNo);
        }
    }
}