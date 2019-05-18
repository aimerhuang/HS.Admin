using System;

namespace Extra.UpGrade.SDK.JingDong.Stream
{
    public interface IStreamImplementation
    {
        bool IsAlive();
        void NextMsg();
        string ParseLine(string msg);
        void OnException(Exception ex);
        void Close();
    }
}
