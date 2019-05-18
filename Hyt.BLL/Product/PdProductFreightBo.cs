using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.BLL.Authentication;
using Hyt.BLL.CRM;
using Hyt.Infrastructure.Caching;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.DataAccess.Basic;
using Hyt.DataAccess.Product;
using System.Data;
using Hyt.Model.LogisApp;
using Hyt.Model.Transfer;
using Hyt.Model.Parameter;
using System.Transactions;
using System.IO;
using Hyt.Model.WorkflowStatus;
using Hyt.BLL.Promotion;
using Hyt.Infrastructure.Memory;

namespace Hyt.BLL.Product
{
    /// <summary>
    /// 运费模板
    /// </summary>
    /// <remarks>
    /// 2015-08-06 王耀发 创建
    /// </remarks>
    public class PdProductFreightBo : BOBase<PdProductFreightBo>
    {

        #region 运费模板

        /// <summary>
        /// 保存运费模板
        /// </summary>
        /// <param name="model">运费模板</param>
        /// <param name="user">操作人</param>
        /// <returns></returns>
        /// <remarks>2015-08-06 王耀发 创建</remarks>
        public Result SavePdProductFreight(PdProductFreight model, SyUser user)
        {
            Result r = new Result()
            {
                Status = false
            };
            //新增数据
            model.CreatedDate = DateTime.Now;
            model.CreatedBy = user.SysNo;
            model.LastUpdateBy = user.SysNo;
            model.LastUpdateDate = DateTime.Now;
            r.StatusCode = Hyt.DataAccess.Product.IPdProductFreightDao.Instance.Insert(model);
            r.Status = true;
            return r;
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="PdProductSysNo">商品编号</param>
        /// <returns>数据实体</returns>
        /// <remarks>2015-08-06 王耀发 创建</remarks>
        public PdProductFreight GetEntityByPdProductSysNo(int PdProductSysNo)
        {
            return Hyt.DataAccess.Product.IPdProductFreightDao.Instance.GetEntityByPdProductSysNo(PdProductSysNo);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="PdProductSysNo">商品代码</param>
        /// <remarks>2015-08-14 王耀发 创建</remarks>
        public void DeleteByPdProductSysNo(int PdProductSysNo)
        {
            Hyt.DataAccess.Product.IPdProductFreightDao.Instance.DeleteByPdProductSysNo(PdProductSysNo);
        }

        #endregion
    }
}
