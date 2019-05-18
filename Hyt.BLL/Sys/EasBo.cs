using System.Collections.Generic;
using Hyt.DataAccess.Sys;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;

namespace Hyt.BLL.Sys
{
    /// <summary>
    /// Eas同步日志
    /// </summary>
    /// <remarks>
    /// 2013-10-22 黄志勇 创建
    /// </remarks>
    public class EasBo : BOBase<EasBo>
    {
        /// <summary>
        /// 获取指定接口流程编号同步次数
        /// </summary>
        /// <param name="interfaceType">接口类型</param>
        /// <param name="flowIdentify">流程编号</param>
        /// <returns></returns>
        /// <remarks>2017-1-4 杨浩 创建</remarks>
        public int GetEasSyncLogCount(int interfaceType, string flowIdentify)
        {
            return IEasDao.Instance.GetEasSyncLogCount(interfaceType, flowIdentify);
        }
        /// <summary>
        /// 添加Eas同步日志
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns></returns>
        /// <remarks>2013-10-22 黄志勇 创建</remarks>
        public int EasSyncLogCreate(EasSyncLog model)
        {
            return IEasDao.Instance.EasSyncLogCreate(model);
        }

        /// <summary>
        /// Eas同步日志查询
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns>分页查询</returns>
        /// <remarks>2013-10-22 黄志勇 创建</remarks>
        public Pager<CBEasSyncLog> GetList(ParaEasSyncLogFilter filter)
        {
            return IEasDao.Instance.GetList(filter);
        }

        /// <summary>
        /// 获取Eas同步日志列表
        /// </summary>
        /// <returns>日志列表</returns>
        /// <remarks>2014-4-9 杨浩 创建</remarks>
        public List<EasSyncLog> GetSyncList(int count,bool isFailure=true)
        {
            var list = IEasDao.Instance.GetSyncWaitList(count);
            if ((list == null || list.Count == 0) && isFailure)
                list = IEasDao.Instance.GetSyncFailureList(count);
            return list;
        }

        /// <summary>
        /// 同步销售出库到到信业Erp
        /// </summary>
        /// <returns></returns>
        /// <remarks>2017-04-25 杨浩 创建</remarks>
        public Result SynchronizeOrderToXinYeErp()
        {
            var result = new Result<dynamic>
            {
                Message = "同步成功！",
                Status = true
            };
          
            var xyClient = Extra.Erp.XingYe.XingYeProviderFactory.CreateProvider();//信业

       
            var xyList = Hyt.BLL.Sys.XingYeBo.Instance.GetSyncList(9999, false);
            int  index = 0;
            for (; index < xyList.Count; index++)
            {
                xyClient.Resynchronization(xyList[index].SysNo);
            }

            if (xyList.Count <= 0)
            {
                result.Status = false;
                result.Message = "没有可同步的数据";
            }

            return result;
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

            var ljClient = Extra.Erp.LiJia.LiJiaProviderFactory.CreateProvider();

            var xyClient=Extra.Erp.XingYe.XingYeProviderFactory.CreateProvider();//信业

            var list = Hyt.BLL.Sys.EasBo.Instance.GetSyncList(9999,false);

           
            int index = 0;
            for (; index < list.Count; index++)
            {
                           
                 client.Resynchronization(list[index].SysNo);                                                                 
            }

            var failureList = IEasDao.Instance.GetSyncFailureList(9999);
            foreach (var item in failureList)
            {             
                client.Resynchronization(item.SysNo);                              
            }



            var ljList = Hyt.BLL.Sys.LiJiaBo.Instance.GetSyncList(9999, false);
            index = 0;
            for (; index < ljList.Count; index++)
            {
                ljClient.Resynchronization(ljList[index].SysNo);
            }


            var xyList=Hyt.BLL.Sys.XingYeBo.Instance.GetSyncList(9999, false);
            index = 0;
            for (; index < xyList.Count; index++)
            {
                xyClient.Resynchronization(xyList[index].SysNo);
            }


            if (list.Count <= 0)
            {
                result.Status = false;
                result.Message = "没有可同步的数据";
            }

            return result;
        }

        /// <summary>
        /// 获取Eas同步日志
        /// </summary>
        /// <param name="sysNos"></param>
        /// <returns></returns>
        /// <remarks>2014-5-4 杨浩 创建</remarks>
        public IList<EasSyncLog> GetList(int[] sysNos)
        {
           return IEasDao.Instance.GetList(sysNos==null?"":string.Join(",",sysNos));
        }
        /// <summary>
        /// 是否存在未处理的关联单据
        /// </summary>
        /// <returns></returns>
        /// <remarks>2014-4-9 杨浩 创建</remarks>
        public bool IsRelate(int sysNo)
        {
            var item = GetEntity(sysNo);
            var list = IEasDao.Instance.GetRelateList(item.FlowIdentify);
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
        public EasSyncLog GetEntity(int sysNo)
        {
            return IEasDao.Instance.GetEntity(sysNo);
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        /// <remarks>2013-10-23 黄志勇 创建</remarks>
        public int Update(EasSyncLog entity)
        {
            return IEasDao.Instance.Update(entity);
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="obj">对象列表</param>
        /// <param name="size">每页条数</param>
        /// <param name="page">当前页</param>
        /// <returns>分页数据</returns>
        /// <remarks>2013-8-13 黄志勇 添加</remarks>
        public Pager<object> GetEasData(object obj, int size, int page)
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
             string voucherNo=IEasDao.Instance.GetVoucherNo(flowType.ToString(),flowIdentify);
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
            return IEasDao.Instance.GetNoSyncStockOutList(warehouseSysNo);
        }
    }
}
