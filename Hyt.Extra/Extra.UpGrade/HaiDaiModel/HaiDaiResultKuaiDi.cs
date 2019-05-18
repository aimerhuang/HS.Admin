using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.UpGrade.HaiDaiModel
{
    /// <summary>
    /// 海带订单快递结果集
    /// </summary>
    /// <remarks>2017-6-13 罗勤尧 创建</remarks>
   public  class HaiDaiResultKuaiDi
    {

        public int result { set; get; }
        public string message { set; get; }
        public KuaiDiData data { set; get; }
    }
   public class KuaiDiData
   {
       /// <summary>
       /// 
       /// </summary>
       public string com { set; get; }
       /// <summary>
       /// 
       /// </summary>
       public int id { set; get; }
       /// <summary>
       /// 
       /// </summary>
       public int ischeck { set; get; }

       /// <summary>
       /// 
       /// </summary>
       public string message { get; set; }
      

      
       /// <summary>
       /// 
       /// </summary>
       public string nu { get; set; }


       /// <summary>
       /// 
       /// </summary>
       public int state { get; set; }

       /// <summary>
       /// 
       /// </summary>
       public int status { get; set; }
       /// <summary>
       /// 
       /// </summary>
       public string expiredTime { get; set; }
       /// <summary>
       /// 状态说明
       /// </summary>
       public int state_name { get; set; }
       /// <summary>
       /// 配送名称
       /// </summary>
       public string com_name { get; set; }
       /// <summary>
       /// 明细
       /// </summary>
       public List<KuaiDiInfo> data { get; set; }
   }
   public class KuaiDiInfo
   {
       public string time { set; get; }
       public string context { set; get; }
       public string ftime { set; get; }

   }
}
