using System.Collections.Generic;
using Hyt.DataAccess.Sys;
using Hyt.Model;
using System;
namespace Hyt.DataAccess.Oracle.Sys
{
    /// <summary>
    /// 我的菜单
    /// </summary>
    /// <remarks>2013-01-15 ZTJ 添加注释</remarks>
    public class SyMyMenuImpl : ISyMyMenuDao
    {

        /// <summary>
        /// 查询实体
        /// </summary>
        /// <param name="userSysNo">用户系统编号</param>
        /// <param name="menuSysNo">菜单系统编号</param>
        /// <returns>菜单实体</returns>
        /// <remarks>2013-01-15 ZTJ 添加注释</remarks>
        public override SyMyMenu GetModel(int userSysNo, int menuSysNo)
        {
            return Context.Sql("select * from SyMyMenu where UserSysNo=@UserSysNo and MenuSysNo = @MenuSysNo")
                          .Parameter("UserSysNo", userSysNo)
                          .Parameter("MenuSysNo", menuSysNo)
                          .QuerySingle<SyMyMenu>();

        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2013-10-09  周瑜 创建</remarks>
        public override int Insert(SyMyMenu entity)
        {
            if (entity.CreatedDate == DateTime.MinValue)
            {
                entity.CreatedDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            if (entity.LastUpdateDate == DateTime.MinValue)
            {
                entity.LastUpdateDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            entity.SysNo = Context.Insert("SyMyMenu", entity)
                                        .AutoMap(o => o.SysNo)
                                        .ExecuteReturnLastId<int>("SysNo");
            return entity.SysNo;
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="userSysNo">用户系统编号</param>
        /// <param name="menuSysNo">菜单系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-10-09  周瑜 创建</remarks>
        public override void Delete(int userSysNo, int menuSysNo)
        {
            Context.Sql("Delete from SyMyMenu where UserSysNo=@UserSysNo and MenuSysNo = @MenuSysNo")
                 .Parameter("UserSysNo", userSysNo)
                 .Parameter("MenuSysNo", menuSysNo)
            .Execute();
        }

        /// <summary>
        /// 获取我的菜单
        /// </summary>
        /// <param name="userSysNo">系统用户号</param>
        /// <returns>菜单列表</returns>
        /// <remarks> 2013-10-09 周瑜 创建</remarks>
        public override IList<SyMenu> GetList(int userSysNo)
        {
            return Context.Sql(
                "select a.* from symenu a inner join symymenu b on a.sysno = b.menusysno where UserSysNo = @UserSysNo")
                   .Parameter("UserSysNo", userSysNo)
                   .QueryMany<SyMenu>();
        }
    }
}
