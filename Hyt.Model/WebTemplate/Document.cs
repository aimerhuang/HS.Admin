using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.WebTemplate
{
    public class Document
    {
        public List<FeAdvertGroup> FeAdvert { get; set; }
        public List<FeProductGroup> FeProduct { get; set; }
        public List<Hyt.Model.Generated.DsDealerMallClass> MallClass { get; set; }
    }
}
