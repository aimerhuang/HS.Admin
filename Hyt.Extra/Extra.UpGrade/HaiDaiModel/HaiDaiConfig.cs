using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.UpGrade.HaiDaiModel
{
   
    /// <summary>
    /// 海带配置
    /// </summary>
    /// <remarks>2017-6-13 罗勤尧 创建</remarks>
    public class HaiDaiConfig
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public HaiDaiConfig()
        {

        }

        /// <summary>
        /// 海带API调用入口
        /// </summary>
        public string ApiUrl { get; set; }
        /// <summary>
        /// 海带API调用测试入口
        /// </summary>
        public string ApiUrlTest { get; set; }
        
        /// <summary>
        /// 海带开放平台应用appkey
        /// </summary>
        public string AppKey { get; set; }

        /// <summary>
        /// 海带开放平台应用appsecret
        /// </summary>
        public string AppSecret { get; set; }

        /// <summary>
        /// 海带开放平台应用登录账号
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 海带开放平台应用登录密码
        /// </summary>
        public string PassWord { get; set; }

        /// <summary>
        /// 海带TOKEN
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 海带mID
        /// </summary>
        public string MemberId { get; set; }
        /// <summary>
        /// 授权回调地址
        /// </summary>
        public string TaobaoCallBack { get; set; }

        /// <summary>
        /// 升舱订单标识
        /// </summary>
        public int SellerFlag { get; set; }

        /// <summary>
        /// 已升舱的订单标识旗
        /// </summary>
        public int ExcludeFlag { get; set; }
        /// <summary>
        /// 默认仓库编号
        /// </summary>
        public int DefaultWarehouseSysNo { get; set; }
        /// <summary>
        /// 海带每天开始接单时间小时部分
        /// </summary>
        public int OrderStartTime { get; set; }
        /// <summary>
        /// 海带每天结束接单时间小时部分
        /// </summary>
        public int OrderEndTime { get; set; }
      
    }
}
