using Hyt.Service.Contract.B2CApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Hyt.Service.Implement.B2CApp
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“Address”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 Address.svc 或 Address.svc.cs，然后开始调试。
    public class Address : BaseService, IAddress
    {
        /// <summary>
        /// 获取区域列表
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        public IList<Model.BsArea> AddressArea(int Code)
        {
            return Hyt.BLL.Basic.BasicAreaBo.Instance.GetAreaList(Code);
        }
        /// <summary>
        /// 获取区域数据
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        public IList<Model.WhWarehouse> AddressAreaWarehouseList(int Code)
        {
            List<int> SysNos = new List<int>();
            SysNos.Add(Code);
            IList<Hyt.Model.WhWarehouse> list = Hyt.BLL.Warehouse.WhWarehouseBo.Instance.GetWhWarehouseListByArea(SysNos);
            return list;
        }
    }
}
