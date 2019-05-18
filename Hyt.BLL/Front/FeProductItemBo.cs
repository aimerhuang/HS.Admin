using System;
using System.Collections.Generic;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.DataAccess.Front;
using Hyt.Model.WorkflowStatus;

namespace Hyt.BLL.Front
{
    /// <summary>
    /// 商品项 业务层
    /// </summary>
    /// <remarks>2013-07-20 苟治国 创建</remarks>
    public class FeProductItemBo : BOBase<FeProductItemBo>
    {
        /// <summary>
        /// 查看商品项
        /// </summary>
        /// <param name="sysNo">商品项主编号</param>
        /// <returns>商品项</returns>
        /// <remarks>2013-06-20 苟治国 创建</remarks>
        public FeProductItem GetModel(int sysNo)
        {
            return IFeProductItemDao.Instance.GetModel(sysNo);
        }

        /// <summary>
        /// 根据条件获取产品项的列表
        /// </summary>
        /// <param name="pageIndex">索引</param>
        /// <param name="groupSysNo">广告组编号</param>
        /// <param name="status">状态</param>
        /// <param name="erpCode">Erp码号</param>
        /// <param name="productName">产品名称</param>
        /// <returns>广告项列表</returns>
        /// <remarks>2013-10-11 苟治国 创建</remarks>
        public PagedList<Model.CBFeProductItem> Seach(int pageIndex, int groupSysNo, int status, string erpCode, int DealerSysNo, bool IsBindDealer, bool IsBindAllDealer, int DealerCreatedBy, string productName = null, int SelectedDealerSysNo = -1)
        {
            var list = new PagedList<CBFeProductItem>();

            var pager = new Pager<CBFeProductItem>();
            pager.CurrentPage = pageIndex;
            pager.PageFilter = new CBFeProductItem
            {
                GroupSysNo = groupSysNo,
                Status = status,
                ProductName=productName,
                ErpCode = erpCode,
                DealerSysNo = DealerSysNo,
                IsBindDealer = IsBindDealer,
                IsBindAllDealer = IsBindAllDealer,
                DealerCreatedBy = DealerCreatedBy,
                SelectedDealerSysNo = SelectedDealerSysNo
            };

            pager.PageSize = list.PageSize;
            pager = IFeProductItemDao.Instance.Seach(pager);
            list = new PagedList<CBFeProductItem>
            {
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows
            };
            return list;
        }

        /// <summary>
        /// 根据商品组获取所有商品项分类
        /// </summary>
        /// <param name="groupSysNo">商品组编号</param>
        /// <returns>商品项列表</returns>
        /// <remarks>2013－06-21 苟治国 创建</remarks>
        public IList<FeProductItem> GetListByGroup(int groupSysNo, int dealersysno)
        {
            return IFeProductItemDao.Instance.GetListByGroup(groupSysNo, dealersysno);
        }

        /// <summary>
        /// 查看在当前类型中是否有相同产品称
        /// </summary>
        /// <param name="mid">产品组编号</param>
        /// <param name="productSysNo">商品编号</param>
        /// <returns>总数</returns>
        /// <remarks>2013－06-21 苟治国 创建</remarks>
        public int GetCount(int mid, int productSysNo)
        {
            return IFeProductItemDao.Instance.GetCount(mid, productSysNo);
        }

        /// <summary>
        /// 新增产品项
        /// </summary>
        /// <param name="model">插入的数据模型</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-06-21 苟治国 创建</remarks>
        public int Insert(FeProductItem model)
        {
            int result = IFeProductItemDao.Instance.Insert(model);
            if (result > 0)
            {
                RemoveCache(model.GroupSysNo);
                BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, string.Format("修改产品项{0}", model.SysNo), LogStatus.系统日志目标类型.商品组展示, Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo);
            }
            else
            {
                BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台, string.Format("修改产品项{0}失败", model.SysNo), LogStatus.系统日志目标类型.商品组展示, Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo);
            }
            return result;
        }

        /// <summary>
        /// 更新产品项
        /// </summary>
        /// <param name="model">更新的数据模型</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-06-21 苟治国 创建</remarks>
        public int Update(FeProductItem model)
        {
            int result = IFeProductItemDao.Instance.Update(model);
            if (result>0)
            {
                if (Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo !=Hyt.Model.SystemPredefined.Constant.UserSysNo)
                {
                    Hyt.BLL.Sys.SyJobPoolPublishBo.Instance.FeAudit(model.SysNo, model.GroupSysNo, Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo, "ProductItem", "修改商品项", Hyt.Model.SystemPredefined.Constant.UserSysNo, (int)Hyt.Model.WorkflowStatus.SystemStatus.任务对象类型.通知);
                }
                
                RemoveCache(model.GroupSysNo);
            }
            return result;
        }

        /// <summary>
        /// 更新商品项状态
        /// </summary>
        /// <param name="collocation">商品项实体</param>
        /// <returns>返回受影响的行</returns>
        /// <remarks>2013－06-21 苟治国 创建</remarks>
        public int UpdateStatus(IList<FeProductItem> collocation)
        {
            int result = 0;
            try
            {
                for (int i = 0; i < collocation.Count; i++)
                {
                    int sysNo = Convert.ToInt32(collocation[i].SysNo);
                    int status = Convert.ToInt32(collocation[i].Status);

                    var model = this.GetModel(sysNo);
                    model.Status = status;
                    result = this.Update(model);
                    if (result > 0)
                    {
                        RemoveCache(model.GroupSysNo);
                        if (Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo !=Hyt.Model.SystemPredefined.Constant.UserSysNo)
                        {
                            Hyt.BLL.Sys.SyJobPoolPublishBo.Instance.FeAudit(model.SysNo, model.GroupSysNo, Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo, "ProductItem", "修改商品项状态", Hyt.Model.SystemPredefined.Constant.UserSysNo, (int)Hyt.Model.WorkflowStatus.SystemStatus.任务对象类型.通知);
                        }
                        
                        BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, string.Format("修改商品项状态{0}", model.SysNo), LogStatus.系统日志目标类型.商品组展示, Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo);
                    }
                    else
                    {
                        BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台, string.Format("修改商品项状态{0}失败", model.SysNo), LogStatus.系统日志目标类型.商品组展示, Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo);
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        /// <summary>
        /// 删除产品项
        /// </summary>
        /// <param name="sysNo">产品项编号</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks>2013-06-21 苟治国 创建</remarks>
        public bool Delete(int sysNo)
        {
            return IFeProductItemDao.Instance.Delete(sysNo);
        }

        /// <summary>
        /// 移除前台广告商品缓存
        /// </summary>
        /// <param name="groupSysNo">商品组编号</param>
        /// <returns>空</returns>
        /// <remarks>2013-12-03 苟治国 创建</remarks>
        public void RemoveCache(int groupSysNo)
        {
            var model = BLL.Front.FeProductGroupBo.Instance.GetModel(groupSysNo);
            if (model != null)
            {
                Hyt.Infrastructure.Caching.CacheManager.RemoveCache(Hyt.Infrastructure.Caching.CacheKeys.Items.WebProductItem_, model.Code);
            }
        }
        /// <summary>
        /// 同步总部已审核的商品项
        /// </summary>
        /// <param name="GroupSysNo"></param>
        /// <param name="DealerSysNo"></param>
        /// <param name="CreatedBy"></param>
        /// <returns></returns>
        /// <remarks>2016-1-13 王耀发 创建</remarks>
        public int ProCreateFeProductItem(int GroupSysNo, int DealerSysNo, int CreatedBy)
        {
            return IFeProductItemDao.Instance.ProCreateFeProductItem(GroupSysNo, DealerSysNo, CreatedBy);
        }
    }
}
