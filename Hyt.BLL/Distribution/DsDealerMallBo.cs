using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.BLL.Log;
using Hyt.DataAccess.Distribution;
using Hyt.Infrastructure.Memory;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.BLL.Distribution
{
    /// <summary>
    /// 分销商商城业务类
    /// </summary>
    /// <remarks>
    /// 2013-09-18 郑荣华 创建
    /// </remarks>
    public class DsDealerMallBo : BOBase<DsDealerMallBo>
    {
        #region 操作

        /// <summary>
        /// 创建分销商商城
        /// </summary>
        /// <param name="model">分销商商城实体</param>
        /// <returns>新加的系统编号</returns>
        /// <remarks>
        /// 2013-09-18 郑荣华 创建
        /// 2014-05-13余勇 修改 清除缓存数据
        /// </remarks>
        public int Create(DsDealerMall model)
        {
            var sysno = IDsDealerMallDao.Instance.Create(model);
            SysLog.Instance.Info(LogStatus.系统日志来源.后台, "创建分销商商城", LogStatus.系统日志目标类型.分销商商城, sysno);
            return sysno;
        }

        /// <summary>
        /// 修改分销商商城,授权码为空不更新
        /// </summary>
        /// <param name="model">分销商商城实体</param>
        /// <returns>受影响的行数</returns>
        /// <remarks>
        /// 2013-09-18 郑荣华 创建
        /// 2014-05-13余勇 修改 清除缓存数据
        /// </remarks>
        public int Update(DsDealerMall model)
        {
            var r = IDsDealerMallDao.Instance.Update(model);
            if (r > 0)
            {
                SysLog.Instance.Info(LogStatus.系统日志来源.后台, "修改分销商商城", LogStatus.系统日志目标类型.分销商商城, model.SysNo);
                MemoryProvider.Default.Remove(string.Format(KeyConstant.DsDealerMall, model.SysNo));
            }
            return r;
        }

        /// <summary>
        /// 分销商商城状态更新
        /// </summary>
        /// <param name="sysNo">分销商商城系统编号</param>
        /// <param name="status">分销商商城状态</param>
        /// <param name="lastUpdateBy">最后更新人</param>
        /// <returns>受影响的行数</returns>
        /// <remarks>
        /// 2013-09-18 郑荣华 创建
        /// 2014-05-13余勇 修改 清除缓存数据
        /// </remarks>
        public int UpdateStatus(int sysNo, DistributionStatus.分销商商城状态 status, int lastUpdateBy)
        {
            var r = IDsDealerMallDao.Instance.UpdateStatus(sysNo, status, lastUpdateBy);
            if (r > 0)
            {
                SysLog.Instance.Info(LogStatus.系统日志来源.后台, "修改分销商商城状态", LogStatus.系统日志目标类型.分销商商城, sysNo);
                MemoryProvider.Default.Remove(string.Format(KeyConstant.DsDealerMall, sysNo));
            }
            return r;
        }
        #endregion

        #region 查询
        /// <summary>
        /// 分页查询分销商商城信息列表
        /// </summary>
        /// <param name="pager">分销商商城信息列表分页对象</param>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        /// <remarks> 
        /// 2013-09-18 郑荣华 创建
        /// </remarks>
        public void GetDsDealerMallList(ref Pager<CBDsDealerMall> pager, ParaDsDealerMallFilter filter)
        {
            IDsDealerMallDao.Instance.GetDsDealerMallList(ref pager, filter);
        }
        /// <summary>
        /// 分销商授权码获取
        /// </summary>
        /// <param name="AuthCode">授权码</param>
        /// <param name="shopid">分销商编号</param>
        /// <returns>受影响的行数</returns>
        /// <remarks>2017-9-1 杨大鹏 创建</remarks>
        public bool UpdateAuthCode(string accessToken, int shopid)
        {

           return  IDsDealerMallDao.Instance.UpdateAuthCode(accessToken,shopid)>0;

        }
        /// <summary>
        /// 获取分销商商城
        /// </summary>
        /// <param name="ridSysNo">排除的商城系统编号</param>
        /// <param name="mallTypeSysNo">商城类型系统编号</param>
        /// <param name="shopAccount">店铺账号</param>
        /// <returns>分销商商城实体</returns>
        /// <remarks> 
        /// 2013-09-18 郑荣华 创建 作唯一性检查使用（商城类型系统编号+店铺账号 唯一）
        /// </remarks>
        public CBDsDealerMall GetDsDealerMall(int? ridSysNo, int mallTypeSysNo, string shopAccount)
        {
            return IDsDealerMallDao.Instance.GetDsDealerMall(ridSysNo, mallTypeSysNo, shopAccount);
        }
        /// <summary>
        /// 获取分销商商城
        /// </summary>
        /// <param name="mallTypeSysNo">商城类型系统编号</param>
        /// <param name="dealerSysNo">分销商系统编号</param>
        /// <returns>分销商商城实体</returns>
        /// <remarks> 
        /// 2016-07-06 杨浩 创建 
        /// </remarks>
        public DsDealerMall GetDsDealerMall(int mallTypeSysNo, int dealerSysNo)
        {
            return IDsDealerMallDao.Instance.GetDsDealerMall(mallTypeSysNo,dealerSysNo);
        }
        /// <summary>
        /// 获取分销商商城信息 
        /// </summary>
        /// <param name="sysNo">分销商商城系统编号</param>
        /// <returns>分销商商城息信息</returns>
        /// <remarks>
        /// 2013-09-18 郑荣华 创建
        /// </remarks>
        public CBDsDealerMall GetDsDealerMall(int sysNo)
        {
            return IDsDealerMallDao.Instance.GetDsDealerMall(sysNo);
        }

        /// 获取分销商商城列表
        /// </summary>
        /// <returns>分销商商城列表</returns>
        /// <remarks>
        /// 2014-02-18 朱成果  创建
        /// </remarks>
        public  List<DsDealerMall> GetList()
        {
            return IDsDealerMallDao.Instance.GetList();
        }

        /// <summary>
        /// 获取分销商商城实体
        /// </summary>
        /// <param name="sysNO">分销商商城编号</param>
        /// <returns></returns>
        /// <remarks>
        /// 2014-06-11 朱成果 创建
        /// </remarks>
        public  DsDealerMall GetEntity(int sysNO)
        {

            return IDsDealerMallDao.Instance.GetEntity(sysNO);
        }

        /// <summary>
        /// 通过DealerAppSysNo查找商城关联数量
        /// </summary>
        /// <param name="appSysNo">appkey系统编号</param>
        /// <returns>关联数量</returns>
        public int GetAppKeyUseNum(int appSysNo)
        {
            return IDsDealerMallDao.Instance.GetAppKeyUseNum(appSysNo);
        }

        /// <summary>
        /// 获取分销商商城信息 
        /// </summary>
        /// <param name="DealerSysNo">分销商商城系统编号</param>
        /// <returns>分销商商城息信息</returns>
        /// <remarks>
        /// 2015-12-11 王耀发 创建
        /// </remarks>
        public CBDsDealerMall GetDsDealerMallByDealerSysNo(int dealerSysNo)
        {
            return IDsDealerMallDao.Instance.GetDsDealerMallByDealerSysNo(dealerSysNo);
        }

        /// <summary>
        /// 根据商城类型获取所有商城
        /// </summary>
        /// <param name="mallTypeSysNo">商城类型系统编号</param>
        /// <returns></returns>
        /// <remarks>2017-11-02 杨浩 创建</remarks>
        public  IList<DsDealerMall> GetDealerMallByMallTypeSysNo(int mallTypeSysNo)
        {
            return IDsDealerMallDao.Instance.GetDealerMallByMallTypeSysNo(mallTypeSysNo);
        }
        #endregion
    }
}
