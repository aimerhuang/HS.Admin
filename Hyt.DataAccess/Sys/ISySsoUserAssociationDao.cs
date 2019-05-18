using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model.Generated;

namespace Hyt.DataAccess.Sys
{
    /// <summary>
    /// SSO系统用户关联抽象类
    /// </summary>
    /// <remarks>2014－06-26 余勇 创建</remarks>
    public abstract class ISySsoUserAssociationDao : DaoBase<ISySsoUserAssociationDao>
    {
        /// <summary>
        /// 插入SSO系统用户关联表
        /// </summary>
        /// <param name="model">插入的数据模型</param>
        /// <returns>返回插入的SysNo</returns>
        /// <remarks>2014－10-14 谭显锋 创建</remarks>
        public abstract int Insert(SySsoUserAssociation model);

        /// <summary>
        /// 通过会员编号获得客户关系实体
        /// </summary>
        /// <param name="costomerSysNo">会员编号</param>
        /// <returns>客户关系o</returns>
        /// <remarks>2014－10-14 谭显锋 创建 创建</remarks>
        public abstract SySsoUserAssociation GetByUserSysNo(int UserSysNo);

        /// <summary>
        /// 通过SSOID获得客户关系实体
        /// </summary>
        /// <param name="ssoID">SSOID</param>
        /// <returns>客户关系o</returns>
        /// <remarks>2014－10-14 谭显锋 创建 创建</remarks>
        public abstract SySsoUserAssociation GetBySsoId(int ssoID);
    }
}
