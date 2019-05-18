namespace Hyt.Model.SystemPredefined
{
    /// <summary>
    /// 用户组 预设值
    /// 数据表：SyUserGroup
    /// </summary>
    /// <remarks>2013-06-18 吴文强 创建</remarks>
    public static class UserGroup
    {
        /// <summary>
        /// 业务员组系统编号
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public static int 业务员组 { get { return 1; } }

        /// <summary>
        /// 客服组系统编号
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public static int 客服组 { get { return 2; } }

        /// <summary>
        /// 包含所有仓库用户组系统编号
        /// 该用户组成员包含所有仓库权限 
        /// </summary>
        /// <remarks>2013-07-04 吴文强 创建</remarks>
        public static int 包含所有仓库的用户组 { get { return 3; } }

        /// <summary>
        /// 包含所有分销商用户组系统编号
        /// 该用户组成员包含所有分销商权限 
        /// </summary>
        /// <remarks>2015-12-19 王耀发 创建</remarks>
        public static int 包含所有分销商的用户组 { get { return 12; } }

        /// <summary>
        /// 包含代理商用户组系统编号
        /// 该用户组成员包含代理商
        /// </summary>
        /// <remarks>2016-3-17 王耀发 创建</remarks>
        public static int 包含代理商的用户组 { get { return 4; } }

        /// <summary>
        /// 包含分销商用户组系统编号
        /// 该用户组成员包含分销商
        /// </summary>
        /// <remarks>2016-3-17 王耀发 创建</remarks>
        public static int 包含分销商的用户组 { get { return 13; } }
    }
}