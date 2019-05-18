using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Util;
using Hyt.Model;
using Hyt.DataAccess.Web;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Model.Parameter;

namespace Hyt.DataAccess.Oracle.Web
{
    /// <summary>
    /// 客户数据访问  
    /// </summary>
    /// <remarks>2013-08-12 苟治国 创建</remarks>
    public class CrCustomerDaoImpl : ICrCustomerDao
    {
        ///<summary>
        ///根据手机获取用户
        /// </summary>
        /// <param name="mobile">手机.</param>
        /// <returns>实体</returns>
        /// <remarks> 2013-08-12 苟治国 创建</remarks>
        public override CrCustomer GetCustomerByCellphone(string mobile)
        {
            return Context.Sql(@"select * from CrCustomer where account = @0", mobile)
                          .QuerySingle<CBCrCustomer>();
        }

        ///<summary>
        ///根据呢称获取用户
        /// </summary>
        /// <param name="nickName">呢称.</param>
        /// <param name="sysNo">客户编号.</param>
        /// <returns>实体</returns>
        /// <remarks>
        /// 2013-08-16 苟治国 创建
        /// </remarks>
        public override CrCustomer GetCustomerByName(string nickName, int sysNo)
        {
            string sql = @"select * from CrCustomer where NickName = @nickName and sysNo!=@sysNO";

            return Context.Sql(sql)
                .Parameter("nickName", nickName)
                .Parameter("sysNO", sysNo)
                .QuerySingle<CrCustomer>();
        }

        /// <summary>
        /// 获取用户收货地址列表
        /// </summary>
        /// <param name="customerSysno">用户编号</param>       
        /// <returns>收货地址列表</returns>
        /// <remarks>
        /// 2013-08-12 唐永勤 创建
        /// </remarks>
        public override IList<CBCrReceiveAddress> GetCustomerReceiveAddress(int customerSysno)
        {
            #region sql query
            string sql = @"select r.*,a.areaname as Region, a.sysno as RegionSysno, c.areaname as City, c.sysno as CitySysno, p.areaname as Province, p.sysno as ProvinceSysno
                        from (select * from CrReceiveAddress where CustomerSysno = {0}) r 
                            left join bsarea a on r.AreaSysno = a.Sysno
                            left join bsarea c on a.ParentSysno = c.Sysno
                            left join bsarea p on c.ParentSysno = p.Sysno
                            ";
            sql = string.Format(sql, customerSysno);
            #endregion

            return Context.Sql(sql).QueryMany<CBCrReceiveAddress>();

        }

        /// <summary>
        /// 获取用户收货地址
        /// </summary>
        /// <param name="receiveSysno">收货地址编号</param>       
        /// <returns>收货地址</returns>
        /// <remarks>
        /// 2013-08-12 唐永勤 创建
        /// </remarks>
        public override CBCrReceiveAddress GetCustomerReceiveAddressBySysno(int receiveSysno)
        {
            #region sql query
            string sql = @"select r.*,a.areaname as Region, a.sysno as RegionSysno, c.areaname as City, c.sysno as CitySysno, p.areaname as Province, p.sysno as ProvinceSysno
                        from (select * from CrReceiveAddress where sysno = {0}) r 
                            left join bsarea a on r.AreaSysno = a.Sysno
                            left join bsarea c on a.ParentSysno = c.Sysno
                            left join bsarea p on c.ParentSysno = p.Sysno
                            ";
            sql = string.Format(sql, receiveSysno);
            #endregion

            return Context.Sql(sql).QuerySingle<CBCrReceiveAddress>();
        }

        /// <summary>
        /// 更新客户
        /// </summary>
        /// <param name="model">客户资料</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-11-11 苟治国 创建</remarks>
        public override int Update(Model.CrCustomer model)
        {
            int rowsAffected = Context.Update<Model.CrCustomer>("CrCustomer", model)
                                      .AutoMap(x => x.SysNo)
                                      .Where(x => x.SysNo)
                                      .Execute();
            return rowsAffected;
        }

        /// <summary>
        /// 更新登录时间、Ip
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <param name="lastLoginIp">客户ip</param>
        /// <returns>空</returns>
        /// <remarks>2013-12-20 苟治国 创建</remarks>
        public override void UpdateDateTimeAndIp(int sysNo, string lastLoginIp)
        {
            Context.Sql("Update CrCustomer Set LastLoginIP = @LastLoginIP,LastLoginDate=@LastLoginDate where sysno = @sysno")
                                      .Parameter("LastLoginIP", lastLoginIp)
                                      .Parameter("LastLoginDate", DateTime.Now)
                                      .Parameter("sysno", sysNo)
                                      .Execute();
        }

        /// <summary>
        /// 更新会员密码
        /// </summary>
        /// <param name="sysno">编号</param>
        /// <param name="passWord">密码</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－09-24 苟治国 创建</remarks>
        public override int UpdatePassWord(int sysno, string passWord)
        {
            int rowsAffected = Context.Sql("Update CrCustomer Set PassWord = @PassWord where sysno = @sysno")
                                      .Parameter("PassWord", passWord)
                                      .Parameter("sysno", sysno)
                                      .Execute();
            return rowsAffected;
        }

        /// <summary>
        /// 更新分销商等级
        /// </summary>
        /// <param name="sysno">编号</param>
        /// <param name="gradeId">分销等级</param>
        /// <returns></returns>
        /// <remarks>2016-12-10 杨浩 创建</remarks>
        public override int UpdateSellBusinessGradeId(int sysno, int gradeId)
        {
            int rowsAffected = Context.Sql("Update CrCustomer Set SellBusinessGradeId= @gradeId where sysno = @sysno")
                                     .Parameter("gradeId", gradeId)
                                     .Parameter("sysno", sysno)
                                     .Execute();
            return rowsAffected;
        }
    }
}
