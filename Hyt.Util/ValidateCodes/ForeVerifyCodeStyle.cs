using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Hyt.Util.ValidateCodes
{
    /// <summary>
    /// 前台验证码Style
    /// </summary>
    /// <remarks>2014-01-10 杨浩 创建</remarks>
    internal sealed class ForeVerifyCodeStyle : ICode
    {
        private string _rndCode;
        private Random _random;
        //文本字体
        private string[] _fontFamily = { "Verdana", "Georgia", "Euphemia" };//"Arial", "Georgia"
        //随机码的旋转角度
        private int _randomAngle = 30;
        //字体最大尺寸
        private int _fontSize = 24;

        public ForeVerifyCodeStyle()
        {
            this._random = new Random();
        }

        /// <summary>
        /// 生成随机码
        /// </summary>
        /// <param name="length">随机码长度</param>
        /// <returns></returns>
        /// <remarks>2014-1-21 黄波 创建</remarks>
        private void CreateRndCode(int length)
        {
            const string str = "0123456789abcdefghkmopqstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ";
            char[] tary = str.ToCharArray();
            for (int i = 0; i < length; i++)
            {
                this._rndCode += tary[this._random.Next(tary.Length)];
            }
        }

        /// <summary>
        /// 生成验证码
        /// </summary>
        /// <param name="imageWidth">宽度</param>
        /// <param name="imageHeight">高度</param>
        /// <param name="length">长度</param>
        /// <returns>验证码对象</returns>
        /// <remarks>2014-01-10 杨浩 创建</remarks>
        public CodeWrap CreateCode(int imageWidth, int imageHeight, int length = 4)
        {
            CreateRndCode(length);
            Bitmap image = new Bitmap(imageWidth, imageHeight);
            Graphics g = Graphics.FromImage(image);
            //清除画面，填充背景
            g.Clear(Color.WhiteSmoke);

            Color tcolor = Color.FromArgb(_random.Next(20, 130), _random.Next(20, 130), _random.Next(20, 130));
            SolidBrush b = new SolidBrush(tcolor); 

            //文字距中
            var format = new StringFormat(StringFormatFlags.NoClip);
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;

            //验证码旋转、粘连，防止机器识别
            char[] chars = _rndCode.ToCharArray();
            Font tmpf;
            float X;
            float Y;
            for (int i = 0; i < chars.Length; i++)
            {
                //随机字体样式
                tmpf = new Font(_fontFamily[_random.Next(_fontFamily.Length - 1)],_random.Next(_fontSize - 6, _fontSize), FontStyle.Regular);
                //转动的度数
                float angle = _random.Next(-this._randomAngle, _randomAngle); 
                Y = _random.Next(12, 20);
                if (i == 0)
                {
                    X = _random.Next(16, 50);
                }
                else
                {
                    X = 12;
                }

                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                //移动光标到指定位置
                g.TranslateTransform(X, Y);
                //旋转
                g.RotateTransform(angle);
                //写文本
                g.DrawString(chars[i].ToString(), tmpf, b, -2, 2, format);
                //旋转回去
                g.RotateTransform(-angle);
                //移动光标到指定位置
                g.TranslateTransform(0, -Y);
            }
            var ms = new System.IO.MemoryStream();
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            var imagebyte = ms.GetBuffer();
            ms.Dispose();
            g.Dispose();
            image.Dispose();
            return new CodeWrap
                {
                    Image = imagebyte,
                    Code = _rndCode
                };
        }
    }
}
