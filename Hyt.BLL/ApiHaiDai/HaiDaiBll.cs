using Extra.UpGrade.HaiDaiModel;
using Extra.UpGrade.Model;
using Extra.UpGrade.UpGrades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.ApiHaiDai
{
    //接口测试
    public class HaiDaiBll : BOBase<HaiDaiBll>
    {
       public string  Test ()
       {
           HaiDaiUpGrade haidai = new HaiDaiUpGrade();
           HaiDaiOrderParameter pa = new HaiDaiOrderParameter();

           OrderParameters p = new OrderParameters();
           p.StartDate = DateTime.Now.AddDays(-5);
           p.EndDate = DateTime.Now;

           AuthorizationParameters au = new AuthorizationParameters();
           au.MallType = 13;
           List<int> orders = new List<int>();
           orders.Add(538924);
           orders.Add(538918);
           //发货测试
           //haidai.ShipGoods("20170", "", "12345678", "shentong");

           //订单列表
           return haidai.GetOrderList(p, au).ToString();
           //接单
           //return haidai.GetMallOrderDetail(orders,"").ToString();

           //发货
           //return haidai.ShipGoods("538918", "", "2001758225", "shentong").ToString();

           //订单详情
           //return haidai.OrderDetail("539020", "").ToString();

       }
      
       
    }
}
