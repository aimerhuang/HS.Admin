using Jd.Link;
using Extra.UpGrade.SDK.JingDong;

namespace Jd.Tmc
{
    //refer to https://gist.github.com/wsky/5027961
    internal class ClientLog : DefaultLogger, IClientLog
    {
        public ClientLog(string name
            , bool isDebugEnabled
            , bool isInfoEnabled
            , bool isWarnEnabled
            , bool isErrorEnabled
            , bool isFatalEnabled)
            : base(name
            , isDebugEnabled
            , isInfoEnabled
            , isWarnEnabled
            , isErrorEnabled
            , isFatalEnabled) { }

        void IJdLogger.Error(string message)
        {
            this.Error(message);
        }

        void IJdLogger.Warn(string message)
        {
            this.Warn(message);
        }

        void IJdLogger.Info(string message)
        {
            this.Info(message);
        }
    }
}