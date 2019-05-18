using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;
using Hyt.Model.Common;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;

namespace Hyt.BLL.ApiIcq
{
    /// <summary>
    /// 商检
    /// </summary>
    /// <remarks>2016-03-19 杨浩 创建</remarks>
    public abstract class IIcqProvider
    {
        /// <summary>
        /// 商检代码
        /// </summary>
        /// <remarks>2015-12-26 杨浩 创建</remarks>
        public abstract Hyt.Model.CommonEnum.商检 Code { get; }
        #region 广州机场
        /// <summary>
        /// 商品备案
        /// </summary>
        /// <returns></returns>
        /// <remarks>2016-3-21 王耀发 添加</remarks>
        public virtual Result PushGoods(string ProductSysNoList)
        {
            return new Result();
        }
        /// <summary>
        /// 推送订单
        /// </summary>
        /// <returns></returns>
        /// <remarks>2016-3-21 王耀发 添加</remarks>
        public virtual Result PushOrder(int OrderSysNo)
        {
            return new Result();
        }

        /// <summary>
        /// 海关商检反馈信息
        /// </summary>
        /// <returns></returns>
        public virtual string[] GetIcpOutResult()
        {
            return new string[] { "" };
        }

        /// <summary>
        /// 下载报文
        /// </summary>
        /// <param name="type"></param>
        /// <param name="localDir"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public virtual string DownResultFileData(string localDir, string filePath)
        {
            return "";
        }
        /// <summary>
        /// 商品商检回执
        /// </summary>
        /// <returns></returns>
        /// <remarks>2016-3-21 王耀发 添加</remarks>
        public virtual Result GetGoodsRec()
        {
            return new Result();
        }
        /// <summary>
        /// 订单商检回执
        /// </summary>
        /// <returns></returns>
        /// <remarks>2016-3-21 王耀发 添加</remarks>
        public virtual Result GetOrderRec()
        {
            return new Result();
        }
        public string GetInfaceParamValue(List<Model.WebTemplate.CBDsDealerMallWarehouseInfaceData> dataList, string txt)
        {
            Hyt.Model.WebTemplate.CBDsDealerMallWarehouseInfaceData mod = dataList.Find(p => p.diti_valueParam == txt);
            if (mod != null)
            {
                return mod.dmwid_Value;
            }
            return null;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>分页</returns>
        /// <remarks>2016-3-23 王耀发 创建</remarks>
        public virtual Pager<CIcp> GetGoodsPagerList(ParaIcpGoodsFilter filter)
        {
            return new Pager<CIcp>();
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>分页</returns>
        /// <remarks>2016-3-23 王耀发 创建</remarks>
        public virtual Pager<CIcp> GetOrderPagerList(ParaIcpGoodsFilter filter)
        {
            return new Pager<CIcp>();
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>分页</returns>
        /// <remarks>2016-3-23 王耀发 创建</remarks>
        public virtual Pager<CBIcpGoodsItem> IcpGoodsItemQuery(ParaIcpGoodsFilter filter)
        {
            return new Pager<CBIcpGoodsItem>();
        }
        #endregion

        #region 广州南沙
        /// <summary>
        /// 推送订单
        /// </summary>
        /// <returns></returns>
        /// <remarks>2015-10-12 杨浩 创建</remarks>
        public virtual Result PushOrder(SoOrder order)
        {
            return new Result();
        }

        public virtual Result CoustomsGoodsPush(Hyt.Model.Icp.GZNanSha.CommodityInspection mod, IList<Hyt.Model.Icp.GZNanSha.CommodityInspectionLists> modLists, List<Model.WebTemplate.CBDsDealerMallWarehouseInfaceData> dataList)
        {
            return new Result();
        }

        public virtual Result CustomsOrderPush(SoOrder order, List<Hyt.Model.Manual.SoOrderItemByPro> proList, List<Model.WebTemplate.CBDsDealerMallWarehouseInfaceData> dataList)
        {
            return new Result();
        }
        public virtual string[] GetCustomsOutResult(Hyt.Model.WorkflowStatus.OrderStatus.商检反馈报文类型 type)
        {
            return new string[]{""};
        }

        public virtual string DownResultFileData(Hyt.Model.WorkflowStatus.OrderStatus.商检反馈报文类型 type, string dir, string filePath)
        {
            return  "";
        }

        #endregion
    }
}
