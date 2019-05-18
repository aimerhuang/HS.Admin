using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.DataAccess.Warehouse;
using Hyt.Model.WorkflowStatus;

namespace Hyt.BLL.Warehouse
{
    /// <summary>
    /// 入库主表
    /// </summary>
    /// <remarks>
    /// 2015-08-06 王耀发 创建
    /// </remarks>
    public class PdProductStockInBo : BOBase<PdProductStockInBo>
    {

        #region 定制商品

        /// <summary>
        /// 获取定制商品信息
        /// </summary>
        /// <param name="sysNo">费模板编号</param>
        /// <returns></returns>
        /// <remarks>2015-08-06 王耀发 创建</remarks>
        public PdProductStockIn GetEntity(int sysNo)
        {
            return IPdProductStockInDao.Instance.GetEntity(sysNo);
        }

        /// <summary>
        /// 保存定制商品
        /// </summary>
        /// <param name="model">定制商品</param>
        /// <param name="user">操作人</param>
        /// <returns></returns>
        /// <remarks>2015-08-06 王耀发 创建</remarks>
        public Result SavePdProductStockIn(PdProductStockIn model, SyUser user)
        {
            Result r = new Result()
            {
                Status = false
            };
            if (model.SysNo > 0)
            {
                IPdProductStockInDetailDao.Instance.DeleteByProductStockInSysNo(model.SysNo);
                //修改数据
                PdProductStockIn entity = IPdProductStockInDao.Instance.GetEntity(model.SysNo);
                model.Status = entity.Status;
                model.AuditDate = entity.AuditDate;
                model.AuditorSysNo = entity.AuditorSysNo;
                model.CreatedDate = entity.CreatedDate;
                model.CreatedBy = entity.CreatedBy;
                model.LastUpdateBy = user.SysNo;
                model.LastUpdateDate = DateTime.Now;
                IPdProductStockInDao.Instance.Update(model);
                r.StatusCode = model.SysNo;
                r.Status = true;
            }
            else
            {
                //新增数据
                model.Status = (int)Hyt.Model.WorkflowStatus.WarehouseStatus.入库单状态.待入库;
                model.AuditDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                model.AuditorSysNo = 0;
                model.CreatedDate = DateTime.Now;
                model.CreatedBy = user.SysNo;
                model.LastUpdateBy = user.SysNo;
                model.LastUpdateDate = DateTime.Now;
                r.StatusCode = IPdProductStockInDao.Instance.Insert(model);
                r.Status = true;
            }
            return r;
        }
        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <remarks>2015-08-21  王耀发 创建</remarks>
        public bool UpdateStatus(int SysNo, WarehouseStatus.入库单状态 Status, SyUser user)
        {
            return IPdProductStockInDao.Instance.UpdateStatus(SysNo, Status, user.SysNo);

        }

        /// <summary>
        /// 更新发送状态
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <remarks>2015-08-21  王耀发 创建</remarks>
        public bool UpdateSendStatus(int SysNo, int SendStatus)
        {
            return IPdProductStockInDao.Instance.UpdateSendStatus(SysNo, SendStatus);

        }


        public List<PdProductStock> GetSelectedPdProductStock(List<int> sysNoList)
        {
            return IPdProductStockInDetailDao.Instance.GetSelectedPdProductStock(sysNoList);
        }
        #endregion
    }
}
