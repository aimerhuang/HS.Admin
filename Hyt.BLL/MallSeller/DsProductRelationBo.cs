using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.MallSeller;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;

namespace Hyt.BLL.MallSeller
{
    public class DsProductRelationBo : BOBase<DsProductRelationBo>
    {
        /// <summary>
        /// 分页查询升舱商品关系维护关联
        /// </summary>
        /// <param name="filter">升舱订单查询参数</param>
        /// <returns>分页查询结果</returns>
        /// <remarks>2014-10-10 谭显锋 创建</remarks>
        public Pager<CBDsProductRelation> Query(ParaDsProductRelationFilter filter)
        {
            return IDsProductRelationDao.Instance.Query(filter);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>返回result</returns>
        /// <remarks>2014-10-10  谭显锋 创建</remarks>
        public Result Delete(int sysNo)
        {
            var res = new Result();
            var r = IDsProductRelationDao.Instance.Delete(sysNo);
            if (r > 0) res.Status = true;
            return res;
        }
    }
}
