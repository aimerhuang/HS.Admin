using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IronPython.Modules;
using PanGu.Framework;

namespace Hyt.BLL.OrderRule
{
    /// <summary>
    /// 处理主要流程
    /// </summary>
    public class OrderEngine
    {
        #region singleton
        public static OrderEngine Instance
        {
            get
            {
                return Nested.Instance;
            }
        }
        class Nested
        {
            static Nested() { }
            internal static readonly OrderEngine Instance = new OrderEngine();
        }
        #endregion

        private IList<ICommand> _commands = new List<ICommand>();
        private string[] keys = new string[] { "not", "and", "or", "(", ")" };

        private OrderEngine() {
            //如果不想手动加可以通过规则用反射去加载            
            _commands.Add(new Command_订单金额大于());
            _commands.Add(new Command_商品数量大于());
            _commands.Add(new Command_顾客留言包含());
            _commands.Add(new Command_收货地区());
            _commands.Add(new Command_对内备注包含());
            _commands.Add(new Command_购买了商品());
            _commands.Add(new Command_经销商城编号是());
            _commands.Add(new Command_经销商城名称是());
            _commands.Add(new Command_只购买了商品());
            _commands.Add(new Command_收货地区和仓库匹配());
            _commands.Add(new Command_百城当日达());
            _commands.Add(new Command_第三方快递());
            _commands.Add(new Command_仓库名称是());
            _commands.Add(new Command_仓库有库存());
            _commands.Add(new Command_非自营仓库());
            _commands.Add(new Command_自营仓库());
            _commands.Add(new Command_配送方式编号是());
            _commands.Add(new Command_预付订单必须完成支付());
            _commands.Add(new Command_Var());
            _commands.Add(new Command_千城当日达());
            _commands.Add(new Command_百度与高德数据一致());
            _commands.Add(new Command_商品名称包含());
            _commands.Add(new Command_订单金额小于());//添加订单金额小于
            _commands.Add(new Command_与高德数据一致());//与高德数据一致
            _commands.Add(new Command_有顾客留言());//顾客有留言不自动审单
            _commands.Add(new Command_高德千城当日达());//高德千城当日达
            _commands.Add(new Command_当日达配送商品());//当日达配送商品
            _commands.Add(new Command_订单来源编号是());//订单来源编号是积分商城的不自动审单
            _commands.Add(new Command_支付状态是());//未支付的不自动审单
        }

        public IList<ICommand> AllCommands
        {
            get
            {
                return _commands;
            }
        }

        /// <summary>
        /// 需要处理的事情
        /// </summary>
        public IList<IThing> handlerThings = new List<IThing>();

        /// <summary>
        /// 业务上调用此方法
        /// </summary>
        /// <param name="orderData"></param>
        public void HandlerUpGradesOrder(OrderData orderData) {
            if (handlerThings.Count < 1)
            {
                //从缓存中获取 订单升舱时 需要处理的事情 
                var syconfig = Sys.SyConfigBo.Instance.GetModel("global", Model.WorkflowStatus.SystemStatus.系统配置类型.升舱订单自动处理配置);
                if (syconfig != null) {
                    handlerThings = GetThings(syconfig.Value);                    
                }
            }

            foreach (var thing in handlerThings) {
                thing.Do(orderData);
            }
            
        }

        /// <summary>
        /// 业务上调用此方法  对[升舱订单自动处理配置]进行修改之后调用此方法 清除缓存 
        /// </summary>
        public void Clear() {
            handlerThings.Clear();
            //工具之间共用一套配置，所以修改[升舱订单自动处理配置]之后需要在memcached中删除约定的key,升舱工具中会根据key做相当的业务逻辑处理。
            Hyt.Infrastructure.Caching.CacheManager.Instance.Delete("Hyt.BLL.OrderRule.OrderEngine_Config");
        }

 

        /// <summary>
        /// 获取需要处理的事情
        /// </summary>
        /// <param name="thingsConfig"></param>
        /// <returns></returns>
        /// <remarks>
        /// 保存配置时调用此方法 如果出现异常，表示配置有错。提示异常消息
        /// </remarks>
        public IList<IThing> GetThings(string thingsConfig) {
            var configs = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<ThingModel>>(thingsConfig);
            IList<IThing> thingList = new List<IThing>();
            foreach (var config in configs) {
                switch (config.Name) { 
                    case "升舱订单自动处理":
                        thingList.Add(new Thing_升舱订单自动处理(config.Rule));
                        break;
                    case "非升舱订单自动处理":
                        thingList.Add(new Thing_非升舱订单自动处理(config.Rule));
                        break;
                    case "给升舱订单增加赠品":
                        throw new Exception("未实现：" + config.Name);
                        //todo:未实现
                        //break;
                    default:
                        throw new Exception("未实现："+ config.Name);
                }
            }
            return thingList;
        }
                

        /// <summary>
        /// 解析命令集合  不同命令之间用“,”间隔
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public IList<ICommand> ParseCommands(string commandsString) {
            commandsString = Format(commandsString);
            if (!string.IsNullOrEmpty(commandsString))
            {
                IList<ICommand> list = new List<ICommand>();
                foreach (var command in commandsString.Split(','))
                {
                    list.Add(ParseCommand(command));
                }
                return list;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 解析命令
        /// </summary>
        /// <param name="commandString"></param>
        /// <returns></returns>
        public ICommand ParseCommand(string commandString) {

            Stack<ICommand> stack = new Stack<ICommand>();

            foreach (var expression in ToPostfixExpression(commandString))
            {
                switch (expression.ToLower())
                {
                    case "not":
                        stack.Push(new Command_NOT(stack.Pop()));
                        break;
                    case "and":
                        stack.Push(new Command_AND(stack.Pop(), stack.Pop()));
                        break;
                    case "or":
                        stack.Push(new Command_OR(stack.Pop(), stack.Pop()));
                        break;
                    default:
                        bool flag=false;
                        foreach (var command in _commands) {
                            var c = command.Parse(expression);
                            if (c != null) {
                                stack.Push(c);
                                flag = true;
                                break;
                            }
                        }
                        if (!flag)
                        {
                            throw new NotImplementedException("规则参数:"+expression+"尚未加入到规则列表");
                        }
                        break;
                }
            }
            return stack.Pop();
        }

        public IList<string> ToPostfixExpression(string command) {
            var commandNodes = Format(command).Split(' ');
            Array.ForEach(commandNodes, (p) =>
            {
                p = p.Trim();
            });
            Stack<string> stackCommand = new Stack<string>();
            Stack<string> stackOper = new Stack<string>();
            stackOper.Push("#");
            foreach (var node in commandNodes)
            {
                if (string.IsNullOrEmpty(node) == true) continue;
                if (IsKey(node) == false)
                {
                    stackCommand.Push(node);
                    continue;
                }
                if (node.ToLower() == ")")
                {
                    bool flag = true;
                    while (flag)
                    {
                        if (stackOper.Peek() == "(")
                        {
                            stackOper.Pop();
                            flag = false;
                        }
                        else
                        {
                            stackCommand.Push(stackOper.Pop());
                        }
                    }
                    continue;
                }
                if (node.Trim().ToLower() == "(")
                {
                    stackOper.Push(node);
                    continue;
                }

                var nodePri = getPri(node);
                if (nodePri >= getPri(stackOper.Peek()))
                {
                    stackOper.Push(node);
                }
                else
                {
                    bool flag = true;
                    while (flag)
                    {
                        if (stackOper.Peek() == "(") {
                            flag = false;
                            continue;
                        }
                        //这里有疑问：应为>=吧？ 小于就直接入stackOper而不进入stackCommand 余勇
                        if (getPri(stackOper.Peek()) >= nodePri)
                        {
                            stackCommand.Push(stackOper.Pop());
                        }
                        else
                        {
                            //stackCommand.Push(stackOper.Pop());
                            flag = false;
                        }
                    }
                    stackOper.Push(node);
                }
            }

            while (stackOper.Peek() != "#") {
                stackCommand.Push(stackOper.Pop());
            }

            var result = stackCommand.ToList();
            result.Reverse();
            return result;
        }
        
        public bool IsKey(string command) {
            if (command.Contains(" ")) return false;
            return keys.Contains(command.ToLower());            
        }

        private int getPri(string oper)
        {
            switch (oper.Trim().ToLower())
            {
                case "(":
                    return -1;
                case ")":
                    return -1;
                case "not":
                    return 3;
                case "and":
                    return 2;
                case "or":
                    return 1;
                case "#":
                    return 0;
                default:
                    return -2;
            }
        }
        
        public string Format(string command){            
            if (!string.IsNullOrEmpty(command))
            {
                command = command.Replace("(", " ( ").Replace(")", " ) ");
                command = System.Text.RegularExpressions.Regex.Replace(command, "\\s+\\(\\s+", " ( ");
                command = System.Text.RegularExpressions.Regex.Replace(command, "\\s+\\)\\s+", " ) ");
                return command.Trim();
            }
            else
            {
                return string.Empty;
            }
        }
          

    }
}
