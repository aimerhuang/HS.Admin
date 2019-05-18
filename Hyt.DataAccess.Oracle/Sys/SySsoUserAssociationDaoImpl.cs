using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Sys;
using Hyt.Model.Generated;

namespace Hyt.DataAccess.Oracle.Sys
{
    public class SySsoUserAssociationDaoImpl : ISySsoUserAssociationDao
    {
        /// <summary>
        /// 插入SSO系统用户关联表
        /// </summary>
        /// <param name="model">插入的数据模型</param>
        /// <returns>返回插入的SysNo</returns>
        /// <remarks>2014－10-14 谭显锋 创建</remarks>
        public override int Insert(SySsoUserAssociation model)
        {
            return Context.Insert<SySsoUserAssociation>("SySsoUserAssociation", model)
                            .AutoMap(x => x.SysNo)
                            .ExecuteReturnLastId<int>("Sysno");
        }

        /// <summary>
        /// 通过会员编号获得客户关系实体
        /// </summary>
        /// <param name="costomerSysNo">会员编号</param>
        /// <returns>客户关系o</returns>
        /// <remarks>2014－10-14 谭显锋 创建 创建</remarks>
        public override SySsoUserAssociation GetByUserSysNo(int UserSysNo)
        {
            return Context.Select<SySsoUserAssociation>("*")
                 .From(@"SySsoUserAssociation")
                 .Where("UserSysNo=@UserSysNo")
                 .Parameter("UserSysNo", UserSysNo)
                 .QuerySingle();
        }

        /// <summary>
        /// 通过SSOID获得客户关系实体
        /// </summary>
        /// <param name="ssoID">SSOID</param>
        /// <returns>客户关系o</returns>
        /// <remarks>2014－10-14 谭显锋 创建 创建</remarks>
        public override SySsoUserAssociation GetBySsoId(int ssoID)
        {
            return Context.Select<SySsoUserAssociation>("*")
            .From(@"SySsoUserAssociation")
            .Where("SsoId=@SsoId")
            .Parameter("SsoId", ssoID)
            .QuerySingle();
        }
    }
}
