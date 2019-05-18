using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Extra.UpGrade.SDK.JingDong.Request;
using Extra.UpGrade.SDK.JingDong;
using Extra.UpGrade.SDK.JingDong.Response;

namespace jos_sdk_net
{
    class Program
    {
        static void Main(string[] args)
        {
            int num = 9980980;

            string url = "https://api.jd.com/routerjson";
            string appkey = "19478A24FB059B02BE36130123D2BF66";
            string appsecret = "9ef99129e6d44a41b12e11011c073214";
            string token = "f28f1700-144e-41a3-a9f6-0130acc8b07b";
            IJdClient client = new DefaultJdClient(url, appkey, appsecret);

            PopOrderSearchRequest req = new PopOrderSearchRequest();
            req.orderState = "WAIT_SELLER_STOCK_OUT ";
            req.optionalFields = "orderId";
            PopOrderSearchResponse response = client.Execute(req, token, DateTime.Now.ToLocalTime());
            Console.WriteLine(response.Body);
            Console.WriteLine(num);

            Console.ReadLine();
        }
    }
}
