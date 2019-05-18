using Hyt.DataAccess.Base;

namespace Hyt.DataAccess.CrossBorderLogisticsOrder
{
    /// <summary>
    /// 推送物流返回订单
    /// </summary>
    /// <remarks>2016-04-14 陈海裕 创建</remarks>
    public abstract class ICrossBorderLogisticsOrderDao : DaoBase<ICrossBorderLogisticsOrderDao>
    {
        public abstract Model.CrossBorderLogisticsOrder InsertEntity(Model.CrossBorderLogisticsOrder entity);

        public abstract Model.CrossBorderLogisticsOrder GetEntityByOrderSysNo(int SoOrderSysNo);

        /// <summary>
        /// 根据订单编号删除物流返回订单记录
        /// </summary>
        /// <param name="orderSysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-07-05 陈海裕 创建</remarks>
        public abstract int DeleteByOrderSysNo(int orderSysNo);

        public abstract System.Collections.Generic.List<Model.CrossBorderLogisticsOrder> GetEntityByOrderSysNoList(int orderSysNo);

        public abstract System.Collections.Generic.List<Model.CrossBorderLogisticsOrder> GetEntityListByLogisticsOrderId(string txt);

        public abstract void DeleteBySysNo(int SysNo);
    }
}
