using Extra.Erp.Model;
using Hyt.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.ApiElectricBusiness
{
    public abstract class IElectricBusinessProvider
    {
        /// <summary>
        /// 物流配置
        /// </summary>
        /// <remarks>2016-3-8 杨浩 创建</remarks>
        protected static ElectricBusinessConfig config = BLL.Config.Config.Instance.GetElectricBusinessConfig();
        /// <summary>
        /// 物流代码
        /// </summary>
        /// <remarks>2015-12-26 杨浩 创建</remarks>
        public abstract Hyt.Model.CommonEnum.电商平台 Code { get; }

        public abstract void OutPutExcelData(List<int> orderSysNos);
    }
}
