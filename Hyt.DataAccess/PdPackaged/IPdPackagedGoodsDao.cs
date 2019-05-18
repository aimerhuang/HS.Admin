using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.PdPackaged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.PdPackaged
{
    /// <summary>
    /// 产品套装
    /// </summary>
    public abstract class IPdPackagedGoodsDao : DaoBase<IPdPackagedGoodsDao>
    {

        /// <summary>
        /// 分页获取盘点作业单
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        /// 2017-8-25 吴琨 创建
        public abstract Pager<PdPackagedGoods> GetPageList(Pager<PdPackagedGoods> pager,int? GetType);


        /// <summary>
        /// 创建套装商品
        /// </summary>
        /// <param name="model">套装商品表</param>
        /// <param name="listModel">套装商品商品明细表</param>
        /// <returns></returns>
        /// 2017-8-25 吴琨 创建
        public abstract bool Add(PdPackagedGoods model,List<PdPackagedGoodsEntry> listModel);


        /// <summary>
        /// 获取套装商品
        /// </summary>
        /// <param name="pager"></param>
        /// <param name="GetType">是否获取商品明细 1是 0否</param>
        /// <returns></returns>
        /// 2017-8-25 吴琨 创建
        public abstract PdPackagedGoods GetPageModels(int SysNo, int? GetType = 0);


        /// <summary>
        /// 更改套装商品状态
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <param name="status">状态值</param>
        /// <returns></returns>
        /// 吴琨 创建
        public abstract bool UpdateStatus(int sysNo, int status, int Auditor, string AuditorName);


        /// <summary>
        /// 查询目前最大Id号,用于生成单据编号
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <param name="status">状态值</param>
        /// <returns></returns>
        /// 吴琨 2017/8/29 创建
        public abstract int GetModelSysNo();
    }
}
