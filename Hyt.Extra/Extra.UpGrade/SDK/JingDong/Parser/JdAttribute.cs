using System;
using System.Reflection;

namespace Extra.UpGrade.SDK.JingDong.Parser
{
    public class JdAttribute
    {
        public string ItemName { get; set; }
        public Type ItemType { get; set; }
        public string ListName { get; set; }
        public Type ListType { get; set; }
        public MethodInfo Method { get; set; }
    }
}
