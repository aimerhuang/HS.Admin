using Extra.UpGrade.SDK.JingDong.Stream.Connect;
using Extra.UpGrade.SDK.JingDong.Stream.Message;

namespace Extra.UpGrade.SDK.JingDong.Stream
{
    public interface IJdCometStream
    {
        void SetConnectionListener(IConnectionLifeCycleListener connectionLifeCycleListener);
        void SetMessageListener(IJdCometMessageListener cometMessageListener);
        void Start();
        void SJd();
    }
}
