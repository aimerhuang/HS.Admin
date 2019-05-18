using System.Collections.Generic;
using System.Linq;
using Hyt.BLL.Log;
using Hyt.BLL.Product;
using Hyt.DataAccess.Distribution;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.BLL.Distribution
{
    /// <summary>
    /// 分销商产品特殊价格
    /// </summary>
    /// <remarks>
    /// 2013-08-19 周唐炬 创建
    /// </remarks>
    public class DsLevelPriceBo : BOBase<DsLevelPriceBo>
    {

        /// <summary>
        /// 根据商品系统编号获取商品的等级价格
        /// </summary>
        /// <param name="productSysNo">商品编号</param>
        /// <returns></returns>
        /// <remarks>2013-09-13 周瑜 创建</remarks>
        public IList<CBPdPrice> GetLevelPriceByProdouctSysNo(int productSysNo)
        {
            //查询分销商等级
            var dsdealerlevel = GetDsDealerLevel();

            //查询该商品的等级价格
            var priceList = PdPriceBo.Instance.GetProductPrice(productSysNo);
            priceList = priceList.Where(m => m.PriceSource == ProductStatus.产品价格来源.分销商等级价.GetHashCode()).ToList();

            return (from level in dsdealerlevel
                    let price = priceList.FirstOrDefault(m => m.SourceSysNo == level.SysNo)
                    select new CBPdPrice
                        {
                            Price = price == null ? 0 : price.Price,
                            PriceName = level.LevelName,
                            PriceSource = ProductStatus.产品价格来源.分销商等级价.GetHashCode(),
                            SysNo = price == null ? 0 : price.SysNo,
                            SourceSysNo = level.SysNo
                        }).ToList();
        }

        /// <summary>
        /// 获取分销商等级
        /// </summary>
        /// <returns></returns>
        /// <remarks>2013-09-13 周瑜 创建</remarks>
        public IList<DsDealerLevel> GetDsDealerLevel()
        {
            return IDsLevelPriceDao.Instance.GetDsDealerLevel();
        }

        /// <summary>
        /// 获取价格等级修改记录列表(分页方法)
        /// </summary>
        /// <param name="pageIndex">起始页码</param>
        /// <param name="status">审批状态</param>
        /// <param name="sysno">商品编号</param>
        /// <param name="erpCode">商品商品编号</param>
        /// <returns>价格等级修改记录列表</returns>
        /// <remarks>2013-09-13 周瑜 创建</remarks>
        /// <remarks>2013-11-04 周唐炬 重构</remarks>
        public PagedList<CBPdPriceHistory> GetPriceHistorieList(int? pageIndex, int? status, int? sysno, string erpCode)
        {
            var model = new PagedList<CBPdPriceHistory>
                {
                    CurrentPageIndex = pageIndex ?? 1
                };
            int count;
            var list = IDsLevelPriceDao.Instance.GetPriceHistorieList(model.CurrentPageIndex, model.PageSize, status, sysno, erpCode, out count);
            model.TData = list;
            model.TotalItemCount = count;
            return model;
        }

        /// <summary>
        /// 根据关系码获取价格等级修改记录列表
        /// </summary>
        /// <param name="relationCode">等级价格记录关系码</param>
        /// <returns>价格等级修改记录列表</returns>
        /// <remarks>2013-09-13 周瑜 创建</remarks>
        public IList<CBPdPriceHistory> GetPriceHistorieListByRelationCode(string relationCode)
        {
            IList<CBPdPriceHistory> model = new List<CBPdPriceHistory>();

            var list = IDsLevelPriceDao.Instance.GetPriceHistorieListByRelationCode(relationCode);
            if (list != null && list.Any())
            {
                model = list;
            }
            return model;
        }

        /// <summary>
        /// 审核商品调价
        /// </summary>
        /// <param name="relationCode">关系码</param>
        /// <param name="opinion">意见</param>
        /// <param name="status">状态</param>
        /// <param name="auditor">审批人</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-09-13 周瑜 创建</remarks>
        public int Update(string relationCode, string opinion, int status, int auditor)
        {
            SysLog.Instance.Info(LogStatus.系统日志来源.后台, "审核商品调价:" + relationCode, LogStatus.系统日志目标类型.分销商特殊价格, 0);
            return IDsLevelPriceDao.Instance.Update(relationCode, opinion, status, auditor);
        }
    }
}
