using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.MallSeller;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Infrastructure.Memory;

namespace Hyt.BLL.MallSeller
{
    public class DsEasBo : BOBase<DsEasBo>
    {
        /// <summary>
        /// 分页查询分销商EAS关联
        /// </summary>
        /// <param name="filter">升舱订单查询参数</param>
        /// <returns></returns>
        /// <remarks>2013-10-10 黄志勇 创建</remarks>
        public Pager<CBDsEasAssociation> Query(ParaDsEasFilter filter)
        {
            return IDsEasDao.Instance.Query(filter);
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>返回新的编号</returns>
        /// <remarks>2013-10-10 黄志勇 创建</remarks>
        public int Insert(DsEasAssociation entity)
        {
            return IDsEasDao.Instance.Insert(entity);
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns></returns>
        /// <remarks>2013-09-25  黄志勇 创建</remarks>
        public void Update(DsEasAssociation entity)
        {
            IDsEasDao.Instance.Update(entity);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-10-11  黄志勇 创建</remarks>
        public int Delete(int sysNo)
        {
            return IDsEasDao.Instance.Delete(sysNo);
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-10-10 黄志勇 创建</remarks>
        public DsEasAssociation GetEntity(int sysNo)
        {
            return IDsEasDao.Instance.GetEntity(sysNo);
        }

        /// <summary>
        /// 根据分销商商城编号获取
        /// </summary>
        /// <param name="dealerMallSysNo">分销商商城系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-10-10 黄志勇 创建</remarks>
        public DsEasAssociation Get(int dealerMallSysNo)
        {
            return IDsEasDao.Instance.Get(dealerMallSysNo);
        }

        /// <summary>
        /// 根据分销商商城编号获取
        /// </summary>
        /// <param name="dealerMallSysNo">分销商商城系统编号</param>
        /// <param name="sellerNick">昵称</param>
        /// <returns></returns>
        /// <remarks>2013-10-18 黄志勇 创建</remarks>
        public DsEasAssociation Get(int dealerMallSysNo, string sellerNick)
        {
            return IDsEasDao.Instance.Get(dealerMallSysNo, sellerNick);
        }

        /// <summary>
        /// 获取全部商城类型
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        /// <remarks>2013-10-10 黄志勇 创建</remarks>
        public List<DsMallType> GetAllMallType()
        {
            return IDsEasDao.Instance.GetAllMallType();
        }

        /// <summary>
        /// 获取全部商城
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        /// <remarks>2013-10-10 黄志勇 创建</remarks>
        public List<DsDealerMall> GetAllMall()
        {
            var allMall = MemoryProvider.Default.Get("AllMall") as List<DsDealerMall>;
            if (allMall != null && allMall.Count > 0) return allMall;
            allMall = IDsEasDao.Instance.GetAllMall();
            MemoryProvider.Default.Set("AllMall", allMall, 10);
            return allMall;
        }

        /// <summary>
        /// 获取新增商城
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        /// <remarks>2013-10-10 黄志勇 创建</remarks>
        public List<DsDealerMall> GetNewMall()
        {
            return IDsEasDao.Instance.GetNewMall();
        }

        /// <summary>
        /// 分销升舱成功后续检查
        /// </summary>
        /// <param name="entity">分销商EAS关联</param>
        /// <returns></returns>
        /// <remarks>2013-10-10 黄志勇 创建</remarks>
        public void CheckEas(DsEasAssociation entity)
        {
            if (entity == null) throw new ArgumentException("分销商EAS关联实体不能为空");
            var model = Get(entity.DealerMallSysNo);
            if (model == null)
            {
                //entity.Code = string.Empty;
                //entity.Status = 1;
                Insert(entity);
            }
        }

        /// <summary>
        /// 升舱订单审核
        /// </summary>
        /// <param name="dealerMallSysNo">分销商商城系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-10-10 黄志勇 创建</remarks>
        public string AuditOrder(int dealerMallSysNo)
        {
            var result = string.Empty;
            var entity = Get(dealerMallSysNo);
            if (entity == null || string.IsNullOrEmpty(entity.Code))
                result = "商品编号未维护，审核不通过";
            return result;
        }

        /// <summary>
        /// 获取全部分销商EAS关联商城系统编号列表
        /// </summary>
        /// <returns></returns>
        /// <remarks>2013-11-1 黄志勇 创建</remarks>
        public List<int> GetAllDsEasAssociation()
        {
            var list = new List<int>();
            list.AddRange(IDsEasDao.Instance.GetAllDsEasAssociation());
            return list;
        }
    }
}
