using Hyt.DataAccess.MallSeller;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.WorkflowStatus;

namespace Hyt.BLL.MallSeller
{
    public class DsDealerLogBo : BOBase<DsDealerLogBo>
    {
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>返回新的编号</returns>
        /// <remarks>2014-03-31 唐文均 创建</remarks>
        public int Insert(DsDealerLog entity)
        {
            return IDsDealerLog.Instance.Insert(entity);
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns></returns>
        /// <remarks>2014-03-31 唐文均 创建</remarks>
        public void Update(DsDealerLog entity)
        {
            IDsDealerLog.Instance.Update(entity);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns></returns>
        /// <remarks>2014-03-31 唐文均 创建</remarks>
        public int Delete(int sysNo)
        {
            return IDsDealerLog.Instance.Delete(sysNo);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>分页</returns>
        /// <remarks>2014-03-31 朱家宏 创建</remarks>
        public Pager<DsDealerLog> GetPagerList(ParaDsDealerLogFilter filter)
        {
            return IDsDealerLog.Instance.Query(filter);
        }
        /// <summary>
        /// 检查订单号是否存在
        /// </summary>
        /// <param name="mallOrderId">商商城订单号</param>
        /// <param name="status">待解决(10),已解决(20)</param>
        /// <param name="mallSysNo">商城系统编号</param>
        /// <returns></returns>
        /// <remarks>2017-11-06 杨浩 创建</remarks>
        public  bool ChekMallOrderId(string mallOrderId, int status, int mallSysNo)
        {
            return IDsDealerLog.Instance.ChekMallOrderId(mallOrderId,status,mallSysNo);
        }
        /// <summary>
        /// 更改升舱日志状态
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>result</returns>
        /// <remarks>2014-03-31 朱家宏 创建</remarks>
        public Result ChangeDealerLogStatus(int sysNo)
        {
            var result = new Result
                {
                    Status = false,
                    Message = ""
                };

            var model = IDsDealerLog.Instance.Get(sysNo);
            if (model != null)
            {
                if (model.Status == (int) DistributionStatus.分销商升舱错误日志状态.待解决)
                {
                    model.Status = (int) DistributionStatus.分销商升舱错误日志状态.已解决;
                    result.StatusCode = (int) DistributionStatus.分销商升舱错误日志状态.已解决;
                }
                else
                {
                    model.Status = (int)DistributionStatus.分销商升舱错误日志状态.待解决;
                    result.StatusCode = (int)DistributionStatus.分销商升舱错误日志状态.待解决;
                }
                IDsDealerLog.Instance.Update(model);
                result.Status = true;
            }
            return result;
        }
    }
}
