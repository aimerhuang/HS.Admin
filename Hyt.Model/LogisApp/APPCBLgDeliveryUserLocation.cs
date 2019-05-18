using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Hyt.Model.LogisApp
{
    /// <summary>
    /// APP 上传GPS数据
    /// </summary>
    ///  <remarks>2013-07-11 周唐炬 创建</remarks>
    [DataContract]
    public class APPCBLgDeliveryUserLocation
    {
        /// <summary>
        /// 纬度
        /// </summary>
        [DataMember]
        public decimal Latitude { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        [DataMember]
        public decimal Longitude { get; set; }

        /// <summary>
        /// 定位时间
        /// </summary>
        [DataMember]
        public string GpsDate { get; set; }

        /// <summary>
        /// 定位类型编码:Gps(10),基站(20)
        /// </summary>
        [DataMember]
        public int LocationType { get; set; }

        /// <summary>
        /// 定位误差
        /// </summary>
        [DataMember]
        public decimal Radius { get; set; }
    }
}
