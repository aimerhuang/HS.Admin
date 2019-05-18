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
    /// 入库明细
    /// </summary>
    /// <remarks>
    /// 2015-08-06 王耀发 创建
    /// </remarks>
    public class PdProductStockInDetailBo : BOBase<PdProductStockInDetailBo>
    {

        #region 定制商品
        /// <summary>
        /// 分页获取入库商品
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns>分页列表</returns>
        /// <remarks>2015-08-06 王耀发 创建</remarks>
        public Pager<PdProductStockInDetailList> GetPdProductStockInDetailList(ParaProductStockInDetailFilter filter)
        {
            return IPdProductStockInDetailDao.Instance.GetPdProductStockInDetailList(filter);
        }

        /// <summary>
        /// 获取入库商品信息
        /// </summary>
        /// <param name="sysNo">入库编号</param>
        /// <returns></returns>
        /// <remarks>2015-08-06 王耀发 创建</remarks>
        public PdProductStockInDetailList GetEntity(int sysNo)
        {
            return IPdProductStockInDetailDao.Instance.GetEntity(sysNo);
        }

        /// <summary>
        /// 保存入库商品
        /// </summary>
        /// <param name="model">入库商品</param>
        /// <param name="user">操作人</param>
        /// <returns></returns>
        /// <remarks>2015-08-06 王耀发 创建</remarks>
        public Result SavePdProductStockInDetail(PdProductStockInDetail model, SyUser user)
        {
            Result r = new Result()
            {
                Status = false
            };
            if (model.SysNo > 0)
            {
                //修改数据
                PdProductStockInDetailList entity = IPdProductStockInDetailDao.Instance.GetEntity(model.SysNo);
                model.DoStorageQuantity = entity.DoStorageQuantity;
                model.CreatedDate = entity.CreatedDate;
                model.CreatedBy = entity.CreatedBy;
                model.LastUpdateBy = user.SysNo;
                model.LastUpdateDate = DateTime.Now;
                IPdProductStockInDetailDao.Instance.Update(model);
                r.StatusCode = model.SysNo;
                r.Status = true;
            }
            else
            {
                //新增数据
                model.DoStorageQuantity = 0;
                model.CreatedDate = DateTime.Now;
                model.CreatedBy = user.SysNo;
                model.LastUpdateBy = user.SysNo;
                model.LastUpdateDate = DateTime.Now;
                r.StatusCode = IPdProductStockInDetailDao.Instance.Insert(model);
                r.Status = true;
            }
            return r;
        }

        public Result AduitUpdatePdProductStockInDetail(PdProductStockInDetail model, SyUser user)
        {
            Result r = new Result()
            {
                Status = false
            };
            if (model.SysNo > 0)
            {
                //修改数据d
                PdProductStockInDetailList entity = IPdProductStockInDetailDao.Instance.GetEntity(model.SysNo);
                model.ProductStockInSysNo = entity.ProductStockInSysNo;
                model.WarehouseSysNo = entity.WarehouseSysNo;
                model.PdProductSysNo = entity.PdProductSysNo;
                model.StorageQuantity = entity.StorageQuantity;
                model.DoStorageQuantity = model.DoStorageQuantity + entity.DoStorageQuantity;
                model.CreatedDate = entity.CreatedDate;
                model.CreatedBy = entity.CreatedBy;
                model.LastUpdateBy = user.SysNo;
                model.LastUpdateDate = DateTime.Now;
                IPdProductStockInDetailDao.Instance.Update(model);
                r.StatusCode = model.SysNo;
                r.Status = true;
            }
            return r;
        }
        /// <summary>
        /// 删除明细
        /// </summary>
        /// <param name="FreightModuleSysNo">运费模板编号</param>
        public void DeleteByProductStockInSysNo(int ProductStockInSysNo)
        {
            IPdProductStockInDetailDao.Instance.DeleteByProductStockInSysNo(ProductStockInSysNo);
        }
        /// <summary>
        /// 获得入库明细
        /// </summary>
        /// <param name="ProductStockInSysNo"></param>
        /// <returns></returns>
        public List<PdProductStockInDetailList> GetProductStockInDetailBy(int ProductStockInSysNo)
        {
            return IPdProductStockInDetailDao.Instance.GetProductStockInDetailBy(ProductStockInSysNo);
        }
        /// <summary>
        /// 获得入库明细
        /// </summary>
        /// <param name="ProductStockInSysNo"></param>
        /// <returns></returns>
        public List<PdProductStockInDetailList> GetAduitProductStockInDetailBy(int ProductStockInSysNo)
        {
            return IPdProductStockInDetailDao.Instance.GetAduitProductStockInDetailBy(ProductStockInSysNo);
        }
        public List<PdProductStockInDetail> GetProductStockInDetail(int ProductStockInSysNo)
        {
            return IPdProductStockInDetailDao.Instance.GetProductStockInDetail(ProductStockInSysNo);
        }
        #endregion
        /// <summary>
        /// 查询当前仓库未入库商品
        /// </summary>
        /// <param name="WarehouseSysNo"></param>
        /// <returns></returns>
        public List<PdProduct> GetNotStockInPd(int WarehouseSysNo)
        {
            return IPdProductStockInDetailDao.Instance.GetNotStockInPd(WarehouseSysNo);
        }
        /// <summary>
        /// 获得推送入库单需要的参数信息
        /// </summary>
        /// <param name="orderSysNo"></param>
        /// <returns>2015-09-02 王耀发 创建</returns>
        public IList<SendSoOrderModel> GetSendSoOrderModelByStockInSysNo(int ProductStockInSysNo)
        {
            return IPdProductStockInDetailDao.Instance.GetSendSoOrderModelByStockInSysNo(ProductStockInSysNo);
        }

        public List<PdProductStock> GetSelectedPdProductStock(List<int> sysNoList)
        {
            return IPdProductStockInDetailDao.Instance.GetSelectedPdProductStock(sysNoList);
        }
    }
}
