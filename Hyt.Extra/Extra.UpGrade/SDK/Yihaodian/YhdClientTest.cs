using System;
using Extra.UpGrade.SDK.Yihaodian;
using Extra.UpGrade.SDK.Yihaodian.Request;
using Extra.UpGrade.SDK.Yihaodian.Response;
using System.Collections.Generic;

namespace yhd
{
    class YhdClientTest
    {

        const string URL = "http://openapi.yhd.com/app/api/rest/router";
        //兼容淘宝接口请求url
		//const string URL = "http://openapi.yhd.com/app/api/rest/newRouter";
        static void Main(string[] args)
        {
            DateTime a = DateTime.Now;
            Console.WriteLine(a);
           
            string appkey = "xxxxxxxxxxx";

            string sessionKey = "xxxxxxxxxxx";

            string secretKey = "xxxxxxxxxxxxx";
			
            YhdClient client = new YhdClient(URL, appkey, secretKey, "json");

			//指定上传文件
            string[] filePathArray = new string[1];
            filePathArray[0] = "E:/photo/1020082_380x380.jpg";
            //filePathArray[1] = "d:/test/IMG_中文测试.jpg";
			
            //测图片上传
            ProductImgUploadRequest req = new ProductImgUploadRequest();
            req.ProductId = 1799943L;
            req.OuterId = "API新增单品白名单验证07";
            req.MainImageName = "IMG_中文测试.jpg";
          //  ProductImgUploadResponse responseData = client.Execute(req, sessionKey, filePathArray);
           
            //测获取普通产品信息
            GeneralProductsSearchRequest req1 = new GeneralProductsSearchRequest();
            req1.ProductIdList = "9049087,17802208";   
       //     GeneralProductsSearchResponse responseData1 = client.Execute(req1, sessionKey);
          
        }
    }
}
