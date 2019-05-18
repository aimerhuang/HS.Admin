using System;
using System.Collections.Generic;
using Hyt.BLL.Log;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.DataAccess.CRM;
using Hyt.Model.WorkflowStatus;

namespace Hyt.BLL.CRM
{
    /// <summary>
    /// 大宗采购业务逻辑类
    /// </summary>
    /// <remarks>2013－06-25 杨晗 创建</remarks>
    public class CrBulkPurchaseBo : BOBase<CrBulkPurchaseBo>
    {
        /// <summary>
        /// 根据系统编号获取大宗采购模型
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>大宗采购实体</returns>
        /// <remarks>2013－06-25 杨晗 创建</remarks>
        public CrBulkPurchase GetModel(int sysNo)
        {
            return ICrBulkPurchaseDao.Instance.GetModel(sysNo);
        }

        /// <summary>
        /// 大宗采购分页查询
        /// </summary>
        /// <param name="pageIndex">起始页</param>
        /// <param name="commitDate">提交时间</param>
        /// <param name="searchStaus">状态</param>
        /// <param name="searchCompany">联系人公司</param>
        /// <param name="searchName">联系人名称</param>
        /// <returns>大宗采购信息列表</returns>
        /// <remarks>2013－06-25 杨晗 创建</remarks>
        public PagedList<CBCrBulkPurchase> Seach(int pageIndex, DateTime? commitDate, int? searchStaus, string searchCompany = null,
                                                 string searchName = null)
        {
            searchStaus = searchStaus ?? 0;

            var list = new PagedList<CBCrBulkPurchase>();

            var pager = new Pager<CBCrBulkPurchase>();
            pager.CurrentPage = pageIndex;
            pager.PageFilter = new CBCrBulkPurchase
                {
                    Status = (int) searchStaus,
                    CommitDate = Convert.ToDateTime(commitDate),
                   
                    CompanyName = searchCompany,
                    ContactName = searchName
                };
            pager.PageSize = list.PageSize;
            pager = ICrBulkPurchaseDao.Instance.Seach(pager);
            list = new PagedList<CBCrBulkPurchase>
                {
                    TData = pager.Rows,
                    CurrentPageIndex = pager.CurrentPage,
                    TotalItemCount = pager.TotalRows
                };
            return list;
        }

        /// <summary>
        /// 插入大宗采购信息
        /// </summary>
        /// <param name="model">插入的数据模型</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－06-25 杨晗 创建</remarks>
        public int Insert(CrBulkPurchase model)
        {
            return ICrBulkPurchaseDao.Instance.Insert(model);
        }

        /// <summary>
        /// 更新大宗采购信息
        /// </summary>
        /// <param name="model">更新的数据模型</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－06-25 杨晗 创建</remarks>
        public int Update(CrBulkPurchase model)
        {
            int u= ICrBulkPurchaseDao.Instance.Update(model);
            SysLog.Instance.Info(LogStatus.系统日志来源.后台,
                                 "大宗采购更改状态", LogStatus.系统日志目标类型.大宗采购, model.SysNo, null, "",
                                 Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo);
            return u;
        }

        /// <summary>
        /// 删除大宗采购信息
        /// </summary>
        /// <param name="sysNo">大宗采购系统编号</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks>2013－06-25 杨晗 创建</remarks>
        public bool Delete(int sysNo)
        {
            SysLog.Instance.Info(LogStatus.系统日志来源.后台,
                                 "删除大宗采购", LogStatus.系统日志目标类型.大宗采购, sysNo, null, "",
                                 Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo);
            return ICrBulkPurchaseDao.Instance.Delete(sysNo);
        }
    }
}
