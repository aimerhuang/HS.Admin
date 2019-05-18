using Extra.Express.Express;
using Hyt.Model.WorkflowStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.Express.Provider
{
    /// <summary>
    /// 快递策略
    /// </summary>
    /// <remarks>2017-12-13 杨浩 创建</remarks>
    public class ExpressProvider
    {
        private static readonly Dictionary<int,IExpress> expressList = new Dictionary<int, IExpress>(); //类实例缓存池
        private static readonly object lockHelper = new object();
        /// <summary>
        /// 返回接口实例
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IExpress GetInstance(int type)
        {
            IExpress instance = null;

            if (!expressList.ContainsKey(type))
            {
                lock (lockHelper)
                {
                    if (!expressList.ContainsKey(type))
                    {                     
                        switch (type)
                        {
                            case (int)ExpressStatus.快递类型预定义.圆通快递:
                                instance = new YtExpress();
                                break;   
                            case (int)ExpressStatus.快递类型预定义.顺丰快递:
                                instance = new YtExpress();
                                break;
                            case (int)ExpressStatus.快递类型预定义.快递100:
                                //instance = new Kd100Express();
                                break;
                            default:
                                throw new Exception(string.Format("\"{0}\"不是已知的快递类型,接口可能未实现!", type));
                        }
                        expressList.Add(type, instance);
                    }
                }
            }
            else
            {
                instance = expressList[type];
            }

            return instance;
        }
    }
}
