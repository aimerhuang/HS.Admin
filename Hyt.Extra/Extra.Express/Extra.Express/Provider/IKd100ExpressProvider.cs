using Extra.Express.Express;
using Hyt.Model.WorkflowStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.Express.Provider
{
   public class IKd100ExpressProvider
    {
        private static readonly Dictionary<int,IKd100Express> expressList = new Dictionary<int, IKd100Express>(); //类实例缓存池
        private static readonly object lockHelper = new object();
        /// <summary>
        /// 返回接口实例
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <remarks>2017-12-14 廖移凤 创建</remarks>
        public static IKd100Express GetInstance(int type)
        {
            IKd100Express instance = null;

            if (!expressList.ContainsKey(type))
            {
                lock (lockHelper)
                {
                    if (!expressList.ContainsKey(type))
                    {                     
                        switch (type)
                        {
                            
                            case (int)ExpressStatus.快递类型预定义.快递100:
                                instance = new Kd100Express();
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
