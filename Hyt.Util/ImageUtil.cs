using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace Hyt.Util
{
    /// <summary>
    /// 图片工具类
    /// </summary>
    /// <remarks>2013-03-14 罗雄伟 创建</remarks>
    public class ImageUtil
    {
        /// <summary>
        /// 略缩图类型
        /// </summary>
        /// <remarks>2013-03-14 罗雄伟 创建</remarks>
        public enum ThumbnailMode
        {
            /// <summary>
            /// 固定宽度，高度做相应计算
            /// </summary>
            Width,

            /// <summary>
            /// 固定高度，宽度做相应计算
            /// </summary>
            Height,

            /// <summary>
            /// 固定宽度和高度
            /// </summary>
            WidthHeighLimitted,
            /// <summary>
            /// 指定高宽裁减（不变形） yhy
            /// </summary>
            Cut
        }

        /// <summary>
        /// 创建高清缩略图
        /// </summary>
        /// <param name="imgStream">原图路径</param>
        /// <param name="width">宽</param>
        /// <param name="height">高</param>
        /// <param name="mode">尺寸模式</param>
        /// <returns>图片流</returns>
        /// <remarks>
        /// 2013-03-14 罗雄伟 创建
        /// 2013-04-09 吴文强 修改
        /// </remarks>
        public static MemoryStream CreateThumbnail(Stream imgStream, int width, int height, ThumbnailMode mode)
        {
            MemoryStream ms = new MemoryStream();

            using (Image source = Image.FromStream(imgStream))
            {
                #region 计算坐标和宽高

                int x = 0;
                int y = 0;
                int ow = source.Width;
                int oh = source.Height;

                switch (mode)
                {
                    case ThumbnailMode.Width:
                        height = (int)Math.Round(source.Height * ((double)width / source.Width));
                        break;
                    case ThumbnailMode.Height:
                        width = (int)Math.Round(source.Width * ((double)height / source.Height));
                        break;
                    case ThumbnailMode.Cut://指定高宽裁减（不变形）  yhy              
                        if ((double)source.Width / (double)source.Height > (double)width / (double)height)
                        {
                            oh = source.Height;
                            ow = source.Height * width / height;
                            y = 0;
                            x = (source.Width - ow) / 2;
                        }
                        else
                        {
                            ow = source.Width;
                            oh = source.Width * height / width;
                            x = 0;
                            y = (source.Height - oh) / 2;
                        }
                        break;
                }

                #endregion

                #region 尺寸不变则直接返回

                if (width == ow && height == oh)
                {
                    imgStream.Position = 0;
                    imgStream.CopyTo(ms);
                    return ms;
                }

                #endregion

                #region 生成缩略图

                using (Bitmap bmp = new Bitmap(width, height))
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.CompositingQuality = CompositingQuality.HighQuality;
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.DrawImage(source, new Rectangle(0, 0, width, height), new Rectangle(x, y, ow, oh), GraphicsUnit.Pixel);

                    EncoderParameters parameters = new EncoderParameters();
                    parameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, new long[] { 100 });

                    string lookupkey = "image/jpeg";
                    var codecjpg = ImageCodecInfo.GetImageEncoders().Where(i => i.MimeType.Equals(lookupkey)).FirstOrDefault();

                    bmp.Save(ms, codecjpg, parameters);
                }

                #endregion
            }
            return ms;
        }


        /// <summary>
        /// 创建高清缩略图
        /// </summary>
        /// <param name="imgStream">原图路径</param>
        /// <param name="width">宽</param>
        /// <param name="height">高</param>
        /// <param name="mode">尺寸模式</param>
        /// <returns>图片流</returns>
        /// <remarks>2017-10-10 罗勤瑶 创建</remarks>
        public static MemoryStream CreateThumbnailB2B(Stream imgStream, int width, int height, ThumbnailMode mode)
        {
            MemoryStream ms = new MemoryStream();

            using (Image source = Image.FromStream(imgStream))
            {
                #region 计算坐标和宽高

                int x = 0;
                int y = 0;
                int ow = source.Width;
                int oh = source.Height;

                switch (mode)
                {
                    case ThumbnailMode.Width:
                        height = (int)Math.Round(source.Height * ((double)width / source.Width));
                        break;
                    case ThumbnailMode.Height:
                        width = (int)Math.Round(source.Width * ((double)height / source.Height));
                        break;
                    case ThumbnailMode.Cut://指定高宽裁减（不变形）  yhy              
                        if ((double)source.Width / (double)source.Height > (double)width / (double)height)
                        {
                            oh = source.Height;
                            ow = source.Height * width / height;
                            y = 0;
                            x = (source.Width - ow) / 2;
                        }
                        else
                        {
                            ow = source.Width;
                            oh = source.Width * height / width;
                            x = 0;
                            y = (source.Height - oh) / 2;
                        }
                        break;
                }

                #endregion

                #region 尺寸不变则直接返回

                if (width == ow && height == oh)
                {
                   // var ms = new MemoryStream();
                    source.Save(ms, ImageFormat.Jpeg);
                    return ms;
                }

                #endregion

                #region 生成缩略图

                using (Bitmap bmp = new Bitmap(width, height))
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.CompositingQuality = CompositingQuality.HighQuality;
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.DrawImage(source, new Rectangle(0, 0, width, height), new Rectangle(x, y, ow, oh), GraphicsUnit.Pixel);

                    EncoderParameters parameters = new EncoderParameters();
                    parameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, new long[] { 100 });

                    string lookupkey = "image/jpeg";
                    var codecjpg = ImageCodecInfo.GetImageEncoders().Where(i => i.MimeType.Equals(lookupkey)).FirstOrDefault();

                    bmp.Save(ms, codecjpg, parameters);
                }

                #endregion
            }
            return ms;
        }

        /// <summary>
        /// 将图片转换为jpg格式
        /// </summary>
        /// <param name="imgStream">文件流</param>
        /// <returns>转换后的文件</returns>
        /// <remarks>2017-10-10 罗勤瑶 创建</remarks>
        public static MemoryStream ConvertToJpg(Stream imgStream)
        {
            using (Image source = Bitmap.FromStream(imgStream))
            {
                if (source.RawFormat.Guid == ImageFormat.Jpeg.Guid)
                {
                    imgStream.Seek(0, SeekOrigin.Begin);
                    byte[] buffer = new byte[imgStream.Length];
                    imgStream.Read(buffer, 0, buffer.Length);
                    imgStream.Seek(0, SeekOrigin.Begin);
                    return new MemoryStream(buffer);
                }
                else
                {
                    var ms = new MemoryStream();
                    source.Save(ms, ImageFormat.Jpeg);
                    return ms;

                }
            }
        }

        /// <summary>
        /// 将图片转换为jpg格式
        /// </summary>
        /// <param name="imgStream">文件流</param>
        /// <returns>转换后的文件</returns>
        /// <remarks>2010-10-10 罗勤瑶 创建</remarks>
        public static MemoryStream ConvertToJpgB2B(Stream imgStream)
        {
            using (Image source = Bitmap.FromStream(imgStream))
            {
                //if (source.RawFormat.Guid == ImageFormat.Jpeg.Guid)
                //{
                //    byte[] img = null;
                //    int size = 1024;
                //    int read = 0;
                //    MemoryStream ms = new MemoryStream();
                //    byte[] buffer = new byte[size];
                //    do
                //    {
                //        buffer = new byte[size];
                //        read = imgStream.Read(buffer, 0, size);
                //        ms.Write(buffer, 0, read);
                //    } while (read > 0);
                //    img = ms.ToArray();
                //    return new MemoryStream(img);
                //}
                //else
                //{
                    var ms = new MemoryStream();
                    source.Save(ms, ImageFormat.Jpeg);
                    return ms;

                //}
            }
            //Bitmap bt = new Bitmap(imgStream); 
            //System.Drawing.Image img = System.Drawing.Image.FromStream(imgStream);
            //MemoryStream ms = new MemoryStream();
            //img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            //bt.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            //using (Image source = Bitmap.FromStream(imgStream))
            //{
               
            //        var ms = new MemoryStream();
            //        source.Save(ms, ImageFormat.Jpeg);
            //        return ms;

                
            //}

           
        }

        /// <summary>
        /// 流转换为Bytes
        /// </summary>
        /// <param name="stream">数据流</param>
        /// <returns>图片字节数组</returns>
        /// <remarks>2013-03-14 罗雄伟 创建</remarks>
        public static byte[] StreamConvertToBytes(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Seek(0, SeekOrigin.Begin);
            stream.Position = 0;
            stream.Read(bytes, 0, bytes.Length);
            return bytes;
        }
    }
}
