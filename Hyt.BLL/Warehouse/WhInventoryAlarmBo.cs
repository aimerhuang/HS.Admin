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
    /// 仓库库存报警
    /// </summary>
    /// <remarks>
    /// 2016-06-15 王耀发 创建
    /// </remarks>
    public class WhInventoryAlarmBo : BOBase<WhInventoryAlarmBo>
    {

        #region 仓库库存报警

        public int InserMod(WhInventoryAlarm mod)
        {
            return IWhInventoryAlarmDao.Instance.Insert(mod);
        }

        public void UpdateMod(WhInventoryAlarm mod)
        {
            IWhInventoryAlarmDao.Instance.Update(mod);
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="ProductStockSysNo">库存编号</param>
        /// <returns></returns>
        /// <remarks>2016-06-15 王耀发 创建</remarks>
        public WhInventoryAlarm GetAlarmByStockSysNo(int ProductStockSysNo)
        {
            return IWhInventoryAlarmDao.Instance.GetAlarmByStockSysNo(ProductStockSysNo);
        }

        /// <summary>
        /// 保存报警信息
        /// </summary>
        /// <param name="productStockSysNo">库存编号</param>
        /// <returns></returns>
        /// <remarks>2016-03-22 王耀发 创建</remarks>
        public Result SaveWhInventoryAlarm(int productStockSysNo, WhInventoryAlarm model)
        {
            Result result = new Result();
            result.Status = true;
            WhInventoryAlarm Entity = IWhInventoryAlarmDao.Instance.GetAlarmByStockSysNo(productStockSysNo);
            try
            {
                if (Entity != null)
                {
                    model.CreatedBy = Entity.CreatedBy;
                    model.CreatedDate = Entity.CreatedDate;
                    model.LastUpdateBy = BLL.Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo;
                    model.LastUpdateDate = DateTime.Now;
                    IWhInventoryAlarmDao.Instance.Update(model);
                }
                else
                {
                    model.CreatedBy = BLL.Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo;
                    model.CreatedDate = DateTime.Now;
                    model.LastUpdateBy = BLL.Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo;
                    model.LastUpdateDate = DateTime.Now;
                    IWhInventoryAlarmDao.Instance.Insert(model);
                }
                result.Status = true;
            }
            catch (Exception e)
            {
                result.Status = false;
                result.Message = e.Message;
            }
            return result;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>分页</returns>
        public Pager<CBWhInventoryAlarm> Query(ParaWhInventoryAlarmFilter filter)
        {
            return IWhInventoryAlarmDao.Instance.Query(filter);
        }
        #endregion
        #region 获取库存警报
        /// <summary>
        /// 搜索报警的商品列表
        /// </summary>
        /// <returns></returns>
        public List<CBWhInventoryAlarm> SearAlarmProductStockList()
        {
            return IWhInventoryAlarmDao.Instance.SearAlarmProductStockList();
        }

        public int GetAlarmProductStockCount()
        {
            return IWhInventoryAlarmDao.Instance.GetAlarmProductStockCount();
        }
        #endregion


        public int GetAlarmProductStockCount(IList<WhWarehouse> list)
        {
            return IWhInventoryAlarmDao.Instance.GetAlarmProductStockCount(list);
        }

        public List<WhInventoryAlarm> GetAllAlarm()
        {
            return IWhInventoryAlarmDao.Instance.GetAllAlarm();
        }
    }
}
