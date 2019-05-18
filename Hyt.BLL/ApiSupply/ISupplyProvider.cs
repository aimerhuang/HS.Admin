using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Security.Cryptography;
using Hyt.Model;
using Hyt.Model.Common;

namespace Hyt.BLL.ApiSupply
{
    /// <summary>
    /// 供应链产品参数
    /// </summary>
    /// <remarks>2016-3-18 杨浩 创建</remarks>
    public class ParaSupplyProductFilter
    {
        private int _pageSize = 0;

        private int _id = 1;
        /// <summary>
        /// 当前页号
        /// </summary>
        public int Id 
        {
            get {return _id;}
            set { _id = value;}
        }

        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize
        {
            get
            {
                if (_pageSize == 0)
                {
                    _pageSize = 50;
                }
                return _pageSize;
            }
            set { _pageSize = value; }
        }
    }
    /// <summary>
    /// 供应链接口
    /// </summary>
    /// <remarks>2016-2-23 杨浩 创建</remarks>
    public abstract class ISupplyProvider
    {
        /// <summary>
        /// 供应链标识
        /// </summary>
        /// <remarks> Create By 刘伟豪 2016-3-8 </remarks>
        public abstract Hyt.Model.CommonEnum.供应链代码 Code { get; }

        /// <summary>
        /// 供应链详情
        /// </summary>
        /// <remarks>2016-3-21 刘伟豪 创建</remarks>
        protected abstract SupplyInfo Config { get; }
        /// <summary>
        /// 获取商品列表
        /// </summary>
        /// <param name="paraFilte">参数</param>
        /// <returns></returns>
        public abstract Result<string> GetGoodsList(ParaSupplyProductFilter paraFilte = null);
        /// <summary>
        /// 商品库存获取接口
        /// </summary>
        /// <returns></returns>
        public abstract Result<string> GetGoodsSku(string skuid);
        /// <summary>
        /// 商品库存获取接口
        /// </summary>
        /// <returns></returns>
        public abstract Result<string> GetAllGoodsSku();
        /// <summary>
        /// 入库个别商品信息
        /// </summary>
        /// <param name="sysNos">商品系统编号，逗号分隔</param>
        /// <returns></returns>
        /// <remarks>2016-4-22 王耀发 创建</remarks> 
        public abstract Result<string> StockInSupplyProduct(string sysNos);

        /// <summary>
        /// 查询订单
        /// </summary>
        /// <returns></returns>
        public abstract Result<string> CheckOrder(int orderSysNo);
        /// <summary>
        /// 订单物流信息获取接口
        /// </summary>
        /// <returns></returns>
        public abstract Result<string> GetShipping();
        /// <summary>
        /// 提交订单
        /// </summary>
        /// <param name="orderSysNo">订单系统编号</param>
        /// <remarks>2016-3-18 刘伟豪 创建</remarks>
        public abstract Result<string> SendOrder(int orderSysNo);
        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="orderSysNo">订单系统编号</param>
        /// <remarks> 2016-6-14 刘伟豪 创建 </remarks>
        public abstract Result<string> CancelOrder(int orderSysNo);
        /// <summary>
        /// 获取商品总页数
        /// </summary>
        /// <param name="paraFilte">查询条件</param>
        /// <returns></returns>
        /// <remarks>2016-3-20 杨浩 创建</remarks>
        public int GetGoodsTotalPage(ParaSupplyProductFilter paraFilte = null) { return 1; }
        /// <summary>
        /// 查询商品类别
        /// </summary>
        /// <remarks> Create By 刘伟豪 2016-3-8 </remarks>
        public virtual Result<string> GetProClass()
        {
            return new Result<string>();
        }

        #region 公共方法
        /// <summary>
        /// 参数排序
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <remarks> Create By Lwh 2016-3-8 </remarks>
        protected string Asc(string input)
        {
            Array arr = input.ToArray();
            Array.Sort(arr);
            string strAsc = "";
            foreach (var item in arr)
            {
                strAsc += item;
            }
            return strAsc;
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="AppKey">待加密字符串</param>
        /// <remarks> Create By Lwh 2016-3-8 </remarks>
        protected string Encrypt_MD5(string AppKey)
        {
            MD5 MD5 = new MD5CryptoServiceProvider();
            byte[] datSource = Encoding.GetEncoding("gb2312").GetBytes(AppKey);
            byte[] newSource = MD5.ComputeHash(datSource);
            StringBuilder sb = new StringBuilder(32);
            for (int i = 0; i < newSource.Length; i++)
            {
                sb.Append(newSource[i].ToString("x").PadLeft(2, '0'));
            }
            string crypt = sb.ToString();
            return crypt;
        }

        /// <summary>  
        /// 获取时间戳
        /// </summary>  
        /// <remarks> Create By Lwh 2016-3-8 </remarks>
        protected string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }
        #endregion
    }
}