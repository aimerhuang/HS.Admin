using System.ServiceModel.Web;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.IO;

namespace Hyt.Service.Contract.FileProcessor
{
    /// <summary>
    /// 图片处理
    /// </summary>
    [ServiceContract]
    public interface IUploadService
    {
        /// <summary>
        /// 上传文件服务程序
        /// </summary>
        /// <param name="request">上传的文件消息</param>
        /// <remarks>2013-12-9 黄波 创建</remarks>
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        bool UploadFile(string savePath, string fileName, byte[] fileData);
    }
}