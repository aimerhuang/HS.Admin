using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 快递100月报表
    /// </summary>
    /// <remarks>2014-05-20 朱成果 添加</remarks>
   public  class CBMonthExpress
    {
       /// <summary>
       /// 月份
       /// </summary>
       public string YearMonth { get; set; }

       /// <summary>
       /// 成功单量
       /// </summary>
       public int SuccessFlgs { get; set; }

       /// <summary>
       /// 失败单量
       /// </summary>
       public int FailFlgs { get; set; }

       /// <summary>
       /// 总单量
       /// </summary>
       public int AllFlgs { get; set; }
    }
}
