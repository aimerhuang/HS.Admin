using Hyt.Model;
using Hyt.Service.Contract.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Hyt.Service.Contract.B2CApp
{
    [ServiceKnownType(typeof(Result))]
    [ServiceContract]
    public interface IAddress : IBaseServiceContract
    {
        /// <summary>
        /// 获取区域列表
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="LevelNum"></param>
        /// <returns></returns>
        [CustomOperationBehavior(false)]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        IList<BsArea> AddressArea(int Code);
        /// <summary>
        /// 获取区域数据
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        [CustomOperationBehavior(false)]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<WhWarehouse> AddressAreaWarehouseList(int Code);
    }
}
