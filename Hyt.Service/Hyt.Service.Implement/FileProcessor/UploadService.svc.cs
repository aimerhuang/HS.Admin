using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Activation;

namespace Hyt.Service.Implement.FileProcessor
{
    /// <summary>
    /// 文件上传服务
    /// </summary>
    /// <remarks>2014-1-8 黄波 创建</remarks>
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class UploadService : Hyt.Service.Contract.FileProcessor.IUploadService
    {
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="savePath">保存路径</param>
        /// <param name="fileName">文件名称</param>
        /// <param name="fileData">文件数据</param>
        /// <returns>是否上传成功</returns>
        /// <remarks>2013-12-9 黄波 创建</remarks>
        public bool UploadFile(string savePath, string fileName, byte[] fileData)
        {
            try
            {
                
                var saveFolder = System.Configuration.ConfigurationManager.AppSettings["ProductImagePath"];
                    //上传文件保存到的文件夹路径

                saveFolder += savePath;
                if (!saveFolder.EndsWith("\\")) saveFolder += "\\";
                if (!Directory.Exists(saveFolder)) Directory.CreateDirectory(saveFolder);

                Hyt.BLL.Log.LocalLogBo.Instance.Write("saveFolder:" + saveFolder + "  fileName:" + fileName);
                var saveFileFullPath = Path.Combine(saveFolder, fileName);
                
                //windows 系统日志
                //EventLog log = new EventLog("UploadFile");
                ////  首先应判断日志来源是否存在，一个日志来源只能同时与一个事件绑定s
                //if (!EventLog.SourceExists("Hyt.Service.Implement.FileProcessor.UploadFile"))
                //    EventLog.CreateEventSource("Hyt.Service.Implement.FileProcessor.UploadFile", "UploadFile");

                //log.Source = "Hyt.Service.Implement.FileProcessor.UploadFile";
               
                //log.WriteEntry("当前路径" + Directory.GetCurrentDirectory()+"上传文件到" + saveFileFullPath, EventLogEntryType.Information);

                Hyt.BLL.Log.LocalLogBo.Instance.Write("当前路径" + Directory.GetCurrentDirectory() + "上传文件到" + saveFileFullPath);

                File.WriteAllBytes(saveFileFullPath, fileData);
            }
            catch (Exception ex)
            {
                Hyt.BLL.Log.LocalLogBo.Instance.Write(ex);
                return false;
            }

            return true;
        }
    }
}
