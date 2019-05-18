using Hyt.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.MobileControl
{
    public class CityControl
    {
        /// <summary>
        /// 获取省份的信息
        /// </summary>
        /// <returns></returns>
        public List<object> GetProvinceData()
        {
            IList<BsArea> areaList =  Hyt.BLL.Web.BsAreaBo.Instance.GetRegion();
            List<object> provinceList = new List<object>();
            foreach(var mod in areaList)
            {
                provinceList.Add(new { name = mod.AreaName, id = mod.SysNo, parent_id = mod.ParentSysNo });
            }
            return provinceList;
        }
        /// <summary>
        /// 通过父编码获取地区内容
        /// </summary>
        /// <param name="pcode"></param>
        /// <returns></returns>
        public  List<object> GetAreaDataByParentCode(int pcode)
        {
            var areaList = Hyt.BLL.Web.BsAreaBo.Instance.GetChildArea(pcode);
            List<object> provinceList = new List<object>();
            foreach (var mod in areaList)
            {
                provinceList.Add(new { name = mod.GetType().GetProperty("Name").GetValue(mod, null).ToString().Trim(), id = mod.GetType().GetProperty("No").GetValue(mod, null), parent_id = pcode });
            }
            return provinceList;
        }
    }
}
