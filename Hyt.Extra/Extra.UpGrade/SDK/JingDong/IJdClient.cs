using System;
using Extra.UpGrade.SDK.JingDong.Request;

namespace Extra.UpGrade.SDK.JingDong
{
    /// <summary>
	/// JD客户端。
    /// </summary>
    public interface IJdClient
    {
        /// <summary>
		/// 执行JD公开API请求。
        /// </summary>
        /// <typeparam name="T">领域对象</typeparam>
		/// <param name="request">具体的JD API请求</param>
        /// <returns>领域对象</returns>
        T Execute<T>(IJdRequest<T> request) where T : JdResponse;

        /// <summary>
		/// 执行JD隐私API请求。
        /// </summary>
        /// <typeparam name="T">领域对象</typeparam>
		/// <param name="request">具体的JD API请求</param>
        /// <param name="session">用户会话码</param>
        /// <returns>领域对象</returns>
        T Execute<T>(IJdRequest<T> request, string session) where T : JdResponse;

        /// <summary>
		/// 执行JD隐私API请求。
        /// </summary>
        /// <typeparam name="T">领域对象</typeparam>
		/// <param name="request">具体的JD API请求</param>
        /// <param name="session">用户会话码</param>
        /// <param name="timestamp">请求时间戳</param>
        /// <returns>领域对象</returns>
        T Execute<T>(IJdRequest<T> request, string session, DateTime timestamp) where T : JdResponse;
    }
}
