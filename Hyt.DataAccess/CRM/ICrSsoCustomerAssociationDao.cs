using Hyt.DataAccess.Base;
using Hyt.Model;

namespace Hyt.DataAccess.CRM
{
    /// <summary>
    /// SSO客户关系抽象类
    /// </summary>
    /// <remarks>2014－06-26 余勇 创建</remarks>
    public abstract class ICrSsoCustomerAssociationDao : DaoBase<ICrSsoCustomerAssociationDao>
    {
        /// <summary>
        /// 插入SSO客户关系表
        /// </summary>
        /// <param name="model">插入的数据模型</param>
        /// <returns>返回插入的SysNo</returns>
        /// <remarks>2014－06-26 余勇 创建</remarks>
        public abstract int Insert(CrSsoCustomerAssociation model);

        /// <summary>
        /// 通过会员编号获得客户关系实体
        /// </summary>
        /// <param name="costomerSysNo">会员编号</param>
        /// <returns>客户关系o</returns>
        /// <remarks>2014－06-26 余勇 创建</remarks>
        public abstract CrSsoCustomerAssociation GetByCustomerSysNo(int costomerSysNo);

        /// <summary>
        /// 通过SSOID获得客户关系实体
        /// </summary>
        /// <param name="ssoID">SSOID</param>
        /// <returns>客户关系o</returns>
        /// <remarks>2014－06-26 余勇 创建</remarks>
        public abstract CrSsoCustomerAssociation GetBySsoId(int ssoID);
    }
}
