
using Hyt.DataAccess.Distribution;
using Hyt.Model;
using Hyt.Model.Parameter;
using System.Collections.Generic;
namespace Hyt.BLL.Distribution
{
    /// <summary>
    /// 分销商用户操作
    /// </summary>
    /// <remarks>2014-06-05  朱成果 创建</remarks>
    public class DsUserBo : BOBase<DsUserBo>
    {
        #region 数据记录增，删，改，查
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2014-06-05  朱成果 创建</remarks>
        public int Insert(DsUser entity)
        {
            return IDsUserDao.Instance.Insert(entity);
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns></returns>
        /// <remarks>2014-06-05  朱成果 创建</remarks>
        public void Update(DsUser entity)
        {
            IDsUserDao.Instance.Update(entity);
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>数据实体</returns>
        /// <remarks>2014-06-05  朱成果 创建</remarks>
        public DsUser GetEntity(int sysNo)
        {
            return IDsUserDao.Instance.GetEntity(sysNo);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns></returns>
        /// <remarks>2014-06-05  朱成果 创建</remarks>
        public void Delete(int sysNo)
        {
            IDsUserDao.Instance.Delete(sysNo);
        }
        #endregion

        /// <summary>
        /// 获取分销商用户
        /// </summary>
        /// <param name="dealerSysNo">分销商编号</param>
        /// <param name="account">帐号</param>
        /// <returns>分销商用户实体</returns>
        /// <remarks>2014-06-05  朱成果 创建</remarks>
        public  DsUser GetEntity(string account)
        {
            return IDsUserDao.Instance.GetEntity(account);
        }

        /// <summary>
        /// 根据分销商编号获取分销商用户列表
        /// </summary>
        /// <param name="dealerSysNo">分销商编号</param>
        /// <returns>分销商用户列表</returns>
        /// <remarks>2014-06-05  朱成果 创建</remarks>
        public  List<DsUser> GetListByDealerSysNo(int dealerSysNo)
        {
            return IDsUserDao.Instance.GetListByDealerSysNo(dealerSysNo);
        }

        public DsUser GetListByDealerSysNo(int dsSysNo, string accout, string pass)
        {
            return IDsUserDao.Instance.GetListByDealerSysNo(dsSysNo, accout, pass);
        }


        public List<DsUser> GetListByDealerSysNo()
        {
            return IDsUserDao.Instance.GetListByDealerSysNo();
        }
    }
}
