<%@ webhandler Language="C#" class="Upload" %>

/**
 * KindEditor ASP.NET
 *
 * 本ASP.NET程序是演示程序，建议不要直接在实际项目中使用。
 * 如果您确定直接使用本程序，使用之前请仔细确认相关安全设置。
 *
 */

using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Web;
using System.IO;
using System.Globalization;
using Hyt.Model;

using Hyt.Util;
using Hyt.Util.Serialization;
using Hyt.Infrastructure.Communication;
using Hyt.Service.Contract.FileProcessor;
using IronPython.Modules;

public class Upload : IHttpHandler
{
	private HttpContext context;
    private readonly AttachmentConfig attachmentConfig =
           Hyt.BLL.Config.Config.Instance.GetAttachmentConfig();
	public void ProcessRequest(HttpContext context)
	{
		//定义允许上传的文件扩展名
		Hashtable extTable = new Hashtable();
        extTable.Add("ei", "gif,jpg,jpeg,png,bmp");
		extTable.Add("image", "gif,jpg,jpeg,png,bmp");
		extTable.Add("flash", "swf,flv");
		extTable.Add("media", "swf,flv,mp3,wav,wma,wmv,mid,avi,mpg,asf,rm,rmvb");
		extTable.Add("file", "doc,docx,xls,xlsx,ppt,htm,html,txt,zip,rar,gz,bz2");

		//最大文件大小
		int maxSize = 2000000;
		this.context = context;

		HttpPostedFile imgFile = context.Request.Files["imgFile"];
		if (imgFile == null)
		{
			showError("请选择文件。");
		}

		String dirName = context.Request.QueryString["dir"];
		if (String.IsNullOrEmpty(dirName))
		{
		    dirName = "image";
		}
		if (!extTable.ContainsKey(dirName)) {
			showError("目录名不正确。");
		}

        String fileName = imgFile.FileName;
		String fileExt = Path.GetExtension(fileName).ToLower();

		if (imgFile.InputStream == null || imgFile.InputStream.Length > maxSize)
		{
			showError("上传文件大小超过限制。");
		}

		if (String.IsNullOrEmpty(fileExt) || Array.IndexOf(((String)extTable[dirName]).Split(','), fileExt.Substring(1).ToLower()) == -1)
		{
			showError("上传文件扩展名是不允许的扩展名。\n只允许" + ((String)extTable[dirName]) + "格式。");
		}

        var attachmentConfig = Hyt.BLL.Config.Config.Instance.GetAttachmentConfig();

	    using (var client = new ServiceProxy<IUploadService>())
	    {
	        var buff = new byte[imgFile.InputStream.Length];
	        imgFile.InputStream.Read(buff, 0, buff.Length);
	        dirName = string.Format("{0}/{1}", dirName,DateTime.Now.ToString("yyyyMM"));
            fileName = NewFileName(Path.GetExtension(fileName).ToLower());//生成统一的GUID文件路径
            
            if (client.Channel.UploadFile(dirName,fileName , buff))
            {
                var fileUrl = string.Format("{0}{1}/{2}", attachmentConfig.FileServer,dirName, fileName);
                
                var hash = new Hashtable();
                hash["error"] = 0;
                hash["url"] = fileUrl;
                context.Response.Write(JsonUtil.ToJson2(hash));
                context.Response.End();
	        }
	    }
        
        
        //fileName = NewFileName(".jpg");
        //string basePath = attachmentConfig.EditorImage + "/" + fileName;
        ////imgFile.SaveAs(filePath);
        //var baseImage = Hyt.Util.ImageUtil.ConvertToJpg(imgFile.InputStream);
        //var ftp = new Hyt.Util.Net.FtpUtil(
        //        attachmentConfig.FtpImageServer,
        //        attachmentConfig.FtpUserName,
        //        attachmentConfig.FtpPassword);
        //ftp.Upload(baseImage, attachmentConfig.FtpImageServer + basePath);

        //String fileUrl = attachmentConfig.FileServer + basePath;

        //Hashtable hash = new Hashtable();
        //hash["error"] = 0;
        //hash["url"] = fileUrl;
        //context.Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
        //baseImage.Dispose();
        //context.Response.Write(JsonUtil.ToJson2(hash));
        //context.Response.End();
	}
    /// <summary>
    /// 生成新文件名称
    /// </summary>
    /// <param name="fileExtension">文件类型（带点）</param>
    /// <returns>新的文件名称</returns>
    /// <returns>2013-6-13 黄波 创建</returns>
    private string NewFileName(string fileExtension)
    {
        return Guid.NewGuid().ToString("N")
            // + DateTime.Now.ToString("hhsMmmyysyyMssdsd") 图片路径太长，去掉
            + fileExtension;
    }
	private void showError(string message)
	{
		Hashtable hash = new Hashtable();
        hash["error"] = 1;
        hash["message"] = message;
		context.Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
        context.Response.Write(JsonUtil.ToJson(hash));
		context.Response.End();
	}

	public bool IsReusable
	{
		get
		{
			return true;
		}
	}
    
}
