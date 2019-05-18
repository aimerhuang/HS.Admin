using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Hyt.Tool.ImageBuilder
{
    /// <summary>
    /// 图片工具类
    /// </summary>
    /// <remarks>
    /// 2013-03-14 罗雄伟 创建
    /// </remarks>
    public class ImageUtil
    {
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
            WidthHeighLimitted
        }

        /// <summary>
        /// 创建高清缩略图
        /// </summary>
        /// <param name="sourceImage">原图路径</param>
        /// <param name="targetImage">缩略图保存路径</param>
        /// <param name="width">宽</param>
        /// <param name="height">高</param>
        /// <param name="mode">尺寸模式</param>
        /// <remarks>
        /// 2013-03-14 罗雄伟 创建
        /// </remarks>
        public static void CreateThumbnail(string sourceImage, string targetImage, int width, int height, ThumbnailMode mode)
        {
            using (Image source = Image.FromFile(sourceImage))
            {

                switch (mode)
                {
                    case ThumbnailMode.Width:
                        height = (int)Math.Round(source.Height * ((double)width / source.Width));
                        break;
                    case ThumbnailMode.Height:
                        width = (int)Math.Round(source.Width * ((double)height / source.Height));
                        break;
                    case ThumbnailMode.WidthHeighLimitted:
                        int w = (int)Math.Round(source.Width * ((double)height / width));
                        int h = (int)Math.Round(source.Height * ((double)width / height));
                        break;
                }

                //Image target = source.GetThumbnailImage(width, height, () => { return false; }, IntPtr.Zero);
                //target.Save(targetImage, ImageFormat.Jpeg);

                using (Bitmap target = new Bitmap(width, height))
                {
                    Graphics g = Graphics.FromImage(target);
                    g.CompositingQuality = CompositingQuality.HighQuality;
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.Clear(Color.Transparent);
                    g.DrawImage(source, new Rectangle(0, 0, width, height), new Rectangle(0, 0, source.Width, source.Height), GraphicsUnit.Pixel);

                    EncoderParameters parameters = new EncoderParameters();
                    parameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, new long[] { 100 });

                    string lookupKey = "image/jpeg";

                    var codecJpg = ImageCodecInfo.GetImageEncoders().Where(i => i.MimeType.Equals(lookupKey)).FirstOrDefault();

                    targetImage = Path.Combine(Path.GetDirectoryName(targetImage), Path.GetFileNameWithoutExtension(targetImage) + ".jpg");

                    if (!Directory.Exists(Path.GetDirectoryName(targetImage)))
                        Directory.CreateDirectory(Path.GetDirectoryName(targetImage));

                    target.Save(targetImage, codecJpg, parameters);
                }
            }

        }
    }
}
