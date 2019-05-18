using Hyt.DataAccess.CrossBorderLogisticsOrder;

namespace Hyt.DataAccess.Oracle.CrossBorderLogistics
{
    /// <summary>
    /// 推送物流返回订单
    /// </summary>
    /// <remarks>2016-04-14 陈海裕 创建</remarks>
    public class CrossBorderLogisticsOrderDaoImpl : ICrossBorderLogisticsOrderDao
    {
        public override Model.CrossBorderLogisticsOrder InsertEntity(Model.CrossBorderLogisticsOrder entity)
        {
            entity.SysNo = Context.Insert("CrossBorderLogisticsOrder", entity)
                                  .AutoMap(o => o.SysNo)
                                  .ExecuteReturnLastId<int>("SysNo");

            return entity;
        }

        /// <summary>
        /// 获取实体详情
        /// </summary>
        /// <param name="SoOrderSysNo">订单编号</param>
        /// <returns>物流详情</returns>
        ///<remarks>2016-04-13 王耀发 创建</remarks>
        public override Model.CrossBorderLogisticsOrder GetEntityByOrderSysNo(int SoOrderSysNo)
        {
            return Context.Sql("select * from CrossBorderLogisticsOrder where SoOrderSysNo=@0", SoOrderSysNo).QuerySingle<Model.CrossBorderLogisticsOrder>();
        }

        /// <summary>
        /// 根据订单编号删除物流返回订单记录
        /// </summary>
        /// <param name="orderSysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-07-05 陈海裕 创建</remarks>
        public override int DeleteByOrderSysNo(int orderSysNo)
        {
            return Context.Delete("CrossBorderLogisticsOrder").Where("SoOrderSysNo", orderSysNo).Execute();
        }

        public override System.Collections.Generic.List<Model.CrossBorderLogisticsOrder> GetEntityByOrderSysNoList(int orderSysNo)
        {
            return Context.Sql("select * from CrossBorderLogisticsOrder where SoOrderSysNo=@0", orderSysNo).QueryMany<Model.CrossBorderLogisticsOrder>();
        }

        public override System.Collections.Generic.List<Model.CrossBorderLogisticsOrder> GetEntityListByLogisticsOrderId(string txt)
        {
            return Context.Sql("select * from CrossBorderLogisticsOrder where LogisticsOrderId  like '%"+txt+"%'").QueryMany<Model.CrossBorderLogisticsOrder>();
        }

        public override void DeleteBySysNo(int SysNo)
        {
             Context.Delete("CrossBorderLogisticsOrder").Where("SysNo", SysNo).Execute();
        }
    }
}
