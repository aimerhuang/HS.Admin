using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Procurement
{
    /// <summary>
    /// 供货商和商品关联
    /// </summary>
    /// <remarks>2016-11-17 杨云奕 添加</remarks>
    public  class PmManufacturProduct
    {
        public int SysNo { get; set; }
        public int PmSysNo { get; set; }
        public int ProductSysNo { get; set; }
    }

    public class CBPmManufacturProduct :PmManufacturProduct
    {
        public string FName { get; set; }
    }
}
