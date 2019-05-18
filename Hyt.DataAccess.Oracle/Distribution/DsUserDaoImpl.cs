

using Hyt.Model;
using System.Collections.Generic;
using Hyt.DataAccess.Base;
namespace Hyt.DataAccess.Distribution
{
    /// <summary>
    /// 分销商用户
    /// </summary>
    /// <remarks>2014-06-05  朱成果 创建</remarks>
    public class DsUserDaoImpl : IDsUserDao
    {
        #region 数据记录增，删，改，查
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2014-06-05  朱成果 创建</remarks>
        public override int Insert(DsUser entity)
        {
            entity.SysNo = Context.Insert("DsUser", entity)
                                        .AutoMap(o => o.SysNo)
                                        .ExecuteReturnLastId<int>("SysNo");
            return entity.SysNo;
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns></returns>
        /// <remarks>2014-06-05  朱成果 创建</remarks>
        public override void Update(DsUser entity)
        {

            Context.Update("DsUser", entity)
                   .AutoMap(o => o.SysNo)
                   .Where("SysNo", entity.SysNo)
                   .Execute();
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>数据实体</returns>
        /// <remarks>2014-06-05  朱成果 创建</remarks>
        public override DsUser GetEntity(int sysNo)
        {

            return Context.Sql("select * from DsUser where SysNo=@SysNo")
                   .Parameter("SysNo", sysNo)
              .QuerySingle<DsUser>();
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns></returns>
        /// <remarks>2014-06-05  朱成果 创建</remarks>
        public override void Delete(int sysNo)
        {
            Context.Sql("Delete from DsUser where SysNo=@SysNo")
                 .Parameter("SysNo", sysNo)
            .Execute();
        }
        #endregion

        /// <summary>
        /// 获取分销商用户
        /// </summary>    
        /// <param name="account">帐号</param>
        /// <returns>分销商用户实体</returns>
        /// <remarks>2014-06-05  朱成果 创建</remarks>
        public override  DsUser GetEntity( string account)
        {
            return Context.Sql("select * from DsUser where  account=@account")
                  .Parameter("account", account)
             .QuerySingle<DsUser>();
        }

        /// <summary>
        /// 根据分销商编号获取分销商用户列表
        /// </summary>
        /// <param name="dealerSysNo">分销商编号</param>
        /// <returns>分销商用户列表</returns>
        /// <remarks>2014-06-05  朱成果 创建</remarks>
        public override List<DsUser> GetListByDealerSysNo(int dealerSysNo)
        {
            return Context.Sql("select * from DsUser where (dealerSysNo=@dealerSysNo or @dealerSysNo=0 ) order by CreatedDate desc")
                  .Parameter("dealerSysNo", dealerSysNo)
                  .QueryMany<DsUser>();
        }


        public override DsUser GetListByDealerSysNo(int dsSysNo, string accout, string pass)
        {
            return Context.Sql("select * from DsUser where dealerSysNo=@dealerSysNo and Account=@Account and Password=@Password order by CreatedDate desc")
                  .Parameter("dealerSysNo", dsSysNo)
                  .Parameter("Account", accout)
                  .Parameter("Password", pass)
                  .QuerySingle<DsUser>();
        }

        public override List<DsUser> GetListByDealerSysNo()
        {
            return Context.Sql("select * from DsUser  order by CreatedDate desc")
                  .QueryMany<DsUser>();
        }
    }
}
