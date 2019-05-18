using Hyt.Model;
using Hyt.DataAccess.Sys;

namespace Hyt.BLL.Sys
{
    /// <summary>
    /// 用户分组
    /// </summary>
    /// <remarks>2013-07-30 黄志勇 创建</remarks>
    public class SyGroupUserBo : BOBase<SyGroupUserBo>
    {
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="syGroupUser">数据实体</param>
        /// <returns></returns>
        /// <remarks>2013-08-08  黄志勇 创建</remarks>
        public void Insert(SyGroupUser syGroupUser)
        {
            syGroupUser.SysNo = ISyGroupUserDao.Instance.Insert(syGroupUser);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="userSysNo">用户编号</param>
        /// <param name="groupSysNo">用户组编号</param>
        /// <returns></returns>
        /// <remarks>2013-07-30 黄志勇 创建</remarks>
        public void Delete(int userSysNo, int groupSysNo)
        {
            ISyGroupUserDao.Instance.Delete(userSysNo, groupSysNo);
        }
    }
}
