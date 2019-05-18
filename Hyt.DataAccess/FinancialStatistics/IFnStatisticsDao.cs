using Hyt.DataAccess.Base;
using Hyt.Model.FinancialStatistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.FinancialStatistics
{
    /// <summary>
    /// 统计表操作
    /// </summary>
    public abstract class IFnStatisticsDao : DaoBase<IFnStatisticsDao>
    {
        #region 财务统计表主表
        /// <summary>
        /// 添加统计表
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public abstract int InsertFnStatistics(FnStatistics mod);
        /// <summary>
        /// 修改统计表示数据
        /// </summary>
        /// <param name="mod"></param>
        public abstract void UpdateFnStatistics(FnStatistics mod);
        /// <summary>
        /// 删除统计表数据
        /// </summary>
        /// <param name="SysNo"></param>
        public abstract void DeleteFnStatistics(int SysNo);
        /// <summary>
        /// 获取统计表明细内容
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public abstract List<CBFnStatistics> GetCBFnStatisticList(int year);
        /// <summary>
        /// 获取统计表明细内容
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public abstract CBFnStatistics GetCBFnStatisticMod(int SysNo);

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <param name="year"></param>
        /// <param name="mouth"></param>
        /// <returns></returns>
        public abstract AllStatistics GetAllStatistics(int year, int mouth);

        public abstract List<StatisticsDataMod> GetStatisticsDataMod(string type, int year, int month);

        public abstract CBFnStatistics GetCBFnStatisticMod(DateTime dateTime);
        #endregion

        #region 财务统计表明细
        /// <summary>
        /// 添加销售或者支出统计表明细数据
        /// </summary>
        /// <param name="SaleOrSpendMod"></param>
        /// <returns></returns>
        public abstract int InsertFnSalesOrSpendStatistics(FnSalesOrSpendStatistics SaleOrSpendMod);
        /// <summary>
        /// 更新销售和支出统计表明细数据
        /// </summary>
        /// <param name="SaleOrSpendMod"></param>
        public abstract void UpdateFnSalesOrSpendStatistics(FnSalesOrSpendStatistics SaleOrSpendMod);
        /// <summary>
        /// 删除销售支出统计表明细数据
        /// </summary>
        /// <param name="SysNo"></param>
        public abstract void DeleteFnSalesOrSpendStatistics(int SysNo);
        /// <summary>
        /// 获取销售支出统计表明细内容
        /// </summary>
        /// <param name="PSysNo"></param>
        /// <returns></returns>
        public abstract List<CBFnSalesOrSpendStatistics> GetCBFnSalesOrSpendStatistics(int PSysNo);
        #endregion

        #region 财务统计表类型
        /// <summary>
        /// 添加统计表类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public abstract int InsertFnStatisticsType(FnStatisticsType type);
        /// <summary>
        /// 更新统计表类型
        /// </summary>
        /// <param name="type"></param>
        public abstract void UpdateFnStatisticsType(FnStatisticsType type);
        /// <summary>
        /// 删除统计表数据
        /// </summary>
        /// <param name="SysNo"></param>
        public abstract void DeleteFnStatisticsType(int SysNo);
        /// <summary>
        /// 获取统计表类型列表
        /// </summary>
        /// <returns></returns>
        public abstract List<FnStatisticsType> GetFnStatisticsTypeList();
        #endregion



    }
}
