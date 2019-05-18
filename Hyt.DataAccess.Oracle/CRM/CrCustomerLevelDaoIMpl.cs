using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hyt.Model;
using Hyt.DataAccess;
using Hyt.DataAccess.CRM;
using Hyt.Model.Transfer;

namespace Hyt.DataAccess.Oracle.CRM
{
    /// <summary>
    /// 会员等级表数据访问接口
    /// </summary>
    /// <remarks>2013-06-27 邵斌 创建</remarks>
    public class CrCustomerLevelDaoIMpl : ICrCustomerLevelDao
    {

        /// <summary>
        /// 读取全部会员等级列表
        /// </summary>
        /// <returns>分会会员等级列表（全部）</returns>
        /// <remarks>2013-06-27 邵斌 创建</remarks>
        public override IList<CrCustomerLevel> List()
        {
            return Context.Select<CrCustomerLevel>("*")
                          .From("CrCustomerLevel")
                          .OrderBy("lowerlimit")
                          .QueryMany();
        }

        /// <summary>
        /// 获取单个最高会员等级信息
        /// </summary>
        /// <returns>返回会员等级对象</returns>
        /// <remarks>2013-12-18 苟治国 创建</remarks>
        public override CrCustomerLevel GetCustomerUpperLevel()
        {
            const string strSql = @"select * from (select * from CrCustomerLevel order by UpperLimit desc,LowerLimit desc) where rownum=1";
            var result = Context.Sql(strSql)
                                .QuerySingle<CrCustomerLevel>();
            return result;

        }

        /// <summary>
        /// 获取单个会员等级信息
        /// </summary>
        /// <param name="sysNo">等级编号</param>
        /// <returns>返回会员等级对象</returns>
        /// <remarks>2013-06-27 邵斌 创建</remarks>
        public override CrCustomerLevel GetCustomerLevel(int sysNo)
        {
            return Context.Select<CrCustomerLevel>("*")
                          .From("CrCustomerLevel")
                          .Where("SysNo = @SysNo")
                          .Parameter("SysNo", sysNo)
                          .QuerySingle();
        }
        /// <summary>
        /// 获取单个会员等级信息
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        public override CBCrCustomerLevelImgUrl GetCustomerLevelImgUrl(int sysNo)
        {
            string sql = " cr.sysno=" + sysNo;
            return Context.Select<CBCrCustomerLevelImgUrl>("cr.*,img.CustomerLevelImgUrl")
                          .From("CrCustomerLevel cr left join CrCustomerLevelImgUrl img on cr.sysno=img.CrCustomerLevelSysNo")
                          .Where(sql)
                          .QuerySingle();
        }
        /// <summary>
        /// 根据会员等级ID查询会员等级图标表
        /// </summary>
        /// <param name="CrCustomerLevelSysNo"></param>
        /// <returns></returns>
        public override CBCrCustomerLevelImgUrl GetCrLevelImgUrl(int CrCustomerLevelSysNo)
        {
            return Context.Select<CBCrCustomerLevelImgUrl>("*")
                          .From("CrCustomerLevelImgUrl")
                          .Where("CrCustomerLevelSysNo = @CrCustomerLevelSysNo")
                          .Parameter("CrCustomerLevelSysNo", CrCustomerLevelSysNo)
                          .QuerySingle();
        }
        /// <summary>
        /// 根据条件获取会员等级的列表
        /// </summary>
        /// <param name="CanPayForProduct">惠源币是否可用于支付货款</param>
        /// <param name="CanPayForService">惠源币是否可用于支付服务</param>
        /// <returns>会员等级列表</returns>
        /// <remarks>2013－07-10 苟治国 创建</remarks>
        public override IList<Model.CrCustomerLevel> Seach(int? CanPayForProduct, int? CanPayForService)
        {
            #region sql
            string sqlWhere = @"(@CanPayForProduct=-1 or ccl.CanPayForProduct =@CanPayForProduct)
                        and (@CanPayForService=-1 or ccl.CanPayForService =@CanPayForService)";
            #endregion

            return Context.Select<CrCustomerLevel>("ccl.*")
                                    .From("CrCustomerLevel ccl")
                                    .Where(sqlWhere)
                                    .Parameter("CanPayForProduct", CanPayForProduct)
                                    .Parameter("CanPayForService", CanPayForService)
                                    .OrderBy("ccl.SysNo desc").QueryMany();
        }

        /// <summary>
        /// 会员等级区间比较
        /// </summary>
        /// <param name="sysNo">会员等级编号</param>
        /// <returns>会员等级列表</returns>
        /// <remarks>2013－07-11 苟治国 创建</remarks>
        public override IList<Model.CrCustomerLevel> compare(int sysNo)
        {
            #region sql
            string sqlWhere = "(@sysNo=-1 or ccl.sysNo<>@sysNo)";
            #endregion

            return Context.Select<CrCustomerLevel>("ccl.*")
                        .From("CrCustomerLevel ccl")
                        .Where(sqlWhere)
                        .Parameter("sysNo", sysNo)
                        .QueryMany();
        }

        /// <summary>
        /// 插入会员等级
        /// </summary>
        /// <param name="model"></param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－07-10 苟治国 创建</remarks>
        public override int Insert(Model.CrCustomerLevel model)
        {
            if (model.CreatedDate == DateTime.MinValue)
            {
                model.CreatedDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            if (model.LastUpdateDate == DateTime.MinValue)
            {
                model.LastUpdateDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            var result = Context.Insert<CrCustomerLevel>("CrCustomerLevel", model)
                                .AutoMap(x => x.SysNo)
                                .ExecuteReturnLastId<int>("SysNo");
            return result;
        }
        /// <summary>
        /// 插入会员等级图标表数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override int InsertCrCustomerLevelImgUrl(CrCustomerLevelImgUrl model)
        {
            var result = Context.Insert<CrCustomerLevelImgUrl>("CrCustomerLevelImgUrl", model)
                                .AutoMap(x => x.SysNo)
                                .ExecuteReturnLastId<int>("SysNo");
            return result;
        }
        /// <summary>
        /// 更新会员等级图标表数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override int UpdateCrCustomerLevelImgUrl(CrCustomerLevelImgUrl model)
        {
            int rowsAffected = Context.Update<Model.CrCustomerLevelImgUrl>("CrCustomerLevelImgUrl", model)
                                      .AutoMap(x => x.CrCustomerLevelSysNo)
                                      .Where(x => x.CrCustomerLevelSysNo)
                                      .Execute();
            return rowsAffected;
        }
        /// <summary>
        /// 更新会员等级
        /// </summary>
        /// <param name="model"></param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－07-10 苟治国 创建</remarks>
        public override int Update(Model.CrCustomerLevel model)
        {
            int rowsAffected = Context.Update<Model.CrCustomerLevel>("CrCustomerLevel", model)
                                      .AutoMap(x => x.SysNo)
                                      .Where(x => x.SysNo)
                                      .Execute();
            return rowsAffected;
        }
        /// <summary>
        /// 更新等级图标
        /// </summary>
        /// <param name="CrCustomerLevelSysNo"></param>
        /// <param name="CrCustomerLevelImgUrl"></param>
        /// <returns></returns>
        public override int UpdateCrCustomerLevelImgUrl(int CrCustomerLevelSysNo, string CrCustomerLevelImgUrl)
        {
            string strSql = "update CrCustomerLevelImgUrl set CustomerLevelImgUrl='" + CrCustomerLevelImgUrl + "' where CrCustomerLevelSysNo=" + CrCustomerLevelSysNo;
            var result = Context.Sql(strSql)
                                .Execute();
            return result;

        }

    }
}
