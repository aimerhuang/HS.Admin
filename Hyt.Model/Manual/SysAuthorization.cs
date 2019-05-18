using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model.Transfer;
using Hyt.Model.Transport;

namespace Hyt.Model
{
    /// <summary>
    /// 系统用户权限认证信息
    /// </summary>
    /// <remarks>
    /// 2013-6-26 杨浩 创建
    /// </remarks>
    [Serializable]
    public class SysAuthorization
    {
        /// <summary>
        /// 当前用户基本信息
        /// </summary>
        public SyUser Base { get; set; }

        /// <summary>
        /// 我的菜单列表
        /// </summary>
        public IList<SyMenu> MyMenuList { get; set; }

        /// <summary>
        /// 当前用户菜单列表
        /// </summary>
        public IList<SyMenu> MenuList { get; set; }

        /// <summary>
        /// 当前用户权限列表
        /// </summary>
        public IList<SyPrivilege> PrivilegeList { get; set; }

        /// <summary>
        /// 获取当前用户所能操作的所有仓库
        /// </summary>
        public IList<WhWarehouse> Warehouses { get; set; }

        /// <summary>
        /// 当前用户对应的分销商
        /// </summary>
        public CBDsDealer Dealer { get; set; }

        /// <summary>
        /// 获取当前用户所能操作的所有分销商
        /// </summary>
        public IList<DsDealer> Dealers { get; set; }
        /// <summary>
        /// 是否有所有仓库权限
        /// </summary>
        public bool IsAllWarehouse { get; set; }
        /// <summary>
        /// 是否绑定经销商
        /// </summary>
        public bool IsBindDealer { get; set; }
        /// <summary>
        /// 是否绑定所有经销商
        /// </summary>
        public bool IsBindAllDealer { get; set; }
        /// <summary>
        /// 经销商创建人
        /// </summary>
        public int DealerCreatedBy { get; set; }
        /// <summary>
        /// 是代理商
        /// </summary>
        public bool IsAgent { get; set; }


        /// <summary>
        /// 转运系统客户
        /// </summary>
        public bool IsWhCustomer { get; set; }
        /// <summary>
        /// 关联的转运系统客户
        /// </summary>
        public DsWhCustomer WhCustomer { get; set; }
        /// <summary>
        /// 代理经销商所管理的客户集合
        /// </summary>
        public List<DsWhCustomer> WhCustomers { get; set; }
    }
}
