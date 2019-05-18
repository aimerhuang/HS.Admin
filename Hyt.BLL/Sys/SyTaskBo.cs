using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Hyt.DataAccess.LevelPoint;
using Hyt.Infrastructure.Memory;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;
using IronPython.Modules;
using Extra.SMS;
using Hyt.BLL.Extras;

namespace Hyt.BLL.Sys
{
    public class SyTaskBo : BOBase<SyTaskBo>
    {
        private const string _key = "Hyt.SysTask.";
        /// <summary>
        /// Add
        /// </summary>
        /// <param name="model">model</param>
        /// <returns>sysNo</returns>
        /// <remarks>2013-10-15 杨浩 创建</remarks>
        public int Add(SyTaskConfig model)
        {
            return Hyt.DataAccess.Sys.ISyTaskDao.Instance.Add(model);
        }

        /// <summary>
        /// 获取任务
        /// </summary>
        /// <param name="sysNo">任务系统编号</param>
        /// <returns>SyTaskConfig</returns>
        /// <remarks>2013-10-16 杨浩 创建</remarks>
        public SyTaskConfig GetTask(int sysNo)
        {
            return MemoryProvider.Default.Get(_key + sysNo,() => Hyt.DataAccess.Sys.ISyTaskDao.Instance.GetTask(sysNo));
            // return Hyt.DataAccess.Sys.ISyTaskDao.Instance.GetTask(sysNo);
        }

        /// <summary>
        /// GetAll
        /// </summary>
        /// <returns>IList</returns>
        /// <remarks>2013-10-16 杨浩 创建</remarks>
        public IList<SyTaskConfig> GetAll()
        {
            return Hyt.DataAccess.Sys.ISyTaskDao.Instance.GetAll();
        }

        /// <summary>
        /// 更新任务
        /// </summary>
        /// <param name="task">任务</param>
        /// <returns></returns>
        /// <remarks>2013-10-16 杨浩 创建</remarks>
        public int UpdateTask(SyTaskConfig task)
        {
            MemoryProvider.Default.Remove(_key + task.SysNo);
            return Hyt.DataAccess.Sys.ISyTaskDao.Instance.UpdateTask(task);
        }

        /// <summary>
        /// 查看任务执行日志
        /// </summary>
        /// <param name="sysNo">任务计划编号</param>
        /// <param name="currentPage">当前页</param>
        /// <param name="pageSize">每页数</param>
        /// <returns></returns>
        /// <remarks>2013-10-16 杨浩 创建</remarks>
        public PagedList<SyTaskLog> GetTaskLogs(int sysNo, int currentPage, int pageSize)
        {
            var data = Hyt.DataAccess.Sys.ISyTaskDao.Instance.GetLogs(sysNo, currentPage, pageSize);
            var pager = new PagedList<SyTaskLog>()
                {
                    TData = data.Rows,
                    CurrentPageIndex = data.CurrentPage,
                    TotalItemCount = data.TotalRows
                };
            return pager;
        }

        /// <summary>
        /// 添加一条任务执行日志
        /// </summary>
        /// <param name="model">任务计划日志</param>
        /// <returns>int</returns>
        /// <remarks>2013-10-16 杨浩 创建</remarks>
        public int AddTaskLog(SyTaskLog model)
        {
            return Hyt.DataAccess.Sys.ISyTaskDao.Instance.AddTaskLog(model);
        }

        /// <summary>
        /// 定时清理任务日志
        /// </summary> 
        /// <returns>int</returns>
        /// <remarks>2013-10-18 杨浩 创建</remarks>
        public void ClearTaskLog()
        {
            var list = GetAll();
            foreach (var item in list)
            {
                Hyt.DataAccess.Sys.ISyTaskDao.Instance.ClearTaskLog(item.SysNo);
            }
        }

        /// <summary>
        /// 发送短信任务
        /// </summary>
        /// <param name="sendCount">发送条数</param>
        /// <returns>int</returns>
        /// <remarks>2013-10-22 苟治国 创建</remarks>
        public void SmsTask(int sendCount)
        {
            IList<NcSms> list= Hyt.DataAccess.Sys.ISyTaskDao.Instance.GetSmsTask(sendCount);
            foreach (var item in list)
            {
                SmsResult result = SmsBO.Instance.发送手机短信(item.MobilePhoneNumber, item.SmsContent);
                if (result.Status == SmsResultStatus.Failue)
                {
                    UpdateSmsTaskStatus(item.SysNo, Hyt.Model.WorkflowStatus.NotificationStatus.短信发送状态.待发);
                }
            }
        }

        /// <summary>
        /// 更新短信状态
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <param name="status">状态</param>
        /// <returns>返回操作状态</returns>
        /// <remarks>2013-10-22 苟治国 创建</remarks>
        public int UpdateSmsTaskStatus(int sysNo, Hyt.Model.WorkflowStatus.NotificationStatus.短信发送状态 status)
        {
            return Hyt.DataAccess.Sys.ISyTaskDao.Instance.UpdateSmsTaskStatus(sysNo, status);
        }

        /// <summary>
        /// 调整会员等级
        /// </summary>
        /// <param name="month">月数</param>
        /// <returns>int</returns>
        /// <remarks>2013-11-8 苟治国 创建</remarks>
        /// <remarks>2013-12-18 苟治国 修改 需要调整</remarks>
        public void AdjustmentGradeTask(int month)
        {
            var nowDate = DateTime.Now;
            //中级会员、高级会员
            var list = Hyt.DataAccess.CRM.ICrCustomerDao.Instance.SearchCustomerList();
            foreach (var customer in list)
            {
                //获取最后一条更新的经验积分日志(增加积分)
                //var experiencePoint = LevelPoint.PointBo.Instance.GetExperiencePointLog(customer.SysNo);

                //获取最后一条等级积分日志(增加积分)
                var levelPointLog = LevelPoint.PointBo.Instance.GetLevelPointList(customer.SysNo);

                if (levelPointLog != null)
                {
                    var changeDate = levelPointLog.CreatedDate.AddMonths(month);
                    if (changeDate < nowDate)
                    {
                        //获取开始日期、结束日期内的经验积分日志(增加积分)
                        IList<CrExperiencePointLog> experiencePointList = LevelPoint.PointBo.Instance.GetExperiencePointLog(customer.SysNo,
                                                                                                    levelPointLog.CreatedDate,
                                                                                                    changeDate);
                        //if (experiencePointList.Count == 1)
                        //{
                            //如积分（经验积分）过期未使用或在有效期内未产生二次消费，则系统自动清为0。
                            BLL.LevelPoint.PointBo.Instance.AdjustExperiencePoint(customer.SysNo, 0, -customer.ExperiencePoint, "过期调整");
                            //获取会员最高等级信息
                            var customerLevel = Hyt.BLL.CRM.CrCustomerLevelBo.Instance.GetCustomerUpperLevel();

                            if (((customer.LevelPoint >= customerLevel.LowerLimit) && (customer.LevelPoint <= customerLevel.UpperLimit)))
                            {
                                int evelPoint = customer.LevelPoint - 500;
                                //更新等级积分,同时调整调整等级
                                BLL.LevelPoint.PointBo.Instance.AdjustLevelPoint(customer.SysNo, 0, -evelPoint, "过期调整");
                            }
                            else
                            {
                                //清零初级、中级会员等级积分
                                BLL.LevelPoint.PointBo.Instance.AdjustLevelPoint(customer.SysNo, 0, -customer.LevelPoint, "过期调整");
                            }
                        //}
                    }
                }
            }

        }

        /// <summary>
        /// 定时生成产品索引
        /// </summary>
        /// <returns>空</returns>
        /// <remarks>2013-11-11 苟治国 创建</remarks>
        public void ProductIndex()
        {
            string INDEX_STORE_PATH = @"E:\Pisen\Hyt\Lucene";// ConfigurationManager.AppSettings["LucenePath"];

            var lists = Hyt.BLL.Product.PdProductBo.Instance.GetAllProduct();
            if (lists == null) throw new ArgumentNullException("lists");

            int count = 0;
            int MaxCount = lists.Count;

            Hyt.Infrastructure.Lucene.ProductIndex.Instance.Path = INDEX_STORE_PATH;
            Hyt.Infrastructure.Lucene.ProductIndex.Instance.CreateIndex(true);
            Hyt.Infrastructure.Lucene.ProductIndex.Instance.MaxMergeFactor = 301;
            Hyt.Infrastructure.Lucene.ProductIndex.Instance.MaxBufferedDocs = 301;
            foreach (var mode in lists)
            {

                Hyt.Infrastructure.Lucene.ProductIndex.Instance.IndexString(mode);
                count++;
                if (count >= MaxCount)
                {
                    break;
                }

                if (count % 300 == 0)
                {
                    Hyt.Infrastructure.Lucene.ProductIndex.Instance.CloseWithoutOptimize();
                    Hyt.Infrastructure.Lucene.ProductIndex.Instance.CreateIndex(false);
                    Hyt.Infrastructure.Lucene.ProductIndex.Instance.MaxMergeFactor = 301;
                    Hyt.Infrastructure.Lucene.ProductIndex.Instance.MaxBufferedDocs = 301;
                }
            }
            Hyt.Infrastructure.Lucene.ProductIndex.Instance.Close();
        }

    }
}
