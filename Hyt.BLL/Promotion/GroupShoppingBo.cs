using System;
using System.Collections.Generic;
using System.Linq;
using Hyt.BLL.Basic;
using Hyt.DataAccess.Promotion;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.BLL.Promotion
{
    /// <summary>
    /// 团购
    /// </summary>
    /// <remarks>2013-08-20 朱家宏 创建</remarks>
    public class GroupShoppingBo : BOBase<GroupShoppingBo>
    {
        /// <summary>
        /// 团购分页列表
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>分页</returns>
        /// <remarks>2013-08-20 朱家宏 创建</remarks>
        public Pager<GsGroupShopping> GetPagerList(ParaGroupShoppingFilter filter)
        {
            return IGsGroupShoppingDao.Instance.Query(filter);
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="sysNo">团购编号</param>
        /// <returns>团购实体</returns>
        /// <remarks>2013-08-22 余勇 创建</remarks>
        public GsGroupShopping Get(int sysNo)
        {
            return IGsGroupShoppingDao.Instance.Get(sysNo);
        }

        /// <summary>
        /// 获取团购商品列表
        /// </summary>
        /// <param name="sysNo">团购编号</param>
        /// <returns>团购实体列表</returns>
        /// <remarks>2013-08-22 余勇 创建</remarks>
        public IList<GsGroupShoppingItem> GetGroupShoppingItem(int sysNo)
        {
            return IGsGroupShoppingItemDao.Instance.GetItem(sysNo);
        }

        /// <summary>
        /// 获取团购的覆盖地区
        /// </summary>
        /// <param name="groupShoppingSysNo">团购编号</param>
        /// <returns>团购的覆盖地区列表</returns>
        /// <remarks>2013-08-22 余勇 创建</remarks>
        public IList<BsArea> GetAreaByGroupShoppingSysNo(int groupShoppingSysNo)
        {
            return IGsGroupShoppingDao.Instance.GetAreaByGroupShoppingSysNo(groupShoppingSysNo);
        }

        /// <summary>
        /// 获取团购的覆盖地区
        /// </summary>
        /// <param name="groupShoppingSysNo">团购系统编号</param>
        /// <returns>团购的覆盖地区列表</returns>
        /// <remarks>2013-08-07 余勇 创建</remarks>
        public List<ZCheckTreeNode> GetAreaTreeByGroupShoppingSysNo(int groupShoppingSysNo)
        {
            var lstResult = new List<ZCheckTreeNode>();
            IList<BsArea> lstM = IGsGroupShoppingDao.Instance.GetAreaByGroupShoppingSysNo(groupShoppingSysNo);//团购的地区
            var lst = BasicAreaBo.Instance.GetAllAreaForTree();//所有地区
            if (lst != null)
            {
                //子级
                BuildTree(0, lst, lstM, lstResult);
            }
            return lstResult;
        }

        /// <summary>
        /// 生成树形
        /// </summary>
        /// <param name="pmeunId">上级地区编号</param>
        /// <param name="lst">所有地区</param>
        /// <param name="lstM">子级编号</param>
        /// <param name="lstResult">结果</param>
        /// <remarks>2013-08-01 朱成果 创建</remarks>
        private void BuildTree(int pmeunId, IEnumerable<BsArea> lst, IList<BsArea> lstM, List<ZCheckTreeNode> lstResult)
        {
            var syMenus = lst as BsArea[] ?? lst.ToArray();
            List<BsArea> sublist = syMenus.Where(m => m.ParentSysNo == pmeunId).ToList();
            if (sublist.Count > 0)
            {
                foreach (BsArea s in sublist)
                {
                    //添加子级
                    lstResult.Add(new ZCheckTreeNode
                    {
                        id = "m_" + s.SysNo,
                        name = s.AreaName,
                        nodetype = 0,
                        open = true,
                        @checked = lstM.Any(m => m.SysNo == s.SysNo),
                        pId = "m_" + s.ParentSysNo
                    });
                    BuildTree(s.SysNo, syMenus, lstM, lstResult);
                }
            }
        }

        /// <summary>
        /// 保存团购信息
        /// </summary>
        /// <param name="model">团购实体</param>
        /// <param name="gsGroupShoppingItemList">团购商品列表</param>
        /// <param name="gsSupportAreaList">团购支持区域列表</param>
        /// <param name="user">当前用户</param>
        /// <returns></returns>
        /// <remarks>2013-09-02 余勇 创建</remarks>
        public Result Save(GsGroupShopping model, IList<GsGroupShoppingItem> gsGroupShoppingItemList, IList<GsSupportArea> gsSupportAreaList, SyUser user)
        {
            var res = new Result();
            if (model.SysNo > 0)
            {
                #region 修改

                var upModel = IGsGroupShoppingDao.Instance.Get(model.SysNo);
                upModel.WarehouseSysNo = model.WarehouseSysNo;
                upModel.DealerSysNo = model.DealerSysNo;
                upModel.Title = model.Title;
                upModel.Subtitle = model.Subtitle;
                upModel.ImageUrl = model.ImageUrl;
                upModel.IconUrl = model.IconUrl;
                upModel.StartTime = model.StartTime;

                upModel.EndTime = model.EndTime;
                upModel.MaxQuantity = model.MaxQuantity;
                upModel.MinQuantity = model.MinQuantity;
                upModel.HaveQuantity = model.HaveQuantity;
                upModel.LimitQuantity = model.LimitQuantity;

                upModel.TotalPrice = model.TotalPrice;
                upModel.GroupPrice = model.GroupPrice;
                upModel.Discount = model.Discount;
                upModel.DisplayOrder = model.DisplayOrder;
                upModel.GroupType = model.GroupType;

                upModel.SupportAreaType = model.SupportAreaType;
                upModel.Remarks = model.Remarks;
                upModel.SysNo = model.SysNo;
                upModel.Status = model.Status;
                upModel.Description = model.Description;
                upModel.LastUpdateBy = user.SysNo;
                upModel.LastUpdateDate = DateTime.Now;
                IGsGroupShoppingDao.Instance.Update(upModel); //修改
                if (model.SysNo > 0) //添加子表
                {
                    IGsGroupShoppingItemDao.Instance.Delete(model.SysNo);
                    IGsSupportAreaDao.Instance.Delete(model.SysNo);
                    if (gsGroupShoppingItemList != null)
                    {
                        foreach (var item in gsGroupShoppingItemList)
                        {
                            var m = new GsGroupShoppingItem
                            {
                                GroupShoppingPrice = item.GroupShoppingPrice,
                                GroupShoppingSysNo = model.SysNo,
                                OriginalPrice = item.OriginalPrice,
                                ProductName = item.ProductName,
                                ProductSysNo = item.ProductSysNo
                            };
                            IGsGroupShoppingItemDao.Instance.InsertEntity(m);

                        }
                    }
                    if (gsSupportAreaList != null)
                    {
                        foreach (var item in gsSupportAreaList)
                        {
                            var m = new GsSupportArea
                            {
                                AreaSysNo = item.AreaSysNo,
                                CreatedBy = user.SysNo,
                                CreatedDate = DateTime.Now,
                                GroupShoppingSysNo = model.SysNo,
                                LastUpdateBy = user.SysNo,
                                LastUpdateDate = DateTime.Now
                            };
                            IGsSupportAreaDao.Instance.InsertEntity(m);

                        }
                    }
                    res.Status = true;
                }
                #endregion
            }
            else //新增
            {
                #region 新增
                model.LastUpdateBy = user.SysNo;
                model.LastUpdateDate = DateTime.Now;
                model.CreatedBy = user.SysNo;
                model.CreatedDate = DateTime.Now;
                model.Status = (int) GroupShoppingStatus.团购状态.待审;
                model = IGsGroupShoppingDao.Instance.InsertEntity(model);
                if (model.SysNo > 0) //添加菜单权限
                {
                    if (gsGroupShoppingItemList != null)
                    {
                        foreach (var item in gsGroupShoppingItemList)
                        {
                            var m = new GsGroupShoppingItem
                            {
                                GroupShoppingPrice = item.GroupShoppingPrice,
                                GroupShoppingSysNo = model.SysNo,
                                OriginalPrice = item.OriginalPrice,
                                ProductName = item.ProductName,
                                ProductSysNo = item.ProductSysNo
                            };
                            IGsGroupShoppingItemDao.Instance.InsertEntity(m);

                        }
                    }
                    if (gsSupportAreaList != null)
                    {
                        foreach (var item in gsSupportAreaList)
                        {
                            var m = new GsSupportArea
                            {
                                AreaSysNo = item.AreaSysNo,
                                CreatedBy = user.SysNo,
                                CreatedDate = DateTime.Now,
                                GroupShoppingSysNo = model.SysNo,
                                LastUpdateBy = user.SysNo,
                                LastUpdateDate = DateTime.Now
                            };
                            IGsSupportAreaDao.Instance.InsertEntity(m);

                        }
                    }
                    res.Status = true;
                }
                #endregion
            }
            return res;
        }

        /// <summary>
        /// 审核团购
        /// </summary>
        /// <param name="model">团购系统编号</param>
        /// <param name="gsGroupShoppingItemList">团购商品列表</param>
        /// <param name="gsSupportAreaList">团购地区列表</param>
        /// <param name="user">操作用户</param>
        /// <returns></returns>
        /// <remarks>2013-09-02 余勇 创建</remarks>
        public Result Audit(GsGroupShopping model, IList<GsGroupShoppingItem> gsGroupShoppingItemList, IList<GsSupportArea> gsSupportAreaList, SyUser user)
        {
            var res = new Result();
            if (model.SysNo > 0)
            {
                #region 修改
                var upModel = IGsGroupShoppingDao.Instance.Get(model.SysNo);
                upModel.LastUpdateBy = user.SysNo;
                upModel.LastUpdateDate = DateTime.Now;
                upModel.Title = model.Title;
                upModel.Subtitle = model.Subtitle;
                upModel.ImageUrl = model.ImageUrl;
                upModel.IconUrl = model.IconUrl;
                upModel.StartTime = model.StartTime;

                upModel.EndTime = model.EndTime;
                upModel.MaxQuantity = model.MaxQuantity;
                upModel.MinQuantity = model.MinQuantity;
                upModel.HaveQuantity = model.HaveQuantity;
                upModel.LimitQuantity = model.LimitQuantity;

                upModel.TotalPrice = model.TotalPrice;
                upModel.GroupPrice = model.GroupPrice;
                upModel.Discount = model.Discount;
                upModel.DisplayOrder = model.DisplayOrder;
                upModel.GroupType = model.GroupType;

                upModel.SupportAreaType = model.SupportAreaType;
                upModel.Remarks = model.Remarks;
                upModel.SysNo = model.SysNo;
                upModel.Description = model.Description;
                upModel.AuditDate = DateTime.Now;
                upModel.Auditor = user.SysNo;
                upModel.Status = (int)GroupShoppingStatus.团购状态.已审;
                IGsGroupShoppingDao.Instance.Update(upModel); //修改
                if (model.SysNo > 0) //添加子表
                {
                    IGsGroupShoppingItemDao.Instance.Delete(model.SysNo);
                    IGsSupportAreaDao.Instance.Delete(model.SysNo);
                    if (gsGroupShoppingItemList != null)
                    {
                        foreach (var item in gsGroupShoppingItemList)
                        {
                            var m = new GsGroupShoppingItem
                            {
                                GroupShoppingPrice = item.GroupShoppingPrice,
                                GroupShoppingSysNo = model.SysNo,
                                OriginalPrice = item.OriginalPrice,
                                ProductName = item.ProductName,
                                ProductSysNo = item.ProductSysNo
                            };
                            IGsGroupShoppingItemDao.Instance.InsertEntity(m);

                        }
                    }
                    if (gsSupportAreaList != null)
                    {
                        foreach (var item in gsSupportAreaList)
                        {
                            var m = new GsSupportArea
                            {
                                AreaSysNo = item.AreaSysNo,
                                CreatedBy = user.SysNo,
                                CreatedDate = DateTime.Now,
                                GroupShoppingSysNo = model.SysNo,
                                LastUpdateBy = user.SysNo,
                                LastUpdateDate = DateTime.Now
                            };
                            IGsSupportAreaDao.Instance.InsertEntity(m);

                        }
                    }
                    res.Status = true;
                }
                #endregion
            }
            return res;
        }

        /// <summary>
        /// 取消审核团购
        /// </summary>
        /// <param name="sysNo">团购系统编号</param>
        /// <param name="user">操作用户</param>
        /// <returns></returns>
        /// <remarks>2013-09-02 余勇 创建</remarks>
        public Result CalcelAuditGroup(int sysNo, SyUser user)
        {
            var res = new Result();
            if (sysNo > 0)
            {
                var model = IGsGroupShoppingDao.Instance.Get(sysNo);
                model.LastUpdateBy = user.SysNo;
                model.LastUpdateDate = DateTime.Now;
                model.Status = (int)GroupShoppingStatus.团购状态.待审;
                IGsGroupShoppingDao.Instance.Update(model); //修改
                res.Status = true;
            }
            return res;
        }

        /// <summary>
        /// 作废团购
        /// </summary>
        /// <param name="sysNo">团购系统编号</param>
        /// <param name="user">操作用户</param>
        /// <returns></returns>
        /// <remarks>2013-09-02 余勇 创建</remarks>
        public Result Invalid(int sysNo, SyUser user)
        {
            var res = new Result();
            if (sysNo > 0)
            {
                var model = IGsGroupShoppingDao.Instance.Get(sysNo);
                model.LastUpdateBy = user.SysNo;
                model.LastUpdateDate = DateTime.Now;
                model.Status = (int)GroupShoppingStatus.团购状态.作废;
                IGsGroupShoppingDao.Instance.Update(model); //修改
                res.Status = true;
            }
            return res;
        }

        /// <summary>
        /// 根据商品系统编号获取团购信息
        /// </summary>
        /// <param name="productSysNo">组合套餐明细系统编号</param>
        /// <returns>团购信息集合</returns>
        /// <remarks>2013-09-25 余勇 创建</remarks>
        public IList<GsGroupShopping> GetGroupShoppingByProductSysNo(int productSysNo)
        {
            return IGsGroupShoppingDao.Instance.GetGroupShoppingByProductSysNo(productSysNo);
        }
    }
}
