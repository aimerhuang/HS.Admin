using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Infrastructure.Memory;
using Hyt.Util;

namespace Hyt.BLL.OrderRule
{
    /// <summary>
    /// 自定义命令 区分大小写
    /// </summary>
    public class Command_Var : ICommand
    {
        private readonly string rulename = "Var";

        public string Key { get; set; }

        public override bool Result(OrderData orderData)
        {
            //根据Key 从缓存中获取它的配置
            var configModel = MemoryProvider.Default.Get(string.Format(KeyConstant.SysConfigInfo, Key), () => BLL.Sys.SyConfigBo.Instance.GetModel(Key, Hyt.Model.WorkflowStatus.SystemStatus.系统配置类型.升舱订单自定义命令));
            if (configModel == null || string.IsNullOrEmpty(configModel.Value)) return false;
            var md5Key = EncryptionUtil.EncryptWithMd5(configModel.Value);
            var command = MemoryProvider.Default.Get(string.Format(KeyConstant.OrderRuleCommand, md5Key), () => OrderEngine.Instance.ParseCommand(configModel.Value));
            return command.Result(orderData);
        }

        public override ICommand Parse(string command)
        {
            if (base.IsMatch(rulename, command) == false) return null;
            var arg = this.GetArgument(rulename, command);
            if (string.IsNullOrEmpty(arg) == true) throw new ArgumentException("Var自定义命令 参数不能为null");
            return new Command_Var()
            {
                Key = arg
            };


            //return this.IsContainCommand(rulename, command) ? new Command_百城当日达() : null;
        }
    }
}
