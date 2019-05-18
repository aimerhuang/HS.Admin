using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model.ExpressList;
using Hyt.DataAccess.Base;
using Hyt.Model;

namespace Hyt.DataAccess.ExpressList
{
    public abstract class IExpressListDao : DaoBase<IExpressListDao>
    {
        /// <summary>
        /// 判断是否已包裹

        /// </summary>
        /// <param name="WhStockInId">出库单</param>
        /// <returns></returns>
        ///  <remarks>2017-11-24 廖移凤</remarks>
        public abstract int GetExpressList(int WhStockInId);

        /// <summary>
        /// 查询快递单号
        /// </summary>
        /// <param name="WhStockInId">出库单号</param>
        /// <returns></returns>
        ///  <remarks> 2017-12-01 廖移凤</remarks>
        public abstract int GetKuaiDiNo(int WhStockInId);

        /// <summary>
        /// 保存快递单
        /// </summary>
        /// <param name="el">快递单实体 </param>
        /// <returns></returns>
        /// <remarks>2017-11-24 廖移凤</remarks>
        public abstract int AddExpressList(ExpressLists el);

        /// <summary>
        /// 保存快递100接口 返回的数据
        /// </summary>
        /// <param name="kn"></param>
        /// <returns></returns>
        ///  <remarks>2017-11-28 廖移凤</remarks>
        public abstract int AddKdOrderNums(KdOrderNums kn);
        /// <summary>
        /// 查询接口参数
        /// </summary>
        /// <param name="StockOutSysNo"></param>
        /// <returns></returns>
        public abstract KdOrderParam GetKdOrderParam(int StockOutSysNo);
        /// <summary>
        /// 查询发货人
        /// </summary>
        /// <param name="StockOutSysNo"></param>
        /// <returns></returns>
        public abstract RecMan GetRecMan(int StockOutSysNo);
        /// <summary>
        /// 查询收货人
        /// </summary>
        /// <param name="StockOutSysNo"></param>
        /// <returns></returns>
        public abstract RecMan GetSRecMan(int StockOutSysNo);
        /// <summary>
        /// 快递单查询
        /// </summary>
        /// <returns></returns>
        public abstract List<KuaiDiNumQuery> GetKuaiDiNumQuery();
        /// <summary>
        /// 删除快递单
        /// </summary>
        /// <returns></returns>
        public abstract int DeleteKuaiDiNum(int KuaiDiNum);

        /// <summary>
        /// 搜索快递单
        /// </summary>
        /// <returns></returns>
        public abstract KuaiDiNumQuery SelectKuaiDiNum(int KuaiDiNum);

        /// <summary>
        /// 分页查询快递单
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        public abstract Pager<KuaiDiNumQuery> GetPage(Pager<KuaiDiNumQuery> pager);
             
    }
}
