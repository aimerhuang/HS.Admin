using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;
using System.IO;
using Hyt.Model.Common;

namespace Hyt.BLL.ApiCustoms
{
    /// <summary>
    /// 海关接口
    /// </summary>
    /// <remarks>2015-10-12 杨浩 创建</remarks>
    public abstract class ICustomsProvider
    {
        /// <summary>
        /// 海关配置文件
        /// </summary>
        protected static CustomsConfig config = BLL.Config.Config.Instance.GetCustomsConfig();
        /// <summary>
        /// 海关内部代码
        /// </summary>
        /// <remarks>2015-12-26 杨浩 创建</remarks>
        public abstract CommonEnum.海关 Code { get; }
        /// <summary>
        /// 推送订单
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <param name="warehouseSysNo">仓库编号</param>
        /// <returns></returns>
        /// <remarks>2015-10-12 杨浩 创建</remarks>
        public abstract Result PushOrder(int orderId, int warehouseSysNo);
        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <param name="warehouseSysNo">仓库编号</param>
        /// <returns></returns>
        /// <remarks>2016-3-23 杨浩 创建</remarks>
        public abstract Result CancelOrder(int orderId, int warehouseSysNo);
        /// <summary>
        /// 查询海关订单
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <returns></returns>
        /// <remarks>2015-12-29 杨浩 创建</remarks>
        public abstract Result SearchCustomsOrder(int orderId);
        /// <summary>
        /// 修改订单
        /// </summary>
        /// <param name="orderSysNo">订单编号</param>
        /// <param name="warehouseSysNo">仓库编号</param>
        /// <returns></returns>
        /// <remarks>2016-8-24 杨浩 添加</remarks>
        public virtual Result ModifyOrder(int orderSysNo, int warehouseSysNo) { return null; }

        /// <summary>
        /// 广州海关报文采用标准的AES加密，加密的密钥 Key
        /// </summary>
        protected virtual string CustomsKey
        {
            get { return "MYgGnQE2+DAS973vd1DFHg=="; }
        }

        /// <summary>
        ///  AES 解密
        /// </summary>
        /// <param name="str">要解密的字符</param>
        /// <returns></returns>
        /// <remarks>2015-10-12 杨浩 创建</remarks>
        protected virtual string Decrypt(string str)
        {
            if (string.IsNullOrEmpty(str)) return null;
            Byte[] toEncryptArray = Convert.FromBase64String(str);

            System.Security.Cryptography.RijndaelManaged rm = new System.Security.Cryptography.RijndaelManaged
            {
                Key = Convert.FromBase64String(CustomsKey),
                Mode = System.Security.Cryptography.CipherMode.ECB,
                Padding = System.Security.Cryptography.PaddingMode.PKCS7
            };

            System.Security.Cryptography.ICryptoTransform cTransform = rm.CreateDecryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Encoding.UTF8.GetString(resultArray);
        }

        /// <summary>
        /// AES 加密
        /// </summary>
        /// <param name="toEncrypt">要加密的字符</param>
        /// <returns></returns>
        /// <remarks>2015-10-12 杨浩 创建</remarks>
        protected virtual string Encrypt(string toEncrypt)
        {
            byte[] toEncryptArray = Encoding.UTF8.GetBytes(toEncrypt);

            System.Security.Cryptography.RijndaelManaged rm = new System.Security.Cryptography.RijndaelManaged
            {
                Key = Convert.FromBase64String(CustomsKey),
                Mode = System.Security.Cryptography.CipherMode.ECB,
                Padding = System.Security.Cryptography.PaddingMode.PKCS7
            };
            System.Security.Cryptography.ICryptoTransform cTransform = rm.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

    }

}
