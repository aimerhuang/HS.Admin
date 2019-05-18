using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;

namespace Extra.UpGrade.SDK.Yihaodian.Request
{
    /// <summary>
    /// YHD请求接口。
    /// </summary>
    public interface IYhdRequest<T> where T : YhdResponse
    {
        /// <summary>
        /// 获取YHD的API名称。
        /// </summary>
        /// <returns>API名称</returns>
        string GetApiName();

        /// <summary>
        /// 获取所有的Key-Value形式的文本请求参数字典。其中：
        /// Key: 请求参数名
        /// Value: 请求参数文本值
        /// </summary>
        /// <returns>文本请求参数字典</returns>
        IDictionary<string, string> GetParameters();
    }
}
