using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Sys;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Util;
using Hyt.Util.Extension;

namespace Hyt.DataAccess.Oracle.Sys
{

    /// <summary>
    /// 用户信息数据访问类
    /// </summary>
    /// <remarks> 
    /// 2013-06-13 郑荣华 创建
    /// </remarks>
    public class SyUserDaoImpl : ISyUserDao
    {
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userSysNo">用户系统编号List</param>
        /// <returns>用户信息列表</returns>
        /// <remarks> 
        /// 2013-06-13 郑荣华 创建
        /// </remarks>
        public override IList<SyUser> GetSyUser(List<int> userSysNo)
        {
            return Context.Sql("select * from syuser where Sysno in(@0)", userSysNo)
                          .QueryMany<SyUser>();
        }

        /// <summary>
        /// 通过用户组取用户列表
        /// </summary>
        /// <param name="groupSysNo">用户组编号</param>
        /// <returns>用户信息列表</returns>
        /// <remarks> 
        /// 2013-11-27 余勇 创建
        /// </remarks>
        public override IList<SyUser> GetSyUserByGroupSysNo(int groupSysNo)
        {
            return Context.Sql("select * from syuser where status=1 and Sysno in( select usersysno from  sygroupuser where groupsysno=@0 union select  distinct t.usersysno from syjobdispatcher t where t.Status=1 and  not exists (select usersysno from  sygroupuser where groupsysno=@1 and usersysno=t.usersysno)) order by UserName", groupSysNo, groupSysNo)
                         .QueryMany<SyUser>();
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userName">用户名称</param>
        /// <returns>用户信息列表</returns>
        /// <remarks> 
        /// 2013-12-31 黄伟 创建
        /// </remarks>
        public override List<SyUser> GetSyUsersByName(string userName)
        {
            return Context.Sql("select * from syuser where username=@0", userName)
                          .QueryMany<SyUser>();
        }

        /// <summary>
        /// 通过用户账号跟用户密码获取用户信息
        /// </summary>
        /// <param name="account">用户账号</param>
        /// <param name="password">用户密码</param>
        /// <returns>返回用户</returns>
        /// <remarks>2013-06-19 周唐炬 Crearte</remarks>
        public override SyUser GetSyUser(string account, string password)
        {
            return Context.Sql(@"SELECT * FROM SYUSER WHERE Account=@Account AND Password=@Password")
                .Parameter("Account", account)
                .Parameter("Password", password).QuerySingle<SyUser>();
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <returns>返回操作行</returns>
        /// <remarks>2013-06-19 周唐炬 Crearte</remarks>
        public override int UpdateSyUser(SyUser user)
        {
            int rowsAffected = 0;
            rowsAffected = Context.Update<SyUser>("SYUSER", user)
                .AutoMap(x => x.SysNo)
                .Where(x => x.SysNo)
                .Execute();
            return rowsAffected;
        }

        /// <summary>
        /// 更新用户状态
        /// </summary>
        /// <param name="sysNo">用户编号</param>
        /// <param name="status">状态</param>
        /// <returns>返回操作行</returns>
        /// <remarks>2013-08-12 黄志勇 Crearte</remarks>
        public override int UpdateSyUserStatus(int sysNo, int status)
        {
            int rowsAffected = Context.Update("SyUser")
            .Column("Status", status)
            .Where("SysNo", sysNo)
            .Execute();
            return rowsAffected;
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2013-07-30  朱成果 创建</remarks>
        public override int InsertSyUser(SyUser entity)
        {
            entity.SysNo = Context.Insert("SyUser", entity)
                                        .AutoMap(o => o.SysNo)
                                        .ExecuteReturnLastId<int>("SysNo");
            return entity.SysNo;
        }

        /// <summary>
        /// 通过SysNo获取用户信息
        /// </summary>
        /// <param name="sysNo">用户系统编号</param>
        /// <returns>用户信息</returns>
        /// <remarks>2013-06-19 周唐炬 修改</remarks>
        public override SyUser GetSyUser(int sysNo)
        {
            return Context.Sql(@"SELECT * FROM SYUSER WHERE SysNo=@SysNo")
                          .Parameter("SysNo", sysNo)
                          .QuerySingle<SyUser>();
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="account">用户系统编号</param>
        /// <returns>用户信息</returns>
        /// <remarks>2013-07-16 yangheyu add</remarks>
        public override SyUser GetSyUser(string account)
        {
            return Context.Sql(@"SELECT * FROM SYUSER WHERE account=@account")
                          .Parameter("account", account)
                          .QuerySingle<SyUser>();
        }

        /// <summary>
        /// 用户列表分页
        /// </summary>
        /// <param name="filter">传入的实体参数</param>
        /// <returns>用户信息</returns>
        /// <remarks>2013-07-16 黄志勇 添加</remarks>
        public override Pager<CBSyUser> GetSyUser(ParaSyUserFilter filter)
        {
            const string sql =
                 @"(select distinct a.sysno, a.Account, a.UserName, a.MobilePhoneNumber, a.EmailAddress, a.Status, a.CreatedDate, a.CreatedBy, a.LastUpdateDate, a.LastUpdateBy
                    from    SyUser a
                            left join SyGroupUser b on a.sysno=b.UserSysNo                           
                    where   
                            (@0 is null or CHARINDEX(a.Account,@0)>0) and  --帐号
                            (@1 is null or CHARINDEX(a.UserName,@1)>0) and  --姓名
                            (@2 is null or CHARINDEX(a.MobilePhoneNumber,@2)>0) and  --手机号
                            (@3 is null or CHARINDEX(convert(varchar(4),a.Status),convert(varchar(4),@3))>0) and  --状态
                            (@4 is null or exists (select 1 from SyGroupUser sgu where sgu.GroupSysNo = @4 and sgu.UserSysNo = a.SysNo)) and  --用户组 
                            (@5 is null or exists (select 1 from SyUserWarehouse suw where suw.WarehouseSysNo = @5 and suw.UserSysNo = a.SysNo)) and  --仓库
                            (@6 is null or (@6=a.CreatedBy or a.sysno=@6))
                    ) tb";

            var paras = new object[]
                {
                    filter.Account,
                    filter.UserName,
                    filter.MobilePhoneNumber,
                    filter.Status,
                    filter.GroupSysNo,
                    filter.WarehouseSysNo,
                    filter.CreatedBy
                };

            var dataList = Context.Select<CBSyUser>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(0)").From(sql);

            dataList.Parameters(paras);
            dataCount.Parameters(paras);

            var pager = new Pager<CBSyUser>
            {
                PageSize = filter.PageSize,
                CurrentPage = filter.Id
            };

            pager.TotalRows = dataCount.QuerySingle();
            var list = dataList.OrderBy("tb.SysNo desc").Paging(pager.CurrentPage, filter.PageSize).QueryMany();
            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    item.GroupUsers = GetGroupUser(item.SysNo);
                }
            }
            pager.Rows = list;

            return pager;
        }

        /// <summary>
        /// 根据用户编号获取用户分组
        /// </summary>
        /// <param name="userSysNo">用户编号</param>
        /// <returns>用户分组列表</returns>
        /// <remarks>2013-07-16 黄志勇 添加</remarks>
        public List<SyGroupUser> GetGroupUser(int userSysNo)
        {
            var list = Context.Sql(@"select * from SyGroupUser where UserSysNo = @0", userSysNo)
                          .QueryMany<SyGroupUser>();
            return list;
        }

        /// <summary>
        /// 插入用户仓库权限
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2013-07-30  黄志勇 创建</remarks>
        public override int InsertSyUserWarehouse(SyUserWarehouse entity)
        {
            entity.SysNo = Context.Insert("SyUserWarehouse", entity)
                                        .AutoMap(o => o.SysNo)
                                        .ExecuteReturnLastId<int>("SysNo");
            return entity.SysNo;
        }

        /// <summary>
        /// 删除用户仓库权限
        /// </summary>
        /// <param name="userSysNo">用户编号</param>
        /// <param name="warehouseSysNo">仓库编号</param>
        /// <returns>受影响行数</returns>
        /// <remarks>2013-08-08  黄志勇 创建</remarks>
        public override int DeleteSyUserWarehouse(int userSysNo, int warehouseSysNo)
        {
            return Context.Sql("delete from SyUserWarehouse where UserSysNo=@UserSysNo and WarehouseSysNo=@WarehouseSysNo")
            .Parameter("UserSysNo", userSysNo)
            .Parameter("WarehouseSysNo", warehouseSysNo)
            .Execute();
        }

        /// <summary>
        /// 判断是否存在用户仓库权限
        /// </summary>
        /// <param name="userSysNo">用户编号</param>
        /// <param name="warehouseSysNo">仓库编号</param>
        /// <returns>true/false</returns>
        /// <remarks>2013-10-31  朱成果 创建</remarks>
        public override bool ExistsSyUserWarehouse(int userSysNo, int warehouseSysNo)
        {

            return Context.Sql("select count(0) from SyUserWarehouse where UserSysNo=@UserSysNo and WarehouseSysNo=@WarehouseSysNo")
          .Parameter("UserSysNo", userSysNo)
          .Parameter("WarehouseSysNo", warehouseSysNo).QuerySingle<int>() > 0;


        }


        /// <summary>
        /// 读取出去指定列表的其余有效用户
        /// </summary>
        /// <param name="users">排除用户系统编号列表</param>
        /// <returns>系统用户列表</returns>
        /// <remarks>2014-03-05  邵斌 创建</remarks>
        public override IList<SyUser> GetUserListWithoutSysNoList(IList<int> users)
        {
            return Context.Sql("select sysno,UserName,Account from SyUser where Status = " +
                        (int)Model.WorkflowStatus.SystemStatus.系统用户状态.启用 + " and sysno not in (" + users.Join(",") +
                        ")").QueryMany<SyUser>();
        }

        public override SyUser GetSyUserByOpenId(string openId)
        {
            return Context.Sql("select sysno,UserName,Account from SyUser where Status = " +
                          (int)Model.WorkflowStatus.SystemStatus.系统用户状态.启用 + " and OpenId  = '" + openId +
                          "'").QuerySingle<SyUser>();
        }

        /// <summary>
        /// 模糊查询姓名、注册账号、手机
        /// </summary>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        public override List<SyUser> GetUtilLike(string keyWord)
        {
            string sqlstr = @"select   distinct  top 50 a.sysno, a.Account, a.UserName, a.MobilePhoneNumber, a.EmailAddress, a.Status, a.CreatedDate, a.CreatedBy, a.LastUpdateDate, a.LastUpdateBy
                    from    SyUser a left join SyGroupUser b on a.sysno=b.UserSysNo   where                         
                    (('%" + keyWord + "%' is null or a.UserName like '%" + keyWord + "%') or ('%" + keyWord + "%' is null or a.Account like '%" + keyWord + "%')  or('%" + keyWord + "%' is null or a.MobilePhoneNumber like '%" + keyWord + "%') )";
            return Context.Sql(sqlstr).QueryMany<SyUser>();
        }
    }
}
