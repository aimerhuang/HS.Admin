using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.DataAccess.Product;
using Hyt.Model.WorkflowStatus;

namespace Hyt.BLL.Product
{
    /// <summary>
    /// 运费模板
    /// </summary>
    /// <remarks>
    /// 2015-08-06 王耀发 创建
    /// </remarks>
    public class PdProductPrivateBo : BOBase<PdProductPrivateBo>
    {

        #region 定制商品
        /// <summary>
        /// 分页获取定制商品
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns>分页列表</returns>
        /// <remarks>2015-08-06 王耀发 创建</remarks>
        public Pager<PdProductPrivateList> GetPdProductPrivateList(ParaProductPrivateFilter filter)
        {
            return IPdProductPrivateDao.Instance.GetPdProductPrivateList(filter);
        }
          /// <summary>
        /// 招商加盟申请列表（改）
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public Pager<PdProductPrivateList> GetPdProductPrivateLists(ParaProductPrivateFilter filter)
        {
            return IPdProductPrivateDao.Instance.GetPdProductPrivateLists(filter);
        }
        /// <summary>
        /// 获取定制商品信息
        /// </summary>
        /// <param name="sysNo">费模板编号</param>
        /// <returns></returns>
        /// <remarks>2015-08-06 王耀发 创建</remarks>
        public PdProductPrivateList GetEntity(int sysNo)
        {
            return IPdProductPrivateDao.Instance.GetEntity(sysNo);
        }
        /// <summary>
        /// 获取数据(改)
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        public PdProductPrivateList GetEntitys(int sysNo)
        {
            return IPdProductPrivateDao.Instance.GetEntitys(sysNo);
        }
        /// <summary>
        /// 保存定制商品
        /// </summary>
        /// <param name="model">定制商品</param>
        /// <param name="user">操作人</param>
        /// <returns></returns>
        /// <remarks>2015-08-06 王耀发 创建</remarks>
        public Result SavePdProductPrivate(PdProductPrivate model, SyUser user)
        {
            Result r = new Result()
            {
                Status = false
            };
            if (model.SysNo > 0)
            {
                //修改数据
                PdProductPrivateList entity = IPdProductPrivateDao.Instance.GetEntity(model.SysNo);
                model.BrandSysNo = entity.BrandSysNo;
                model.ProductDesc = entity.ProductDesc;
                model.ProductRemark = entity.ProductRemark;
                model.ContactWay = entity.ContactWay;
                model.ProductImage = entity.ProductImage;
                model.AuditDate = DateTime.Now;
                model.CreatedDate = entity.CreatedDate;
                model.CreatedBy = entity.CreatedBy;
                model.LastUpdateBy = user.SysNo;
                model.LastUpdateDate = DateTime.Now;
                IPdProductPrivateDao.Instance.Update(model);
                r.StatusCode = model.SysNo;
                r.Status = true;
            }
            //else
            //{
            //    //新增数据
            //    model.Status = (int)Hyt.Model.WorkflowStatus.LogisticsStatus.运费模板状态.待审核;
            //    model.AuditDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            //    model.CreatedDate = DateTime.Now;
            //    model.CreatedBy = user.SysNo;
            //    model.LastUpdateBy = user.SysNo;
            //    model.LastUpdateDate = DateTime.Now;
            //    r.StatusCode = Hyt.DataAccess.Logistics.ILgFreightModuleDao.Instance.Insert(model);
            //    r.Status = true;
            //}
            return r;
        }
        /// <summary>
        /// 保存定制商品(改)
        /// </summary>
        /// <param name="model"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public Result SavePdProductPrivates(PdProductPrivate model, SyUser user)
        {
            Result r = new Result()
            {
                Status = false
            };
            if (model.SysNo > 0)
            {
                //修改数据
                PdProductPrivateList entity = IPdProductPrivateDao.Instance.GetEntitys(model.SysNo);
                model.BrandSysNo = entity.BrandSysNo;
                model.ProductDesc = entity.ProductDesc;
                model.ProductRemark = entity.ProductRemark;
                model.ContactWay = entity.ContactWay;
                model.ProductImage = entity.ProductImage;
                model.AuditDate = DateTime.Now;
                model.CreatedDate = entity.CreatedDate;
                model.CreatedBy = entity.CreatedBy;
                model.LastUpdateBy = user.SysNo;
                model.LastUpdateDate = DateTime.Now;
                IPdProductPrivateDao.Instance.Update(model);
                r.StatusCode = model.SysNo;
                r.Status = true;
            }
            return r;
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>删除</returns>
        /// <remarks>2015-08-30 王耀发 创建</remarks>
        public Result Delete(int sysNo)
        {
            var res = new Result();
            var r = IPdProductPrivateDao.Instance.Delete(sysNo);
            if (r > 0) res.Status = true;
            return res;
        }
        #endregion
    }
}
