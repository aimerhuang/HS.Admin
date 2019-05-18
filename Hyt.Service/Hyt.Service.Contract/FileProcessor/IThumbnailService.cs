using System.ServiceModel.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace Hyt.Service.Contract.FileProcessor
{
    /// <summary>
    /// 略缩图处理服务
    /// </summary>
    /// <remarks>2013-12-9 黄波 创建</remarks>
    [ServiceContract]
    public interface IThumbnailService
    {
        /// <summary>
        /// 产品略缩图处理 
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="productSysNo">产品编号</param>
        /// <remarks>2013-12-9 黄波 创建</remarks>
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        bool ProductThumbnailProcessor(string fileName,int productSysNo);
    }
}