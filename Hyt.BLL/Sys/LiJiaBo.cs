using System.Collections.Generic;
using Hyt.DataAccess.Sys;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.Generated;

namespace Hyt.BLL.Sys
{
    /// <summary>
    /// 利嘉同步日志
    /// </summary>
    /// <remarks>2017-11-10 罗熙 创建 </remarks>
    public class LiJiaBo : BOBase<LiJiaBo>
    {
        /// <summary>
        /// 添加LiJia同步日志
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns></returns>
        /// <remarks>2013-10-22 黄志勇 创建</remarks>
        public int LiJiaSyncLogCreate(LiJiaSyncLog model)
        {
            return ILiJiaDao.Instance.LiJiaSyncLogCreate(model);
        }

        /// <summary>
        /// LiJia同步日志查询
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns>分页查询</returns>
        /// <remarks>2013-10-22 黄志勇 创建</remarks>
        public Pager<CBLiJiaSyncLog> GetList(ParaLiJiaSyncLogFilter filter)
        {
            return ILiJiaDao.Instance.GetList(filter);
        }

        /// <summary>
        /// 获取LiJia同步日志列表
        /// </summary>
        /// <returns>日志列表</returns>
        /// <remarks>2014-4-9 杨浩 创建</remarks>
        public List<LiJiaSyncLog> GetSyncList(int count, bool isFailure = true)
        {
            var list = ILiJiaDao.Instance.GetSyncWaitList(count);
            if ((list == null || list.Count == 0) && isFailure)
                list = ILiJiaDao.Instance.GetSyncFailureList(count);
            return list;
        }
        /// <summary>
        /// 同步销售出库到到Erp
        /// </summary>
        /// <returns></returns>
        /// <remarks>2017-04-25 杨浩 创建</remarks>
        public Result SynchronizeOrderToErp()
        {
            var result = new Result<dynamic>
            {
                Message = "同步成功！",
                Status = true
            };

            var client = Extra.Erp.Kis.KisProviderFactory.CreateProvider();

            var list = Hyt.BLL.Sys.LiJiaBo.Instance.GetSyncList(9999, false);






            int index = 0;
            for (; index < list.Count; index++)
            {
                var _result = client.Resynchronization(list[index].SysNo);
                //StatusCode == "9999"  标示已经导入过了
                if (result.Status || result.StatusCode.ToString() == "9999")
                {
                    continue;
                }
                else
                {
                    if (result.StatusCode.ToString() == "9998")//传递中
                        index = index - 1;
                    index = list.FindLastIndex(x => x.FlowIdentify == list[index].FlowIdentify);
                }
            }

            var failureList = ILiJiaDao.Instance.GetSyncFailureList(9999);
            foreach (var item in failureList)
            {
                client.Resynchronization(item.SysNo);
            }

            if (list.Count <= 0)
            {
                result.Status = false;
                result.Message = "没有可同步的数据";
            }

            return result;
        }

        /// <summary>
        /// 获取LiJia同步日志
        /// </summary>
        /// <param name="sysNos"></param>
        /// <returns></returns>
        /// <remarks>2014-5-4 杨浩 创建</remarks>
        public IList<LiJiaSyncLog> GetList(int[] sysNos)
        {
            return ILiJiaDao.Instance.GetList(sysNos == null ? "" : string.Join(",", sysNos));
        }
        /// <summary>
        /// 是否存在未处理的关联单据
        /// </summary>
        /// <returns></returns>
        /// <remarks>2014-4-9 杨浩 创建</remarks>
        public bool IsRelate(int sysNo)
        {
            var item = GetEntity(sysNo);
            var list = ILiJiaDao.Instance.GetRelateList(item.FlowIdentify);
            //判断此同步日志是否处于待同步队列的头部
            if (list.FindIndex(x => x.SysNo == sysNo) == 0)
                return false;
            else
                return true;
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-10-23 黄志勇 创建</remarks>
        public LiJiaSyncLog GetEntity(int sysNo)
        {
            return ILiJiaDao.Instance.GetEntity(sysNo);
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        /// <remarks>2013-10-23 黄志勇 创建</remarks>
        public int Update(LiJiaSyncLog entity)
        {
            return ILiJiaDao.Instance.Update(entity);
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="obj">对象列表</param>
        /// <param name="size">每页条数</param>
        /// <param name="page">当前页</param>
        /// <returns>分页数据</returns>
        /// <remarks>2013-8-13 黄志勇 添加</remarks>
        public Pager<object> GetLiJiaData(object obj, int size, int page)
        {
            return MemoryBo.Instance.GetPagerDataRow(obj, string.Empty, string.Empty, size, page);
        }

        /// <summary>
        /// 获取Kis单据编号
        /// </summary>
        /// <param name="flowType">流程类型</param>
        /// <param name="flowIdentify">流程编号</param>
        /// <returns></returns>
        /// <remarks>2017-05-05 杨浩 创建</remarks>
        public string GetVoucherNo(int flowType, string flowIdentify)
        {
            string voucherNo = ILiJiaDao.Instance.GetVoucherNo(flowType.ToString(), flowIdentify);
            if (string.IsNullOrEmpty(voucherNo))
                return "";
            return voucherNo;
        }
        /// <summary>
        /// 获取没有同步金蝶的出库单
        /// </summary>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <returns></returns>
        /// <remarks>2017-09-28 杨浩 创建</remarks>
        public IList<WhStockOut> GetNoSyncStockOutList(int warehouseSysNo)
        {
            return ILiJiaDao.Instance.GetNoSyncStockOutList(warehouseSysNo);
        }

    }
}
