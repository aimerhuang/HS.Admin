using Hyt.BLL.Log;
using Hyt.BLL.Logistics;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;
using Hyt.Service.Contract.Task;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using System.Transactions;

namespace Hyt.Service.Implement.Task
{
    /// <summary>
    /// 计划任务服务
    /// RESTful 服务 
    /// 注:svc文件需配置Factory="System.ServiceModel.Activation.WebServiceHostFactory" 例 
    /// <%@ ServiceHost Language="C#" Debug="true" Factory="System.ServiceModel.Activation.WebServiceHostFactory" Service="Hyt.Service.Implement.Task.TaskService" CodeBehind="TaskService.svc.cs" %>
    /// </summary>
    /// <remarks>2016-5-31 杨浩 创建</remarks>
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class TaskService : ITaskService
    {
        /// <summary>
        /// 心跳检测
        /// </summary>
        /// <returns></returns>
        /// <remarks>2016-6-2 杨浩 创建</remarks>
        public Result Ping()
        {
            var result = new Result()
            {
                Status = true
            };

            return result;
        }
        /// <summary>
        /// 执行订单返利操作
        /// </summary>
        /// <returns></returns>      
        public Result ExecuteRebatesRecord()
        {
            var result = new Result()
            {
                Status = true
            };

            try
            {
                Hyt.BLL.SellBusiness.CrCustomerRebatesRecordBo.Instance.CrCustomerRebatesRecordToCustomer(Hyt.BLL.Config.Config.Instance.GetGeneralConfig().OrderRebatesRecordDay);
                //BLL.Log.LocalLogBo.Instance.Write("测试", "TaskServiceExecuteRebatesRecordLog");
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "定时执行订单返利操作错误:" + ex.Message,
                                LogStatus.系统日志目标类型.用户, 0, ex);
            }

            return result;
        }
        /// <summary>
        /// 自动确认收货
        /// </summary>
        /// <returns></returns>     
        public Result ExecuteConfirmationOfReceipt()
        {
            var result = new Result()
            {
                Status = true
            };

            try
            {

                Hyt.BLL.Order.SoOrderBo.Instance.AutoConfirmationOfReceipt(Hyt.BLL.Config.Config.Instance.GetGeneralConfig().OrderConfirmationReceiptDay);
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "定时自动确认收货错误:" + ex.Message,
                                LogStatus.系统日志目标类型.用户, 0, ex);
            }


            return result;
        }
        /// <summary>
        /// 清理订单
        /// </summary>
        /// <returns></returns>    
        public Result ExecuteOrder()
        {
            var result = new Result()
            {
                Status = true
            };

            try
            {
                using (var tran = new TransactionScope())
                {
                    Hyt.BLL.Order.SoOrderBo.Instance.ClearOrder(3);
                    tran.Complete();
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "定时清理订单错误:" + ex.Message,
                                LogStatus.系统日志目标类型.用户, 0, ex);
            }
            return result;
        }
        /// <summary>
        /// 执行任务池自动分配
        /// </summary>
        /// <returns></returns>       
        public Result ExecuteSyJob()
        {
            var result = new Result()
            {
                Status = true
            };
            try
            {
                Hyt.BLL.Sys.SyJobPoolManageBo.Instance.AutoAssignJob();
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "定时分配任务池错误:" + ex.Message,
                                 LogStatus.系统日志目标类型.用户, 0, ex);
            }


            return result;
        }


        /// <summary>
        /// 同步订单100  
        /// </summary>
        /// <returns></returns>
        /// <remarks>2017-04-25 杨浩 创建</remarks>
        public Result ExecuteImportErpOrder100()
        {
            var result = new Result()
            {
                Status = true
            };
            try
            {
                BLL.Order.SoOrderBo.Instance.ImportERPOrder100();
              
                //BLL.Log.LocalLogBo.Instance.Write("成功！", "ExecuteImportErpOrder100Log");
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
                //SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "定时同步订单100:" + ex.Message,
                                 //LogStatus.系统日志目标类型.用户, 0, ex);
            }

            return result;
        }

        /// <summary>
        /// 同步第三方商城平台订单
        /// </summary>
        /// <returns></returns>
        /// <remarks>2017-08-31 杨浩 创建</remarks>
        public Result SynchronizeDsMallOrder()
        {
            var result = new Result()
            {
                Status = true
            };

            try
            {               
                BLL.Order.SoOrderBo.Instance.ImportDsMallOrder();           
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;            
            }

            return result;
        }
        /// <summary>
        /// 同步销售出库单到ERP
        /// </summary>
        /// <returns></returns>
        /// <remarks>2017-04-25 杨浩 创建</remarks>      
        public Result ExecuteOrderToErp()
        {
            var result = new Result()
            {
                Status = true
            };
            try
            {
                Hyt.BLL.Sys.EasBo.Instance.SynchronizeOrderToErp();
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "定时同步销售出库单到Erp:" + ex.Message,
                                 LogStatus.系统日志目标类型.用户, 0, ex);
            }

            return result;
        }
        /// <summary>
        /// 同步销售出库单到信业ERP
        /// </summary>
        /// <returns></returns>
        /// <remarks>2017-04-25 杨浩 创建</remarks>
        public Result ExecuteOrderToXinYeErp()
        {
            var result = new Result()
            {
                Status = true
            };
            try
            {
                Hyt.BLL.Sys.EasBo.Instance.SynchronizeOrderToXinYeErp();
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;            
            }

            return result;
        }

        /// <summary>
        /// 同步订单发货信息至第三方商城平台
        /// </summary>
        /// <returns></returns>
        /// 吴琨 2017-08-31 创建
        public Result SynchroOrder()
        {
            var result = new Result()
            {
                Status = true
            };
            try
            {
                LgDeliveryBo.Instance.SynchroOrder();
            }
            catch (Exception e)
            {
                result.Status = false;
                result.Message = e.Message;
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "定时同步订单快递信息至第三方商城:" + e.Message,
                                 LogStatus.系统日志目标类型.用户, 0, e);
            }
            return result;
        }



    }
}
