using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Hyt.DataAccess.Promotion;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using Hyt.Util;

namespace Hyt.BLL.Promotion
{
    /// <summary>
    /// 组合套餐业务
    /// </summary>
    /// <remarks>2013-08-31 吴文强 创建</remarks>
    public class SpComboBo : BOBase<SpComboBo>
    {
        /// <summary>
        /// 根据组主商品系统编号获取组合套餐信息
        /// </summary>
        /// <param name="productSysNo">组合套餐明细系统编号</param>
        /// <returns>组合套餐集合</returns>
        /// <remarks>2013-09-06 吴文强 创建</remarks>
        public IList<SpCombo> GetComboByMasterProductSysNo(int productSysNo)
        {
            return ISpComboDao.Instance.GetComboByMasterProductSysNo(productSysNo);
        }

        /// <summary>
        /// 获取组合套餐明细列表
        /// </summary>
        /// <param name="comboSysNo">组合套餐系统编号</param>
        /// <returns>组合套餐明细列表</returns>
        /// <remarks>2013-09-06 吴文强 创建</remarks>
        public List<SpComboItem> GetListByComboSysNo(int comboSysNo)
        {
            return ISpComboItemDao.Instance.GetListByComboSysNo(comboSysNo);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>分页</returns>
        /// <remarks>2016-1-15 王耀发 创建</remarks>
        public Pager<CBSpCombo> GetPagerList(ParaSpComboFilter filter)
        {
            return ISpComboDao.Instance.Query(filter);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="sysNo">套餐编号</param>
        /// <returns>套餐实体</returns>
        /// <remarks>2016-1-15 王耀发 创建</remarks>
        public SpCombo GetEntity(int sysNo)
        {
            return ISpComboDao.Instance.GetEntity(sysNo);
        }
        /// <summary>
        /// 保存组合套餐信息
        /// </summary>
        /// <param name="model">组合套餐实体</param>
        /// <param name="gsGroupShoppingItemList">组合套餐商品列表</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2016-1-15 王耀发 创建</remarks>
        public Result Save(SpCombo model, IList<SpComboItem> spComboItemList, SyUser user)
        {
            var res = new Result();
            if (model.SysNo > 0)
            {
                #region 修改

                var upModel = ISpComboDao.Instance.GetEntity(model.SysNo);
                upModel.PromotionSysNo = model.PromotionSysNo;
                upModel.Title = model.Title;
                upModel.WarehouseSysNo = model.WarehouseSysNo;
                upModel.DealerSysNo = model.DealerSysNo;
                upModel.StartTime = model.StartTime;
                upModel.EndTime = model.EndTime;
                upModel.ComboQuantity = model.ComboQuantity;
                upModel.SaleQuantity = model.SaleQuantity;
                upModel.Status = model.Status;
                upModel.SysNo = model.SysNo;
                upModel.AuditorSysNo = model.AuditorSysNo;
                upModel.AuditDate = model.AuditDate;
                upModel.LastUpdateBy = user.SysNo;
                upModel.LastUpdateDate = DateTime.Now;
                ISpComboDao.Instance.Update(upModel); //修改
                if (model.SysNo > 0) //添加子表
                {
                    ISpComboItemDao.Instance.Delete(model.SysNo);
                    if (spComboItemList != null)
                    {
                        foreach (var item in spComboItemList)
                        {
                            var m = new SpComboItem
                            {
                                ComboSysNo = model.SysNo,
                                ProductSysNo = item.ProductSysNo,
                                ProductName = item.ProductName,
                                DiscountAmount = item.DiscountAmount,
                                IsMaster = item.IsMaster
                            };
                            ISpComboItemDao.Instance.Insert(m);

                        }
                    }
                    res.Status = true;
                }
                #endregion
            }
            else //新增
            {
                if (ISpComboItemDao.Instance.IsRepeatSpCombo(model.DealerSysNo, model.WarehouseSysNo, model.Title) > 0)
                {
                    res.Status = false;
                    res.Message = "此组合套餐名称不能重复，请修改!";
                }
                else
                {
                    #region 新增
                    model.LastUpdateBy = user.SysNo;
                    model.LastUpdateDate = DateTime.Now;
                    model.CreatedBy = user.SysNo;
                    model.CreatedDate = DateTime.Now;
                    model.Status = (int)PromotionStatus.组合套餐状态.待审核;
                    model = ISpComboDao.Instance.InsertEntity(model);
                    if (model.SysNo > 0) //添加菜单权限
                    {
                        if (spComboItemList != null)
                        {
                            foreach (var item in spComboItemList)
                            {
                                var m = new SpComboItem
                                {
                                    ComboSysNo = model.SysNo,
                                    ProductSysNo = item.ProductSysNo,
                                    ProductName = item.ProductName,
                                    DiscountAmount = item.DiscountAmount,
                                    IsMaster = item.IsMaster
                                };
                                ISpComboItemDao.Instance.Insert(m);

                            }
                        }
                        res.Status = true;
                    }
                    #endregion
                }
            }
            return res;
        }

        /// <summary>
        /// 审核该套餐
        /// </summary>
        /// <param name="model"></param>
        /// <param name="spComboItemList"></param>
        /// <returns></returns>
        /// <remarks>2016-1-15 王耀发 创建</remarks>
        public Result Audit(SpCombo model, IList<SpComboItem> spComboItemList, SyUser user)
        {
            var res = new Result();
            if (model.SysNo > 0)
            {
                #region 修改

                var upModel = ISpComboDao.Instance.GetEntity(model.SysNo);
                upModel.PromotionSysNo = model.PromotionSysNo;
                upModel.Title = model.Title;
                upModel.StartTime = model.StartTime;
                upModel.EndTime = model.EndTime;
                upModel.ComboQuantity = model.ComboQuantity;
                upModel.SaleQuantity = model.SaleQuantity;
                upModel.Status = model.Status;
                upModel.SysNo = model.SysNo;
                upModel.AuditorSysNo = user.SysNo;
                upModel.AuditDate = DateTime.Now;
                upModel.Status = (int)PromotionStatus.组合套餐状态.已审核;
                upModel.LastUpdateBy = user.SysNo;
                upModel.LastUpdateDate = DateTime.Now;
                ISpComboDao.Instance.Update(upModel); //修改
                if (model.SysNo > 0) //添加子表
                {
                    ISpComboItemDao.Instance.Delete(model.SysNo);
                    if (spComboItemList != null)
                    {
                        foreach (var item in spComboItemList)
                        {
                            var m = new SpComboItem
                            {
                                ComboSysNo = model.SysNo,
                                ProductSysNo = item.ProductSysNo,
                                ProductName = item.ProductName,
                                DiscountAmount = item.DiscountAmount,
                                IsMaster = item.IsMaster
                            };
                            ISpComboItemDao.Instance.Insert(m);

                        }
                    }
                    res.Status = true;
                }
                #endregion
            }
            return res;
        }
        /// <summary>
        /// 审核套餐
        /// </summary>
        /// <param name="sysNo">套餐系统编号</param>
        /// <param name="user">操作用户</param>
        /// <returns></returns>
        /// <remarks>2016-1-15 王耀发 创建</remarks>
        public Result AuditCombo(int sysNo, SyUser user)
        {
            var res = new Result();
            if (sysNo > 0)
            {
                var model = ISpComboDao.Instance.GetEntity(sysNo);
                model.LastUpdateBy = user.SysNo;
                model.LastUpdateDate = DateTime.Now;
                model.Status = (int)PromotionStatus.组合套餐状态.已审核;
                ISpComboDao.Instance.Update(model); //修改
                res.Status = true;
            }
            return res;
        }
        /// <summary>
        /// 取消审核套餐
        /// </summary>
        /// <param name="sysNo">套餐系统编号</param>
        /// <param name="user">操作用户</param>
        /// <returns></returns>
        /// <remarks>2016-1-15 王耀发 创建</remarks>
        public Result CalcelAuditCombo(int sysNo, SyUser user)
        {
            var res = new Result();
            if (sysNo > 0)
            {
                var model = ISpComboDao.Instance.GetEntity(sysNo);
                model.LastUpdateBy = user.SysNo;
                model.LastUpdateDate = DateTime.Now;
                model.Status = (int)PromotionStatus.组合套餐状态.待审核;
                ISpComboDao.Instance.Update(model); //修改
                res.Status = true;
            }
            return res;
        }
        /// <summary>
        /// 作废套餐
        /// </summary>
        /// <param name="sysNo">套餐系统编号</param>
        /// <param name="user">操作用户</param>
        /// <returns></returns>
        /// <remarks>2016-1-15 王耀发 创建</remarks>
        public Result Invalid(int sysNo, SyUser user)
        {
            var res = new Result();
            if (sysNo > 0)
            {
                var model = ISpComboDao.Instance.GetEntity(sysNo);
                model.LastUpdateBy = user.SysNo;
                model.LastUpdateDate = DateTime.Now;
                model.Status = (int)PromotionStatus.组合套餐状态.作废;
                ISpComboDao.Instance.Update(model); //修改
                res.Status = true;
            }
            return res;
        }
    }
}