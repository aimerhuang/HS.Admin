
using Hyt.DataAccess.Distribution;
using Hyt.Model;
using Hyt.Model.Parameter;
using System.Collections.Generic;

namespace Hyt.BLL.Distribution
{
    public class DsDealerWharehouseBo : BOBase<DsDealerWharehouseBo>
    {
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>返回新的编号</returns>
        /// <remarks>2014-10-15 余勇 创建</remarks>
        public int Insert(DsDealerWharehouse entity)
        {
            var dealerWharehouse = GetByWarehousSysNo(entity.WarehouseSysNo);
            if (dealerWharehouse == null)
            {
                return IDsDealerWharehouse.Instance.Insert(entity);
            }
            return 0;
        }

        /// <summary>
        /// 更新数据(先删后增)
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns></returns>
        /// <remarks>
        /// 2014-10-15 余勇 创建
        /// 2016-4-6 杨浩 修改总部经销商系统编号为0不能修改的bug
        /// </remarks>
        public void Update(DsDealerWharehouse entity)
        {
            var dealerWharehouse = DsDealerWharehouseBo.Instance.GetByWarehousSysNo(entity.WarehouseSysNo);
            if (dealerWharehouse != null)
            {
               Delete(dealerWharehouse.SysNo);
            }
            if (entity.DealerSysNo >= 0)
            {
               Insert(entity);
            }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns></returns>
        /// <remarks>2014-10-15 余勇 创建</remarks>
        public int Delete(int sysNo)
        {
            return IDsDealerWharehouse.Instance.Delete(sysNo);
        }

        /// <summary>
        /// 取单条数据
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>model</returns>
        /// <remarks>2014-10-15 余勇 创建</remarks>
        public DsDealerWharehouse Get(int sysNo)
		{
			return IDsDealerWharehouse.Instance.Get(sysNo);
		}

        /// <summary>
        /// 通过仓库编号取单条数据
        /// </summary>
        /// <param name="warehousSysNo">仓库编号</param>
        /// <returns>model</returns>
        /// <remarks>2014-10-15 余勇 创建</remarks>
        public DsDealerWharehouse GetByWarehousSysNo(int warehousSysNo)
        {
            return IDsDealerWharehouse.Instance.GetByWarehousSysNo(warehousSysNo);
        }

        public DsDealerWharehouse GetByDsUserSysNo(int UserSysNo)
        {
            return IDsDealerWharehouse.Instance.GetByDsUserSysNo(UserSysNo);
        }

        public List<CBDsDealerWharehouse> GetAllDealerWarehouse()
        {
            return IDsDealerWharehouse.Instance.GetAllDealerWarehouse();
        }
    }
}

