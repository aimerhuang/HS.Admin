using System;
using System.Collections.Generic;
using System.Data;

namespace Hyt.Model
{
    /// <summary>
    /// 基础返回json结果
    /// </summary>
    /// <remarks>
    /// 2013-03-12 何方 创建
    /// 2013-06-18何方  增加 bool Status;
    /// </remarks>
    [Serializable]
    public class Result : BaseEntity
    {
        /// <summary>
        /// 是否
        /// </summary>
        public bool Status;

        /// <summary>
        /// 状态代码
        /// </summary>
        public int StatusCode;

        /// <summary>
        /// 消息
        /// </summary>
        public String Message = string.Empty;

        /// <summary>
        /// 错误码
        /// </summary>
        public String errCode = string.Empty;

    }

    /// <summary>
    /// 带数据的json返回结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks> 2013-3-12 何方 创建 </remarks>
    public class Result<T> : Result
    {
        /// <summary>
        /// 数据
        /// </summary>
        public T Data;
    }

    /// <summary>
    /// 带数据的json返回结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks> 2013-3-12 何方 创建 </remarks>
    public class ResultList<T> : Result
    {
        /// <summary>
        /// 数据
        /// </summary>
        public List<T> Data;
    }

    /// <summary>
    /// 带数据的json返回结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks> 2013-3-12 何方 创建 </remarks>
    public class Resuldt : Result
    {
        /// <summary>
        /// 数据
        /// </summary>
        public DataTable Data;

        /// <summary>
        /// 数据
        /// </summary>
        public List<CBSimplePdProduct> listModel;
    }


    /// <summary>
    /// 带数据的json返回结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks> 2013-3-12 何方 创建 </remarks>
    public class Resuldt<T> : Result
    {
        /// <summary>
        /// 数据
        /// </summary>
        public DataTable Data;

        /// <summary>
        /// 数据
        /// </summary>
        public List<T> listModel;
    }

    /// <summary>
    /// App
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks> 2013-8-1 杨浩 创建 </remarks>
    public class ResultPager<T> : Result
    {
        /// <summary>
        /// 针对App请求,是否有更多数据
        /// </summary>
        public bool HasMore { get; set; }
        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 分页总数
        /// </summary>
        public int PageCount { get; set; }
        /// <summary>
        /// 总记录数
        /// </summary>
        public int RecCount { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public T Data;
    }

    /// <summary>
    /// 日期时间范围对象
    /// </summary>
    /// <remarks>
    /// 2013-06-18 郑荣华 创建
    /// </remarks>
    [Serializable]
    public class DateRange : BaseEntity
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime;

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime;
    }

    /// <summary>
    /// 坐标
    /// </summary>
    /// <remarks>
    /// 2013-08-14 郑荣华 创建
    /// </remarks>
    [Serializable]
    public class Coordinate : BaseEntity
    {

        /// <summary>
        /// 经度X
        /// </summary>
        public double X;

        /// <summary>
        /// 纬度Y
        /// </summary>
        public double Y;

        /// <summary>
        /// 所在象限，原点，坐标轴1,2,3,4
        /// 为0时当正号处理
        /// </summary>
        public int Quadrant
        {
            get
            {
                if (X >= 0)
                {
                    if (Y >= 0) return 1;
                    if (Y < 0) return 4;
                    //return 14;
                }
                if (X < 0)
                {
                    if (Y >= 0) return 2;
                    if (Y < 0) return 3;
                    //return 23;
                }

                //if (Y > 0) return 12;
                //if (Y < 0) return 34;
                return 0;
            }
        }
    }
    /// <summary>
    /// 基础返回json结果
    /// </summary>
    /// <remarks>
    /// 2015-09-17 王耀发 创建
    /// </remarks>
    [Serializable]
    public class ApiResult : BaseEntity
    {

        /// <summary>
        /// 状态代码
        /// </summary>
        public String code = string.Empty;

        /// <summary>
        /// 消息
        /// </summary>
        public String msg = string.Empty;

    }
}