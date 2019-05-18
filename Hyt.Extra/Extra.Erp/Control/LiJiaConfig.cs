using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.Erp.Control.LiJiaNew
{
    public   class LiJiaConfig
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public LiJiaConfig() { 
        
        
        }
        /// <summary>
        /// 利嘉入口
        /// </summary>
        public static string ApiUrl = "http://szxyerpapi.nitago.com";
        /// <summary>
        /// Api对接Key
        /// </summary>
        public static string AppKey = "5E6F56EAE41D95EF7BF22DBB21C54231";
        /// <summary>
        /// 接口地址
        /// </summary>
        public string ApiJieKouUrl {get;set; }
        /// <summary>
        /// 密钥
        /// </summary>
        public static string AppSecret = "5840A5BD42A82100FC097335CD29B99A744BB7A643A82100";
        /// <summary>
        /// 入库单地址
        /// </summary>
        public static string ApiStockIn = "/stockinoutbound/addstockin";
        /// <summary>
        /// 出库单地址
        /// </summary>
        public static string ApiStockOut = "/stockinoutbound/addoutbound";
    }
}
