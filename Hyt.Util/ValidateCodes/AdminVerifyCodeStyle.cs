using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Hyt.Util.ValidateCodes
{
    /// <summary>
    /// 生成验证码
    /// </summary>
    /// <remarks>
    /// 2013-06-07 由杨晗移植于hf 2013-06-07 杨晗 修改
    /// 2014-01-10 杨浩 重构
    /// </remarks>
    internal sealed class AdminVerifyCodeStyle : ICode
    {
        #region 构造函数
        /// <summary>
        /// 生成验证码
        /// </summary>
        /// <remarks>
        /// 2013-06-07 由杨晗移植于hf
        /// </remarks>
        public AdminVerifyCodeStyle()
        {
            CreateImageCode();
        }

        /// <summary>
        /// 生成验证码
        /// </summary>
        /// <param name="code">验证码字符串</param>
        /// <remarks>
        /// 2013-06-07 由杨晗移植于hf
        /// </remarks>
        public AdminVerifyCodeStyle(string code)
        {
            _code = code;

            CreateImageCode();
        }

        /// <summary>
        /// 生成验证码
        /// </summary>
        /// <param name="length">验证码长度</param>
        /// <remarks>
        /// 2013-06-07 由杨晗移植于hf
        /// </remarks>
        public AdminVerifyCodeStyle(int length)
        {
            _length = length;

            CreateImageCode();
        }

        /// <summary>
        /// 生成验证码
        /// </summary>
        /// <param name="code">验证码字符串</param>
        /// <param name="fontSize">字体大小</param>
        /// <param name="imageWidth">图片宽度</param>
        /// <param name="imageHeight">图片高度</param>
        /// <param name="padding">图片内边距</param>
        /// <remarks>
        /// 2013-06-07 由杨晗移植于hf
        /// </remarks>
        public AdminVerifyCodeStyle(string code, int fontSize, int imageWidth, int imageHeight, int padding)
        {
            _code = code;
            _fontSize = fontSize;
            _imageWidth = imageWidth;
            _imageHeight = imageHeight;
            _padding = padding;

            CreateImageCode();
        }

        /// <summary>
        /// 生成验证码
        /// </summary>
        /// <param name="length">验证码长度</param>
        /// <param name="fontSize">字体大小</param>
        /// <param name="imageWidth">图片宽度</param>
        /// <param name="imageHeight">图片高度</param>
        /// <param name="padding">图片内边距</param>
        /// <remarks>
        /// 2013-06-07 由杨晗移植于hf
        /// </remarks>
        public AdminVerifyCodeStyle(int length, int fontSize, int imageWidth, int imageHeight, int padding)
        {
            _length = length;
            _fontSize = fontSize;
            _imageWidth = imageWidth;
            _imageHeight = imageHeight;
            _padding = padding;

            CreateImageCode();

        }

        /// <summary>
        /// 生成验证码
        /// </summary>
        /// <param name="length">验证码长度</param>
        /// <param name="fontSize">字体大小</param>
        /// <param name="imageWidth">图片宽度</param>
        /// <param name="imageHeight">图片高度</param>
        /// <param name="padding">图片内边距</param>
        /// <param name="channelType">渠道类型</param>
        /// <remarks>
        /// 2013-06-07 由杨晗移植于hf 2013-06-07 杨晗 修改
        /// </remarks>
        public AdminVerifyCodeStyle(int length, int fontSize, int imageWidth, int imageHeight, int padding, int channelType)
        {
            _length = length;
            _fontSize = fontSize;
            _imageWidth = imageWidth;
            _imageHeight = imageHeight;
            _padding = padding;
            _channelType = channelType;

            CreateImageCode();

        }

        /// <summary>
        /// 生成验证码
        /// </summary>
        /// <param name="fontSize">字体大小</param>
        /// <param name="imageWidth">图片宽度</param>
        /// <param name="imageHeight">图片高度</param>
        /// <param name="padding">图片内边距</param>
        /// <remarks>
        /// 2013-06-07 由杨晗移植于hf
        /// </remarks>
        public AdminVerifyCodeStyle(int fontSize, int imageWidth, int imageHeight, int padding)
        {

            _fontSize = fontSize;
            _imageWidth = imageWidth;
            _imageHeight = imageHeight;
            _padding = padding;

            CreateImageCode();

        }

        /// <summary>
        /// 生成验证码
        /// </summary>
        /// <param name="fontSize">字体大小</param>
        /// <param name="imageWidth">图片宽度</param>
        /// <param name="imageHeight">图片高度</param>
        /// <remarks>
        /// 2013-06-07 由杨晗移植于hf
        /// </remarks>
        public AdminVerifyCodeStyle(int fontSize, int imageWidth, int imageHeight)
        {

            _fontSize = fontSize;
            _imageWidth = imageWidth;
            _imageHeight = imageHeight;

            CreateImageCode();
        }
        #endregion

        #region 生成校验码图片
        /// <summary>
        /// 生成校验码图片
        /// 校验码保存在Session["ImgGen"]中
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 2013-06-07 移植于hf 2013-06-07杨晗 修改
        /// </remarks>
        public void CreateImageCode()
        {
            if (string.IsNullOrWhiteSpace(_code))
            {
                _code = CreateVerifyCode(_length);
            }
            //string code = CreateVerifyCode(Length);
            //HttpContext.Current.Session["ImgGen"] = code.ToUpper();

            int fSize = _fontSize;
            int fWidth = fSize + Padding;
            int imageWidth = _imageWidth != 0 ? _imageWidth : (int)(_code.Length * fWidth) + 4 + Padding * 2;
            int imageHeight = _imageHeight != 0 ? _imageHeight : fSize * 2 + Padding;

            System.Drawing.Bitmap image = new System.Drawing.Bitmap(imageWidth, imageHeight);
            Graphics g = Graphics.FromImage(image);
            g.Clear(ChannelType == 0 ? BackgroundColor : ColorTranslator.FromHtml("#EEF1F3"));
            Random rand = new Random();

            //给背景添加随机生成的燥点
            if (ChannelType == 0)
            {
                if (Chaos)
                {
                    Pen pen = new Pen(ChaosColor, 0);
                    int c = Length * 100 * 3;

                    for (int i = 0; i < c; i++)
                    {
                        int x = rand.Next(image.Width);
                        int y = rand.Next(image.Height);

                        g.DrawRectangle(pen, x, y, 1, 1);
                    }
                }
            }
            int left = 0, top = 0, top1 = 1, top2 = 1;

            int n1 = (imageHeight - FontSize - Padding * 2);
            int n2 = n1 / 4;
            top1 = n2;
            top2 = n2 * 2;

            Font f;
            Brush b;
            int cindex, findex;

            //随机字体和颜色的验证码字符
            for (int i = 0; i < _code.Length; i++)
            {
                cindex = rand.Next(Colors.Length - 1);
                findex = rand.Next(Fonts.Length - 1);

                f = new System.Drawing.Font(Fonts[findex], fSize, ChannelType == 0 ? System.Drawing.FontStyle.Bold : FontStyle.Italic);
                b = new System.Drawing.SolidBrush(ChannelType == 0 ? Colors[cindex] : ColorTranslator.FromHtml("#0A87DB"));

                int paddingHight = imageHeight / 2 - f.Height / 2;
                top = rand.Next(-paddingHight, paddingHight * 2);

                left = i * (fWidth);

                g.DrawString(_code.Substring(i, 1), f, b, left, top);
            }
            if (ChannelType == 0)
            {
                //画一个边框 边框颜色为Color.Gainsboro
                g.DrawRectangle(new Pen(Color.Gainsboro, 0), 0, 0, image.Width - 1, image.Height - 1);
                g.Dispose();

                //产生波形
                image = TwistImage(image, false, 0, 0);
            }
            _image = image;
        }
        #endregion

        #region 属性

        private int _imageWidth = 100;

        private int _imageHeight = 50;

        #region 验证码图片
        private Bitmap _image;

        /// <summary>
        /// 验证码图片对象
        /// </summary>
        public Bitmap Image
        {
            get { return _image; }
            //set { _image = value; }
        }
        #endregion

        #region 渠道类型
        private int _channelType = 0;
        /// <summary>
        /// 使用渠道类型(0前台,1后台) 
        /// </summary>
        /// <remarks>
        /// 2013-06-07 杨晗 添加
        /// </remarks>
        public int ChannelType
        {
            get { return _channelType; }
            set { _channelType = value; }
        }
        #endregion

        #region 验证码字符串
        private string _code;

        /// <summary>
        /// 验证码字符串
        /// </summary>
        /// <remarks>
        /// 2013-06-07 由杨晗移植于hf
        /// </remarks>
        public string Code
        {
            get { return _code; }
            set { _code = value; }
        }
        #endregion

        #region 验证码长度
        int _length = 4;
        /// <summary>
        /// 验证码长度
        /// </summary>
        /// <remarks>
        /// 2013-06-07 由杨晗移植于hf
        /// </remarks>
        public int Length
        {
            get { return _length; }
            set { _length = value; }
        }
        #endregion

        #region 验证码字体大小(为了显示扭曲效果，默认40像素，可以自行修改)
        int _fontSize = 26;
        /// <summary>
        /// 验证码字体大小
        /// </summary>
        /// <remarks>
        /// 2013-06-07 由杨晗移植于hf
        /// </remarks>
        public int FontSize
        {
            get { return _fontSize; }
            set { _fontSize = value; }
        }
        #endregion

        #region 边框补(默认1像素)
        int _padding = 1;
        /// <summary>
        /// 边框补
        /// </summary>
        /// <remarks>
        /// 2013-06-07 由杨晗移植于hf
        /// </remarks>
        public int Padding
        {
            get { return _padding; }
            set { _padding = value; }
        }
        #endregion

        #region 是否输出燥点(默认不输出)
        bool chaos = true;
        /// <summary>
        /// 是否燥点
        /// </summary>
        /// <remarks>
        /// 2013-06-07 由杨晗移植于hf
        /// </remarks>
        public bool Chaos
        {
            get { return chaos; }
            set { chaos = value; }
        }
        #endregion

        #region 输出燥点的颜色(默认灰色)
        Color chaosColor = Color.FromArgb(193, 193, 193);
        /// <summary>
        /// 燥点颜色
        /// </summary>
        /// <remarks>
        /// 2013-06-07 由杨晗移植于hf
        /// </remarks>
        public Color ChaosColor
        {
            get { return chaosColor; }
            set { chaosColor = value; }
        }
        #endregion

        #region 自定义背景色(默认白色)
        Color backgroundColor = Color.FromArgb(222, 222, 222);
        /// <summary>
        /// 自定义背景色
        /// </summary>
        /// <remarks>
        /// 2013-06-07 由杨晗移植于hf
        /// </remarks>
        public Color BackgroundColor
        {
            get { return backgroundColor; }
            set { backgroundColor = value; }
        }
        #endregion

        #region 自定义随机颜色数组
        //Color[] colors = { Color.Black, Color.Black, Color.Black, Color.Black, Color.Black, Color.Black, Color.Black, Color.Black };
        Color[] colors = { Color.FromArgb(128, 128, 128) };
        /// <summary>
        /// 自定义随机颜色数组
        /// </summary>
        /// <remarks>
        /// 2013-06-07 由杨晗移植于hf
        /// </remarks>
        public Color[] Colors
        {
            get { return colors; }
            set { colors = value; }
        }
        #endregion

        #region 自定义字体数组
        string[] fonts = { "微软雅黑" };//"宋体","华文中宋",
        /// <summary>
        /// 自定义自提数组
        /// </summary>
        /// <remarks>
        /// 2013-06-07 由杨晗移植于hf
        /// </remarks>
        public string[] Fonts
        {
            get { return fonts; }
            set { fonts = value; }
        }
        #endregion

        #region 自定义随机码字符串序列(使用逗号分隔)
        string codeSerial = "2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,j,k,m,n,p,q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,J,K,L,M,N,P,Q,R,S,T,U,V,W,X,Y,Z";
        /// <summary>
        /// 自定义随机码字符串序列
        /// </summary>
        /// <remarks>
        /// 2013-06-07 由杨晗移植于hf
        /// </remarks>
        public string CodeSerial
        {
            get { return codeSerial; }
            set { codeSerial = value; }
        }
        #endregion
        #endregion

        #region 产生波形滤镜效果

        private const double PI = 3.1415926535897932384626433832795;
        private const double PI2 = 6.283185307179586476925286766559;

        /// <summary>
        /// 正弦曲线Wave扭曲图片（Edit By 51aspx.com）
        /// </summary>
        /// <param name="srcBmp">图片路径</param>
        /// <param name="bXDir">如果扭曲则选择为True</param>
        /// <param name="nMultValue">波形的幅度倍数，越大扭曲的程度越高，一般为3</param>
        /// <param name="dPhase">波形的起始相位，取值区间[0-2*PI)</param>
        /// <returns>生成的图片</returns>
        /// <remarks>
        /// 2013-06-07 由杨晗移植于hf
        /// </remarks>
        public System.Drawing.Bitmap TwistImage(Bitmap srcBmp, bool bXDir, double dMultValue, double dPhase)
        {
            System.Drawing.Bitmap destBmp = new Bitmap(srcBmp.Width, srcBmp.Height);

            // 将位图背景填充为白色
            System.Drawing.Graphics graph = System.Drawing.Graphics.FromImage(destBmp);
            graph.FillRectangle(new SolidBrush(System.Drawing.Color.White), 0, 0, destBmp.Width, destBmp.Height);
            graph.Dispose();

            double dBaseAxisLen = bXDir ? (double)destBmp.Height : (double)destBmp.Width;

            for (int i = 0; i < destBmp.Width; i++)
            {
                for (int j = 0; j < destBmp.Height; j++)
                {
                    double dx = 0;
                    dx = bXDir ? (PI2 * (double)j) / dBaseAxisLen : (PI2 * (double)i) / dBaseAxisLen;
                    dx += dPhase;
                    double dy = Math.Sin(dx);

                    // 取得当前点的颜色
                    int nOldX = 0, nOldY = 0;
                    nOldX = bXDir ? i + (int)(dy * dMultValue) : i;
                    nOldY = bXDir ? j : j + (int)(dy * dMultValue);

                    System.Drawing.Color color = srcBmp.GetPixel(i, j);
                    if (nOldX >= 0 && nOldX < destBmp.Width
                     && nOldY >= 0 && nOldY < destBmp.Height)
                    {
                        destBmp.SetPixel(nOldX, nOldY, color);
                    }
                }
            }

            return destBmp;
        }

        #endregion

        #region 生成随机字符码
        /// <summary>
        /// 生成随机字符串
        /// </summary>
        /// <param name="codeLen">字符长度</param>
        /// <returns>生成的字符串</returns>
        /// <remarks>
        /// 2013-06-07 由杨晗移植于hf
        /// </remarks>
        public string CreateVerifyCode(int codeLen)
        {
            if (codeLen == 0)
            {
                codeLen = Length;
            }

            string[] arr = CodeSerial.Split(',');

            string code = "";

            int randValue = -1;

            Random rand = new Random(unchecked((int)DateTime.Now.Ticks));

            for (int i = 0; i < codeLen; i++)
            {
                randValue = rand.Next(0, arr.Length - 1);

                code += arr[randValue];
            }

            return code;
        }

        /// <summary>
        /// 生成4个长度的字符串
        /// </summary>
        /// <returns>4个长度的字符串</returns>
        /// <remarks>
        /// 2013-06-07 由杨晗移植于hf
        /// </remarks>
        public string CreateVerifyCode()
        {
            return CreateVerifyCode(4);
        }
        #endregion

        /// <summary>
        /// 生成验证码图片
        /// </summary>
        /// <param name="imageWidth">图片宽度</param>
        /// <param name="imageHeight">图片高度</param>
        /// <param name="length">验证码字符串长度</param>
        /// <returns>验证码对象</returns>
        /// <remarks>杨浩 2014-1-21 添加</remarks>
        public CodeWrap CreateCode(int imageWidth, int imageHeight, int length = 4)
        {
            _length = length;
            _fontSize = 18;
            _imageWidth = imageWidth;
            _imageHeight = imageHeight;
            _padding = 5;
            _channelType = 1;

            CreateImageCode();

            var ms = new System.IO.MemoryStream();
            Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            var imagebyte = ms.GetBuffer();
            ms.Dispose();

            return new CodeWrap
                {
                    Image = imagebyte,
                    Code = Code
                };
        }
    }
}
