using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Extra.UpGrade.UpGrades;
using Hyt.Model.WorkflowStatus;

namespace Extra.UpGrade.Provider
{
    /// <summary>
    /// 升舱策略
    /// </summary>
    /// <remarks>2016-07-16 杨浩 创建</remarks>
    public class UpGradeProvider
    {
        private static readonly Dictionary<int, IUpGrade> upGradeList = new Dictionary<int, IUpGrade>(); //类实例缓存池
        private static readonly object lockHelper = new object();
        /// <summary>
        /// 返回接口实例
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IUpGrade GetInstance(int type)
        {
            IUpGrade instance = null;

            if (!upGradeList.ContainsKey(type))
            {
                lock (lockHelper)
                {
                    if (!upGradeList.ContainsKey(type))
                    {
                        //根据当前授权用户商城类型选择实现类
                        switch (type)
                        {
                            case (int)DistributionStatus.商城类型预定义.天猫商城:
                            case (int)DistributionStatus.商城类型预定义.淘宝分销:
                                instance = new TaobaoUpGrade();
                                break;
                            case (int)DistributionStatus.商城类型预定义.拍拍网购:
                                instance = new PaipaiUpGrade();
                                break;
                            case (int)DistributionStatus.商城类型预定义.一号店:
                                instance = new YihaodianUpGrade();
                                break;
                            case (int)DistributionStatus.商城类型预定义.海带网:
                                instance = new HaiDaiUpGrade();
                                break;
                            case (int)DistributionStatus.商城类型预定义.有赞:
                                instance = new YouZanUpGrade();
                                break;
                            case (int)DistributionStatus.商城类型预定义.京东商城:
                                instance=new JingDongUpGrade();
                                break;
                            case(int)DistributionStatus.商城类型预定义.格格家:
                                instance = new GeGeJiaUpGrade();
                                break;
                            case (int)DistributionStatus.商城类型预定义.国美在线:
                                instance = new GuoMeiUpGrade();
                                break;
                            case (int)DistributionStatus.商城类型预定义.苏宁易购:
                                instance = new SUNINGUpGrade();
                                break;
                            case (int)DistributionStatus.商城类型预定义.海拍客:
                                instance = new HipacUpGrade();
                                break;
                            case (int)DistributionStatus.商城类型预定义.国内货栈:
                                instance = new B2BUpGrade();
                                break;
                            default:
                                throw new Exception(string.Format("\"{0}\"不是已知的店铺类型,接口可能未实现!", type));
                        }
                        upGradeList.Add(type, instance);
                    }
                }
            }
            else
            {
                instance = upGradeList[type];
            }
               
            return instance;
        }
    }
}
