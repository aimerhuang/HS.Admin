using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.DataAccess.Base;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;

namespace Hyt.DataAccess.Sys
{
    /// <summary>
    /// 用户信息接口类
    /// </summary>
    /// <remarks> 
    /// 2013-06-13 郑荣华 创建
    /// </remarks>
    public abstract class ISyUserDao : DaoBase<ISyUserDao>
    {
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userSysNo">用户系统编号List</param>
        /// <returns>用户信息列表</returns>
        /// <remarks> 
        /// 2013-06-13 郑荣华 创建
        /// </remarks>
        public abstract IList<SyUser> GetSyUser(List<int> userSysNo);

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userName">用户名称</param>
        /// <returns>用户信息列表</returns>
        /// <remarks> 
        /// 2013-12-31 黄伟 创建
        /// </remarks>
        public abstract List<SyUser> GetSyUsersByName(string userName);

        /// <summary>
        /// 通过用户组取用户列表
        /// </summary>
        /// <param name="groupSysNo">用户组编号</param>
        /// <returns>用户信息列表</returns>
        /// <remarks> 
        /// 2013-11-27 余勇 创建
        /// </remarks>
        public abstract IList<SyUser> GetSyUserByGroupSysNo(int groupSysNo);

        /// <summary>
        /// 通过用户账号跟用户密码获取用户信息
        /// </summary>
        /// <param name="account">用户账号</param>
        /// <param name="password">用户密码</param>
        /// <returns>返回用户</returns>
        /// <remarks>2013-06-19 周唐炬 Crearte</remarks>
        public abstract SyUser GetSyUser(string account, string password);

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <returns>返回操作行</returns>
        ///  <remarks>2013-06-19 周唐炬 Crearte</remarks>
        public abstract int UpdateSyUser(SyUser user);

        /// <summary>
        /// 更新用户状态
        /// </summary>
        /// <param name="sysNo">用户编号</param>
        /// <param name="status">状态</param>
        /// <returns>返回操作行</returns>
        /// <remarks>2013-08-12 黄志勇 Crearte</remarks>
        public abstract int UpdateSyUserStatus(int sysNo, int status);

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2013-07-30  朱成果 创建</remarks>
        public abstract int InsertSyUser(SyUser entity);

        /// <summary>
        /// 通过SysNo获取用户信息
        /// </summary>
        /// <param name="sysNo">用户系统编号</param>
        /// <returns>用户信息</returns>
        /// <remarks>2013-06-19 周唐炬 修改</remarks>
        public abstract SyUser GetSyUser(int sysNo);

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="account">用户系统编号</param>
        /// <returns>用户信息</returns>
        /// <remarks>2013-07-16 yangheyu add</remarks>
        public abstract SyUser GetSyUser(string account);

        /// <summary>
        /// 用户列表分页
        /// </summary>
        /// <param name="filter">传入的实体参数</param>
        /// <returns>用户信息</returns>
        /// <remarks>2013-07-16 黄志勇 添加</remarks>
        public abstract Pager<CBSyUser> GetSyUser(ParaSyUserFilter filter);

        /// <summary>
        /// 插入用户仓库权限
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2013-07-30  黄志勇 创建</remarks>
        public abstract int InsertSyUserWarehouse(SyUserWarehouse entity);

        /// <summary>
        /// 删除用户仓库权限
        /// </summary>
        /// <param name="userSysNo">用户编号</param>
        /// <param name="warehouseSysNo">仓库编号</param>
        /// <returns>受影响行数</returns>
        /// <remarks>2013-08-08  黄志勇 创建</remarks>
        public abstract int DeleteSyUserWarehouse(int userSysNo, int warehouseSysNo);

        /// <summary>
        /// 判断是否存在用户仓库权限
        /// </summary>
        /// <param name="userSysNo">用户编号</param>
        /// <param name="warehouseSysNo">仓库编号</param>
        /// <returns></returns>
        /// <remarks>2013-10-31  朱成果 创建</remarks>
        public abstract bool ExistsSyUserWarehouse(int userSysNo, int warehouseSysNo);

        /// <summary>
        /// 读取出去指定列表的其余有效用户
        /// </summary>
        /// <param name="users">排除用户系统编号列表</param>
        /// <returns>系统用户列表</returns>
        /// <remarks>2014-03-05  邵斌 创建</remarks>
        public abstract IList<SyUser> GetUserListWithoutSysNoList(IList<int> users);

        public abstract SyUser GetSyUserByOpenId(string openId);


        /// <summary>
        /// 模糊查询姓名、注册账号、手机
        /// </summary>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        public abstract List<SyUser> GetUtilLike(string keyWord);
      
    }
}
