using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.ExpressList
{
    /// <summary>
    /// 快递100 电子面单 收货人--寄件人
    /// </summary>
    /// <remarks>2017-11-27 廖移凤 创建</remarks>
    public class RecMan
    {

        public RecMan()
        {
            name = "";
            mobile = "";
            tel = "";
            zipCode = "";
            province = "";
            city = "";
            district = "";
            addr = "";
            printAddr = "";
            company = "";


        }
        /// <summary>
        /// 收件人姓名，必填
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 收件人的手机号，手机号和电话号二者其一必填
        /// </summary>
        public string mobile { get; set; }
        /// <summary>
        /// 收件人的电话号，手机号和电话号二者其一必填
        /// </summary>
        public string tel { get; set; }
        /// <summary>
        /// 收件人所在地的编箱号，非必填
        /// </summary>
        public string zipCode { get; set; }
        /// <summary>
        /// 收件人所在省份，如广东省，province,city,distinct,addr 和 printAddr 任选一个必填
        /// </summary>
        public string province { get; set; }
        /// <summary>
        /// 收件人所在市，如深圳市, province,city,distinct,addr 和 printAddr 任选一个必填
        /// </summary>
        public string city { get; set; }
        /// <summary>
        /// 收件人所在区，如南山区, province,city,distinct,addr 和 printAddr 任选一个必填
        /// </summary>
        public string district { get; set; }
        /// <summary>
        /// 收件人所在地址，如科技南十二路2号金蝶软件园, province,city,distinct,addr 和 printAddr 任选一个必填
        /// </summary>
        public string addr { get; set; }
        /// <summary>
        /// 收件人所在完整地址,province,city,distinct,addr 和 printAddr 任选一个必填。如果有填写province，city，distinct，addr 则系统优先读取province，city，distinct，addr；如果只填写printAddr，系统将自动识别对应的省、市与区
        /// </summary>
        public string printAddr { get; set; }
        /// <summary>
        /// 收件人所在公司名称，非必填
        /// </summary>
        public string company { get; set; }
    }
}
