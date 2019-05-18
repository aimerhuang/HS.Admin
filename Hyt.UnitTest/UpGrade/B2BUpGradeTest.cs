using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hyt.BLL.Base;
using Extra.UpGrade.Model;
using Hyt.Model.WorkflowStatus;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.UnitTest.UpGrade
{
    #region 货栈单元测试
    [TestClass]
    public class B2BUpGradeTest
    {
        #region 附加测试特性
        [ClassInitialize]
        public static void MyClassInitialize(TestContext testContext)
        {
            DataProviderBo.Set(Activator.CreateInstance(Type.GetType("Hyt.DataAccess.Oracle.DataProvider,Hyt.DataAccess.Oracle")));
        }
        #endregion
        [TestMethod]
        public void ImportDsMallOrder()
        {
           var  result= Hyt.BLL.Order.SoOrderBo.Instance.ImportDsMallOrder();
        }
        #region 国美批量获取指定时间区间的订单
        [TestMethod]
        public void GetOrderList()
        {

            //threeMallSyncLogInfo.LastSyncTime = ;
        

            var param = new OrderParameters()
            {
                PageIndex = 1,
                PageSize = 20,
                StartDate = DateTime.Now.AddYears(-30).AddHours(-2),
                EndDate = DateTime.Now.AddHours(2),// Convert.ToDateTime("2018-03-24 00:00:00")
            };

            var g = new Extra.UpGrade.UpGrades.B2BUpGrade();
            var result = g.GetOrderList(param, null);
        }
        #endregion

        #region 国美订单出库、发货
        [TestMethod]
        public void SendDelivery()
        {
            //订单号：MallOrderId   物流公司ID:CompanyCode  运单号：HytExpressNo
            //注：使用该物流商ID需要注意商家有没有跟该物流公司合作，可以在商家后台那里查看
            //CompanyCode:  国美代运（顺丰）:21000118   速美:80007204    东易日盛:80008764    华企快运:99900001   国通快递:99900002   大田物流:99900003
            //德邦物流:99900004    EMS:99900005     飞康达:99900006    港中能达:99900007   共速达:99900008    百世快运:99900009   天地华宇:99900010
            //天天快递:99900011     佳吉快运:99900012   佳怡物流:99900013   急先达:99900014    快捷速递:99900015   龙邦物流:99900016   联邦快递:99900017
            //联昊通:99900018   全一快递:99900019      全晨快递:99900020   全日通快递:99900021      全峰快递:99900022       申通快递:99900023
            //顺丰速运:99900024     速尔快递:99900025   盛辉物流:99900026   TNT:99900027    优速物流:99900029   新邦物流:99900030   信丰物流:99900031
            //圆通速递:99900032     韵达快运:99900033   亚风速递:99900034   源伟丰:99900035    远成物流:99900036   元智捷诚:99900037   运通中港:99900038
            //中通速递:99900039     中铁快运:99900040   宅急送:99900041    中铁物流:99900042   通和天下:99900043   微特派:99900044    商家自有物流:99900046
            //成都同康:99900049     四川星程:99900050   都市速代:99900053   广州欧妮斯:99900055      星晨急便:99900057   上海浩川:99900058   亚马逊物流:99900059
            //国内小包:99900060     邮政平邮:99900061   速通物流:99900062   汇强快递:99900063   如风达:99900064    宏伟物流:99900065   兴达伟业物流:99900066
            //京利达物流:99900067    百世快递:99900068   安能物流:99900069   大达物流:99900070   增益速递:99900071   城际速递:99900072   加运美:99900073
            //全通快运:99900074     安时达:99900075    宏递快运:99900076   北京康晟物流:99900077     日日顺物流:99900079      恒路物流:99900080
            //优速快递:99900081     佳吉物流:99900082   国美安迅物流:99900086     国美供应商厂商配送:99900087      EWE全球快递:99900088    EMS国际快递:99900089
            //笨鸟海淘:99900090     飞犇快递:99900091   安达信:99900092    安迅:99900093     城市100:99900094      飞鹿:99900095     飞远:99900096
            //丰程:99900097    红马甲:99900098    黄马甲:99900099    汇文:99900100    门店自送:99900101   门对门:99900102    尚达峰:99900103    顺捷丰达:99900104
            //腾达:99900105    天地速递:99900106   同携:99900107    万博:99900108     新华赫:99900109    贵州星程:99900110   雪狐:99900111     云南中诚:99900112
            //长沙三人行:99900113    芝麻开门:99900114    志行:99900115    众人行:99900116    晟邦物流:99900117    向天物流:99900118     安鲜达:99900119
            //ECMS易客满:99900120    国美物流:99900121     捷安达国际速递:99900122
            Extra.UpGrade.Provider.UpGradeProvider.GetInstance((int)DistributionStatus.商城类型预定义.国美在线).SendDelivery(
                  new Extra.UpGrade.Model.DeliveryParameters { CompanyCode = "99900024", HytExpressNo = "70092526531314", MallOrderId = "60625990838" },
                  null);
        }
        #endregion

    }
    #endregion
}
