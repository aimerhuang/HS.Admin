using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.ExpressList;
using Hyt.Model.ExpressList;
using Hyt.Model;

namespace Hyt.BLL.ExpressList
{
    /// <summary>
    /// 快递单
    /// </summary>
    public class ExpressListBLL : BOBase<ExpressListBLL>
    {
        /// <summary>
        /// 判断是否已经包裹
        /// </summary>
        /// <param name="WhStockInId">出库单ID</param>
        /// <returns>true:已包裹 </returns>
        /// <remarks>2017-11-24 廖移凤 创建</remarks>
        public bool GetExpressList(int WhStockInId)
        {
            return IExpressListDao.Instance.GetExpressList(WhStockInId) > 0;
        }
        /// <summary>
        /// 查询快递单号
        /// </summary>
        /// <param name="WhStockInId">出库单号</param>
        /// <returns></returns>
        ///  <remarks> 2017-12-01 廖移凤</remarks>
        public  int GetKuaiDiNo(int WhStockInId)
        {
                return IExpressListDao.Instance.GetKuaiDiNo(WhStockInId);
        }


        /// <summary>
        /// 保存快递单
        /// </summary>
        /// <param name="el">快递单实体 </param>
        /// <returns>true:已保存</returns>
        /// <remarks>2017-11-24 廖移凤 创建</remarks>
        public bool AddExpressList(ExpressLists el)
        {

            return IExpressListDao.Instance.AddExpressList(el) > 0;
        }


        /// <summary>
        /// 保存快递100接口 返回的数据
        /// </summary>
        /// <param name="kn"></param>
        /// <returns></returns>
        ///  <remarks>2017-11-28 廖移凤</remarks>
        public int AddKdOrderNums(KdOrderNums kn)
        {
            return IExpressListDao.Instance.AddKdOrderNums(kn);
        }


        /// <summary>
        /// 查询接口参数
        /// </summary>
        /// <param name="StockOutSysNo"></param>
        /// <returns></returns>
        public KdOrderParam GetKdOrderParam(int StockOutSysNos)
        {

            return IExpressListDao.Instance.GetKdOrderParam(StockOutSysNos);

        }
        /// <smmary>
        /// 查询发货人
        /// </summary>
        /// <param name="StockOutSysNo"></param>
        /// <returns></returns>
        public RecMan GetRecMan(int StockOutSysNo)
        {

            return IExpressListDao.Instance.GetRecMan(StockOutSysNo);

        }
        /// <summary>
        /// 查询收货人
        /// </summary>
        /// <param name="StockOutSysNo"></param>
        /// <returns></returns>
        public RecMan GetSRecMan(int StockOutSysNo)
        {
            return IExpressListDao.Instance.GetSRecMan(StockOutSysNo);
        }

        /// <summary>
        /// 快递单查询
        /// </summary>
        /// <returns></returns>
        public  List<KuaiDiNumQuery> GetKuaiDiNumQuery() {

            return IExpressListDao.Instance.GetKuaiDiNumQuery();
        }
        /// <summary>
        /// 删除快递单
        /// </summary>
        /// <returns></returns>
        public int DeleteKuaiDiNum(int KuaiDiNum)
        {

            return IExpressListDao.Instance.DeleteKuaiDiNum(KuaiDiNum);
        }

        /// <summary>
        /// 搜索快递单
        /// </summary>
        /// <returns></returns>
        public KuaiDiNumQuery SelectKuaiDiNum(int KuaiDiNum)
        {
            return IExpressListDao.Instance.SelectKuaiDiNum(KuaiDiNum); ;
        }


        /// <summary>
        /// 分页查询快递单
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        public  Pager<KuaiDiNumQuery> GetPage(Pager<KuaiDiNumQuery> pager)
        {
            return IExpressListDao.Instance.GetPage(pager);
        }
    }
}
