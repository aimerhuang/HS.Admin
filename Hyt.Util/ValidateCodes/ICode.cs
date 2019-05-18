namespace Hyt.Util.ValidateCodes
{
    /// <summary>
    /// 公共验证码接口
    /// </summary>
    /// <remarks>2014-01-10 杨浩 创建</remarks>
    public interface  ICode
    {
        /// <summary>
        /// 生成验证码
        /// </summary>
        /// <param name="imageWidth">宽度</param>
        /// <param name="imageHeight">高度</param>
        /// <param name="length">长度</param>
        /// <returns></returns>
        /// <remarks>2014-01-10 杨浩 创建</remarks>
        CodeWrap CreateCode(int imageWidth, int imageHeight, int length=4);
    }
}
