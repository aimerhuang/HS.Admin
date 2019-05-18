
namespace Hyt.BLL.CrossBorderLogistics
{
    public class CrossBorderLogisticsOrderBo : BOBase<CrossBorderLogisticsOrderBo>
    {
        public Model.CrossBorderLogisticsOrder InsertEntity(Model.CrossBorderLogisticsOrder entity)
        {
            return DataAccess.CrossBorderLogisticsOrder.ICrossBorderLogisticsOrderDao.Instance.InsertEntity(entity);
        }

        /// <summary>
        /// 获取实体详情
        /// </summary>
        /// <param name="SoOrderSysNo">订单编号</param>
        /// <returns>物流详情</returns>
        ///<remarks>2016-04-13 王耀发 创建</remarks>
        public Model.CrossBorderLogisticsOrder GetEntityByOrderSysNo(int SoOrderSysNo)
        {
            return DataAccess.CrossBorderLogisticsOrder.ICrossBorderLogisticsOrderDao.Instance.GetEntityByOrderSysNo(SoOrderSysNo);
        }

        /// <summary>
        /// 根据订单编号删除物流返回订单记录
        /// </summary>
        /// <param name="orderSysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-07-05 陈海裕 创建</remarks>
        public bool DeleteByOrderSysNo(int orderSysNo)
        {
            return DataAccess.CrossBorderLogisticsOrder.ICrossBorderLogisticsOrderDao.Instance.DeleteByOrderSysNo(orderSysNo) > 0;
        }

        public System.Collections.Generic.List<Model.CrossBorderLogisticsOrder> GetEntityByOrderSysNoList(int orderSysNo)
        {
            return DataAccess.CrossBorderLogisticsOrder.ICrossBorderLogisticsOrderDao.Instance.GetEntityByOrderSysNoList(orderSysNo);
        }

        public System.Collections.Generic.List<Model.CrossBorderLogisticsOrder> GetEntityListByLogisticsOrderId(string txt)
        {
            return DataAccess.CrossBorderLogisticsOrder.ICrossBorderLogisticsOrderDao.Instance.GetEntityListByLogisticsOrderId(txt);
        }

        public void DeleteBySysNo(int SysNo)
        {
            DataAccess.CrossBorderLogisticsOrder.ICrossBorderLogisticsOrderDao.Instance.DeleteBySysNo(SysNo);
        }
    }
}
