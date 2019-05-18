using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.WebTemplate
{
    public class DsMallConfigTemple
    {
        public int SysNo { get; set; }
        public string TempleDir { get; set; }
        public string TempleDefaultPage { get; set; }
        public string TempleConfigPath { get; set; }
        public string TempleImages { get; set; }
        public string TempleVersion { get; set; }
        public string TempleVersionHistory { get; set; }
        public DateTime CreateTime { get; set; }

        public string TempleName { get; set; }
    }
}
