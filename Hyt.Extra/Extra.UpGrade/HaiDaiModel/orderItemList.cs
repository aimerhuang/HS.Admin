using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.UpGrade.HaiDaiModel
{
    /// <summary>
    /// 海带订单明细
    /// </summary>
    /// <remarks>2017-6-13 罗勤尧 创建</remarks>
   public  class orderItemList
    {

        /// <summary>
        /// 仓库名称
        /// </summary>
       public string depotName { set; get; }

       /// <summary>
       /// 商品id
       /// </summary>
       public int goodsId { set; get; }
       /// <summary>
       /// 商品图片
       /// </summary>
       public string image { set; get; }
       /// <summary>
       /// 明细id
       /// </summary>
       public int itemId { set; get; }
       /// <summary>
       /// 商品名称
       /// </summary>
       public string name { set; get; }
       /// <summary>
       /// 数量
       /// </summary>
       public int num { set; get; }
       /// <summary>
       /// 供应商货号
       /// </summary>
       public string psn { set; get; }
       /// <summary>
       /// 
       /// </summary>
       public string specName { set; get; }
       /// <summary>
       /// 供应价格
       /// </summary>
       public decimal supplyPrice { set; get; }
       /// <summary>
       /// 商标名
       /// </summary>
       public string tradeName { set; get; }
       /// <summary>
       /// 有效时间
       /// </summary>
       public string valideTime { set; get; }
       /// <summary>
       /// 规格值
       /// </summary>
       public int specNum { get; set; }
       /// <summary>
       /// 商品总数
       /// </summary>
       public int totalNum
       {
           get
           {
               return specNum * num;
           }
       }
       
    }
}
