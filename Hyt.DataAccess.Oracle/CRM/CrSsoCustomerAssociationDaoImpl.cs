using Hyt.DataAccess.CRM;
using Hyt.Model;

namespace Hyt.DataAccess.Oracle.CRM
{
    /// <summary>
    /// SSO客户关系数据访问实现类
    /// </summary>
    /// <remarks>2014－06-26 余勇 创建</remarks>
    public class CrSsoCustomerAssociationDaoImpl : ICrSsoCustomerAssociationDao
    {
        /// <summary>
        /// 插入SSO客户关系表
        /// </summary>
        /// <param name="model">插入的数据模型</param>
        /// <returns>返回插入的SysNo</returns>
        /// <remarks>2014－06-26 余勇 创建</remarks>
        public override int Insert(CrSsoCustomerAssociation model)
        {
            return Context.Insert<CrSsoCustomerAssociation>("CrSsoCustomerAssociation", model)
                            .AutoMap(x => x.SysNo)
                            .ExecuteReturnLastId<int>("Sysno");
        }

        /// <summary>
        /// 通过会员编号获得客户关系实体
        /// </summary>
        /// <param name="costomerSysNo">会员编号</param>
        /// <returns>客户关系o</returns>
        /// <remarks>2014－06-26 余勇 创建</remarks>
        public override CrSsoCustomerAssociation GetByCustomerSysNo(int costomerSysNo)
        {
            return Context.Select<CrSsoCustomerAssociation>("*")
                       .From(@"CrSsoCustomerAssociation")
                       .Where("CustomerSysNo=@CustomerSysNo")
                       .Parameter("CustomerSysNo", costomerSysNo)
                       .QuerySingle();
        }
        /// <summary>
        /// 通过SSOID获得客户关系实体
        /// </summary>
        /// <param name="ssoID">SSOID</param>
        /// <returns>客户关系o</returns>
        /// <remarks>2014－06-26 余勇 创建</remarks>
        public override CrSsoCustomerAssociation GetBySsoId(int ssoID)
        {
            return Context.Select<CrSsoCustomerAssociation>("*")
           .From(@"CrSsoCustomerAssociation")
           .Where("SsoId=@SsoId")
           .Parameter("SsoId", ssoID)
           .QuerySingle();
        }
    }
}
