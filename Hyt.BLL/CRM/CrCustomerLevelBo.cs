using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.DataAccess.CRM;
using Hyt.Infrastructure.Pager;
using Hyt.Infrastructure.Caching;
using Hyt.Model.WorkflowStatus;
using Hyt.Model.Transfer;

namespace Hyt.BLL.CRM
{
    /// <summary>
    /// 会员等级表数据访问接口
    /// </summary>
    /// <remarks>2013-06-27 邵斌 创建</remarks>
    public class CrCustomerLevelBo : BOBase<CrCustomerLevelBo>
    {

        /// <summary>
        /// 读取全部会员等级列表
        /// </summary>
        /// <returns>分会会员等级列表（全部）</returns>
        /// <remarks>2013-06-27 邵斌 创建</remarks>
        public IList<CrCustomerLevel> List()
        {
            return CacheManager.Get<IList<CrCustomerLevel>>(CacheKeys.Items.CustomerLevel, () => ICrCustomerLevelDao.Instance.List());            
        }

        /// <summary>
        /// 获取单个最高会员等级信息
        /// </summary>
        /// <param name=" ">  </param>
        /// <returns>返回会员等级对象</returns>
        /// <remarks>2013-12-18 苟治国 创建</remarks>
        public CrCustomerLevel GetCustomerUpperLevel()
        {
            return ICrCustomerLevelDao.Instance.GetCustomerUpperLevel();
        }

        /// <summary>
        /// 获取单个会员等级信息
        /// </summary>
        /// <param name="sysNo">等级编号</param>
        /// <returns>返回会员等级对象</returns>
        /// <remarks>2013-07-01 邵斌 创建</remarks>
        public CrCustomerLevel GetCustomerLevel(int sysNo)
        {
            return ICrCustomerLevelDao.Instance.GetCustomerLevel(sysNo);
        }
        /// <summary>
        /// 获取单个会员等级信息
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        public CBCrCustomerLevelImgUrl GetCustomerLevelImgUrl(int sysNo)
        {
            return ICrCustomerLevelDao.Instance.GetCustomerLevelImgUrl(sysNo);
        }
        /// <summary>
        /// 根据会员等级ID查询会员等级图标表
        /// </summary>
        /// <param name="CrCustomerLevelSysNo"></param>
        /// <returns></returns>
        public CBCrCustomerLevelImgUrl GetCrLevelImgUrl(int CrCustomerLevelSysNo)
        {
            return ICrCustomerLevelDao.Instance.GetCrLevelImgUrl(CrCustomerLevelSysNo);
        }
        /// <summary>
        /// 根据条件获取会员等级的列表
        /// </summary>
        /// <param name="canPayForProduct">惠源币是否可用于支付货款</param>
        /// <param name="canPayForService">惠源币是否可用于支付服务</param>
        /// <returns>会员等级列表</returns>
        /// <remarks>2013－07-10 苟治国 创建</remarks>
        public IList<Model.CrCustomerLevel> Seach(int? canPayForProduct, int? canPayForService)
        {
            return ICrCustomerLevelDao.Instance.Seach(canPayForProduct, canPayForService);
        }

        /// <summary>
        /// 会员等级区间比较
        /// </summary>
        /// <param name="sysNo">会员等级编号</param>
        /// <returns>会员等级列表</returns>
        /// <remarks>2013－07-11 苟治国 创建</remarks>
        public IList<Model.CrCustomerLevel> compare(int sysNo)
        {
            return ICrCustomerLevelDao.Instance.compare(sysNo);
        }

        /// <summary>
        /// 插入会员等级
        /// </summary>
        /// <param name="model">会员等级实体</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－07-10 苟治国 创建</remarks>
        public int Insert(Model.CrCustomerLevel model)
        {
            int result = ICrCustomerLevelDao.Instance.Insert(model);
            if (result > 0)
                BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, string.Format("创建客户等级{0}",model.LevelName), LogStatus.系统日志目标类型.客户等级, Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo);
            else
            {
                BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台, string.Format("创建客户等级{0}失败", model.LevelName), LogStatus.系统日志目标类型.客户等级, Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo);
            }
            return result;
        }

        /// <summary>
        /// 更新会员等级
        /// </summary>
        /// <param name="model">会员等级实体</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－07-10 苟治国 创建</remarks>
        /// <remarks>2014－06-10 余勇 修改 原因：update成功时，记录日志方法应为info而非Error</remarks>
        public int Update(Model.CrCustomerLevel model)
        {
            int result = ICrCustomerLevelDao.Instance.Update(model);
            if (result > 0)
                BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, string.Format("修改客户等级{0}", model.SysNo), LogStatus.系统日志目标类型.客户等级,model.SysNo, Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo);
            else
            {
                BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台, string.Format("修改客户等级{0}失败", model.SysNo), LogStatus.系统日志目标类型.客户等级, model.SysNo,Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo);
            }
            return result;
        }

        /// <summary>
        /// 获取用户初级等级实体
        /// </summary>
        /// <param name=" ">  </param>
        /// <returns>初级会员等级实体</returns>
        /// <remarks>2013－08-06 唐永勤 创建</remarks>
        public CrCustomerLevel GetJuniorLevel()
        {
            IList<CrCustomerLevel> list = List();
            return list.OrderBy(x => x.LowerLimit).First();
        }
        /// <summary>
        /// 插入会员等级图标表数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int InsertCrCustomerLevelImgUrl(CrCustomerLevelImgUrl model)
        {
            int result = ICrCustomerLevelDao.Instance.InsertCrCustomerLevelImgUrl(model);
            if (result > 0)
                BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, string.Format("创建客户等级图标{0}", model.CrCustomerLevelSysNo), LogStatus.系统日志目标类型.客户等级, Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo);
            else
            {
                BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台, string.Format("创建客户等级图标{0}失败", model.CrCustomerLevelSysNo), LogStatus.系统日志目标类型.客户等级, Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo);
            }
            return result;
        }
        /// <summary>
        /// 更新会员等级图标表数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateCrCustomerLevelImgUrl(CrCustomerLevelImgUrl model)
        {
            int result = ICrCustomerLevelDao.Instance.UpdateCrCustomerLevelImgUrl(model);
            if (result > 0)
                BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, string.Format("修改客户等级图标{0}", model.CrCustomerLevelSysNo), LogStatus.系统日志目标类型.客户等级, model.SysNo, Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo);
            else
            {
                BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台, string.Format("修改客户等级图标{0}失败", model.CrCustomerLevelSysNo), LogStatus.系统日志目标类型.客户等级, model.SysNo, Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo);
            }
            return result;
        }
        /// <summary>
        /// 更新等级图标
        /// </summary>
        /// <param name="CrCustomerLevelSysNo"></param>
        /// <param name="CrCustomerLevelImgUrl"></param>
        /// <returns></returns>
        public int UpdateCrCustomerLevelImgUrl(int CrCustomerLevelSysNo, string CrCustomerLevelImgUrl)
        {
            return ICrCustomerLevelDao.Instance.UpdateCrCustomerLevelImgUrl(CrCustomerLevelSysNo, CrCustomerLevelImgUrl);
        }
    }
}
