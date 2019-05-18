using Hyt.DataAccess.FinancialStatistics;
using Hyt.Model.FinancialStatistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.FinancialStatistics
{
    /// <summary>
    /// 添加统计报表图片
    /// </summary>
    /// <remarks>
    /// 2016-03-22 杨云奕 添加
    /// </remarks>
    public class FnStatisticsBo : BOBase<FnStatisticsBo>
    {
        /// <summary>
        /// 添加统计报表
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public  int InsertFnStatistics(Model.FinancialStatistics.FnStatistics mod)
        {
            return IFnStatisticsDao.Instance.InsertFnStatistics(mod);
        }
        /// <summary>
        ///  更新统计报表
        /// </summary>
        /// <param name="mod"></param>
        public  void UpdateFnStatistics(Model.FinancialStatistics.FnStatistics mod)
        {
            IFnStatisticsDao.Instance.UpdateFnStatistics(mod);
        }
        /// <summary>
        /// 删除统计报表
        /// </summary>
        /// <param name="SysNo"></param>
        public  void DeleteFnStatistics(int SysNo)
        {
            IFnStatisticsDao.Instance.DeleteFnStatistics(SysNo);
        }
        /// <summary>
        /// 获取统计报表列表
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public  List<Model.FinancialStatistics.CBFnStatistics> GetCBFnStatisticList(int year)
        {
            return IFnStatisticsDao.Instance.GetCBFnStatisticList(year);
        }
        /// <summary>
        /// 获取统计报表实体数据
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public  Model.FinancialStatistics.CBFnStatistics GetCBFnStatisticMod(int SysNo)
        {
            return IFnStatisticsDao.Instance.GetCBFnStatisticMod(SysNo);
        }
        /// <summary>
        /// 获取统计报表实体数据
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public Model.FinancialStatistics.CBFnStatistics GetCBFnStatisticMod(DateTime dateTime)
        {
            return IFnStatisticsDao.Instance.GetCBFnStatisticMod(dateTime);
        }
        /// <summary>
        /// 获取订单数据
        /// </summary>
        /// <param name="type"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public List<StatisticsDataMod> GetStatisticsDataMod(string type, int year, int month)
        {
            return IFnStatisticsDao.Instance.GetStatisticsDataMod(type,year,month);
        }

        /// <summary>
        /// 添加统计报表明细数据
        /// </summary>
        /// <param name="SaleOrSpendMod"></param>
        /// <returns></returns>
        public  int InsertFnSalesOrSpendStatistics(Model.FinancialStatistics.FnSalesOrSpendStatistics SaleOrSpendMod)
        {
            return IFnStatisticsDao.Instance.InsertFnSalesOrSpendStatistics(SaleOrSpendMod);
        }
        /// <summary>
        /// 更新统计报表明细
        /// </summary>
        /// <param name="SaleOrSpendMod"></param>
        public  void UpdateFnSalesOrSpendStatistics(Model.FinancialStatistics.FnSalesOrSpendStatistics SaleOrSpendMod)
        {
            IFnStatisticsDao.Instance.UpdateFnSalesOrSpendStatistics(SaleOrSpendMod);
        }
        /// <summary>
        /// 删除统计报表明细数据
        /// </summary>
        /// <param name="SysNo"></param>
        public  void DeleteFnSalesOrSpendStatistics(int SysNo)
        {
            IFnStatisticsDao.Instance.DeleteFnSalesOrSpendStatistics(SysNo);
        }
        /// <summary>
        /// 获取统计报表明细数据
        /// </summary>
        /// <param name="PSysNo"></param>
        /// <returns></returns>
        public  List<Model.FinancialStatistics.CBFnSalesOrSpendStatistics> GetCBFnSalesOrSpendStatistics(int PSysNo)
        {
            return IFnStatisticsDao.Instance.GetCBFnSalesOrSpendStatistics(PSysNo);
        }
        /// <summary>
        /// 添加统计报表类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public  int InsertFnStatisticsType(Model.FinancialStatistics.FnStatisticsType type)
        {
            return IFnStatisticsDao.Instance.InsertFnStatisticsType(type);
        }
        /// <summary>
        /// 更新统计报表类型
        /// </summary>
        /// <param name="type"></param>
        public  void UpdateFnStatisticsType(Model.FinancialStatistics.FnStatisticsType type)
        {
            IFnStatisticsDao.Instance.UpdateFnStatisticsType(type);
        }
        /// <summary>
        /// 删除统计报表类型
        /// </summary>
        /// <param name="SysNo"></param>
        public  void DeleteFnStatisticsType(int SysNo)
        {
            IFnStatisticsDao.Instance.DeleteFnStatisticsType(SysNo);
        }
        /// <summary>
        /// 获取统计报表类型
        /// </summary>
        /// <returns></returns>
        public  List<Model.FinancialStatistics.FnStatisticsType> GetFnStatisticsTypeList()
        {
            return IFnStatisticsDao.Instance.GetFnStatisticsTypeList();
        }

        public Model.FinancialStatistics.AllStatistics GetAllStatistics(int year, int mouth)
        {
            return IFnStatisticsDao.Instance.GetAllStatistics(year, mouth);
        }
    }
}
