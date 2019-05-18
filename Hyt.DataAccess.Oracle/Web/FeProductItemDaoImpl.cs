using System.Collections.Generic;
using Hyt.DataAccess.Web;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Oracle.Web
{
    /// <summary>
    /// 商品组
    /// </summary>
    /// <remarks>2013-08-06 黄波 创建</remarks>
    public class FeProductItemDaoImpl : IFeProductItemDao
    {
        /// <summary>
        /// 根据商品组代码获取商品
        /// 返回部分列数据
        /// </summary>
        /// <param name="platformType">平台类型</param>
        /// <param name="groupCode">组代码</param>
        /// <returns>商品列表</returns>
        /// <remarks>2013-08-06 黄波 创建</remarks>
        public override IList<Model.FeProductItem> GetFeProductItems(ForeStatus.商品组平台类型 platformType, string groupCode)
        {
            string sql = @"
                                  select
                                         fi.ProductSysNo,fi.BeginDate,fi.EndDate,fi.DisplayOrder,fi.DispalySymbol,fi.DealerSysNo,fi.Status 
                                   from
                                        FeProductItem fi 
                                   where
                                        fi.GroupSysNo=(select fg.sysno from FeProductGroup fg where fg.code=@Code and fg.Status=@FGStatus and fg.PlatformType=@PlatformType )
                                    and
                                        fi.Status=@Status
                                    order by 
                                        fi.DisplayOrder desc
                                    ";
            return Context.Sql(sql)
                .Parameter("Code", groupCode)
                .Parameter("FGStatus", (int)ForeStatus.商品组状态.启用)
                .Parameter("PlatformType", (int)platformType)
                .Parameter("Status", (int)ForeStatus.商品项状态.已审)
                .QueryMany<Model.FeProductItem>();
        }

        /// <summary>
        /// 根据商品组系统编号获取商品
        /// 返回部分列数据
        /// </summary>
        /// <param name="groupSysNo">组系统编号</param>
        /// <returns>商品列表</returns>
        /// <remarks>2013-08-21 周瑜 创建</remarks>
        public override IList<Model.CBFeProductItem> GetFeProductItems(int groupSysNo)
        {
            const string sql = @"select fi.*,a.productname,a.productimage from FeProductItem fi 
inner join pdproduct a on fi.productsysno = a.sysno
where fi.GroupSysNo= @GroupSysNo and fi.Status=@Status
order by fi.DisplayOrder desc";
            return Context.Sql(sql)
                .Parameter("GroupSysNo", groupSysNo)
                .Parameter("Status", (int)ForeStatus.商品项状态.已审)
                .QueryMany<Model.CBFeProductItem>();
        }
    }
}
