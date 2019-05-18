using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using System.Transactions;
using Hyt.BLL.Product;
using Hyt.DataAccess.MallSeller;
using Hyt.DataAccess.Sys;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.DataAccess.Distribution;

namespace Hyt.BLL.Distribution
{
    /// <summary>
    /// 禁止升舱管理业务层
    /// </summary>
    /// <remarks>
    /// 2014-03-21 余勇 创建
    /// </remarks>
    public class DsForbidProductBo : BOBase<DsForbidProductBo>
    {
        #region 操作

        ///  <summary>
        /// 创建禁止升舱商品
        ///  </summary>
        ///  <param name="sysNoList">商品编号数组</param>
        /// <param name="userSysNo">操作人</param>
        /// <returns>执行结果</returns>
        ///  <remarks>2013-06-21 余勇 创建</remarks>
        public Result Create(int[] sysNoList,int userSysNo)
        {
            var res = new Result();

            
                if (sysNoList != null)
                {
                    foreach (var sysNo in sysNoList)
                    {
                       var dsForbidProduct = IDsForbidProductDao.Instance.GetByProductSysNo(sysNo);
                        if (dsForbidProduct == null)
                        {
                            var erpCode = PdProductBo.Instance.GetProductErpCode(sysNo);
                            var r = IDsForbidProductDao.Instance.Create(new DsForbidProduct
                                {
                                    CreatedBy = userSysNo,
                                    CreatedDate = DateTime.Now,
                                    ProductErpCode = erpCode,
                                    ProductSysNo = sysNo
                                });
                        }
                    }
                }
                
                res.Status = true;
            
            return res;
        }


        /// <summary>
        /// 通过SysNo删除该记录
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>执行结果</returns>
        /// <remarks>2013-06-19 余勇 创建</remarks>
        public Result Delete(int sysNo)
        {
            var res = new Result();
            var r= IDsForbidProductDao.Instance.Delete(sysNo);
            if (r > 0) res.Status = true;
            return res;
        }
        #endregion

        #region 查询
        /// <summary>
        /// 通过过滤条件获取禁止升舱商品列表
        /// </summary>
        /// <param name="product">商品名称或编号</param>
        /// <param name="currentPage">当前页号</param>
        /// <param name="pageSize">分页大小</param>
        /// <returns>分页列表</returns>
        /// <remarks>2013-08-05 余勇 创建</remarks>
        public Pager<DsForbidProduct> GetPagerList(string product, int currentPage, int pageSize)
        {
            return IDsForbidProductDao.Instance.Query(product, currentPage, pageSize);
        }
        #endregion
    }
}
