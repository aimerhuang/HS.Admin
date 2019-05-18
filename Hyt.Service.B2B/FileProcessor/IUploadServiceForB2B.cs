using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace Hyt.ServiceB2B.FileProcessor
{
    /// <summary>
    /// 图片处理(其他平台调用)
    /// </summary>
    [ServiceContract]
    public interface IUploadServiceForB2B
    {
        /// <summary>
        /// 上传文件服务程序
        /// </summary>
        /// <param name="request">上传的文件消息</param>
        /// <remarks>2017-10-10 罗勤瑶 创建</remarks>
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        bool UploadFile(string savePath, string fileName, byte[] fileData);
    }
}
