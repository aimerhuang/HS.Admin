using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.WebTemplate
{
    public class CBDsDealerMallTemplate : DsDealerMallTemplate
    {
        public int ConfigTemplateId { get; set; }
        public string TempleName { get; set; }
        public string TempleConfigPath { get; set; }
        public string TempleImages { get; set; }
    }
}
