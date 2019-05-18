using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{


    [Serializable]
    public class AuditInfoDto : JdObject
    {


        [XmlElement("task_id")]
        public string
          taskId { get; set; }


        [XmlElement("approver_code")]
        public string
          approverCode { get; set; }


        [XmlElement("approver_name")]
        public string
          approverName { get; set; }


        [XmlElement("opinion")]
        public string
          opinion { get; set; }


        [XmlElement("state")]
        public string
          state { get; set; }


        [XmlElement("approve_time")]
        public DateTime
          approveTime { get; set; }


    }
}
