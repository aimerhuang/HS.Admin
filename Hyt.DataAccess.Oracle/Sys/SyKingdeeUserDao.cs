using Hyt.DataAccess.Sys;
using Hyt.Model;
using Hyt.Model.Generated;
using Hyt.Model.Parameter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.Sys
{
    /// <summary>
    /// 系统_金蝶_用户关联表
    /// </summary>
    public class SyKingdeeUserDao : ISyKingdeeUserDao
    {
        /// <summary>
        /// 获取业务员用户
        /// </summary>
        /// <returns></returns>
        /// 2018-1-4 吴琨 创建
        public override List<SyUser> GetSyUser()
        {
            return Context.Sql(" select suser.* from SyUser as suser")
                .QueryMany<SyUser>();
        }



        /// <summary>
        /// 获取业务员用户
        /// </summary>
        /// <returns></returns>
        /// 2018-1-6 吴琨 创建
        public override SyKingdeeUser GetModels(int sysNo)
        {
            return Context.Sql(" select * from SyKingdeeUser where SysNo=@SysNo")
                .Parameter("SysNo", sysNo)
                .QuerySingle<SyKingdeeUser>();
        }


        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="para">查询参数</param>
        /// <returns></returns>
        /// <remarks> 2018-1-4 吴琨 创建</remarks>
        public override Pager<SyKingdeeUser> GetPages(ParaPrPurchaseFilter para)
        {
            var paras = new List<object>();
            //paras.Add(para.WarehouseSysNo);

            string whereStr = " where 1=1 ";
            if (!string.IsNullOrEmpty(para.KeyWord))
            {
                whereStr += @" and (SyUser.Account like @" + paras.Count + @"  
                            or SyUser.UserName like @" + paras.Count + @" 
                            or SyUser.MobilePhoneNumber like @" + paras.Count + @" 
                            or SyKingdeeUser.KingdeeUserCode like @" + paras.Count + @" 
                            or SyUser.SysNo like @" + paras.Count + @" 
                            or SyUser.Account like @" + paras.Count + @" 
                            )";
                paras.Add("%" + para.KeyWord + "%");
            }
            string sql = @"
              (
                select SyKingdeeUser.*,SyUser.Account,SyUser.UserName as SyUserName from SyKingdeeUser 
                inner join SyUser on SyUser.SysNo=SyKingdeeUser.SyUserSysNo " + whereStr + @"
              ) tb";

            var dataList = Context.Select<SyKingdeeUser>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(0)").From(sql);
            dataList.Parameters(paras.ToArray());
            dataCount.Parameters(paras.ToArray());
            var pager = new Pager<SyKingdeeUser>
            {
                PageSize = para.PageSize,
                CurrentPage = para.Id,
                TotalRows = dataCount.QuerySingle(),
                Rows = dataList.OrderBy("tb.SysNo desc").Paging(para.Id, para.PageSize).QueryMany()
            };
            return pager;
        }

        /// <summary>
        /// 创建第三方关联用户
        /// </summary>
        /// <returns></returns>
        ///  2018-1-4 吴琨 创建
        public override bool AddModels(SyKingdeeUser models)
        {
            return Context.Insert("SyKingdeeUser")
                 .Column("Type", models.Type)
                 .Column("SyUserSysNo", models.SyUserSysNo)
                 .Column("KingdeeUserCode", models.KingdeeUserCode)
                 .Execute() > 0;
        }

        /// <summary>
        /// 编辑第三方关联用户
        /// </summary>
        /// <returns></returns>
        ///  2018-1-6 吴琨 创建
        public override bool UpModels(SyKingdeeUser models)
        {
            return Context.Update("SyKingdeeUser")
                 .Column("Type", models.Type)
                 .Column("SyUserSysNo", models.SyUserSysNo)
                 .Column("KingdeeUserCode", models.KingdeeUserCode)
                 .Where("SysNo", models.SysNo)
                 .Execute() > 0;
        }

        /// <summary>
        /// 根据编号删除
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        ///  2018-1-4 吴琨 创建
        public override bool delModels(int SysNo)
        {
            return Context.Delete("SyKingdeeUser")
                 .Where("SysNo", SysNo)
                 .Execute() > 0;
        }
        /// <summary>
        /// 获取金蝶用户代码
        /// </summary>
        /// <param name="type">类型（10:金蝶）</param>
        /// <param name="userSysno">用户系统编号</param>
        /// <returns></returns>
        /// <remarks>2018-1-5 杨浩 创建</remarks>
        public override string GetKingdeeUserCode(int userSysno, int type = 10)
        {
            return Context.Sql("select top 1 KingdeeUserCode from SyKingdeeUser where type=@type and SyUserSysNo=@SyUserSysNo")
                 .Parameter("type", type)
                 .Parameter("SyUserSysNo", userSysno)
                 .QuerySingle<string>();
        }

    }
}
