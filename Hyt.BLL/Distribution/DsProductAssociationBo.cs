using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using Hyt.DataAccess.MallSeller;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.DataAccess.Distribution;
using Hyt.BLL.Sys;

namespace Hyt.BLL.Distribution
{
    public class DsProductAssociationBo : BOBase<DsProductAssociationBo>
    {
        /// <summary>
        /// 获取商品详细信息列表
        /// </summary>
        /// <param name="pager">商品详细信息查询列表</param>
        /// <param name="dealerMallSysNo">分销商商城系统编号</param>
        /// <param name="condition">动态条件，CBPdProductDetail里不包含的条件</param>
        /// <returns></returns>
        /// <remarks>2015-12-10 王耀发 创建</remarks>
        public void GetDealerMallProductList(ref Pager<CBPdProductDetail> pager, int dealerMallSysNo, ParaProductFilter condition)
        {
            IDsProductAssociationDao.Instance.GetDealerMallProductList(ref pager,dealerMallSysNo, condition);
        }
        /// <summary>
        /// 插入记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Insert(DsProductAssociation model)
        {
            var SysNo = IDsProductAssociationDao.Instance.Insert(model);
            return SysNo;
        }

        /// <summary>
        /// 更新商品状态值
        /// </summary>
        /// <param name="SysNo">商品关系编号</param>
        /// <param name="Status">状态值</param>
        /// <returns></returns>
        /// <remarks>2015-12-10 王耀发  创建</remarks>
        public void UpdateDealerMallProductStatus(int SysNo, int Status)
        {
            Hyt.DataAccess.Distribution.IDsProductAssociationDao.Instance.UpdateDealerMallProductStatus(SysNo, Status);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-10-11  黄志勇 创建</remarks>
        public void Delete(int sysNo)
        {
            Hyt.DataAccess.Distribution.IDsProductAssociationDao.Instance.Delete(sysNo);
        }
    }
}
