using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Pos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Pos
{
    /// <summary>
    /// 订单推送客户端
    /// </summary>
    /// <remarks>2016-10-08 杨云奕 添加</remarks>
    public abstract class ISyWSOrderToClientJobDao : DaoBase<ISyWSOrderToClientJobDao>
    {
        /// <summary>
        /// 添加推送数据
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public abstract int InnerMod(SyWSOrderToClientJob mod);
        /// <summary>
        /// 更新推送数据
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public abstract int UpdateMod(SyWSOrderToClientJob mod);
        /// <summary>
        /// 指定天数内没有推送订单到客户端的数据
        /// </summary>
        /// <param name="dayValue">天数范围</param>
        /// <returns>返回订单列表</returns>
        public abstract List<SoOrder> GetNotPosthOrderDataToClientList(int dayValue);
        /// <summary>
        /// 推送单到客户端的
        /// </summary>
        /// <param name="DsSysNo">经销商编号</param>
        /// <returns></returns>
        public abstract List<SyWSOrderToClientJob> GetPoshDataToClientList(int? DsSysNo);
    }
}
