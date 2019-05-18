using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Security.Cryptography;
using Hyt.Model;
using Hyt.Model.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Hyt.BLL.ApiSupply.PaPaGo
{
    /// <summary>
    /// 啪啪购供应链接口
    /// </summary>
    /// <remarks>
    /// 2016-05-31 刘伟豪 创建
    /// </remarks>

    public class PaPaGoProvider : ISupplyProvider
    {
        #region 属性字段

        public override CommonEnum.供应链代码 Code
        {
            get { return CommonEnum.供应链代码.啪啪购; }
        }
        protected override SupplyInfo Config
        {
            get { return Hyt.BLL.Config.Config.Instance.GetSupplyConfig().SupplyList.FirstOrDefault(s => s.Key == Code.ToString()); }
        }
        private static object lockHelper = new object();
        public PaPaGoProvider() { }

        #endregion

        #region 商品管理
        /// <summary>
        /// 获取所有商品
        /// </summary>
        /// <remarks> 
        /// 2016- 刘伟豪 实现 
        /// </remarks>
        public override Result<string> GetGoodsList(ParaSupplyProductFilter paraFilte = null)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取个别商品
        /// </summary>
        /// <remarks> 
        /// 2016- 刘伟豪 实现 
        /// </remarks>
        public override Result<string> GetGoodsSku(string skuids)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 商品入库到平台
        /// </summary>
        /// <remarks> 
        /// 2016- 刘伟豪 实现 
        /// </remarks>
        public override Result<string> StockInSupplyProduct(string sysNos)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取所有类别
        /// </summary>
        /// <remarks> 
        /// 2016- 刘伟豪 实现 
        /// </remarks>
        public override Result<string> GetProClass()
        {
            throw new NotImplementedException();
        }

        public override Result<string> GetAllGoodsSku()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region 订单管理
        public override Result<string> GetShipping()
        {
            throw new NotImplementedException();
        }

        public override Result<string> CancelOrder(int orderSysNo)
        {
            throw new NotImplementedException();
        }

        public override Result<string> CheckOrder(int orderSysNo)
        {
            throw new NotImplementedException();
        }

        public override Result<string> SendOrder(int orderSysNo)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region 私有方法

        #endregion

    }
}