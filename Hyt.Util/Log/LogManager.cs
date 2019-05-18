
namespace Hyt.Util.Log
{
    /// <summary>
    /// 日志记录
    /// </summary>
    /// <remarks>2013-3-21 杨浩 添加</remarks>
    public class LogManager
    {
        private static readonly ILog ilog = new LogFile();
        //保证不能被外部实例化
        private LogManager() { }

        /// <summary>
        /// 日志记录实例
        /// </summary>
        public static ILog Instance
        {
            get 
            {
                return ilog;
            }
        }
    }    
}
