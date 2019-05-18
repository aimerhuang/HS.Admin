
namespace Hyt.Util.Validator
{
    /// <summary>
    /// 验证结果
    /// </summary>
    /// <remarks>2013-12-30 黄志勇 注释</remarks>
    public class VResult
    {
        /// <summary>
        /// 验证结果
        /// </summary>
        /// <remarks>2013-12-30 黄志勇 注释</remarks>
        public VResult()
        {
            IsPass = true;
        }

        /// <summary>
        /// 验证错误消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 验证是否通过
        /// </summary>
        public bool IsPass { get; set; }

        #region 运算符重载

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>true:真 false:假</returns>
        /// <remarks>2013-12-30 黄志勇 注释</remarks>
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        /// <summary>
        /// GetHashCode
        /// </summary>
        /// <returns>HashCode</returns>
        /// <remarks>2013-12-30 黄志勇 注释</remarks>
        public override int GetHashCode()
        {
            return IsPass.GetHashCode();
        }

        /// <summary>
        /// !
        /// </summary>
        /// <param name="result">验证结果</param>
        /// <returns>true:真 false:假</returns>
        /// <remarks>2013-12-30 黄志勇 注释</remarks>
        public static bool operator !(VResult result)
        {
            return !result.IsPass;
        }

        /// <summary>
        /// true
        /// </summary>
        /// <param name="result">验证结果</param>
        /// <returns>true:真 false:假</returns>
        /// <remarks>2013-12-30 黄志勇 注释</remarks>
        public static bool operator true(VResult result)
        {
            return result.IsPass;
        }

        /// <summary>
        /// false
        /// </summary>
        /// <param name="result">验证结果</param>
        /// <returns>true:真 false:假</returns>
        /// <remarks>2013-12-30 黄志勇 注释</remarks>
        public static bool operator false(VResult result)
        {
            return !result.IsPass;
        }

        /// <summary>
        /// ==
        /// </summary>
        /// <param name="result">验证结果</param>
        /// <param name="b"></param>
        /// <returns>true:真 false:假</returns>
        /// <remarks>2013-12-30 黄志勇 注释</remarks>
        public static bool operator ==(VResult result, bool b)
        {
            return result.IsPass == b;
        }

        /// <summary>
        /// ==
        /// </summary>
        /// <param name="b"></param>
        /// <param name="result">验证结果</param>
        /// <returns>true:真 false:假</returns>
        /// <remarks>2013-12-30 黄志勇 注释</remarks>
        public static bool operator ==(bool b, VResult result)
        {
            return result.IsPass == b;
        }

        /// <summary>
        /// !=
        /// </summary>
        /// <param name="result">验证结果</param>
        /// <param name="b"></param>
        /// <returns>true:真 false:假</returns>
        /// <remarks>2013-12-30 黄志勇 注释</remarks>
        public static bool operator !=(VResult result, bool b)
        {
            return result.IsPass != b;
        }

        /// <summary>
        /// !=
        /// </summary>
        /// <param name="b"></param>
        /// <param name="result">!=</param>
        /// <returns>true:真 false:假</returns>
        /// <remarks>2013-12-30 黄志勇 注释</remarks>
        public static bool operator !=(bool b, VResult result)
        {
            return result.IsPass != b;
        }

        #endregion
    }
}
