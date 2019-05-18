using System.ComponentModel;

namespace Hyt.Model.WorkflowStatus
{
    /// <summary>
    /// 系统状态
    /// </summary>
    /// <remarks>2013-09-10 吴文强 创建</remarks>
    public class SystemStatus
    {

        /// <summary>
        /// 系统用户状态
        /// 数据表:SyUser 字段:Status
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 系统用户状态
        {
            启用 = 1,
            禁用 = 0,
        }

        /// <summary>
        /// 用户组状态
        /// 数据表:SyUserGroup 字段:Status
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 用户组状态
        {
            启用 = 1,
            禁用 = 0,
        }

        /// <summary>
        /// 是否系统分组
        /// 数据表:SyUserGroup 字段:IsSystem
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 是否系统分组
        {
            是 = 1,
            否 = 0,
        }

        /// <summary>
        /// 菜单状态
        /// 数据表:SyMenu 字段:Status
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 菜单状态
        {
            启用 = 1,
            禁用 = 0,
        }

        /// <summary>
        /// 是否导航栏显示
        /// 数据表:SyMenu 字段:IsNavigate
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 是否导航栏显示
        {
            是 = 1,
            否 = 0,
        }

        /// <summary>
        /// 授权来源
        /// 数据表:SyPermission 字段:Source
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 授权来源
        {
            系统用户 = 10,
            用户组 = 20,
        }

        /// <summary>
        /// 授权目标
        /// 数据表:SyPermission 字段:Target
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 授权目标
        {
            菜单 = 10,
            角色 = 20,
            权限 = 30,
        }

        /// <summary>
        /// 权限状态
        /// 数据表:SyPrivilege 字段:Status
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 权限状态
        {
            启用 = 1,
            禁用 = 0,
        }

        /// <summary>
        /// 角色状态
        /// 数据表:SyRole 字段:Status
        /// </summary>
        /// <remarks>2013-08-06 吴文强 创建</remarks>
        public enum 角色状态
        {
            启用 = 1,
            禁用 = 0,
        }

        /// <summary>
        /// 任务对象类型
        /// 数据表:SyJobPool 字段:TaskType
        /// </summary>
        /// <remarks>2013-08-06 吴文强 创建</remarks>
        /// <remarks>2014-02-21 邵  斌 修改：添加短信咨询通知类型</remarks>
        public enum 任务对象类型
        {
            客服订单审核 = 10,
            客服订单提交出库 = 15,
            商品评论审核 = 50,
            商品评论回复审核 = 55,
            商品晒单审核 = 60,
            通知 = 70
        }

        /// <summary>
        /// 任务池状态
        /// 数据表:SyJobPool 字段:Status
        /// </summary>
        /// <remarks>2013-08-06 吴文强 创建</remarks>
        public enum 任务池状态
        {
            待分配 = 10,
            待处理 = 20,
            处理中 = 30,
            已锁定 = 40,
        }

        /// <summary>
        /// 任务池优先级
        /// 数据表:SyJobPool 字段:Priority
        /// </summary>
        /// <remarks>2014-01-10 余勇 创建</remarks>
        public enum 任务池优先级
        {
            普通 = 2,
            紧急 = 3,
            特急 = 4,
        }

        /// <summary>
        /// 角色状态
        /// 数据表:SyJobSmsConfig 字段:Status
        /// </summary>
        /// <remarks>2014-01-10 余勇 创建</remarks>
        public enum 任务池短信设置状态
        {
            启用 = 1,
            禁用 = 0,
        }

        /// <summary>
        /// 锁定任务自动解锁状态
        /// 数据表:SyLockJob 字段:Status
        /// </summary>
        /// <remarks>2014-08-05 余勇 创建</remarks>
        public enum 锁定任务状态
        {
            自动解锁 = 1,
            手动解锁 = 0,
        }

        /// <summary>
        /// Eas同步日志状态
        /// 数据表:EasSyncLog 字段:Status
        /// </summary>
        /// <remarks>2013-10-22 黄志勇 创建</remarks>
        public enum Eas同步日志状态
        {
            等待同步 = 5,
            成功 = 1,
            失败 = 0,
            作废 = -1
        }

        /// <summary>
        /// 利嘉同步日志状态
        /// 数据表:LiJiaSyncLog 字段:Status
        /// </summary>
        /// <remarks>2013-11-10 罗熙 创建</remarks>
        public enum LiJia同步日志状态
        {
            等待同步 = 5,
            成功 = 1,
            失败 = 0,
            作废 = -1
        }

        /// <summary>
        /// 兴业嘉同步日志状态
        /// 数据表:XingYeSyncLog 字段:Status
        /// </summary>
        /// <remarks>2018-03-22 罗熙 创建</remarks>
        public enum XingYe同步日志状态
        {
            等待同步 = 5,
            成功 = 1,
            失败 = 0,
            作废 = -1
        }

        /// <summary>
        /// 系统配置类型
        /// 数据表：SyConfig 字段：TypeId
        /// </summary>
        /// <remarks>2014-01-15 陶辉 创建</remarks>
        public enum 系统配置类型
        {
            常规配置 = 1,
            支付配置 = 2,
            图片服务器配置 = 3,
            升舱配置 = 4,
            ERP配置 = 5,
            App商城配置 = 6,
            升舱订单自动处理配置 = 7,
            升舱订单自定义命令 = 8
        }
        /// <summary>
        /// 任务消息类型
        /// 数据表：SyJobMessage 字段：MessageType
        /// </summary>
        public enum 任务消息类型
        {
            订单创建消息 = 10,
            订单推送门店消息=20
        }
    }
}
