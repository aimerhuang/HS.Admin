using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.BLL.Log;
using Hyt.DataAccess.QJT;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;

namespace Hyt.BLL.QJT
{
    /// <summary>
    /// 千机团串码记录
    /// </summary>
    /// <remarks>2016-02-17 杨浩 创建</remarks>    
    public class QJTStockOutImeiBo : BOBase<QJTStockOutImeiBo>
    {
        /// <summary>
        /// 添加千机团串码记录
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>系统编号</returns>       
        /// <remarks>2016-02-17 杨浩 创建</remarks>
        public int Add(QJTStockOutImei model)
        {
            model.SysNo = IQJTStockOutImeiDao.Instance.Create(model);
            return model.SysNo;
        }

        /// <summary>
        /// 获取千机团串码记录
        /// </summary>
        /// <param name="sysno">系统编号</param>
        /// <returns>实体</returns>       
        /// <remarks>2016-02-17 杨浩 创建</remarks>
        public QJTStockOutImei Get(int sysno)
        {
            return IQJTStockOutImeiDao.Instance.Get(sysno);

        }

        /// <summary>
        /// 根据商品编号获取商品的串码
        /// </summary>
        /// <param name="stockOutItemSysNo">出库单明细编号</param>
        /// <returns>商品的串码</returns>
        /// <remarks>2016-02-17 杨浩 创建</remarks>
        public IList<QJTStockOutImei> GetImeiByStockOutItemSysNo(int stockOutItemSysNo)
        {
            return IQJTStockOutImeiDao.Instance.GetImeiByStockOutItemSysNo(stockOutItemSysNo);
        }

        /// <summary>
        /// 更新商品的串码
        /// </summary>
        /// <param name="model">千机团串码记录实体</param>
        /// <returns>系统编号</returns>
        /// <remarks>2016-02-19 杨浩 创建</remarks>
        public int Update(QJTStockOutImei model)
        {
            return IQJTStockOutImeiDao.Instance.Update(model);
        }

          /// <summary>
        /// 根据出库单编号获取串码列表
        /// </summary>
        /// <param name="stockoutsysno">出库单编号</param>
        /// <returns></returns>
        /// <remarks>2016-02-17 杨浩 创建</remarks>
        public  IList<QJTStockOutImei> GetImeiByStockOutSysNo(int stockoutsysno)
        {
            return IQJTStockOutImeiDao.Instance.GetImeiByStockOutSysNo(stockoutsysno);
        }
    }
}
