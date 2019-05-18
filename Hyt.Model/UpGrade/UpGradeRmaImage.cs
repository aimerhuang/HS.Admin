using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Hyt.Model.UpGrade
{
    [DataContract]
    public class UpGradeRmaImage
    {
        [DataMember]
        public int ReturnSysNo { get; set; }
        [DataMember]
        public string FileName { get; set; }
        [DataMember]
        public byte[] FileData { get; set; }
        [DataMember]
        public string ImageUrl { get; set; }
    }
}
