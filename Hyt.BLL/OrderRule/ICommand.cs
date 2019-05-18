using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Util.Validator.Rule;

namespace Hyt.BLL.OrderRule
{
    /// <summary>
    /// 命令
    /// </summary>
    public abstract class ICommand 
    {
        public abstract bool Result(OrderData orderData);

        /// <summary>
        /// 解析命令并返回ICommand对象
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public abstract ICommand Parse(string command);


        /// <summary>
        /// 命令标识和命令字符串是否匹配
        /// </summary>
        /// <param name="commandIdentity"></param>
        /// <param name="commandString"></param>
        /// <returns></returns>
        protected bool IsMatch(string commandIdentity, string commandString) {
            commandString = commandString.Trim();
            return commandString.StartsWith(commandIdentity);
        }

        /// <summary>
        /// 返回命令的参数
        /// </summary>
        /// <param name="commandIdentity">命令标识.</param>
        /// <param name="commandString">命令字符串.</param>
        /// <returns></returns>
        protected string GetArgument(string commandIdentity, string commandString)
        {
            commandString = commandString.Trim().Replace(":", "").Replace("：", "").Replace(" ","");
            if (commandString.Contains(" ") == true) throw new ArgumentException("命令【" + commandString + "】包含空格");
            if (commandString.StartsWith(commandIdentity) == true)
            {
                return commandString.Substring(commandIdentity.Length);
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 是否包含命令
        /// </summary>
        /// <param name="commandIdentity">命令标识.</param>
        /// <param name="commandString">命令字符串.</param>
        /// <returns>true或false</returns>
        protected bool IsContainCommand(string commandIdentity, string commandString)
        {
            commandString = commandString.Trim().Replace(":", "").Replace("：", "").Replace(" ", "");
            if (commandString.Contains(" ") == true) throw new ArgumentException("命令【" + commandString + "】包含空格");
            return commandString.StartsWith(commandIdentity) == true;
        }


        /// <summary>
        /// 返回命令的参数
        /// </summary>
        /// <param name="commandIdentity">命令标识.</param>
        /// <param name="commandString">命令字符串.</param>
        /// <returns></returns>
        protected string[] GetArgumentKeys(string commandIdentity, string commandString)
        {
            var arg = GetArgument(commandIdentity,commandString);
            if (!string.IsNullOrEmpty(arg))
            {
                return arg.Split('、').Select(m => m.Trim()).ToArray();
            }
            else
            {
                return null;
            }
        }
    }


    public class Command_OR : ICommand
    {
        public Command_OR(ICommand c1, ICommand c2) {
            this.c1 = c1;
            this.c2 = c2;
        }
        private ICommand c1 { get; set; }
        private ICommand c2 { get; set; }

        public override bool Result(OrderData orderData)
        {
            return c1.Result(orderData) || c2.Result(orderData);
        }

        public override ICommand Parse(string command)
        {
            throw new NotImplementedException("Command_OR 无Parse方法");
        }
    }

    public class Command_AND : ICommand
    {
        public Command_AND(ICommand c1, ICommand c2)
        {
            this.c1 = c1;
            this.c2 = c2;
        }
        private ICommand c1 { get; set; }
        private ICommand c2 { get; set; }

        public override bool Result(OrderData orderData)
        {
            return c1.Result(orderData) && c2.Result(orderData);
        }

        public override ICommand Parse(string command)
        {
            throw new NotImplementedException("Command_AND 无Parse方法");
        }
    }

    public class Command_NOT : ICommand
    {
        public Command_NOT(ICommand c1)
        {
            this.c1 = c1;
        }
        private ICommand c1 { get; set; }

        public override bool Result(OrderData orderData)
        {
            return !c1.Result(orderData);
        }
        public override ICommand Parse(string command)
        {
            throw new NotImplementedException("Command_NOT 无Parse方法");
        }
    }    

}
