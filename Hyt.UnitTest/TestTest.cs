using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Hyt.BLL;
using Hyt.BLL.Log;
using Hyt.Model;
using Hyt.Model.Manual;
using Hyt.Model.WorkflowStatus;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Hyt.Util.Serialization;
using System.Text.RegularExpressions;
using Extra.UpGrade.Model;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.Net;
using System.Text;
using System.IO;
using Hyt.Util;
using Hyt.Model.UpGrade;
namespace Hyt.UnitTest
{

    /// <summary>
    ///这是 TestTest 的测试类，旨在
    ///包含所有 TestTest 单元测试
    ///</summary>
    [TestClass()]
    public class TestTest
    {

        private TestContext testContextInstance;

        /// <summary>
        ///获取或设置测试上下文，上下文提供
        ///有关当前测试运行及其功能的信息。
        ///</summary>
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        #region 附加测试特性

        // 
        //编写测试时，还可使用以下特性:
        //
        //使用 ClassInitialize 在运行类中的第一个测试前先运行代码
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            Infrastructure.Initialize.Init();
        }

        //
        //使用 ClassCleanup 在运行完类中的所有测试后再运行代码
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //使用 TestInitialize 在运行每个测试前先运行代码
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //使用 TestCleanup 在运行完每个测试后运行代码
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //

        #endregion

        internal virtual SysLog CreateISysLog()
        {

        
            // TODO: 实例化相应的具体类。
            SysLog target = SysLog.Instance;
            return target;
        }
        [TestMethod()]
        public void Test()
        {
            var config=BLL.Config.Config.Instance.GetExpressConfig();
            //int hour=DateTime.Now.Hour;
            //string orderNo = "10000543";
            //string newOrder=orderNo.Substring(5);
            //var regex = new Regex("44449", RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            //bool isExist=regex.IsMatch("444444444449");
            //string dd2 = null;
            //var dd = dd2.ToObject<StoreExtensions>();
        }
        /// <summary>
        ///InsSysLog 的测试
        ///</summary>
        [TestMethod()]
        public void InsSysLogTest()
        {
            try
            {
                using (var tran = new TransactionScope())
                {
                    Test target = new Test(); // TODO: 初始化为适当的值
                    target.InsSysLog("127.0.0.1");
                    target.InsSysLog("127.0.0.11");
                    //Assert.Inconclusive("无法验证不返回值的方法。");
                    tran.Complete();
                }
            }
            catch (Exception exception)
            {

                throw;
            }
        }

        /// <summary>
        ///Error 的测试
        ///</summary>
        [TestMethod()]
        public void ErrorTest()
        {
            ISysLog target = CreateISysLog(); // TODO: 初始化为适当的值
            const LogStatus.系统日志来源 source = LogStatus.系统日志来源.物流App; // TODO: 初始化为适当的值
            const string message = "ErrorTest测试测试"; // TODO: 初始化为适当的值
            var exception = new Exception("测试测试"); // TODO: 初始化为适当的值
            target.Error(source, message, exception);
            Assert.IsTrue(true);
        }

        /// <summary>
        /// 添加管理员组所有菜单权限
        /// </summary>
        [TestMethod]
        public void UpdateUserGroupPrivilegeTest()
        {
            const int groupId = 1043; //超级管理员组ID
            Hyt.BLL.Sys.SyUserGroupBo.Instance.AssignAllMenuPrivilegeToUserGroup(groupId);
        }

        /// <summary>
        ///InsSysLog 的测试
        ///</summary>
        [TestMethod()]
        public void TestLinq()
        {
            
            var num = new List<int>() { };
            var a = num.Where(t => t == 12);
            var b = num.FindIndex(t => t == 12);

            var sysno = System.Text.RegularExpressions.Regex.Replace("T000000000255854", "T0*", "");

            Assert.IsTrue(sysno != "255854");
            Assert.IsTrue(a != null);
            Assert.IsTrue(b != null);

        }
        /// <summary>
        ///获取第三方支持的快递
        /// </summary>
        /// <param name="auth">授权参数</param>
        /// <returns>2018-03-25 罗勤尧</returns>
         [TestMethod()]
        public void GetExpress()
        {
            //DsDealerApp ds = new DsDealerApp()
            //{
            //    AppKey = "08d0d3f2eaf5449bb9d567a600653e61",
            //    AppSecret = "GGJ_SHUN YING TRADING CO.,LTD",
            //};
            //AuthorizationParameters auth = new AuthorizationParameters()
            //{
            //    DealerApp = ds,
            //};
            //var result = new Result() { Status = true };
            //var _param = new Dictionary<string, string>();

         

            ////订单类型，0：渠道订单，1：格格家订单，2：格格团订单，3：格格团全球购订单，4：环球捕手订单，5：燕网订单，6：b2b订单，7：手q，8：云店
            //string parm = "{\"params\":{},\"partner\":\"" + auth.DealerApp.AppSecret + "\",\"timestamp\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\"}";

            ////MD5加密签名
            //string sign = MD5Encrypt(auth.DealerApp.AppKey + parm + auth.DealerApp.AppKey).ToUpper();

            //var response = PostJson("http://openapi.gegejia.com:8902/api/express/names", parm, sign);

            var response = "{\"names\":[\"119洲际速递\",\"A1快运\",\"AAE-中国\",\"AOL澳通\",\"AU100\",\"BHT\",\"CJ物流\",\"CNE壹速递\",\"COE\",\"coe物流\",\"DHL\",\"DHL-en\",\"DHL-德国\",\"DHL国际\",\"DHL德国\",\"DPEX\",\"D速快递\",\"EFS快递\",\"EMS\",\"EMS-LH\",\"EMS-国际\",\"EMS-英文\",\"EMS国际\",\"ETA易达速递\",\"ETK\",\"EWE\",\"EWE国际物流\",\"E特快\",\"e邮客\",\"FASTGO\",\"FedEx-国际\",\"FedEx-美国\",\"FedEx快递国际\",\"FQ狂派速递\",\"GGEXPRESS\",\"GLOBELL\",\"GLS\",\"GOOD快递\",\"koreapost\",\"logen路坚\",\"lpl速递\",\"MAYIMA\",\"MTY TRACKING\",\"OCS\",\"Ontrac\",\"P2UEXPRESS\",\"Parcelforce\",\"PCAExpress\",\"PostElbe\",\"POSTNZ\",\"SQK国际速运\",\"TNT\",\"TNT-en\",\"Track Parcel\",\"UEQ\",\"UPS\",\"UPS-en\",\"USPS\",\"ヤマト運輸\",\"一号仓\",\"一号线国际速递\",\"一统飞鸿\",\"一速递\",\"一邦速递\",\"七天快递\",\"七天连锁\",\"万家康快递\",\"万家物流\",\"万庚国际速递\",\"万德隆\",\"万象物流\",\"三态速递\",\"三象速递\",\"上大物流\",\"世纪同城速递\",\"东骏快捷物流\",\"中外速运\",\"中天万运\",\"中欧速运\",\"中澳速递\",\"中环国际（澳洲件） \",\"中翼国际物流\",\"中诺物流\",\"中通国际\",\"中通快运\",\"中通澳新物流\",\"中通速递\",\"中速快递\",\"中邮中环\",\"中邮物流\",\"中邮速递\",\"中铁快运\",\"中铁物流\",\"丰泰国际快递\",\"乐天快递\",\"乐捷递\",\"九曳供应链\",\"九曳鲜配\",\"云物流\",\"五亨国际\",\"亚风速递\",\"亚马逊物流\",\"京东快递\",\"京华亿家\",\"京广速递\",\"伍圆速递\",\"优达生鲜\",\"优速物流\",\"传喜物流\",\"佐川急便\",\"佳吉快运\",\"佳怡物流\",\"信丰物流\",\"元智捷诚\",\"全一\",\"全一快递\",\"全峰快递\",\"全日通\",\"全晨快递\",\"全橙快递\",\"全球快运 \",\"全速通\",\"全际通\",\"八达通\",\"共速达\",\"内蒙EMS\",\"冻到家物流\",\"凡客\",\"凤凰快递\",\"加拿大邮政-法文版\",\"加拿大邮政-英文版\",\"加运美\",\"包裹/平邮\",\"北青小红帽\",\"华中快递\",\"华企快运\",\"华夏龙\",\"华美国际快递\",\"南京100\",\"南方传媒物流\",\"卡蒙\",\"原飞航\",\"吉祥物流\",\"启辰国际速递\",\"呦货邮\",\"品速国际快递\",\"品骏快递\",\"嘉里大通\",\"国通快递\",\"国际包裹\",\"国际航空\",\"圆通速递\",\"圣安物流\",\"城市100\",\"城际速递\",\"增益速递\",\"大田物流\",\"大达物流\",\"天地华宇\",\"天天快递\",\"天越物流\",\"央广购物\",\"如风达\",\"威时沛运 \",\"宅急送\",\"安信达\",\"安捷快递\",\"安能物流\",\"安能电商小包\",\"安鲜达\",\"宏品物流\",\"富腾达\",\"尚橙\",\"山东速递\",\"山西红马甲\",\"希伊艾斯\",\"希优特\",\"广东邮政\",\"康力物流\",\"建华快递\",\"彩虹速运 \",\"彪记快递\",\"微特派\",\"德中物流\",\"德国邮政\",\"德邦物流\",\"心怡物流\",\"忠信达\",\"快客速运\",\"快捷速递\",\"快速递\",\"思迈\",\"急先达\",\"恒路物流\",\"意大利邮政\",\"成都东骏快捷物流有限公司\",\"成都商报物流\",\"捷特快递\",\"文捷航空\",\"斑马物流\",\"新亚物流\",\"新干线快递\",\"新蛋奥硕\",\"新西兰秦远国际物流\",\"新邦物流\",\"新顺丰\",\"方舟速递\",\"日日顺物流\",\"日本郵便\",\"昊盛物流\",\"明亮物流\",\"易时联国际速递\",\"易达通\",\"星云速递\",\"星晨急便\",\"星辰快递\",\"星运快递\",\"晋越快递\",\"晟邦物流\",\"极客冷链\",\"桔子公社\",\"民航快递\",\"汇城国际\",\"汇强快递\",\"汇文快递\",\"汇通快运\",\"河北建华\",\"泛捷国际速递\",\"泛远国际物流\",\"济南猎豹速递\",\"海外环球\",\"海星桥快递\",\"海淘客365htk\",\"海盟速递\",\"海红网送\",\"港中能达\",\"渼通快递 \",\"源伟丰\",\"源安达\",\"澳世速递\",\"澳大利亚秦远国际物流\",\"澳大利亚邮政-英文\",\"澳德快递\",\"澳洲新干线快递 \",\"澳洲极地快递\",\"澳运速递\",\"澳通速递\",\"澳邮中国快运\",\"爱送配送\",\"环球速运\",\"申通快递\",\"畅灵国际物流\",\"百世汇通\",\"百福东方\",\"百通物流\",\"盛丰物流\",\"盛辉物流\",\"神盾快运\",\"秀驿物流\",\"程光快递\",\"穗佳物流\",\"立即送\",\"美仓快递 \",\"美国快递\",\"美速通\",\"美邦快递\",\"耀启物流\",\"联昊通\",\"联昊通速递\",\"联邦快递\",\"能达速递\",\"航空便\",\"良藤国际速递\",\"芝麻开门\",\"苏宁快递\",\"荷兰快递\",\"荷兰皇家快递\",\"蓝天快递\",\"蓝镖快递\",\"虚拟物品免物流\",\"行必达\",\"西濃運輸\",\"诚义物流\",\"贝海国际速递\",\"贝海快递\",\"货运皇\",\"赛澳递\",\"越丰物流\",\"跨越速递\",\"转运四方\",\"迅物流\",\"迅达速递\",\"运通中港\",\"远成快运\",\"远成物流\",\"递四方\",\"通和天下\",\"速尔快递\",\"速翼快递\",\"速通达跨境物流\",\"邦送物流\",\"邮政小包\",\"邮政快递包裹\",\"郑州建华\",\"配思货运\",\"重庆中环\",\"金大物流\",\"金牌物流\",\"鑫飞鸿\",\"钱报速运\",\"银捷速递\",\"长宇物流\",\"长江国际速递\",\"门对门\",\"阳光国际速递 \",\"隆浪快递\",\"韵达快运\",\"韵达快递\",\"顺丰-英文版\",\"顺丰速运\",\"顺达快递\",\"顺通物流\",\"风先生物流\",\"风行天下\",\"飞康达\",\"飞康达速运\",\"飞快达\",\"飞洋快递\",\"飞豹快递\",\"香港速邮\",\"香港邮政\",\"香港鸿宇\",\"鸿宇物流\",\"鹏程快递\",\"鹰运国际速递\",\"黄马甲\",\"黑狗冷链\",\"黑猫宅急便\",\"龙象快递\",\"龙邦物流\",\"龙邦速递\"],\"success\":true}";

            var s2cspec = JObject.Parse(response).ToString();
            JObject S2bjObject = JObject.Parse(s2cspec);
            var s2bjoDate = S2bjObject["names"];
           
            if (s2bjoDate is JArray)
            {
                //for (int i = 0; i < s2bjoDate.Count(); i++)
                //{

                //}
                //S2bspec = s2bjoDate[0]["spec"].ToString();
                //S2bprice = decimal.Parse(s2bjoDate[0]["price"].ToString());
                //SpecValues.unit = s2bjoDate[0]["unit"].ToString();
            }
            else
            {
               
            }
             var Assreturn = JsonSerializationHelper.JsonToObject<UpGradeExpress>(response);
            // var _response = JObject.Parse(response.ToString());
            //var names = _response.Property("names");
            //var count = names.Count;
            //if (_response.Property("errMsg") != null)
            //{
            //    //result.Status = false;
            //    //result.Message = _response.Property("errMsg").ToString();
            //    //result.errCode = _response.Property("errCode").ToString();
            //}
           
        }

         #region 给一个字符串进行MD5加密
         /// <summary>  
         /// 给一个字符串进行MD5加密  
         /// </summary>  
         /// <param   name="strText">待加密字符串</param>  
         /// <returns>加密后的字符串</returns>  
         /// <remarks>2013-10-22 杨浩 添加</remarks>
         public string MD5Encrypt(string strText)
         {
             MD5 md5 = new MD5CryptoServiceProvider();
             byte[] result = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(strText));

             string outString = "";
             for (int i = 0; i < result.Length; i++)
             {
                 outString += result[i].ToString("x2");
             }

             return outString;

         }
         #endregion

         #region Post json 数据
         /// <summary>
         /// Post json 数据
         /// </summary>
         /// <param name="url">接收数据链接</param>
         /// <param name="param">json参数</param>
         /// <returns></returns>
         /// <remarks>2015-12-29 杨浩 创建</remarks>
         public string PostJson(string url, string param, string sign)
         {
             byte[] postData = param == "" ? new byte[0] : Encoding.UTF8.GetBytes(param);

             HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
             req.Method = "POST";
             req.ContentType = "application/json;charset=utf-8";
             //req.ContentType = "application/vnd.ehking-v1.0+json;charset=UTF-8";
             req.KeepAlive = true;
             req.Timeout = 300000;
             req.ContentLength = postData.Length;
             req.Headers.Add("sign", sign);

             Stream reqStream = req.GetRequestStream();

             reqStream.Write(postData, 0, postData.Length);

             reqStream.Close();
             HttpWebResponse rsp = (HttpWebResponse)req.GetResponse();

             Stream stream = rsp.GetResponseStream();

             StreamReader sr = new StreamReader(stream, Encoding.UTF8);
             string result = sr.ReadToEnd();
             sr.Close();
             stream.Close();
             return result;
         }
         #endregion

    }
}
