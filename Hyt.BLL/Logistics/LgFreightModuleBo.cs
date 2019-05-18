using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.DataAccess.Logistics;
using Hyt.Model.WorkflowStatus;
using Hyt.Model.Common;

namespace Hyt.BLL.Logistics
{
    /// <summary>
    /// 运费模板
    /// </summary>
    /// <remarks>
    /// 2015-08-06 王耀发 创建
    /// </remarks>
    public class LgFreightModuleBo : BOBase<LgFreightModuleBo>
    {

        #region 运费模板
        /// <summary>
        /// 分页获取运费模板
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns>分页列表</returns>
        /// <remarks>2015-08-06 王耀发 创建</remarks>
        public Pager<LgFreightModule> GetLgFreightModuleList(ParaFreightModule filter)
        {
            return ILgFreightModuleDao.Instance.GetLgFreightModuleList(filter);
        }

        /// <summary>
        /// 获取运费模板信息
        /// </summary>
        /// <param name="sysNo">费模板编号</param>
        /// <returns></returns>
        /// <remarks>2015-08-06 王耀发 创建</remarks>
        public LgFreightModule GetEntity(int sysNo)
        {
            return Hyt.DataAccess.Logistics.ILgFreightModuleDao.Instance.GetEntity(sysNo);
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="ProductAddress">商品地址编号</param>
        /// <returns>数据实体</returns>
        /// <remarks>2015-08-06 王耀发 创建</remarks>
        public LgFreightModule GetEntityByProductAddress(int ProductAddress)
        {
            return Hyt.DataAccess.Logistics.ILgFreightModuleDao.Instance.GetEntityByProductAddress(ProductAddress);
        }

        /// <summary>
        /// 保存运费模板
        /// </summary>
        /// <param name="model">运费模板</param>
        /// <param name="user">操作人</param>
        /// <returns></returns>
        /// <remarks>2015-08-06 王耀发 创建</remarks>
        public Result SaveFreightModule(LgFreightModule model, SyUser user)
        {
            Result r = new Result()
            {
                Status = false
            };
            if (model.SysNo > 0)
            {
                //Hyt.DataAccess.Logistics.IFreightModuleDetailsDao.Instance.DeleteByFreightModuleSysNo(model.SysNo);
                //修改数据
                LgFreightModule entity = Hyt.DataAccess.Logistics.ILgFreightModuleDao.Instance.GetEntity(model.SysNo);
                model.Status = entity.Status;
                model.AuditDate = entity.AuditDate;
                model.CreatedDate = entity.CreatedDate;
                model.CreatedBy = entity.CreatedBy;
                model.LastUpdateBy = user.SysNo;
                model.LastUpdateDate = DateTime.Now;
                Hyt.DataAccess.Logistics.ILgFreightModuleDao.Instance.Update(model);
                r.StatusCode = model.SysNo;
                r.Status = true;
            }
            else
            {
                //新增数据
                model.Status = (int)Hyt.Model.WorkflowStatus.LogisticsStatus.运费模板状态.待审核;
                model.AuditDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                model.CreatedDate = DateTime.Now;
                model.CreatedBy = user.SysNo;
                model.LastUpdateBy = user.SysNo;
                model.LastUpdateDate = DateTime.Now;
                r.StatusCode = Hyt.DataAccess.Logistics.ILgFreightModuleDao.Instance.Insert(model);
                r.Status = true;
            }
            return r;
        }

        /// <summary>
        /// 审核运费模板
        /// </summary>
        /// <param name="sysNo">运费模板编号</param>
        /// <param name="user">操作人</param>
        /// <param name="status">审核状态</param>
        /// <returns></returns>
        /// <remarks>
        /// 2015-08-06 王耀发 创建
        /// 2015-11-22 杨浩 增加审核状态参数
        /// </remarks>
        public Result AuditFreightModule(int sysNo, SyUser user, int status = 20)
        {
            Result r = new Result()
            {
                Status = false
            };
            var entity = GetEntity(sysNo);
            if (entity != null)
            {

                entity.LastUpdateBy = user.SysNo;
                entity.LastUpdateDate = DateTime.Now;
                entity.Status = status;
                entity.AuditDate = DateTime.Now;
                entity.AuditorSysNo = user.SysNo;
                Hyt.DataAccess.Logistics.ILgFreightModuleDao.Instance.Update(entity);
                r.Status = true;

            }
            else
            {
                r.Message = "模板数据不存在";
            }
            return r;
        }

        /// <summary>
        /// 作废运费模板
        /// </summary>
        /// <param name="sysNo">运费模板编号</param>
        /// <param name="user">操作人</param>
        /// <returns></returns>
        /// <remarks>2015-08-06 王耀发 创建</remarks>
        public Result CancelFreightModule(int sysNo, SyUser user)
        {
            Result r = new Result()
            {
                Status = false
            };
            var entity = GetEntity(sysNo);
            if (entity != null)
            {

                entity.LastUpdateBy = user.SysNo;
                entity.LastUpdateDate = DateTime.Now;
                entity.Status = (int)Hyt.Model.WorkflowStatus.LogisticsStatus.运费模板状态.作废;
                Hyt.DataAccess.Logistics.ILgFreightModuleDao.Instance.Update(entity);
                r.Status = true;

            }
            else
            {
                r.Message = "模板数据不存在";
            }
            return r;
        }

        /// <summary>
        /// 获取运费模板列表
        /// </summary>
        /// <param name="pager">模板查询条件</param>
        /// <returns>模板列表</returns>
        /// <remarks>2015-08-06 王耀发 创建</remarks>
        public Pager<LgFreightModule> GetFreightModuleList(Pager<LgFreightModule> pager)
        {
            return ILgFreightModuleDao.Instance.GetFreightModuleList(pager);
        }

        /// <summary>
        /// 获取运费模板列表
        /// </summary>
        /// <returns>模板列表</returns>
        /// <remarks>2015-08-06 杨云奕 创建</remarks>
        public List<LgFreightModule> GetFreightModuleList()
        {
            return ILgFreightModuleDao.Instance.GetFreightModuleList();
        }
        #endregion

    }
}
