using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.UpGrade
{
    /// <summary>
    /// 导出excel类
    /// </summary>
    /// <remarks>
    /// 2016-03-16 杨云奕 导出excel类
    /// </remarks>
    public class Export
    {
        public string InnerExportDataHaveHaed(ExportHead head, List<object> list, bool bsub = false, string subKey = "")
        {
            return ExcelHead().Replace("{0}", InitExportData(head, list, bsub, subKey)); //string.Format(, sbTitle.ToString());
        }
        public string InitExportData(ExportHead head, List<object> list, bool bsub = false, string subKey = "")
        {

            StringBuilder sbTitle = new StringBuilder();
            if (string.IsNullOrEmpty(head.HeadText))
            {
                sbTitle.Append("<tr>");
                sbTitle.Append(" <td style=\"font-size:16px; \" colspan=\"" + head.ThDataList.Count + "\"> <b> ");
                sbTitle.Append(head.HeadText);
                if (string.IsNullOrEmpty(head.SubHeadText))
                {
                    sbTitle.Append("<br/>" + head.SubHeadText);
                }
                sbTitle.Append(" </b> </td>");
                sbTitle.Append("</tr>");
            }
            sbTitle.Append("<tr>");
            foreach (string key in head.ThDataList.Keys)
            {
                sbTitle.Append(" <th  STYLE='MSO-NUMBER-FORMAT:\\@' >  ");
                sbTitle.Append(head.ThDataList[key].ToString());
                sbTitle.Append(" </th>");
            }
            sbTitle.Append("</tr>");

            foreach (object obj in list)
            {
                sbTitle.Append("<tr>");
                foreach (string key in head.ThDataList.Keys)
                {
                    sbTitle.Append(" <td " + (head.ThDataList[key].GetTypeInfo()) + " " + (bsub ? "color=\"#ff0000\"" : "") + " > ");
                    sbTitle.Append(getProperties(obj, key));
                    sbTitle.Append("  </td>");
                }
                sbTitle.Append("</tr>");
                if (bsub)
                {
                    //sbTitle.Append("<tr>");
                    ////sbTitle.Append(" <td STYLE='MSO-NUMBER-FORMAT:\\@'></td>");

                    //sbTitle.Append(" <td STYLE='MSO-NUMBER-FORMAT:\\@' >  </td>");

                    //sbTitle.Append("</tr>");
                    sbTitle.Append("[=" + getProperties(obj, subKey) + "]");
                }
            }
            return sbTitle.ToString();
        }

        public string getProperties<T>(T t, string key)
        {
            string tStr = string.Empty;
            if (t == null) { return tStr; }
            System.Reflection.PropertyInfo[] properties = t.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            if (properties.Length <= 0) { return tStr; }
            foreach (System.Reflection.PropertyInfo item in properties)
            {
                string name = item.Name;
                if (key == name)
                {
                    object value = item.GetValue(t, null);
                    if (value != null)
                    {
                        tStr = value.ToString();
                    }
                    else
                    {
                        tStr = "";
                    }
                }

            }

            return tStr;
        }

        public string ExcelHead()
        {
            StringBuilder strb = new StringBuilder();
            strb.Append(" <html xmlns:o=\"urn:schemas-microsoft-com:office:office\"");
            strb.Append("xmlns:x=\"urn:schemas-microsoft-com:office:excel\"");
            strb.Append("xmlns=\"http://www.w3.org/TR/REC-html40\"");
            strb.Append(" <head> <meta http-equiv='Content-Type' content='text/html; charset=utf-8'>");
            strb.Append(" <style>");
            strb.Append(".xl26");
            strb.Append(" {mso-style-parent:style0;");
            strb.Append(" font-family:\"Times New Roman\", serif;");
            strb.Append(" mso-font-charset:0;");
            strb.Append(" mso-number-format:\"@\";}");
            strb.Append(" </style>");
            strb.Append(" <xml>");
            strb.Append(" <x:ExcelWorkbook>");
            strb.Append("  <x:ExcelWorksheets>");
            strb.Append("  <x:ExcelWorksheet>");
            strb.Append("    <x:Name>Sheet1 </x:Name>");
            strb.Append("    <x:WorksheetOptions>");
            strb.Append("    <x:DefaultRowHeight>285 </x:DefaultRowHeight>");
            strb.Append("    <x:Selected/>");
            strb.Append("    <x:Panes>");
            strb.Append("      <x:Pane>");
            strb.Append("      <x:Number>3 </x:Number>");
            strb.Append("      <x:ActiveCol>1 </x:ActiveCol>");
            strb.Append("      </x:Pane>");
            strb.Append("    </x:Panes>");
            strb.Append("    <x:ProtectContents>False </x:ProtectContents>");
            strb.Append("    <x:ProtectObjects>False </x:ProtectObjects>");
            strb.Append("    <x:ProtectScenarios>False </x:ProtectScenarios>");
            strb.Append("    </x:WorksheetOptions>");
            strb.Append("  </x:ExcelWorksheet>");
            strb.Append("  <x:WindowHeight>6750 </x:WindowHeight>");
            strb.Append("  <x:WindowWidth>10620 </x:WindowWidth>");
            strb.Append("  <x:WindowTopX>480 </x:WindowTopX>");
            strb.Append("  <x:WindowTopY>75 </x:WindowTopY>");
            strb.Append("  <x:ProtectStructure>False </x:ProtectStructure>");
            strb.Append("  <x:ProtectWindows>False </x:ProtectWindows>");
            strb.Append(" </x:ExcelWorkbook>");
            strb.Append(" </xml>");
            strb.Append("");
            strb.Append(" </head> <body>");
            strb.Append(" <table align=\"center\" style='border-collapse:collapse;table-layout:fixed'> ");
            strb.Append("   {0}");
            strb.Append(" </table>");
            strb.Append(" </body> </html>");
            return strb.ToString();
        }
    }
}
