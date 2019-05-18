using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Hyt.Model;

namespace Hyt.Admin
{
    /// <summary>
    /// 文件上传类
    /// </summary>
    /// <return>文件上传后的路径</return>
    /// <remarks>2013-06-13 黄波 添加</remarks>
    public class upload : IHttpHandler
    {
        //附件ftp信息
        private readonly AttachmentConfig attachmentConfig =
            Hyt.BLL.Config.Config.Instance.GetAttachmentConfig();

        /// <summary>
        /// 处理请求
        /// </summary>
        /// <param name="context">http请求内容</param>
        /// <returns>void</returns>
        /// <remarks>2013-06-13 黄波 添加</remarks>
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            var path = string.Empty;

            //接收上传后的文件
            HttpPostedFile file = context.Request.Files["Filedata"];
            //配置名称
            string config = context.Request["config"];

            var uploadConfig = Hyt.BLL.Config.Config.Instance.GetUpLoadFileConfig();
            var useUploadConfigOption = uploadConfig.OtherConfig.Find(o => o.EncryptAlias == config);
            if (useUploadConfigOption == null) useUploadConfigOption = uploadConfig.DefaultConfig;

            if (config == Hyt.Model.SystemPredefined.UploadConfigAlias.ProductImageConfigAlias)
            {
                path = SaveProductImage(file.InputStream, useUploadConfigOption, file.FileName);
            }
            else
            {
                path = SaveFile(file.InputStream, useUploadConfigOption, file.FileName);
            }

            context.Response.Write(path);
        }

        #region 保存产品图片
        /// <summary>
        /// 保存产品图片
        /// </summary>
        /// <param name="image">图片流</param>
        /// <param name="options">上传配置选项</param>
        /// <param name="fileName">原文件名称</param>
        /// <returns>图片路径</returns>
        /// <returns>2013-6-13 黄波 创建</returns>
        public string SaveProductImage(Stream image, Model.Common.FileConfigOption options, string fileName)
        {

            fileName = NewFileName(".jpg");
            //100*100
            var fileSamillName = fileName;// + ".small.jpg";

            var productImageConfig = Hyt.BLL.Config.Config.Instance.GetProductImageConfig();

            string yearMonth = DateTime.Now.ToString("yyyyMM");

            #region 路径
            string basePath = string.Format(
                    productImageConfig.ProductImagePathFormat,
                    "",
                    productImageConfig.BaseFolder,
                    ""
                    );
            string imageBigPath = string.Format(
                    productImageConfig.ProductImagePathFormat,
                    "",
                    productImageConfig.BigFloder,
                    ""
                    );
            string imageSmallPath = string.Format(
                    productImageConfig.ProductImagePathFormat,
                    "",
                    productImageConfig.SmallFloder,
                    ""
                    );
            #endregion

            #region 缩略图
            //转换原图
            var baseImage = Hyt.Util.ImageUtil.ConvertToJpg(image);
            //生成Big
            var imageBig = Hyt.Util.ImageUtil.CreateThumbnail(image,
                productImageConfig.BigWidth,
                productImageConfig.BigHeight,
                Hyt.Util.ImageUtil.ThumbnailMode.Cut);
            //生成small
            var imageSmall = Hyt.Util.ImageUtil.CreateThumbnail(image,
                productImageConfig.SmallWidth,
                productImageConfig.SmallHeight,
                Hyt.Util.ImageUtil.ThumbnailMode.Cut);
            #endregion

            var status = true;
            var B2Bstatus = true;
            try
            {
                using (var service = new Hyt.Infrastructure.Communication.ServiceProxy<Hyt.Service.Contract.FileProcessor.IUploadService>())
                {
                    status = service.Channel.UploadFile(basePath, fileName, Hyt.Util.ImageUtil.StreamConvertToBytes(baseImage));
                    status = service.Channel.UploadFile(imageBigPath, fileName, Hyt.Util.ImageUtil.StreamConvertToBytes(imageBig));
                    status = service.Channel.UploadFile(imageSmallPath, fileSamillName, Hyt.Util.ImageUtil.StreamConvertToBytes(imageSmall));
                }
                //baseImage.Dispose();
                //imageBig.Dispose();
                //imageSmall.Dispose();
                //image.Dispose();
                //上传图片到B2B的服务器 罗勤瑶 210101010
                using (var service = new Hyt.Infrastructure.Communication.ServiceProxy<B2B.Service.Img.FileProcessor.IUploadServiceForB2B>())
                {
                    B2Bstatus = service.Channel.UploadFile(basePath, fileName, Hyt.Util.ImageUtil.StreamConvertToBytes(baseImage));
                    B2Bstatus = service.Channel.UploadFile(imageBigPath, fileName, Hyt.Util.ImageUtil.StreamConvertToBytes(imageBig));
                    B2Bstatus = service.Channel.UploadFile(imageSmallPath, fileSamillName, Hyt.Util.ImageUtil.StreamConvertToBytes(imageSmall));
                }
                baseImage.Dispose();
                imageBig.Dispose();
                imageSmall.Dispose();
                image.Dispose();
            }
            catch (Exception ex)
            {
                Hyt.BLL.Log.LocalLogBo.Instance.Write(ex);
            }
            if (!status)
                throw new Exception("文件上传失败!");

            return string.Format(productImageConfig.ProductImagePathFormat, "{0}", "{1}", fileName);//attachmentConfig.FileServer
        }
        #endregion

        #region 保存文件

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="file">文件流</param>
        /// <param name="options">上传配置选项</param>
        /// <param name="fileName">原文件名称</param>
        /// <param name="isRelativePath">是否相对路径</param>
        /// <returns>文件路径</returns>
        /// <remarks>2013-6-13 黄波 创建</remarks>
        public string SaveFile(Stream file, Model.Common.FileConfigOption options, string fileName, bool? isRelativePath = false)
        {
            fileName = NewFileName(Path.GetExtension(fileName));

            string yearMonth = DateTime.Now.ToString("yyyyMM");

            string filePath = string.Format(options.Folder + "/{0}/{1}", yearMonth, fileName);

            var status = true;
            try
            {
                using (var service = new Hyt.Infrastructure.Communication.ServiceProxy<Hyt.Service.Contract.FileProcessor.IUploadService>())
                {
                    status = service.Channel.UploadFile(options.Folder + "/" + yearMonth, fileName, Hyt.Util.ImageUtil.StreamConvertToBytes(file));
                }
                file.Dispose();
            }
            catch (Exception ex)
            {
                Hyt.BLL.Log.LocalLogBo.Instance.Write(ex);
            }
            if (!status)
                throw new Exception("文件上传失败!");

            return options.Folder + "/" + yearMonth + "/" + fileName;
        }
        #endregion

        #region 生成新文件名称
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
        #endregion

        /// <summary>
        /// 获取一个值，该值指示其他请求是否可以使用 IHttpHandler 实例
        /// 设置IsReusable为true的时候，一定要保证线程安全，并且不依赖Request项，
        /// 当然也不应该有成员变量，因为成员变量在同一个实例下是随意可用的
        /// </summary>
        /// <returns>其他请求是否可以使用 IHttpHandler 实例</returns>
        /// <remarks>2013-06-13 黄波 添加</remarks>
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}