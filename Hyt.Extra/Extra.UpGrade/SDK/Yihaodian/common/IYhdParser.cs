using System;
using Extra.UpGrade.SDK.Yihaodian.Response;

namespace Extra.UpGrade.SDK.Yihaodian.Parser
{
    /// <summary>
    /// YHD API响应解释器接口。响应格式可以是XML, JSON。
    /// </summary>
    public interface IYhdParser
    {
        /// <summary>
        /// 把响应xml字符串解释成相应的领域对象。
        /// </summary>
        /// <typeparam name="T">领域对象</typeparam>
        /// <param name="responseResult">响应字符串</param>
        /// <returns>领域对象</returns>
        T ParseToXml<T>(string responseResult) where T : YhdResponse;

        /// <summary>
        /// 把响应json字符串解释成相应的领域对象。
        /// </summary>
        /// <typeparam name="T">领域对象</typeparam>
        /// <param name="responseResult">响应字符串</param>
        /// <returns>领域对象</returns>
        T ParseToJson<T>(string responseResult) where T : YhdResponse;
    }
}
